﻿namespace Data;

public sealed class Round : IdentityRecord<Round>
{
	/// <summary>
	///     Holds calculated scores (sum of scores in prior rounds of the same tournament) for a given round.
	///     These calculations are held in this cache because they are expensive and the result often requested
	///     during seeding.
	/// </summary>
	private readonly Dictionary<int, List<double>> _preRoundGames = [];

	private int? _scoringSystemId;

	public int Number;

	internal int TournamentId { get; private set; }

	public override string Name => $"{Number}";

	public Game[] SeededGames => [..Games.Where(static game => game.Status is Seeded)];

	public Game[] StartedGames => [..Games.Where(static game => game.Status is not Seeded)];

	public Game[] FinishedGames => [..Games.Where(static game => game.Status is Finished)];

	public bool GamesSeeded => SeededGames.Length is not 0;
	public bool GamesStarted => StartedGames.Length is not 0;
	public Game.Statuses Status => GamesSeeded
									   ? Seeded
									   : Games.Any(static game => game.Status is Underway)
										   ? Underway
										   : Finished;

	//	TODO - It may be useful to have Workable return true if a specific Setting is set,
	//	TODO - allowing ALL rounds to be workable, even when finished
	public bool Workable => Number == (Tournament.Rounds.Length is 0 ? default : Tournament.Rounds.Max(static round => round.Number))
                         && (Number < Tournament.TotalRounds || Games.Any(static game => game.Status is not Finished))
                         || Games.Length is 0;

	public int ScoringSystemId => _scoringSystemId ?? Tournament.ScoringSystemId;

	public ScoringSystem ScoringSystem
	{
		get => field.Id == ScoringSystemId
				   ? field
				   : field = ReadById<ScoringSystem>(ScoringSystemId);
		set
		{
			field = value;
			_scoringSystemId = value.Id == Tournament.ScoringSystemId
								   ? null
								   : value.Id;
			Games.ForSome(game => game.ScoringSystem.Id == ScoringSystemId,
						  game => game.ScoringSystem = value);
		}
	} = ScoringSystem.None;

	public bool ScoringSystemIsDefault => _scoringSystemId is null;

	public Tournament Tournament
	{
		get => field.Id == TournamentId
				   ? field
				   : field = ReadById<Tournament>(TournamentId);
		internal init => (field, TournamentId) = (value, value.Id);
	} = Tournament.None;

	public Game[] Games => [..ReadMany<Game>(game => game.RoundId == Id).OrderBy(static game => game.Number)];

	public RoundPlayer[] RoundPlayers => [..ReadMany<RoundPlayer>(roundPlayer => roundPlayer.RoundId == Id)];

	public int Conflict => Games.Sum(static game => game.Conflict);

	public void AddPlayer(Player player)
		=> CreateOne(new RoundPlayer { Round = this, Player = player });

