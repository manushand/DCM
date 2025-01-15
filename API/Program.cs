using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using static System.Reflection.Assembly;
using static System.Reflection.BindingFlags;
using static System.Text.Json.Serialization.JsonIgnoreCondition;
//
using API;
using static DCM.DCM;
using static Data.Data;
using static Data.Data.DatabaseTypes;

//	Connect to the database
var configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json")
											  .Build();
var dbType = configuration.GetValue<string>("dbType")?.As<DatabaseTypes>() ?? SqlServer;
var connectionString = configuration.GetConnectionString($"{dbType}").OrThrow();
switch (dbType)
{
case SqlServer:
	ConnectToSqlServerDatabase(connectionString);
	break;
case Access:
	ConnectToAccessDatabase(connectionString);
	break;
case None:
	throw new ArgumentException("No database in settings");
default:
	throw new ArgumentException($"Invalid database type: {dbType}");
}

//	Configure the Web API
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.Configure<JsonOptions>(static options =>
										{
											var serializerOptions = options.SerializerOptions;
											serializerOptions.DefaultIgnoreCondition = WhenWritingNull;
											serializerOptions.PropertyNamingPolicy = null;
											serializerOptions.Converters.Add(new JsonStringEnumConverter());
										});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(static config => config.SwaggerDoc("v1", new () { Title = "DCM API", Version = "v1" }));

var app = builder.Build();
if (app.Environment.IsDevelopment())
	app.MapOpenApi();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(static config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "DCM API v1"));

const string getAll = nameof (Group.GetAll),
			 getOne = nameof (Group.GetOne),
			 putOne = nameof (Group.PutOne),
			 postOne = nameof (Group.PostOne);
const BindingFlags bindingFlags = Static | Public | FlattenHierarchy | InvokeMethod;
var restTypes = GetExecutingAssembly().GetTypes()
									  .Where(static type => typeof (IRest).IsAssignableFrom(type)
														 && type is
														 {
															 IsClass: true,
															 IsAbstract: false,
															 Name: nameof (Group)
																or nameof (Player)
																or nameof (ScoringSystem)
																or nameof (Tournament)
														 });
foreach (var type in restTypes)
{
	var name = type.Name;
	app.MapPost(name,
				(object @object) => type.InvokeMember(postOne, bindingFlags, null, null, [@object]))
	   .WithName($"{postOne}{name}")
	   .WithDescription($"Add a new {name}.");
	app.MapGet(name,
			   () => type.InvokeMember(getAll, bindingFlags, null, null, null))
		   .WithName($"{getAll}{name}s")
		   .WithDescription($"List all {name}s.");
	app.MapGet($"{name}/{{id:int}}",
			   (int id) => type.InvokeMember(getOne, bindingFlags, null, null, [id]))
	   .WithName($"{getOne}{name}")
	   .WithDescription($"Get a specific {name}.");
	app.MapPut($"{name}/{{id:int}}",
			   (int id, object @object) => type.InvokeMember(putOne, bindingFlags, null, null, [id, @object]))
	   .WithName($"{putOne}{name}")
	   .WithDescription($"Modify a specific {name}.");
	app.MapDelete($"{name}/{{id:int}}",
				  (int id) => type.InvokeMember(nameof (Group.DeleteOne), bindingFlags, null, null, [id]))
	   .WithName($"DeleteOne{name}")
	   .WithDescription($"Remove a specific {name}.");
}

#region Group endpoints

app.MapGet("group/{id:int}/players/eligible", Group.GetNonMembers)
   .WithName("GetGroupPlayerCandidates")
   .WithDescription("List all players who are not members of the group.");
app.MapGet("group/{id:int}/players", Group.GetMembers)
   .WithName("GetGroupPlayers")
   .WithDescription("List all players who are members of the group.");
app.MapPost("group/{id:int}/players/{playerId:int}", Group.AddMember)
   .WithName("AddGroupPlayer")
   .WithDescription("Add a player to the group.");
app.MapDelete("group/{id:int}/players/{playerId:int}", Group.DropMember)
   .WithName("DropGroupPlayer")
   .WithDescription("Remove a player from the group.");

#endregion

#region Player endpoints

//	TODO

#endregion

#region ScoringSystem endpoints

//	TODO

#endregion

#region Tournament endpoints

app.MapGet("tournament/{id:int}/players/eligible", Tournament.GetUnregistered)
   .WithName("GetTournamentPlayerCandidates")
   .WithDescription("List all players who are not registered for the tournament.");
app.MapGet("tournament/{id:int}/players", Tournament.GetRegistered)
   .WithName("GetTournamentPlayers")
   .WithDescription("List all players registered for the tournament, with the rounds for which each player is registered.");
app.MapPost("tournament/{id:int}/players/{playerId:int}" /* ?round=1&round=2... */, Tournament.RegisterPlayer)
   .WithName("AddTournamentPlayer")
   .WithDescription("Register a player for the tournament while setting, updating, or clearing the player's round registration.");
app.MapDelete("tournament/{id:int}/players/{playerId:int}", Tournament.UnregisterPlayer)
   .WithName("DropTournamentPlayer")
   .WithDescription("Unregister a player from the tournament.");

//	TODO

#endregion

app.Run();
