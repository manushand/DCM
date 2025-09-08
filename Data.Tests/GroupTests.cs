using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;
using static System.Activator;

namespace Data.Tests;

using DCM;
using Helpers;

[PublicAPI]
public sealed class GroupTests
{
	[Fact]
	public void Load_Sets_Fields()
	{
		var values = new Dictionary<string, object?>
					 {
						 { nameof (Group.Id), 3 },
						 { nameof (Group.Name), "League" },
						 { nameof (Group.Description), "Desc" },
						 { nameof (Group.Conflict), 7 }
					 };
		using var reader = new FakeDbDataReader("Group", values);
		var g = new Group();

		g.Load(reader);

		Assert.Equal(3, g.Id);
		Assert.Equal("League", g.Name);
		Assert.Equal("Desc", g.Description);
		Assert.Equal(7, g.Conflict);
	}

	private static readonly int[] Expected = [10, 11];
	private static readonly int[] ExpectedArray = [1002, 1001];

	[Fact]
	public void Players_Returns_From_GroupPlayers()
	{
		var group = new Group { Id = 1, Name = "G" };
		var p1 = new Player { Id = 10, Name = "Ann" };
		var p2 = new Player { Id = 11, Name = "Bob" };
		var gp1 = new GroupPlayer { Player = p1 };
		SetNonPublicProperty(gp1, "GroupId", group.Id);
		var gp2 = new GroupPlayer { Player = p2 };
		SetNonPublicProperty(gp2, "GroupId", group.Id);

		using (SeedCache(map =>
						{
							AddOne(map, typeof (Group), group);
							AddMany(map, typeof (GroupPlayer), gp1, gp2);
							AddMany(map, typeof (Player), p1, p2);
							// Empty maps to prevent accidental reads
							AddEmpty(map, typeof (Round));
							AddEmpty(map, typeof (Tournament));
							AddEmpty(map, typeof (Game));
						}))
			Assert.Equal(Expected, group.Players.Select(static p => p.Id).OrderBy(static x => x).ToArray());
	}

	[Fact]
	public void HostRound_Resolves_By_Tournament_GroupId_And_Exposes_ScoringSystemId()
	{
		var group = new Group { Id = 5, Name = "Group" };
		var t = new Tournament { Id = 50, Name = "T" };
		SetNonPublicProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 500, Number = 1 };
		SetNonPublicProperty(r, "TournamentId", t.Id);
		// Ensure Round.Tournament points to our tournament without DB/cache fallback
		SetPrivateField(r, "<Tournament>k__BackingField", t);
		// Give the round its own scoring system id
		SetPrivateField(r, "_scoringSystemId", 9);
		// Also attach as HostRound to avoid accessing Round.None.Tournament
		SetPrivateField(group, "<HostRound>k__BackingField", r);

