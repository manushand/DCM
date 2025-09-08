using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Data.Tests;

public sealed class DataCacheRemoveAndFetchTests
{
	private static Type CacheType() => typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
	private static FieldInfo DataField() => CacheType().GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data not found");
	private static FieldInfo StoresField() => CacheType().GetField("Stores", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache.Stores not found");

	[Fact]
	public void FetchOne_ByRecordKey_And_Remove_By_Key_And_Params()
	{
		var cacheType = CacheType();
		var dataField = DataField();
		var original = dataField.GetValue(null) ?? throw new NullReferenceException();
		var mapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sdType = mapType.GetGenericArguments()[1];
		var map = Activator.CreateInstance(mapType)!;
		var sd = Activator.CreateInstance(sdType)!; // for Player
		var addKvp = sdType.GetMethod("Add")!;

		var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
		var p2 = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
		var p3 = new Player { Id = 3, FirstName = "Cat", LastName = "C" };
		addKvp.Invoke(sd, new object?[] { p1.PrimaryKey, p1 });
		addKvp.Invoke(sd, new object?[] { p2.PrimaryKey, p2 });
		addKvp.Invoke(sd, new object?[] { p3.PrimaryKey, p3 });
		mapType.GetMethod("Add")!.Invoke(map, new object?[] { typeof(Player), sd });
		dataField.SetValue(null, map);

		try
		{
			// Invoke Cache.FetchOne<T>(T record)
			var fetchOneRecord = cacheType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
				.First(m => m.Name == "FetchOne" && m.GetParameters().Length == 1 && !m.GetParameters()[0].ParameterType.IsGenericType)
				.MakeGenericMethod(typeof(Player));
			var keyOnly = new Player { Id = 2, FirstName = "X", LastName = "Y" }; // same primary key as p2
			var found = (Player?)fetchOneRecord.Invoke(null, new object?[] { keyOnly });
			Assert.NotNull(found);
			Assert.Same(p2, found);

			// Invoke Cache.Remove<T>(string key)
			var removeByKey = cacheType.GetMethod("Remove", BindingFlags.NonPublic | BindingFlags.Static, null, new[] { typeof(string) }, null)!
				.MakeGenericMethod(typeof(Player));
			removeByKey.Invoke(null, new object?[] { p1.PrimaryKey });
			var fetchAll = cacheType.GetMethod("FetchAll", BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(typeof(Player));
			var all = ((System.Collections.IEnumerable)fetchAll.Invoke(null, null)!).Cast<Player>().ToList();
			Assert.DoesNotContain(all, r => r.Id == 1);

 			// Invoke Cache.Remove<T>(params T[] records) via method discovery on generic array parameter
			var removeParams = cacheType
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
				.First(m => m.Name == "Remove" && m.IsGenericMethodDefinition && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.IsArray)
				.MakeGenericMethod(typeof(Player));
			removeParams.Invoke(null, new object?[] { new[] { p2, p3 } });
			all = ((System.Collections.IEnumerable)fetchAll.Invoke(null, null)!).Cast<Player>().ToList();
			Assert.Empty(all);
		}
		finally
		{
			dataField.SetValue(null, original);
		}
	}

	[Fact]
	public void Restore_Uses_Factory_For_New_Store_Key()
	{
		var cacheType = CacheType();
		var dataField = DataField();
		var storesField = StoresField();
		var originalData = dataField.GetValue(null) ?? throw new NullReferenceException();
		var storesObj = storesField.GetValue(null) ?? throw new NullReferenceException();
		var storesType = storesObj.GetType(); // Dictionary<string, CacheType>

		// Snapshot existing entries so we can restore later
		var snapshot = ((System.Collections.IEnumerable)storesObj).Cast<object>().ToList();
		var keyProp = snapshot.FirstOrDefault()?.GetType().GetProperty("Key");
		var valueProp = snapshot.FirstOrDefault()?.GetType().GetProperty("Value");
		var clearMethod = storesType.GetMethod("Clear")!;
		var addMethod = storesType.GetMethod("Add")!;

		// Clear to ensure GetOrSet factory branch runs
		clearMethod.Invoke(storesObj, null);
		try
		{
			// Call Restore with a brand-new key so Stores.GetOrSet factory branch executes
			var restore = cacheType.GetMethod("Restore", BindingFlags.NonPublic | BindingFlags.Static)!;
			const string NewKey = "__NEW_STORE__";
			var containsBefore = (bool)storesType.GetMethod("ContainsKey")!.Invoke(storesObj, new object?[] { NewKey })!;
			Assert.False(containsBefore);
			restore.Invoke(null, new object?[] { NewKey });
			var current = dataField.GetValue(null);
			Assert.NotNull(current);
			// And make sure Stores now contains the new key (factory executed)
			var containsKey = (bool)(storesType.GetMethod("ContainsKey")!.Invoke(storesObj, new object?[] { NewKey })!);
			Assert.True(containsKey);
		}
		finally
		{
			// Restore original Stores content
			clearMethod.Invoke(storesObj, null);
			if (keyProp != null && valueProp != null)
			{
				foreach (var item in snapshot)
					addMethod.Invoke(storesObj, new object?[] { keyProp.GetValue(item), valueProp.GetValue(item) });
			}
			dataField.SetValue(null, originalData);
		}
	}
}
