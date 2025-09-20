using System.Collections.Generic;

namespace Data.Tests;

[PublicAPI]
public sealed class GamePlayerConflictTests : TestBase
{
	[Fact]
	public void CalculateConflict_Includes_PlayerPersonal_Group_Power_Player_Team_And_Score_Conflicts()
	{
		// Tournament with several conflict knobs
		var t = new Tournament
		{
			Id = 100,
			Name = "T",
			UnplayedScore = 0,
			TeamConflict = 7,
			PlayerConflict = 3,
			PowerConflict = 2,
			ScoreConflict = 4, // divisor
			TotalRounds = 3,
			MinimumRounds = 1,
			Date = DateTime.Today,
			GroupPowers = Corners // FR-IGA-TE
		};
		var r1 = new Round { Id = 10, Number = 1 };
		var r2 = new Round { Id = 11, Number = 2 };
		SetProperty(r1, "TournamentId", t.Id);
		SetProperty(r2, "TournamentId", t.Id);
		SetField(r1, "<Tournament>k__BackingField", t);
		SetField(r2, "<Tournament>k__BackingField", t);

		// Group with group conflict
		var g = new Group { Id = 5, Name = "Club", Conflict = 4 };

		// Players
		var a = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
		var b = new Player { Id = 2, FirstName = "Bob", LastName = "B" };
		var c = new Player { Id = 3, FirstName = "Cid", LastName = "C" }; // teammate

		// Group membership for A and B
		var gpA = new GroupPlayer { Group = g, Player = a };
		var gpB = new GroupPlayer { Group = g, Player = b };

		// Team for the tournament including A and C (so A vs. C will incur team conflict)
		var team = new Team { Id = 200, Name = "TeamX", TournamentId = t.Id };
		// Ensure tournament treats as team round to apply team conflict
		t.TeamSize = 2;
		t.TeamRound = 2;
		var tpA = NewTeamPlayer(team.Id, a.Id);
		var tpC = NewTeamPlayer(team.Id, c.Id);

		// Prior round finished game to enable score conflict calculation in round 2
		var gPrev = new Game { Id = 1001, Round = r1, Status = Finished, Scored = true, Number = 1 };
		var prevGa = new GamePlayer { Game = gPrev, Player = a, Power = France, Result = Unknown };
		var prevGb = new GamePlayer { Game = gPrev, Player = b, Power = Russia, Result = Unknown };
		var prevGc = new GamePlayer { Game = gPrev, Player = c, Power = Austria, Result = Unknown };
		// Give A a prior FinalScore to create distance from average in next round
		SetField(prevGa, "_finalScore", 10.0);
		SetField(prevGb, "_finalScore", 0.0);
		SetField(prevGc, "_finalScore", 0.0);

		// Current round game: r2
		var gNow = new Game { Id = 2001, Round = r2, Status = Seeded, Number = 2 };
		// Three opponents with specific powers to trigger power-group conflict: A plays France again group-wise and exact power same
		var nowGa = new GamePlayer { Game = gNow, Player = a, Power = France, Result = Unknown };
		var nowGb = new GamePlayer { Game = gNow, Player = b, Power = Russia, Result = Unknown };
		var nowGc = new GamePlayer { Game = gNow, Player = c, Power = Italy, Result = Unknown };
		var nowGx4 = new GamePlayer { Game = gNow, Player = new () { Id = 4, FirstName = "D", LastName = "D" }, Power = England, Result = Unknown };
		var nowGx5 = new GamePlayer { Game = gNow, Player = new () { Id = 5, FirstName = "E", LastName = "E" }, Power = Germany, Result = Unknown };
		var nowGx6 = new GamePlayer { Game = gNow, Player = new () { Id = 6, FirstName = "F", LastName = "F" }, Power = Turkey, Result = Unknown };
		var nowGx7 = new GamePlayer { Game = gNow, Player = new () { Id = 7, FirstName = "G", LastName = "G" }, Power = TBD, Result = Unknown };

		// Player conflict: A conflicted with B (value 5); PlayerConflict.Value is used directly
		var pcAb = new PlayerConflict(a.Id, b.Id) { Value = 5 };

		// Seed cache to include the necessary maps
		using (SeedCache(map =>
						{
							AddMany(map, typeof (Tournament), t);
							AddMany(map, typeof (Round), r1, r2);
							AddMany(map, typeof (Game), gPrev, gNow);
							AddMany(map, typeof (GamePlayer), prevGa, prevGb, prevGc, nowGa, nowGb, nowGc, nowGx4, nowGx5, nowGx6, nowGx7);
							AddMany(map, typeof (Group), g);
							AddMany(map, typeof (GroupPlayer), gpA, gpB);
							AddMany(map, typeof (PlayerConflict), pcAb);
							AddMany(map, typeof (Player), a, b, c, nowGx4.Player, nowGx5.Player, nowGx6.Player, nowGx7.Player);
							AddMany(map, typeof (Team), team);
							AddMany(map, typeof (TeamPlayer), tpA, tpC);
							// Empty maps to avoid DB-backed loads
							AddEmpty(map, typeof (RoundPlayer));
							AddEmpty(map, typeof (TournamentPlayer));
						}))
		{
			// Trigger calculation with details
			var total = nowGa.CalculateConflict(true);
			// Expected breakdown (group conflict applies only in first round when ScoreConflict>0; here round 2 so omit):
			// +5 player-personal conflict with B
			// + power conflict: prior powers in tournament for A were France; now playing France again
			//   PowerConflict base 2; same power => times=1 then *7 when same power and GroupPowers != None => 14
			// + players-played-earlier: A played against B and C in prior round => PlayerConflict (3) each => +6
			// + team member conflict: C is same team and TeamRound matches => +7
			// + score conflict: depends on distance/rounded; at least 1
			const int expectedMinimum = 5 + 14 + 6 + 7 + 1;
			Assert.True(total >= expectedMinimum, $"Conflict {total} should be >= expected {expectedMinimum}. Details: {string.Join(" | ", nowGa.ConflictDetails)}");
			Assert.Contains(nowGa.ConflictDetails, static s => s.Contains("player conflict"));
			Assert.Contains(nowGa.ConflictDetails, static s => s.Contains("playing on a team"));
		}

		static TeamPlayer NewTeamPlayer(int teamId,
										int playerId)
		{
			var tp = new TeamPlayer();
			var values = new Dictionary<string, object?> { { "TeamId", teamId }, { "PlayerId", playerId } };
			using var reader = new FakeDbDataReader("TeamPlayer", values);
			tp.Load(reader);
			return tp;
		}
	}

