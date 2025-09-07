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
public sealed class PlayerTests
{
	[Fact]
	public void Load_Sets_Fields_And_Formatting()
	{
		var values = new Dictionary<string, object?>
					 {
						 { nameof (Player.Id), 5 },
						 { nameof (Player.FirstName), "Ann" },
						 { nameof (Player.LastName), "O'Neil" },
						 { nameof (Player.EmailAddress), "ann@example.com" }
					 };
		using var reader = new FakeDbDataReader("Player", values);
		var p = new Player();

		p.Load(reader);

		Assert.Equal(5, p.Id);
		Assert.Equal("Ann", p.FirstName);
		Assert.Equal("O'Neil", p.LastName);
		Assert.Equal("Ann O'Neil", p.Name);
		Assert.Equal("O'Neil Ann", p.LastFirst);
	}

	[Fact]
	public void IsHuman_Depends_On_FirstName_Initial()
	{
		Assert.True(new Player { FirstName = "Alice", LastName = "L" }.IsHuman);
		Assert.False(new Player { FirstName = "1Bot", LastName = "X" }.IsHuman);
	}

	[Fact]
	public void EmailAddresses_Splits_By_Comma_And_Semicolon_And_Trims()
	{
		var p = new Player { FirstName = "Ann", LastName = "L", EmailAddress = " a@x.com ; b@y.com, c@z.com ;; " };
		var emails = p.EmailAddresses.ToArray();
		Assert.Equal(ExpectedEmails, emails);
	}

	[Fact]
	public void Games_Returns_Player_Games_Ordered_By_Date_Then_RoundNumber()
	{
		var p = new Player { Id = 20, FirstName = "P", LastName = "Q" };
		var r1 = new Round { Id = 300, Number = 1 };
		var r2 = new Round { Id = 301, Number = 2 };
		var gEarlyLaterRound = new Game { Id = 400, Round = r2, Date = new (2024, 1, 1) };
		var gLaterEarlierRound = new Game { Id = 401, Round = r1, Date = new (2024, 1, 2) };
		var gp1 = new GamePlayer { Game = gEarlyLaterRound, Player = p, Power = GamePlayer.Powers.Austria, Result = GamePlayer.Results.Unknown };
		var gp2 = new GamePlayer { Game = gLaterEarlierRound, Player = p, Power = GamePlayer.Powers.England, Result = GamePlayer.Results.Unknown };
		using (SeedCache(map =>
		{
			AddOne(map, typeof (Player), p);
			AddMany(map, typeof (GamePlayer), gp1, gp2);
			AddMany(map, typeof (Game), gEarlyLaterRound, gLaterEarlierRound);
			AddMany(map, typeof (Round), r1, r2);
			AddEmpties(map);
		}))
		{
			var games = p.Games.ToArray();
			Assert.Equal(ExpectedGameIds, games.Select(static g => g.Id).ToArray());
		}
	}

	[Fact]
	public void PlayerConflicts_Returns_Conflicts_Involving_Player()
	{
		var p = new Player { Id = 30, FirstName = "A", LastName = "B" };
		var other = new Player { Id = 31, FirstName = "C", LastName = "D" };
		// Create a conflict between player 30 and 31 – PlayerConflict constructor sorts ids
		var pc = new PlayerConflict(30, 31);
		using (SeedCache(map =>
		{
			AddMany(map, typeof (Player), p, other);
			AddOne(map, typeof (PlayerConflict), pc);
			AddEmpties(map);
		}))
		{
			var conflicts = p.PlayerConflicts;
			Assert.Single(conflicts);
			Assert.True(conflicts[0].Involves(p.Id));
		}
	}

	[Fact]
	public void Groups_Returns_From_GroupPlayers_And_GroupMemberships_Formats_Text()
	{
		var p = new Player { Id = 40, FirstName = "Ann", LastName = "Lee" };
		var g1 = new Group { Id = 501, Name = "Group One" };
		var g2 = new Group { Id = 502, Name = "Group Two" };
		var gp1 = new GroupPlayer { Player = p, Group = g1 };
		var gp2 = new GroupPlayer { Player = p, Group = g2 };
		using (SeedCache(map =>
		{
			AddOne(map, typeof (Player), p);
			AddMany(map, typeof (GroupPlayer), gp1, gp2);
			AddMany(map, typeof (Group), g1, g2);
			AddEmpties(map);
		}))
		{
			var groups = p.Groups.Select(static x => x.Id).OrderBy(static x => x).ToArray();
			Assert.Equal(ExpectedGroupIds, groups);
			var text = p.GroupMemberships;
			Assert.Contains("Ann Lee is a member of the following groups", text);
			Assert.Contains("• Group One", text);
			Assert.Contains("• Group Two", text);
		}
		// No groups case
		using (SeedCache(map =>
		{
			AddOne(map, typeof (Player), p);
			AddEmpty(map, typeof (GroupPlayer));
			AddEmpty(map, typeof (Group));
			AddEmpties(map);
		}))
		{
			var text = p.GroupMemberships;
			Assert.Equal("Ann Lee is not a member of any groups.", text);
		}
	}

