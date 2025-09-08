using System;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace Data.Tests;

using DCM;

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
}
