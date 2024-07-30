namespace DCM.DB;

[PublicAPI]
internal sealed class GamePlayer : LinkRecord, IInfoRecord, IComparable<GamePlayer>
{
	internal enum Results : sbyte
	{
		//	IMPORTANT: Must be -1, 0, 1 to match ComboBox item order
		Unknown = -1,
		Loss = 0,
		Win = 1
	}

	internal int? Centers;
	internal decimal Other;
	internal decimal PlayerAnte;
	internal PowerNames Power;
	internal decimal ProvisionalScore;
	internal Results Result;
	internal int? Years;

	private decimal? _finalScore;
	private Game? _game;
	private Tournament? _tournament;

	[DisplayName(nameof (Game))]
	public int GameNumber => Game.Number;

	[DisplayName(nameof (Player))]
	public Player DisplayPlayer => Player;

	[DisplayName(nameof (Power))]
	public string PowerName => Power.InCaps();

	public string Status => Game.Status switch
							{
								Seeded => Empty,
								Underway => PlayComplete
												? "⬤"
												: "◯",
								Finished => "✔", // or ✓ or ✅
								_        => throw new ArgumentOutOfRangeException() //	TODO
							};

	internal int GameId { get; private set; }

	internal Game Game
	{
		get => _game ??= ReadById<Game>(GameId).OrThrow();
		set => (_game, GameId) = (value, value.Id);
	}

	internal decimal FinalScore
	{
		get
		{
			//	TODO: Changing this to if (!Game.Scored) causes a stack overflow
			if (_finalScore is null)
				Game.CalculateScores();
			return _finalScore ?? 0;
		}
		set
		{
			_finalScore = value;
			if (_game is not null) //	ScoringSystem test games will be null here
				Game.Tournament.Rounds.Skip(Game.Round.Number).ForEach(round => round.ClearPreRoundScore(Player));
		}
	}

	internal bool PlayComplete => !PlayIncomplete;

	private bool PlayIncomplete => Power is TBD
								|| Game.ScoringSystem.UsesGameResult && Result is Unknown
								|| Game.ScoringSystem.UsesCenterCount && Centers is null
								|| Game.ScoringSystem.UsesYearsPlayed && Years is null;

	private Tournament Tournament => _tournament ??= Game.Tournament;

	#region IComparable interface implementation

	//	TODO: In C# 11, the parameter can be "other!!" and the null check/throw removed
	public int CompareTo(GamePlayer? other)
		=> other is null
			   ? throw new ()
			   : (GameId | other.OrThrow().GameId) is 0
				   ? Power.CompareTo(other.Power)
				   : (GameNumber, Power, Player.Name).CompareTo((other.GameNumber, other.Power, other.Player.Name));

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
		Power = record.IntegerAs<PowerNames>(nameof (Power));
		Result = record.IntegerAs<Results>(nameof (Result));
		Years = record.NullableInteger(nameof (Years));
		Centers = record.NullableInteger(nameof (Centers));
		Other = record.Decimal(nameof (Other));
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

	[Browsable(false)]
	public string FieldValues => Format(FieldValuesFormat,
										PlayerId,
										Power.ForSql(),
										Result.ForSql(),
										Years.ForSql(),
										Centers.ForSql(),
										Other);

	#endregion

	#region Seeding data and code

	private int? _conflict;

	internal int Conflict
	{
		//	BUG: https://youtrack.jetbrains.com/issue/RSRP-475010 but R# is actually correct, viz.:
		//	BUG: https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-04-15.md#result-type-of--expression
		get => _conflict ??= CalculateConflict();
		private set => _conflict = value;
	}

	internal readonly List<string> ConflictDetails = [];

	private int? _roundNumber;

	private List<PlayerConflict> PlayerConflicts { get; } = [];

	private List<PowerNames> PowersPlayedInTournament { get; } = [];

	private List<int> PlayerIdsPlayedInTournament { get; } = [];

	private List<int> TournamentTeamPlayerIds { get; } = [];

	private Dictionary<Group, int[]> PlayerGroups { get; } = [];

