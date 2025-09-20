namespace Data;

public interface IRecord
{
	internal string PrimaryKey => throw new NotSupportedException();

	public void Load(DbDataReader _)
		=> throw new NotSupportedException();
}
