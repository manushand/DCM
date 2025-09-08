using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

using static GamePlayer.Powers;
using static GamePlayer.Results;

[PublicAPI]
public sealed class GamePlayerFieldValuesTests
{
	[Fact]
	public void FieldValues_Formats_All_Columns_With_ForSql_On_Nulls_And_Enums()
	{
		var p = new Player { Id = 5, FirstName = "Ann", LastName = "L" };
		var g = new Game { Id = 7, Round = new () { Id = 3, Number = 1 } };
		var gp = new GamePlayer
		{
			Player = p, // sets PlayerId via LinkRecord
			Game = g,
			Power = England,
			Result = Win,
			Years = null, // ensure Null formatting
			Centers = 12,
			Other = 1.5
		};

		var sql = gp.FieldValues;
		// Expected segments
		Assert.Contains("[PlayerId] = 5", sql);
		Assert.Contains("[Power] = 1", sql); // England enum => 1
		Assert.Contains("[Result] = 1", sql); // Win enum => 1
		Assert.Contains("[Years] = Null", sql); // null -> Null
		Assert.Contains("[Centers] = 12", sql);
		Assert.Contains("[Other] = 1.5", sql);
	}
}
