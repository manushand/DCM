using System.Runtime.Serialization;
using JetBrains.Annotations;
using Data;
using DCM;

namespace API;

[PublicAPI]
internal sealed class System : Rest<System, ScoringSystem, System.SystemDetails>
{
	public int Id => Identity;
	public string Name => RecordedName;

	public enum FormulaLanguages : byte
	{
		DCM,
		[EnumMember(Value = "C#")]
		CSharp
	}

	[PublicAPI]
	internal sealed class SystemDetails : DetailClass
	{
		public int? PointsPerGame { get; set; }
		public int SignificantDigits { get; set; }
		public ScoringSystem.DrawRules DrawPermissions { get; set; }
		public int? FinalGameYear { get; set; }
		public bool UsesGameResult { get; set; }
		public bool UsesCenterCount { get; set; }
		public bool UsesYearsPlayed { get; set; }
		public bool UsesOtherScore { get; set; }
		public string? OtherScoreAlias { get; set; }
		public string? PlayerAnteFormula { get; set; }
		public string? ProvisionalScoreFormula { get; set; }
		required public string FinalScoreFormula { get; set; }
		required public FormulaLanguages FormulaLanguage { get; set; }
		required public IEnumerable<TestGamePlayer> TestGame { get; set; }
	}

	protected override SystemDetails Detail => new ()
											   {
												   PointsPerGame = Record.PointsPerGame,
												   SignificantDigits = Record.SignificantDigits,
												   DrawPermissions = Record.DrawPermissions,
												   FinalGameYear = Record.FinalGameYear,
												   UsesGameResult = Record.UsesGameResult,
												   UsesCenterCount = Record.UsesCenterCount,
												   UsesYearsPlayed = Record.UsesYearsPlayed,
												   UsesOtherScore = Record.UsesOtherScore,
												   OtherScoreAlias = Record.OtherScoreAlias.NullIfEmpty(),
												   FormulaLanguage = Record.UsesCompiledFormulas
																		 ? FormulaLanguages.CSharp
																		 : FormulaLanguages.DCM,
												   PlayerAnteFormula = Record.UsesPlayerAnte
																		   ? Record.PlayerAnteFormula.NullIfEmpty()
																		   : null,
												   ProvisionalScoreFormula = Record.UsesProvisionalScore
																				 ? Record.ProvisionalScoreFormula.NullIfEmpty()
																				 : null,
												   FinalScoreFormula = Record.FinalScoreFormula,
												   TestGame = TestData()
											   };

	[PublicAPI]
	public sealed class TestGamePlayer(Game.GamePlayer gamePlayer)
	{
		public GamePlayer.PowerNames Power { get; init; } = gamePlayer.Power;
		public Data.GamePlayer.Results Result { get; init; } = gamePlayer.Result;
		public int? Years { get; init; } = gamePlayer.Years;
		public int? Centers { get; init; } = gamePlayer.Centers;
		public double? Other { get; init; } = gamePlayer.Other;
		public double? PlayerAnte { get; init; } = gamePlayer.PlayerAnte;
		public double? ProvisionalScore { get; init; } = gamePlayer.ProvisionalScore;
		public double? FinalScore { get; init; } = gamePlayer.FinalScore;
	}

	private IEnumerable<TestGamePlayer> TestData()
	{
		if (Record.TestGamePlayers is null)
			return [];
		if (!Record.ScoreWithResults(Record.TestGamePlayers, out _))
			throw new ();
		return Record.TestGamePlayers.Select(gamePlayer => new TestGamePlayer(new (gamePlayer, Record)));
	}

	protected internal override string[] Update(System system)
	{
		if (system.Details is null)
			return ["No details provided."];
		//	TODO - add more validation

		Record.Name = system.Name;

		Record.PointsPerGame = system.Details.PointsPerGame;
		Record.SignificantDigits = system.Details.SignificantDigits;
		Record.DrawPermissions = system.Details.DrawPermissions;
		Record.FinalGameYear = system.Details.FinalGameYear;
		Record.UsesGameResult = system.Details.UsesGameResult;
		Record.UsesCenterCount = system.Details.UsesCenterCount;
		Record.UsesYearsPlayed = system.Details.UsesYearsPlayed;
		Record.OtherScoreAlias = system.Details.OtherScoreAlias ?? string.Empty;
		Record.PlayerAnteFormula = system.Details.PlayerAnteFormula ?? string.Empty;
		Record.ProvisionalScoreFormula = system.Details.ProvisionalScoreFormula ?? string.Empty;
		Record.FinalScoreFormula = system.Details.FinalScoreFormula;
		Record.SetCompiledFormulae(system.Details.FormulaLanguage is FormulaLanguages.CSharp);
		try
		{
			var testGame = new Data.Game { ScoringSystem = Record };
			var testData = system.Details.TestGame.Select(player => MakeGamePlayer(player, testGame)).ToList();
			if (testData.Count is not 7
			|| testData.Any(power => power.Power is GamePlayer.PowerNames.TBD
									 || power.Years < 1 || power.Years > Record.FinalGameYear - 1900
									 || power.Centers > 34)
			|| testData.Select(static power => power.Power).Distinct().Count() is not 7
			|| testData.Sum(static power => power.Centers) is < 22 or > 34)
				throw new ();
			Record.TestGamePlayers = testData;
		}
		catch
		{
			return ["Test Game data is invalid."];
		}
		return Record.ScoreWithResults(Record.TestGamePlayers, out var results)
				   ? []
				   : results.OfType<string>().ToArray();

		static GamePlayer MakeGamePlayer(dynamic updated, Data.Game testGame)
			=> new ()
			   {
				   Power = ((string)updated.Power).As<GamePlayer.PowerNames>(),
				   Result = ((string)updated.Result).As<global::Data.GamePlayer.Results>(),
				   Centers = updated.Centers,
				   Years = updated.Years,
				   Other = updated.Other is null ? default : double.Parse((string)updated.Other),
				   Game = testGame
			   };
	}
}
