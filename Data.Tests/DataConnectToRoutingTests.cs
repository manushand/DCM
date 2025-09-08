using System;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class DataConnectToRoutingTests
{
	[Fact]
	public void ConnectTo_With_SqlLike_ConnectionString_Throws()
	{
		// This exercises the ConnectTo routing path that chooses SQL Server when the string contains '='
		// It will attempt to open a SqlConnection and should throw quickly on a dev machine without a server.
		const string sqlLike = "Server=.;Database=nonexistent;Trusted_Connection=True;";
		Assert.ThrowsAny<Exception>(static () => Data.ConnectTo(sqlLike));
	}
}
