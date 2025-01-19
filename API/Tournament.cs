namespace API;

using static Data.Data;

[PublicAPI]
internal class Tournament : Rest<Tournament, Data.Tournament, Tournament.Detail>
{
	public int Id => Identity;
	public string Name => RecordedName;

	[PublicAPI]
	internal sealed class Detail : DetailClass
	{
		[PublicAPI]
		public sealed class SeedingDetail
		{
			required public bool AssignPowers { get; set; }
			required public PowerGroups GroupPowers { get; set; }
			required public int PlayerConflict { get; set; }
			required public int PowerConflict { get; set; }
			required public bool ProgressiveScoreConflict { get; set; }
			required public int? TeamConflict { get; set; }
		}

		[PublicAPI]
		public sealed class ScoringDetail
		{
			required public int SystemId { get; set; }
			required public int UnplayedScore { get; set; }
			required public int MinimumRounds { get; set; }
			required public int RoundsToDrop { get; set; }
			required public bool DropBeforeFinalRound { get; set; }
			required public int RoundsToScale { get; set; }
			required public int? ScalePercentage { get; set; }
		}

		[PublicAPI]
		public sealed class TeamDetail
		{
			required public int TeamSize { get; set; }
			required public int TeamConflict { get; set; }
			required public bool TeamsPlayMultipleRounds { get; set; }
			required public int? TeamRound { get; set; }
			required public bool PlayerCanJoinManyTeams { get; set; }
		}

		required public string Date { get; set; }
		required public string? Description { get; set; }
		required public int TotalRounds { get; set; }
		required public SeedingDetail Seeding { get; set; }
		required public ScoringDetail Scoring { get; set; }
		required public TeamDetail? TeamTournament { get; set; }
	}

	protected override Detail Info => new ()
												   {
													   Date = $"{Record.Date:d}",
													   Description = Record.Description.NullIfEmpty(),
													   TotalRounds = Record.TotalRounds,
													   Seeding = new ()
																 {
																	 AssignPowers = Record.AssignPowers,
																	 GroupPowers = Record.GroupPowers,
																	 PlayerConflict = Record.PlayerConflict,
																	 PowerConflict = Record.PowerConflict,
																	 ProgressiveScoreConflict = Record.ProgressiveScoreConflict,
																	 TeamConflict = Record.TeamSize is 0
																						? null
																						: Record.TeamConflict
																 },
													   Scoring = new ()
																 {
																	 SystemId = Record.ScoringSystemId,
																	 UnplayedScore = Record.UnplayedScore,
																	 MinimumRounds = Record.MinimumRounds,
																	 RoundsToDrop = Record.RoundsToDrop,
																	 DropBeforeFinalRound = Record.RoundsToDrop is not 0 && Record.DropBeforeFinalRound,
																	 RoundsToScale = Record.RoundsToScale,
																	 ScalePercentage = Record.RoundsToScale is 0
																						   ? null
																						   : Record.ScalePercentage
																 },
													   TeamTournament = Record.TeamSize is 0
																			? null
																			: new ()
																			  {
																				  TeamSize = Record.TeamSize,
																				  TeamConflict = Record.TeamConflict,
																				  TeamsPlayMultipleRounds = Record.TeamsPlayMultipleRounds,
																				  TeamRound = Record.TeamsPlayMultipleRounds
																								  ? null
																								  : Record.TeamRound,
																				  PlayerCanJoinManyTeams = Record.PlayerCanJoinManyTeams
																			  }
												   };

	private static readonly string[] InvalidRoundNumber = ["Invalid round number(s)."];
	private static readonly string[] RoundNumbersDisllowed = ["Round number(s) disallowed when unregistering from a tournament."];

	new private protected static void CreateNonCrudEndpoints(WebApplication app, string tag)
	{
		//	Players
		app.MapGet("tournament/{id:int}/players",
				   GetPlayerRegistration)
		   .WithDescription("List all players registered or unregistered for the tournament, with the rounds for which each player is registered.")
		   .Produces<RoundPlayer[]>()
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapPatch("tournament/{id:int}/player/{playerId:int}" /* ?register=true&round=1&round=2... */,
					 UpdateRegistration)
		   .WithDescription("Register a player for the tournament while setting, updating, or clearing the player's round registration.")
		   .Produces(Status200OK)
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(tag);

		//	Rounds
		app.MapGet("tournament/{id:int}/rounds", GetRounds)
		   .WithDescription("List the tournament rounds.")
		   .Produces<Round[]>()
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapGet("tournament/{id:int}/round/{roundNumber:int}/players",
				   static (int id, int roundNumber, bool registered = true) => GetPlayerRegistration(id, [roundNumber], registered))
		   .WithDescription("Get the registered players for a registered tournament player.")
		   .Produces(Status200OK)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapPatch("tournament/{id:int}/round/{roundNumber:int}/player/{playerId:int}" /* ?register=true */,
					 static (int id, int playerId, int roundNumber, bool register) => UpdateRegistration(id, playerId, [roundNumber], register, true))
		   .WithDescription("Set the round registration for a registered tournament player.")
		   .Produces(Status200OK)
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapGet("tournament/{id:int}/round/{roundNumber:int}/games", GetRoundGames)
		   .WithDescription("List games in a tournament round.")
		   .Produces(Status200OK)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapGet("tournament/{id:int}/round/{roundNumber:int}/game/{gameNumber:int}", GetRoundGame)
		   .WithDescription("Get details on a tournament game.")
		   .Produces(Status200OK)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
	}

