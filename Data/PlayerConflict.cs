namespace Data;

public sealed class PlayerConflict : LinkRecord, IInfoRecord
{
	#region Public interface

	#region Data

	public int Value;

	public IEnumerable<int> ConflictedPlayerIds => [PlayerIds.playerId, PlayerIds.otherPlayerId];
	public override int PlayerId => PlayerIds.playerId;

	#endregion

	#region Constructors

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

	#endregion

	#region Methods

	public Player PlayerConflictedWith(int playerId)
		=> Players.Single(player => player.Id != playerId);

	public bool Involves(int playerId)
		=> playerId == PlayerIds.playerId || OtherPlayerId == playerId;

	#endregion

	#region IInfoRecord implementation

	#region IRecord implementation

	protected override string PlayerIdColumnName => "player1";
	protected override string LinkKey => $"[{OtherPlayerColumnName}] = {OtherPlayerId}";

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<PlayerConflict>();
		PlayerIds = (record.Integer(PlayerIdColumnName), record.Integer(OtherPlayerColumnName));
		Value = record.Integer(nameof (Value));
		return this;
	}

	#endregion

	public string FieldValues => Format($$"""
										  [{{nameof (Value)}}] = {0}
										  """, Value);

	#endregion

	#endregion

	#region Private implementation

	#region Data

	private const string OtherPlayerColumnName = "player2";

	private (int playerId, int otherPlayerId) PlayerIds { get; set; }
	private int OtherPlayerId => PlayerIds.otherPlayerId;
	private IEnumerable<Player> Players => ReadMany<Player>(player => Involves(player.Id));

	#endregion

	#endregion
}