		using (SeedCache(map =>
						{
							AddOne(map, typeof (Group), group);
							AddOne(map, typeof (Tournament), t);
							AddOne(map, typeof (Round), r);
							AddEmpty(map, typeof (Game));
							AddEmpty(map, typeof (ScoringSystem));
							AddEmpty(map, typeof (Player));
							AddEmpty(map, typeof (GroupPlayer));
						}))
		{
			// HostRound should resolve to r via Round.Tournament.GroupId == group.Id
			Assert.Equal(r.Id, group.HostRound.Id);
			Assert.Equal(9, group.ScoringSystemId);
		}
	}

	[Fact]
	public void Games_Are_Ordered_By_Date_And_FinishedGames_Filter()
	{
		var group = new Group { Id = 6, Name = "G" };
		var t = new Tournament { Id = 60, Name = "T" };
		SetNonPublicProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 600, Number = 1 };
		SetNonPublicProperty(r, "TournamentId", t.Id);
		SetPrivateField(r, "<Tournament>k__BackingField", t);
		// Attach HostRound directly to avoid extra cache lookup work
		SetPrivateField(group, "<HostRound>k__BackingField", r);
		var g1 = new Game { Id = 1001, Round = r, Status = Game.Statuses.Finished, Date = new (2024, 1, 2) };
		var g2 = new Game { Id = 1002, Round = r, Status = Game.Statuses.Underway, Date = new (2024, 1, 1) };

		using (SeedCache(map =>
						 {
							 AddOne(map, typeof (Group), group);
							 AddOne(map, typeof (Tournament), t);
							 AddOne(map, typeof (Round), r);
							 AddMany(map, typeof (Game), g1, g2);
							 AddEmpty(map, typeof (GroupPlayer));
							 AddEmpty(map, typeof (Player));
							 AddEmpty(map, typeof (ScoringSystem));
						 }))
		{
			var games = group.Games;
			Assert.Equal(ExpectedArray, games.Select(static g => g.Id).ToArray()); // ordered by Date ascending
			Assert.Single(group.FinishedGames);
			Assert.Equal(1001, group.FinishedGames[0].Id);
		}
	}

	[Fact]
	public void IsRatable_Respects_Modes()
	{
		var group = new Group { Id = 7, Name = "G" };
		var t = new Tournament { Id = 70, Name = "T" };
		SetNonPublicProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 700, Number = 1 };
		SetNonPublicProperty(r, "TournamentId", t.Id);
		SetPrivateField(r, "<Tournament>k__BackingField", t);
		SetPrivateField(r, "_scoringSystemId", 9);
		SetPrivateField(group, "<HostRound>k__BackingField", r);
		var g = new Game { Id = 2000, Round = r, Status = Game.Statuses.Finished, ScoringSystem = new () { Id = 9 } };

		using (SeedCache(map =>
						 {
							 AddOne(map, typeof (Group), group);
							 AddOne(map, typeof (Tournament), t);
							 AddOne(map, typeof (Round), r);
							 AddOne(map, typeof (Game), g);
							 AddEmpty(map, typeof (ScoringSystem));
						 }))
		{
			Assert.True(group.IsRatable(g, Group.GamesToRate.GroupGamesOnly));
			Assert.True(group.IsRatable(g, Group.GamesToRate.GamesUsingGroupSystem));
			Assert.True(group.IsRatable(g, Group.GamesToRate.AllGamesScoreableWithGroupSystem));

			g.Status = Game.Statuses.Underway;
			Assert.False(group.IsRatable(g, Group.GamesToRate.AllGamesScoreableWithGroupSystem));
		}
	}

	[Fact]
	public void RatePlayer_Returns_Null_When_No_Qualifying_Games()
	{
		var group = new Group { Id = 8, Name = "G" };
		var t = new Tournament { Id = 80, Name = "T" };
		SetNonPublicProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 800, Number = 1 };
		SetNonPublicProperty(r, "TournamentId", t.Id);
		SetPrivateField(r, "<Tournament>k__BackingField", t);
		SetPrivateField(r, "_scoringSystemId", 9);
		var sc = new ScoringSystem { Id = 9, PointsPerGame = 0, PlayerAnteFormula = string.Empty };
		var player = new Player { Id = 111, Name = "P" };

		// Attach HostRound directly to avoid dereferencing Round.None.Tournament
		SetPrivateField(group, "<HostRound>k__BackingField", r);
		using (SeedCache(map =>
						{
							AddOne(map, typeof (Group), group);
							AddOne(map, typeof (Tournament), t);
							AddOne(map, typeof (Round), r);
							AddOne(map, typeof (ScoringSystem), sc);
							AddEmpty(map, typeof (Game));
							AddEmpty(map, typeof (GamePlayer));
							AddOne(map, typeof (Player), player);
						}))
			Assert.Null(group.RatePlayer(player));
	}

	[Fact]
	public void RatePlayer_Sums_FinalScores_When_PointsPerGame_Zero()
	{
		var group = new Group { Id = 9, Name = "G" };
		var t = new Tournament { Id = 90, Name = "T" };
		SetNonPublicProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 900, Number = 1 };
		SetNonPublicProperty(r, "TournamentId", t.Id);
		SetPrivateField(r, "<Tournament>k__BackingField", t);
		SetPrivateField(r, "_scoringSystemId", 9);
		var sc = new ScoringSystem { Id = 9, PointsPerGame = 0, PlayerAnteFormula = string.Empty };
		var player = new Player { Id = 222, Name = "P" };
		var g1 = new Game { Id = 3001, Round = r, Status = Game.Statuses.Finished, Scored = true };
		var g2 = new Game { Id = 3002, Round = r, Status = Game.Statuses.Finished, Scored = true };

		// Build 7 players for each game including the target player; assign FinalScore via private field to avoid scoring
		var powers = new[] { GamePlayer.Powers.Austria, GamePlayer.Powers.England, GamePlayer.Powers.France, GamePlayer.Powers.Germany, GamePlayer.Powers.Italy, GamePlayer.Powers.Russia, GamePlayer.Powers.Turkey };
		var gamePlayers = new List<GamePlayer>();
		for (var i = 0; i < 7; i++)
		{
			var p = i is 0 ? player : new () { Id = 1000 + i, Name = $"P{i}" };
			var gp = new GamePlayer { Game = g1, Player = p, Power = powers[i], Result = GamePlayer.Results.Unknown };
			if (i is 0)
				SetPrivateField(gp, "_finalScore", 3.0);
			gamePlayers.Add(gp);
		}
		for (var i = 0; i < 7; i++)
		{
			var p = i is 0 ? player : new () { Id = 2000 + i, Name = $"Q{i}" };
			var gp = new GamePlayer { Game = g2, Player = p, Power = powers[i], Result = GamePlayer.Results.Unknown };
			if (i is 0)
				SetPrivateField(gp, "_finalScore", 5.0);
			gamePlayers.Add(gp);
		}

		// Attach HostRound directly
		SetPrivateField(group, "<HostRound>k__BackingField", r);
		using (SeedCache(map =>
						{
							AddOne(map, typeof (Group), group);
							AddOne(map, typeof (Tournament), t);
							AddOne(map, typeof (Round), r);
							AddOne(map, typeof (ScoringSystem), sc);
							AddMany(map, typeof (Game), g1, g2);
							AddMany(map, typeof (GamePlayer), gamePlayers.ToArray<object>());
							AddOne(map, typeof (Player), player);
						}))
		{
			var rating = group.RatePlayer(player);
			Assert.NotNull(rating);
			Assert.Equal(8.0, rating.Rating);
			Assert.Equal(2, rating.Games);
			Assert.Equal(player, rating.Player);
		}
	}

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", BindingFlags.NonPublic)
									 .OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static)
							 .OrThrow("Cache._data field not found");
		var original = field.GetValue(null)
							.OrThrow();
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

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		var sdAdd = sortedDictType.GetMethod("Add");
		foreach (var r in records)
			sdAdd?.Invoke(sd, [GetPrimaryKey(r), r]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	private static void AddEmpty(object typeMap, Type type)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	private static string GetPrimaryKey(object record)
		=> (record.GetType()
				  .GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				  .OrThrow($"PrimaryKey property not found on {record.GetType().Name}")
				  .GetValue(record) as string)
			.OrThrow();

	private static void SetNonPublicProperty(object target, string propertyName, object? value)
	{
		var prop = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
		if (prop?.SetMethod is null)
			throw new InvalidOperationException($"Property {propertyName} not found or has no setter");
		prop.SetValue(target, value);
	}

	private static void SetPrivateField(object target, string fieldName, object? value)
	{
		var field = target.GetType()
						  .GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
						  .OrThrow($"Field {fieldName} not found");
		field.SetValue(target, value);
	}
}