	//	It is VERY important to pre-set all these things before seeding since they
	//	will be re-referenced a lot during seeding when swaps are made and unmade.
	internal GamePlayer PrepareForSeeding()
	{
		Conflict = 0;
		ConflictDetails.Clear();
		_roundNumber = Game.Round
						   .Number;
		PlayerConflicts.FillWith(Player.PlayerConflicts);
		var tournamentId = Tournament.Id;
		var gamePlayersInTournamentGames = Player.Games
												 .Where(game => game.Round.TournamentId == tournamentId
															 && game.Round.Number < _roundNumber)
												 .SelectMany(static game => game.GamePlayers)
												 .ToArray();
		PowersPlayedInTournament.FillWith(gamePlayersInTournamentGames.WithPlayerId(PlayerId)
																	  .Select(static gamePlayer => gamePlayer.Power)
																	  .Where(static power => power is not TBD));
		PlayerIdsPlayedInTournament.FillWith(gamePlayersInTournamentGames.Select(static gamePlayer => gamePlayer.PlayerId)
																		 .Where(id => id != PlayerId)); //  Not strictly needed
		TournamentTeamPlayerIds.FillWith(Player.TournamentTeamPlayers(tournamentId)
											   .Ids());
		PlayerGroups.Clear();
		Player.Groups.ForEach(group => PlayerGroups.Add(group, [..group.Players
																	   .Ids()]));
		return this;
	}

	internal int CalculateConflict(bool fillDetails = false)
		=> CalculateConflict(Game.GamePlayers, fillDetails);

	internal int CalculateConflict(IEnumerable<GamePlayer> seeding,
								   bool fillDetails = false)
	{
		if (_roundNumber is null)
			PrepareForSeeding();
		if (fillDetails)
			ConflictDetails.Clear();
		var opponents = seeding.Where(gamePlayer => gamePlayer.GameId == GameId
												 && gamePlayer.PlayerId != PlayerId)
							   .Select(static gamePlayer => gamePlayer.Player)
							   .ToArray();
		var opponentIds = opponents.Ids()
								   .ToArray();

		//	Player-Personal conflicts
		Conflict = PlayerConflicts.Where(playerConflict => playerConflict.PlayerIds.Any(opponentIds.Contains))
								  .DefaultIfEmpty()
								  .Sum(playerConflict =>
									   {
										   var conflict = playerConflict?.Value ?? 0;
										   if (fillDetails && playerConflict is not null)
											   ConflictDetails.Add($"{conflict.Points()} for player conflict with {playerConflict.PlayerConflictedWith(PlayerId)}");
										   return conflict;
									   });

		//	Player-Group Conflicts
		foreach (var (group, memberIds) in PlayerGroups)
			opponentIds.ForSome(memberIds.Contains, opponent =>
													{
														var conflict = group.Conflict;
														Conflict += conflict;
														if (fillDetails)
															ConflictDetails.Add($"{conflict.Points()} for being in the group {group} with {opponent}.");
													});

		//	Powers-Played-Earlier Conflicts
		if (Power is not TBD)
		{
			var powerConflicts = PowersPlayedInTournament.Where(power => Tournament.SharePowerGroup(power, Power))
														 .ToArray();
			if (powerConflicts.Length is not 0)
				Conflict += powerConflicts.Distinct()
										  .Sum(power =>
											   {
												   var times = powerConflicts.Count(which => power == which);
												   var conflict = Tournament.PowerConflict * times;
												   //	The worst thing in the world is to play the SAME power.  Repeating
												   //	play in a Power GROUP is unavoidable, but repeating the same POWER
												   //	is unforgivable.  Make sure that is graded TEN TIMES WORSE.
												   if (power == Power && Tournament.GroupPowers is not Tournament.PowerGroups.None)
													   conflict *= 10;
												   if (fillDetails)
													   ConflictDetails.Add($"{conflict.Points()} for playing {power} {ThisMany(times)} earlier{
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
													ConflictDetails.Add($"{conflict.Points()} for playing {
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
												ConflictDetails.Add($"{conflict.Points()} for playing on a team with {
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
					ConflictDetails.Add($"{conflict.Points()} for being {distanceFromAverage:F2} tournament points off average for game's players.");
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
