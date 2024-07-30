namespace DCM.DB;

internal sealed class PlayerConflict : LinkRecord, IInfoRecord
{
	internal readonly int[] PlayerIds = new int[2];

	internal int Value;

	internal override int PlayerId => PlayerIds[0];
	private int OtherPlayerId => PlayerIds[1];

	private IEnumerable<Player> Players => ReadMany<Player>(player => PlayerIds.Contains(player.Id));

	[UsedImplicitly]
	public PlayerConflict() { }

	internal PlayerConflict(int playerId,
							int conflictedPlayerId)
	{
		if (playerId <= 0)
			throw new ArgumentException("Bad playerId", nameof (playerId));
		if (conflictedPlayerId <= 0 || playerId == conflictedPlayerId)
			throw new ArgumentException("Bad or identical conflicted playerId", nameof (conflictedPlayerId));
		Value = 1;
		PlayerIds[0] = Math.Min(playerId, conflictedPlayerId);
		PlayerIds[1] = Math.Max(playerId, conflictedPlayerId);
	}

	internal Player PlayerConflictedWith(int playerId)
		=> Players.Single(player => player.Id != playerId);

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	protected override string PlayerIdColumnName => "player1";
	protected override string LinkKey => $"[{OtherPlayerColumnName}] = {OtherPlayerId}";

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<PlayerConflict>();
		PlayerIds[0] = record.Integer(PlayerIdColumnName);
		PlayerIds[1] = record.Integer(OtherPlayerColumnName);
		Value = record.Integer(nameof (Value));
		return this;
	}

	private const string OtherPlayerColumnName = "player2";

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (Value)}}] = {0}
	                                           """;

	public string FieldValues => Format(FieldValuesFormat, Value);

	#endregion
}
