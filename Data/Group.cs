namespace Data;

using static Group.GamesToRate;

public sealed partial class Group : IdentityRecord
{
	public enum GamesToRate : byte
	{
		//	IMPORTANT: Must be 0, 1, 2 to match tab index in GroupRatingsForm
		GroupGamesOnly = 0,
		GamesUsingGroupSystem = 1,
		AllGamesScoreableWithGroupSystem = 2
	}

	public int Conflict;

	public string Description = Empty;

	public int? ScoringSystemId => Tournament?.ScoringSystemId;

	public IEnumerable<Player> Players => ReadMany<GroupPlayer>(groupPlayer => groupPlayer.GroupId == Id).Select(static groupPlayer => groupPlayer.Player);

	public Tournament? Tournament => ReadOne<Tournament>(tournament => tournament.GroupId == Id);

	public ScoringSystem? ScoringSystem
	{
		get => Tournament?.ScoringSystem;
		set
		{
			if (value is null)
			{
				if (Tournament is null)
					return;
				var games = Tournament.Games;
				Delete(games.SelectMany(static game => game.GamePlayers));
				Delete(games);
				Delete(Tournament.Rounds);
				Delete(Tournament);
			}
			else if (Tournament is null)
				CreateOne(new Round
						  {
							  Tournament = CreateOne(new Tournament { Group = this, ScoringSystem = value })
						  });
			else
			{
				Tournament.ScoringSystem = value;
				UpdateOne(Tournament);
			}
		}
	}

	public Game[] Games => Tournament?.Rounds
									 .Single()
									 .Games
									 .OrderBy(static game => game.Date)
									 .ToArray()
						   ?? [];

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

	public RatingInfo? RatePlayer(Player player,
								  Game? beforeThisGame = null,
								  GamesToRate gamesToRate = default,
								  bool includeTheBeforeGame = false)
	{
		var scoringSystem = ScoringSystem.OrThrow();
		var playerId = player.Id;
		var comparer = includeTheBeforeGame.AsInteger();
		var scores = ReadMany<Game>(game => IsRatable(game, gamesToRate)
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
										.FinalScore)
					.ToArray();
		return scores.Length is 0
				   ? null
				   : new RatingInfo(player,
									scoringSystem.UsesPlayerAnte || scoringSystem.PointsPerGame is 0
										? scores.Sum()
										: scores.Average(),
									scoringSystem,
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
