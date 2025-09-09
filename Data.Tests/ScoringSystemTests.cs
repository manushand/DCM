using System.Collections.Generic;
using static System.String;

namespace Data.Tests;

using static ScoringSystem.DrawRules;

[PublicAPI]
public sealed class ScoringSystemTests
{
	[Fact]
	public void Flags_Computed_From_Properties()
	{
		var s = new ScoringSystem
				{
					ProvisionalScoreFormula = "x",
					PlayerAnteFormula = "y",
					OtherScoreAlias = "Other"
				};

		Assert.True(s.UsesProvisionalScore);
		Assert.True(s.UsesPlayerAnte);
		Assert.True(s.UsesOtherScore);

		s.ProvisionalScoreFormula = Empty;
		s.PlayerAnteFormula = Empty;
		s.OtherScoreAlias = Empty;

		Assert.False(s.UsesProvisionalScore);
		Assert.False(s.UsesPlayerAnte);
		Assert.False(s.UsesOtherScore);
	}

	[Fact]
	public void Draw_Rules_Flags()
	{
		var s = new ScoringSystem { DrawPermissions = None };
		Assert.False(s.DrawsAllowed);
		Assert.False(s.DrawsIncludeAllSurvivors);

		s.DrawPermissions = All;
		Assert.True(s.DrawsAllowed);
		Assert.False(s.DrawsIncludeAllSurvivors);

		s.DrawPermissions = DIAS;
		Assert.True(s.DrawsAllowed);
		Assert.True(s.DrawsIncludeAllSurvivors);
	}

	[Fact]
	public void FormattedScore_Respects_SignificantDigits_And_Trim()
	{
		var s = new ScoringSystem { SignificantDigits = 2 };
		Assert.Equal("1.23", s.FormattedScore(1.2345));
		Assert.Equal("1.20", s.FormattedScore(1.20, true));
	}

	[Fact]
	public void SetCompiledFormulae_Sets_Compiled_Suffix()
	{
		var s = new ScoringSystem { FinalScoreFormula = "42" };
		Assert.False(s.UsesCompiledFormulas);
		s.SetCompiledFormulae(true);
		Assert.True(s.UsesCompiledFormulas);
	}

	[Fact]
	public void Load_Sets_TestGamePlayers_From_TestGameData()
	{
		const string testData = "Austria,Win,18,10,0|" +
								"England,Loss,0,4,0|" +
								"France,Loss,0,3,0|" +
								"Germany,Loss,0,3,0|" +
								"Italy,Loss,0,2,0|" +
								"Russia,Loss,0,3,0|" +
								"Turkey,Loss,0,2,0";
		var values = new Dictionary<string, object?>
					 {
						 { nameof (ScoringSystem.Id), 5 },
						 { nameof (ScoringSystem.Name), "Test System" },
						 { nameof (ScoringSystem.DrawPermissions), All.AsInteger },
						 { nameof (ScoringSystem.UsesGameResult), 1 },
						 { nameof (ScoringSystem.UsesCenterCount), 1 },
						 { nameof (ScoringSystem.UsesYearsPlayed), 1 },
						 { nameof (ScoringSystem.FinalGameYear), null },
						 { nameof (ScoringSystem.ProvisionalScoreFormula), Empty },
						 { nameof (ScoringSystem.FinalScoreFormula), "42" },
						 { nameof (ScoringSystem.SignificantDigits), 2 },
						 { "TestGameData", testData },
						 { nameof (ScoringSystem.PointsPerGame), null },
						 { nameof (ScoringSystem.OtherScoreAlias), Empty },
						 { nameof (ScoringSystem.PlayerAnteFormula), Empty }
					 };
		using var reader = new FakeDbDataReader("ScoringSystem", values);
		var s = new ScoringSystem();

		s.Load(reader);

		Assert.NotNull(s.TestGamePlayers);
		Assert.Equal(7, s.TestGamePlayers.Count);
		Assert.Equal(2, s.SignificantDigits);
		Assert.True(s.DrawsAllowed);
	}

	[Fact]
	public void FinalScoreFormulaMissing_When_Empty()
	{
		var s = new ScoringSystem { FinalScoreFormula = Empty };
		Assert.True(s.FinalScoreFormulaMissing);
	}

	[Fact]
	public void ScoreWithResults_Succeeds_With_Constant_Formula()
	{
		var s = new ScoringSystem
				{
					FinalScoreFormula = "1",
					SignificantDigits = 0,
					UsesGameResult = false,
					UsesCenterCount = false,
					UsesYearsPlayed = false,
					PointsPerGame = null
				};
		var players = new List<GamePlayer>
					{
						new () { Power = Austria, Result = Unknown },
						new () { Power = England, Result = Unknown },
						new () { Power = France, Result = Unknown },
						new () { Power = Germany, Result = Unknown },
						new () { Power = Italy, Result = Unknown },
						new () { Power = Russia, Result = Unknown },
						new () { Power = Turkey, Result = Unknown }
					};

		var ok = s.ScoreWithResults(players, out var results);

		Assert.True(ok);
		Assert.All(players, static p => Assert.Equal(1, p.FinalScore));
		Assert.Contains(results, static r => r?.StartsWith("Final score for Austria") is true);
	}

