using Data;

namespace API;

using DCM;

[PublicAPI]
internal class Player : Rest<Player, Data.Player, Player.Detail>
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;

	private IEnumerable<Game> Games => Game.RestFrom(Record.Games);

	[PublicAPI]
	internal sealed class Detail : DetailClass
	{
		required public ICollection<string>? EmailAddresses { get; set; }
	}

	private protected override void LoadFromDataRecord(Data.Player record)
	{
		FirstName = record.FirstName;
		LastName = record.LastName;
		Info = new ()
			   {
				   EmailAddresses = Record.EmailAddresses.NullIfEmpty()
			   };
	}

	internal static void CreateEndpoints(WebApplication app)
	{
		CreateCrudEndpoints(app);

		app.MapGet("player/{id:int}/games", GetGames)
		   .WithName("GetPlayerGames")
		   .WithDescription("List all games in which a player was involved.")
		   .Produces<IEnumerable<Game>>()
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);

		app.MapGet("player/{id:int}/groups", GetGroups)
		   .WithName("GetPlayerGroups")
		   .WithDescription("List all groups to which a player belongs.")
		   .Produces<IEnumerable<Group>>()
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);

		app.MapGet("player/{id:int}/conflicts", GetConflicts)
		   .WithName("GetPlayerConflicts")
		   .WithDescription("Get the list of all conflicts for a player.")
		   .Produces<IEnumerable<Conflict>>()
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);

		app.MapPatch("player/{id:int}/conflict/{playerId:int}", SetPlayerConflict)
		   .WithName("GetOrSetPlayerConflict")
		   .WithDescription("Get or update a player conflict.")
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
	}

	public static IResult GetGames(int id)
	{
		var player = RestForId(id)?.Record;
		return player is null
				   ? NotFound()
				   : Ok(Game.RestFrom(player.Games));
	}

	public static IResult GetGroups(int id)
	{
		var player = RestForId(id)?.Record;
		return player is null
				   ? NotFound()
				   : Ok(Group.RestFrom(player.Groups));
	}

	private protected override string[] UpdateRecordForDatabase(Player player)
	{
		var first = player.FirstName.Trim();
		var last = player.LastName.Trim();
		if (first.Length is 0 || last.Length is 0)
			return ["Player first and last names are required."];

		Record.FirstName = first;
		Record.LastName = last;
		var addresses = player.Details?.EmailAddresses ?? [];
		if (addresses.Any(static address => !address.IsValidEmail()))
			return ["Invalid player email address."];
		Record.EmailAddress = Join(",", addresses);
		return [];
	}

	public override bool Unlink()
	{
		var hasPlayedGames = Record.LinksOfType<GamePlayer>().Length is not 0;
		if (hasPlayedGames)
			return false;
		Delete(Record.LinksOfType<GroupPlayer>());
		Delete(Record.LinksOfType<TeamPlayer>());
		Delete(Record.LinksOfType<RoundPlayer>());
		Delete(Record.LinksOfType<TournamentPlayer>());
		return true;
	}

	[PublicAPI]
	private sealed record Conflict(Player Player, int Value);

	public static IResult GetConflicts(int id)
		=> RestForId(id) is null
			   ? NotFound()
			   : Ok(ReadMany<PlayerConflict>(pc => pc.Involves(id))
						.Select(pc => new Conflict(RestFrom(pc.PlayerConflictedWith(id)), pc.Value)));

	public static IResult SetPlayerConflict(int id,
											int playerId,
											int? value)
	{
		var player = RestForId(id);
		var other = RestForId(playerId, false);
		if (player is null || other is null)
			return NotFound();
		var playerConflict = ReadOne<PlayerConflict>(conflict => conflict.Involves(id) && conflict.Involves(playerId));
		var result = new Conflict(other, value ?? playerConflict?.Value ?? default);
		if (value is null)
			return Ok(result);
		if (playerConflict is not null)
			if (value is 0)
				Delete(playerConflict);
			else
			{
				playerConflict.Value = value.Value;
				UpdateOne(playerConflict);
			}
		else if (value is not 0)
			CreateOne(new PlayerConflict(id, playerId) { Value = value.Value });
		return Ok(result);
	}
}
