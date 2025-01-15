using JetBrains.Annotations;
namespace API;

[PublicAPI]
internal interface IRest
{
	int Id { get; }
	string Name { get; }
	dynamic? Details { get; }

	static virtual string Endpoint { get; } = string.Empty;

	static abstract IResult GetOne(int recordId);
	static abstract IResult PutOne(int recordId, object updated);

	bool Unlink();
}
