namespace API;

[PublicAPI]
internal sealed class Event : Rest<Event, Data.Tournament, Event.Detail>
{
	[PublicAPI]
	internal sealed class Detail : DetailClass
	{
		required public string Date { get; set; }
		required public string? Description { get; set; }
		required public int TotalRounds { get; set; }
		required public SeedingDetail Seeding { get; set; }
		required public ScoringDetail Scoring { get; set; }
		required public TeamDetail? TeamTournament { get; set; }
	}

	[PublicAPI]
	internal sealed class SeedingDetail
	{
		required public bool AssignPowers { get; set; }
		required public PowerGroups GroupPowers { get; set; }
		required public int PlayerConflict { get; set; }
		required public int PowerConflict { get; set; }
		required public bool ProgressiveScoreConflict { get; set; }
	}

	[PublicAPI]
	internal sealed class ScoringDetail
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
	internal sealed class TeamDetail
	{
		required public int TeamSize { get; set; }
		required public int TeamConflict { get; set; }
		required public bool TeamsPlayMultipleRounds { get; set; }
		required public int? TeamRound { get; set; }
		required public bool PlayerCanJoinManyTeams { get; set; }
	}

	[PublicAPI]
	internal sealed class RoundPlayer : Player
	{
		//	TODO: Does TournamentScore maybe ride here?
		required public int[]? Rounds { get; init; }
	}

	private protected override void LoadFromDataRecord(Data.Tournament record)
		=> Info = new ()
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
									ProgressiveScoreConflict = Record.ProgressiveScoreConflict
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
	private static readonly string[] RoundNumbersDisallowed = ["Round number(s) disallowed when unregistering from a event."];
	private static readonly string[] RegistrationClosed = ["Registration is closed for a finished round or event."];
	private static readonly string[] AllRoundsCreated = ["All event rounds have been created."];

	internal static void CreateEndpoints(WebApplication app)
	{
		CreateCrudEndpoints(app);

		//	Players
		app.MapGet("event/{id:int}/players", GetPlayerRegistration)
		   .WithDescription("List all players registered or unregistered for the event, with the rounds for which each player is registered.")
		   .Produces<RoundPlayer[]>()
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapPatch("event/{id:int}/player/{playerId:int}", UpdateRegistration)
		   .WithDescription("Register a player for the event while setting, updating, or clearing the player's round registration.")
		   .Produces(Status200OK)
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);

