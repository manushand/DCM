namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team, Team.TeamDetails>
{
	public int Id => Identity;
	public string Name => RecordedName;

	internal sealed class TeamDetails : DetailClass;

	protected override TeamDetails Detail => new ();

	private IEnumerable<Player> Players => Player.RestFrom(Record.Players);
}
