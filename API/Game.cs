namespace API;

using static GameResults;
using static Powers;

[PublicAPI]
internal sealed class Game : Rest<Game, Data.Game, Game.Detail>
{
	public int Number { get; set; }
	public Statuses Status { get; set; }

	private protected override void LoadFromDataRecord(Data.Game record)
	{
		Number = record.Number;
		Status = record.Status;
		Info = new ()
			   {
				   Description = Record.Tournament.IsEvent
									 ? $"{Record.Tournament} - Round {Record.Round.Number} - Game {Record.Number}"
									 : $"{Record.Tournament.Group} - Game {Record.Number}",
				   GroupId = Record.Tournament.GroupId,
				   TournamentId = Record.Tournament.IsEvent
									  ? Record.Tournament.Id
									  : null,
				   RoundNumber = Record.Tournament.IsEvent
									 ? Record.Round.Number
									 : null,
				   Players = Record.GamePlayers.Select(static gamePlayer => new GamePlayer(gamePlayer))
			   };
	}

	[PublicAPI]
	public sealed class Detail : DetailClass
	{
		required public string? Description { get; set; }
		required public int? GroupId { get; set; }
		required public int? TournamentId { get; set; }
		required public int? RoundNumber { get; set; }
		required public IEnumerable<GamePlayer> Players { get; set; }
	}

	[PublicAPI]
	internal sealed class GamePlayer
	{
		public Player? Player => _gamePlayer.Player.IsNone
									 ? null
									 : Player.RestForId(_gamePlayer.PlayerId, false);
		public Powers Power => _gamePlayer.Power;
		public GameResults Result => _gamePlayer.Result;
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

		private readonly Data.GamePlayer _gamePlayer;
		private readonly Data.ScoringSystem _scoringSystem = Data.ScoringSystem.None;
		private bool _gameScored;

		public GamePlayer(Data.GamePlayer gamePlayer,
						  Data.ScoringSystem? scoringSystem = null)
		{
			_gamePlayer = gamePlayer;
			_scoringSystem = scoringSystem ?? gamePlayer.Game.ScoringSystem;
			if (gamePlayer.Game is { Status: Finished, Scored: false })
				gamePlayer.Game.CalculateScores();
			_gameScored = gamePlayer.Game.IsNone || gamePlayer.Game.Scored;
		}

		internal GamePlayer(Powers power,
							GameResults result,
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

	internal string[] Create(Data.Round round)
	{
		if (Id is not 0 || Number is not 0)
			return ["Id and Number may not be provided at creation."];
		Record = new () { Number = round.Games.Length + 1, Round = round };
		var issues = Update(out var updatedGamePlayers);
		if (issues.Length is not 0)
			return issues;
		Record.Create();
		CreateMany([..updatedGamePlayers]);
		return [];
	}

	internal string[] Update(out List<Data.GamePlayer> updatedPlayers)
	{
		updatedPlayers = [];
		var players = Details?.Players.ToList();
		if (players is not null)
		{
			if (players.Count is not 7
			|| players.Select(static player => player.Power)
					  .Where(static power => power is not TBD)
					  .Distinct()
					  .Count() is not 7)
				return ["Invalid player power assignments"];
			if (Status is Seeded
			&& players.Any(static player => (player.Result, player.Years, player.Centers, player.Other) is not (Unknown, null, null, null)))
				return ["Player game data not allowed for game in Seeded status"];
			var system = Record.ScoringSystem;
			if (Status is Finished
			&& players.Any(player => system.UsesGameResult != player.Result is Unknown
								  || system.UsesYearsPlayed != player.Years is null
								  || system.UsesCenterCount != player.Centers is null
								  || system.UsesOtherScore != player.Other is null))
				return ["Incomplete player game data for a Finished game"];
			var finalYear = players.Max(static player => player.Years ?? int.MaxValue);
			if (players.Any(player => player.Years < 1 || player.Years > system.FinalGameYear
								   || player.Centers < 0
								   || (player.Result, player.Centers) is (Win, 0))
			|| Status is Underway
			&& players.Any(player => !system.UsesGameResult && player.Result is not Unknown
								  || !system.UsesYearsPlayed && player.Years is not null
								  || !system.UsesCenterCount && player.Centers is not null
								  || !system.UsesOtherScore && player.Other is not null)
			|| Status is Finished && (players.All(static player => player.Result is Loss)
								  || players.Any(player => player.Result is Win && player.Years < finalYear)))
				return ["Invalid player game data"];
			foreach (var player in players)
			{
				var playerFromId = Player.GetById(player.Player?.Id ?? default);
				if (playerFromId.IsNone)
					return ["Invalid Player Id {player.Player?.Id}"];
				updatedPlayers.Add(new ()
								   {
									   Game = Record,
									   Player = playerFromId,
									   Result = player.Result,
									   Centers = player.Centers,
									   Years = player.Years,
									   Other = player.Other ?? default
								   });
			}
		}
		Record.Name = Name;
		Record.Status = Status;
		return [];
	}
}
