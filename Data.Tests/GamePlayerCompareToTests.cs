using System;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class GamePlayerCompareToTests
{
	[Fact]
	public void CompareTo_Throws_When_Other_Is_Null()
	{
		var gp = new GamePlayer { Power = GamePlayer.Powers.Austria };
		Assert.ThrowsAny<Exception>(() => gp.CompareTo(null));
	}

	[Fact]
	public void CompareTo_When_GameId_Zero_Compares_By_Power()
	{
		var gp1 = new GamePlayer { Power = GamePlayer.Powers.Austria };
		var gp2 = new GamePlayer { Power = GamePlayer.Powers.England };
		// Austria (0) < England (1)
		Assert.True(gp1.CompareTo(gp2) < 0);
		Assert.True(gp2.CompareTo(gp1) > 0);
	}

	[Fact]
	public void CompareTo_With_GameIds_Compares_By_GameNumber_Then_Power_Then_PlayerName()
	{
		var r = new Round { Id = 1, Number = 1 };
		var g1 = new Game { Id = 10, Number = 1, Round = r };
		var g2 = new Game { Id = 11, Number = 2, Round = r };
		var a = new Player { Id = 1, FirstName = "Ann", LastName = "A" };
		var b = new Player { Id = 2, FirstName = "Bob", LastName = "B" };

		var gpGame1PowerA = new GamePlayer { Game = g1, Player = a, Power = GamePlayer.Powers.Austria };
		var gpGame2PowerA = new GamePlayer { Game = g2, Player = a, Power = GamePlayer.Powers.Austria };
		// Different game numbers => compare by Game.Number first
		Assert.True(gpGame1PowerA.CompareTo(gpGame2PowerA) < 0);
		Assert.True(gpGame2PowerA.CompareTo(gpGame1PowerA) > 0);

		// Same game number: compare by Power
		var gp1 = new GamePlayer { Game = g1, Player = a, Power = GamePlayer.Powers.Austria };
		var gp2 = new GamePlayer { Game = g1, Player = a, Power = GamePlayer.Powers.England };
		Assert.True(gp1.CompareTo(gp2) < 0);

		// Same game and power: compare by Player.Name (Ann A < Bob B)
		var gp1Name = new GamePlayer { Game = g1, Player = a, Power = GamePlayer.Powers.Austria };
		var gp2Name = new GamePlayer { Game = g1, Player = b, Power = GamePlayer.Powers.Austria };
		Assert.True(gp1Name.CompareTo(gp2Name) < 0);
	}
}
