using System.Runtime.Serialization;
using Data;

namespace API;

[PublicAPI]
internal sealed class System : Rest<System, ScoringSystem, System.Detail>
{
	public enum Languages : byte
	{
		DCM,
		[EnumMember(Value = "C#")]
		CSharp
	}

	public int Id => Identity;
	public string Name => RecordedName;

	[PublicAPI]
	internal sealed record Detail(
		int? PointsPerGame,
		int SignificantDigits,
		ScoringSystem.DrawRules DrawPermissions,
		int? FinalGameYear,
		bool UsesGameResult,
		bool UsesCenterCount,
		bool UsesYearsPlayed,
		bool UsesOtherScore,
		string? OtherScoreAlias,
		string? PlayerAnteFormula,
		string? ProvisionalScoreFormula,
		string FinalScoreFormula,
		Languages Language,
		IEnumerable<Tester> TestGame) : DetailClass;

	protected override Detail Info => new (Record.PointsPerGame,
										   Record.SignificantDigits,
										   Record.DrawPermissions,
										   Record.FinalGameYear,
										   Record.UsesGameResult,
										   Record.UsesCenterCount,
										   Record.UsesYearsPlayed,
										   Record.UsesOtherScore,
										   Record.OtherScoreAlias.NullIfEmpty(),
										   Record.UsesPlayerAnte
											   ? Record.PlayerAnteFormula.NullIfEmpty()
											   : null,
										   Record.UsesProvisionalScore
											   ? Record.ProvisionalScoreFormula.NullIfEmpty()
											   : null,
										   Record.FinalScoreFormula,
										   Record.UsesCompiledFormulas
														 ? Languages.CSharp
														 : Languages.DCM,
										   TestData());

	[PublicAPI]
	public sealed class Tester(Game.GamePlayer gamePlayer)
	{
		public Powers Power { get; init; } = gamePlayer.Power;
		public GamePlayer.Results Result { get; init; } = gamePlayer.Result;
		public int? Years { get; init; } = gamePlayer.Years;
		public int? Centers { get; init; } = gamePlayer.Centers;
		public double? Other { get; init; } = gamePlayer.Other;
		public double? PlayerAnte { get; init; } = gamePlayer.PlayerAnte;
		public double? ProvisionalScore { get; init; } = gamePlayer.ProvisionalScore;
		public double? FinalScore { get; init; } = gamePlayer.FinalScore;

		internal static void SetTestGameSystem(System system)
			=> _testGame.ScoringSystem = system.Record;

		private static Data.Game _testGame = new ();

		internal GamePlayer Record
			=> new ()
			   {
				   Power = Power,
				   Result = Result,
				   Centers = Centers,
				   Years = Years,
				   Other = Other ?? default,
				   Game = _testGame
			   };
	}

	private IEnumerable<Tester> TestData()
	{
		if (Record.TestGamePlayers is null)
			return [];
		if (!Record.ScoreWithResults(Record.TestGamePlayers, out _))
			throw new ();
		return Record.TestGamePlayers.Select(gamePlayer => new Tester(new (gamePlayer, Record)));
	}

	internal static void CreateEndpoints(WebApplication app)
		=> CreateCrudEndpoints(app);

	private protected override string[] Update(System system)
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
		Record.SetCompiledFormulae(system.Details.Language is Languages.CSharp);
		try
		{
			Tester.SetTestGameSystem(this);
			var testData = system.Details.TestGame.Select(static player => player.Record).ToList();
			if (testData.Count is not 7
			|| testData.Any(power => power.Power is Powers.TBD
								  || Record.UsesYearsPlayed && (power.Years < 1 || power.Years > Record.FinalGameYear - 1900)
								  || Record.UsesCenterCount && power.Centers > 34)
			|| testData.Select(static power => power.Power).Distinct().Count() is not 7
			|| Record.UsesCenterCount && testData.Sum(static power => power.Centers) is < 22 or > 34)
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
	}
}
