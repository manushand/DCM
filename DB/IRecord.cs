namespace DCM.DB;

internal interface IRecord
{
	[Browsable(false)]
	public string PrimaryKey => throw new NotSupportedException();

	public IRecord Load(DbDataReader _)
		=> throw new NotSupportedException();
}
