namespace DCM;

public sealed partial class Scoring
{
	[PublicAPI]
	public sealed class PowerData
	{
		private readonly int? _centers;
		private readonly decimal? _other;
		private readonly Results? _result;
		private readonly int? _years;
		private decimal? _playerAnte;
		private decimal? _provisionalScore;
		private Scoring Scoring { get; }
		public PowerNames Power { get; }

		public int Centers => _centers.OrThrow("Invalid reference to center count.");
		public int Years => _years.OrThrow("Invalid reference to years played.");

		public decimal OtherScore => _other.OrThrow("Invalid reference to other score.");

		public decimal ProvisionalScore
		{
			get => _provisionalScore.OrThrow("Invalid reference to provisional score.");
			internal set => _provisionalScore = value;
		}

		public decimal PlayerAnte
		{
			get => _playerAnte.OrThrow("Invalid reference to player ante.");
			internal set => _playerAnte = value;
		}

		public decimal RunningScore { get; }
		public decimal AverageGameScore { get; }

		public bool Won => Result is Win;
		public bool Lost => Result is Loss;
		public bool Eliminated => Centers is 0;
		public bool Uneliminated => !Eliminated;
		public bool Survived => Lost && Uneliminated;
		public bool WonAlone => Won && Scoring.Winners is 1;
		public bool WonSolo => Won && Centers > 17;
		public bool WonConcession => WonAlone && Centers < 18;
		public bool WonDraw => Won && Scoring.Winners > 1;
		public bool WasLeader => Centers == Scoring.LeaderCenters;
		public bool SurvivedSolo => Survived && Scoring.LeaderCenters > 17;
		public bool SurvivedDraw => Lost && Scoring.Winners > 1;
		public bool SurvivedConcession => Survived && Scoring.Winners is 1 && Scoring.LeaderCenters < 18;
		public bool Conceded => SurvivedConcession;

		public int CentersSquared => Centers * Centers;
		public int WorstCenterRank => Scoring.Powers.Count(power => power.Value.Centers >= Centers); //	This lambda is NOT repeated in the next property
		public int BestCenterRank => Scoring.Powers.Count(power => power.Value.Centers > Centers) + 1;
		public int CenterRankSharers => Scoring.Powers.Count(power => power.Value.Centers == Centers);

		public int WorstSurvivorRank => Survived
											? Scoring.Winners + Scoring.Powers.Count(power => power.Value.Survived
																						   && power.Value.Centers >= Centers)
											: 0; //	This lambda is NOT repeated in the next property

		public int BestSurvivorRank => Survived
										   ? Scoring.Winners + Scoring.Powers.Count(power => power.Value.Survived
																						  && power.Value.Centers > Centers) + 1
										   : 0;

		public int SurvivorRankSharers => Scoring.Powers.Count(power => power.Value.BestSurvivorRank == BestSurvivorRank);
		public int EliminatedEarlier => OrderOfElimination(EliminationOrderType.Earlier);
		public int BestEliminationOrder => OrderOfElimination(EliminationOrderType.Best);
		public int WorstEliminationOrder => OrderOfElimination(EliminationOrderType.Worst);
		public int EliminationOrderSharers => Scoring.Powers.Count(power => power.Value.BestEliminationOrder == BestEliminationOrder);

		public decimal CenterRank => (WorstCenterRank + BestCenterRank) / 2m;
		public decimal SurvivorRank => (BestSurvivorRank + WorstSurvivorRank) / 2m;
		public decimal EliminationOrder => (BestEliminationOrder + WorstEliminationOrder) / 2m;
		private Results Result => _result.OrThrow("Invalid reference to win/loss result.");

		internal PowerData(Scoring scoring,
						   GamePlayer gamePlayer)
		{
			Scoring = scoring;
			Power = gamePlayer.Power;
			if (Power is TBD)
				throw new ArgumentException("The gamePlayer Power is not established", nameof (gamePlayer)); //	TODO
			var system = Scoring.ScoringSystem;
			if (system.UsesGameResult && gamePlayer.Result > Unknown)
				_result = gamePlayer.Result;
			if (system.UsesCenterCount)
				_centers = gamePlayer.Centers;
			if (system.UsesYearsPlayed)
				_years = gamePlayer.Years;
			if (system.UsesOtherScore)
				_other = gamePlayer.Other;
			//	ScoringSystem test games have a GameId of 0
			if (gamePlayer.GameId is 0)
				return;
			var game = gamePlayer.Game;
			RunningScore = game.PreGameScore(gamePlayer);
			AverageGameScore = game.Tournament.Group is null
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
		///     if type is Best => one-greater than the result returned if type is 0
		///     if type is Worst => number of players eliminated before OR ON this player's final year.
		/// </returns>
		private int OrderOfElimination(EliminationOrderType type)
			=> Won            ? 0
			   : Uneliminated ? 7 - Scoring.Winners
								: Scoring.Powers
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
