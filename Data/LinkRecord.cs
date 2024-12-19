namespace Data;

/// <summary>
///     Abstract class for IRecord types having a primary key consisting
///     of two fields, such that .PrimaryKey.Contains(And) is true, and
///     supporting a Player and PlayerId field.
/// </summary>
public abstract class LinkRecord : IRecord
{
	public Player Player
	{
		get => field == Player.None
				   ? field = ReadById<Player>(PlayerId).OrThrow()
				   : field;
		set => (field, PlayerId) = (value, value.Id);
	} = Player.None;

	public virtual int PlayerId { get; private protected set; }

	protected virtual string PlayerIdColumnName => nameof (PlayerId);

	protected abstract string LinkKey { get; }

	private string PlayerLinkKey => $"[{PlayerIdColumnName}] = {PlayerId}";

	internal IEnumerable<string> KeyFieldAssignments => [PlayerLinkKey, LinkKey];

	public string PrimaryKey => Join(" AND ", KeyFieldAssignments);

	public abstract IRecord Load(DbDataReader record);
}
