using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal interface IRest
{
	int Id { get; }
	string Name { get; }
	dynamic? Details { get; }

	static abstract IResult GetOne(int recordId, bool detailed);
	static abstract IResult PutOne(int recordId, object updated);
}
