using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Data.Tests.Helpers;
using Xunit;

namespace Data.Tests;

public sealed class DataSqlBuilderTests
{
	[Fact]
	public void UpdateStatement_And_DeleteStatement_And_WhereClause_Format_Correctly()
	{
		var dataType = typeof(Data);
		// Access private generic methods via reflection
  var updateStatement2 = dataType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
			.First(m => m.Name == "UpdateStatement" && m.IsGenericMethodDefinition && m.GetParameters().Length == 2)
			.MakeGenericMethod(typeof(Player));
		var deleteStatement = dataType.GetMethod("DeleteStatement", BindingFlags.NonPublic | BindingFlags.Static)!
			.MakeGenericMethod(typeof(Player));
		var whereClauseForRecord = dataType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
			.First(m => m.Name == "WhereClause" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(IRecord));
		var whereClauseForKey = dataType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
			.First(m => m.Name == "WhereClause" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(string));

		var p = new Player { Id = 10, FirstName = "Bob", LastName = "Builder", EmailAddress = "b@x.com" };
		var currentPk = p.PrimaryKey;

		var updateSql = (string)updateStatement2.Invoke(null, new object?[] { currentPk, p })!;
		Assert.StartsWith("UPDATE [Player] SET ", updateSql);
		Assert.Contains("[FirstName] = 'Bob'", updateSql);
		Assert.Contains("[LastName] = 'Builder'", updateSql);
		Assert.Contains(" WHERE [Id] = 10", updateSql);

		var delSql = (string)deleteStatement.Invoke(null, null)!;
		Assert.Equal("DELETE FROM [Player]", delSql);

		var wcRecord = (string)whereClauseForRecord.Invoke(null, new object?[] { p })!;
		Assert.Equal(" WHERE [Id] = 10", wcRecord);

		var wcKey = (string)whereClauseForKey.Invoke(null, new object?[] { "[Id] = 11" })!;
		Assert.Equal(" WHERE [Id] = 11", wcKey);
	}
}
