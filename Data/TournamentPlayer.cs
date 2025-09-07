namespace Data;

//	TODO: Maybe rename this (including the db table) to TournamentRegistration?
public sealed class TournamentPlayer : LinkRecord, IInfoRecord
{
	#region Public interface

	#region Data

	public int[] Rounds => [..Range(1, Tournament.TotalRounds).Where(RegisteredForRound)];

	public int TournamentId { get; private set; }

	public string RoundsRegistered
	{
		get
		{
			var roundNumbers = Rounds;
			var roundsRegistered = roundNumbers.Length;
			return roundsRegistered == Tournament.TotalRounds
					   ? "All Rounds"
					   : roundsRegistered is 0
						   ? Empty
						   : $"{"Round".Pluralize(roundsRegistered)} {Join($"{Comma} ", roundNumbers)}";
		}
	}

	internal Tournament Tournament
	{
		private get => field.Id != TournamentId
						   ? field = ReadById<Tournament>(TournamentId)
						   : field;
		init => (field, TournamentId) = (value, value.Id);
	} = Tournament.None;

	#endregion

	#region Methods

	public bool RegisteredForRound(int roundNumber)
		=> (RoundNumbers & 1 << --roundNumber) > 0;

	public void RegisterForRound(int roundNumber)
		=> RoundNumbers |= 1 << --roundNumber;

	public void UnregisterForRound(int roundNumber)
		=> RoundNumbers &= ~(1 << --roundNumber);

	#endregion

	#region IInfoRecord implementation

	#region IRecord implementation

	protected override string LinkKey => Format($"[{nameof (TournamentId)}] = {{0}}", TournamentId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<TournamentPlayer>();
		TournamentId = record.Integer(nameof (TournamentId));
		PlayerId = record.Integer(nameof (PlayerId));
		RoundNumbers = record.Integer(nameof (RoundNumbers));
		return this;
	}

	#endregion

	public string FieldValues => Format($$"""
										  [{{nameof (RoundNumbers)}}] = {0}
										  """,
										RoundNumbers);

	#endregion

	#endregion

	#region Private implementation

	#region Data

	private int RoundNumbers { get; set; } //	...you mean, like zero?  LOL

	#endregion

	#endregion
}
