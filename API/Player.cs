using Data;

namespace API;

using static DCM.DCM;
using static Data.Data;

[PublicAPI]
internal class Player : Rest<Player, Data.Player, Player.Detail>
{
	public int Id => Identity;

	public string FirstName
	{
		get => Record.FirstName;
		private set => Record.FirstName = value;
	}
	public string LastName
	{
		get => Record.LastName;
		private set => Record.LastName = value;
	}

	[PublicAPI]
	internal sealed class Detail : DetailClass
	{
		required public ICollection<string>? EmailAddresses { get; set; }
	}

	protected override Detail Info => new ()
											   {
												   EmailAddresses = Record.EmailAddresses.NullIfEmpty()
											   };

	private IEnumerable<Game> Games => Game.RestFrom(Record.Games);

	new private protected static void CreateNonCrudEndpoints(WebApplication app, string tag)
	{
		app.MapGet("player/{id:int}/games", GetGames)
		   .WithName("GetPlayerGames")
		   .WithDescription("List all games in which a player was involved.")
		   .Produces<IEnumerable<Game>>()
		   .Produces(Status404NotFound)
		   .WithTags(tag);

		app.MapGet("player/{id:int}/groups", GetGroups)
		   .WithName("GetPlayerGroups")
		   .WithDescription("List all groups to which a player belongs.")
		   .Produces<IEnumerable<Group>>()
		   .Produces(Status404NotFound)
		   .WithTags(tag);

		//	TODO
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

	private protected override string[] Update(Player player)
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
		Record.EmailAddress = string.Join(",", addresses);
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
}
