﻿global using static Microsoft.AspNetCore.Http.StatusCodes;
global using static Microsoft.AspNetCore.Http.Results;
//
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Reflection.Assembly;
using static System.Text.Json.Serialization.JsonIgnoreCondition;

namespace API;

using static DCM.DCM;
using static Data.Data;
using static Data.Data.DatabaseTypes;

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
			throw new ArgumentException("No database in settings");
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
			   .AddOpenApi()
			   .AddEndpointsApiExplorer()
			   .AddSwaggerGen(config =>
							  {
								  config.SwaggerDoc(version, new () { Title = Title, Version = version });
								  config.SchemaFilter<EnumSchemaFilter>();
							  });

		var app = builder.Build();
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.UseSwagger()
			   .UseSwaggerUI(config => config.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{Title} {version}"));
		}
		else
			app.UseHttpsRedirection()
			   .UseHsts();

		var restTypes = GetExecutingAssembly().GetTypes()
											  .Where(static type => typeof (IRest).IsAssignableFrom(type)
																 && type is
																	{
																		IsClass: true,
																		IsAbstract: false,
																		Name: nameof (Group)
																		  or  nameof (Player)
																		  or  nameof (System)
																		  or  nameof (Tournament)
																	});
		foreach (var type in restTypes)
			type.InvokeMember(nameof (IRest.CreateEndpoints), IRest.BindingFlags, null, null, [app]);

		app.Run();
	}

	internal static string? NullIfEmpty(this string? s)
		=> string.IsNullOrWhiteSpace(s) ? null : s;

	internal static ICollection<T>? NullIfEmpty<T>(this ICollection<T> i)
		=> i.Count is 0 ? null : i;

	[PublicAPI]
	private class EnumSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema model, SchemaFilterContext context)
		{
			if (!context.Type.IsEnum)
				return;
			model.Enum =
			[
				..Enum.GetNames(context.Type)
					  .Select(enumName =>
							  {
								  var enumMemberAttribute = context.Type
																   .GetMember(enumName)
																   .FirstOrDefault(m => m.DeclaringType == context.Type)
																   ?.GetCustomAttributes(typeof (EnumMemberAttribute), false)
																   .OfType<EnumMemberAttribute>()
																   .FirstOrDefault();
								  return new OpenApiString(string.IsNullOrWhiteSpace(enumMemberAttribute?.Value)
															   ? enumName
															   : enumMemberAttribute.Value);
							  })
			];
		}
	}
}
