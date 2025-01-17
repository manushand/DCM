using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team, Team.TeamDetails>
{
	public int Id => Identity;
	public string Name => RecordedName;

	[PublicAPI]
	public sealed class TeamDetails : DetailClass
	{
		public int TournamentId { get; set; }
		//	Score, et al.?
	}

	private IEnumerable<Player> Players => Record.Players.Select(static player => new Player { Record = player });

	protected override TeamDetails Detail => new ()
											 {
												 TournamentId = Record.TournamentId
												 //	Score, Games, et al.?
											 };
}
