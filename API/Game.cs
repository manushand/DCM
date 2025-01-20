namespace API;

[PublicAPI]
internal sealed class Game : Rest<Game, Data.Game, Game.Detail>
{
	public int Id => Record.Id;
	public string? Name => Record.Name.NullIfEmpty();

	public Statuses Status => Record.Status;
	public int Number => Record.Number;

	[PublicAPI]
	public sealed record Detail(string? EndpointDescriptionAttribute, int? TournamentId, int? RoundNumber, int? GroupId) : DetailClass;

	private IEnumerable<GamePlayer> Players => Record.GamePlayers.Select(static gamePlayer => new GamePlayer(gamePlayer));

	protected override Detail Info => new (Record.Tournament.IsEvent
																   ? $"{Record.Tournament} - Round {Record.Round.Number} - Game {Record.Number}"
																   : $"{Record.Tournament.Group} - Game {Record.Number}",
										   Record.Tournament.IsEvent
																	? Record.Tournament.Id
																	: null,
										   Record.Tournament.IsEvent
															   ? Record.Round.Number
															   : null,
										   Record.Tournament.GroupId);

	[PublicAPI]
	internal sealed class GamePlayer
	{
		private readonly Data.GamePlayer _gamePlayer;
		public Player? Player => _gamePlayer.Player.IsNone
									 ? null
									 : Player.RestForId(_gamePlayer.PlayerId, false);
		public Powers Power => _gamePlayer.Power;
		public Data.GamePlayer.Results Result => _gamePlayer.Result;
		public int? Years => _gameScored && _scoringSystem.UsesYearsPlayed
								 ? _gamePlayer.Years
								 : null;
		public int? Centers => _gameScored && _scoringSystem.UsesCenterCount
								   ? _gamePlayer.Centers
								   : null;
		public double? Other => _gameScored && _scoringSystem.UsesOtherScore
									? _gamePlayer.Other
									: null;
		public double? PlayerAnte => _gameScored && _scoringSystem.UsesPlayerAnte
										 ? _gamePlayer.PlayerAnte
										 : null;
		public double? ProvisionalScore => _gameScored && _scoringSystem.UsesProvisionalScore
											   ? _gamePlayer.ProvisionalScore
											   : null;
		public double? FinalScore => _gameScored
										 ? _gamePlayer.FinalScore
										 : null;

		private bool _gameScored;
		private readonly Data.ScoringSystem _scoringSystem = Data.ScoringSystem.None;

		public GamePlayer(Data.GamePlayer gamePlayer, Data.ScoringSystem? scoringSystem = null)
		{
			_gamePlayer = gamePlayer;
			_scoringSystem = scoringSystem ?? gamePlayer.Game.ScoringSystem;
			if (gamePlayer.Game is { Status: Finished, Scored: false })
				gamePlayer.Game.CalculateScores();
			_gameScored = gamePlayer.Game.IsNone || gamePlayer.Game.Scored;
		}

		internal GamePlayer(Powers power,
							Data.GamePlayer.Results result,
							int? centers,
							int? years,
							double other)
			=> _gamePlayer = new ()
							 {
								 Power = power,
								 Result = result,
								 Centers = centers,
								 Years = years,
								 Other = other
							 };
	}

	new public static IEnumerable<Game> GetAll()
		=> throw new NotImplementedException();
}
