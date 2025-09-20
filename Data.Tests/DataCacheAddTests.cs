using System.Collections;
using System.Linq;
using System.Reflection;

namespace Data.Tests;

[UsedImplicitly]
public sealed class DataCacheAddTests : TestBase
{
	[Fact]
	public void Cache_Add_And_AddRange_Insert_Into_Existing_Map_Without_Load()
	{
		// Arrange: access nested private Cache and its _data store
		var original = DataField.GetValue(null)
								.OrThrow();
		var mapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sdType = mapType.GetGenericArguments()[1];

		// Create a fresh, empty map and pre-register an empty SortedDictionary for Player
		var newMap = CreateInstance(mapType);
		var emptySd = CreateInstance(sdType);
		mapType.GetMethod("Add")
			   .OrThrow()
			   .Invoke(newMap, [typeof (Player), emptySd]);
		// Point Cache._data to our prepared map so Get<T>() won't try to Load from DB
		DataField.SetValue(null, newMap);

		try
		{
			// Get Add<T> and AddRange<T>
			MethodInfo addMethod = CacheType.GetMethod("Add", NonPublic | Static).OrThrow().MakeGenericMethod(typeof (Player)),
					   addRangeMethod = CacheType.GetMethod("AddRange", NonPublic | Static).OrThrow().MakeGenericMethod(typeof (Player)),
					   fetchAll = CacheType.GetMethod("FetchAll", NonPublic | Static).OrThrow().MakeGenericMethod(typeof (Player));

			// Act: add one, then a range
			Player a = new () { Id = 1, FirstName = "Ann", LastName = "A" },
				   b = new () { Id = 2, FirstName = "Bob", LastName = "B" },
				   c = new () { Id = 3, FirstName = "Cat", LastName = "C" };
			addMethod.Invoke(null, [a]);
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
			DataField.SetValue(null, original);
		}
	}
}
