using System.Collections;
using System.Linq;

namespace Data.Tests;

[UsedImplicitly]
public sealed class DataCacheRemoveAndFetchTests : TestBase
{
	[Fact]
	public void FetchOne_ByRecordKey_And_Remove_By_Key_And_Params()
	{
		var original = DataField.GetValue(null).OrThrow();
		var mapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sdType = mapType.GetGenericArguments()[1];
		var map = CreateInstance(mapType);
		var sd = CreateInstance(sdType); // for Player
		var addKvp = sdType.GetMethod("Add").OrThrow();

		Player p1 = new () { Id = 1, FirstName = "Ann", LastName = "A" },
			   p2 = new () { Id = 2, FirstName = "Bob", LastName = "B" },
			   p3 = new () { Id = 3, FirstName = "Cat", LastName = "C" };
		addKvp.Invoke(sd, [p1.PrimaryKey, p1]);
		addKvp.Invoke(sd, [p2.PrimaryKey, p2]);
		addKvp.Invoke(sd, [p3.PrimaryKey, p3]);
		mapType.GetMethod("Add")
			   .OrThrow()
			   .Invoke(map, [typeof (Player), sd]);
		DataField.SetValue(null, map);

		try
		{
			// Invoke Cache.FetchOne<T>(T record)
			var fetchOneRecord = CacheType.GetMethods(NonPublic | Static)
										  .First(static m => m.Name is "FetchOne"
														  && m.GetParameters().Length is 1
														  && !m.GetParameters()[0].ParameterType.IsGenericType)
										  .MakeGenericMethod(typeof (Player));
			var keyOnly = new Player { Id = 2, FirstName = "X", LastName = "Y" }; // same primary key as p2
			var found = (Player?)fetchOneRecord.Invoke(null, [keyOnly]);
			Assert.NotNull(found);
			Assert.Same(p2, found);

			// Invoke Cache.Remove<T>(string key)
			var removeByKey = CacheType.GetMethod("Remove", NonPublic | Static, null, [typeof (string)], null)
									   .OrThrow()
									   .MakeGenericMethod(typeof (Player));
			removeByKey.Invoke(null, [p1.PrimaryKey]);
			var fetchAll = CacheType.GetMethod("FetchAll", NonPublic | Static)
									.OrThrow()
									.MakeGenericMethod(typeof (Player));
			var all = ((IEnumerable)fetchAll.Invoke(null, null).OrThrow()).Cast<Player>();
			Assert.DoesNotContain(all, static r => r.Id is 1);

			// Invoke Cache.Remove<T>(params T[] records) via method discovery on generic array parameter
			var removeParams = CacheType.GetMethods(NonPublic | Static)
										.First(static m => m is { Name: "Remove", IsGenericMethodDefinition: true }
														&& m.GetParameters().Length is 1
														&& m.GetParameters()[0].ParameterType.IsArray)
										.MakeGenericMethod(typeof (Player));
			removeParams.Invoke(null, [new[] { p2, p3 }]);
			all = ((IEnumerable)fetchAll.Invoke(null, null).OrThrow()).Cast<Player>();
			Assert.Empty(all);
		}
		finally
		{
			DataField.SetValue(null, original);
		}
	}

	[Fact]
	public void Restore_Uses_Factory_For_New_Store_Key()
	{
		var originalData = DataField.GetValue(null).OrThrow();
		var storesObj = StoresField.GetValue(null).OrThrow();
		var storesType = storesObj.GetType(); // Dictionary<string, CacheType>

		// Snapshot existing entries so we can restore later
		var snapshot = ((IEnumerable)storesObj).Cast<object>().ToList();
		var keyProp = snapshot.FirstOrDefault()?.GetType().GetProperty("Key");
		var valueProp = snapshot.FirstOrDefault()?.GetType().GetProperty("Value");
		var clearMethod = storesType.GetMethod("Clear").OrThrow();
		var addMethod = storesType.GetMethod("Add").OrThrow();

		// Clear to ensure GetOrSet factory branch runs
		clearMethod.Invoke(storesObj, null);
		try
		{
			// Call Restore with a brand-new key so Stores.GetOrSet factory branch executes
			var restore = CacheType.GetMethod("Restore", NonPublic | Static).OrThrow();
			const string newKey = "__NEW_STORE__";
			var containsBefore = (bool)storesType.GetMethod("ContainsKey")
												 .OrThrow()
												 .Invoke(storesObj, [newKey])
												 .OrThrow();
			Assert.False(containsBefore);
			restore.Invoke(null, [newKey]);
			Assert.NotNull(DataField.GetValue(null));
			// And make sure Stores now contains the new key (factory executed)
			var containsKey = (bool)storesType.GetMethod("ContainsKey")
											  .OrThrow()
											  .Invoke(storesObj, [newKey])
											  .OrThrow();
			Assert.True(containsKey);
		}
		finally
		{
			// Restore original Stores content
			clearMethod.Invoke(storesObj, null);
			if (keyProp is not null && valueProp is not null)
				foreach (var item in snapshot)
					addMethod.Invoke(storesObj, [keyProp.GetValue(item), valueProp.GetValue(item)]);
			DataField.SetValue(null, originalData);
		}
	}
}
