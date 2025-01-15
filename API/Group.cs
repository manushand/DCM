using DCM;
using JetBrains.Annotations;

namespace API;

using static Data.Data;

[PublicAPI]
internal class Group : Rest<Group, Data.Group>
{
	protected override dynamic Detail => new
										 {
											 Description = Data.Description.NullIfEmpty(),
											 Data.ScoringSystemId,
											 Data.Conflict,
											 Players = Data.Players.Select(static player => new Player { Data = player }),
											 Games = Data.Games.Select(static game => new Game { Data = game })
										 };

	private bool HasPlayer(int playerId)
		=> Data.Players.Any(player => player.Id == playerId);

	public static IResult GetMembers(int id)
	{
		var group = Lookup(id);
		return group is null
				   ? Results.NotFound()
				   : Results.Ok(group.Detail.Players);
	}

	public static IResult GetNonMembers(int id)
	{
		var record = Lookup(id);
		return record is null
				   ? Results.NotFound()
				   : Results.Ok(ReadAll<Data.Player>().Where(player => !player.Groups.Select(static group => group.Id).Contains(id))
													  .Select(static player => new Player { Data = player }));
	}

	public static IResult AddMember(int id, int playerId)
	{
		var group = Lookup(id);
		var player = Player.Lookup(playerId)?.Data;
		if (group is null || player is null)
			return Results.NotFound();
		if (group.HasPlayer(playerId))
			return Results.Conflict();
		CreateOne(new Data.GroupPlayer { Group = group.Data, Player = player });
		return Results.Created();
	}

	public static IResult DropMember(int id, int playerId)
	{
		var group = Lookup(id);
		var player = Player.Lookup(playerId)?.Data;
		if (group is null || player is null)
			return Results.NotFound();
		if (!group.HasPlayer(playerId))
			return Results.NoContent();
		Delete(ReadOne<Data.GroupPlayer>(groupPlayer => groupPlayer.GroupId == id && groupPlayer.PlayerId == playerId).OrThrow());
		return Results.Ok();
	}

	public override bool Unlink()
	{
		Delete(ReadMany<Data.GroupPlayer>(groupPlayer => groupPlayer.GroupId == Id));
		return true;
	}
}
