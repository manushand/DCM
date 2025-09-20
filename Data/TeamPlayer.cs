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

	private protected override string LinkKey => $"[{nameof (TeamId)}] = {TeamId}";

	public override void Load(DbDataReader record)
	{
		record.CheckDataType<TeamPlayer>();
		TeamId = record.Integer(nameof (TeamId));
		PlayerId = record.Integer(nameof (PlayerId));
	}

	#endregion

	#endregion
}
