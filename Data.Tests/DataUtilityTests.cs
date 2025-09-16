using System.Collections.Generic;
using System.Linq;

namespace Data.Tests;

[PublicAPI]
public sealed class DataUtilityTests : TestBase
{
	private static readonly string[] Expected = ["Alpha", "Zulu"];
	private static readonly int[] ExpectedArray = [7, 10];

	[Fact]
	public void Sorted_Sorts_By_Name_Or_LastName()
	{
		var p1 = new Player { FirstName = "Bob", LastName = "Zed" };   // Name: Bob Zed, LastFirst: Zed Bob
		var p2 = new Player { FirstName = "Ann", LastName = "Young" }; // Name: Ann Young, LastFirst: Young Ann
		var list = new List<Player> { p1, p2 };

		// Default sorts by Name
		var byName = list.Sorted().ToArray();
		Assert.Same(p2, byName[0]);
		Assert.Same(p1, byName[1]);

		// Sort by last name
		var byLast = list.Sorted(true).ToArray();
		Assert.Same(p2, byLast[0]);
		Assert.Same(p1, byLast[1]);
	}

	[Fact]
	public void SelectSorted_Projects_And_Sorts_By_Comparable()
	{
		var t1 = new Team { Name = "Zulu" };
		var t2 = new Team { Name = "Alpha" };
		var names = new[] { t1, t2 }.SelectSorted(static t => t.Name).ToArray();
		Assert.Equal(Expected, names);
	}

	[Fact]
	public void Ids_Extracts_Identity_Ids()
	{
		var a = new Team { Id = 10, Name = "A" };
		var b = new Team { Id = 7, Name = "B" };
		var ids = new[] { a, b }.Ids().OrderBy(static x => x).ToArray();
		Assert.Equal(ExpectedArray, ids);
	}

	[Fact]
	public void HasPlayerId_And_ByPlayerId_Work_For_LinkRecords()
	{
		var tp1 = NewTeamPlayerViaLoad(1, 5);
		var tp2 = NewTeamPlayerViaLoad(1, 6);
		var list = new[] { tp1, tp2 };
		Assert.True(list.HasPlayerId(6));
		Assert.False(list.HasPlayerId(99));
		Assert.Equal(5, list.ByPlayerId(5).PlayerId);
	}

	[Fact]
	public void ConnectToAccessDatabase_Returns_False_When_No_Providers_Work()
		// We pass a bogus file path; providers will throw/open-fail and method should return false.
		=> Assert.False(Data.ConnectToAccessDatabase(@"X:\nonexistent\db.accdb"));

	[Fact]
	public void ReadByName_And_NameExists_Use_Cache_CaseInsensitive()
	{
		var p1 = new Player { Id = 1, FirstName = "Ann", LastName = "Lee" };
		using var cache = SeedCache(map => AddOne(map, typeof (Player), p1));
		var read = Data.ReadByName<Player>("ann lee");
		Assert.NotNull(read);
		Assert.Equal(1, read.Id);

		// NameExists returns false when comparing with itself
		Assert.False(Data.NameExists(p1));

		// But true for a different instance having same name
		var p2 = new Player { Id = 2, FirstName = "Ann", LastName = "Lee" };
		Assert.True(Data.NameExists(p2));
	}
}