	new private protected static IResult GetAll()
		=> Ok(RestFrom(GetMany(static tournament => tournament.IsEvent)));

	private static IResult GetPlayerRegistration(int id, int[] round, bool registered = true)
	{
		var tournament = RestForId(id)?.Record;
		if (tournament is null || !tournament.IsEvent)
			return NotFound();
		if (round.Any(number => number < 1 || number > tournament.TotalRounds))
			return BadRequest(InvalidRoundNumber);
		if (registered || round.Length is not 0)
		{
			var players = round.Aggregate(tournament.TournamentPlayers
													.Select(static tp => new RoundPlayer { Record = tp.Player, Rounds = tp.Rounds }),
										  (current, number) => current.Where(tp => tp.Rounds is not null && tp.Rounds.Contains(number) == registered));
			return Ok(players);
		}
		var playerIds = tournament.TournamentPlayers.Select(static player => player.PlayerId);
		return Ok(RoundPlayer.RestFrom(Player.GetMany(player => !playerIds.Contains(player.Id))));
	}

	private static IResult UpdateRegistration(int id, int playerId, int[] round, bool register, bool forSingleRound = false)
	{
		var tournament = RestForId(id)?.Record;
		var player = Player.GetById(playerId);
		if (tournament is null || player.IsNone || !tournament.IsEvent)
			return NotFound();
		if (!register || round.Length is not 0)
			return BadRequest(RoundNumbersDisllowed);
		if (round.Any(number => number < 1 || number > tournament.TotalRounds))
			return BadRequest(InvalidRoundNumber);
		var roundList = forSingleRound
							? round
							: Enumerable.Range(1, tournament.TotalRounds)
										.ToArray();
		if (roundList.Any(roundNumber => (forSingleRound && !register || !round.Contains(roundNumber))
									  && tournament.Games
												   .Any(game => game.Round.Number == roundNumber
															 && game.GamePlayers.Any(gamePlayer => gamePlayer.PlayerId == playerId))))
			return Conflict("Cannot unregister player from round when involved in a game in that round.");
		var tournamentPlayer = tournament.TournamentPlayers
										 .SingleOrDefault(tournamentPlayer => tournamentPlayer.PlayerId == playerId);
		if (tournamentPlayer is null)
			if (register)
				tournamentPlayer = tournament.AddPlayer(player);
			else
				return NoContent();
		foreach (var roundNumber in roundList)
			if (round.Contains(roundNumber))
				tournamentPlayer.RegisterForRound(roundNumber);
			else
				tournamentPlayer.UnregisterForRound(roundNumber);
		if (!register)
			Delete(tournamentPlayer);
		return Ok();
	}

	private static IResult GetRounds(int id)
	{
		var tournament = RestForId(id);
		return tournament is null || !tournament.Record.IsEvent
			? NotFound()
			: Ok(Round.RestFrom(tournament.Record.Rounds.OrderBy(static round => round.Number)));
	}

	public static IResult GetRoundGames(int id, int roundNumber)
	{
		var round = RestForId(id)?.Record.Rounds.SingleOrDefault(round => round.Number == roundNumber);
		return round is null
				   ? NotFound()
				   : Ok(Game.RestFrom(round.Games.OrderBy(static game => game.Number)));
	}

	public static IResult GetRoundGame(int id, int roundNumber, int gameNumber)
	{
		var game = RestForId(id)?.Record.Rounds.SingleOrDefault(round => round.Number == roundNumber)
								?.Games.SingleOrDefault(game => game.Number == gameNumber);
		return game is null
				   ? NotFound()
				   : Ok(RestForId(game.Id));
	}

	private protected override string[] Update(Tournament tournament)
	{
		//	TODO - Add more validation - name collision, ridiculous date, etc., etc.
		var details = tournament.Details;
		if (details is null)
			return [];

		Record.Name = tournament.Name;

		Record.Date = DateTime.Parse(details.Date);
		Record.Description = details.Description ?? string.Empty;
		Record.TotalRounds = details.TotalRounds;

		if (details.TeamTournament?.TeamSize is null or 0)
			Record.TeamSize = 0;
		else
		{
			Record.TeamSize = details.TeamTournament.TeamSize;
			Record.TeamsPlayMultipleRounds = details.TeamTournament.TeamsPlayMultipleRounds;
			Record.PlayerCanJoinManyTeams = details.TeamTournament.PlayerCanJoinManyTeams;
			Record.TeamRound = details.TeamTournament.TeamRound ?? default;
		}

		Record.AssignPowers = details.Seeding.AssignPowers;
		Record.GroupPowers = details.Seeding.GroupPowers;
		Record.PlayerConflict = details.Seeding.PlayerConflict;
		Record.PowerConflict = details.Seeding.PowerConflict;
		Record.ProgressiveScoreConflict = details.Seeding.ProgressiveScoreConflict;

		Record.ScoringSystem = System.GetById(details.Scoring.SystemId);
		Record.UnplayedScore = details.Scoring.UnplayedScore;
		Record.MinimumRounds = details.Scoring.MinimumRounds;
		Record.RoundsToDrop = details.Scoring.RoundsToDrop;
		Record.DropBeforeFinalRound = details.Scoring.DropBeforeFinalRound;
		Record.RoundsToScale = details.Scoring.RoundsToScale;
		Record.ScalePercentage = details.Scoring.RoundsToScale;
		return [];
	}

	[PublicAPI]
	internal sealed class RoundPlayer : Player
	{
		public int[]? Rounds { get; init; }
	}
}
