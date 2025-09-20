using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Data.Tests;

[UsedImplicitly]
public sealed class DataSqlGenerationTests
{
	private sealed class TestLink : LinkRecord
	{
		private int OtherId { get; }
		private protected override string LinkKey => $"[OtherId] = {OtherId}";

		public override void Load(DbDataReader record) { }

		public TestLink(int playerId, int other)
			=> (PlayerId, OtherId) = (playerId, other);
	}

	private static readonly int[] Expected = [1, 2, 3];

	[Fact]
	public void Sorted_And_SelectSorted_Work()
	{
		Player a = new () { Id = 1, FirstName = "Bob", LastName = "Zed" },
			   b = new () { Id = 2, FirstName = "Ann", LastName = "Able" };
		var sortedByName = new[] { a, b }.Sorted().ToList();
		Assert.Equal(2, sortedByName[0].Id); // Ann Able comes before Bob Zed

		var projected = new[] { a, b }.SelectSorted(static p => p.LastFirst).ToList();
		Assert.Equal("Able Ann", projected[0]);
	}

	[Fact]
	public void Ids_HasPlayerId_ByPlayerId_Work()
	{
		var players = new List<Player>
		{
			new () { Id = 1 },
			new () { Id = 2 },
			new () { Id = 3 }
		};
		var ids = players.Ids().ToArray();
		Assert.Equal(Expected, ids);

		TestLink l1 = new (10, 100),
				 l2 = new (11, 100);
		var links = new[] { l1, l2 };
		Assert.True(links.HasPlayerId(11));
		Assert.Same(l1, links.ByPlayerId(10));
	}
}
