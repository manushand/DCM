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

		var updateSql = (string)updateStatement2.Invoke(null, [p, null])
												.OrThrow();
		Assert.StartsWith("UPDATE [Player] SET ", updateSql);
		Assert.Contains("[FirstName] = 'Bob'", updateSql);
		Assert.Contains("[LastName] = 'Builder'", updateSql);
		Assert.Contains(" WHERE [Id] = 10", updateSql);

		var delSql = (string)deleteStatement.Invoke(null, [])
											.OrThrow();
		Assert.Equal("DELETE FROM [Player]", delSql);

		var wcRecord = (string)whereClauseForRecord.Invoke(null, [p])
												   .OrThrow();
		Assert.Equal(" WHERE [Id] = 10", wcRecord);

		var wcKey = (string)whereClauseForKey.Invoke(null, ["[Id] = 11"])
											 .OrThrow();
		Assert.Equal(" WHERE [Id] = 11", wcKey);
	}
}
