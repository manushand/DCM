namespace API;

[PublicAPI]
internal sealed class Round : Rest<Round, Data.Round, Round.RoundDetails>
{
	public int Number => Record.Number;
	public bool Workable => Record.Workable;
	public Statuses Status => Record.Status;
	public int? SystemId { get; set; }

	internal sealed class RoundDetails : DetailClass;

	protected override RoundDetails Detail => new ();

	private IEnumerable<Game> Games => Game.RestFrom(Record.Games);
}
