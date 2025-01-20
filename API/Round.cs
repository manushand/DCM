namespace API;

[PublicAPI]
internal sealed class Round : Rest<Round, Data.Round, Round.Detail>
{
	public int Number => Record.Number;
	public bool Workable => Record.Workable;
	public Statuses Status => Record.Status;
	public int? SystemId { get; set; }

	internal sealed record Detail : DetailClass;

	protected override Detail Info => new ();

	private IEnumerable<Game> Games => Game.RestFrom(Record.Games);
}
