namespace Data;

using static Group.GamesToRate;

public sealed class Group : IdentityRecord<Group>, IdInfoRecord.IEvent
{
	public sealed record RatingRecord(Player Player, double Rating, int Games);

	public enum GamesToRate : byte
	{
		//	IMPORTANT: Must be 0, 1, 2 to match tab index in GroupRatingsForm
		GroupGamesOnly = 0,
		GamesUsingGroupSystem = 1,
		AllGamesScoreableWithGroupSystem = 2
	}

	public int Conflict;

	public string Description = Empty;

	public int ScoringSystemId => HostRound.IsNone ? default : HostRound.ScoringSystemId;

	public IEnumerable<Player> Players => ReadMany<GroupPlayer>(groupPlayer => groupPlayer.GroupId == Id).Select(static groupPlayer => groupPlayer.Player);

	//  HostRound for Group games (which are modeled as a single-round Tournament)
	public Round HostRound
	{
		get => field.Tournament.GroupId == Id
				   ? field
				   : field = ReadOne<Round>(round => round.Tournament.GroupId == Id) ?? field;
		private set;
	} = Round.None;

	public ScoringSystem ScoringSystem
	{
		get => HostRound.ScoringSystem;
		set
		{
			if (value.IsNone)
			{
				if (HostRound.IsNone)
					return;
				var games = Games;
				Delete(games.SelectMany(static game => game.GamePlayers));
				Delete(games);
				Delete(HostRound);
				Delete(HostRound.Tournament);
			}
			else if (HostRound.IsNone)
				HostRound = CreateOne(new Round
									  {
										  ScoringSystem = value,
										  Tournament = CreateOne(new Tournament
																 {
																	 Name = $"{this} Group Games",
																	 Group = this
																 })
									  });
			else
			{
				HostRound.ScoringSystem = value;
				UpdateOne(HostRound);
			}
		}
	}

	public Game[] Games => [..HostRound.Games
									   .OrderBy(static game => game.Date)];

	public Game[] FinishedGames => [..Games.Where(static game => game.Status is Finished)];

	public bool IsRatable(Game game,
						  GamesToRate gamesToRate)
		=> game.Status is Finished
		&& gamesToRate switch
		   {
			   GroupGamesOnly                   => game.Tournament.GroupId == Id,
			   GamesUsingGroupSystem            => game.ScoringSystemId == ScoringSystemId,
			   AllGamesScoreableWithGroupSystem => true,
			   _                                => throw new ArgumentOutOfRangeException(nameof (gamesToRate), "Unrecognized GamesToRate value")
		   };

	public RatingRecord? RatePlayer(Player player,
									Game? beforeThisGame = null,
									GamesToRate gamesToRate = default,
									bool includeTheBeforeGame = false)
	{
		var scoringSystem = ScoringSystem.OrThrow();
		var playerId = player.Id;
		var comparer = includeTheBeforeGame.AsInteger();
		double[] scores = [..ReadMany<Game>(game => IsRatable(game, gamesToRate)
												 && (beforeThisGame is null
												 || (game.Date, game.Round.Number, game.Number).CompareTo((beforeThisGame.Date,
																										   beforeThisGame.Round.Number,
																										   beforeThisGame.Number)) < comparer))
							 .OrderBy(static game => game.Date)
							 .Where(game => (game.Scored && game.ScoringSystemId == ScoringSystemId
										 || game.CalculateScores(scoringSystem))
											//  Make sure this last clause is the final one in the &&-chain,
											//  because to get accurate antes, we need to calculate ALL prior
											//  games, whether this player played in them or not.
										 && game.GamePlayers.HasPlayerId(playerId))
							 .Select(game => game.GamePlayers
												 .ByPlayerId(playerId)
												 .FinalScore)];
		return scores.Length is 0
				   ? null
				   : new RatingRecord(player,
									  scoringSystem.UsesPlayerAnte || scoringSystem.PointsPerGame is 0
										  ? scores.Sum()
										  : scores.Average(),
									  scores.Length);
	}

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<Group>();
		Id = record.Integer(nameof (Id));
		Name = record.String(nameof (Name));
		Description = record.String(nameof (Description));
		Conflict = record.Integer(nameof (Conflict));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (Name)}}] = {0},
	                                           [{{nameof (Description)}}] = {1},
	                                           [{{nameof (Conflict)}}] = {2}
	                                           """;

	public override string FieldValues => Format(FieldValuesFormat,
												 Name.ForSql(),
												 Description.ForSql(),
												 Conflict);

	#endregion
}
