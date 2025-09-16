namespace Data.Tests;

[PublicAPI]
public sealed class GamePlayerPlayIncompleteTests : TestBase
{
	[Fact]
	public void PlayIncomplete_When_Requires_Result_And_Result_Unknown()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ()
										{
											Id = 0,
											UsesGameResult = true
										},
										Underway),
			Power = Austria,
			Result = Unknown
		};
		// Underway + requires result but unknown => Status should be open circle (incomplete)
		Assert.Equal(UnderwayMark, gp.Status);
	}

	[Fact]
	public void PlayIncomplete_When_Requires_Centers_But_Centers_Null()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ()
										{
											Id = 0,
											UsesCenterCount = true
										},
										Underway),
			Power = Austria,
			Centers = null,
			Result = Unknown
		};
		Assert.Equal(UnderwayMark, gp.Status);
	}

	[Fact]
	public void PlayIncomplete_When_Requires_Years_But_Years_Null()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ()
										{
											Id = 0,
											UsesYearsPlayed = true
										},
										Underway),
			Power = Austria,
			Years = null,
			Result = Unknown
		};
		Assert.Equal(UnderwayMark, gp.Status);
	}

	[Fact]
	public void PlayComplete_When_No_Requirements_And_Power_Assigned()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ()
										{
											Id = 0,
											UsesGameResult = false,
											UsesCenterCount = false,
											UsesYearsPlayed = false
										},
										Underway),
			Power = England,
			Result = Unknown
		};
		Assert.Equal(CompleteMark, gp.Status);
	}
}
