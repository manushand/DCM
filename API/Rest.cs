using System.Text.Json;
using static Newtonsoft.Json.Linq.JObject;

namespace API;

using DCM;
using Data;
using static Data.Data;

internal abstract class Rest<T1, T2> : IRest
	where T1 : Rest<T1, T2>, new()
	where T2 : IdInfoRecord, new()
{
	public int Id => Record.Id;
	public virtual string Name => Record.Name;
	public dynamic? Details => Detailed ? Detail : null;

	private bool Detailed { get; set; }
	protected abstract dynamic Detail { get; }

	private readonly T2? _record;
	protected internal T2 Record
	{
		get => _record.OrThrow();
		init => _record = value;
	}

	internal static IEnumerable<T1> GetAll()
		=> ReadAll<T2>().Select(static iRecord => new T1 { Record = iRecord });

	public static IResult GetOne(int recordId, bool detailed)
	{
		var record = Lookup(recordId);
		if (record is null)
			return Results.NotFound();
		record.Detailed = detailed;
		return Results.Ok(record);
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
		UpdateOne(record.Record);
		return Results.NoContent();
	}

	protected virtual void Update(dynamic record)
		=> throw new NotImplementedException();

	internal static T1? Lookup(int recordId)
	{
		var record = ReadByIdOrNull<T2>(recordId);
		return record is null
				   ? null
				   : new T1 { Record = record, Detailed = true };
	}
}
