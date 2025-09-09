using System.Collections.Generic;
using System.Data;

namespace Data.Tests;

[PublicAPI]
public sealed class DataHelpersTests
{
	[Fact]
	public void ForSql_String_And_Int_Bool_Date_Enum_Handle_Nulls_And_Escapes()
	{
		Assert.Equal("'O''Neil'", "O'Neil".ForSql());
		Assert.Equal("Null", ((string?)null).ForSql());
		Assert.Equal("Null", ((int?)null).ForSql());
		Assert.Equal(1, true.ForSql());
		Assert.Equal(0, false.ForSql());
		var dt = new DateTime(2024, 1, 2);
		Assert.Equal("'1/2/2024'", dt.ForSql());
		Assert.Equal("Null", ((DateTime?)null).ForSql());
		Assert.Equal((int)Finished, Finished.ForSql());
	}

	[Fact]
	public void IDataRecord_Extensions_Read_And_Type_Switch_Work()
	{
		var values = new Dictionary<string, object?>
		{
			["B"] = true,
			["S"] = "Text",
			["I"] = 5,
			["INull"] = null,
			["Dbl"] = 1.25,
			["Dec"] = 2.50m,
			["Date"] = new DateTime(2024, 3, 4),
			["DateNull"] = null,
			["EnumInt"] = (int)Underway
		};
		using var reader = new FakeDbDataReader("Game", values);
		IDataRecord rec = reader;
		Assert.True(rec.Boolean("B"));
		Assert.Equal("Text", rec.String("S"));
		Assert.Equal(5, rec.Integer("I"));
		Assert.Null(rec.NullableInteger("INull"));
		// Double/Decimal should read as provided types
		Assert.Equal(1.25, rec.Double("Dbl"));
		Assert.Equal(2.50m, rec.Decimal("Dec"));
		Assert.Equal(Underway, rec.IntegerAs<Game.Statuses>("EnumInt"));
		Assert.Equal(new DateTime(2024, 3, 4), rec.NullableDate("Date"));
		Assert.Null(rec.NullableDate("DateNull"));
	}

	[Fact]
	public void IDataRecord_Decimal_Double_Switches_When_Other_Type()
	{
		// Provide Decimal for Double field and Double for Decimal field to exercise switch branches
		var values = new Dictionary<string, object?>
		{
			["Dbl"] = 2.5m,
			["Dec"] = 3.5
		};
		using var reader = new FakeDbDataReader("Game", values);
		IDataRecord rec = reader;
		Assert.Equal(2.5, rec.Double("Dbl"));   // Decimal -> Double
		Assert.Equal(3.5m, rec.Decimal("Dec")); // Double -> Decimal
	}

	[Fact]
	public void GroupSharedBy_Matches_According_To_Attribute_Groups()
	{
		// Using PowerGroups.Corners mapping: FR-IGA-TE
		Assert.True(Corners.GroupSharedBy(France, Russia));
		Assert.True(Corners.GroupSharedBy(Italy, Austria));
		Assert.False(Corners.GroupSharedBy(France, Italy));
	}

	[Fact]
	public void CheckDataType_Verifies_BaseTableName_And_Throws_On_Mismatch()
	{
		var okValues = new Dictionary<string, object?> { ["Id"] = 1 };
		using var okReader = new FakeDbDataReader("Game", okValues);
		// Does not throw
		okReader.CheckDataType<Game>();

		using var badReader = new FakeDbDataReader("Player", okValues);
		Assert.Throws<ArgumentException>(() => badReader.CheckDataType<Game>());
	}
}
