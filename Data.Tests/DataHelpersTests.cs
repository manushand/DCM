using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

using Helpers;

[PublicAPI]
public sealed class DataHelpersTests
{
	[Fact]
	public void ForSql_String_And_Int_Bool_Date_Enum_Handle_Nulls_And_Escapes()
	{
  Assert.Equal("'O''Neil'", global::Data.Data.ForSql("O'Neil"));
  Assert.Equal("Null", global::Data.Data.ForSql((string?)null));
  Assert.Equal("Null", global::Data.Data.ForSql((int?)null));
  Assert.Equal(1, global::Data.Data.ForSql(true));
  Assert.Equal(0, global::Data.Data.ForSql(false));
		var dt = new DateTime(2024, 1, 2);
  Assert.Equal("'1/2/2024'", global::Data.Data.ForSql(dt));
  Assert.Equal("Null", global::Data.Data.ForSql((DateTime?)null));
  Assert.Equal((int)Game.Statuses.Finished, global::Data.Data.ForSql(Game.Statuses.Finished));
	}

	[Fact]
	public void IDataRecord_Extensions_Read_And_Type_Switch_Work()
	{
		var values = new Dictionary<string, object?>
		{
			{ "B", true },
			{ "S", "Text" },
			{ "I", 5 },
			{ "INull", null },
			{ "Dbl", 1.25 },
			{ "Dec", 2.50m },
			{ "Date", new DateTime(2024, 3, 4) },
			{ "DateNull", null },
			{ "EnumInt", (int)Game.Statuses.Underway }
		};
		using var reader = new FakeDbDataReader("Game", values);
		IDataRecord rec = reader;
  Assert.True(global::Data.Data.Boolean(rec, "B"));
  Assert.Equal("Text", global::Data.Data.String(rec, "S"));
  Assert.Equal(5, global::Data.Data.Integer(rec, "I"));
  Assert.Null(global::Data.Data.NullableInteger(rec, "INull"));
		// Double/Decimal should read as provided types
  Assert.Equal(1.25, global::Data.Data.Double(rec, "Dbl"));
  Assert.Equal(2.50m, global::Data.Data.Decimal(rec, "Dec"));
  Assert.Equal(Game.Statuses.Underway, global::Data.Data.IntegerAs<Game.Statuses>(rec, "EnumInt"));
  Assert.Equal(new DateTime(2024,3,4), global::Data.Data.NullableDate(rec, "Date"));
  Assert.Null(global::Data.Data.NullableDate(rec, "DateNull"));
	}

	[Fact]
	public void IDataRecord_Decimal_Double_Switches_When_Other_Type()
	{
		// Provide Decimal for Double field and Double for Decimal field to exercise switch branches
		var values = new Dictionary<string, object?>
		{
			{ "Dbl", 2.5m },
			{ "Dec", 3.5 }
		};
		using var reader = new FakeDbDataReader("Game", values);
		IDataRecord rec = reader;
  Assert.Equal(2.5, global::Data.Data.Double(rec, "Dbl")); // Decimal -> Double
  Assert.Equal(3.5m, global::Data.Data.Decimal(rec, "Dec")); // Double -> Decimal
	}

	[Fact]
	public void GroupSharedBy_Matches_According_To_Attribute_Groups()
	{
		// Using PowerGroups.Corners mapping: FR-IGA-TE
  Assert.True(global::Data.Data.GroupSharedBy(Tournament.PowerGroups.Corners, GamePlayer.Powers.France, GamePlayer.Powers.Russia));
  Assert.True(global::Data.Data.GroupSharedBy(Tournament.PowerGroups.Corners, GamePlayer.Powers.Italy, GamePlayer.Powers.Austria));
  Assert.False(global::Data.Data.GroupSharedBy(Tournament.PowerGroups.Corners, GamePlayer.Powers.France, GamePlayer.Powers.Italy));
	}

	[Fact]
	public void CheckDataType_Verifies_BaseTableName_And_Throws_On_Mismatch()
	{
		var okValues = new Dictionary<string, object?> { { "Id", 1 } };
		using var okReader = new FakeDbDataReader("Game", okValues);
		// Does not throw
  global::Data.Data.CheckDataType<Game>(okReader);

		using var badReader = new FakeDbDataReader("Player", okValues);
  Assert.Throws<ArgumentException>(() => global::Data.Data.CheckDataType<Game>(badReader));
	}
}
