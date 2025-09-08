using System;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;
using static System.Activator;

namespace Data.Tests;

using DCM;

[PublicAPI]
public sealed class GamePreGameScoreTests
{
	[Fact]
	public void PreGameScore_Event_Uses_PreRoundScore()
	{
		// Event tournament (Group is None by default)
		var t = new Tournament { Id = 1, Name = "Event", UnplayedScore = 0, TotalRounds = 3 };
		var r1 = new Round { Id = 10, Number = 1 };
		var r2 = new Round { Id = 11, Number = 2 };
		SetNonPublic(r1, "TournamentId", t.Id);
		SetPrivate(r1, "<Tournament>k__BackingField", t);
		SetNonPublic(r2, "TournamentId", t.Id);
		SetPrivate(r2, "<Tournament>k__BackingField", t);
		var p = new Player { Id = 5, FirstName = "Ann", LastName = "L" };

		// Prior finished game in r1 for player p, FinalScore = 6
		var g1 = new Game { Id = 100, Round = r1, Status = Game.Statuses.Finished, Scored = true, Date = new (2024, 1, 1), Number = 1 };
		var gp1 = new GamePlayer { Game = g1, Player = p, Power = GamePlayer.Powers.Austria, Result = GamePlayer.Results.Unknown };
		SetPrivate(gp1, "_finalScore", 6.0);

		// Current game in r2; PreGameScore should equal PreRoundScore (sum of prior rounds' finished games)
		var gNow = new Game { Id = 200, Round = r2, Status = Game.Statuses.Seeded, Date = new (2024, 2, 1), Number = 1 };
		var gpNow = new GamePlayer { Game = gNow, Player = p, Power = GamePlayer.Powers.England, Result = GamePlayer.Results.Unknown };

		using (SeedCache(map =>
						{
							AddMany(map, typeof (Tournament), t);
							AddMany(map, typeof (Round), r1, r2);
							AddMany(map, typeof (Game), g1, gNow);
							AddMany(map, typeof (GamePlayer), gp1, gpNow);
							AddMany(map, typeof (Player), p);
							AddEmpty(map, typeof (Group));
							AddEmpty(map, typeof (GroupPlayer));
							AddEmpty(map, typeof (RoundPlayer));
							AddEmpty(map, typeof (TournamentPlayer));
							AddEmpty(map, typeof (Team));
							AddEmpty(map, typeof (TeamPlayer));
							AddEmpty(map, typeof (PlayerConflict));
							AddEmpty(map, typeof (ScoringSystem));
						}))
		{
			var pre = gNow.PreGameScore(gpNow);
			Assert.Equal(6.0, pre);
		}
	}