	public int Seed(List<RoundPlayer> roundPlayers,
					bool assignPowers)
	{
		if (roundPlayers.Count % 7 is not 0)
			throw new ArgumentOutOfRangeException(nameof (roundPlayers), "Invalid number of roundPlayers (must be multiple of 7)");

		StartTransaction();

		//	Create the Game objects and store them in the database.
		var lastSeededGameNumber = Games.Length;
		var games = CreateMany(Range(1, roundPlayers.Count / 7).Select(number => new Game
																				 {
																					 Number = lastSeededGameNumber + number,
																					 Round = this,
																					 Status = Seeded
																				 }));
		//	Adding another .ToArray() here (before that last semicolon) is a 5-15% performance hit.  I know, right?

		//	Create all the GamePlayer objects, seeding players into them from best
		//	score to worst (if the Tournament uses a ScoreConflict) or randomly
		//	(if it doesn't). Do not store them in the database yet, though.  Yes,
		//	doing it by score really does improve performance (by about a third).
		//	Note that doing it this way should, I think, mean that the "best" games
		//	(having players with the best scores) should be the lower-numbered games.
		List<GamePlayer> gamePlayers = [];
		using (var seedingPlayers = roundPlayers.OrderByDescending(roundPlayer => Tournament.ScoreConflict is 0
																					  ? RandomNumber()
																					  : PreRoundScore(roundPlayer.Player))
												.ThenBy(static _ => RandomNumber())
												.GetEnumerator())
			foreach (var game in games)
				for (var power = Austria; power <= Turkey; ++power)
				{
					seedingPlayers.MoveNext();
					var roundPlayer = seedingPlayers.Current
													.OrThrow();
					roundPlayers.Remove(roundPlayer);
					gamePlayers.Add(new ()
									{
										Game = game,
										Power = assignPowers
													? power
													: TBD,
										Player = roundPlayer.Player,
										Result = Unknown
									});
				}

		/*  TODO: I see no reason why I was making a copy of the gamePlayers List and using it instead below.
		var seedList = [..gamePlayers];
		*/

		//	Now add in for optimization all the existing players in seeded but not started games

		var preSeeded = SeededGames.SelectMany(static game => game.GamePlayers)
								   .ToArray();
		gamePlayers.AddRange(preSeeded);
		Delete(preSeeded);

		//	Create the list of seeding GamePlayers and prepare them all for seeding
		var seeding = gamePlayers.Select(static gamePlayer => gamePlayer.PrepareForSeeding())
								 .ToArray();

		//	TODO: here is where we could maybe CalculateConflict(seeding) in order to
		//	TODO: let the user decline optimization or know how much it improves things

		var finalConflict = Optimize(seeding);

		//	Store the GamePlayer objects in the database.
		CreateMany(seeding);

		EndTransaction();

		//	Return the total conflict for all newly seeded GamePlayers
		return finalConflict;

		static int Optimize(GamePlayer[] seeding)
		{
			//	All we REALLY know (without going through seeding) is
			//	that if there are no possible negative conflicts, and
			//	if the total conflict for a seeding attempt hits zero,
			//	it's the best possible; we can stop trying to improve
			//	it.  Even when there are possible negatives, zero may
			//	be impossible (if all positives cannot be eliminated).
			//	In that case, and in the cases when there ARE possible
			//	negative conflicts, we cannot easily predetermine the
			//	lowest possible total conflict score, so full seeding
			//	needs to take place.  (Full-seeding more than a dozen
			//	games seems to take only about eight seconds.)
			var canBeatZero = seeding.SelectMany(static gamePlayer => gamePlayer.Player.PlayerConflicts)
									 .Distinct()
									 .Any(playerConflict => playerConflict.Value < 0 && playerConflict.ConflictedPlayerIds.All(SeedingIds))
						   || seeding.SelectMany(static gamePlayer => gamePlayer.Player.Groups)
									 .Distinct()
									 .Any(game => game.Conflict < 0 && game.Players.Ids().Count(SeedingIds) > 1);

			//	Get the total to beat.
			var totalConflict = CalculateConflict();

			//	Order the GamePlayers from worst Conflicts to best and loop through
			//	them, swapping each worse one with progressively better ones, starting
			//	over every time a swap improves the total conflicts to beat.
			var playerCount = seeding.Length;
			var swapperCount = playerCount - 1;
			for (var lastSeeded = 0; lastSeeded < swapperCount; ++lastSeeded)
			{
				//	If we're sure that it's as good as it can get, we're done here.
				if (totalConflict is 0 && !canBeatZero)
					break;
				//	When beginning (or re-beginning) at the top of the list,
				//	ensure that it is ordered from most conflict to least.
				if (lastSeeded is 0)
					seeding = [..seeding.OrderByDescending(static gamePlayer => gamePlayer.Conflict)];
				for (var swapWith = lastSeeded + 1; swapWith < playerCount; ++swapWith)
				{
					//	Swap the game/power assignments of the [lastSeeded] and [swapWith]
					//	players and see if this improves the total conflict or not. Ties go
					//	to the runner (i.e., if the swap is no better, keep what we have).
					var updatedConflict = SwapSeeders(lastSeeded, swapWith);
					if (updatedConflict < totalConflict)
					{
						//	The swap improved things.  Keep it.
						//	Reorder the GamePlayers and start again
						//	hoping to only make things even better.
						totalConflict = updatedConflict;
						//	TODO: Do we really need to start completely over?
						lastSeeded = -1;
						break;
					}

					//	The swap didn't help; undo it, and we'll try swapping with the next
					//	one down the list. (This is necessary even if the swap didn't hurt.)
					SwapSeeders(lastSeeded, swapWith);
				}
			}

			return totalConflict;

			bool SeedingIds(int i)
				=> seeding.Select(static gamePlayer => gamePlayer.PlayerId)
						  .Contains(i);

			int SwapSeeders(int lastSeeded,
							int swapWith)
			{
				//	It's tempting to change the PlayerIds instead of both the Power and GameId.
				//	Don't fall for that (again), though; pre-calculated conflict-helping data
				//	in the GamePlayer object is specific to its .Player so the .Player needs
				//	to stay the same inside that object while the other things change.
				var (current, swapper) = (seeding[lastSeeded], seeding[swapWith]);
				(current.Power, swapper.Power) = (swapper.Power, current.Power);
				(current.Game, swapper.Game) = (swapper.Game, current.Game);

				//	This swap will affect one or two games worth of players;
				//	recalculate their Conflicts and get the updated total.
				return CalculateConflict(swapper.GameId, current.GameId);
			}

			//	This doesn't get the optimal seeding if the GamePlayer objects,
			//	instead of their .GameId values, are passed in.  I tested it.
			int CalculateConflict(params int[] gameIds)
				=> seeding.Sum(gamePlayer => gameIds.Length is 0 || gameIds.Contains(gamePlayer.GameId)
												 ? gamePlayer.CalculateConflict(seeding)
												 : gamePlayer.Conflict);
		}
	}

