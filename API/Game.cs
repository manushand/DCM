using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal sealed class Game : Rest<Game, Data.Game>
{
	public override string Name => Record.Name.NullIfEmpty()
						  ?? (Record.Tournament.IsEvent
								  ? $"{Record.Tournament} - Round {Record.Round.Number} - Game {Record.Number}"
								  : $"{Record.Tournament.Group} - Game {Record.Number}");
	public Data.Game.Statuses Status => Record.Status;
	public int Number => Record.Number;

	protected override dynamic Detail => new
										 {
											 TournamentId = Record.Tournament.IsEvent
																? Record.Tournament.Id
																: (int?)null,
											 RoundId = Record.Tournament.IsEvent
														   ? Record.RoundId
														   : (int?)null,
											 Record.Tournament.GroupId,
											 Players = Record.GamePlayers.Select(static gamePlayer => new GamePlayer(gamePlayer))
										 };

	[PublicAPI]
	internal sealed class GamePlayer
	{
		private readonly Data.GamePlayer _gamePlayer;
		public Player Player => new () { Record = _gamePlayer.Player };
		public Data.GamePlayer.PowerNames Power => _gamePlayer.Power;
		public Data.GamePlayer.Results Result => _gamePlayer.Result;
		public int? Years => _gamePlayer.Game is { Scored: true, ScoringSystem.UsesYearsPlayed: true }
								 ? _gamePlayer.Years
								 : null;
		public int? Centers => _gamePlayer.Game is { Scored: true, ScoringSystem.UsesCenterCount: true }
								 ? _gamePlayer.Centers
								 : null;
		public double? OtherScore => _gamePlayer.Game is { Scored: true, ScoringSystem.UsesOtherScore: true }
								 ? _gamePlayer.Other
								 : null;
		public double? PlayerAnte => _gamePlayer.Game is { Scored: true, ScoringSystem.UsesPlayerAnte: true }
										 ? _gamePlayer.PlayerAnte
										 : null;
		public double? ProvisionalScore => _gamePlayer.Game is { Scored: true, ScoringSystem.UsesProvisionalScore: true }
										 ? _gamePlayer.ProvisionalScore
										 : null;
		public double? FinalScore => _gamePlayer.Game.Scored
										 ? _gamePlayer.FinalScore
										 : null;

		public GamePlayer(Data.GamePlayer gamePlayer)
		{
			_gamePlayer = gamePlayer;
			if (gamePlayer.Game.Status is Data.Game.Statuses.Finished && !gamePlayer.Game.Scored)
				gamePlayer.Game.CalculateScores();
		}
	}

	new internal static IEnumerable<Game> GetAll()
		=> throw new FileNotFoundException();
}
