global using System;
global using JetBrains.Annotations;
global using Xunit;
global using static System.Activator;
global using static System.Reflection.BindingFlags;
//
global using DCM;
global using Data.Tests.Helpers;
global using static Data.Game.Statuses;
global using static Data.GamePlayer.Powers;
global using static Data.GamePlayer.Results;
global using static Data.Tournament.PowerGroups;
//
using System.Reflection;

namespace Data.Tests;

public abstract class TestBase
{
	protected sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	protected static void SetField(object target, string field, object? value)
		=> target.GetType()
				 .GetField(field, Instance | NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	protected static void SetProperty(object target, string prop, object? value)
		=> target.GetType()
				 .GetProperty(prop, Instance | Public | NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	protected static string GetPrimaryKey(object record)
		=> (record.GetType()
				  .GetProperty("PrimaryKey", Instance | Public)
				  .OrThrow($"PrimaryKey property not found on {record.GetType().Name}")
				  .GetValue(record) as string)
			.OrThrow();
}
