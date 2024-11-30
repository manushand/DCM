namespace DCM.DB;

internal sealed class Game : IdentityRecord
{
	private DateTime? _date;
	private int? _scoringSystemId;

	internal int Number;
	internal bool Scored;
	internal Statuses Status;
	internal int RoundId { get; private set; }
	internal int ScoringSystemId => _scoringSystemId ?? Round.ScoringSystemId;

	internal DateTime Date
	{
		get => _date ?? Tournament.Date;
		set => _date = value;
	}

	internal Tournament Tournament => Round.Tournament;

	internal string FullName => Name.Length is 0
									? $"{Tournament} {Round.Number}─{Number}"
									: Name;

	// <summary>
	// True if the Game uses the ScoringSystem set for its Round;
	// false if the Game overrides this to use another (even if it
	// does so to use the default ScoringSystem for the Tournament).
	// </summary>
	internal bool ScoringSystemIsDefault => _scoringSystemId is null;

	//	TODO: should this be made a private and only determined once, instead of a property?  Is this time-consuming?
	//	TODO: yes, it is time-consuming in seeding, but since players swap in and out of games, can we hold it private?
	internal IEnumerable<GamePlayer> GamePlayers => ReadMany<GamePlayer>(gamePlayer => gamePlayer.GameId == Id).Order();

	internal Round Round
	{
		get => field == Round.Empty
				   ? field = ReadById<Round>(RoundId).OrThrow()
				   : field;
		init => (field, RoundId) = (value, value.Id);
	} = Round.Empty;

	internal ScoringSystem ScoringSystem
	{
		get => field == ScoringSystem.Empty
				   ? field = ReadById<ScoringSystem>(ScoringSystemId).OrThrow()
				   : field;
		set => (field, _scoringSystemId) = (value, value.Id == Round.ScoringSystemId ? null : value.Id);
	} = ScoringSystem.Empty;

	internal IEnumerable<int> PlayerIds => GamePlayers.Select(static gamePlayer => gamePlayer.PlayerId);

	internal double AveragePreGameScore => GamePlayers.Select(PreGameScore)
													   .ToArray() //	These two .ToArray() call aren't needed, ...
													   .DefaultIfEmpty(Tournament.UnplayedScore)
													   .ToArray() //	...but for some reason they increase speed big-time
													   .Average();

	internal int Conflict => GamePlayers.Sum(static game => game.Conflict);

	/// <summary>
	///     Returns the tournament score (sum of previous rounds' games) or group rating score (using prior
	///     games according to the group's ratings rules) for this player as-of before this Game was played.
	/// </summary>
	/// <returns></returns>
	internal double PreGameScore(GamePlayer gamePlayer)
		=> Tournament.Group is null
			   ? Round.PreRoundScore(gamePlayer)
			   : Tournament.Group
						   .RatePlayer(gamePlayer.Player, this)?
						   .Rating ?? 0;

	/// <summary>
	///     Calculates the FinalScore for all GamePlayers in the game, according to the
	///     ScoringSystem of the game (or Tournament).
	/// </summary>
	/// <param name="scoringSystem">
	///     The ScoringSystem to use; if null, the system specified by the Game (or its
	///     Round or Tournament) will be used.
	/// </param>
	/// <returns>
	///     True if game scoring succeeded, false if it failed.
	/// </returns>
	internal bool CalculateScores(ScoringSystem? scoringSystem = null)
		=> CalculateScores(out _, scoringSystem);

	/// <summary>
	///     Calculates the FinalScore for all GamePlayers in the game, according to the
	///     ScoringSystem of the game (or Tournament).
	/// </summary>
	/// <param name="errors">
	///     An out parameter that will be returned filled with any errors that occurred
	///     during scoring.
	/// </param>
	/// <param name="scoringSystem">
	///     The ScoringSystem to use; if null, the system specified by the Game (or its
	///     Round or Tournament) will be used.
	/// </param>
	/// <returns>
	///     True if game scoring succeeded, false if it failed.
	/// </returns>
	internal bool CalculateScores(out List<string?> errors,
								  ScoringSystem? scoringSystem = null)
		=> Scored = (scoringSystem ?? ScoringSystem).ScoreWithResults([..GamePlayers], out errors);

	internal void FillFinalScores(List<Label> scoreLabels,
								  Label totalScoreLabel,
								  ToolTip toolTip)
	{
		var format = ScoringSystem.ScoreFormat;
		var gamePlayers = GamePlayers.ToArray();
		var isGroup = Tournament.Group is not null;
		var scoreOrRating = isGroup
								? "Rating"
								: "Score";
		var usesAnte = isGroup && ScoringSystem.UsesPlayerAnte;
		var details = new List<string>();
		scoreLabels.Apply((scoreLabel, i) =>
						  {
							  var gamePlayer = gamePlayers[i];
							  var score = gamePlayer.FinalScore;
							  scoreLabel.Text = score.ToString(format);
							  var preGame = Round.PreRoundScore(gamePlayer);
							  var postGame = isGroup
												 ? Tournament.Group
															 .OrThrow()
															 .RatePlayer(gamePlayer.Player, this, includeTheBeforeGame: true)?
															 .Rating ?? 0
												 : preGame + score;
							  var ante = gamePlayer.PlayerAnte;
							  details.Clear();
							  details.Add($"Pre-Game {scoreOrRating}: {preGame}");
							  if (usesAnte)
							  {
								  details.Add($"Player Ante: {ante}");
								  details.Add($"Game Score: {ante + score}");
							  }
							  if (isGroup)
								  details.Add($"Rating Change: {postGame - preGame}");
							  details.Add($"Post-Game {scoreOrRating}: {postGame}");
							  toolTip.SetToolTip(scoreLabels[i], $"{gamePlayer.Player}{details.BulletList()}");
						  });
		totalScoreLabel.Text = gamePlayers.Sum(static gamePlayer => gamePlayer.FinalScore)
										  .ToString(format);
	}

	internal enum Statuses : byte
	{
		//	IMPORTANT: Must be 0, 1, 2 to match ComboBox item order
		Seeded = 0,
		Underway = 1,
		Finished = 2
	}

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<Game>();
		Id = record.Integer(nameof (Id));
		Number = record.Integer(nameof (Number));
		Status = record.IntegerAs<Statuses>(nameof (Status));
		RoundId = record.Integer(nameof (RoundId));
		Name = record.String(nameof (Name));
		_scoringSystemId = record.NullableInteger(nameof (ScoringSystemId));
		_date = record.NullableDate(nameof (Date));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (Number)}}] = {0},
	                                           [{{nameof (Status)}}] = {1},
	                                           [{{nameof (RoundId)}}] = {2},
	                                           [{{nameof (Name)}}] = {3},
	                                           [{{nameof (ScoringSystemId)}}] = {4},
	                                           [{{nameof (Date)}}] = {5}
	                                           """;

	public override string FieldValues => Format(FieldValuesFormat,
												 Number,
												 Status.ForSql(),
												 RoundId,
												 Name.ForSql(),
												 _scoringSystemId.ForSql(),
												 _date.ForSql());

	#endregion
}
