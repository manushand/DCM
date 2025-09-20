namespace Data.Tests;

[PublicAPI]
public sealed class GamePlayerCompareToTests
{
	[Fact]
	public void CompareTo_Throws_When_Other_Is_Null()
		=> Assert.ThrowsAny<Exception>(static () => new GamePlayer { Power = Austria }.CompareTo(null));

	[Fact]
	public void CompareTo_When_GameId_Zero_Compares_By_Power()
	{
		GamePlayer gp1 = new () { Power = Austria },
				   gp2 = new () { Power = England };
		// Austria (0) < England (1)
		Assert.True(gp1.CompareTo(gp2) < 0);
		Assert.True(gp2.CompareTo(gp1) > 0);
	}

	[Fact]
	public void CompareTo_With_GameIds_Compares_By_GameNumber_Then_Power_Then_PlayerName()
	{
		Round r = new () { Id = 1, Number = 1 };
		Game g1 = new () { Id = 10, Number = 1, Round = r },
			 g2 = new () { Id = 11, Number = 2, Round = r };
		Player a = new () { Id = 1, FirstName = "Ann", LastName = "A" },
			   b = new () { Id = 2, FirstName = "Bob", LastName = "B" };

		GamePlayer gpGame1PowerA = new () { Game = g1, Player = a, Power = Austria },
				   gpGame2PowerA = new () { Game = g2, Player = a, Power = Austria };
		// Different game numbers => compare by Game.Number first
		Assert.True(gpGame1PowerA.CompareTo(gpGame2PowerA) < 0);
		Assert.True(gpGame2PowerA.CompareTo(gpGame1PowerA) > 0);

		// Same game number: compare by Power
		GamePlayer gp2 = new () { Game = g1, Player = a, Power = England },
				   gp2Name = new () { Game = g1, Player = b, Power = Austria };
		Assert.True(gpGame1PowerA.CompareTo(gp2) < 0);

		// Same game and power: compare by Player.Name (Ann A < Bob B)
		Assert.True(gpGame1PowerA.CompareTo(gp2Name) < 0);
	}
}
