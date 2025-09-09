using System.Collections.Generic;
using System.Linq;

namespace Data.Tests;

using static GamePlayer;

public sealed class PowerDataTests
{
	private readonly record struct PlayerData(Powers Power,
											  Results Result,
											  int Centers,
											  int Years,
											  double Other);

	private static Scoring BuildScoring(IEnumerable<PlayerData> players,
										Action<ScoringSystem>? configure = null)
	{
		var system = new ScoringSystem
		{
			UsesGameResult = true,
			UsesCenterCount = true,
			UsesYearsPlayed = true,
			OtherScoreAlias = "Other"
		};
		configure?.Invoke(system);

		var list = new List<GamePlayer>();
		foreach (var (power, result, centers, years, other) in players)
			list.Add(new ()
					 {
						 Power = power,
						 Result = result,
						 Centers = centers,
						 Years = years,
						 Other = other
					 });
		// Ensure all 7 powers exist (fill any missing with neutral losses)
		for (var pow = Austria; pow <= Turkey; ++pow)
			if (list.All(gp => gp.Power != pow))
				list.Add(new ()
						 {
							 Power = pow,
							 Result = Loss,
							 Centers = 0,
							 Years = 0,
							 Other = 0
						 });
		return new (system, list);
	}

	[Fact]
	public void Constructor_Throws_When_Power_TBD()
	{
		var system = new ScoringSystem
					 {
						 UsesGameResult = true,
						 UsesCenterCount = true,
						 UsesYearsPlayed = true
					 };
		var gp = new GamePlayer
				 {
					 Power = TBD,
					 Result = Win,
					 Centers = 18,
					 Years = 10
				 };
		Assert.Throws<ArgumentException>(() => new Scoring(system, [gp]));
	}

	[Fact]
	public void Solo_Win_Flags_Work()
	{
		var scoring = BuildScoring([
									   new (Austria, Win, 18, 10, 1.0),
									   new (England, Loss, 0, 5, 0),
									   new (France, Loss, 0, 4, 0),
									   new (Germany, Loss, 0, 3, 0),
									   new (Italy, Loss, 0, 2, 0),
									   new (Russia, Loss, 0, 3, 0),
									   new (Turkey, Loss, 0, 2, 0)
								   ]);

		var pd = scoring.Austria;
		Assert.True(pd.Won);
		Assert.True(pd.WonAlone);
		Assert.True(pd.WonSolo);
		Assert.False(pd.WonConcession);
		Assert.False(pd.WonDraw);
		Assert.True(pd.WasLeader);
		Assert.Equal(18, pd.Centers);
		Assert.Equal(18 * 18, pd.CentersSquared);
		Assert.Equal(1, scoring.Winners);
	}

	[Fact]
	public void Concession_Win_Flags_Work()
	{
		// Winner has < 18 centers and is sole winner => concession
		var scoring = BuildScoring([
									   new (England, Win, 17, 10, 0),
									   new (Austria, Loss, 0, 5, 0),
									   new (France, Loss, 0, 4, 0),
									   new (Germany, Loss, 0, 3, 0),
									   new (Italy, Loss, 0, 2, 0),
									   new (Russia, Loss, 0, 3, 0),
									   new (Turkey, Loss, 0, 2, 0)
								   ]);
		var pd = scoring.England;
		Assert.True(pd.Won);
		Assert.True(pd.WonAlone);
		Assert.False(pd.WonSolo);
		Assert.True(pd.WonConcession);
		Assert.False(pd.WonDraw);
	}

	[Fact]
	public void Draw_Win_Flags_Work()
	{
		// Two winners sharing top centers => draw
		var scoring = BuildScoring([
									   new (France, Win, 12, 10, 0),
									   new (Germany, Win, 12, 10, 0),
									   new (Austria, Loss, 6, 8, 0),
									   new (England, Loss, 2, 6, 0),
									   new (Italy, Loss, 0, 5, 0),
									   new (Russia, Loss, 0, 4, 0),
									   new (Turkey, Loss, 0, 3, 0)
								   ]);
		Assert.True(scoring.France.WonDraw);
		Assert.True(scoring.Germany.WonDraw);
		Assert.Equal(2, scoring.Winners);
		Assert.True(scoring.France.WasLeader);
		Assert.True(scoring.Germany.WasLeader);
	}

