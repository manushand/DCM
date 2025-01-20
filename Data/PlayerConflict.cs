namespace Data;

public sealed class PlayerConflict : LinkRecord, IInfoRecord
{
	private (int playerId, int otherPlayerId) PlayerIds { get; set; } = (default, default);

	public int Value;
	public IEnumerable<int> ConflictedPlayerIds => [PlayerIds.playerId, PlayerIds.otherPlayerId];

	public override int PlayerId => PlayerIds.playerId;
	private int OtherPlayerId => PlayerIds.otherPlayerId;

	private IEnumerable<Player> Players => ReadMany<Player>(player => Involves(player.Id));

	[UsedImplicitly]
	public PlayerConflict() { }

	public PlayerConflict(int playerId,
						  int conflictedPlayerId)
	{
		if (playerId <= 0)
			throw new ArgumentException("Bad playerId", nameof (playerId));
		if (conflictedPlayerId <= 0 || playerId == conflictedPlayerId)
			throw new ArgumentException("Bad or identical conflicted playerId", nameof (conflictedPlayerId));
		Value = 1;
		PlayerIds = (Math.Min(playerId, conflictedPlayerId), Math.Max(playerId, conflictedPlayerId));
	}

	public Player PlayerConflictedWith(int playerId)
		=> Players.Single(player => player.Id != playerId);

	public bool Involves(int playerId)
		=> playerId == PlayerIds.playerId || OtherPlayerId == playerId;

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	protected override string PlayerIdColumnName => "player1";
	protected override string LinkKey => $"[{OtherPlayerColumnName}] = {OtherPlayerId}";

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<PlayerConflict>();
		PlayerIds = (record.Integer(PlayerIdColumnName), record.Integer(OtherPlayerColumnName));
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
