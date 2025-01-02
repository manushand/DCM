using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Group : Rest<Group, Data.Group>
{
	protected override dynamic Detail => new
										 {
											 Description = Record.Description.NullIfEmpty(),
											 Games = Record.Games.Select(static game => new Game { Record = game })
										 };
}
