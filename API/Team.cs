namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team, Team.Detail>
{
	public IEnumerable<Player> Players => Player.RestFrom(Record.Players);

	internal sealed class Detail : DetailClass;
}
