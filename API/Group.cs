using Data;

namespace API;

using DCM;
using static Data.Data;

[PublicAPI]
internal class Group : Rest<Group, Data.Group, Group.GroupDetails>
{
	public int Id => Identity;
	public string Name => RecordedName;

	[PublicAPI]
	internal sealed class GroupDetails : DetailClass
	{
		required public string? Description { get; set; }
		required public int SystemId { get; set; }
		required public int Conflict { get; set; }
	}

	protected override GroupDetails Detail => new ()
											  {
												  Description = Record.Description.NullIfEmpty(),
												  SystemId = Record.ScoringSystemId,
												  Conflict = Record.Conflict
											  };

	private IEnumerable<Player> Players => Player.RestFrom(Record.Players);
	private IEnumerable<Game> Games => Game.RestFrom(Record.Games);

	new private protected static void CreateNonCrudEndpoints(WebApplication app, string tag)
	{
		app.MapGet("group/{id:int}/games", GetGames)
		   .WithName("GetGroupGames")
		   .WithDescription("List all games played by the group.")
		   .Produces<Game[]>()
		   .WithTags(tag);
		app.MapGet("group/{id:int}/game/{gameNumber:int}", GetGame)
		   .WithName("GetGroupGame")
		   .WithDescription("Get details on a game played by the group.")
		   .Produces<Game>()
		   .WithTags(tag);
		app.MapGet("group/{id:int}/players", /* ?members=true */ GetMembers)
		   .WithName("GetGroupPlayers")
		   .WithDescription("List all players who are members or non-members of the group.")
		   .Produces<Player[]>()
		   .WithTags(tag);
		app.MapPatch("group/{id:int}/player/{playerId:int}", ChangeMembership)
		   .WithName("ChangeGroupPlayerMembership")
		   .WithDescription("Add or remove a player from the group.")
		   .Produces(Status200OK)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
	}

	public static IResult GetGames(int id)
	{
		var record = RestForId(id);
		return record is null
				   ? NotFound()
				   : Ok(record.Games.OrderBy(static game => game.Number));
	}

	public static IResult GetGame(int id, int gameNumber)
	{
		var game = RestForId(id)?.Games.SingleOrDefault(game => game.Number == gameNumber);
		game?.AddDetail();
		return game is null
				   ? NotFound()
				   : Ok(game);
	}

	public static IResult GetMembers(int id, bool members = true)
	{
		var record = RestForId(id);
		if (record is null)
			return NotFound();
		if (members)
			return Ok(record.Players);
		var memberIds = record.Players
							  .Select(static player => player.Identity)
							  .ToList();
		return Ok(Player.RestFrom(Player.GetMany(player => !memberIds.Contains(player.Id))));
	}

	public static IResult ChangeMembership(int id, int playerId, bool member)
	{
		var group = RestForId(id);
		var player = Player.RestForId(playerId);
		if (group is null || player is null)
			return NotFound();
		if (member == group.HasPlayer(playerId))
			return NoContent();
		if (member)
			CreateOne(new GroupPlayer { Group = group.Record, Player = player.Record });
		else
			//	TODO - Hmm.  Can a player be dropped even if they've played games that are group games?
			Delete(ReadOne<GroupPlayer>(groupPlayer => groupPlayer.GroupId == id && groupPlayer.PlayerId == playerId).OrThrow());
		return Ok();
	}

	public override bool Unlink()
	{
		Delete(ReadMany<GroupPlayer>(groupPlayer => groupPlayer.GroupId == Id));
		return true;
	}

	private protected override string[] Update(Group group)
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
		=> Players.Any(player => player.Identity == playerId);
}
