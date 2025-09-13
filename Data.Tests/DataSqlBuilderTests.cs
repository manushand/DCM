using System.Linq;

namespace Data.Tests;

[UsedImplicitly]
public sealed class DataSqlBuilderTests
{
	[Fact]
	public void UpdateStatement_And_DeleteStatement_And_WhereClause_Format_Correctly()
	{
		var dataType = typeof (Data);
		// Access private generic methods via reflection
		var methods = dataType.GetMethods(NonPublic | Static);
		var updateStatement2 = methods.First(static m => m is { Name: "UpdateStatement", IsGenericMethodDefinition: true }
													  && m.GetParameters().Length is 2)
									  .MakeGenericMethod(typeof (Player));
		var deleteStatement = methods.Where(static m => m.Name is "DeleteStatement")
									 .Single(static m => m.GetParameters().Length is 0)
									 .MakeGenericMethod(typeof (Player));
		var whereClauseForRecord = methods.First(static m => m.Name is "WhereClause"
														  && m.GetParameters().Length is 1
														  && m.GetParameters()[0].ParameterType == typeof (IRecord));
		var whereClauseForKey = methods.First(static m => m.Name is "WhereClause"
													   && m.GetParameters().Length is 1
													   && m.GetParameters()[0].ParameterType == typeof (string));

		var p = new Player
				{
					Id = 10,
					FirstName = "Bob",
					LastName = "Builder",
					EmailAddress = "b@x.com"
				};

		var updateSql = updateStatement2.Invoke(null, [p, null]) as string;
		Assert.StartsWith("UPDATE [Player] SET ", updateSql);
		Assert.Contains("[FirstName] = 'Bob'", updateSql);
		Assert.Contains("[LastName] = 'Builder'", updateSql);
		Assert.Contains(" WHERE [Id] = 10", updateSql);

		var delSql = deleteStatement.Invoke(null, []) as string;
		Assert.Equal("DELETE FROM [Player]", delSql);

		var wcRecord = whereClauseForRecord.Invoke(null, [p]) as string;
		Assert.Equal(" WHERE [Id] = 10", wcRecord);

		var wcKey = whereClauseForKey.Invoke(null, ["[Id] = 11"]) as string;
		Assert.Equal(" WHERE [Id] = 11", wcKey);
	}
}
