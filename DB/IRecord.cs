namespace DCM.DB;

internal interface IRecord
{
	[Browsable(false)]
	internal string PrimaryKey => throw new NotSupportedException();

	internal IRecord Load(DbDataReader _)
		=> throw new NotSupportedException();
}
