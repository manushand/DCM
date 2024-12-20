namespace Data;

public sealed class TeamPlayer : LinkRecord
{
	internal int TeamId { get; private set; }

	public Team Team
	{
		get => field.IsNone
				   ? field = ReadById<Team>(TeamId).OrThrow()
				   : field;
		internal init => (field, TeamId) = (value, value.Id);
	} = Team.None;

	#region IRecord interface implementation

	private const string LinkKeyFormat = $"[{nameof (TeamId)}] = {{0}}";

	protected override string LinkKey => Format(LinkKeyFormat, TeamId);

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<TeamPlayer>();
		TeamId = record.Integer(nameof (TeamId));
		PlayerId = record.Integer(nameof (PlayerId));
		return this;
	}

	#endregion
}