	[Fact]
	public void Survivor_And_Eliminated_Flags_Work()
	{
		var scoring = BuildScoring([
									   new (Austria, Loss, 5, 8, 0),
									   new (England, Loss, 0, 5, 0),
									   new (France, Win, 18, 10, 0),
									   new (Germany, Loss, 3, 7, 0),
									   new (Italy, Loss, 0, 4, 0),
									   new (Russia, Loss, 2, 6, 0),
									   new (Turkey, Loss, 0, 3, 0)
								   ]);
		Assert.True(scoring.Austria.Survived);
		Assert.False(scoring.Austria.Eliminated);
		Assert.True(scoring.England.Eliminated);
		Assert.False(scoring.England.Survived);
	}

	[Fact]
	public void Ranks_And_Sharers_Work_With_Ties()
	{
		var scoring = BuildScoring([
									   new (Austria, Loss, 6, 8, 0),
									   new (England, Loss, 6, 8, 0),
									   new (France, Loss, 3, 7, 0),
									   new (Germany, Loss, 3, 6, 0),
									   new (Italy, Win, 8, 10, 0),
									   new (Russia, Loss, 0, 4, 0),
									   new (Turkey, Loss, 0, 3, 0)
								   ]);
		var pdA = scoring.Austria;
		// Centers 8 (winner), 6, 6, 3, 3, 0, 0
		Assert.Equal(2, pdA.BestCenterRank); // one power strictly higher (8), so best rank is 2
		Assert.Equal(3, pdA.WorstCenterRank); // include equal (other 6) gives rank 3
		Assert.Equal(2, pdA.CenterRankSharers); // two with 6
	}

	[Fact]
	public void EliminationOrder_Computes_For_Winners_Survivors_And_Eliminations()
	{
		// Years: earlier elimination smaller years; survivors have centers > 0
		var scoring = BuildScoring([
									   new (Austria, Win, 12, 10, 0), // winner
									   new (England, Loss, 4, 9, 0),  // survivor
									   new (France, Loss, 0, 5, 0),   // eliminated year 5
									   new (Germany, Loss, 0, 7, 0),  // eliminated year 7
									   new (Italy, Loss, 0, 7, 0),    // eliminated year 7 (tie)
									   new (Russia, Loss, 0, 3, 0),   // eliminated year 3
									   new (Turkey, Loss, 2, 8, 0)    // survivor
								   ]);
		var pdWinner = scoring.Austria;
		var pdSurvivor = scoring.England; // survived
		var pdElim5 = scoring.France;     // eliminated at 5
		var pdElim7 = scoring.Germany;    // eliminated at 7

		// Winner => all flavors 0
		Assert.Equal(0, pdWinner.BestEliminationOrder);
		Assert.Equal(0, pdWinner.WorstEliminationOrder);
		Assert.Equal(0, pdWinner.EliminatedEarlier);

		// Survivor => 7 - Winners (1 winner)
		Assert.Equal(6, pdSurvivor.BestEliminationOrder);
		Assert.Equal(6, pdSurvivor.WorstEliminationOrder);
		Assert.Equal(6, pdSurvivor.EliminatedEarlier);

		// Eliminated at year 5: earlier eliminated are those with years < 5 (Russia at 3)
		Assert.Equal(1 + 1, pdElim5.BestEliminationOrder); // Best = Earlier + 1
		Assert.Equal(2, pdElim5.WorstEliminationOrder);    // Worst = <= year (3 and 5 itself)
		Assert.Equal(1, pdElim5.EliminatedEarlier);

		// Eliminated at year 7: earlier eliminated are years < 7 (3 and 5) = 2;
		// worst includes those at same year (Germany & Italy) = 4
		Assert.Equal(3, pdElim7.BestEliminationOrder);
		Assert.Equal(4, pdElim7.WorstEliminationOrder);
		Assert.Equal(2, pdElim7.EliminatedEarlier);
	}

	[Fact]
	public void OtherScore_Accessible_When_Configured()
	{
		var scoring = BuildScoring([
									   new (Austria, Loss, 1, 1, 2.5),
									   new (England, Loss, 1, 1, 1.5),
									   new (France, Loss, 0, 1, 0),
									   new (Germany, Loss, 0, 1, 0),
									   new (Italy, Loss, 0, 1, 0),
									   new (Russia, Loss, 0, 1, 0),
									   new (Turkey, Loss, 0, 1, 0)
								   ],
								   static s => s.OtherScoreAlias = "Other");
		Assert.Equal(2.5, scoring.Austria.OtherScore);
		Assert.True(scoring.AverageOtherScore > 0);
	}
}
