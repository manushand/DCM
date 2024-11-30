namespace DCM.DB;

internal sealed class TeamPlayer : LinkRecord
{
	internal int TeamId { get; private set; }

	internal Team Team
	{
		get => field == Team.Null
				   ? field = ReadById<Team>(TeamId).OrThrow()
				   : field;
		init => (field, TeamId) = (value, value.Id);
	} = Team.Null;

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
