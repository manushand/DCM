namespace Data;

public abstract class IdentityRecord : IInfoRecord, IComparable<IdentityRecord>
{
	public int Id;

	public virtual string Name { get; set; } = Empty;

	public bool Is(IdentityRecord other)
		=> Id == other.Id;

	public bool IsNot(IdentityRecord other)
		=> !Is(other);

	#region IComparable interface implementation

	//	TODO: In C# 11 (maybe), the parameter can be "other!!" and the null check/throw removed
	public int CompareTo(IdentityRecord? other)
		=> Compare(Name, (other?.Name).OrThrow(), StringComparison.InvariantCultureIgnoreCase);

	#endregion

	public sealed override string ToString()
		=> Name;

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	private const string PrimaryKeyFormat = $"[{nameof (Id)}] = {{0}}";

	public string PrimaryKey => Format(PrimaryKeyFormat, Id);

	public abstract IRecord Load(DbDataReader record);

	#endregion

	public abstract string FieldValues { get; }

	#endregion
}
