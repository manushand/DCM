namespace Data;

public sealed class GroupPlayer : LinkRecord
{
	#region Public interface

	#region Data

	public int GroupId { get; private set; }

	public Group Group
	{
		get => ReadById<Group>(GroupId);
		init => GroupId = value.Id;
	}

	#endregion

	#region IRecord implementation

	protected override string LinkKey => Format($"[{nameof (GroupId)}] = {{0}}",
												GroupId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<GroupPlayer>();
		GroupId = record.Integer(nameof (GroupId));
		PlayerId = record.Integer(nameof (PlayerId));
		return this;
	}

	#endregion

	#endregion
}
