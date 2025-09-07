using Microsoft.AspNetCore.Mvc;

namespace API;

using DCM;
using Data;

[PublicAPI]
internal abstract class Rest<T1, T2, T3> : IRest
	where T1 : Rest<T1, T2, T3>, new()
	where T2 : IdInfoRecord, new()
	where T3 : Rest<T1, T2, T3>.DetailClass
{
	internal abstract class DetailClass;

	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public T3? Details
	{
		get => Detailed ? Info : null;
		set => Info = value.OrThrow();
	}

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
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapGet($"{TypeName}/{{id:int}}", GetOne)
		   .WithName($"Get{SwaggerTag}Details")
		   .WithDescription($"Get details for {articled}.")
		   .Produces(Status200OK, Type)
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapPost(TypeName, PostOne)
		   .WithName($"Add{SwaggerTag}")
		   .WithDescription($"Add a new {TypeName}.")
		   .Produces(Status201Created)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapPut($"{TypeName}/{{id:int}}", PutOne)
		   .WithName($"Update{SwaggerTag}")
		   .WithDescription($"Update details for {articled}.")
		   .Produces(Status204NoContent)
		   .Produces<string[]>(Status400BadRequest)
		   .Produces<string>(Status409Conflict)
		   .Produces<Error>(Status500InternalServerError)
		   .WithTags(SwaggerTag);
		app.MapDelete($"{TypeName}/{{id:int}}", DeleteOne)
		   .WithName($"Delete{SwaggerTag}")
		   .WithDescription($"Delete {articled}.")
		   .Produces(Status204NoContent)
		   .Produces(Status404NotFound)
		   .Produces<Error>(Status500InternalServerError)
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

	private string[] CheckName()
	{
		Name = Name.Trim();
		switch (this)
		{
		case Player player:
			if (Name.Length > 0)
				return Player.NameIsDetermined;
			player.FirstName = player.FirstName.Trim();
			player.LastName = player.LastName.Trim();
			if (player.FirstName.Length is 0 || player.LastName.Length is 0)
				return Player.NamesAreRequired;
			Name = $"{player.FirstName} {player.LastName}".Trim();
			goto default;
		case not Game when Name.Length is 0:
			return ["Name is required."];
		default:
			return [];
		}
	}

	private protected bool WouldCollide()
		=> (ReadByName<T2>(Name)?.Id ?? Id) != Id;

	private protected static IResult PutOne(int id,
											[FromBody] T1 updated,
											[FromQuery] bool force = false)
	{
		if (force && updated is not Player and not Game)
			return BadRequest("Force update is only allowed for players and games.");
		var rest = RestForId(id);
		if (rest is null)
			return NotFound();
		var issues = updated.CheckName();
		if (issues.Length is not 0)
			return BadRequest(issues);
		if (updated.Name.Length > 0 && !force && updated.WouldCollide())
			return Conflict("Proposed new name already in use.");
		issues = updated.Id != id
						 ? ["IDs do not match."]
						 : rest.UpdateRecordForDatabase(updated);
		if (issues.Length is not 0)
			return BadRequest(issues);
		UpdateOne(rest.Record);
		return NoContent();
	}

	private protected static IResult PostOne(HttpRequest request,
											 [FromBody] T1 candidate,
											 [FromQuery] bool force = false)
	{
		if (force && candidate is not Player and not Game)
			return BadRequest("Force update is only allowed for players and games.");
		var issues = candidate.CheckName();
		if (issues.Length is not 0)
			return BadRequest(issues);
		if (candidate.Name.Length > 0 && !force && candidate.WouldCollide())
			return Conflict("Name already in use.");
		T1 record = new ();
		issues = candidate.Id is not 0
						 ? ["ID must be null or 0 for POST."]
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
		T1 result = new () { Id = @object.Id, Name = @object.Name, Record = @object, Detailed = details };
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
