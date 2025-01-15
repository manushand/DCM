using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal sealed class Round : Rest<Round, Data.Round>
{
	public int Number => Data.Number;

	protected override dynamic Detail => new
										 {
											 TournamentId = Data.Tournament.IsEvent
																? Data.Tournament.Id
																: (int?)null,
											 Games = Data.Tournament.IsEvent
														 ? Data.Games.Select(static game => new Game { Data = game })
														 : null
										 };

	new internal static IEnumerable<Round> GetAll()
		=> throw new NotImplementedException();
}
