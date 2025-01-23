namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team, Team.Detail>
{
	internal sealed class Detail : DetailClass;

	private IEnumerable<Player> Players => Player.RestFrom(Record.Players);
}
