using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Scripting;
using static System.Diagnostics.Stopwatch;
using static System.Environment;
using static System.Reflection.Assembly;
using static Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript;

namespace Data;

public sealed partial class ScoringSystem : IdentityRecord<ScoringSystem>
{
	public enum DrawRules : byte
	{
		//	IMPORTANT: Must be 0, 1, 2 to match SQL storage
		None = 0,
		All = 1,
		DIAS = 2
	}

	public static bool ShowTimingData { private get; set; }

	public DrawRules DrawPermissions;
	public List<GamePlayer>? TestGamePlayers;
	public string OtherScoreAlias = Empty;
	public string PlayerAnteFormula = Empty;
	public string ProvisionalScoreFormula = Empty;
	public int? FinalGameYear;
	public int? PointsPerGame;
	public int SignificantDigits;
	public bool UsesCenterCount;
	public bool UsesGameResult;
	public bool UsesYearsPlayed;

	public string FinalScoreFormula
	{
		get => _finalScoreFormula.TrimEnd(CompiledFormulaSuffix);
		set => _finalScoreFormula = value;
	}

	public bool UsesProvisionalScore => ProvisionalScoreFormula.Length is not 0;
	public bool UsesPlayerAnte => PlayerAnteFormula.Length is not 0;
	public bool FinalScoreFormulaMissing => RemoveComments(FinalScoreFormula).Length is 0;
	public bool DrawsAllowed => DrawPermissions is not DrawRules.None;
	public bool DrawsIncludeAllSurvivors => DrawPermissions is DrawRules.DIAS;
	public bool UsesOtherScore => OtherScoreAlias.Length is not 0;
	public bool UsesCompiledFormulas => _finalScoreFormula.LastOrDefault() is CompiledFormulaSuffix;

	public List<Tournament> Tournaments => [..ReadMany<Tournament>(tournament => tournament.ScoringSystemId == Id
																			  || tournament.Rounds.Any(round => round.ScoringSystemId == Id)
																			  || tournament.Games.Any(game => game.ScoringSystemId == Id))];

	public IEnumerable<Game> Games => [..ReadMany<Game>(game => game.ScoringSystemId == Id)];
	public string ScoreFormat => $"F{SignificantDigits}";

	private enum FormulaType : byte
	{
		FinalScore,
		ProvisionalScore,
		PlayerAnte
	}

	private const char Bar = '|';

	private static readonly TimeSpan ScriptTimeout = TimeSpan.FromSeconds(7);

	private string _finalScoreFormula = Empty;

	private string TestGameData
	{
		get => TestGamePlayers is null
				   ? Empty
				   : Join(Bar,
						  TestGamePlayers.Select(static player => Join(Comma,
																	   player.Power,
																	   player.Result,
																	   player.Centers,
																	   player.Years,
																	   player.Other)));
		set
		{
			if (value.Length is 0)
			{
				TestGamePlayers = null;
				return;
			}
			//	The format of the TestGameData string is seven vertical-bar
			//	separated sets of "PowerName,GameResult,Centers,Years,Other".
			//	For example, Austria,Win,18,10,2.3|England,Loss,0,4,0.1|...
			//	Use absolute emptiness for null (when game result, centers,
			//	years, or other score does not matter in the system).
			var powers = value.Split(Bar)
							  .Select(static testData => testData.Split(Comma)
																 .Select(static s => s.Trim())
																 .ToArray())
							  .ToArray();
			if (powers.Any(static data => data.Length is not 5))
				throw new InvalidOperationException(); // TODO
			TestGamePlayers = [
								..powers.Select(static data => new GamePlayer
															   {
																   Power = data[0].As<Powers>(),
																   Result = data[1].Length is 0
																				? Unknown
																				: data[1].As<Results>(),
																   Centers = data[2].AsNullableInteger(),
																   Years = data[3].AsNullableInteger(),
																   Other = data[4].AsDouble()
															   })
										.Order()
							  ];
			//	TODO: Could add a lot more checks, like that winners all play to the final
			//	TODO: game-year, or that no Centers are given if the scoring system doesn't
			//	TODO: call for them, etc., or that if it does call for Centers, all seven
			//	TODO: players provide it, or that the total center count is between 22 andAddRe
			//	TODO: 34, etc., etc.  However, the only strings coming in here should come
			//	TODO: from test games; they have all this validation already.
			if (TestGamePlayers.Any(static gamePlayer => gamePlayer.Power is TBD)
			||  TestGamePlayers.Select(static gamePlayer => gamePlayer.Power)
							   .Distinct()
							   .Count() < 7)
				throw new InvalidOperationException(); // TODO
		}
	}

