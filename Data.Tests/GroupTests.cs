using System.Collections.Generic;
using System.Linq;

namespace Data.Tests;

[PublicAPI]
public sealed class GroupTests : TestBase
{
	private static readonly int[] Expected = [10, 11];
	private static readonly int[] ExpectedArray = [1002, 1001];

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

	[Fact]
	public void Players_Returns_From_GroupPlayers()
	{
		var group = new Group { Id = 1, Name = "G" };
		var p1 = new Player { Id = 10, Name = "Ann" };
		var p2 = new Player { Id = 11, Name = "Bob" };
		var gp1 = new GroupPlayer { Player = p1 };
		SetProperty(gp1, "GroupId", group.Id);
		var gp2 = new GroupPlayer { Player = p2 };
		SetProperty(gp2, "GroupId", group.Id);

		using (SeedCache(map =>
						{
							Add(map, group);
							Add(map, gp1, gp2);
							Add(map, p1, p2);
							// Empty maps to prevent accidental reads
							Add<Round>(map);
							Add<Tournament>(map);
							Add<Game>(map);
						}))
			Assert.Equal(Expected, group.Players.Select(static p => p.Id).OrderBy(static x => x));
	}

	[Fact]
	public void HostRound_Resolves_By_Tournament_GroupId_And_Exposes_ScoringSystemId()
	{
		var group = new Group { Id = 5, Name = "Group" };
		var t = new Tournament { Id = 50, Name = "T" };
		SetProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 500, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		// Ensure Round.Tournament points to our tournament without a DB/cache fallback
		SetField(r, "<Tournament>k__BackingField", t);
		// Give the round its own scoring system id
		SetField(r, "_scoringSystemId", 9);
		// Also attach as HostRound to avoid accessing Round.None.Tournament
		SetField(group, "<HostRound>k__BackingField", r);

		using (SeedCache(map =>
						{
							Add(map, group);
							Add(map, t);
							Add(map, r);
							Add<Game>(map);
							Add<ScoringSystem>(map);
							Add<Player>(map);
							Add<GroupPlayer>(map);
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
		SetProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 600, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		// Attach HostRound directly to avoid extra cache lookup work
		SetField(group, "<HostRound>k__BackingField", r);
		var g1 = new Game { Id = 1001, Round = r, Status = Finished, Date = new (2024, 1, 2) };
		var g2 = new Game { Id = 1002, Round = r, Status = Underway, Date = new (2024, 1, 1) };

		using (SeedCache(map =>
						 {
							 Add(map, group);
							 Add(map, t);
							 Add(map, r);
							 Add(map, g1, g2);
							 Add<GroupPlayer>(map);
							 Add<Player>(map);
							 Add<ScoringSystem>(map);
						 }))
		{
			Assert.Equal(ExpectedArray, group.Games
											 .Select(static g => g.Id)); // ordered by Date ascending
			Assert.Single(group.FinishedGames);
			Assert.Equal(1001, group.FinishedGames[0].Id);
		}
	}

	[Fact]
	public void IsRatable_Respects_Modes()
	{
		var group = new Group { Id = 7, Name = "G" };
		var t = new Tournament { Id = 70, Name = "T" };
		SetProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 700, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		SetField(r, "_scoringSystemId", 9);
		SetField(group, "<HostRound>k__BackingField", r);
		var g = new Game { Id = 2000, Round = r, Status = Finished, ScoringSystem = new () { Id = 9 } };

		using (SeedCache(map =>
						 {
							 Add(map, group);
							 Add(map, t);
							 Add(map, r);
							 Add(map, g);
							 Add<ScoringSystem>(map);
						 }))
		{
			Assert.True(group.IsRatable(g, Group.GamesToRate.GroupGamesOnly));
			Assert.True(group.IsRatable(g, Group.GamesToRate.GamesUsingGroupSystem));
			Assert.True(group.IsRatable(g, Group.GamesToRate.AllGamesScoreableWithGroupSystem));

			g.Status = Underway;
			Assert.False(group.IsRatable(g, Group.GamesToRate.AllGamesScoreableWithGroupSystem));
		}
	}

	[Fact]
	public void RatePlayer_Returns_Null_When_No_Qualifying_Games()
	{
		var group = new Group { Id = 8, Name = "G" };
		var t = new Tournament { Id = 80, Name = "T" };
		SetProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 800, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		SetField(r, "_scoringSystemId", 9);
		var sc = new ScoringSystem { Id = 9, PointsPerGame = 0, PlayerAnteFormula = string.Empty };
		var player = new Player { Id = 111, Name = "P" };

		// Attach HostRound directly to avoid dereferencing Round.None.Tournament
		SetField(group, "<HostRound>k__BackingField", r);
		using (SeedCache(map =>
						{
							Add(map, group);
							Add(map, t);
							Add(map, r);
							Add(map, sc);
							Add<Game>(map);
							Add<GamePlayer>(map);
							Add(map, player);
						}))
			Assert.Null(group.RatePlayer(player));
	}

	[Fact]
	public void RatePlayer_Sums_FinalScores_When_PointsPerGame_Zero()
	{
		var group = new Group { Id = 9, Name = "G" };
		var t = new Tournament { Id = 90, Name = "T" };
		SetProperty(t, "GroupId", group.Id);
		var r = new Round { Id = 900, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		SetField(r, "_scoringSystemId", 9);
		var sc = new ScoringSystem { Id = 9, PointsPerGame = 0, PlayerAnteFormula = string.Empty };
		var player = new Player { Id = 222, Name = "P" };
		var g1 = new Game { Id = 3001, Round = r, Status = Finished, Scored = true };
		var g2 = new Game { Id = 3002, Round = r, Status = Finished, Scored = true };

		// Build 7 players for each game including the target player; assign FinalScore via private field to avoid scoring
		var powers = new[] { Austria, England, France, Germany, Italy, Russia, Turkey };
		var gamePlayers = new List<GamePlayer>();
		for (var i = 0; i < 7; i++)
		{
			var p = i is 0
						? player
						: new () { Id = 1000 + i, Name = $"P{i}" };
			var gp = new GamePlayer { Game = g1, Player = p, Power = powers[i], Result = Unknown };
			if (i is 0)
				SetField(gp, "_finalScore", 3.0);
			gamePlayers.Add(gp);
		}
		for (var i = 0; i < 7; i++)
		{
			var p = i is 0 ? player : new () { Id = 2000 + i, Name = $"Q{i}" };
			var gp = new GamePlayer { Game = g2, Player = p, Power = powers[i], Result = Unknown };
			if (i is 0)
				SetField(gp, "_finalScore", 5.0);
			gamePlayers.Add(gp);
		}

		// Attach HostRound directly
		SetField(group, "<HostRound>k__BackingField", r);
		using (SeedCache(map =>
						{
							Add(map, group);
							Add(map, t);
							Add(map, r);
							Add(map, sc);
							Add(map, g1, g2);
							Add(map, [..gamePlayers]);
							Add(map, player);
						}))
		{
			var rating = group.RatePlayer(player);
			Assert.NotNull(rating);
			Assert.Equal(8.0, rating.Rating);
			Assert.Equal(2, rating.Games);
			Assert.Equal(player, rating.Player);
		}
	}
}
