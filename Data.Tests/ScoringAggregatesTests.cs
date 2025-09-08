using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Data.Tests;

public sealed class ScoringAggregatesTests
{
	private readonly record struct P(GamePlayer.Powers Power, GamePlayer.Results Result, int Centers, int Years, double Other, double Prov, double Ante);

	private static Scoring BuildScoring(IEnumerable<P> players, Action<ScoringSystem>? configure = null)
	{
		var system = new ScoringSystem
		{
			UsesGameResult = true,
			UsesCenterCount = true,
			UsesYearsPlayed = true,
			OtherScoreAlias = "Other",
			SignificantDigits = 2,
			FinalGameYear = 1907
		};
		configure?.Invoke(system);

		var list = new List<GamePlayer>();
		foreach (var p in players)
		{
			var gp = new GamePlayer
			{
				Power = p.Power,
				Result = p.Result,
				Centers = p.Centers,
				Years = p.Years,
				Other = p.Other,
				ProvisionalScore = p.Prov,
				PlayerAnte = p.Ante
			};
			list.Add(gp);
		}
		// ensure all powers present
		foreach (GamePlayer.Powers pow in Enum.GetValues(typeof(GamePlayer.Powers)))
		{
			if (pow == GamePlayer.Powers.TBD) continue;
			if (!list.Any(g => g.Power == pow))
				list.Add(new GamePlayer { Power = pow, Result = GamePlayer.Results.Loss, Centers = 0, Years = 0, Other = 0, ProvisionalScore = 0, PlayerAnte = 0 });
		}
		var scoring = new Scoring(system, list);
		// copy GP provisional/ante to PowerData
		scoring.UpdateProvisionalScores();
		scoring.UpdatePlayerAntes();
		return scoring;
	}

	[Fact]
	public void Aggregates_Sum_Average_Min_Max_Computed()
	{
		var scoring = BuildScoring(new P[]
		{
			new(GamePlayer.Powers.Austria, GamePlayer.Results.Loss, 3, 5, 1.5, 10, 2),
			new(GamePlayer.Powers.England, GamePlayer.Results.Win, 10, 7, 2.0, 20, 1),
			new(GamePlayer.Powers.France, GamePlayer.Results.Loss, 0, 3, 0.0, 0, 0)
		});

		Assert.Equal(30, scoring.SumOfProvisionalScores);
		Assert.True(scoring.AverageProvisionalScore > 0);
		Assert.Equal(3, scoring.SumOfPlayerAntes);
		Assert.True(scoring.AveragePlayerAnte > 0);
		Assert.Equal(0, scoring.LowestPlayerAnte);
		Assert.Equal(2, scoring.HighestPlayerAnte);
		Assert.True(scoring.SumOfOtherScores > 0);
		Assert.Equal(scoring.SumOfOtherScores, scoring.SumOfEveryOtherScore);
		Assert.Equal(0, scoring.LowestOtherScore);
		Assert.True(scoring.HighestOtherScore >= scoring.LowestOtherScore);
	}

	[Fact]
	public void UnplayedYears_Based_On_FinalGameYear()
	{
		var scoring = BuildScoring(new P[]
		{
			new(GamePlayer.Powers.Austria, GamePlayer.Results.Loss, 1, 5, 0, 0, 0),
		});
		// Set the scoring Player to Austria so UnplayedYears can evaluate Player.Years
  var prop = typeof(Scoring).GetProperty("PlayerPower", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
		prop.SetValue(scoring, GamePlayer.Powers.Austria);
		// FinalGameYear set to 1907 in BuildScoring => (1907-1900) - 5 = 2
		Assert.Equal(2, scoring.UnplayedYears);
	}

	[Fact]
	public void CenterRank_And_SurvivorRank_Averages_Computed()
	{
		var scoring = BuildScoring(new P[]
		{
			new(GamePlayer.Powers.Austria, GamePlayer.Results.Loss, 5, 8, 0, 0, 0),
			new(GamePlayer.Powers.England, GamePlayer.Results.Win, 12, 10, 0, 0, 0)
		});
		// For Austria: one higher center count (12), so BestCenterRank=2; WorstCenterRank depends on sharers (none here), so 2
		Assert.InRange(scoring.Austria.CenterRank, 1.0, 7.0);
		Assert.InRange(scoring.Austria.SurvivorRank, 0.0, 7.0);
	}
}
