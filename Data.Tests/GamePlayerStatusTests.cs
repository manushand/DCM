using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class GamePlayerStatusTests
{
	[Fact]
	public void Status_Seeded_Is_Empty_String()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ScoringSystem { Id = 0, UsesGameResult = false, UsesCenterCount = false, UsesYearsPlayed = false }, Game.Statuses.Seeded),
			Power = GamePlayer.Powers.Austria,
			Result = GamePlayer.Results.Unknown
		};
		Assert.Equal(string.Empty, gp.Status);
	}

	[Fact]
	public void Status_Underway_Incomplete_Shows_OpenCircle()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ScoringSystem { Id = 0, UsesGameResult = true }, Game.Statuses.Underway),
			Power = GamePlayer.Powers.Austria,
			Result = GamePlayer.Results.Unknown // Incomplete because UsesGameResult=true and Result=Unknown
		};
		Assert.Equal("◯", gp.Status);
	}

	[Fact]
	public void Status_Underway_Complete_Shows_FilledCircle()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ScoringSystem { Id = 0, UsesGameResult = false, UsesCenterCount = false, UsesYearsPlayed = false }, Game.Statuses.Underway),
			Power = GamePlayer.Powers.Austria,
			Result = GamePlayer.Results.Unknown // Complete because system doesn't require result/centers/years and power not TBD
		};
		Assert.Equal("⬤", gp.Status);
	}

	[Fact]
	public void Status_Finished_Shows_CheckMark()
	{
		var gp = new GamePlayer
		{
			Game = CreateGameWithSystem(new ScoringSystem { Id = 0, UsesGameResult = true }, Game.Statuses.Finished),
			Power = GamePlayer.Powers.Austria,
			Result = GamePlayer.Results.Unknown
		};
		Assert.Equal("✔", gp.Status);
	}

	private static Game CreateGameWithSystem(ScoringSystem system, Game.Statuses status)
	{
		var t = new Tournament { Id = 11, Name = "T" };
		var r = new Round { Id = 12, Number = 1 };
		typeof(Round).GetProperty("TournamentId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)!
			.SetValue(r, t.Id);
		typeof(Round).GetField("<Tournament>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
			.SetValue(r, t);
		var g = new Game { Id = 13, Round = r, Status = status };
		g.ScoringSystem = system;
		return g;
	}
}
