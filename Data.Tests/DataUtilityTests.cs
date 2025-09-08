using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

using DCM;
using static System.Activator;

[PublicAPI]
public sealed class DataUtilityTests
{
	[Fact]
	public void Sorted_Sorts_By_Name_Or_LastName()
	{
		var p1 = new Player { FirstName = "Bob", LastName = "Zed" };   // Name: Bob Zed, LastFirst: Zed Bob
		var p2 = new Player { FirstName = "Ann", LastName = "Young" }; // Name: Ann Young, LastFirst: Young Ann
		var list = new List<Player> { p1, p2 };

		// Default sorts by Name
		var byName = list.Sorted().ToArray();
		Assert.Same(p2, byName[0]);
		Assert.Same(p1, byName[1]);

		// Sort by last name
		var byLast = list.Sorted(byLastName: true).ToArray();
		Assert.Same(p2, byLast[0]);
		Assert.Same(p1, byLast[1]);
	}

	[Fact]
	public void SelectSorted_Projects_And_Sorts_By_Comparable()
	{
		var t1 = new Team { Name = "Zulu" };
		var t2 = new Team { Name = "Alpha" };
		var names = new[] { t1, t2 }.SelectSorted(static t => t.Name).ToArray();
		Assert.Equal(new[] { "Alpha", "Zulu" }, names);
	}

	[Fact]
	public void Ids_Extracts_Identity_Ids()
	{
		var a = new Team { Id = 10, Name = "A" };
		var b = new Team { Id = 7, Name = "B" };
		var ids = new[] { a, b }.Ids().OrderBy(static x => x).ToArray();
		Assert.Equal(new[] { 7, 10 }, ids);
	}

	[Fact]
	public void HasPlayerId_And_ByPlayerId_Work_For_LinkRecords()
	{
		var tp1 = NewTeamPlayerViaLoad(teamId: 1, playerId: 5);
		var tp2 = NewTeamPlayerViaLoad(teamId: 1, playerId: 6);
		var list = new[] { tp1, tp2 };
		Assert.True(list.HasPlayerId(6));
		Assert.False(list.HasPlayerId(99));
		Assert.Equal(5, list.ByPlayerId(5).PlayerId);
	}

	[Fact]
	public void ConnectToAccessDatabase_Returns_False_When_No_Providers_Work()
	{
		// We pass a bogus file path; providers will throw/open-fail and method should return false.
		var ok = Data.ConnectToAccessDatabase("X:\\nonexistent\\db.accdb");
		Assert.False(ok);
	}

	[Fact]
	public void ReadByName_And_NameExists_Use_Cache_CaseInsensitive()
	{
		var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "Lee" };
		using (SeedCache(map =>
		{
			AddOne(map, typeof(Player), p1);
		}))
		{
			var read = Data.ReadByName<Player>("ann lee");
			Assert.NotNull(read);
			Assert.Equal(1, read!.Id);

			// NameExists returns false when comparing with itself
			Assert.False(Data.NameExists(p1));

			// But true for a different instance having same name
			var p2 = new Player { Id = 2, FirstName = "Ann", LastName = "Lee" };
			Assert.True(Data.NameExists(p2));
		}
	}

	// Helpers (mirrors of similar helpers in existing tests)
	private static TeamPlayer NewTeamPlayerViaLoad(int teamId, int playerId)
	{
		var tp = new TeamPlayer();
		var values = new Dictionary<string, object?>
		{
			{ "TeamId", teamId },
			{ "PlayerId", playerId }
		};
		using var reader = new Helpers.FakeDbDataReader("TeamPlayer", values);
		tp.Load(reader);
		return tp;
	}

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic)
					 ?? throw new InvalidOperationException("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static)
				 ?? throw new InvalidOperationException("Cache._data field not found");
		var original = field.GetValue(null) ?? throw new NullReferenceException();
		var typeMapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var typeMap = CreateInstance(typeMapType).OrThrow();
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new (original, field);
	}

	private static void AddOne(object typeMap, Type type, object record)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		sortedDictType.GetMethod("Add")?.Invoke(sd, [GetPrimaryKey(record), record]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	private static string GetPrimaryKey(object record)
	{
		var prop = record.GetType()
					 .GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					 .OrThrow($"PrimaryKey property not found on {record.GetType().Name}");
		return prop.GetValue(record) as string ?? throw new NullReferenceException();
	}
}
