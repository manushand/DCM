namespace Data;

public abstract class IdentityRecord<T> : IdInfoRecord
	where T : class, new()
{
	public static T None { get; } = new ();
}
