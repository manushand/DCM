using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Data.Tests;

public sealed class DataCacheAddTests
{
	[Fact]
	public void Cache_Add_And_AddRange_Insert_Into_Existing_Map_Without_Load()
	{
		// Arrange: access nested private Cache and its _data store
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var dataField = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data not found");
		var original = dataField.GetValue(null) ?? throw new NullReferenceException();
		var mapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sdType = mapType.GetGenericArguments()[1];

		// Create a fresh, empty map and pre-register an empty SortedDictionary for Player
		var newMap = Activator.CreateInstance(mapType)!;
		var emptySd = Activator.CreateInstance(sdType)!;
		mapType.GetMethod("Add")!.Invoke(newMap, new object?[] { typeof(Player), emptySd });
		// Point Cache._data to our prepared map so Get<T>() won't try to Load from DB
		dataField.SetValue(null, newMap);

		try
		{
			// Get Add<T> and AddRange<T>
			var addMethod = cacheType.GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(typeof(Player));
			var addRangeMethod = cacheType.GetMethod("AddRange", BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(typeof(Player));
			var fetchAll = cacheType.GetMethod("FetchAll", BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(typeof(Player));

			// Act: add one, then a range
			var a = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
			addMethod.Invoke(null, new object?[] { a });

			var b = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
			var c = new Player { Id = 3, FirstName = "Cat", LastName = "C" };
			addRangeMethod.Invoke(null, new object?[] { new[] { b, c } });

			// Assert: underlying dictionary now has three entries
			var all = ((System.Collections.IEnumerable)fetchAll.Invoke(null, null)!).Cast<Player>().ToList();
			Assert.Equal(3, all.Count);
			Assert.Contains(all, p => p.Id == 1);
			Assert.Contains(all, p => p.Id == 2);
			Assert.Contains(all, p => p.Id == 3);
		}
		finally
		{
			// restore
			dataField.SetValue(null, original);
		}
	}
}
