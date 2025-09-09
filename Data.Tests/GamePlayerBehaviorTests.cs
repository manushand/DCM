namespace Data.Tests;

[UsedImplicitly]
public sealed class GamePlayerBehaviorTests
{
	private static void Set<T>(T obj, string member, object value) where T : class
	{
		var type = obj.GetType();
		var prop = type.GetProperty(member, Instance | Public | NonPublic);
		if (prop is not null && prop.CanWrite)
			prop.SetValue(obj, value);
		else
			type.GetField(member, Instance | Public | NonPublic)
				.OrThrow($"Member {member} not found on {type}")
				.SetValue(obj, value);
	}

	[Fact]
	public void CompareTo_Orders_By_Power_When_No_Game()
	{
		var a = new GamePlayer { Power = Austria };
		var e = new GamePlayer { Power = England };
		// (GameId | other.GameId) == 0 => compare by Power enum value
		Assert.True(a.CompareTo(e) < 0);
		Assert.True(e.CompareTo(a) > 0);
	}

	[Fact]
	public void CompareTo_Orders_By_GameNumber_Then_Power_Then_PlayerName()
	{
		// Create two players in different games with same power to force tie-breakers
		var gp1 = new GamePlayer { Power = France };
		var gp2 = new GamePlayer { Power = France };

		// Create minimal Game instances and assign distinct Game Numbers
		var g1 = new Game();
		var g2 = new Game();
		Set(g1, nameof (Game.Number), 1);
		Set(g2, nameof (Game.Number), 2);

		// Give games real Ids so CompareTo uses game tuple ordering
		g1.Id = 1;
		g2.Id = 2;
		// Attach games by setting Game property; that will set GameId via setter
		gp1.Game = g1;
		gp2.Game = g2;

		// Different game numbers drive ordering
		Assert.True(gp1.CompareTo(gp2) < 0);
		Assert.True(gp2.CompareTo(gp1) > 0);

		// If game numbers equal, use (Power, Player.Name)
		Set(g2, nameof (Game.Number), 1);
		// Set Player names to break ties
		var pA = new Player { Id = 1, FirstName = "Ann", LastName = "Z" };
		var pB = new Player { Id = 2, FirstName = "Bob", LastName = "A" };
		// Assign players directly to avoid cache/db dependency
		gp1.Player = pA;
		gp2.Player = pB;

		// Now compare: same game number and power, so compare Player.Name (A..Z vs B..A)
		Assert.True(gp1.CompareTo(gp2) < 0);
	}

	[Fact]
	public void Status_Shows_Correct_Symbols_For_Game_Status()
	{
		var gp = new GamePlayer();

		// Seeded => empty string
		var gSeeded = new Game();
		Set(gSeeded, nameof (Game.Status), Seeded);
		gp.Game = gSeeded;
		Assert.Equal(string.Empty, gp.Status);

		// Underway and incomplete => open circle; force incomplete by leaving Power TBD
		var gUnderway = new Game();
		Set(gUnderway, nameof (Game.Status), Underway);
		gp = new ()
			 {
				 Power = TBD,
				 Game = gUnderway
			 };
		Assert.Equal("◯", gp.Status);

		// Underway and complete => filled circle; make Power not TBD and ensure no required fields => complete
		var gUnderway2 = new Game();
		Set(gUnderway2, nameof (Game.Status), Underway);
		var gpComplete = new GamePlayer { Power = Austria };
		// Use a ScoringSystem with all Uses* flags false so PlayIncomplete is false
		var ssExisting = new ScoringSystem { UsesGameResult = false, UsesCenterCount = false, UsesYearsPlayed = false };
		// Assign scoring system directly to the game to avoid DB/cache
		gUnderway2.ScoringSystem = ssExisting;

		gpComplete.Game = gUnderway2;
		Assert.Equal("⬤", gpComplete.Status);

		// Finished => check mark
		var gFinished = new Game();
		Set(gFinished, nameof (Game.Status), Finished);
		gpComplete.Game = gFinished;
		Assert.Equal("✔", gpComplete.Status);
	}
}