	[Fact]
	public void PreGameScore_Group_Uses_Group_RatePlayer()
	{
		// Group tournament: Tournament.Group set => IsEvent = false
		var group = new Group { Id = 50, Name = "Club" };
		var t = new Tournament { Id = 2, Name = "GroupT", UnplayedScore = 0, TotalRounds = 3 };
		// Wire backing field so t.IsEvent becomes false
		SetPrivate(t, "<Group>k__BackingField", group);

		// Host round for group with a scoring system id
		var hostRound = new Round { Id = 60, Number = 1 };
		SetNonPublic(hostRound, "TournamentId", t.Id);
		SetPrivate(hostRound, "<Tournament>k__BackingField", t);
		SetPrivate(hostRound, "_scoringSystemId", 9); // group's ScoringSystemId
		SetPrivate(group, "<HostRound>k__BackingField", hostRound);
		var scoring = new ScoringSystem { Id = 9, PointsPerGame = 0, PlayerAnteFormula = string.Empty };

		// Prior finished group games containing player p with FinalScores 3 and 5
		var r1 = new Round { Id = 21, Number = 1 };
		var r2 = new Round { Id = 22, Number = 2 };
		SetNonPublic(r1, "TournamentId", t.Id);
		SetPrivate(r1, "<Tournament>k__BackingField", t);
		SetNonPublic(r2, "TournamentId", t.Id);
		SetPrivate(r2, "<Tournament>k__BackingField", t);
		SetPrivate(t, "<ScoringSystem>k__BackingField", scoring);
		var p = new Player { Id = 7, FirstName = "Pat", LastName = "Q" };
		var gA = new Game { Id = 300, Round = r1, Status = Game.Statuses.Finished, Scored = true, Date = new (2024, 1, 1), Number = 1 };
		var gB = new Game { Id = 301, Round = r2, Status = Game.Statuses.Finished, Scored = true, Date = new (2024, 1, 15), Number = 1 };
		// Ensure these prior games use the group's scoring system id (9) to satisfy Group.RatePlayer filter
		gA.ScoringSystem = scoring;
		gB.ScoringSystem = scoring;
		var gpA = new GamePlayer { Game = gA, Player = p, Power = GamePlayer.Powers.England, Result = GamePlayer.Results.Unknown };
		var gpB = new GamePlayer { Game = gB, Player = p, Power = GamePlayer.Powers.France, Result = GamePlayer.Results.Unknown };
		SetPrivate(gpA, "_finalScore", 3.0);
		SetPrivate(gpB, "_finalScore", 5.0);

		// Current game in r2 (later date than gA and gB)
		var gNow = new Game { Id = 400, Round = r2, Status = Game.Statuses.Seeded, Date = new (2024, 2, 1), Number = 2 };
		var gpNow = new GamePlayer { Game = gNow, Player = p };

		// Tie tournament to group so Group.IsRatable(GroupGamesOnly) matches game.Tournament.GroupId
		SetNonPublic(t, nameof (Tournament.GroupId), group.Id);

		using (SeedCache(map =>
		{
			AddMany(map, typeof (Group), group);
			AddMany(map, typeof (Tournament), t);
			AddMany(map, typeof (Round), hostRound, r1, r2);
			AddMany(map, typeof (ScoringSystem), scoring);
			AddMany(map, typeof (Game), gA, gB, gNow);
			AddMany(map, typeof (GamePlayer), gpA, gpB, gpNow);
			AddMany(map, typeof (Player), p);
			AddEmpty(map, typeof (GroupPlayer));
			AddEmpty(map, typeof (RoundPlayer));
			AddEmpty(map, typeof (TournamentPlayer));
			AddEmpty(map, typeof (Team));
			AddEmpty(map, typeof (TeamPlayer));
			AddEmpty(map, typeof (PlayerConflict));
		}))
		{
			var pre = gNow.PreGameScore(gpNow);
			// PointsPerGame == 0 => Group.RatePlayer sums (3 + 5) = 8
			Assert.Equal(8.0, pre);
		}
	}

	private static void SetNonPublic(object target, string prop, object? value)
		=> target.GetType()
				 .GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	private static void SetPrivate(object target, string field, object? value)
		=> target.GetType()
				 .GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", BindingFlags.NonPublic).OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static).OrThrow("Cache._data field not found");
		var original = field.GetValue(null).OrThrow();
		var typeMapType = original.GetType();
		var typeMap = CreateInstance(typeMapType).OrThrow();
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new (original, field);
	}

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		var sdAdd = sortedDictType.GetMethod("Add").OrThrow();
		foreach (var r in records)
		{
			var key = (string)r.GetType()
							   .GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
							   .OrThrow()
							   .GetValue(r)
							   .OrThrow();
			sdAdd.Invoke(sd, [key, r]);
		}
		typeMapType.GetMethod("Add")
				   .OrThrow()
				   .Invoke(typeMap, [type, sd]);
	}

	private static void AddEmpty(object typeMap, Type type)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		typeMapType.GetMethod("Add")
				   .OrThrow()
				   .Invoke(typeMap, [type, sd]);
	}
}