		//	Rounds
		app.MapGet("event/{id:int}/rounds", GetRounds)
		   .WithDescription("List the event rounds.")
		   .Produces<Round[]>()
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapGet("event/{id:int}/round/{roundNumber:int}", GetRound)
		   .WithDescription("Get details for an event round.")
		   .Produces<Round>()
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapGet("event/{id:int}/round/{roundNumber:int}/players",
				   static (int id, int roundNumber, bool registered = true) => GetPlayerRegistration(id, [roundNumber], registered))
		   .WithDescription("Get the registered players for an event round.")
		   .Produces(Status200OK)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapPatch("event/{id:int}/round/{roundNumber:int}/player/{playerId:int}",
					 static (int id, int playerId, int roundNumber, bool register) => UpdateRegistration(id, playerId, [roundNumber], register, true))
		   .WithDescription("Set registration for a specific round for an event player.")
		   .Produces(Status200OK)
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .Produces<string>(Status409Conflict)
		   .WithTags(SwaggerTag);
		app.MapPost("event/{id:int}/round", CreateRound)
		   .WithDescription("Create the next event round.")
		   .Produces(Status201Created)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapPost("event/{id:int}/round/seed", SeedRound)
		   .WithDescription("Seed the current event round.")
		   .Produces(Status201Created)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);

		//	Games
		app.MapGet("event/{id:int}/round/{roundNumber:int}/games", GetRoundGames)
		   .WithDescription("List games in an event round.")
		   .Produces(Status200OK)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapGet("event/{id:int}/round/{roundNumber:int}/game/{gameNumber:int}", GetGame)
		   .WithDescription("Get details on an event game.")
		   .Produces(Status200OK)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);

		//	Teams
		//	TODO
	}

	private static IResult GetPlayerRegistration(int id,
												 int[] round,
												 bool registered = true)
	{
		var tournament = RestForId(id)?.Record;
		if (tournament is null)
			return NotFound();
		if (round.Any(number => number < 1 || number > tournament.TotalRounds))
			return BadRequest(InvalidRoundNumber);
		if (registered || round.Length is not 0)
			return Ok(round.Aggregate(tournament.TournamentPlayers
												.Select(static tp => new RoundPlayer { Record = tp.Player, Rounds = tp.Rounds }),
									  (current, number) => current.Where(tp => tp.Rounds is not null && tp.Rounds.Contains(number) == registered)));
		var playerIds = tournament.TournamentPlayers
								  .Select(static player => player.PlayerId);
		return Ok(RoundPlayer.RestFrom(ReadMany<Data.Player>(player => !playerIds.Contains(player.Id))));
	}

	private static IResult UpdateRegistration(int id,
											  int playerId,
											  int[] round,
											  bool register,
											  bool singleRound = false)
	{
		var tournament = RestForId(id)?.Record;
		var player = Player.GetById(playerId);
		if (tournament is null || player.IsNone)
			return NotFound();
		if (tournament.Rounds.Count(static round => !round.Workable) == tournament.TotalRounds)
			return BadRequest(RegistrationClosed);
		if (!register || round.Length is not 0)
			return BadRequest(RoundNumbersDisallowed);
		if (round.Any(number => number < 1 || number > tournament.TotalRounds))
			return BadRequest(InvalidRoundNumber);
		var roundList = singleRound
							? round
							: Enumerable.Range(1, tournament.TotalRounds)
										.ToArray();
		if (roundList.Any(roundNumber => (singleRound && !register || !round.Contains(roundNumber))
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
		foreach (var roundNumber in roundList.Where(number => round.Contains(number) != tournamentPlayer.RegisteredForRound(number)))
		{
			var heat = tournament.Rounds
								 .SingleOrDefault(heat => heat.Number == roundNumber);
			if (heat?.Workable is false)
				return BadRequest(RegistrationClosed);
			//	If registering for a round that is underway, we must either add or delete a RoundPlayer record.
			var roundPlayer = heat?.Workable is true
								  ? new Data.RoundPlayer { Player = player, Round = heat }
								  : null;
			if (round.Contains(roundNumber))
			{
				tournamentPlayer.RegisterForRound(roundNumber);
				roundPlayer?.Create();
				continue;
			}
			tournamentPlayer.UnregisterForRound(roundNumber);
			roundPlayer?.Delete();
		}
		if (!register)
			Delete(tournamentPlayer);
		return Ok();
	}

	private static IResult GetRounds(int id)
	{
		var tournament = RestForId(id)?.Record;
		return tournament is null
			? NotFound()
			: Ok(Round.RestFrom(tournament.Rounds.OrderBy(static round => round.Number)));
	}

	public static IResult GetRound(int id,
								   int roundNumber)
	{
		var round = RestForId(id)?.Record.Rounds.SingleOrDefault(round => round.Number == roundNumber);
		return round is null
				   ? NotFound()
				   : Ok(Round.RestFrom(round));
	}

	public static IResult GetRoundGames(int id,
										int roundNumber)
	{
		var round = RestForId(id)?.Record.Rounds.SingleOrDefault(round => round.Number == roundNumber);
		return round is null
				   ? NotFound()
				   : Ok(Game.RestFrom(round.Games.OrderBy(static game => game.Number)));
	}

	public static IResult GetGame(int id,
									   int roundNumber,
									   int gameNumber)
	{
		var gameId = RestForId(id)?.Record.Rounds.SingleOrDefault(round => round.Number == roundNumber)
								  ?.Games.SingleOrDefault(game => game.Number == gameNumber)
								  ?.Id;
		return gameId is null
				   ? NotFound()
				   : Ok(RestForId(gameId.Value));
	}

	public static IResult CreateRound(int id)
	{
		var tournament = RestForId(id)?.Record;
		if (tournament is null)
			return NotFound();
		if (tournament.Rounds.Length == tournament.TotalRounds)
			return BadRequest(AllRoundsCreated);
		var round = tournament.CreateRound();
		return Created($"tournament/{id}/round/{round.Number}", null);
	}

	public static IResult SeedRound(int id)
	{
		throw new NotImplementedException();
	}

	private protected override string[] UpdateRecordForDatabase(Event @event)
	{
		//	TODO - Add more validation - name collision, ridiculous date, etc., etc.
		var details = @event.Details;
		if (details is null)
			return [];

		Record.Name = @event.Name;

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
}