	[Fact]
	public void ScoreWithResults_Fails_When_Not_All_Powers_Assigned()
	{
		var s = new ScoringSystem
				{
					FinalScoreFormula = "1",
					SignificantDigits = 0
				};
		var players = new List<GamePlayer>
					{
						new () { Power = Austria, Result = Unknown },
						new () { Power = England, Result = Unknown },
						new () { Power = France, Result = Unknown },
						new () { Power = Germany, Result = Unknown },
						new () { Power = Italy, Result = Unknown },
						new () { Power = Russia, Result = Unknown }
					};

		var ok = s.ScoreWithResults(players, out var issues);

		Assert.False(ok);
		Assert.Contains("All powers must be assigned.", issues);
	}

	[Fact]
	public void ScoreFormat_Reflects_SignificantDigits()
	{
		var s = new ScoringSystem { SignificantDigits = 3 };
		Assert.Equal("F3", s.ScoreFormat);
	}

	[Fact]
	public void FinalScoreFormula_Get_Strips_Compiled_Suffix()
	{
		var s = new ScoringSystem { FinalScoreFormula = "X" };
		s.SetCompiledFormulae(true);
		Assert.True(s.UsesCompiledFormulas);
		Assert.Equal("X", s.FinalScoreFormula);
	}

	[Fact]
	public void ScoreWithResults_Includes_Timing_Line_When_ShowTimingData_Enabled()
	{
		ScoringSystem.ShowTimingData = true;
		try
		{
			var s = new ScoringSystem
					{
						FinalScoreFormula = "1",
						SignificantDigits = 0,
						PointsPerGame = null
					};
			var players = new List<GamePlayer>
					{
						new () { Power = Austria, Result = Unknown },
						new () { Power = England, Result = Unknown },
						new () { Power = France, Result = Unknown },
						new () { Power = Germany, Result = Unknown },
						new () { Power = Italy, Result = Unknown },
						new () { Power = Russia, Result = Unknown },
						new () { Power = Turkey, Result = Unknown }
					};

			var ok = s.ScoreWithResults(players, out var results);

			Assert.True(ok);
			Assert.Contains(results, static r => r?.StartsWith("Time to score:") is true);
		}
		finally
		{
			ScoringSystem.ShowTimingData = false;
		}
	}

	[Fact]
	public void ScoreWithResults_Computes_PlayerAnte_And_Subtracts_From_FinalScore_For_GroupGames()
	{
		var s = new ScoringSystem
				{
					FinalScoreFormula = "5",
					PlayerAnteFormula = "2",
					SignificantDigits = 0,
					PointsPerGame = null
				};
		var players = new List<GamePlayer>
				{
					new () { Power = Austria, Result = Unknown },
					new () { Power = England, Result = Unknown },
					new () { Power = France, Result = Unknown },
					new () { Power = Germany, Result = Unknown },
					new () { Power = Italy, Result = Unknown },
					new () { Power = Russia, Result = Unknown },
					new () { Power = Turkey, Result = Unknown }
				};

		var ok = s.ScoreWithResults(players, out var results);

		Assert.True(ok);
		// Each player's ante should be 2 and final score 5 - 2 = 3
		Assert.All(players, static p => Assert.Equal(2, p.PlayerAnte));
		Assert.All(players, static p => Assert.Equal(3, p.FinalScore));
		Assert.Contains(results, static r => r?.StartsWith("Player Ante for Austria") is true);
	}

	[Fact]
	public void ScoreWithResults_Computes_ProvisionalScores_When_Configured()
	{
		var s = new ScoringSystem
				{
					ProvisionalScoreFormula = "10",
					FinalScoreFormula = "3",
					SignificantDigits = 0,
					PointsPerGame = null
				};
		var players = new List<GamePlayer>
				{
					new () { Power = Austria, Result = Unknown },
					new () { Power = England, Result = Unknown },
					new () { Power = France, Result = Unknown },
					new () { Power = Germany, Result = Unknown },
					new () { Power = Italy, Result = Unknown },
					new () { Power = Russia, Result = Unknown },
					new () { Power = Turkey, Result = Unknown }
				};

		var ok = s.ScoreWithResults(players, out var results);

		Assert.True(ok);
		Assert.All(players, static p => Assert.Equal(10, p.ProvisionalScore));
		Assert.All(players, static p => Assert.Equal(3, p.FinalScore));
		Assert.Contains(results, static r => r?.StartsWith("Provisional score for Austria") is true);
	}
}
