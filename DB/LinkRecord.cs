namespace DCM.DB;

/// <summary>
///     Abstract class for IRecord types having a primary key consisting
///     of two fields, such that .PrimaryKey.Contains(And) is true, and
///     supporting a Player and PlayerId field.
/// </summary>
internal abstract class LinkRecord : IRecord
{
	internal Player Player
	{
		get => field == Player.Empty
				   ? field = ReadById<Player>(PlayerId).OrThrow()
				   : field;
		set => (field, PlayerId) = (value, value.Id);
	} = Player.Empty;

	internal virtual int PlayerId { get; private protected set; }

	protected virtual string PlayerIdColumnName => nameof (PlayerId);

	protected abstract string LinkKey { get; }

	private string PlayerLinkKey => $"[{PlayerIdColumnName}] = {PlayerId}";

	internal IEnumerable<string> KeyFieldAssignments => [PlayerLinkKey, LinkKey];

	[Browsable(false)]
	public string PrimaryKey => $"{PlayerLinkKey} AND {LinkKey}";

	public abstract IRecord Load(DbDataReader record);
}
