using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal sealed class ScoringSystem : Rest<ScoringSystem, Data.ScoringSystem>
{
	protected override dynamic Detail => new
										 {
											 Data.PointsPerGame,
											 Data.SignificantDigits,
											 Data.DrawPermissions,
											 Data.FinalGameYear,
											 Data.UsesGameResult,
											 Data.UsesCenterCount,
											 Data.UsesYearsPlayed,
											 Data.UsesOtherScore,
											 OtherScoreAlias = Data.OtherScoreAlias.NullIfEmpty(),
											 FormulaLanguage = Data.UsesCompiledFormulas
																   ? "C#"
																   : "DCM",
											 PlayerAnteFormula = Data.UsesPlayerAnte
																	 ? Data.PlayerAnteFormula.NullIfEmpty()
																	 : null,
											 ProvisionalScoreFormula = Data.UsesProvisionalScore
																		   ? Data.ProvisionalScoreFormula.NullIfEmpty()
																		   : null,
											 Data.FinalScoreFormula,
											 Data.TestGamePlayers
										 };

	protected override void Update(dynamic record)
	{
		//	TODO - some of these things should be required and cause a 400 if not given properly
		Data.PointsPerGame = (int)record.PointsPerGame;
		Data.SignificantDigits = (int)record.SignificantDigits;
		Data.DrawPermissions = (Data.ScoringSystem.DrawRules)record.DrawPermissions;
		Data.FinalGameYear = (int?)record.FinalGameYear;
		Data.UsesGameResult = (bool)record.UsesGameResult;
		Data.UsesCenterCount = (bool)record.UsesCenterCount;
		Data.UsesYearsPlayed = (bool)record.UsesYearsPlayed;
		Data.OtherScoreAlias = (string)record.OtherScoreAlias ?? string.Empty;
		Data.PlayerAnteFormula = (string)record.PlayerAnteFormula ?? string.Empty;
		Data.ProvisionalScoreFormula = (string)record.ProvisionalScoreFormula ?? string.Empty;
		Data.FinalScoreFormula = (string)record.FinalScoreFormula ?? string.Empty;
		Data.SetCompiledFormulae(record.FormulaLanguage is "C#");
	}
}
