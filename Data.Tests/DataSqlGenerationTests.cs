using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Data.Tests.Helpers;
using Microsoft.Data.SqlClient;
using Xunit;

namespace Data.Tests;

public sealed class DataSqlGenerationTests
{
	private static void SetPrivateStatic(Type type, string field, object? value)
	{
		var f = type.GetField(field, BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException(field);
		f.SetValue(null, value);
	}

	private static object GetPrivateStatic(Type type, string field)
		=> (type.GetField(field, BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException(field)).GetValue(null)!;


	[Fact]
	public void Sorted_And_SelectSorted_Work()
	{
		var a = new Player { Id = 1, FirstName = "Bob", LastName = "Zed" };
		var b = new Player { Id = 2, FirstName = "Ann", LastName = "Able" };
		var sortedByName = new[] { a, b }.Sorted().ToList();
		Assert.Equal(2, sortedByName[0].Id); // Ann Able comes before Bob Zed

		var projected = new[] { a, b }.SelectSorted(p => p.LastFirst).ToList();
		Assert.Equal("Able Ann", projected[0]);
	}

	private sealed class TestLink : LinkRecord
	{
		public int OtherId { get; private set; }
		protected override string LinkKey => $"[OtherId] = {OtherId}";
		public override IRecord Load(DbDataReader record) => this;
		public void Set(int playerId, int other) { PlayerId = playerId; OtherId = other; }
	}

	[Fact]
	public void Ids_HasPlayerId_ByPlayerId_Work()
	{
		var players = new List<Player>
		{
			new() { Id = 1 }, new() { Id = 2 }, new() { Id = 3 }
		};
		var ids = players.Ids().ToArray();
		Assert.Equal(new[] {1,2,3}, ids);

		var l1 = new TestLink(); l1.Set(10, 100);
		var l2 = new TestLink(); l2.Set(11, 100);
		var links = new[] { l1, l2 };
		Assert.True(links.HasPlayerId(11));
		Assert.Same(l1, links.ByPlayerId(10));
	}
}
