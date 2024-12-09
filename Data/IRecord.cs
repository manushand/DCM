namespace Data;

public interface IRecord
{
	internal string PrimaryKey => throw new NotSupportedException();

	internal IRecord Load(DbDataReader _)
		=> throw new NotSupportedException();
}
