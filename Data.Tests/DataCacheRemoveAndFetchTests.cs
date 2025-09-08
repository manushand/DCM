using System;
using System.Linq;
using System.Reflection;
using DCM;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[UsedImplicitly]
public sealed class DataCacheRemoveAndFetchTests
{
	private static Type CacheType() => typeof (Data).GetNestedType("Cache", BindingFlags.NonPublic).OrThrow("Cache type not found");
	private static FieldInfo DataField() => CacheType().GetField("_data", BindingFlags.NonPublic | BindingFlags.Static).OrThrow("Cache._data not found");
	private static FieldInfo StoresField() => CacheType().GetField("Stores", BindingFlags.NonPublic | BindingFlags.Static).OrThrow("Cache.Stores not found");

	[Fact]
	public void FetchOne_ByRecordKey_And_Remove_By_Key_And_Params()
	{
		var cacheType = CacheType();
		var dataField = DataField();
		var original = dataField.GetValue(null).OrThrow();
		var mapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sdType = mapType.GetGenericArguments()[1];
		var map = Activator.CreateInstance(mapType).OrThrow();
		var sd = Activator.CreateInstance(sdType).OrThrow(); // for Player
		var addKvp = sdType.GetMethod("Add").OrThrow();

		var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
		var p2 = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
		var p3 = new Player { Id = 3, FirstName = "Cat", LastName = "C" };
		addKvp.Invoke(sd, [p1.PrimaryKey, p1]);
		addKvp.Invoke(sd, [p2.PrimaryKey, p2]);
		addKvp.Invoke(sd, [p3.PrimaryKey, p3]);
		mapType.GetMethod("Add").OrThrow().Invoke(map, [typeof (Player), sd]);
		dataField.SetValue(null, map);

		try
		{
			// Invoke Cache.FetchOne<T>(T record)
			var fetchOneRecord = cacheType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
				.First(static m => m.Name is "FetchOne" && m.GetParameters().Length == 1 && !m.GetParameters()[0].ParameterType.IsGenericType)
				.MakeGenericMethod(typeof (Player));
			var keyOnly = new Player { Id = 2, FirstName = "X", LastName = "Y" }; // same primary key as p2
			var found = (Player?)fetchOneRecord.Invoke(null, [keyOnly]);
			Assert.NotNull(found);
			Assert.Same(p2, found);

			// Invoke Cache.Remove<T>(string key)
			var removeByKey = cacheType.GetMethod("Remove", BindingFlags.NonPublic | BindingFlags.Static, null, [typeof (string)], null)
									   .OrThrow()
									   .MakeGenericMethod(typeof (Player));
			removeByKey.Invoke(null, [p1.PrimaryKey]);
			var fetchAll = cacheType.GetMethod("FetchAll", BindingFlags.NonPublic | BindingFlags.Static)
									.OrThrow()
									.MakeGenericMethod(typeof (Player));
			var all = ((System.Collections.IEnumerable)fetchAll.Invoke(null, null).OrThrow()).Cast<Player>().ToList();
			Assert.DoesNotContain(all, static r => r.Id is 1);

			// Invoke Cache.Remove<T>(params T[] records) via method discovery on generic array parameter
			var removeParams = cacheType
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
				.First(static m => m is { Name: "Remove", IsGenericMethodDefinition: true } && m.GetParameters().Length is 1 && m.GetParameters()[0].ParameterType.IsArray)
				.MakeGenericMethod(typeof (Player));
			removeParams.Invoke(null, [new[] { p2, p3 }]);
			all = ((System.Collections.IEnumerable)fetchAll.Invoke(null, null).OrThrow()).Cast<Player>().ToList();
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
		var originalData = dataField.GetValue(null).OrThrow();
		var storesObj = storesField.GetValue(null).OrThrow();
		var storesType = storesObj.GetType(); // Dictionary<string, CacheType>

		// Snapshot existing entries so we can restore later
		var snapshot = ((System.Collections.IEnumerable)storesObj).Cast<object>().ToList();
		var keyProp = snapshot.FirstOrDefault()?.GetType().GetProperty("Key");
		var valueProp = snapshot.FirstOrDefault()?.GetType().GetProperty("Value");
		var clearMethod = storesType.GetMethod("Clear").OrThrow();
		var addMethod = storesType.GetMethod("Add").OrThrow();

		// Clear to ensure GetOrSet factory branch runs
		clearMethod.Invoke(storesObj, null);
		try
		{
			// Call Restore with a brand-new key so Stores.GetOrSet factory branch executes
			var restore = cacheType.GetMethod("Restore", BindingFlags.NonPublic | BindingFlags.Static).OrThrow();
			const string newKey = "__NEW_STORE__";
			var containsBefore = (bool)storesType.GetMethod("ContainsKey").OrThrow().Invoke(storesObj, [newKey]).OrThrow();
			Assert.False(containsBefore);
			restore.Invoke(null, [newKey]);
			var current = dataField.GetValue(null);
			Assert.NotNull(current);
			// And make sure Stores now contains the new key (factory executed)
			var containsKey = (bool)storesType.GetMethod("ContainsKey").OrThrow().Invoke(storesObj, [newKey]).OrThrow();
			Assert.True(containsKey);
		}
		finally
		{
			// Restore original Stores content
			clearMethod.Invoke(storesObj, null);
			if (keyProp is not null && valueProp is not null)
				foreach (var item in snapshot)
					addMethod.Invoke(storesObj, [keyProp.GetValue(item), valueProp.GetValue(item)]);
			dataField.SetValue(null, originalData);
		}
	}
}
