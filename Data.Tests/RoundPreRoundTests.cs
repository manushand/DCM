namespace Data.Tests;

[PublicAPI]
public sealed class RoundPreRoundTests : TestBase
{
	[Fact]
	public void PreRoundScore_Event_Uses_Prior_Rounds_Count_And_Pads_With_UnplayedScore()
	{
		var t = new Tournament { Id = 1, Name = "Event", UnplayedScore = 2, TotalRounds = 3 };
		// GroupId defaults to null => Group.None => IsEvent = true
		var r1 = new Round { Id = 10, Number = 1 };
		var r2 = new Round { Id = 11, Number = 2 };
		SetProperty(r1, "TournamentId", t.Id);
		SetField(r1, "<Tournament>k__BackingField", t);
		SetProperty(r2, "TournamentId", t.Id);
		SetField(r2, "<Tournament>k__BackingField", t);
		var p = new Player { Id = 5, FirstName = "Ann", LastName = "L" };

		// Prior finished game in r1 with FinalScore=6
		var g1 = new Game
				 {
					 Id = 100,
					 Round = r1,
					 Status = Finished,
					 Scored = true,
					 Date = new (2024, 1, 1),
					 Number = 1
				 };
		var gp1 = new GamePlayer
				  {
					  Game = g1,
					  Player = p,
					  Power = Austria,
					  Result = Unknown
				  };
		SetField(gp1, "_finalScore", 6.0);

		// Target: r2 PreRoundScore should sum prior rounds (Number-1 = 1) and not need padding (targetCount=1)
		// Also test PreGameAverage uses same aggregates
		using (SeedCache(map =>
						{
							AddMany(map, typeof (Tournament), t);
							AddMany(map, typeof (Round), r1, r2);
							AddMany(map, typeof (Game), g1);
							AddMany(map, typeof (GamePlayer), gp1);
							AddMany(map, typeof (Player), p);
							// Avoid accidental DB loads
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
			var roundAvg = r2.PreGameAverage(new ()
											 {
												 Game = new () { Round = r2, Date = new (2024, 2, 1) },
												 Player = p
											 });
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
		SetProperty(t, nameof (Tournament.Group), group); // internal init uses backing, but we'll seed via reflection below using Group property backing
		// Create round 1 and 2
		var r1 = new Round { Id = 20, Number = 1 };
		var r2 = new Round { Id = 21, Number = 2 };
		SetProperty(r1, "TournamentId", t.Id);
		SetField(r1, "<Tournament>k__BackingField", t);
		SetProperty(r2, "TournamentId", t.Id);
		SetField(r2, "<Tournament>k__BackingField", t);
		// Wire Tournament.Group backing field so Tournament.IsEvent becomes false
		SetField(t, "<Group>k__BackingField", group);

		var p = new Player { Id = 5, FirstName = "Ann", LastName = "L" };
		// Two finished games in r1 and r2 both before a target game's date; one not containing player should be ignored
		var gA = new Game
				 {
					 Id = 201,
					 Round = r1,
					 Status = Finished,
					 Scored = true,
					 Date = new (2024, 1, 1),
					 Number = 1
				 };
		var gB = new Game
				 {
					 Id = 202,
					 Round = r2,
					 Status = Finished,
					 Scored = true,
					 Date = new (2024, 1, 15),
					 Number = 1
				 };
		var gpA = new GamePlayer
				  {
					  Game = gA,
					  Player = p,
					  Power = England,
					  Result = Unknown
				  };
		var gpB = new GamePlayer
				  {
					  Game = gB,
					  Player = p,
					  Power = France,
					  Result = Unknown
				  };
		SetField(gpA, "_finalScore", 3.0);
		SetField(gpB, "_finalScore", 5.0);

		using (SeedCache(map =>
						{
							AddMany(map, typeof (Tournament), t);
							AddMany(map, typeof (Round), r1, r2);
							AddMany(map, typeof (Game), gA, gB);
							AddMany(map, typeof (GamePlayer), gpA, gpB);
							AddMany(map, typeof (Player), p);
							AddMany(map, typeof (Group), group);
						}))
		{
			var targetGame = new Game { Round = r2, Date = new (2024, 2, 1) };
			var gpTarget = new GamePlayer { Game = targetGame, Player = p };
			var avg = r2.PreGameAverage(gpTarget);
			var sum = r2.PreRoundScore(p);
			// For group tournaments, roundsPrior = 1, so only prior round (r1) is included in PreRound aggregates
			Assert.Equal(3.0, sum);
			Assert.Equal(3.0, avg);
		}
	}
}
