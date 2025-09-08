using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class DataQueryCacheTests
{
	[Fact]
	public void ReadAll_And_ReadMany_Work_From_Cache()
	{
		// Seed Cache with three Players
		using var scope = SeedCache(map =>
		{
			AddMany(map, typeof(Player),
				new Player { Id = 1, FirstName = "Ann", LastName = "A" },
				new Player { Id = 2, FirstName = "Bob", LastName = "B" },
				new Player { Id = 3, FirstName = "Cat", LastName = "C" }
			);
		});

		var all = Data.ReadAll<Player>().ToList();
		Assert.Equal(3, all.Count);
		var many = Data.ReadMany<Player>(p => p.Id > 1).ToList();
		Assert.Equal(new[] { 2, 3 }, many.Select(p => p.Id).ToArray());
	}

	[Fact]
	public void ReadByName_And_NameExists_Use_Cache_And_Are_Case_Insensitive()
	{
		var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "A" }; // Name = "Ann A"
		var p2 = new Player { Id = 2, FirstName = "Bob", LastName = "B" }; // Name = "Bob B"
		using var scope = SeedCache(map => AddMany(map, typeof(Player), p1, p2));

		var found = Data.ReadByName<Player>("ann a");
		Assert.NotNull(found);
		Assert.Equal(1, found!.Id);

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
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data field not found");
		var original = field.GetValue(null) ?? throw new NullReferenceException();
		var typeMapType = original.GetType();
		var typeMap = System.Activator.CreateInstance(typeMapType)!;
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new(original, field);
	}

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable { public void Dispose() => Field.SetValue(null, Original); }

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = System.Activator.CreateInstance(sortedDictType)!;
		var sdAdd = sortedDictType.GetMethod("Add")!;
		foreach (var r in records)
		{
			var key = (string)r.GetType().GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(r)!;
			sdAdd.Invoke(sd, new object?[] { key, r });
		}
		typeMapType.GetMethod("Add")!.Invoke(typeMap, new object?[] { type, sd });
	}
}
