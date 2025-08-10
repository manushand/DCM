namespace Data;

public sealed class GamePlayer : LinkRecord, IInfoRecord, IComparable<GamePlayer>
{
	#region Enumerations

	public enum Powers : sbyte
	{
		//	IMPORTANT: Values must be -1 through 6
		TBD = -1,
		Austria = 0,
		England = 1,
		France = 2,
		Germany = 3,
		Italy = 4,
		Russia = 5,
		Turkey = 6
	}

	public enum Results : sbyte
	{
		//	IMPORTANT: Must be -1, 0, 1 to match ComboBox item order
		Unknown = -1,
		Loss = 0,
		Win = 1
	}

	#endregion

	#region Fields

	public int? Centers;
	public int? Years;
	public double Other;
	public double PlayerAnte;
	public Powers Power;
	public Results Result;

	private double? _finalScore;

	#endregion

	#region Properties

	public string PowerName => Power.InCaps;

	[UsedImplicitly]
	public string Status => Game.Status switch
							{
								Seeded => Empty,
								Underway => PlayComplete
												? "⬤"
												: "◯",
								Finished => "✔", // or ✓ or ✅
								_        => throw new ArgumentOutOfRangeException(nameof (Game.Status), Game.Status, "Invalid Game Status")
							};

	public int GameId { get; private set; }

	public Game Game
	{
		get => field.Id == GameId
				   ? field
				   : field = ReadById<Game>(GameId);
		set => (field, GameId) = (value, value.Id);
	} = Game.None;

	public double FinalScore
	{
		get
		{
			//	TODO: Changing this to if (!Game.Scored) causes a stack overflow
			if (_finalScore is null)
				Game.CalculateScores();
			return _finalScore ?? default;
		}
		internal set
		{
			_finalScore = value;
			if (!Game.IsNone) // Test Games will be Game.None here
				Game.Tournament
					.Rounds
					.Skip(Game.Round.Number)
					.ForEach(round => round.ClearPreRoundScore(Player));
		}
	}

	public double ProvisionalScore { get; internal set; }

	public bool PlayComplete => !PlayIncomplete;

	private bool PlayIncomplete => Power is TBD
								|| Game.ScoringSystem.UsesGameResult && Result is Unknown
								|| Game.ScoringSystem.UsesCenterCount && Centers is null
								|| Game.ScoringSystem.UsesYearsPlayed && Years is null;

	private Tournament Tournament => Game.Tournament;

	#endregion

	#region IComparable interface implementation

	//	TODO: In C# 11, the parameter can be "other!!" and the null check/throw removed
	public int CompareTo(GamePlayer? other)
		=> other is null
			   ? throw new ()
			   : (GameId | other.GameId) is 0
				   ? Power.CompareTo(other.Power)
				   : (Game.Number, Power, Player.Name).CompareTo((other.Game.Number, other.Power, other.Player.Name));

	#endregion

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	private const string LinkKeyFormat = $"[{nameof (GameId)}] = {{0}}";

	protected override string LinkKey => Format(LinkKeyFormat, GameId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<GamePlayer>();
		GameId = record.Integer(nameof (GameId));
		PlayerId = record.Integer(nameof (PlayerId));
		Power = record.IntegerAs<Powers>(nameof (Power));
		Result = record.IntegerAs<Results>(nameof (Result));
		Years = record.NullableInteger(nameof (Years));
		Centers = record.NullableInteger(nameof (Centers));
		Other = record.Double(nameof (Other));
		return this;
	}

	#endregion

