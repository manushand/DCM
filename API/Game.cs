using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Game : Rest<Game, Data.Game, Game.GameDetails>
{
	public string? Name => Record.Name.NullIfEmpty();

	public Data.Game.Statuses Status => Record.Status;
	public int Number => Record.Number;

	[PublicAPI]
	public sealed class GameDetails : DetailClass
	{
		public string? Description { get; set; }
		public int? TournamentId { get; set; }
		public int? RoundNumber { get; set; }
		public int? GroupId { get; set; }
	}

	private IEnumerable<GamePlayer> Players => Record.GamePlayers.Select(static gamePlayer => new GamePlayer(gamePlayer));

	protected override GameDetails Detail => new ()
											 {
												 Description = Record.Tournament.IsEvent
																   ? $"{Record.Tournament} - Round {Record.Round.Number} - Game {Record.Number}"
																   : $"{Record.Tournament.Group} - Game {Record.Number}",
												 TournamentId = Record.Tournament.IsEvent
																	? Record.Tournament.Id
																	: null,
												 RoundNumber = Record.Tournament.IsEvent
															   ? Record.Round.Number
															   : null,
												 GroupId = Record.Tournament.GroupId
											 };

	[PublicAPI]
	internal sealed class GamePlayer
	{
		private readonly Data.GamePlayer _gamePlayer;
		public Player? Player => _gamePlayer.Player.IsNone
									 ? null
									 : Player.RestForId(_gamePlayer.PlayerId, false);
		public Data.GamePlayer.PowerNames Power => _gamePlayer.Power;
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
			if (gamePlayer.Game is { IsNone: false, Status: Data.Game.Statuses.Finished, Scored: false })
				gamePlayer.Game.CalculateScores();
			_gameScored = gamePlayer.Game.IsNone || gamePlayer.Game.Scored;
		}

		internal GamePlayer(Data.GamePlayer.PowerNames power,
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
