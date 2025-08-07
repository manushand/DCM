namespace Data;

public abstract class IdInfoRecord : IInfoRecord, IComparable<IdInfoRecord>
{
	public interface IEvent;

	public int Id;
	public abstract string FieldValues { get; }
	public virtual string Name { get; set; } = Empty;

	public bool Is(IdInfoRecord other)
		=> Id == other.Id;

	public bool IsNot(IdInfoRecord other)
		=> !Is(other);

	#region IRecord interface implementation

	private const string PrimaryKeyFormat = $"[{nameof (Id)}] = {{0}}";

	public string PrimaryKey => Format(PrimaryKeyFormat, Id);

	public abstract IRecord Load(DbDataReader record);

	#endregion

	#region IComparable interface implementation

	public int CompareTo(IdInfoRecord? other)
		=> Compare(Name, (other?.Name).OrThrow(), StringComparison.InvariantCultureIgnoreCase);

	#endregion

	public sealed override string ToString()
		=> Name;
}
