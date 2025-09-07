using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DCM;
using JetBrains.Annotations;
using Xunit;
using static System.Activator;

namespace Data.Tests;

using Helpers;

[PublicAPI]
public sealed class TeamTests
{
	[Fact]
	public void Load_Sets_Fields()
	{
		var values = new Dictionary<string, object?>
					 {
						 { nameof (Team.Id), 12 },
						 { nameof (Team.Name), "Team Alpha" },
						 { nameof (Team.TournamentId), 34 }
					 };
		using var reader = new FakeDbDataReader("Team", values);
		var t = new Team();

		t.Load(reader);

		Assert.Equal(12, t.Id);
		Assert.Equal("Team Alpha", t.Name);
		Assert.Equal(34, t.TournamentId);
	}

	[Fact]
	public void TeamPlayers_Return_From_Cache_Filtered_By_TeamId()
	{
		var team = new Team { Id = 1, Name = "T" };
		var p1 = new Player { Id = 10, Name = "Ann" };
		var tp1 = NewTeamPlayerViaLoad(1, p1.Id);
		var tpOtherTeam = NewTeamPlayerViaLoad(2, 999);

		using (SeedCache(map =>
		{
			AddOne(map, typeof (Team), team);
			AddMany(map, typeof (TeamPlayer), tp1, tpOtherTeam);
			AddOne(map, typeof (Player), p1);
			AddEmpties(map);
		}))
		{
			var teamPlayers = team.TeamPlayers;
			Assert.Single(teamPlayers);
			Assert.Equal(p1.Id, teamPlayers[0].PlayerId);
		}
	}

	private static readonly int[] Expected = [21, 22];

	[Fact]
	public void Players_Return_From_TeamPlayers()
	{
		var team = new Team { Id = 2, Name = "T2" };
		var p1 = new Player { Id = 21, Name = "Bob" };
		var p2 = new Player { Id = 22, Name = "Cid" };
		var tp1 = NewTeamPlayerViaLoad(team.Id, p1.Id);
		var tp2 = NewTeamPlayerViaLoad(team.Id, p2.Id);

		using (SeedCache(map =>
		{
			AddOne(map, typeof (Team), team);
			AddMany(map, typeof (TeamPlayer), tp1, tp2);
			AddMany(map, typeof (Player), p1, p2);
			AddEmpties(map);
		}))
		{
			var players = team.Players.Select(static x => x.Id).OrderBy(static x => x).ToArray();
			Assert.Equal(Expected, players);
		}
	}

	[Fact]
	public void FieldValues_Formats_Name_And_TournamentId()
	{
		var t = new Team { Name = "Red's Team", TournamentId = 42 };
		var sql = t.FieldValues;
		Assert.Contains("[Name] = 'Red''s Team'", sql);
		Assert.Contains("[TournamentId] = 42", sql);
	}

	// Helpers
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

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose()
			=> Field.SetValue(null, Original);
	}

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", BindingFlags.NonPublic)
						?? throw new InvalidOperationException("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static)
					?? throw new InvalidOperationException("Cache._data field not found");
		var original = field.GetValue(null)
					   ?? throw new NullReferenceException();
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

	private static void AddEmpties(object typeMap)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var mapAdd = typeMapType.GetMethod("Add")
								.OrThrow();
		AddEmpty(typeof (Round));
		AddEmpty(typeof (Tournament));
		AddEmpty(typeof (ScoringSystem));
		AddEmpty(typeof (Game));
		AddEmpty(typeof (RoundPlayer));
		AddEmpty(typeof (TournamentPlayer));
		AddEmpty(typeof (Group));
		AddEmpty(typeof (GroupPlayer));
		// Do not add empty maps for Team, TeamPlayer, Player here because tests already add them
		AddEmpty(typeof (PlayerConflict));

		void AddEmpty(Type t)
			=> mapAdd.Invoke(typeMap, [t, CreateInstance(sortedDictType)]);
	}

	private static string GetPrimaryKey(object record)
	{
		var prop = record.GetType()
						 .GetProperty("PrimaryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						 .OrThrow($"PrimaryKey property not found on {record.GetType().Name}");
		return prop.GetValue(record) as string ?? throw new NullReferenceException();
	}
}
