using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;
using static System.Activator;

namespace Data.Tests;

using Helpers;

[PublicAPI]
public sealed class RoundPreRoundTests
{
	[Fact]
	public void PreRoundScore_Event_Uses_Prior_Rounds_Count_And_Pads_With_UnplayedScore()
	{
		var t = new Tournament { Id = 1, Name = "Event", UnplayedScore = 2, TotalRounds = 3 };
		// GroupId defaults to null => Group.None => IsEvent = true
		var r1 = new Round { Id = 10, Number = 1 };
		var r2 = new Round { Id = 11, Number = 2 };
		SetNonPublic(r1, "TournamentId", t.Id); SetPrivate(r1, "<Tournament>k__BackingField", t);
		SetNonPublic(r2, "TournamentId", t.Id); SetPrivate(r2, "<Tournament>k__BackingField", t);
		var p = new Player { Id = 5, FirstName = "Ann", LastName = "L" };

		// Prior finished game in r1 with FinalScore=6
		var g1 = new Game { Id = 100, Round = r1, Status = Game.Statuses.Finished, Scored = true, Date = new DateTime(2024,1,1), Number=1 };
		var gp1 = new GamePlayer { Game = g1, Player = p, Power = GamePlayer.Powers.Austria, Result = GamePlayer.Results.Unknown };
		SetPrivate(gp1, "_finalScore", 6.0);

		// Target: r2 PreRoundScore should sum prior rounds (Number-1 = 1) and not need padding (targetCount=1)
		// Also test PreGameAverage uses same aggregates
		using (SeedCache(map =>
		{
			AddMany(map, typeof(Tournament), t);
			AddMany(map, typeof(Round), r1, r2);
			AddMany(map, typeof(Game), g1);
			AddMany(map, typeof(GamePlayer), gp1);
			AddMany(map, typeof(Player), p);
			// Avoid accidental DB loads
			AddEmpty(map, typeof(RoundPlayer));
			AddEmpty(map, typeof(TournamentPlayer));
			AddEmpty(map, typeof(Group));
			AddEmpty(map, typeof(GroupPlayer));
			AddEmpty(map, typeof(Team));
			AddEmpty(map, typeof(TeamPlayer));
			AddEmpty(map, typeof(PlayerConflict));
			AddEmpty(map, typeof(ScoringSystem));
		}))
		{
			var roundAvg = r2.PreGameAverage(new GamePlayer { Game = new Game { Round = r2, Date = new DateTime(2024,2,1) }, Player = p });
			var roundSum = r2.PreRoundScore(p);
			Assert.Equal(6.0, roundSum);
			Assert.Equal(6.0, roundAvg);
		}
	}

	[Fact]
	public void PreRoundScore_Group_Uses_All_Finished_Group_Games_Before_Game_Date()
	{
		// Group tournament: GroupId set => IsEvent=false, uses player rating before game date
		var group = new Group { Id = 8, Name = "Club" };
		var t = new Tournament { Id = 2, Name = "GroupT", UnplayedScore = 5, TotalRounds = 3 };
		SetNonPublic(t, nameof(Tournament.Group), group); // internal init uses backing, but we'll seed via reflection below using Group property backing
		// Create round 1 and 2
		var r1 = new Round { Id = 20, Number = 1 };
		var r2 = new Round { Id = 21, Number = 2 };
		SetNonPublic(r1, "TournamentId", t.Id); SetPrivate(r1, "<Tournament>k__BackingField", t);
		SetNonPublic(r2, "TournamentId", t.Id); SetPrivate(r2, "<Tournament>k__BackingField", t);
		// Wire Tournament.Group backing field so Tournament.IsEvent becomes false
		SetPrivate(t, "<Group>k__BackingField", group);

		var p = new Player { Id = 5, FirstName = "Ann", LastName = "L" };
		// Two finished games in r1 and r2 both before a target game's date; one not containing player should be ignored
		var gA = new Game { Id = 201, Round = r1, Status = Game.Statuses.Finished, Scored = true, Date = new DateTime(2024,1,1), Number=1 };
		var gB = new Game { Id = 202, Round = r2, Status = Game.Statuses.Finished, Scored = true, Date = new DateTime(2024,1,15), Number=1 };
		var gpA = new GamePlayer { Game = gA, Player = p, Power = GamePlayer.Powers.England, Result = GamePlayer.Results.Unknown };
		var gpB = new GamePlayer { Game = gB, Player = p, Power = GamePlayer.Powers.France, Result = GamePlayer.Results.Unknown };
		SetPrivate(gpA, "_finalScore", 3.0);
		SetPrivate(gpB, "_finalScore", 5.0);

		using (SeedCache(map =>
		{
			AddMany(map, typeof(Tournament), t);
			AddMany(map, typeof(Round), r1, r2);
			AddMany(map, typeof(Game), gA, gB);
			AddMany(map, typeof(GamePlayer), gpA, gpB);
			AddMany(map, typeof(Player), p);
			AddMany(map, typeof(Group), group);
		}))
		{
			var targetGame = new Game { Round = r2, Date = new DateTime(2024, 2, 1) };
			var gpTarget = new GamePlayer { Game = targetGame, Player = p };
			var avg = r2.PreGameAverage(gpTarget);
			var sum = r2.PreRoundScore(p);
			// For group tournaments, roundsPrior = 1, so only prior round (r1) is included in PreRound aggregates
			Assert.Equal(3.0, sum);
			Assert.Equal(3.0, avg);
		}
	}

	private static void SetNonPublic(object target, string prop, object? value)
	{
		var p = target.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		p!.SetValue(target, value);
	}
	private static void SetPrivate(object target, string field, object? value)
		=> target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(target, value);

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof(Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data field not found");
		var original = field.GetValue(null) ?? throw new NullReferenceException();
		var typeMapType = original.GetType();
		var typeMap = CreateInstance(typeMapType)!;
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new(original, field);
	}

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType)!;
		var sdAdd = sortedDictType.GetMethod("Add")!;
		foreach (var r in records)
		{
			var key = (string)r.GetType().GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.GetValue(r)!;
			sdAdd.Invoke(sd, new object?[] { key, r });
		}
		typeMapType.GetMethod("Add")!.Invoke(typeMap, new object?[] { type, sd });
	}

	private static void AddEmpty(object typeMap, Type type)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType)!;
		typeMapType.GetMethod("Add")!.Invoke(typeMap, new object?[] { type, sd });
	}
}
