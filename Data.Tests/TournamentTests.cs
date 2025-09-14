using System.Collections.Generic;

namespace Data.Tests;

[PublicAPI]
public sealed class TournamentTests : TestBase
{
	[Fact]
	public void Load_Parses_Combined_Fields_And_Defaults()
	{
		// Arrange: craft a row that exercises combined negative/decimal fields
		var values = new Dictionary<string, object?>
					 {
						 [nameof (Tournament.Id)] = 42,
						 [nameof (Tournament.Name)] = "WDC",
						 [nameof (Tournament.Date)] = null, // should default to DateTime.Today
						 [nameof (Tournament.Description)] = "Desc",
						 [nameof (Tournament.GroupId)] = null,
						 [nameof (Tournament.ScoringSystemId)] = 3,
						 [nameof (Tournament.TeamConflict)] = 1,
						 [nameof (Tournament.PlayerConflict)] = 2,
						 [nameof (Tournament.PowerConflict)] = 3,
						 [nameof (Tournament.TotalRounds)] = 4,
						 [nameof (Tournament.MinimumRounds)] = 2,
						 [nameof (Tournament.AssignPowers)] = 1,
						 [nameof (Tournament.GroupPowers)] = Corners.AsInteger,
						 [nameof (Tournament.UnplayedScore)] = 7,
						 [nameof (Tournament.RoundsToDrop)] = -2,       // negative => DropBeforeFinalRound = true, RoundsToDrop = 2
						 [nameof (Tournament.ScalePercentage)] = 2.75m, // => RoundsToScale = 2; ScalePercentage = 75
						 [nameof (Tournament.TeamSize)] = -3,           // negative => PlayerCanJoinManyTeams = true, TeamSize = 3
						 [nameof (Tournament.TeamRound)] = -4,          // negative => TeamsPlayMultipleRounds = true, TeamRound = 4
						 [nameof (Tournament.ScoreConflict)] = -5       // negative => ProgressiveScoreConflict = true, ScoreConflict = 5
					 };
		using var reader = new FakeDbDataReader("Tournament", values);
		var t = new Tournament();

		// Act
		t.Load(reader);

		// Assert
		Assert.Equal(42, t.Id);
		Assert.Equal("WDC", t.Name);
		Assert.Equal("Desc", t.Description);
		Assert.Equal(DateTime.Today, t.Date.Date);
		Assert.Equal(Corners, t.GroupPowers);
		Assert.Equal(2, t.RoundsToDrop);
		Assert.True(t.DropBeforeFinalRound);
		Assert.Equal(2, t.RoundsToScale);
		Assert.Equal(75, t.ScalePercentage);
		Assert.Equal(3, t.TeamSize);
		Assert.True(t.PlayerCanJoinManyTeams);
		Assert.Equal(4, t.TeamRound);
		Assert.True(t.TeamsPlayMultipleRounds);
		Assert.Equal(5, t.ScoreConflict);
		Assert.True(t.ProgressiveScoreConflict);
		Assert.True(t.IsEvent); // GroupId was null => Group.None => IsEvent true
	}

	[Fact]
	public void HasTeamTournament_Depends_On_TeamSize()
	{
		var t = new Tournament { TeamSize = 0 };
		Assert.False(t.HasTeamTournament);
		t.TeamSize = 3;
		Assert.True(t.HasTeamTournament);
	}

	[Fact]
	public void Games_Aggregate_From_Rounds_And_FinishedGames_Filter_And_Cache_Behavior()
	{
		var t = new Tournament { Id = 1 };
		var r1 = new Round { Id = 10, Number = 1 };
		var r2 = new Round { Id = 11, Number = 2 };
		// Tie rounds to tournament and set private _games
		SetProperty(r1, "TournamentId", t.Id);
		SetProperty(r2, (string)"TournamentId", t.Id);
		// Ensure rounds reference tournament without cache/DB by setting Tournament backing field via reflection
		SetField(r1, "<Tournament>k__BackingField", t);
		SetField(r2, "<Tournament>k__BackingField", t);
		var g1 = new Game { Status = Seeded };
		var g2 = new Game { Status = Underway };
		var g3 = new Game { Status = Finished };
		var g4 = new Game { Status = Finished };
		SetRoundGames(r1, g1, g2);
		SetRoundGames(r2, g3);

		using (SeedTournamentAndRoundsCache(t, [r1, r2]))
		{
			// First aggregation
			var games = t.Games;
			Assert.Equal(3, games.Length);
			Assert.Single(t.FinishedGames);

			// Modify a round's games after first read; Games should remain cached
			SetRoundGames(r2, g3, g4);
			Assert.Equal(3, t.Games.Length); // still cached

			// Force cache reset via private field and verify recompute
			SetField(t, "_games", null);
			Assert.Equal(4, t.Games.Length);
			Assert.Equal(2, t.FinishedGames.Length);
		}
	}

