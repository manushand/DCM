using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal sealed class ScoringSystem : Rest<ScoringSystem, Data.ScoringSystem>
{
	protected override dynamic Detail => new
										 {
											 Record.PointsPerGame,
											 Record.SignificantDigits,
											 Record.DrawPermissions,
											 Record.FinalGameYear,
											 Record.UsesGameResult,
											 Record.UsesCenterCount,
											 Record.UsesYearsPlayed,
											 Record.UsesOtherScore,
											 OtherScoreAlias = Record.OtherScoreAlias.NullIfEmpty(),
											 FormulaLanguage = Record.UsesCompiledFormulas
																   ? "C#"
																   : "DCM",
											 PlayerAnteFormula = Record.UsesPlayerAnte
																	 ? Record.PlayerAnteFormula.NullIfEmpty()
																	 : null,
											 ProvisionalScoreFormula = Record.UsesProvisionalScore
																		   ? Record.ProvisionalScoreFormula.NullIfEmpty()
																		   : null,
											 Record.FinalScoreFormula
										 };
}
