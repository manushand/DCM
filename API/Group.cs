using JetBrains.Annotations;

namespace API;

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
}
