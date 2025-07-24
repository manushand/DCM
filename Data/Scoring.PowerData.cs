namespace Data;

using static GamePlayer;

public sealed partial class Scoring
{
	[PublicAPI]
	public sealed class PowerData
	{
		private readonly int? _centers;
		private readonly int? _years;
		private readonly double? _other;
		private readonly Results? _result;
		private readonly Scoring _scoring;

		private double? _playerAnte;
		private double? _provisionalScore;

		private Results Result => _result ?? throw ScoringException(nameof (Result));

		public Powers Power { get; }

		public int Centers => _centers ?? throw ScoringException(nameof (Centers));
		public int Years => _years ?? throw ScoringException(nameof (Years));

		public double OtherScore => _other ?? throw ScoringException();

		public double ProvisionalScore
		{
			get => _provisionalScore ?? throw ScoringException(nameof (ProvisionalScore));
			internal set => _provisionalScore = value;
		}

		public double PlayerAnte
		{
			get => _playerAnte ?? throw ScoringException(nameof (PlayerAnte));
			internal set => _playerAnte = value;
		}

		public double RunningScore { get; }
		public double AverageGameScore { get; }

		public bool Won => Result is Win;
		public bool Lost => Result is Loss;
		public bool Eliminated => Centers is 0;
		public bool Uneliminated => !Eliminated;
		public bool Survived => Lost && Uneliminated;
		public bool WonAlone => Won && _scoring.Winners is 1;
		public bool WonSolo => Won && Centers > 17;
		public bool WonConcession => WonAlone && Centers < 18;
		public bool WonDraw => Won && _scoring.Winners > 1;
		public bool WasLeader => Centers == _scoring.LeaderCenters;
		public bool SurvivedSolo => Survived && _scoring.LeaderCenters > 17;
		public bool SurvivedDraw => Lost && _scoring.Winners > 1;
		public bool SurvivedConcession => Survived && _scoring.Winners is 1 && _scoring.LeaderCenters < 18;
		public bool Conceded => SurvivedConcession;
		public bool LostToSoleWinner => Survived && _scoring.Winners > 1;

		public int CentersSquared => Centers * Centers;
		public int WorstCenterRank => _scoring.Powers.Count(power => power.Value.Centers >= Centers);   //	This lambda is NOT repeated in the next property
		public int BestCenterRank => _scoring.Powers.Count(power => power.Value.Centers > Centers) + 1;
		public int CenterRankSharers => _scoring.Powers.Count(power => power.Value.Centers == Centers);
		public int WorstSurvivorRank => Survived
											? _scoring.Winners + _scoring.Powers.Count(power => power.Value.Survived
																						     && power.Value.Centers >= Centers)
											: 0; //	This lambda is NOT repeated in the next property
		public int BestSurvivorRank => Survived
										   ? _scoring.Winners + _scoring.Powers.Count(power => power.Value.Survived
                                                                                            && power.Value.Centers > Centers) + 1
										   : 0;
		public int SurvivorRankSharers => _scoring.Powers.Count(power => power.Value.BestSurvivorRank == BestSurvivorRank);
		public int EliminatedEarlier => OrderOfElimination(EliminationOrderType.Earlier);
		public int BestEliminationOrder => OrderOfElimination(EliminationOrderType.Best);
		public int WorstEliminationOrder => OrderOfElimination(EliminationOrderType.Worst);
		public int EliminationOrderSharers => _scoring.Powers.Count(power => power.Value.BestEliminationOrder == BestEliminationOrder);

		public double CenterRank => (double)(WorstCenterRank + BestCenterRank) / 2;
		public double SurvivorRank => (double)(BestSurvivorRank + WorstSurvivorRank) / 2;
		public double EliminationOrder => (double)(BestEliminationOrder + WorstEliminationOrder) / 2;

		internal PowerData(Scoring scoring,
						   GamePlayer gamePlayer)
		{
			_scoring = scoring;
			Power = gamePlayer.Power;
			if (Power is TBD)
				throw new ArgumentException("The gamePlayer Power is not established", nameof (gamePlayer)); //	TODO
			var system = _scoring.ScoringSystem;
			if (system.UsesGameResult && gamePlayer.Result > Unknown)
				_result = gamePlayer.Result;
			if (system.UsesCenterCount)
				_centers = gamePlayer.Centers;
			if (system.UsesYearsPlayed)
				_years = gamePlayer.Years;
			if (system.UsesOtherScore)
				_other = gamePlayer.Other;
			//	ScoringSystem test games are Game.None
			if (gamePlayer.Game.IsNone)
				return;
			var game = gamePlayer.Game;
			RunningScore = game.PreGameScore(gamePlayer);
			AverageGameScore = game.Tournament.IsEvent
								   ? game.Round.PreGameAverage(gamePlayer)
								   : game.PreGameScore(gamePlayer);
		}

		/// <summary>
		///     Returns an integer giving a specific flavor of the player's elimination order.
		/// </summary>
		/// <param name="type">
		///     Determines which result is returned for an eliminated player.
		/// </param>
		/// <returns>
		///     0 if the player's result was a Win, the number of non-winners if the player survived,
		///     and a specific result (based on the parameter sent in) if the player was eliminated:
		///     if type is Earlier => number of players eliminated before this player's final year
		///     if type is Best => one-greater than the result returned when the type is Earlier
		///     if type is Worst => number of players eliminated before OR ON this player's final year.
		/// </returns>
		private int OrderOfElimination(EliminationOrderType type)
			=> Won ? 0
				   : Uneliminated ? 7 - _scoring.Winners
								  : _scoring.Powers
											.Count(power => power.Value.Eliminated
														 && power.Value.Years.CompareTo(Years) < (type is EliminationOrderType.Worst).AsInteger())
									+ (type is EliminationOrderType.Best).AsInteger();

		private enum EliminationOrderType : byte
		{
			Earlier,
			Best,
			Worst
		}
	}
}
