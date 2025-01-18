namespace API;

using Data;
using static Data.Data;

[PublicAPI]
internal abstract class Rest<T1, T2, T3> : IRest
	where T1 : Rest<T1, T2, T3>, new()
	where T2 : IdInfoRecord, new()
	where T3 : Rest<T1, T2, T3>.DetailClass
{
	internal int Identity => Record.Id;
	internal virtual string RecordedName => Record.Name;
	public T3? Details => Detailed ? Detail : null;

	internal abstract class DetailClass;

	private bool Detailed { get; set; }
	protected abstract T3 Detail { get; }

	protected internal T2 Record { get; init; } = new ();

	internal void AddDetail()
		=> Detailed = true;

	internal static void CreateEndpoints(WebApplication app)
	{
		var type = typeof (T1);
		var tag = type.Name;
		var name = type.Name.ToLower();
		app.MapGet($"{name}s", GetAll)
		   .WithName($"List{tag}s")
		   .WithDescription($"List all {name}s.")
		   .Produces(Status200OK, type.MakeArrayType())
		   .WithTags(tag);
		app.MapGet($"{name}/{{id:int}}", static (int id) => GetOne(id))
		   .WithName($"Get{tag}Details")
		   .WithDescription($"Get details for a {name}.")
		   .Produces(Status200OK, type)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapPost(name, PostOne)
		   .WithName($"Add{tag}")
		   .WithDescription($"Add a new {name}.")
		   .Produces(Status201Created)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .WithTags(tag);
		app.MapPut($"{name}/{{id:int}}", static (int id, T1 @object) => PutOne(id, @object))
		   .WithName($"Update{tag}")
		   .WithDescription($"Update details for a {name}.")
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .WithTags(tag);
		app.MapDelete($"{name}/{{id:int}}", static (int id) => DeleteOne(id))
		   .WithName($"Delete{tag}")
		   .WithDescription($"Delete a {name}.")
		   .Produces(Status204NoContent)
		   .Produces(Status404NotFound)
		   .WithTags(tag);

		CreateNonCrudEndpoints(app, tag);
	}

	private protected static void CreateNonCrudEndpoints(WebApplication app, string tag) { }

	private protected static IResult GetAll()
		=> Ok(RestFrom(GetMany(static _ => true)));

	private protected static IResult GetOne(int recordId)
	{
		var record = RestForId(recordId);
		return record is null
				   ? NotFound()
				   : Ok(record);
	}

	private protected static IResult PutOne(int recordId, T1 updated)
	{
		var record = RestForId(recordId);
		if (record is null)
			return NotFound();
		var id = updated.Identity;
		if ((ReadOne<T2>(system => system.Name == updated.RecordedName)?.Id ?? id) != id)
			return Conflict("Proposed name already in use.");
		var issues = id != recordId
						 ? ["IDs do not match."]
						 : updated.RecordedName.Length is 0
							 ? ["Name is required"]
							 : record.Update(updated);
		if (issues.Length is not 0)
			return BadRequest(issues);
		UpdateOne(record.Record);
		return NoContent();
	}

	private protected static IResult PostOne(HttpRequest request, T1 candidate)
	{
		if (ReadOne<T2>(@object => @object.Name == candidate.RecordedName) is not null)
			return Conflict("Name already in use.");
		var record = new T1();
		var issues = candidate.Identity is not 0
						 ? ["ID must be null or 0 for POST."]
						 : candidate.RecordedName.Length is 0
							 ? ["Name is required"]
							 : record.Update(candidate);
		if (issues.Length is not 0)
			return BadRequest(issues);
		CreateOne(record.Record);
		return Created($"{request.Path}/{record.Record.Id}", null);
	}

	private protected static IResult DeleteOne(int recordId)
	{
		var record = RestForId(recordId);
		if (record is null)
			return NotFound();
		record.Unlink();
		Delete(record.Record);
		return NoContent();
	}

	private protected static T1? RestForId(int recordId, bool details = true)
	{
		var record = ReadByIdOrNull<T2>(recordId);
		return record is null
				   ? null
				   : RestFrom(record, details);
	}

	private protected static T2 GetById(int recordId)
		=> ReadById<T2>(recordId);

	private protected static T1 RestFrom(T2 @object, bool details)
		=> new () { Record = @object, Detailed = details };

	private protected static IEnumerable<T1> RestFrom(IEnumerable<T2> objects, bool details = false)
		=> objects.Select(@object => RestFrom(@object, details));

	private protected static IEnumerable<T2> GetMany(Func<T2, bool> predicate)
		=> ReadMany(predicate);

	private protected virtual string[] Update(T1 record)
		=> throw new NotImplementedException();

	public virtual bool Unlink()
		=> true;
}
