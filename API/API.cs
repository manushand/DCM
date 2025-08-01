﻿global using JetBrains.Annotations;
global using static System.String;
global using static Microsoft.AspNetCore.Http.StatusCodes;
global using static Microsoft.AspNetCore.Http.Results;
global using static Data.Data;
global using static Data.Game;
global using static Data.Game.Statuses;
global using static Data.GamePlayer;
global using static Data.Tournament;
global using GameResults = Data.GamePlayer.Results;
//
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Text.Json.JsonSerializer;
using static System.Text.Json.Serialization.JsonIgnoreCondition;
using static System.Threading.Monitor;

namespace API;

using Data;
using static DCM.DCM;

internal static class API
{
	private const string Title = "DCM API";
	private const int Version = 1;
	private const string SettingsFile = "appSettings.json";
	private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder().AddJsonFile(SettingsFile)
																						 .Build();
	private static readonly Dictionary<string, string> Users = Configuration.GetSection(nameof (Users))
																			.Get<Dictionary<string, string>>()?
																			.ToDictionary(static pair => pair.Key,
																						  static pair => Configuration.GetConnectionString(pair.Value)
																										 ?? throw new ($"Bad Connection for User {pair.Key}"))
															   ?? throw new ($"No API Users in {SettingsFile}");

	internal static void Startup()
	{
		var version = $"v{Version}";
		var builder = WebApplication.CreateBuilder();
		builder.Services
			   .Configure<JsonOptions>(static options =>
									   {
										   var serializerOptions = options.SerializerOptions;
										   serializerOptions.DefaultIgnoreCondition = WhenWritingNull;
										   serializerOptions.Converters.Add(new JsonStringEnumConverter());
									   })
			   .AddCors(static options => options.AddDefaultPolicy(static policy => policy.AllowAnyOrigin()
																						  .AllowAnyMethod()
																						  .AllowAnyHeader()))
			   .AddOpenApi()
			   .AddEndpointsApiExplorer()
			   .AddSwaggerGen(config =>
							  {
								  config.SwaggerDoc(version, new () { Title = Title, Version = version });
								  config.SchemaFilter<EnumSchemaFilter>();
								  config.CustomSchemaIds(static type => type.FullName?
																			.Replace($"{nameof (API)}.", string.Empty)
																			.Replace($"{nameof (Data)}.{nameof (GamePlayer)}", nameof (Player))
																			.Replace($"{nameof (Data)}.", string.Empty)
																			.Replace(nameof (ScoringSystem), nameof (System))
																			.Replace($"{nameof (Game.Detail)}+", string.Empty)
																			.Replace("+", "."));
							  });

		var app = builder.Build();
		app.Use(Handle);
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.UseDeveloperExceptionPage()
			   .UseSwagger()
			   .UseSwaggerUI(config => config.SwaggerEndpoint($"./{version}/swagger.json", $"{Title} {version}"));
		}
		else
			app.UseHttpsRedirection()
			   .UseHsts()
			   .UseExceptionHandler(ExceptionHandler);

		Group.CreateEndpoints(app);
		Player.CreateEndpoints(app);
		System.CreateEndpoints(app);
		Event.CreateEndpoints(app);

		app.Run();

		static void ExceptionHandler(IApplicationBuilder app)
			=> app.Run(static async context =>
					   {
						   var response = context.Response;
						   response.StatusCode = HttpStatusCode.InternalServerError.AsInteger();
						   response.ContentType = "application/json";
						   var handler = context.Features.Get<IExceptionHandlerPathFeature>();
						   Error error = new (handler?.Error ?? new ("Unknown error"));
						   await response.WriteAsync(Serialize(error));
					   });
	}

	internal static string? NullIfEmpty(this string? s)
		=> IsNullOrWhiteSpace(s)
			   ? null
			   : s;

	internal static ICollection<T>? NullIfEmpty<T>(this ICollection<T> i)
		=> i.Count is 0
			   ? null
			   : i;

	internal static void Create<T>(this T record)
		where T : class, IRecord
		=> CreateOne(record);

	internal static void Delete<T>(this T record)
		where T : class, IRecord
		=> Data.Delete(record);

	private static string _currentConnection = string.Empty;
	private static int _activeUsers;
	private static readonly object Locker = new ();

	private static async Task Handle(HttpContext context, RequestDelegate next)
	{
		var user = context.Request.Headers.Authorization.ToString();
		if (!Users.TryGetValue(user, out var connectionString))
			throw new UnauthorizedAccessException($"Bad {nameof (context.Request.Headers.Authorization)}");
		lock (Locker)
		{
			if (connectionString != _currentConnection)
				while (_activeUsers is not 0)
					Wait(Locker);
			if (connectionString != _currentConnection)
				ConnectTo(_currentConnection = connectionString);
			++_activeUsers;
		}
		await next(context);
		lock (Locker)
		{
			--_activeUsers;
			Pulse(Locker);
		}
	}

	[PublicAPI]
	private class EnumSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema model,
						  SchemaFilterContext context)
		{
			if (context.Type.IsEnum)
				model.Enum =
				[
					..Enum.GetNames(context.Type)
						  .Select(enumName => new OpenApiString(context.Type
																	   .GetMember(enumName)
																	   .FirstOrDefault(m => m.DeclaringType == context.Type)?
																	   .GetCustomAttributes(typeof (EnumMemberAttribute), false)
																	   .OfType<EnumMemberAttribute>()
																	   .FirstOrDefault()?
																	   .Value?
																	   .Trim()
																	   .NullIfEmpty() ?? enumName))
				];
		}
	}
}
