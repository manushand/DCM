using JetBrains.Annotations;

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
		app.MapGet($"{name}s",
				   () => type.InvokeMember(nameof (GetAll), IRest.BindingFlags, null, null, null))
		   .WithName($"List{tag}s")
		   .WithDescription($"List all {name}s.")
		   .Produces(Status200OK, type.MakeArrayType())
		   .WithTags(tag);
		app.MapGet($"{name}/{{id:int}}",
				   (int id) => type.InvokeMember(nameof (GetOne), IRest.BindingFlags, null, null, [id]))
		   .WithName($"Get{tag}Details")
		   .WithDescription($"Get details for a {name}.")
		   .Produces(Status200OK, type)
		   .Produces(Status404NotFound)
		   .WithTags(tag);
		app.MapPost(name,
					(HttpRequest request, T1 @object) => type.InvokeMember(nameof (PostOne), IRest.BindingFlags, null, null, [request, @object]))
		   .WithName($"Add{tag}")
		   .WithDescription($"Add a new {name}.")
		   .Produces(Status201Created)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .WithTags(tag);
		app.MapPut($"{name}/{{id:int}}",
				   (int id, T1 @object) => type.InvokeMember(nameof (PutOne), IRest.BindingFlags, null, null, [id, @object]))
		   .WithName($"Update{tag}")
		   .WithDescription($"Update details for a {name}.")
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .WithTags(tag);
		app.MapDelete($"{name}/{{id:int}}",
					  (int id) => type.InvokeMember(nameof (DeleteOne), IRest.BindingFlags, null, null, [id]))
		   .WithName($"Delete{tag}")
		   .WithDescription($"Delete a {name}.")
		   .Produces(Status204NoContent)
		   .Produces(Status404NotFound)
		   .WithTags(tag);

		type.InvokeMember(nameof (CreateNonCrudEndpoints), IRest.BindingFlags, null, null, [app, tag]);
	}

	protected internal static void CreateNonCrudEndpoints(WebApplication app, string tag) { }

	protected internal static IResult GetAll()
		=> Ok(RestFrom(GetMany(static _ => true)));

	protected internal static IResult GetOne(int recordId)
	{
		var record = RestForId(recordId);
		return record is null
				   ? NotFound()
				   : Ok(record);
	}

	protected internal static IResult PutOne(int recordId, T1 updated)
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

	protected internal static IResult PostOne(HttpRequest request, T1 candidate)
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

	protected internal static IResult DeleteOne(int recordId)
	{
		var record = RestForId(recordId);
		if (record is null)
			return NotFound();
		record.Unlink();
		Delete(record.Record);
		return NoContent();
	}

	protected internal static T1? RestForId(int recordId, bool details = true)
	{
		var record = ReadByIdOrNull<T2>(recordId);
		return record is null
				   ? null
				   : new T1 { Record = record, Detailed = details };
	}

	protected internal static IEnumerable<T1> RestFrom(IEnumerable<T2> objects, bool details = false)
		=> objects.Select(@object => new T1 { Record = @object, Detailed = details });

	protected internal static IEnumerable<T2> GetMany(Func<T2, bool> predicate)
		=> ReadMany(predicate);

	protected internal virtual string[] Update(T1 record)
		=> throw new NotImplementedException();

	public virtual bool Unlink()
		=> true;
}
