using System.Collections.Generic;
using System.Linq;

namespace Data.Tests;

[PublicAPI]
public sealed class TeamTests : TestBase
{
	[Fact]
	public void Load_Sets_Fields()
	{
		var values = new Dictionary<string, object?>
					 {
						 [nameof (Team.Id)] = 12,
						 [nameof (Team.Name)] = "Team Alpha",
						 [nameof (Team.TournamentId)] = 34
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
							Add(map, team);
							Add(map, tp1, tpOtherTeam);
							Add(map, p1);
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
		Player p1 = new () { Id = 21, Name = "Bob" },
			   p2 = new () { Id = 22, Name = "Cid" };
		var tp1 = NewTeamPlayerViaLoad(team.Id, p1.Id);
		var tp2 = NewTeamPlayerViaLoad(team.Id, p2.Id);

		using (SeedCache(map =>
						{
							Add(map, team);
							Add(map, tp1, tp2);
							Add(map, p1, p2);
							AddEmpties(map);
						}))
			Assert.Equal(Expected, team.Players
									   .Select(static x => x.Id)
									   .OrderBy(static x => x));
	}

	[Fact]
	public void FieldValues_Formats_Name_And_TournamentId()
	{
		var sql = new Team { Name = "Red's Team", TournamentId = 42 }.FieldValues;
		Assert.Contains("[Name] = 'Red''s Team'", sql);
		Assert.Contains("[TournamentId] = 42", sql);
	}
}
