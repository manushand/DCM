namespace Data;

public sealed class GroupPlayer : LinkRecord
{
	#region Public interface

	#region Data

	public int GroupId { get; private set; }

	public Group Group
	{
		get => ReadById<Group>(GroupId);
		internal init => GroupId = value.Id;
	}

	#endregion

	#region IRecord implementation

	private protected override string LinkKey => $"[{nameof (GroupId)}] = {GroupId}";

	public override void Load(DbDataReader record)
	{
		record.CheckDataType<GroupPlayer>();
		GroupId = record.Integer(nameof (GroupId));
		PlayerId = record.Integer(nameof (PlayerId));
	}

	#endregion

	#endregion
}
