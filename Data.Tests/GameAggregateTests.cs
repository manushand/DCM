using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;
using static System.Activator;

namespace Data.Tests;

[PublicAPI]
public sealed class GameAggregateTests
{
	[Fact]
	public void AveragePreGameScore_Defaults_To_UnplayedScore_When_No_GamePlayers()
	{
		var t = new Tournament { Id = 1, Name = "T", UnplayedScore = 7 };
		var r = new Round { Id = 2, Number = 1 };
		// Wire Round -> Tournament without DB/cache
		typeof(Round).GetProperty("TournamentId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
			.SetValue(r, t.Id);
		typeof(Round).GetField("<Tournament>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(r, t);
		var g = new Game { Id = 3, Round = r };

		// Seed empty GamePlayer cache for this game to ensure enumeration is empty
		using (SeedCache(map =>
		{
			AddOne(map, typeof(Tournament), t);
			AddOne(map, typeof(Round), r);
			AddOne(map, typeof(Game), g);
			AddEmpty(map, typeof(GamePlayer));
			AddEmpty(map, typeof(ScoringSystem));
			AddEmpty(map, typeof(RoundPlayer));
			AddEmpty(map, typeof(TournamentPlayer));
			AddEmpty(map, typeof(Group));
			AddEmpty(map, typeof(GroupPlayer));
			AddEmpty(map, typeof(Team));
			AddEmpty(map, typeof(TeamPlayer));
			AddEmpty(map, typeof(PlayerConflict));
			AddEmpty(map, typeof(Player));
		}))
		{
			Assert.Equal(7, g.AveragePreGameScore);
		}
	}

	[Fact]
	public void Conflict_Sums_GamePlayer_Conflicts()
	{
		var t = new Tournament { Id = 10, Name = "T" };
		var r = new Round { Id = 11, Number = 1 };
		typeof(Round).GetProperty("TournamentId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
			.SetValue(r, t.Id);
		typeof(Round).GetField("<Tournament>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(r, t);
		var g = new Game { Id = 12, Round = r };
		var p1 = new Player { Id = 101, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 102, FirstName = "B", LastName = "B" };
		var gp1 = new GamePlayer { Game = g, Player = p1, Power = GamePlayer.Powers.Austria, Result = GamePlayer.Results.Unknown };
		var gp2 = new GamePlayer { Game = g, Player = p2, Power = GamePlayer.Powers.England, Result = GamePlayer.Results.Unknown };
		// Set private _conflict fields directly to avoid invoking CalculateConflict
		typeof(GamePlayer).GetField("_conflict", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(gp1, 3);
		typeof(GamePlayer).GetField("_conflict", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(gp2, 4);

		using (SeedCache(map =>
		{
			AddOne(map, typeof(Tournament), t);
			AddOne(map, typeof(Round), r);
			AddOne(map, typeof(Game), g);
			AddMany(map, typeof(GamePlayer), gp1, gp2);
			AddMany(map, typeof(Player), p1, p2);
			AddEmpty(map, typeof(ScoringSystem));
			AddEmpty(map, typeof(RoundPlayer));
			AddEmpty(map, typeof(TournamentPlayer));
			AddEmpty(map, typeof(Group));
			AddEmpty(map, typeof(GroupPlayer));
			AddEmpty(map, typeof(Team));
			AddEmpty(map, typeof(TeamPlayer));
			AddEmpty(map, typeof(PlayerConflict));
		}))
		{
			Assert.Equal(7, g.Conflict);
		}
	}

	[Fact]
	public void PlayerIds_Reflect_GamePlayers()
	{
		var t = new Tournament { Id = 20, Name = "T" };
		var r = new Round { Id = 21, Number = 1 };
		typeof(Round).GetProperty("TournamentId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
			.SetValue(r, t.Id);
		typeof(Round).GetField("<Tournament>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(r, t);
		var g = new Game { Id = 22, Round = r };
		var p1 = new Player { Id = 111, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 222, FirstName = "B", LastName = "B" };
		var gp1 = new GamePlayer { Game = g, Player = p1, Power = GamePlayer.Powers.Austria, Result = GamePlayer.Results.Unknown };
		var gp2 = new GamePlayer { Game = g, Player = p2, Power = GamePlayer.Powers.England, Result = GamePlayer.Results.Unknown };

		using (SeedCache(map =>
		{
			AddOne(map, typeof(Tournament), t);
			AddOne(map, typeof(Round), r);
			AddOne(map, typeof(Game), g);
			AddMany(map, typeof(GamePlayer), gp1, gp2);
			AddMany(map, typeof(Player), p1, p2);
			AddEmpty(map, typeof(ScoringSystem));
			AddEmpty(map, typeof(RoundPlayer));
			AddEmpty(map, typeof(TournamentPlayer));
			AddEmpty(map, typeof(Group));
			AddEmpty(map, typeof(GroupPlayer));
			AddEmpty(map, typeof(Team));
			AddEmpty(map, typeof(TeamPlayer));
			AddEmpty(map, typeof(PlayerConflict));
		}))
		{
			Assert.Equal(new[] { 111, 222 }, g.PlayerIds);
		}
	}


	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable { public void Dispose() => Field.SetValue(null, Original); }

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof(global::Data.Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data field not found");
		var original = field.GetValue(null) ?? throw new NullReferenceException();
		var typeMapType = original.GetType();
		var typeMap = CreateInstance(typeMapType)!;
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new(original, field);
	}

	private static void AddOne(object typeMap, Type type, object record)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType)!;
		sortedDictType.GetMethod("Add")?.Invoke(sd, new object?[] { GetPrimaryKey(record), record });
		typeMapType.GetMethod("Add")?.Invoke(typeMap, new object?[] { type, sd });
	}

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType)!;
		var sdAdd = sortedDictType.GetMethod("Add");
		foreach (var r in records)
			sdAdd?.Invoke(sd, new object?[] { GetPrimaryKey(r), r });
		typeMapType.GetMethod("Add")?.Invoke(typeMap, new object?[] { type, sd });
	}

	private static void AddEmpty(object typeMap, Type type)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType)!;
		typeMapType.GetMethod("Add")?.Invoke(typeMap, new object?[] { type, sd });
	}

	private static string GetPrimaryKey(object record)
	{
		var prop = record.GetType().GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				   ?? throw new InvalidOperationException($"PrimaryKey property not found on {record.GetType().Name}");
		return prop.GetValue(record) as string ?? throw new NullReferenceException();
	}
}
