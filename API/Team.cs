using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Team : Rest<Team, Data.Team>
{
	protected override dynamic Detail => new
										 {
											 Data.TournamentId,
											 Players = Data.Players.Select(static player => new Player { Data = player })
											 //	Score, Games, et al.?
										 };

	new public static IEnumerable<Team> GetAll()
		=> throw new FileNotFoundException();
}
