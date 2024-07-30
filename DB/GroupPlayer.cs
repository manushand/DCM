namespace DCM.DB;

internal sealed class GroupPlayer : LinkRecord
{
	internal int GroupId { get; private set; }

	internal Group Group
	{
		get => ReadById<Group>(GroupId).OrThrow();
		init => GroupId = value.Id;
	}

	#region IRecord interface implementation

	private const string LinkKeyFormat = $"[{nameof (GroupId)}] = {{0}}";

	protected override string LinkKey => Format(LinkKeyFormat, GroupId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<GroupPlayer>();
		GroupId = record.Integer(nameof (GroupId));
		PlayerId = record.Integer(nameof (PlayerId));
		return this;
	}

	#endregion
}
