namespace Data;

public sealed class RoundPlayer : LinkRecord
{
	#region Public interface

	#region Data

	public Round Round
	{
		init => RoundId = value.Id;
	}

	internal int RoundId { get; private set; }

	#endregion

	#region IRecord implementation

	private protected override string LinkKey => $"[{nameof (RoundId)}] = {RoundId}";

	public override void Load(DbDataReader record)
	{
		record.CheckDataType<RoundPlayer>();
		RoundId = record.Integer(nameof (RoundId));
		PlayerId = record.Integer(nameof (PlayerId));
	}

	#endregion

	#endregion
}
