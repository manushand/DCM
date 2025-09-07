using System;
using System.Reflection;

namespace Data.Tests.Helpers;

internal sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
{
	public void Dispose()
		=> Field.SetValue(null, Original);
}
