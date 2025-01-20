namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team, Team.Detail>
{
	public int Id => Identity;
	public string Name => RecordedName;

	internal sealed record Detail : DetailClass;

	protected override Detail Info => new ();

	private IEnumerable<Player> Players => Player.RestFrom(Record.Players);
}
