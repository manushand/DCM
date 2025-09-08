using DCM;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

using static Game.Statuses;
using static GamePlayer.Powers;
using static GamePlayer.Results;

[PublicAPI]
public sealed class GamePlayerPlayIncompleteTests
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
		Assert.Equal("◯", gp.Status);
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
		Assert.Equal("◯", gp.Status);
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
		Assert.Equal("◯", gp.Status);
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
		Assert.Equal("⬤", gp.Status);
	}

	private static Game CreateGameWithSystem(ScoringSystem system, Game.Statuses status)
	{
		var t = new Tournament { Id = 1, Name = "T" };
		var r = new Round { Id = 2, Number = 1 };
		// Attach tournament to round via private fields to avoid cache/DB
		typeof (Round).GetProperty("TournamentId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
					  .OrThrow()
					  .SetValue(r, t.Id);
		typeof (Round).GetField("<Tournament>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).OrThrow()
			.SetValue(r, t);
		var g = new Game
				{
					Id = 3,
					Round = r,
					Status = status,
					ScoringSystem = system
				};
		return g;
	}
}
