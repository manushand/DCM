namespace Data;

//	TODO: Maybe rename this (including the db table) to TournamentRegistration?
public sealed class TournamentPlayer : LinkRecord, IInfoRecord
{
	public int TournamentId { get; private set; }
	private int RoundNumbers { get; set; } //	...you mean, like zero?  LOL

	internal Tournament Tournament
	{
		private get  => field.IsNone
							? field = ReadById<Tournament>(TournamentId).OrThrow()
							: field;
		init => (field, TournamentId) = (value, value.Id);
	} = Tournament.None;

	public string RoundsRegistered
	{
		get
		{
			int[] roundNumbers = [..Range(1, Tournament.TotalRounds).Where(RegisteredForRound)];
			var roundsRegistered = roundNumbers.Length;
			return roundsRegistered == Tournament.TotalRounds
					   ? "All Rounds"
					   : roundsRegistered is 0
						   ? Empty
						   : $"{"Round".Pluralize(roundsRegistered)} {Join($"{Comma} ", roundNumbers)}";
		}
	}

	public bool RegisteredForRound(int roundNumber)
		=> (RoundNumbers & 1 << --roundNumber) > 0;

	public void RegisterForRound(int roundNumber)
		=> RoundNumbers |= 1 << --roundNumber;

	public void UnregisterForRound(int roundNumber)
		=> RoundNumbers &= ~(1 << --roundNumber);

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	private const string LinkKeyFormat = $"[{nameof (TournamentId)}] = {{0}}";

	protected override string LinkKey => Format(LinkKeyFormat, TournamentId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<TournamentPlayer>();
		TournamentId = record.Integer(nameof (TournamentId));
		PlayerId = record.Integer(nameof (PlayerId));
		RoundNumbers = record.Integer(nameof (RoundNumbers));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
											   [{{nameof (RoundNumbers)}}] = {0}
											   """;

	public string FieldValues => Format(FieldValuesFormat, RoundNumbers);

	#endregion
}
