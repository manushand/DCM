//	#define Routines

using System.Text.RegularExpressions;
using static System.Double;
using static System.StringComparer;

namespace Data;

using static Scoring;

public sealed partial class ScoringSystem
{
	private sealed partial class Calculator
	{
		internal static Scoring Scoring
		{
			get => field.IsNone
					   ? throw new NullReferenceException(nameof (Scoring))
					   : field;
			set;
		} = Scoring.None;

		private readonly PowerData _powerData;
		private string _formula;
		private double _term;

		private enum Operators : byte
		{
			Reset = default,
			If, //	IMPORTANT: "Reset" and "If" must precede all other names
			Becomes,
			IsError,
			Plus,
			Minus,
			Times,
			Over,
			TruncateOver,
			RoundOver,
			CeilingOver,
			FloorOver,
			Mod,
			ToPower,
			LessThan,
			LessOrEqual,
			Exceeds,
			ExceedsOrIs,
			Equals,
			Is = Equals,
			IsNot,
			And,
			Or,
			Min,
			Max
		}

		private static readonly SortedDictionary<string, Operators> ShorthandLookups =
			new ()
			{
				//	Note that there are also these unary operators (parsed in GetTerm):
				//	- (mathematical negative of)	!  (logical negative of)
				//	+ (absolute value of)			\| (square root of)
				//	# (center rank could be)		~  (elimination order could be)		$ (survivor rank could be)
				//	@ (won and numWinners is) with @# giving 0 if the player did not win, or Winners if he did
				[$"{Semicolon}"] = Operators.Reset,
				["{"] = Operators.If,
				["->"] = Operators.Becomes,
				["?"] = Operators.IsError,
				["+"] = Operators.Plus,
				["-"] = Operators.Minus,
				["*"] = Operators.Times,
				["/"] = Operators.Over,
				[@"\"] = Operators.TruncateOver,
				[@"\\"] = Operators.RoundOver,
				[@"/\"] = Operators.CeilingOver,
				[@"\/"] = Operators.FloorOver,
				["%"] = Operators.Mod,
				["^"] = Operators.ToPower,
				["<"] = Operators.LessThan,
				["<="] = Operators.LessOrEqual,
				[">="] = Operators.ExceedsOrIs,
				[">"] = Operators.Exceeds,
				["="] = Operators.Equals,
				["<>"] = Operators.IsNot,
				["&"] = Operators.And, //	But not shortcut
				["|"] = Operators.Or   //	But not shortcut
			};

		private static readonly SortedDictionary<Operators, Func<double, double, double>> Operate =
			new ()
			{
				[Operators.Reset] = static (result, _) => result, //	Not 0 but terminal ; will always give 0
				[Operators.If] = static (_, term) => term,        //	Pre-calculated (result of one of the clauses)
				[Operators.Becomes] = static (_, term) => term,   //	Same lambda as Operators.If
				[Operators.IsError] = static (_, _) => default,   //	Error check failed, proceeding to Reset
				[Operators.Plus] = static (number, addend) => number + addend,
				[Operators.Minus] = static (number, subtraction) => number - subtraction,
				[Operators.Times] = static (number, multiplicand) => number * multiplicand,
				[Operators.Over] = static (result, term) => term is 0 ? 0 : result / term,
				[Operators.RoundOver] = static (result, term) => term is 0 ? 0 : Round(result / term),
				[Operators.TruncateOver] = static (result, term) => term is 0 ? 0 : Truncate(result / term),
				[Operators.CeilingOver] = static (result, term) => term is 0 ? 0 : Ceiling(result / term),
				[Operators.FloorOver] = static (result, term) => term is 0 ? 0 : Floor(result / term),
				[Operators.Mod] = static (result, term) => term is 0 ? 0 : result % term,
				[Operators.ToPower] = static (result, term) => result < 0 && term.NotEquals(Truncate(term)) || result is 0 && term < 0
																   ? 0
																   : Math.Pow(result, term),
				[Operators.LessThan] = static (result, term) => (result < term).AsInteger(),
				[Operators.Exceeds] = static (result, term) => (result > term).AsInteger(),
				[Operators.Is] = static (result, term) => result.Equals(term).AsInteger(),
				[Operators.IsNot] = static (result, term) => result.NotEquals(term).AsInteger(),
				[Operators.ExceedsOrIs] = static (result, term) => (result >= term).AsInteger(),
				[Operators.LessOrEqual] = static (result, term) => (result <= term).AsInteger(),
				[Operators.And] = static (result, term) => Math.Abs(Math.Sign(result * term)),
				[Operators.Or] = static (result, term) => Math.Sign(Math.Abs(result) + Math.Abs(term)),
				[Operators.Min] = Math.Max, //	Yes, min means get max of the two,
				[Operators.Max] = Math.Min  //	and vice-versa. I.e., 60 max 40 = 40
			};

		[GeneratedRegex(@"^[a-z][a-z\d]*", RegexOptions.IgnoreCase)]
		private static partial Regex Alias();

		[GeneratedRegex(@"^(?=-?\.?\d)-?\d*\.?\d*")]
		private static partial Regex Number();

		//	The Operator*hands collections must be ordered arrays, due to things like = vs. == and Is vs. IsNot.
		//	These are detected using .FirstOrDefault(Formula.Starts()), which provides the case-insensitivity.
		private static readonly string[] OperatorShorthands = [..ShorthandLookups.Keys
																				 .OrderByDescending(static @operator => @operator.Length)];

		//	No longhand for Reset or If (not until I can think of an "ENDIF" or "FI" that I like)
		private static readonly string[] OperatorLonghands = [..Enum.GetNames(typeof (Operators))
																	.Where(static longhand => longhand.As<Operators>() > Operators.If)
																	.OrderByDescending(static @operator => @operator.Length)];

		//	The PowerNames and *Aliases collections can be HashSets and Dictionaries
		//	(with case-insensitive comparer), since a full (long-as-can-be) alias is
		//	parsed from the Formula before a match for it is sought in these collections.
		private static readonly HashSet<string> PowerNames = Enum.GetNames<PowerNames>()
																 .Where(static power => power != $"{TBD}")
																 .ToHashSet(OrdinalIgnoreCase);

		private static readonly HashSet<string> ReservedAliases = new (OrdinalIgnoreCase);

		private static readonly SortedDictionary<string, double> Aliases = new (OrdinalIgnoreCase);

#if Routines
		private static readonly SortedDictionary<string, string> Routines = new (OrdinalIgnoreCase);

		private const string RoutineQuotes = "\"'`";
#else
		private const string RoutineQuotes = "";
#endif
		private const char RepeatQuote = '`';

		private static readonly string AllQuotes = $"{RoutineQuotes}{RepeatQuote}";

		private static readonly Dictionary<string, Func<double>> GameDataAliases =
			new (OrdinalIgnoreCase)
			{
				//	Integers
				[nameof (Scoring.Losers)] = static () => Scoring.Losers,
				[nameof (Scoring.Winners)] = static () => Scoring.Winners,
				[nameof (Scoring.DrawSize)] = static () => Scoring.DrawSize,
				[nameof (Scoring.Survivors)] = static () => Scoring.Survivors,
				[nameof (Scoring.LeaderCenters)] = static () => Scoring.LeaderCenters,
				[nameof (Scoring.FewestCenters)] = static () => Scoring.FewestCenters,
				[nameof (Scoring.WinnersCenters)] = static () => Scoring.WinnersCenters,
				[nameof (Scoring.SurvivorsCenters)] = static () => Scoring.SurvivorsCenters,
				[nameof (Scoring.GameYears)] = static () => Scoring.GameYears,
				[nameof (Scoring.SumOfYears)] = static () => Scoring.SumOfYears,
				[nameof (Scoring.SumOfCentersSquared)] = static () => Scoring.SumOfCentersSquared,
				//	Doubles
				[nameof (Scoring.SumOfProvisionalScores)] = static () => Scoring.SumOfProvisionalScores,
				[nameof (Scoring.AverageProvisionalScore)] = static () => Scoring.AverageProvisionalScore,
				[nameof (Scoring.PointsPerGame)] = static () => Scoring.PointsPerGame,
				[nameof (Scoring.SumOfPlayerAntes)] = static () => Scoring.SumOfPlayerAntes,
				[nameof (Scoring.AveragePlayerAnte)] = static () => Scoring.AveragePlayerAnte,
				[nameof (Scoring.LowestPlayerAnte)] = static () => Scoring.LowestPlayerAnte,
				[nameof (Scoring.HighestPlayerAnte)] = static () => Scoring.HighestPlayerAnte,
				[nameof (Scoring.SumOfRunningScores)] = static () => Scoring.SumOfRunningScores,
				[nameof (Scoring.AverageRunningScore)] = static () => Scoring.AverageRunningScore,
				[nameof (Scoring.LowestRunningScore)] = static () => Scoring.LowestRunningScore,
				[nameof (Scoring.HighestRunningScore)] = static () => Scoring.HighestRunningScore,
				[nameof (Scoring.AverageOfAverageGameScores)] = static () => Scoring.AverageOfAverageGameScores,
				//	Uses of the OtherScoreAlias are converted to these but these names will work too.
				[nameof (Scoring.SumOfOtherScores)] = static () => Scoring.SumOfOtherScores,
				[nameof (Scoring.SumOfEveryOtherScore)] = static () => Scoring.SumOfEveryOtherScore,
				[nameof (Scoring.AverageOtherScore)] = static () => Scoring.AverageOtherScore,
				[nameof (Scoring.LowestOtherScore)] = static () => Scoring.LowestOtherScore,
				[nameof (Scoring.HighestOtherScore)] = static () => Scoring.HighestOtherScore
			};

		private static readonly Dictionary<string, Func<PowerData, double>> PowerDataAliases =
			new (OrdinalIgnoreCase)
			{
				//	Booleans (integer 1 or 0)
				[nameof (Scoring)] = static powerData => (Scoring.Player.Power == powerData.Power).AsInteger(),
				[nameof (Scoring.Won)] = static powerData => powerData.Won.AsInteger(),
				[nameof (Scoring.WonAlone)] = static powerData => powerData.WonAlone.AsInteger(),
				[nameof (Scoring.WonSolo)] = static powerData => powerData.WonSolo.AsInteger(),
				[nameof (Scoring.WonConcession)] = static powerData => powerData.WonConcession.AsInteger(),
				[nameof (Scoring.WonDraw)] = static powerData => powerData.WonDraw.AsInteger(),
				[nameof (Scoring.Lost)] = static powerData => powerData.Lost.AsInteger(),
				[nameof (Scoring.Survived)] = static powerData => powerData.Survived.AsInteger(),
				[nameof (Scoring.SurvivedSolo)] = static powerData => powerData.SurvivedSolo.AsInteger(),
				[nameof (Scoring.SurvivedConcession)] = static powerData => powerData.SurvivedConcession.AsInteger(),
				[nameof (Scoring.SurvivedDraw)] = static powerData => powerData.SurvivedDraw.AsInteger(),
				[nameof (Scoring.Conceded)] = static powerData => powerData.Conceded.AsInteger(),
				[nameof (Scoring.Eliminated)] = static powerData => powerData.Eliminated.AsInteger(),
				[nameof (Scoring.Uneliminated)] = static powerData => powerData.Uneliminated.AsInteger(),
				[nameof (Scoring.WasLeader)] = static powerData => powerData.WasLeader.AsInteger(),
				//	Integers
				[nameof (Scoring.Years)] = static powerData => powerData.Years,
				[nameof (Scoring.Centers)] = static powerData => powerData.Centers,
				[nameof (Scoring.CentersSquared)] = static powerData => powerData.CentersSquared,
				[nameof (Scoring.WorstCenterRank)] = static powerData => powerData.WorstCenterRank,
				[nameof (Scoring.BestCenterRank)] = static powerData => powerData.BestCenterRank,
				[nameof (Scoring.CenterRankSharers)] = static powerData => powerData.CenterRankSharers,
				[nameof (Scoring.WorstSurvivorRank)] = static powerData => powerData.WorstSurvivorRank,
				[nameof (Scoring.BestSurvivorRank)] = static powerData => powerData.BestSurvivorRank,
				[nameof (Scoring.SurvivorRankSharers)] = static powerData => powerData.SurvivorRankSharers,
				[nameof (Scoring.EliminatedEarlier)] = static powerData => powerData.EliminatedEarlier,
				[nameof (Scoring.BestEliminationOrder)] = static powerData => powerData.BestEliminationOrder,
				[nameof (Scoring.WorstEliminationOrder)] = static powerData => powerData.WorstEliminationOrder,
				[nameof (Scoring.EliminationOrderSharers)] = static powerData => powerData.EliminationOrderSharers,
				//	Doubles
				[nameof (Scoring.CenterRank)] = static powerData => powerData.CenterRank,
				[nameof (Scoring.EliminationOrder)] = static powerData => powerData.EliminationOrder,
				[nameof (Scoring.ProvisionalScore)] = static powerData => powerData.ProvisionalScore,
				[nameof (Scoring.PlayerAnte)] = static powerData => powerData.PlayerAnte,
				[nameof (Scoring.RunningScore)] = static powerData => powerData.RunningScore,
				[nameof (Scoring.AverageGameScore)] = static powerData => powerData.AverageGameScore,
				[nameof (Scoring.OtherScore)] = static powerData => powerData.OtherScore
			};

		private double Result { get; }

		static Calculator()
			=> ReservedAliases.UnionWith([
											 nameof (Scoring),
											 ..typeof (Scoring).GetProperties(Instance | Public)
															   .Where(static property => property.PropertyType.IsValueType)
															   .Select(static property => property.Name),
											 ..PowerNames,
											 ..OperatorLonghands
										 ]);

		internal static double Calculate(string formula)
		{
			Aliases.Clear();
			return new Calculator(formula, Scoring.Player).Result;
		}

		private Calculator(string formula,
						   PowerData contextPlayer)
		{
			_formula = formula.Trim();
			_powerData = contextPlayer;
			var @operator = Operators.Reset;
			while (_formula.Length is not 0)
			{
				if (@operator is Operators.Reset)
					Result = 0;
				GetOperator();
				if (_formula.Length is 0 && @operator is not Operators.Reset)
					throw new InvalidOperationException($"Missing term for '{@operator}' operation.");
				GetTerm();
				Result = Operate.TryGetValue(@operator, out var func)
							 ? func(Result, _term)
							 : throw new InvalidOperationException($"Unimplemented operator ({@operator}).");
			}

			void GetOperator()
			{
				if (@operator is Operators.Reset)
				{
					while (_formula.FirstOrDefault() is Semicolon)
						DropCount();
					if (_formula.Length is not 0)
						@operator = Operators.Plus;
					return;
				}
				//	If the next character is a RepeatQuote, that is handled as a binary operator
				if (_formula.FirstOrDefault() is RepeatQuote)
				{
					@operator = Operators.Plus;
					return;
				}
				var shorthand = OperatorShorthands.FirstOrDefault(text => _formula.Starts(text, true));
				if (shorthand is null)
				{
					shorthand = OperatorLonghands.FirstOrDefault(text => _formula.Starts(text))
												 .OrThrow($"Unknown operator: {_formula.Split().First()}");
					@operator = shorthand.As<Operators>();
				}
				else
					@operator = ShorthandLookups[shorthand];
				DropText(shorthand);
			}

			void DropText(string parsed)
				=> DropCount(parsed.Length);

			void DropCount(int length = 1)
				=> _formula = _formula[length..].Trim();

			//	Gets (and removes) the Term for the current @operator, from the beginning of
			//	the Formula string. Formula is guaranteed to contain at least one character.
			void GetTerm()
			{
				var position = 0;
				//	Special handling for four operators:  Reset, If, Becomes, and IsError.
				//	The first of these does not need a Term at all, and the other three all
				//	must handle the right-hand side specially (that is, in a way a that an
				//	operation implemented using a Func<double, double, double> cannot).
				switch (@operator)
				{
				//	Don't parse for a Term if we're doing a Reset. Just return.
				case Operators.Reset:
					return;
				//	If we are getting a Term for the If operator, we scan to the ending
				//	right brace, and then choose one of the two expressions within the
				//	braces (separated by a colon or "else") based on the current value
				//	of Result, and run that expression to produce the requested Term,
				//	which itself will become the final Result of the If operation.
				case Operators.If:
					var (braceLevel, start) = (1, 0);
					string? formulaToRun = null;
					while (braceLevel > 0 && position < _formula.Length)
					{
						var ch = _formula[position];
						if (braceLevel is 1
						&& (ch is Colon || _formula[position..].Starts("else") //	.Starts checks Formula[4] for us
										&& (position is 0 || !char.IsLetterOrDigit(_formula[position - 1]))))
						{
							formulaToRun = _formula[..(position - 1)];
							start =
								position += ch is Colon
												? 1
												: 4;
							continue;
						}
						braceLevel += AdjustEmbedCount(ch, '{', '}');
						++position;
					}
					if (braceLevel > 0)
						throw new InvalidOperationException("Unclosed if/else brace.");
					if (Result is not 0)
						formulaToRun ??= _formula[..(position - 2)];
					else if (start is not 0)
						formulaToRun = _formula[start..(position - 1)];
					DropCount(position);
					_term = formulaToRun is null
								? 0
								: new Calculator(formulaToRun, _powerData).Result;
					return;
				//	If getting a Term for the Becomes operator, the next "Term" is an alias name; set it.
				case Operators.Becomes when GetAlias(out var alias):
					if (ReservedAliases.Contains(alias))
						throw new InvalidOperationException($"Attempt to redefine reserved alias name: {alias}");
					Aliases[alias] =
						_term =
							Result;
					return;
				case Operators.Becomes:
					throw new InvalidOperationException($"Invalid alias name: {_formula.Split().First()}");
				//	Error check
				case Operators.IsError:
					var error = _formula.Split(Semicolon)
										.First();
					DropText(error);
					if (Result is 0)
						return;
					error = error.Trim();
					throw new InvalidOperationException(error.Length is 0
															? "Unspecified Error Condition Detected in Formula."
															: error);
				//	All other operators parse the Formula to get their numeric Term in
				//	the same way.  Exit this switch; that parsing happens in the next one.
				case Operators.Plus:
				case Operators.Minus:
				case Operators.Times:
				case Operators.Over:
				case Operators.RoundOver:
				case Operators.TruncateOver:
				case Operators.CeilingOver:
				case Operators.FloorOver:
				case Operators.Mod:
				case Operators.ToPower:
				case Operators.LessThan:
				case Operators.LessOrEqual:
				case Operators.Exceeds:
				case Operators.ExceedsOrIs:
				case Operators.Equals:
				case Operators.IsNot:
				case Operators.And:
				case Operators.Or:
				case Operators.Min:
				case Operators.Max:
					break;
				default:
					throw new NotImplementedException($"Unrecognized {nameof (Operators)} value"); //	TODO
				}
				//	Otherwise the Term should be either a parenthesized expression
				//	(which we run to produce the value of the requested Term), or
				//	a number (which may be negative), an alias (which may be
				//	prefaced by a power name and a dot), a power name with a
				//	dotted or square-bracketed expression to run with that power
				//	in-context, or any of the above preceded by a unary operator.
				switch (_formula[0])
				{
				//	Parenthesized expression.
				case '(':
					var parenCount = 1;
					while (parenCount > 0 && ++position < _formula.Length)
						parenCount += AdjustEmbedCount(_formula[position], '(', ')');
					if (parenCount > 0)
						throw new InvalidOperationException("Unclosed parenthesis.");
					_term = new Calculator(_formula[1..position], _powerData).Result;
					DropCount(++position);
					return;
				//	Numbers
				case var _ when GetNumericTerm():
					return;
				//	UNARY OPERATOR CASES
				case '-':
					_term = -GetUnaryTerm();
					return;
				case '!':
					_term = (GetUnaryTerm() is 0).AsInteger();
					return;
				case '+':
					_term = Math.Abs(GetUnaryTerm());
					return;
				case '@' when _formula.Starts("@#"):
					DropCount(2);
					_term = _powerData.Won
								? Scoring.Winners
								: 0;
					return;
				case '@':
					//	Don't reverse the order of the && or the GetUnaryTerm may not get parsed past!
					_term = (Scoring.Winners == (int)GetUnaryTerm()
						 &&  _powerData.Won).AsInteger();
					return;
				case '#':
					_term = GetUnaryTerm().IsBetween(_powerData.BestCenterRank, _powerData.WorstCenterRank)
										  .AsInteger();
					return;
				case '$':
					_term = GetUnaryTerm().IsBetween(_powerData.BestSurvivorRank, _powerData.WorstSurvivorRank)
										  .AsInteger();
					return;
				case '~':
					_term = GetUnaryTerm().IsBetween(_powerData.WorstEliminationOrder, _powerData.BestEliminationOrder)
										  .AsInteger();
					return;
				case '\\' when _formula.Starts(@"\|"):
					DropCount(); //	Since this is a two-character unary operator, we need to drop one of the two ourselves.
					_term = GetUnaryTerm() < 0
								? 0
								: Math.Sqrt(_term);
					return;
				//	END OF UNARY OPERATOR CASES
				//	Aliases
				case var _ when GetAlias(out var alias):
					//	Game alias lookups
					if (GameDataAliases.TryGetValue(alias, out var gameDataFunc))
					{
						_term = gameDataFunc();
						return;
					}
					//	Power reference lookup.
					var isPowerAlias = PowerNames.Contains(alias);
					var powerContext = isPowerAlias
										   ? Scoring.Powers[alias.As<PowerNames>()]
										   : _powerData;
					if (isPowerAlias)
						switch (_formula.FirstOrDefault())
						{
						case '[':
							//	Power [ bracketedFormula ] handling
							var bracketLevel = 1;
							while (bracketLevel > 0 && ++position < _formula.Length)
								bracketLevel += AdjustEmbedCount(_formula[position], '[', ']');
							if (bracketLevel > 0)
								throw new InvalidOperationException($"Unclosed power reference bracket after {alias}.");
							_term = new Calculator(_formula[1..position], powerContext).Result;
							DropCount(++position);
							return;
						case '.':
							//	Power+dot -- prep for the coming formula-alias
							DropCount();
							GetAlias(out alias);
							break;
						default:
							throw new InvalidOperationException($"Missing dot or bracket after {alias}.");
						}
					//	Power formula-alias lookups
					if (PowerDataAliases.TryGetValue(alias, out var powerDataFunc))
						_term = powerDataFunc(powerContext);
#if Routines
					else if (Routines.TryGetValue(alias, out var routine))
						_term = new Calculator(routine, powerContext).Result;
#endif
					//	In constructions like Austria.X and Italy.X the X
					//	must have been one of the above, or it's an error
					else if (isPowerAlias)
						throw new InvalidOperationException($"Unrecognized power-data alias after dot-reference of {powerContext.Power}.");
					//	User-defined alias lookup
					else if (Aliases.TryGetValue(alias, out var term))
						_term = term;
					else
						//	All lookups failed.
						throw new InvalidOperationException($"Unrecognized formula alias: {alias}");
					return;
				case var quote when AllQuotes.Contains(quote):
					//	Parse to the matching end-quote (with embedded open/closes of the other types allowed, which
					//	could, within them, embed this type) and then demand a Becomes operator and a name (and then
					//	a semicolon maybe?).  Store the quoted text in the Routines Dictionary, so it has the provided
					//	alias name.  One problem is what we would then return for Term (hence requiring a Semicolon?).
					var (active, index, length) = ($"{quote}", 0, _formula.Length);
					while (++index < length && active.Length is not 0)
					{
						var ch = _formula[index];
						if (!AllQuotes.Contains(ch))
							continue;
						if (ch == active.Last())
							active = active.TrimEnd(ch);
						else
							active += ch;
					}
					if (active.Length is not 0)
						throw new InvalidOperationException($"Unclosed quotation mark: {quote}.");
					var routineText = _formula[1..(index - 1)].Trim();
					DropCount(index);
					if (quote is not RepeatQuote)
						return;
					//	The @operator has already been changed to Operators.Plus; figure out the addend.
					if (_powerData.Lost)
						_term = -Result;
					else
					{
						var drawSize = Scoring.Winners;
						if (drawSize is 1)
							_term = 0;
						else
						{
							var equation = $"{Result}";
							while (--drawSize > 0)
								equation += routineText;
							_term = new Calculator(equation, _powerData).Result - Result;
						}
					}
#if Routines
					else
					{
						GetOperator();
						if (@operator is not Operators.Becomes)
							throw new InvalidOperationException("Routine definition not followed by Becomes operator.");
						if (!GetAlias(out var routineName))
							throw new InvalidOperationException("Routine definition missing required routine name.");
						if (ReservedAliases.Contains(routineName))
							throw new InvalidOperationException("Attempt to redefine reserved alias as routine name.");
						if (Formula.FirstOrDefault() is not Semicolon)
							throw new InvalidOperationException("Routine definition missing required terminal semicolon.");
						DropCount();
						//	TODO: Throw exception if the Formula is empty after semicolons and whitespace removed.
						@operator = Operators.Reset;
						Routines[routineName] = routineText;
					}
#endif
					return;
				default:
					throw new InvalidOperationException($"Unrecognized formula term: {_formula.Split().First()}");
				}

				static int AdjustEmbedCount(char ch,
											char open,
											char close)
					=> ch == open ? +1 : ch == close ? -1 : 0;

				double GetUnaryTerm()
				{
					DropCount();
					if (_formula.Length is 0)
						throw new InvalidOperationException("Invalid unary operator in formula.");
					GetTerm();
					return _term;
				}

				bool GetAlias(out string alias)
				{
					alias = Alias().Match(_formula)
								   .Value;
					if (alias.Length is 0)
						return false;
					DropText(alias);
					return true;
				}

				bool GetNumericTerm()
				{
					var stringTerm = Number().Match(_formula)
											 .Value;
					if (stringTerm.Length is 0)
						return false;
					_term = stringTerm.AsDouble();
					DropText(stringTerm);
					return true;
				}
			}
		}
	}
}
