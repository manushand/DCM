using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class DataCacheTests
{
	[Fact]
	public void Cache_Fetch_Flush_Work_Via_Reflection()
	{
		// Arrange: get nested Cache type and its backing _data field
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var dataField = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data not found");
		var original = dataField.GetValue(null) ?? throw new NullReferenceException();
		var typeMapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var typeMap = Activator.CreateInstance(typeMapType) ?? throw new NullReferenceException();

		// install empty maps for Player and others we'll use
		AddEmpty(typeMap, typeof(Player));

		// Point the cache to our empty map instance
		dataField.SetValue(null, typeMap);
		// Sanity: ensure the map contains Player key to avoid triggering DB-backed Load
		var containsKey = (bool)typeMapType.GetMethod("ContainsKey")!.Invoke(typeMap, new object?[] { typeof(Player) })!;
		Assert.True(containsKey, "Test setup failed: Cache does not contain key for Player (raw map)");
		var containsKeyMethod = cacheType.GetMethod("ContainsKey", BindingFlags.NonPublic | BindingFlags.Static)!;
		// Do not call Cache.ContainsKey<T>() to avoid triggering Load
		try
		{
			// Resolve generic methods we will use
			var fetchAllMethod = cacheType.GetMethod("FetchAll", BindingFlags.NonPublic | BindingFlags.Static);
			var fetchOneFuncMethod = cacheType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
				.First(m => m.Name == "FetchOne" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.IsGenericType);
			var flushMethod = cacheType.GetMethod("Flush", BindingFlags.NonPublic | BindingFlags.Static);

			Assert.NotNull(fetchOneFuncMethod);
			Assert.NotNull(fetchAllMethod);
			Assert.NotNull(flushMethod);

			// Manually seed players into the underlying SortedDictionary to avoid invoking Cache.Add which may trigger Load
			var typeMapTypeLocal = typeMapType;
			var sd = typeMapTypeLocal.GetGenericArguments()[1];
			var sdInstance = Activator.CreateInstance(sd)!;
			var sdAdd = sd.GetMethod("Add")!;
			var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
			var p2 = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
			sdAdd.Invoke(sdInstance, new object?[] { p1.PrimaryKey, p1 });
			sdAdd.Invoke(sdInstance, new object?[] { p2.PrimaryKey, p2 });
			// Replace the whole map with one that contains only the Player sd to avoid duplicate Add
			var newMap = Activator.CreateInstance(typeMapTypeLocal)!;
			typeMapTypeLocal.GetMethod("Add")!.Invoke(newMap, new object?[] { typeof(Player), sdInstance });
			dataField.SetValue(null, newMap);
			typeMap = newMap;

			// Verify FetchAll returns both
			var all = (System.Collections.IEnumerable)fetchAllMethod!.MakeGenericMethod(typeof(Player)).Invoke(null, null)!;
			var list = all.Cast<Player>().ToList();
			Assert.Contains(list, x => x.Id == 1);
			Assert.Contains(list, x => x.Id == 2);

			// FetchOne with predicate
			Func<Player, bool> pred = pl => pl.Id == 2;
			var result = fetchOneFuncMethod!.MakeGenericMethod(typeof(Player)).Invoke(null, new object?[] { pred });
			Assert.IsType<Player>(result);
			Assert.Equal(2, ((Player)result!).Id);

			// Flush clears all
			flushMethod!.Invoke(null, null);
			// Inspect the underlying map directly to avoid triggering Load
			var mapAfterFlush = dataField.GetValue(null) ?? throw new NullReferenceException();
			var countProp = mapAfterFlush.GetType().GetProperty("Count");
			var count = (int)(countProp?.GetValue(mapAfterFlush) ?? 0);
			Assert.Equal(0, count);
		}
		finally
		{
			// Restore original cache to not affect other tests
			dataField.SetValue(null, original);
		}

		void AddEmpty(object typeMapObj, Type type)
		{
			var mapType = typeMapObj.GetType();
			var sdType = mapType.GetGenericArguments()[1];
			var sd = Activator.CreateInstance(sdType)!;
			mapType.GetMethod("Add")!.Invoke(typeMapObj, new object?[] { type, sd });
		}
	}

	[Fact]
	public void Cache_Restore_Switches_Stores()
	{
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var dataField = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data not found");
		var storesField = cacheType.GetField("Stores", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache.Stores not found");

		var originalData = dataField.GetValue(null) ?? throw new NullReferenceException();
		var storesObj = storesField.GetValue(null) ?? throw new NullReferenceException();
		var storesType = storesObj.GetType();
		var clearMethod = storesType.GetMethod("Clear")!;
		var addMethod = storesType.GetMethod("Add")!;
		var snapshot = ((System.Collections.IEnumerable)storesObj)
			.Cast<object>()
			.ToList();

		try
		{
			// Create two separate store maps
			var mapType = originalData.GetType();
			var storeA = Activator.CreateInstance(mapType)!;
			var storeB = Activator.CreateInstance(mapType)!;
			// Place in Stores under two keys via reflection
			clearMethod.Invoke(storesObj, null);
			addMethod.Invoke(storesObj, new object?[] { "A", storeA });
			addMethod.Invoke(storesObj, new object?[] { "B", storeB });

			// Switch to A
			cacheType.GetMethod("Restore", BindingFlags.NonPublic | BindingFlags.Static)!.Invoke(null, new object?[] { "A" });
			Assert.Same(storeA, dataField.GetValue(null));
			// Switch to B
			cacheType.GetMethod("Restore", BindingFlags.NonPublic | BindingFlags.Static)!.Invoke(null, new object?[] { "B" });
			Assert.Same(storeB, dataField.GetValue(null));
		}
		finally
		{
			// reset stores to snapshot and data to original
			clearMethod.Invoke(storesObj, null);
			var kvpType = snapshot.FirstOrDefault()?.GetType();
			if (kvpType is not null)
			{
				var keyProp = kvpType.GetProperty("Key")!;
				var valueProp = kvpType.GetProperty("Value")!;
				foreach (var item in snapshot)
					addMethod.Invoke(storesObj, new object?[] { keyProp.GetValue(item), valueProp.GetValue(item) });
			}
			dataField.SetValue(null, originalData);
		}
	}

	[Fact]
	public void Data_Any_Uses_Cache_Exists()
	{
		// Setup cache with three players
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var dataField = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data not found");
		var original = dataField.GetValue(null) ?? throw new NullReferenceException();
		var mapType = original.GetType();
		var sdType = mapType.GetGenericArguments()[1];
		var typeMap = Activator.CreateInstance(mapType)!;
		var sd = Activator.CreateInstance(sdType)!;
		var add = sdType.GetMethod("Add")!;
		var p1 = new Player { Id = 1, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 2, FirstName = "B", LastName = "B" };
		var p3 = new Player { Id = 3, FirstName = "C", LastName = "C" };
		add.Invoke(sd, new object?[] { p1.PrimaryKey, p1 });
		add.Invoke(sd, new object?[] { p2.PrimaryKey, p2 });
		add.Invoke(sd, new object?[] { p3.PrimaryKey, p3 });
		mapType.GetMethod("Add")!.Invoke(typeMap, new object?[] { typeof(Player), sd });

		dataField.SetValue(null, typeMap);
		try
		{
			Assert.True(Data.Any<Player>());
			Assert.True(Data.Any<Player>(pl => pl.Id == 2));
			Assert.False(Data.Any<Player>(pl => pl.Id == 9));
		}
		finally
		{
			dataField.SetValue(null, original);
		}
	}
}
