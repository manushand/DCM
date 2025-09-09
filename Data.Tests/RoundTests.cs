using System.Collections.Generic;

namespace Data.Tests;

[PublicAPI]
public sealed class RoundTests
{
	private static void SetGames(Round r, params Game[] games)
		=> typeof (Round).GetField("_games", Instance | NonPublic)?.SetValue(r, games);

	[Fact]
	public void Load_Sets_Fields()
	{
		var values = new Dictionary<string, object?>
					 {
						 [nameof (Round.Id)] = 11,
						 [nameof (Round.Number)] = 3,
						 ["TournamentId"] = 99,
						 [nameof (Round.ScoringSystemId)] = null
					 };
		using var reader = new FakeDbDataReader("Round", values);
		var r = new Round();

		r.Load(reader);

		Assert.Equal(11, r.Id);
		Assert.Equal(3, r.Number);
		Assert.True(r.ScoringSystemIsDefault);
	}

	[Fact]
	public void ScoringSystem_IsDefault_Depends_On_Loaded_ScoringSystemId()
	{
		// Default/null scoring system id -> default
		var r = new Round();
		var values1 = new Dictionary<string, object?>
					  {
						  [nameof (Round.Id)] = 1,
						  [nameof (Round.Number)] = 1,
						  ["TournamentId"] = 1,
						  [nameof (Round.ScoringSystemId)] = null
					  };
		using var reader1 = new FakeDbDataReader("Round", values1);
		r.Load(reader1);
		Assert.True(r.ScoringSystemIsDefault);

		// Non-default scoring system id -> override
		r = new ();
		var values2 = new Dictionary<string, object?>
					  {
						  [nameof (Round.Id)] = 2,
						  [nameof (Round.Number)] = 1,
						  ["TournamentId"] = 1,
						  [nameof (Round.ScoringSystemId)] = 1
					  };
		using var reader2 = new FakeDbDataReader("Round", values2);
		r.Load(reader2);
		Assert.False(r.ScoringSystemIsDefault);
		Assert.Equal(1, r.ScoringSystemId);
	}

	[Fact]
	public void Name_Returns_Number_String()
		=> Assert.Equal("5", new Round { Number = 5 }.Name);

	[Fact]
	public void CompareTo_Compares_By_Number()
	{
		var r1 = new Round { Number = 1 };
		var r2 = new Round { Number = 2 };
		Assert.True(r1.CompareTo(r2) < 0);
		Assert.True(r2.CompareTo(r1) > 0);
		Assert.Equal(0, r1.CompareTo(new () { Number = 1 }));
	}

	[Fact]
	public void Games_Filtering_And_Flags_Work()
	{
		var r = new Round();
		var g1 = new Game { Status = Seeded };
		var g2 = new Game { Status = Underway };
		var g3 = new Game { Status = Finished };
		SetGames(r, g1, g2, g3);

		Assert.Single(r.SeededGames);
		Assert.Contains(g1, r.SeededGames);

		Assert.Equal(2, r.StartedGames.Length);
		Assert.Contains(g2, r.StartedGames);
		Assert.Contains(g3, r.StartedGames);

		Assert.Single(r.FinishedGames);
		Assert.Contains(g3, r.FinishedGames);

		Assert.True(r.GamesSeeded);
		Assert.True(r.GamesStarted);
	}

	[Fact]
	public void Status_Computed_From_Games()
	{
		var r = new Round();

		SetGames(r, new Game { Status = Seeded });
		Assert.Equal(Seeded, r.Status);

		SetGames(r, new Game { Status = Underway });
		Assert.Equal(Underway, r.Status);

		SetGames(r, new Game { Status = Finished });
		Assert.Equal(Finished, r.Status);
	}

	[Fact]
	public void ScoringSystem_Setter_Toggles_Default_State()
	{
		var r = new Round();
		SetGames(r); // prevent DB/cache calls via Games
		Assert.True(r.ScoringSystemIsDefault);

		r.ScoringSystem = new () { Id = 0 };
		Assert.True(r.ScoringSystemIsDefault);

		r.ScoringSystem = new () { Id = 5 };
		Assert.False(r.ScoringSystemIsDefault);
		Assert.Equal(5, r.ScoringSystemId);
	}

	[Fact]
	public void RoundPlayers_Returns_Reflected_Field()
	{
		var r = new Round();
		var rp1 = new RoundPlayer();
		var rp2 = new RoundPlayer();
		var field = typeof (Round).GetField("_roundPlayers", Instance | NonPublic).OrThrow();
		field.SetValue(r, new[] { rp1, rp2 });
		Assert.Same(rp1, r.RoundPlayers[0]);
		Assert.Same(rp2, r.RoundPlayers[1]);
	}

	[Fact]
	public void ScoringSystem_Setter_Propagates_To_Games_Using_Round_System()
	{
		var r = new Round();
		// Two games in this round: one using id 5 (will be aligned to round), one using id 6 (should remain override)
		var g1 = new Game { Round = r, ScoringSystem = new () { Id = 5 } };
		var g2 = new Game { Round = r, ScoringSystem = new () { Id = 6 } };
		SetGames(r, g1, g2);

		// Now set the round scoring system to 5; this should propagate to g1 (making it default), not affect g2
		r.ScoringSystem = new () { Id = 5 };

		Assert.False(r.ScoringSystemIsDefault);
		Assert.Equal(5, r.ScoringSystemId);
		Assert.True(g1.ScoringSystemIsDefault);
		Assert.Equal(6, g2.ScoringSystemId);
	}
}
