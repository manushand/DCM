using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal sealed class Round : Rest<Round, Data.Round>
{
	public int Number => Record.Number;

	protected override dynamic Detail => new
										 {
											 TournamentId = Record.Tournament.IsEvent
																? Record.Tournament.Id
																: (int?)null,
											 Games = Record.Tournament.IsEvent
														 ? Record.Games.Select(static game => new Game { Record = game })
														 : null
										 };

	new internal static IEnumerable<Round> GetAll()
		=> throw new FileNotFoundException();
}