	public string FormattedScore(double score,
								 bool trim = false)
	{
		var formatted = score.ToString(ScoreFormat);
		if (trim)
			formatted = InsignificantDigits.Replace(formatted, Empty);
		return formatted;
	}

	public void SetCompiledFormulae(bool isCompiled)
		=> FinalScoreFormula += isCompiled
									? CompiledFormulaSuffix
									: Empty;

	public bool ScoreWithResults(List<GamePlayer> gamePlayers,
								 out List<string?> results)
	{
		if (!GameDataValid(out results))
			return false;
		const string bar = "――――――――――――――――――――";
		var watch = ShowTimingData
						? StartNew()
						: null;
		gamePlayers = [..gamePlayers.OrderBy(static gamePlayer => gamePlayer.Power)];
		var scoring =
			Calculator.Scoring =
				new (this, gamePlayers);
		//	Group games (and scoring system test games) determine each PlayerAnte
		//	(if the scoring system uses one) by running the PlayerAnte formula.
		//	This calculated ante is later subtracted from the player's FinalScore.
		//	Tournament games, however, use PointsPerGame / 7 as the PlayerAnte,
		//	and do NOT auto-subtract it from the FinalScore.
		var game = gamePlayers.First()
							  .Game;
		var calculateAnte = UsesPlayerAnte && (game.IsNone || !game.Tournament.IsEvent);
		if (UsesPlayerAnte)
		{
			//	Set only each of the GamePlayer objects' .PlayerAnte at first
			var defaultAnte = PointsPerGame / 7 ?? default;
			foreach (var gamePlayer in gamePlayers)
				try
				{
					gamePlayer.PlayerAnte = calculateAnte
												? Calculate(gamePlayer.Power, FormulaType.PlayerAnte)
												: defaultAnte;
					results.Add($"Player Ante for {gamePlayer.Power} = {gamePlayer.PlayerAnte:0.#######}");
				}
				catch (Exception exception)
				{
					results.Add($"Player ante calculation for {gamePlayer.Power} FAILED: {ErrorDetail(exception)}");
					return false;
				}
			results.Add(bar);
			//	And only NOW update the scoring provider objects with the player antes.
			//  This way, the ante formula can't refer to any other power's .PlayerAnte.
			scoring.UpdatePlayerAntes();
		}
		if (UsesProvisionalScore)
		{
			//	Set only each of the GamePlayer objects' .ProvisionalScore at first
			foreach (var gamePlayer in gamePlayers)
				try
				{
					gamePlayer.ProvisionalScore = Calculate(gamePlayer.Power, FormulaType.ProvisionalScore);
					results.Add($"Provisional score for {gamePlayer.Power} = {gamePlayer.ProvisionalScore:0.#######}");
				}
				catch (Exception exception)
				{
					results.Add($"Provisional scoring for {gamePlayer.Power} FAILED: {ErrorDetail(exception)}");
					return false;
				}
			results.Add(bar);
			//	And only NOW update the scoring provider objects with the provisional scores.
			//  This way, the provisional formula can't refer to any other power's .ProvisionalScore.
			scoring.UpdateProvisionalScores();
		}
		foreach (var gamePlayer in gamePlayers)
			try
			{
				gamePlayer.FinalScore = Calculate(gamePlayer.Power, FormulaType.FinalScore);
				if (calculateAnte)
					gamePlayer.FinalScore -= gamePlayer.PlayerAnte;
				results.Add($"Final score for {gamePlayer.Power} = {FormattedScore(gamePlayer.FinalScore, true)}");
			}
			catch (Exception exception)
			{
				var fault = scoring.OtherScoreValid is false
								? "game data"
								: $"{gamePlayer.Power}";
				results.Add($"Final scoring for {fault} FAILED: {ErrorDetail(exception)}");
				return false;
			}
		watch?.Stop();
		double roundedAntes;
		const string rounded = " (rounded)";
		if (UsesPlayerAnte)
		{
			var totalAntes = scoring.SumOfPlayerAntes;
			roundedAntes = double.Round(totalAntes);
			results.AddRange(bar, $"Total player antes{(totalAntes.Equals(roundedAntes) ? null : rounded)} = {roundedAntes}");
		}
		else
			roundedAntes = 0;
		var total = gamePlayers.Sum(static gamePlayer => gamePlayer.FinalScore);
		var roundedTotal = PointsPerGame is null
							   ? total
							   : double.Round(total);
		if (roundedTotal.Equals(double.NegativeZero))
			roundedTotal = 0;
		results.AddRange(bar, $"Total points awarded{(total.Equals(roundedTotal) ? null : rounded)} = {FormattedScore(roundedTotal, true)}");
		if (watch is not null)
			results.AddRange(bar, $"Time to score: {watch.ElapsedMilliseconds / 1_000m} sec.");
		if (PointsPerGame is null)
			return true;
		var expectedTotal = calculateAnte
								? 0
								: PointsPerGame;
		var error = expectedTotal != (int)roundedTotal
						? $"Total did not match expected points per game of {expectedTotal}."
						: UsesPlayerAnte && (int)roundedAntes != PointsPerGame
							? $"Player antes did not total to expected points per game of {PointsPerGame}."
							: null;
		var status = error is null;
		if (!status)
			results.AddRange(null, error);
		return status;

		static string ErrorDetail(Exception exception)
		{
			var message = $"{exception.Message} {exception.InnerException?.Message}";
			return exception switch
				   {
					   CompilationErrorException when message.Count(static c => c is Colon) > 1 => Join(Colon, message.Split(Colon).Skip(2)),
					   AggregateException aggregateException                                    => Join(NewLine, aggregateException.InnerExceptions
																																   .Select(static inner => inner.Message)),
					   _                                                                        => message
				   };
		}

		bool GameDataValid(out List<string?> issues)
		{
			issues = [];
			var powers = gamePlayers.Select(static gamePlayer => gamePlayer.Power)
									.ToArray();
			if (powers.Contains(TBD) || powers.Length is not 7)
				issues.Add("All powers must be assigned.");
			if (powers.Distinct().Count() is not 7)
				issues.Add("Power multiply assigned.");
			var unknownResults = gamePlayers.Count(static gamePlayer => gamePlayer.Result is Unknown);
			if (UsesGameResult)
			{
				if (unknownResults is not 0)
					issues.Add("Power found without win/loss result.");
				if (gamePlayers.All(static gamePlayer => gamePlayer.Result is Loss))
					issues.Add("No winning player(s) found.");
				else if (!DrawsAllowed && gamePlayers.Count(static gamePlayer => gamePlayer.Result is Win) is not 1)
					issues.Add("No solo victor found.");
			}
			else if (unknownResults is not 7)
				issues.Add("Win/Loss info found but not used in scoring system.");
			if (UsesCenterCount)
			{
				var totalCenters = gamePlayers.Sum(static gamePlayer => gamePlayer.Centers);
				if (gamePlayers.Any(static gamePlayer => gamePlayer.Centers < 0))
					issues.Add("Negative SC count found.");
				if (totalCenters is < 22 or > 34)
					issues.Add($"Invalid total SC count ({totalCenters}).");
				if (UsesGameResult)
				{
					if (gamePlayers.Any(static gamePlayer => gamePlayer is { Centers: 0, Result: Win }))
						issues.Add("Eliminated player indicated as winning.");
					if (gamePlayers.Any(static gamePlayer => gamePlayer is { Centers: > 17, Result: Loss }))
						issues.Add("Solo victor not credited with win.");
					if (DrawsIncludeAllSurvivors
					&&  gamePlayers.Count(static gamePlayer => gamePlayer.Result is Win) > 1 //	allows for solo or concession
					&&  gamePlayers.Any(static gamePlayer => gamePlayer is { Centers: not 0, Result: Loss }))
						issues.Add("Survivor not included in mandated DIAS.");
				}
			}
			else if (gamePlayers.Any(static gamePlayer => gamePlayer.Centers.HasValue))
				issues.Add("Center count found but not used in scoring system.");
			if (UsesYearsPlayed)
			{
				if (gamePlayers.Any(static gamePlayer => gamePlayer.Years is < 2 or > LatestFinalGameYear - 1900))
					issues.Add("Invalid years played found.");
				var lastGameYear = gamePlayers.Max(static gamePlayer => gamePlayer.Years);
				if (FinalGameYear > 0 && lastGameYear > FinalGameYear)
					issues.Add("Final game year surpassed.");
				if (UsesGameResult
				&&  gamePlayers.Any(gamePlayer => gamePlayer.Result is Win && gamePlayer.Years < lastGameYear))
					issues.Add("Winning power(s) exiting game early.");
				if (UsesCenterCount
				&&  gamePlayers.Any(gamePlayer => gamePlayer.Centers > 0 && gamePlayer.Years < lastGameYear))
					issues.Add("Power with centers found not surviving final year.");
			}
			else if (gamePlayers.Any(static gamePlayer => gamePlayer.Years.HasValue))
				issues.Add("Years-Played info provided but not used in scoring system.");
			return issues.Count is 0;
		}

		double Calculate(Powers powerName,
						 FormulaType formulaType)
		{
			var code = formulaType switch
					   {
						   FormulaType.PlayerAnte       => PlayerAnteFormula,
						   FormulaType.ProvisionalScore => ProvisionalScoreFormula,
						   FormulaType.FinalScore       => FinalScoreFormula,
						   _                            => throw new NotImplementedException("Unrecognized FormulaType value")
					   };
			if (code.Length is 0)
				return 0;
			scoring.PlayerPower = powerName;
			//	Pull the cleaned (no comments and OtherScore alias replaced)
			//	formula from cache, or clean it and put it there for later calls.
			var formula = CleanedFormulas.GetOrSet(code, RemoveComments);
			var result = UsesCompiledFormulas
							 ? CalculateCompiled()
							 : CalculateStandard();
			return formulaType is FormulaType.ProvisionalScore
					   ? result
					   : double.Round(result, SignificantDigits);

			double CalculateStandard()
				=> Calculator.Calculate(formula);

			double CalculateCompiled()
			{
				var compiled = Scripts.GetOrSet(formula,
												static text =>
												{
													if (text.Last() is Semicolon)
														text = $"new Func<double>(() => {{{text}}}).Invoke()";
													return Create<double>($"return (double)({text});", Options, typeof (Scoring));
												});
				using CancellationTokenSource cancellationTokenSource = new ();
				using var task = compiled.RunAsync(scoring, cancellationTokenSource.Token);
				return task.Wait(ScriptTimeout)
						   ? task.Result.ReturnValue
						   : throw new TimeoutException();
			}
		}
	}

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<ScoringSystem>();
		Id = record.Integer(nameof (Id));
		Name = record.String(nameof (Name));
		DrawPermissions = record.IntegerAs<DrawRules>(nameof (DrawPermissions));
		UsesGameResult = record.Boolean(nameof (UsesGameResult));
		UsesCenterCount = record.Boolean(nameof (UsesCenterCount));
		UsesYearsPlayed = record.Boolean(nameof (UsesYearsPlayed));
		FinalGameYear = record.NullableInteger(nameof (FinalGameYear));
		ProvisionalScoreFormula = record.String(nameof (ProvisionalScoreFormula));
		_finalScoreFormula = record.String(nameof (FinalScoreFormula));
		SignificantDigits = record.Integer(nameof (SignificantDigits));
		TestGameData = record.String(nameof (TestGameData));
		PointsPerGame = record.NullableInteger(nameof (PointsPerGame));
		OtherScoreAlias = record.String(nameof (OtherScoreAlias));
		PlayerAnteFormula = record.String(nameof (PlayerAnteFormula));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
											   [{{nameof (Name)}}] = {0},
											   [{{nameof (DrawPermissions)}}] = {1},
											   [{{nameof (UsesGameResult)}}] = {2},
											   [{{nameof (UsesCenterCount)}}] = {3},
											   [{{nameof (UsesYearsPlayed)}}] = {4},
											   [{{nameof (FinalGameYear)}}] = {5},
											   [{{nameof (PointsPerGame)}}] = {6},
											   [{{nameof (ProvisionalScoreFormula)}}] = {7},
											   [{{nameof (FinalScoreFormula)}}] = {8},
											   [{{nameof (TestGameData)}}] = {9},
											   [{{nameof (SignificantDigits)}}] = {10},
											   [{{nameof (OtherScoreAlias)}}] = {11},
											   [{{nameof (PlayerAnteFormula)}}] = {12}
											   """;

	public override string FieldValues => Format(FieldValuesFormat,
												 Name.ForSql(),
												 DrawPermissions.ForSql(),
												 UsesGameResult.ForSql(),
												 UsesCenterCount.ForSql(),
												 UsesYearsPlayed.ForSql(),
												 FinalGameYear.ForSql(),
												 PointsPerGame.ForSql(),
												 ProvisionalScoreFormula.ForSql(),
												 _finalScoreFormula.ForSql(),
												 TestGameData.ForSql(),
												 SignificantDigits,
												 OtherScoreAlias.ForSql(),
												 PlayerAnteFormula.ForSql());

	#endregion

	#region Formula Calculation

	private const string DocumentCommentSplitter = "**";
	private const char CompiledFormulaSuffix = '\a'; //	Cannot be 0, as this truncates the SQL insert

	private static readonly ScriptOptions Options = ScriptOptions.Default
																 .WithReferences(GetAssembly(typeof (Enumerable)).OrThrow(),
																				 GetExecutingAssembly())
																 .WithImports(nameof (System),
																			  typeof (List<int>).Namespace.OrThrow(),
																			  typeof (Enumerable).Namespace.OrThrow(),
																			  typeof (Math).FullName.OrThrow()); // nameof (DCM) used to be here too; unnecessary???

	/// <summary>
	///     Holds formulae after comments have been removed and OtherScoreAlias instances swapped out.
	/// </summary>
	private static readonly SortedDictionary<string, string> CleanedFormulas = [];

	/// <summary>
	///     Holds compiled C# formulae, and doing so really speeds up things considerably.
	/// </summary>
	private static readonly SortedDictionary<string, Script<double>> Scripts = [];

	[GeneratedRegex(@"[\s_]")]
	private static partial Regex StripperRegex();
	private static readonly Regex Stripper = StripperRegex();

	[GeneratedRegex("OtherScores?")]
	private static partial Regex OtherScoresRegex();
	private static readonly Regex OtherScores = OtherScoresRegex();

	[GeneratedRegex("[.]0*$")]
	private static partial Regex InsignificantDigitsRegex();
	private static readonly Regex InsignificantDigits = InsignificantDigitsRegex();

	private string RemoveComments(string formula)
	{
		const string lineCommentStart = "//",
					 blockComment = "/[*].*?[*]/",
					 lineComment = $@"{lineCommentStart}.*?\n",
					 rawString = """[$]*("{3,}).*?\1""",
					 simpleString = """[$]*"(\\(.|\r?\n)|[^\n\\"])*["]""",
					 verbatimString = """(@[$]*|[$]+@)("[^"]*")+""",
					 formulaRegex = $"{blockComment}|{lineComment}",
					 cSharpRegex = $"{rawString}|{simpleString}|{verbatimString}|{formulaRegex}",
					 underbar = "_";

		//	NOTE: In the cSharpRegex, the comments regex must be AFTER the strings, so that comment-like strings in literals are protected
		formula = Regex.Replace(formula.Trim(CompiledFormulaSuffix) + NewLine, //	Add NewLine to catch text-final line comments
								UsesCompiledFormulas
									? cSharpRegex
									: formulaRegex,
								static match => match.Value.Starts("/")
													? NewLine
													: match.Value,
								RegexOptions.Singleline);
		if (!UsesCompiledFormulas)
			//	Important: remove non-document comments before document comment
			formula = formula.Split(DocumentCommentSplitter)
							 .First()
							 .Replace(underbar, null);
		if (UsesOtherScore)
			ReplaceOtherScoreAlias();
		return formula.Trim();

		//	Replaces, say, "AverageTotalVote" (if "TotalVote" is the formula-supplied alias)
		//	with "AverageOtherScore" (the Scoring/PowerData object's field name), etc., etc.
		//	TODO: Using Reflection.Emit, we COULD create a new class that inherits from PowerData,
		//	TODO: give it the properties named with the alias, with getter implementations that
		//	TODO: return the appropriate OtherScore properties from the base PowerData class
		//	TODO: (these properties could then be made protected so that only the alias could be
		//	TODO: used to get the data), and then use that derived class instead of PowerData
		//	TODO: during scoring.  Seems like more trouble than it's worth right now, though.
		void ReplaceOtherScoreAlias()
		{
			//	Set "alias" to the OtherScoreAlias without spaces or underbars.
			var alias = Stripper.Replace(OtherScoreAlias, Empty);
			var options = UsesCompiledFormulas
							  ? RegexOptions.None
							  : RegexOptions.IgnoreCase;
			//	TODO: This changes the identifier everywhere, even in error text (DCM) and quoted strings (C#)
			formula = Scoring.OtherScoreAliases
							 .Aggregate($" {formula} ",
										(current, other) => Regex.Replace(current,
																		  $@"([^\w]){OtherScores.Replace(other, alias)}([^\w])",
																		  $"$1{other}$2",
																		  options));
			//	The spaces we added on either end of "{formula}" should be removed; the caller will do it
		}
	}

	#endregion
}
