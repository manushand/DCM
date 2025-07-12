﻿global using JetBrains.Annotations;
global using static Microsoft.AspNetCore.Http.StatusCodes;
global using static Microsoft.AspNetCore.Http.Results;
global using static Data.Data;
global using static Data.Game;
global using static Data.Game.Statuses;
global using static Data.GamePlayer;
global using static Data.Tournament;
global using GameResults = Data.GamePlayer.Results;
global using static System.String;
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

namespace API;

using Data;
using static DCM.DCM;
using static DatabaseTypes;

internal static class API
{
	private const string Title = "DCM API";
	private const int Version = 1;

	internal static void Connect()
	{
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
			throw new ArgumentException("No database type in settings");
		default:
			throw new ArgumentException($"Invalid database type: {dbType}");
		}
	}

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
			   .AddCors(static options => options.AddDefaultPolicy(static policy =>
																   {
																	   policy.AllowAnyOrigin()
																			 .AllowAnyMethod()
																			 .AllowAnyHeader();
																   }))
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
