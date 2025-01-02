using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team>
{
	protected override dynamic Detail => new
										 {
											 Record.TournamentId
										 };
}
