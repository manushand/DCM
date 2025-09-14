using System.Collections;
using System.Linq;

namespace Data.Tests;

[UsedImplicitly]
public sealed class DataCacheAddTests
{
	[Fact]
	public void Cache_Add_And_AddRange_Insert_Into_Existing_Map_Without_Load()
	{
		// Arrange: access nested private Cache and its _data store
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic)
									 .OrThrow("Cache type not found");
		var dataField = cacheType.GetField("_data", NonPublic | Static)
								 .OrThrow("Cache._data not found");
		var original = dataField.GetValue(null)
								.OrThrow();
		var mapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sdType = mapType.GetGenericArguments()[1];

		// Create a fresh, empty map and pre-register an empty SortedDictionary for Player
		var newMap = CreateInstance(mapType);
		var emptySd = CreateInstance(sdType);
		mapType.GetMethod("Add").OrThrow().Invoke(newMap, [typeof (Player), emptySd]);
		// Point Cache._data to our prepared map so Get<T>() won't try to Load from DB
		dataField.SetValue(null, newMap);

		try
		{
			// Get Add<T> and AddRange<T>
			var addMethod = cacheType.GetMethod("Add", NonPublic | Static).OrThrow().MakeGenericMethod(typeof (Player));
			var addRangeMethod = cacheType.GetMethod("AddRange", NonPublic | Static).OrThrow().MakeGenericMethod(typeof (Player));
			var fetchAll = cacheType.GetMethod("FetchAll", NonPublic | Static).OrThrow().MakeGenericMethod(typeof (Player));

			// Act: add one, then a range
			var a = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
			addMethod.Invoke(null, [a]);

			var b = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
			var c = new Player { Id = 3, FirstName = "Cat", LastName = "C" };
			addRangeMethod.Invoke(null, [new[] { b, c }]);

			// Assert: underlying dictionary now has three entries
			var all = ((IEnumerable)fetchAll.Invoke(null, null).OrThrow()).Cast<Player>().ToList();
			Assert.Equal(3, all.Count);
			Assert.Contains(all, static p => p.Id is 1);
			Assert.Contains(all, static p => p.Id is 2);
			Assert.Contains(all, static p => p.Id is 3);
		}
		finally
		{
			// restore
			dataField.SetValue(null, original);
		}
	}
}
