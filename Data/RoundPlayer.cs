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

	protected override string LinkKey => Format($"[{nameof (RoundId)}] = {{0}}", RoundId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<RoundPlayer>();
		RoundId = record.Integer(nameof (RoundId));
		PlayerId = record.Integer(nameof (PlayerId));
		return this;
	}

	#endregion

	#endregion
}