	//	The PlayerId usually won't change, but in the case of the ReplaceButton it will, so it must be listed.
	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (PlayerId)}}] = {0},
	                                           [{{nameof (Power)}}] = {1},
	                                           [{{nameof (Result)}}] = {2},
	                                           [{{nameof (Years)}}] = {3},
	                                           [{{nameof (Centers)}}] = {4},
	                                           [{{nameof (Other)}}] = {5}
	                                           """;

	public string FieldValues => Format(FieldValuesFormat,
										PlayerId,
										Power.ForSql(),
										Result.ForSql(),
										Years.ForSql(),
										Centers.ForSql(),
										Other);

	#endregion

	#region Seeding data and code

	private int? _roundNumber;
	private int? _conflict;

	public int Conflict
	{
		get => _conflict ??= CalculateConflict();
		private set => _conflict = value;
	}

	public List<string> ConflictDetails { get; } = [];

	private List<PlayerConflict> PlayerConflicts { get; } = [];
	private List<Powers> PowersPlayedInTournament { get; } = [];
	private List<int> PlayerIdsPlayedInTournament { get; } = [];
	private List<int> TournamentTeamPlayerIds { get; } = [];
	private Dictionary<Group, int[]> PlayerGroups { get; } = [];

	//	It is VERY important to pre-set all these things before seeding since they
	//	will be re-referenced a lot during seeding when swaps are made and unmade.
	public GamePlayer PrepareForSeeding()
	{
		Conflict = default;
		ConflictDetails.Clear();
		_roundNumber = Game.Round
						   .Number;
		PlayerConflicts.FillWith(Player.PlayerConflicts);
		var tournamentId = Tournament.Id;
		GamePlayer[] gamePlayersInTournamentGames = [..Player.Games
															 .Where(game => game.Round.TournamentId == tournamentId
																		 && game.Round.Number < _roundNumber)
															 .SelectMany(static game => game.GamePlayers)];
		PowersPlayedInTournament.FillWith(gamePlayersInTournamentGames.WithPlayerId(PlayerId)
																	  .Select(static gamePlayer => gamePlayer.Power)
																	  .Where(static power => power is not TBD));
		PlayerIdsPlayedInTournament.FillWith(gamePlayersInTournamentGames.Select(static gamePlayer => gamePlayer.PlayerId)
																		 .Where(id => id != PlayerId)); //  Not strictly needed
		TournamentTeamPlayerIds.FillWith(Player.TournamentTeamPlayers(Tournament)
											   .Ids());
		PlayerGroups.Clear();
		Player.Groups
			  .ForEach(group => PlayerGroups.Add(group, [..group.Players.Ids()]));
		return this;
	}

	public int CalculateConflict(bool fillDetails = false)
		=> CalculateConflict(Game.GamePlayers, fillDetails);

	internal int CalculateConflict(IEnumerable<GamePlayer> seeding,
								   bool fillDetails = false)
	{
		if (_roundNumber is null)
			PrepareForSeeding();
		if (fillDetails)
			ConflictDetails.Clear();
		Player[] opponents = [..seeding.Where(gamePlayer => gamePlayer.GameId == GameId
														 && gamePlayer.PlayerId != PlayerId)
									   .Select(static gamePlayer => gamePlayer.Player)];
		int[] opponentIds = [..opponents.Ids()];

		//	Player-Personal conflicts
		Conflict = PlayerConflicts.Where(playerConflict => playerConflict.ConflictedPlayerIds.Any(opponentIds.Contains))
								  .DefaultIfEmpty()
								  .Sum(playerConflict =>
									   {
										   var conflict = playerConflict?.Value ?? 0;
										   if (fillDetails && playerConflict is not null)
											   ConflictDetails.Add($"{conflict.Points} for player conflict with {playerConflict.PlayerConflictedWith(PlayerId)}");
										   return conflict;
									   });

		//	Player-Group Conflicts
		//	These are applied only in the first round if the tournament uses a score conflict.
		if (Tournament.ScoreConflict is 0 || _roundNumber is 1)
			foreach (var (group, memberIds) in PlayerGroups)
				opponentIds.ForSome(memberIds.Contains,
									opponent =>
									{
										var player = group.Players.Single(player => player.Id == opponent);
										var conflict = group.Conflict;
										Conflict += conflict;
										if (fillDetails)
											ConflictDetails.Add($"{conflict.Points} for being in the group {group} with {player}.");
									});

		//	Powers-Played-Earlier Conflicts
		if (Power is not TBD)
		{
			Powers[] powerConflicts = [..PowersPlayedInTournament.Where(power => Tournament.GroupPowers.GroupSharedBy(power, Power))];
			if (powerConflicts.Length is not 0)
				Conflict += powerConflicts.Distinct()
										  .Sum(power =>
											   {
												   var times = powerConflicts.Count(which => power == which);
												   var conflict = Tournament.PowerConflict * times;
												   //	The worst thing in the world is to play the SAME power.  Repeating
												   //	play in a Power GROUP is unavoidable, but repeating the same POWER
												   //	is unforgivable.  Make sure that is graded SEVEN TIMES WORSE.
												   if (power == Power && Tournament.GroupPowers is not None)
													   conflict *= 7;
												   if (fillDetails)
													   ConflictDetails.Add($"{conflict.Points} for playing {power} {ThisMany(times)} earlier{
														   (power == Power ? null : $" (same power group as {Power})")}.");
												   return conflict;
											   });
		}

		//	Players-Played-Earlier Conflicts
		var playedAgainstBefore = opponentIds.Where(PlayerIdsPlayedInTournament.Contains);
		Conflict += playedAgainstBefore.Sum(opponentId =>
											{
												var times = PlayerIdsPlayedInTournament.Count(playerId => playerId == opponentId);
												var conflict = Tournament.PlayerConflict * times;
												if (fillDetails)
													ConflictDetails.Add($"{conflict.Points} for playing {
														opponents.Single(player => player.Id == opponentId)} {ThisMany(times)} earlier.");
												return conflict;
											});

		//	Tournament Team Member Conflicts
		if (Tournament.HasTeamTournament
		&& (_roundNumber == Tournament.TeamRound || Tournament.TeamsPlayMultipleRounds))
		{
			var teamMembers = opponentIds.Where(TournamentTeamPlayerIds.Contains);
			Conflict += teamMembers.Sum(opponentId =>
										{
											var conflict = Tournament.TeamConflict;
											if (fillDetails)
												ConflictDetails.Add($"{conflict.Points} for playing on a team with {
													opponents.Single(player => player.Id == opponentId)}.");
											return conflict;
										});
		}

		//	Score Conflicts
		if (Tournament.ScoreConflict > 0 && _roundNumber > 1)
		{
			var divisor = Tournament.ScoreConflict;
			if (Tournament.ProgressiveScoreConflict)
				divisor /= _roundNumber.Value - 1;
			if (divisor < 1)
				divisor = 1;
			var distanceFromAverage = Math.Abs(Game.AveragePreGameScore - Game.PreGameScore(this));
			var conflict = (int)distanceFromAverage / divisor;
			if (conflict > 0)
			{
				Conflict += conflict;
				if (fillDetails)
					ConflictDetails.Add($"{conflict.Points} for being {distanceFromAverage:F2} tournament points off average for game's players.");
			}
		}

		if (fillDetails && ConflictDetails.Count is 0)
			ConflictDetails.Add("No conflicts.");

		return Conflict;

		static string ThisMany(int times)
			=> times switch
			   {
				   1 => "once",
				   2 => "twice",
				   _ => $"{times} times"
			   };
	}

	#endregion
}