	private List<double> PriorGameScores(GamePlayer gamePlayer)
		=> _preRoundGames.GetOrSet(gamePlayer.PlayerId,
								   playerId =>
								   {
									   var roundsPrior = Tournament.IsEvent
															 ? Number - 1
															 : 1;
									   var scores = Tournament.Rounds
															  .Take(roundsPrior)
															  .SelectMany(static r => r.FinishedGames)
															  .Where(game => (Tournament.IsEvent || game.Date < gamePlayer.Game.Date)
																		  && game.GamePlayers.HasPlayerId(playerId))
															  .Select(game => game.GamePlayers.ByPlayerId(playerId).FinalScore)
															  //	Leave this .DefaultIfEmpty; it's important that at least one score is in the List
															  .DefaultIfEmpty(Tournament.UnplayedScore)
															  .ToList();
									   while (scores.Count < roundsPrior)
										   scores.Add(Tournament.UnplayedScore);
									   return scores;
								   });

	/// <summary>
	///     Returns the average of a player's game scores from all prior rounds in
	///     a tournament (or finished Group games), calculating (then caching) it if
	///     necessary.
	/// </summary>
	/// <param name="gamePlayer">The player whose average score is being requested.</param>
	/// <returns>The average of the player's game scores in all prior tournament rounds.</returns>
	internal double PreGameAverage(GamePlayer gamePlayer)
		=> PriorGameScores(gamePlayer).Average();

	/// <summary>
	///     Returns the sum of a player's scores from all prior rounds in
	///     the tournament or, if this is a Group, the player's group rating,
	///     calculating (then caching) it if necessary.
	/// </summary>
	/// <param name="gamePlayer">The player whose score is being requested.</param>
	/// <returns>The sum of the player's game scores in all prior tournament rounds.</returns>
	public double PreRoundScore(GamePlayer gamePlayer)
		=> PriorGameScores(gamePlayer).Sum();

	public double PreRoundScore(Player player)
		=> PriorGameScores(new () { Player = player }).Sum();

	/// <summary>
	///     When a GamePlayer.FinalScore is changed for a game in a prior round in this Round's
	///     Tournament, this method should be called to ensure that the next time a RunningScore
	///     is needed for this player, it is recalculated.
	/// </summary>
	/// <param name="player">The player whose FinalScore is changing.</param>
	internal void ClearPreRoundScore(Player player)
		=> _preRoundGames.Remove(player.Id);

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<Round>();
		Id = record.Integer(nameof (Id));
		Number = record.Integer(nameof (Number));
		TournamentId = record.Integer(nameof (TournamentId));
		_scoringSystemId = record.NullableInteger(nameof (ScoringSystemId));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (Number)}}] = {0},
	                                           [{{nameof (TournamentId)}}] = {1},
	                                           [{{nameof (ScoringSystemId)}}] = {2}
	                                           """;

	public override string FieldValues => Format(FieldValuesFormat,
												 Number,
												 TournamentId,
												 _scoringSystemId.ForSql());

	#endregion
}
