using System.Text.Json;
using static Newtonsoft.Json.Linq.JObject;

namespace API;

using Data;
using static Data.Data;

internal abstract class Rest<T1, T2> : IRest
	where T1 : Rest<T1, T2>, new()
	where T2 : IdInfoRecord, new()
{
	public int Id => Data.Id;
	public virtual string Name => Data.Name;
	public dynamic? Details => Detailed ? Detail : null;

	private bool Detailed { get; init; }
	protected abstract dynamic Detail { get; }

	protected internal T2 Data { get; init; } = new ();

	public static IEnumerable<T1> GetAll()
		=> ReadAll<T2>().Select(static idInfoRecord => new T1 { Data = idInfoRecord });

	public static IResult GetOne(int recordId)
	{
		var record = Lookup(recordId);
		return record is null
				   ? Results.NotFound()
				   : Results.Ok(record);
	}

	public static IResult PutOne(int recordId, object updated)
	{
		var record = Lookup(recordId);
		if (record is null)
			return Results.NotFound();
		//	TODO: Somehow (?) check that updated is T1
		dynamic dynamo = Parse(JsonSerializer.Serialize(updated));
		if (dynamo.Id != recordId)
			return Results.BadRequest("Ids do not match");
		record.Update(dynamo);
		UpdateOne(record.Data);
		return Results.NoContent();
	}

	protected virtual void Update(dynamic record)
		=> throw new NotImplementedException();

	internal static T1? Lookup(int recordId)
	{
		var record = ReadByIdOrNull<T2>(recordId);
		return record is null
				   ? null
				   : new T1 { Data = record, Detailed = true };
	}
}
