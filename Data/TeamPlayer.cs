namespace Data;

public sealed class TeamPlayer : LinkRecord
{
	#region Public interface

	#region Data

	public Team Team
	{
		get => field.Id == TeamId
				   ? field
				   : field = ReadById<Team>(TeamId);
		internal init => (field, TeamId) = (value, value.Id);
	} = Team.None;

	internal int TeamId { get; private set; }

	#endregion

	#region IRecord implementation

	protected override string LinkKey => Format($"[{nameof (TeamId)}] = {{0}}", TeamId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<TeamPlayer>();
		TeamId = record.Integer(nameof (TeamId));
		PlayerId = record.Integer(nameof (PlayerId));
		return this;
	}

	#endregion

	#endregion
}
