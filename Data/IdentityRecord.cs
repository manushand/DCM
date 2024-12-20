namespace Data;

public abstract class IdentityRecord<T> : IdInfoRecord
	where T : IdInfoRecord, new()
{
	public static T None { get; } = new () { Name = "── NONE ──" };
	public bool IsNone => this == None;
}
