namespace API;

using DCM;
using Data;

[PublicAPI]
internal abstract class Rest<T1, T2, T3> : IRest
	where T1 : Rest<T1, T2, T3>, new()
	where T2 : IdInfoRecord, new()
	where T3 : Rest<T1, T2, T3>.DetailClass
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public T3? Details
	{
		get => Detailed ? Info : null;
		set => Info = value.OrThrow();
	}

	internal abstract class DetailClass;
	internal T2 Record { get; set; } = new ();

	private protected bool Detailed { private get; set; }
	private protected T3? Info { get; set; }

	private static readonly Type Type = typeof (T1);
	private static string TypeName => SwaggerTag.ToLower();

	private protected static string SwaggerTag => Type.Name;

	internal static void CreateCrudEndpoints(WebApplication app)
	{
		var articled = $"a{(Type == typeof (Event) ? 'n' : null)} {TypeName}";

		app.MapGet($"{TypeName}s", GetAll)
		   .WithName($"List{SwaggerTag}s")
		   .WithDescription($"List all {TypeName}s.")
		   .Produces(Status200OK, Type.MakeArrayType())
		   .WithTags(SwaggerTag);
		app.MapGet($"{TypeName}/{{id:int}}", GetOne)
		   .WithName($"Get{SwaggerTag}Details")
		   .WithDescription($"Get details for {articled}.")
		   .Produces(Status200OK, Type)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
		app.MapPost(TypeName, PostOne)
		   .WithName($"Add{SwaggerTag}")
		   .WithDescription($"Add a new {TypeName}.")
		   .Produces(Status201Created)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .WithTags(SwaggerTag);
		app.MapPut($"{TypeName}/{{id:int}}", PutOne)
		   .WithName($"Update{SwaggerTag}")
		   .WithDescription($"Update details for {articled}.")
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .WithTags(SwaggerTag);
		app.MapDelete($"{TypeName}/{{id:int}}", DeleteOne)
		   .WithName($"Delete{SwaggerTag}")
		   .WithDescription($"Delete {articled}.")
		   .Produces(Status204NoContent)
		   .Produces(Status404NotFound)
		   .WithTags(SwaggerTag);
	}

	private protected static IResult GetAll()
		=> Ok(RestFrom(ReadAll<T2>().Where(static t2 => t2 is not Tournament { IsEvent: false })));

	private protected static IResult GetOne(int id)
	{
		var record = RestForId(id);
		return record is null
				   ? NotFound()
				   : Ok(record);
	}

	private protected static IResult PutOne(int id,
											T1 updated)
	{
		var rest = RestForId(id);
		if (rest is null)
			return NotFound();
		var recordId = updated.Id;
		if (updated is not Game && (ReadOne<T2>(@object => @object.Name == updated.Name)?.Id ?? id) != id)
			return Conflict("Proposed new name already in use.");
		var issues = id != recordId
						 ? ["IDs do not match."]
						 : updated.Name.Length is 0
							 ? ["Name is required"]
							 : rest.UpdateRecordForDatabase(updated);
		if (issues.Length is not 0)
			return BadRequest(issues);
		UpdateOne(rest.Record);
		return NoContent();
	}

	private protected static IResult PostOne(HttpRequest request,
											 T1 candidate)
	{
		var record = new T1();
		var named = candidate.Name.Length > 0;
		if (named && ReadOne<T2>(@object => @object.Name == candidate.Name) is not null)
			return Conflict("Name already in use.");
		var issues = candidate.Id is not 0
						 ? ["ID must be null or 0 for POST."]
						 : !named
							 ? ["Name is required"]
							 : record.UpdateRecordForDatabase(candidate);
		if (issues.Length is not 0)
			return BadRequest(issues);
		CreateOne(record.Record);
		return Created($"{request.Path}/{record.Id}", null);
	}

	private protected static IResult DeleteOne(int id)
	{
		var record = RestForId(id);
		if (record is null)
			return NotFound();
		record.Unlink();
		Delete(record.Record);
		return NoContent();
	}

	private protected static T1? RestForId(int id,
										   bool details = true)
	{
		var record = ReadOne<T2>(record => record.Id == id);
		return record is null or Tournament { IsEvent: false }
				   ? null
				   : RestFrom(record, details);
	}

	private protected static T2 GetById(int id)
		=> ReadById<T2>(id);

	private protected static T1 RestFrom(T2 @object,
										 bool details = false)
	{
		var result = new T1 { Id = @object.Id, Name = @object.Name, Record = @object, Detailed = details };
		result.LoadFromDataRecord(@object);
		return result;
	}

	private protected static IEnumerable<T1> RestFrom(IEnumerable<T2> objects,
													  bool details = false)
		=> objects.Select(@object => RestFrom(@object, details));

	private protected virtual void LoadFromDataRecord(T2 record) { }

	private protected virtual string[] UpdateRecordForDatabase(T1 record)
		=> throw new NotImplementedException();

	public virtual bool Unlink()
		=> true;
}
