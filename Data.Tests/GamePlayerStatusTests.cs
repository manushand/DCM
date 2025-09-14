namespace Data.Tests;

[PublicAPI]
public sealed class GamePlayerStatusTests : TestBase
{
	[Fact]
	public void Status_Seeded_Is_Empty_String()
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
												 Seeded),
					 Power = Austria,
					 Result = Unknown
				 };
		Assert.Equal(string.Empty, gp.Status);
	}

	[Fact]
	public void Status_Underway_Incomplete_Shows_OpenCircle()
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
					 Result = Unknown // Incomplete because UsesGameResult=true and Result=Unknown
				 };
		Assert.Equal(UnderwayMark, gp.Status);
	}

	[Fact]
	public void Status_Underway_Complete_Shows_FilledCircle()
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
					 Power = Austria,
					 Result = Unknown // Complete because system doesn't require result/centers/years and power not TBD
				 };
		Assert.Equal(CompleteMark, gp.Status);
	}

	[Fact]
	public void Status_Finished_Shows_CheckMark()
	{
		var gp = new GamePlayer
				 {
					 Game = CreateGameWithSystem(new () { Id = 0, UsesGameResult = true }, Finished),
					 Power = Austria,
					 Result = Unknown
				 };
		Assert.Equal(FinishedMark, gp.Status);
	}

	private static Game CreateGameWithSystem(ScoringSystem system,
											 Game.Statuses status)
	{
		var t = new Tournament { Id = 11, Name = "T" };
		var r = new Round { Id = 12, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		return new ()
			   {
				   Id = 13,
				   Round = r,
				   Status = status,
				   ScoringSystem = system
			   };
	}
}