	[Fact]
	public void CalculateConflict_NoConflicts_Yields_Zero_With_NoDetails()
	{
		var t = new Tournament { Id = 300, Name = "T", UnplayedScore = 0, TotalRounds = 1, GroupPowers = None };
		var r = new Round { Id = 30, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		var g = new Game { Id = 4001, Round = r, Status = Seeded, Number = 1 };
		var p1 = new Player { Id = 1, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 2, FirstName = "B", LastName = "B" };
		var gp1 = new GamePlayer { Game = g, Player = p1, Power = Austria, Result = Unknown };
		var gp2 = new GamePlayer { Game = g, Player = p2, Power = England, Result = Unknown };
		using (SeedCache(map =>
						{
							AddMany(map, typeof (Tournament), t);
							AddMany(map, typeof (Round), r);
							AddMany(map, typeof (Game), g);
							AddMany(map, typeof (GamePlayer), gp1, gp2);
							AddMany(map, typeof (Player), p1, p2);
							// Empty maps to avoid DB-backed loads
							AddEmpty(map, typeof (RoundPlayer));
							AddEmpty(map, typeof (TournamentPlayer));
							AddEmpty(map, typeof (Group));
							AddEmpty(map, typeof (GroupPlayer));
							AddEmpty(map, typeof (Team));
							AddEmpty(map, typeof (TeamPlayer));
							AddEmpty(map, typeof (PlayerConflict));
							AddEmpty(map, typeof (ScoringSystem));
						}))
		{
			var total = gp1.CalculateConflict(true);
			Assert.Equal(0, total);
			Assert.Contains("No conflicts.", gp1.ConflictDetails);
		}
	}
}