	[Fact]
	public void ScoringSystem_Setter_Propagates_To_Rounds_Using_Tournament_Default()
	{
		var t = new Tournament { Id = 2 };
		// initial ScoringSystemId defaults to 0
		var rDefault = new Round { Id = 20, Number = 1 }; // uses tournament default (no override)
		var rOverride = new Round { Id = 21, Number = 2 }; // has its own scoring system id
		SetProperty(rDefault, "TournamentId", t.Id);
		SetProperty(rOverride, "TournamentId", t.Id);
		// Ensure rounds reference tournament without cache/DB by setting Tournament property via reflection
		SetProperty(rDefault, "Tournament", t);
		SetProperty(rOverride, "Tournament", t);
		// set an override _scoringSystemId = 9 on rOverride
		SetField(rOverride, "_scoringSystemId", 9);

		using (SeedTournamentAndRoundsCache(t, [rDefault, rOverride]))
		{
			var newSystem = new ScoringSystem { Id = 5 };
			t.ScoringSystem = newSystem;
			// Tournament should adopt new id
			Assert.Equal(5, GetNonPublicProperty<int>(t, "ScoringSystemId"));
			// rDefault should now report new id
			Assert.Equal(5, rDefault.ScoringSystemId);
			// rOverride should remain unchanged
			Assert.Equal(9, rOverride.ScoringSystemId);
		}
	}

	// Helpers
	private static void SetRoundGames(Round r,
									  params Game[] games)
		=> SetField(r, "_games", games);

	private static CacheScope SeedTournamentAndRoundsCache(IdInfoRecord t,
														   IEnumerable<Round> rounds)
	{
		var dataType = typeof (Data);
		var cacheType = dataType.GetNestedType("Cache", NonPublic).OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", NonPublic | Static).OrThrow("Cache._data field not found");
		var original = field.GetValue(null).OrThrow();

		var typeMapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sortedDictType = typeMapType.GetGenericArguments()[1]; // SortedDictionary<string, IRecord>
		var typeMap = CreateInstance(typeMapType);
		var mapAdd = typeMapType.GetMethod("Add").OrThrow();

		// Tournament map
		var sdTournament = CreateInstance(sortedDictType).OrThrow();
		sortedDictType.GetMethod("Add")?.Invoke(sdTournament, [t.PrimaryKey, t]);
		mapAdd.Invoke(typeMap, [typeof (Tournament), sdTournament]);

		// Rounds map
		var sdRound = CreateInstance(sortedDictType);
		var sdAdd = sortedDictType.GetMethod("Add").OrThrow();
		foreach (var r in rounds)
			sdAdd.Invoke(sdRound, [r.PrimaryKey, r]);
		mapAdd.Invoke(typeMap, [typeof (Round), sdRound]);

		AddAnEmpty(typeof (Game));
		AddAnEmpty(typeof (ScoringSystem));
		AddAnEmpty(typeof (RoundPlayer));
		AddAnEmpty(typeof (TournamentPlayer));
		AddAnEmpty(typeof (Group));
		AddAnEmpty(typeof (GroupPlayer));
		AddAnEmpty(typeof (Team));
		AddAnEmpty(typeof (TeamPlayer));
		AddAnEmpty(typeof (PlayerConflict));

		field.SetValue(null, typeMap);
		return new (original, field);

		// Optional empty maps to prevent any accidental loads
		void AddAnEmpty(Type tType)
			=> mapAdd.Invoke(typeMap, [tType, CreateInstance(sortedDictType)]);
	}

	private static TVal GetNonPublicProperty<TVal>(object target,
												   string propertyName)
	{
		var prop = target.GetType().GetProperty(propertyName, Instance | NonPublic | Public);
		if (prop?.GetMethod is null)
			throw new InvalidOperationException($"Property {propertyName} not found or has no getter");
		return (TVal)prop.GetValue(target).OrThrow();
	}
}
