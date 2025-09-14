using System.Linq;

namespace Data.Tests;

using static Data;

[PublicAPI]
public sealed class DataCacheTests
{
	[Fact]
	public void Cache_Fetch_Flush_Work_Via_Reflection()
	{
		// Arrange: get nested Cache type and its backing _data field
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic).OrThrow("Cache type not found");
		var dataField = cacheType.GetField("_data", NonPublic | Static).OrThrow("Cache._data not found");
		var original = dataField.GetValue(null).OrThrow();
		var typeMapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var typeMap = CreateInstance(typeMapType).OrThrow();

		// install empty maps for Player and others we'll use
		AddEmpty(typeMap, typeof (Player));

		// Point the cache to our empty map instance
		dataField.SetValue(null, typeMap);
		// Sanity: ensure the map contains Player key to avoid triggering DB-backed Load
		var containsKey = (bool)typeMapType.GetMethod("ContainsKey")
										   .OrThrow()
										   .Invoke(typeMap, [typeof (Player)])
										   .OrThrow();
		Assert.True(containsKey, "Test setup failed: Cache does not contain key for Player (raw map)");
		// Do not call Cache.ContainsKey<T>() to avoid triggering Load
		try
		{
			// Resolve generic methods we will use
			var fetchAllMethod = cacheType.GetMethod("FetchAll", NonPublic | Static);
			var fetchOneFuncMethod = cacheType.GetMethods(NonPublic | Static)
											  .First(static m => m.Name is "FetchOne"
															  && m.GetParameters().Length is 1
															  && m.GetParameters()[0].ParameterType.IsGenericType);
			var flushMethod = cacheType.GetMethod("Flush", NonPublic | Static);

			Assert.NotNull(fetchOneFuncMethod);
			Assert.NotNull(fetchAllMethod);
			Assert.NotNull(flushMethod);

			// Manually seed players into the underlying SortedDictionary to avoid invoking Cache.Add which may trigger Load
			var sd = typeMapType.GetGenericArguments()[1];
			var sdInstance = CreateInstance(sd);
			var sdAdd = sd.GetMethod("Add").OrThrow();
			var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
			var p2 = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
			sdAdd.Invoke(sdInstance, [p1.PrimaryKey, p1]);
			sdAdd.Invoke(sdInstance, [p2.PrimaryKey, p2]);
			// Replace the whole map with one that contains only the Player sd to avoid duplicate Add
			var newMap = CreateInstance(typeMapType);
			typeMapType.GetMethod("Add")
					   .OrThrow()
					   .Invoke(newMap, [typeof (Player), sdInstance]);
			dataField.SetValue(null, newMap);

			// Verify FetchAll returns both
			var all = (System.Collections.IEnumerable)fetchAllMethod.MakeGenericMethod(typeof (Player))
																	.Invoke(null, null)
																	.OrThrow();
			var list = all.Cast<Player>().ToList();
			Assert.Contains(list, static x => x.Id == 1);
			Assert.Contains(list, static x => x.Id == 2);

			// FetchOne with predicate
			static bool Pred(Player pl)
				=> pl.Id == 2;

			var result = fetchOneFuncMethod.MakeGenericMethod(typeof (Player))
										   .Invoke(null, [(Func<Player, bool>)Pred]);
			Assert.IsType<Player>(result);
			Assert.Equal(2, ((Player)result).Id);

			// Flush clears all
			flushMethod.Invoke(null, null);
			// Inspect the underlying map directly to avoid triggering Load
			var mapAfterFlush = dataField.GetValue(null).OrThrow();
			var countProp = mapAfterFlush.GetType().GetProperty("Count");
			var count = (int)(countProp?.GetValue(mapAfterFlush) ?? 0);
			Assert.Equal(0, count);
		}
		finally
		{
			// Restore original cache to not affect other tests
			dataField.SetValue(null, original);
		}

		static void AddEmpty(object typeMapObj, Type type)
		{
			var mapType = typeMapObj.GetType();
			var sdType = mapType.GetGenericArguments()[1];
			var sd = CreateInstance(sdType);
			mapType.GetMethod("Add").OrThrow().Invoke(typeMapObj, [type, sd]);
		}
	}

	[Fact]
	public void Cache_Restore_Switches_Stores()
	{
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic).OrThrow("Cache type not found");
		var dataField = cacheType.GetField("_data", NonPublic | Static).OrThrow("Cache._data not found");
		var storesField = cacheType.GetField("Stores", NonPublic | Static).OrThrow("Cache.Stores not found");

		var originalData = dataField.GetValue(null).OrThrow();
		var storesObj = storesField.GetValue(null).OrThrow();
		var storesType = storesObj.GetType();
		var clearMethod = storesType.GetMethod("Clear").OrThrow();
		var addMethod = storesType.GetMethod("Add").OrThrow();
		var snapshot = ((System.Collections.IEnumerable)storesObj).Cast<object>()
																  .ToList();

		try
		{
			// Create two separate store maps
			var mapType = originalData.GetType();
			var storeA = CreateInstance(mapType);
			var storeB = CreateInstance(mapType);
			// Place in Stores under two keys via reflection
			clearMethod.Invoke(storesObj, null);
			addMethod.Invoke(storesObj, ["A", storeA]);
			addMethod.Invoke(storesObj, ["B", storeB]);

			// Switch to A
			cacheType.GetMethod("Restore", NonPublic | Static).OrThrow().Invoke(null, ["A"]);
			Assert.Same(storeA, dataField.GetValue(null));
			// Switch to B
			cacheType.GetMethod("Restore", NonPublic | Static).OrThrow().Invoke(null, ["B"]);
			Assert.Same(storeB, dataField.GetValue(null));
		}
		finally
		{
			// reset stores to snapshot and data to original
			clearMethod.Invoke(storesObj, null);
			var kvpType = snapshot.FirstOrDefault()?.GetType();
			if (kvpType is not null)
			{
				var keyProp = kvpType.GetProperty("Key").OrThrow();
				var valueProp = kvpType.GetProperty("Value").OrThrow();
				foreach (var item in snapshot)
					addMethod.Invoke(storesObj, [keyProp.GetValue(item), valueProp.GetValue(item)]);
			}
			dataField.SetValue(null, originalData);
		}
	}

	[Fact]
	public void Data_Any_Uses_Cache_Exists()
	{
		// Setup cache with three players
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic).OrThrow("Cache type not found");
		var dataField = cacheType.GetField("_data", NonPublic | Static).OrThrow("Cache._data not found");
		var original = dataField.GetValue(null).OrThrow();
		var mapType = original.GetType();
		var sdType = mapType.GetGenericArguments()[1];
		var typeMap = CreateInstance(mapType);
		var sd = CreateInstance(sdType);
		var add = sdType.GetMethod("Add").OrThrow();
		var p1 = new Player { Id = 1, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 2, FirstName = "B", LastName = "B" };
		var p3 = new Player { Id = 3, FirstName = "C", LastName = "C" };
		add.Invoke(sd, [p1.PrimaryKey, p1]);
		add.Invoke(sd, [p2.PrimaryKey, p2]);
		add.Invoke(sd, [p3.PrimaryKey, p3]);
		mapType.GetMethod("Add")
			   .OrThrow()
			   .Invoke(typeMap, [typeof (Player), sd]);

		dataField.SetValue(null, typeMap);
		try
		{
			Assert.True(Any<Player>());
			Assert.True(Any<Player>(static pl => pl.Id == 2));
			Assert.False(Any<Player>(static pl => pl.Id == 9));
		}
		finally
		{
			dataField.SetValue(null, original);
		}
	}
}
