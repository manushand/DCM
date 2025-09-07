using Data;

namespace API;

[PublicAPI]
internal sealed class Group : Rest<Group, Data.Group, Group.Detail>
{
	[PublicAPI]
	internal sealed class Detail : DetailClass
	{
		required public string? Description { get; set; }
		required public int SystemId { get; set; }
		required public int Conflict { get; set; }
	}

	private protected override void LoadFromDataRecord(Data.Group record)
		=> Info = new ()
				  {
					  Description = Record.Description.NullIfEmpty(),
					  SystemId = Record.ScoringSystemId,
					  Conflict = Record.Conflict
				  };

	private IEnumerable<Player> Players => Player.RestFrom(Record.Players);
	private int HostRoundId => Record.HostRound.Id;
	private IEnumerable<Game> Games => Game.RestFrom(Record.Games);

	internal static void CreateEndpoints(WebApplication app)
	{
		CreateCrudEndpoints(app);

		app.MapGet("group/{id:int}/games", GetGames)
		   .WithName("GetGroupGames")
		   .WithDescription("List all games played by the group.")
		   .Produces<Game[]>()
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapGet("group/{id:int}/game/{gameNumber:int}", GetGame)
		   .WithName("GetGroupGame")
		   .WithDescription("Get details on a game played by the group.")
		   .Produces<Game>()
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapGet("group/{id:int}/players", /* ?members=true */ GetMembers)
		   .WithName("GetGroupPlayers")
		   .WithDescription("List all players who are members or non-members of the group.")
		   .Produces<Player[]>()
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapPatch("group/{id:int}/player/{playerId:int}", ChangeMembership)
		   .WithName("ChangeGroupPlayerMembership")
		   .WithDescription("Add or remove a player from the group.")
		   .Produces(Status200OK)
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);

		app.MapPost("group/{id:int}/game", AddGroupGame)
		   .WithName("AddGroupGame")
		   .WithDescription("Create a new game played by the group.")
		   .Produces(Status201Created)
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
	}

	public static IResult AddGroupGame(int id,
									   Game game)
	{
		var hostRound = RestForId(id)?.Record.HostRound;
		if (hostRound is null)
			return NotFound();
		var issues = hostRound.IsNone
			? ["Group is not a game-playing group."]
			: game.Create(hostRound);
		return issues.Length is 0
			? Created($"group/{id}/game/{game.Number}", null)
			: BadRequest(issues);
	}

	public static IResult GetGames(int id)
	{
		var group = RestForId(id);
		return group is null
				   ? NotFound()
				   : Ok(group.Games.OrderBy(static game => game.Number));
	}

	public static IResult GetGame(int id,
								  int gameNumber)
		=> Game.GetOne(RestForId(id)?.Games.SingleOrDefault(game => game.Number == gameNumber)?.Id ?? default);

	public static IResult GetMembers(int id,
									 bool members = true)
	{
		var record = RestForId(id);
		if (record is null)
			return NotFound();
		if (members)
			return Ok(record.Players);
		var memberIds = record.Players
							  .Select(static player => player.Id)
							  .ToList();
		return Ok(Player.RestFrom(ReadMany<Data.Player>(player => !memberIds.Contains(player.Id))));
	}

	public static IResult ChangeMembership(int id,
										   int playerId,
										   bool member)
	{
		var group = RestForId(id);
		var player = Player.RestForId(playerId);
		if (group is null || player is null)
			return NotFound();
		if (member == group.HasPlayer(playerId))
			return NoContent();
		if (member)
			group.Record += player.Record;
		else
			//	TODO - Hmm.  Can a player be dropped even if they've played games that are group games?
			group.Record -= player.Record;
		return Ok();
	}

	public override bool Unlink()
	{
		Delete(ReadMany<GroupPlayer>(groupPlayer => groupPlayer.GroupId == Id));
		return true;
	}

	private protected override string[] UpdateRecordForDatabase(Group group)
	{
		if (group.Details is null)
			return ["Details are required"];
		// TODO
		Record.Name = group.Name;
		Record.Description = group.Details.Description?.Trim() ?? string.Empty;
		Record.ScoringSystem = System.GetById(group.Details.SystemId);
		Record.Conflict = group.Details.Conflict;
		return [];
	}

	private bool HasPlayer(int playerId)
		=> Players.Any(player => player.Id == playerId);
}
