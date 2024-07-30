namespace DCM.DB;

/// <summary>
///     Abstract class for IRecord types having a primary key consisting
///     of two fields, such that .PrimaryKey.Contains(And) is true, and
///     supporting a Player and PlayerId field.
/// </summary>
internal abstract class LinkRecord : IRecord
{
	private Player? _player;

	internal Player Player
	{
		get => _player ??= ReadById<Player>(PlayerId).OrThrow();
		set => (_player, PlayerId) = (value, value.Id);
	}

	internal virtual int PlayerId { get; private protected set; }

	protected virtual string PlayerIdColumnName => nameof (PlayerId);

	protected abstract string LinkKey { get; }

	private string PlayerLinkKey => $"[{PlayerIdColumnName}] = {PlayerId}";

	internal IEnumerable<string> KeyFieldAssignments => [PlayerLinkKey, LinkKey];

	[Browsable(false)]
	public string PrimaryKey => $"{PlayerLinkKey} AND {LinkKey}";

	public abstract IRecord Load(DbDataReader record);
}
