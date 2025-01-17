using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal sealed class Round : Rest<Round, Data.Round, Round.RoundDetails>
{
	public int Number => Record.Number;
	public bool Workable => Record.Workable;
	public Data.Game.Statuses Status => Record.Status;

	[PublicAPI]
	public sealed class RoundDetails : DetailClass
	{
		public int TournamentId { get; set; }
		public int? SystemId { get; set; }
	}

	protected override RoundDetails Detail => new ()
											  {
												  TournamentId = Record.Tournament.Id,
												  SystemId = Record.ScoringSystemId
											  };

	private IEnumerable<Game> Games => Record.Games.Select(static game => new Game { Record = game });
}
