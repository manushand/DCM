namespace Data;

public sealed class RoundPlayer : LinkRecord
{
	internal int RoundId { get; private set; }

	public Round Round
	{
		init => RoundId = value.Id;
	}

	#region IRecord interface implementation

	private const string LinkKeyFormat = $"[{nameof (RoundId)}] = {{0}}";

	protected override string LinkKey => Format(LinkKeyFormat, RoundId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<RoundPlayer>();
		RoundId = record.Integer(nameof (RoundId));
		PlayerId = record.Integer(nameof (PlayerId));
		return this;
	}

	#endregion
}
