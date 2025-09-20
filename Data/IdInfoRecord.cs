namespace Data;

public abstract class IdInfoRecord : IInfoRecord, IComparable<IdInfoRecord>
{
	#region Public interface

	#region Type

	public interface IEvent;

	#endregion

	#region Data

	public int Id;
	public abstract string FieldValues { get; }
	public virtual string Name { get; set; } = Empty;

	#endregion

	#region Methods

	public bool Is(IdInfoRecord other)
		=> Id == other.Id;

	public bool IsNot(IdInfoRecord other)
		=> !Is(other);

	public sealed override string ToString()
		=> Name;

	#endregion

	#region IRecord implementation

	private const string PrimaryKeyFormat = $"[{nameof (Id)}] = {{0}}";

	public string PrimaryKey => Format(PrimaryKeyFormat, Id);

	public abstract void Load(DbDataReader record);

	#endregion

	#region IComparable implementation

	public int CompareTo(IdInfoRecord? other)
		=> Compare(Name, (other?.Name).OrThrow(), StringComparison.InvariantCultureIgnoreCase);

	#endregion

	#endregion
}