	private static readonly int[] Expected = [800, 801];
	private static readonly int[] ExpectedArray = [800];
	private static readonly int[] ExpectedGameIds = [400, 401];
	private static readonly int[] ExpectedGroupIds = [501, 502];
	private static readonly string[] ExpectedEmails = ["a@x.com", "b@y.com", "c@z.com"];

	[Fact]
	public void Teams_Filters_By_Tournament()
	{
		var p = new Player { Id = 60, FirstName = "P", LastName = "Q" };
		var t1 = new Tournament { Id = 700, Name = "T1" };
		var t2 = new Tournament { Id = 701, Name = "T2" };
		var team1 = new Team { Id = 800, Name = "A", TournamentId = t1.Id };
		var team2 = new Team { Id = 801, Name = "B", TournamentId = t2.Id };
		var tp1 = NewTeamPlayerViaLoad(team1.Id, p.Id);
		var tp2 = NewTeamPlayerViaLoad(team2.Id, p.Id);
		using (SeedCache(map =>
		{
			AddOne(map, typeof (Player), p);
			AddMany(map, typeof (TeamPlayer), tp1, tp2);
			AddMany(map, typeof (Team), team1, team2);
			AddMany(map, typeof (Tournament), t1, t2);
			AddEmpties(map);
		}))
		{
			// Tournament.None => all
			var allTeams = p.Teams(Tournament.None).Select(static x => x.Id).OrderBy(static x => x).ToArray();
			Assert.Equal(Expected, allTeams);
			// Filter to t1
			var filtered = p.Teams(t1).Select(static x => x.Id).ToArray();
			Assert.Equal(ExpectedArray, filtered);
		}
	}

	[Fact]
	public void FieldValues_Formats_Strings_ForSql()
	{
		var p = new Player { FirstName = "Ann", LastName = "O'Neil", EmailAddress = "a@x.com" };
		var sql = p.FieldValues;
		Assert.Contains("[FirstName] = 'Ann'", sql);
		Assert.Contains("[LastName] = 'O''Neil'", sql);
		Assert.Contains("[EmailAddress] = 'a@x.com'", sql);
	}

	private static TeamPlayer NewTeamPlayerViaLoad(int teamId, int playerId)
	{
		var tp = new TeamPlayer();
		var values = new Dictionary<string, object?>
				{
					{ "TeamId", teamId },
					{ "PlayerId", playerId }
				};
		using var reader = new FakeDbDataReader("TeamPlayer", values);
		tp.Load(reader);
		return tp;
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

	private static void AddEmpties(object typeMap)
	{
		var typeMapType = typeMap.GetType();
		var contains = typeMapType.GetMethod("ContainsKey")
								  .OrThrow();
		AddIfMissing(typeof (Round));
		AddIfMissing(typeof (Tournament));
		AddIfMissing(typeof (ScoringSystem));
		AddIfMissing(typeof (Game));
		AddIfMissing(typeof (GamePlayer));
		AddIfMissing(typeof (TournamentPlayer));
		AddIfMissing(typeof (GroupPlayer));
		AddIfMissing(typeof (Team));
		AddIfMissing(typeof (TeamPlayer));
		AddIfMissing(typeof (PlayerConflict));

		void AddIfMissing(Type t)
		{
			var item = contains.Invoke(typeMap, [t])
							   .OrThrow();
			if (!(bool)item)
				AddEmpty(typeMap, t);
		}
	}

	private static string GetPrimaryKey(object record)
	{
		var prop = record.GetType()
						 .GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						 .OrThrow($"PrimaryKey property not found on {record.GetType().Name}");
		return prop.GetValue(record) as string ?? throw new NullReferenceException();
	}
}
