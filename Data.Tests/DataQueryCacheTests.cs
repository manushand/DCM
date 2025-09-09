using System.Linq;

namespace Data.Tests;

[PublicAPI]
public sealed class DataQueryCacheTests : TestBase
{
	private static readonly int[] Expected = [2, 3];

	[Fact]
	public void ReadAll_And_ReadMany_Work_From_Cache()
	{
		// Seed Cache with three Players
		using var scope = SeedCache(static map =>
									{
										AddMany(map,
												typeof (Player),
												new Player { Id = 1, FirstName = "Ann", LastName = "A" },
												new Player { Id = 2, FirstName = "Bob", LastName = "B" },
												new Player { Id = 3, FirstName = "Cat", LastName = "C" });
									});

		var all = Data.ReadAll<Player>().ToList();
		Assert.Equal(3, all.Count);
		var many = Data.ReadMany<Player>(static p => p.Id > 1).ToList();
		Assert.Equal(Expected, many.Select(static p => p.Id).ToArray());
	}

	[Fact]
	public void ReadByName_And_NameExists_Use_Cache_And_Are_Case_Insensitive()
	{
		var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "A" }; // Name = "Ann A"
		var p2 = new Player { Id = 2, FirstName = "Bob", LastName = "B" }; // Name = "Bob B"
		using var scope = SeedCache(map => AddMany(map, typeof (Player), p1, p2));

		var found = Data.ReadByName<Player>("ann a");
		Assert.NotNull(found);
		Assert.Equal(1, found.Id);

		// NameExists returns true when some other Player has same name
		var updating = new Player { Id = 99, FirstName = "Bob", LastName = "B" };
		Assert.True(Data.NameExists(updating));

		// When unique name, returns false
		updating.FirstName = "Zoe";
		updating.LastName = "Z";
		Assert.False(Data.NameExists(updating));
	}

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic)
									 .OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", NonPublic | Static)
							 .OrThrow("Cache._data field not found");
		var original = field.GetValue(null)
							.OrThrow();
		var typeMapType = original.GetType();
		var typeMap = CreateInstance(typeMapType)
							   .OrThrow();
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new (original, field);
	}

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType)
						  .OrThrow();
		var sdAdd = sortedDictType.GetMethod("Add")
								  .OrThrow();
		foreach (var r in records)
		{
			var key = (string)r.GetType()
							   .GetProperty("PrimaryKey", Instance | Public)
							   .OrThrow()
							   .GetValue(r)
							   .OrThrow();
			sdAdd.Invoke(sd, [key, r]);
		}
		typeMapType.GetMethod("Add")
				   .OrThrow()
				   .Invoke(typeMap, [type, sd]);
	}
}
