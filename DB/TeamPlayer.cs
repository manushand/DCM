namespace DCM.DB;

internal sealed class TeamPlayer : LinkRecord
{
	private Team? _team;
	internal int TeamId { get; private set; }

	internal Team Team
	{
		get => _team ??= ReadById<Team>(TeamId).OrThrow();
		init => (_team, TeamId) = (value, value.Id);
	}

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
