using System.Reflection;
using JetBrains.Annotations;
using static System.Reflection.BindingFlags;

namespace API;

[PublicAPI]
internal interface IRest
{
	const BindingFlags BindingFlags = Static | NonPublic | FlattenHierarchy | InvokeMethod;

	static void CreateEndpoints() { }
}
