namespace Data;

/// <summary>
///     Abstract class for IRecord types having a primary key consisting
///     of two fields, such that .PrimaryKey.Contains(And) is true, and
///     supporting a Player and PlayerId field.
/// </summary>
public abstract class LinkRecord : IRecord
{
	public virtual int PlayerId { get; private protected set; }

	public Player Player
	{
		get => field.Id == PlayerId
				   ? field
				   : field = ReadById<Player>(PlayerId);
		set => (field, PlayerId) = (value, value.Id);
	} = Player.None;

	public string PrimaryKey => Join(" AND ", KeyFieldAssignments);

	public abstract void Load(DbDataReader record);

	internal IEnumerable<string> KeyFieldAssignments => [PlayerLinkKey, LinkKey];

	protected virtual string PlayerIdColumnName => nameof (PlayerId);

	private protected abstract string LinkKey { get; }

	private string PlayerLinkKey => $"[{PlayerIdColumnName}] = {PlayerId}";
}
