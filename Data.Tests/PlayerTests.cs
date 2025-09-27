using System.Collections.Generic;
using System.Linq;

namespace Data.Tests;

[PublicAPI]
public sealed class PlayerTests : TestBase
{
	private static readonly int[] Expected = [800, 801],
								  ExpectedArray = [800],
								  ExpectedGameIds = [400, 401],
								  ExpectedGroupIds = [501, 502];
	private static readonly string[] ExpectedEmails = ["a@x.com", "b@y.com", "c@z.com"];

	[Fact]
	public void Load_Sets_Fields_And_Formatting()
	{
		var values = new Dictionary<string, object?>
					 {
						 [nameof (Player.Id)] = 5,
						 [nameof (Player.FirstName)] = "Ann",
						 [nameof (Player.LastName)] = "O'Neil",
						 [nameof (Player.EmailAddress)] = "ann@example.com"
					 };
		using var reader = new FakeDbDataReader(nameof (Player), values);
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
		var p = new Player
				{
					FirstName = "Ann",
					LastName = "L",
					EmailAddress = " a@x.com ; b@y.com, c@z.com ;; "
				};
		var emails = p.EmailAddresses.ToArray();
		Assert.Equal(ExpectedEmails, emails);
	}

	[Fact]
	public void Games_Returns_Player_Games_Ordered_By_Date_Then_RoundNumber()
	{
		var p = new Player { Id = 20, FirstName = "P", LastName = "Q" };
		Round r1 = new () { Id = 300, Number = 1 },
			  r2 = new () { Id = 301, Number = 2 };
		Game gEarlyLaterRound = new () { Id = 400, Round = r2, Date = new (2024, 1, 1) },
			 gLaterEarlierRound = new () { Id = 401, Round = r1, Date = new (2024, 1, 2) };
		GamePlayer gp1 = new () { Game = gEarlyLaterRound, Player = p, Power = Austria, Result = Unknown },
				   gp2 = new () { Game = gLaterEarlierRound, Player = p, Power = England, Result = Unknown };
		using (SeedCache(map =>
						{
							Add(map, p);
							Add(map, gp1, gp2);
							Add(map, gEarlyLaterRound, gLaterEarlierRound);
							Add(map, r1, r2);
							AddEmpties(map);
						}))
			Assert.Equal(ExpectedGameIds, p.Games.Select(static g => g.Id));
	}

	[Fact]
	public void PlayerConflicts_Returns_Conflicts_Involving_Player()
	{
		Player p1 = new () { Id = 30, FirstName = "A", LastName = "B" },
			   p2 = new () { Id = 31, FirstName = "C", LastName = "D" };
		// Create a conflict between player 30 and 31 – PlayerConflict constructor sorts ids
		var pc = new PlayerConflict(30, 31);
		using (SeedCache(map =>
						{
							Add(map, p1, p2);
							Add(map, pc);
							AddEmpties(map);
						}))
		{
			var conflicts = p1.PlayerConflicts;
			Assert.Single(conflicts);
			Assert.True(conflicts[0].Involves(p1.Id));
		}
	}

	[Fact]
	public void Groups_Returns_From_GroupPlayers_And_GroupMemberships_Formats_Text()
	{
		var p = new Player { Id = 40, FirstName = "Ann", LastName = "Lee" };
		Group g1 = new () { Id = 501, Name = "Group One" },
			  g2 = new () { Id = 502, Name = "Group Two" };
		GroupPlayer gp1 = new () { Player = p, Group = g1 },
					gp2 = new () { Player = p, Group = g2 };
		using (SeedCache(map =>
						{
							Add(map, p);
							Add(map, gp1, gp2);
							Add(map, g1, g2);
							AddEmpties(map);
						}))
		{
			Assert.Equal(ExpectedGroupIds, p.Groups.Select(static x => x.Id).OrderBy(static x => x).ToArray());
			var text = p.GroupMemberships;
			Assert.Contains("Ann Lee is a member of the following groups", text);
			Assert.Contains("• Group One", text);
			Assert.Contains("• Group Two", text);
		}
		// No groups case
		using (SeedCache(map =>
						{
							Add(map, p);
							Add<GroupPlayer>(map);
							Add<Group>(map);
							AddEmpties(map);
						}))
			Assert.Equal("Ann Lee is not a member of any groups.", p.GroupMemberships);
	}

	[Fact]
	public void Teams_Filters_By_Tournament()
	{
		Tournament t1 = new () { Id = 700, Name = "T1" },
				   t2 = new () { Id = 701, Name = "T2" };
		Team team1 = new () { Id = 800, Name = "A", TournamentId = t1.Id },
			 team2 = new () { Id = 801, Name = "B", TournamentId = t2.Id };
		var p = new Player { Id = 60, FirstName = "P", LastName = "Q" };
		TeamPlayer tp1 = NewTeamPlayerViaLoad(team1.Id, p.Id),
				   tp2 = NewTeamPlayerViaLoad(team2.Id, p.Id);
		using (SeedCache(map =>
						{
							Add(map, p);
							Add(map, tp1, tp2);
							Add(map, team1, team2);
							Add(map, t1, t2);
							AddEmpties(map);
						}))
		{
			// Tournament.None => all
			Assert.Equal(Expected, p.Teams(Tournament.None)
									.Select(static x => x.Id)
									.OrderBy(static x => x));
			Assert.Equal(ExpectedArray, p.Teams(t1).Select(static x => x.Id));
		}
	}

	[Fact]
	public void FieldValues_Formats_Strings_ForSql()
	{
		var sql = new Player { FirstName = "Ann", LastName = "O'Neil", EmailAddress = "a@x.com" }.FieldValues;
		Assert.Contains("[FirstName] = 'Ann'", sql);
		Assert.Contains("[LastName] = 'O''Neil'", sql);
		Assert.Contains("[EmailAddress] = 'a@x.com'", sql);
	}
}
