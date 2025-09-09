using System.Collections.Generic;

namespace Data.Tests;

[PublicAPI]
public sealed class GameTests : TestBase
{
	[Fact]
	public void Load_Sets_Fields_And_Defaulters()
	{
		var values = new Dictionary<string, object?>
					 {
						 { nameof (Game.Id), 7 },
						 { nameof (Game.Number), 2 },
						 { nameof (Game.Status), Seeded.AsInteger },
						 { nameof (Game.RoundId), 123 },
						 { nameof (Game.Name), "Board 2" },
						 { nameof (Game.ScoringSystemId), null },
						 { nameof (Game.Date), null }
					 };
		using var reader = new FakeDbDataReader("Game", values);
		var g = new Game();

		g.Load(reader);

		Assert.Equal(7, g.Id);
		Assert.Equal(2, g.Number);
		Assert.Equal(Seeded, g.Status);
		Assert.Equal('B', g.Letter);
		Assert.Equal("Board 2", g.Name);
		Assert.Equal("Board 2", g.FullName);
		Assert.True(g.ScoringSystemIsDefault);
	}

	[Fact]
	public void FullName_Falls_Back_When_Name_Empty()
	{
		var g = new Game { Number = 1, Round = new () { Number = 1 } };
		// Name defaults to empty; Round and Tournament are default None except Round.Number set
		Assert.Contains("── NONE ──", g.FullName);
		Assert.Contains("1", g.FullName);
		Assert.Equal('A', g.Letter);
	}

	[Fact]
	public void ScoringSystem_IsDefault_Toggle()
	{
		var g = new Game { Round = new () };
		Assert.True(g.ScoringSystemIsDefault);

		g.ScoringSystem = new () { Id = 0 };
		Assert.True(g.ScoringSystemIsDefault);

		g.ScoringSystem = new () { Id = 2 };
		Assert.False(g.ScoringSystemIsDefault);
		Assert.Equal(2, g.ScoringSystemId);
	}

	[Fact]
	public void CompareTo_Compares_By_Number()
	{
		var g1 = new Game { Number = 1 };
		var g2 = new Game { Number = 2 };
		Assert.True(g1.CompareTo(g2) < 0);
		Assert.True(g2.CompareTo(g1) > 0);
		Assert.Equal(0, g1.CompareTo(new () { Number = 1 }));
	}

	[Fact]
	public void Round_Init_Sets_RoundId()
	{
		var g = new Game { Round = new () { Id = 10 } };
		Assert.Equal(10, g.RoundId);
		Assert.Equal(10, g.Round.Id);
	}

	[Fact]
	public void Date_Defaults_To_Tournament_Date_And_Can_Be_Set()
	{
		var g = new Game { Round = new () };
		var fallback = g.Tournament.Date;
		Assert.Equal(fallback, g.Date);
		var dt = new DateTime(2024, 1, 1);
		g.Date = dt;
		Assert.Equal(dt, g.Date);
	}

	[Fact]
	public void CalculateScores_Returns_False_When_Not_Enough_Players()
	{
		var g = new Game { Id = 100, Round = new () { Id = 200 } };
		// Ensure cache has an entry for GamePlayer but with zero entries for this Game
		using (SeedGamePlayersCache([]))
		{
			var ok = g.CalculateScores(out var issues);
			Assert.False(ok);
			Assert.False(g.Scored);
			Assert.Contains("All powers must be assigned.", issues);
		}
	}

	[Fact]
	public void CalculateScores_Succeeds_With_Seven_Players_And_Constant_Formula()
	{
		var g = new Game { Id = 101, Round = new () { Id = 201 } };
		var players = SevenPlayersFor(g);
		using (SeedGamePlayersCache(players))
		{
			var system = new ScoringSystem { FinalScoreFormula = "1", SignificantDigits = 0 };
			var ok = g.CalculateScores(out var errors, system);
			Assert.True(ok);
			Assert.True(g.Scored);
			Assert.DoesNotContain(errors, static e => e is not null && (e.StartsWith("No solo") || e.StartsWith("All powers")));
			Assert.All(players, static p => Assert.Equal(1, p.FinalScore));
		}
	}

	[Fact]
	public void CalculateScores_Uses_Override_ScoringSystem_When_Provided()
	{
		var g = new Game { Id = 102, Round = new () { Id = 202 } };
		var players = SevenPlayersFor(g);
		using (SeedGamePlayersCache(players))
		{
			g.ScoringSystem = new () { FinalScoreFormula = "2", SignificantDigits = 0, Id = 5 };
			var overrideSystem = new ScoringSystem { FinalScoreFormula = "3", SignificantDigits = 0 };
			var ok = g.CalculateScores(out _, overrideSystem);
			Assert.True(ok);
			Assert.All(players, static p => Assert.Equal(3, p.FinalScore));
		}
	}

	private static List<GamePlayer> SevenPlayersFor(Game g)
	{
		var powerValues = new[]
		{
			Austria,
			England,
			France,
			Germany,
			Italy,
			Russia,
			Turkey
		};
		var list = new List<GamePlayer>();
		for (var i = 0; i < 7; i++)
		{
			var gp = new GamePlayer
			{
				Power = powerValues[i],
				Result = Unknown,
				Game = g,
				Player = new () { Id = i + 1, Name = $"P{i + 1}" }
			};
			list.Add(gp);
		}
		return list;
	}

	private static CacheScope SeedGamePlayersCache(List<GamePlayer> players)
	{
		var dataType = typeof (Data);
		var cacheType = dataType.GetNestedType("Cache", NonPublic)
								.OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", NonPublic | Static)
							 .OrThrow("Cache._data field not found");
		var original = field.GetValue(null)
							.OrThrow();

		// Use the existing generic types from the original cache instance
		var typeMapType = original.GetType();                      // Dictionary<Type, SortedDictionary<string, IRecord>>
		var sortedDictType = typeMapType.GetGenericArguments()[1]; // SortedDictionary<string, IRecord>
		var typeMap = CreateInstance(typeMapType);
		var sd = CreateInstance(sortedDictType); // for GamePlayer
		var sdAdd = sortedDictType.GetMethod("Add")
								  .OrThrow();
		foreach (var gp in players)
			sdAdd.Invoke(sd, [gp.PrimaryKey, gp]);
		var mapAdd = typeMapType.GetMethod("Add")
								.OrThrow();
		mapAdd.Invoke(typeMap, [typeof (GamePlayer), sd]);

		AddEmpty(typeof (Round));
		AddEmpty(typeof (Tournament));
		AddEmpty(typeof (ScoringSystem));
		AddEmpty(typeof (Game));
		AddEmpty(typeof (RoundPlayer));
		AddEmpty(typeof (TournamentPlayer));
		AddEmpty(typeof (Group));
		AddEmpty(typeof (GroupPlayer));
		AddEmpty(typeof (Team));
		AddEmpty(typeof (TeamPlayer));
		AddEmpty(typeof (PlayerConflict));
		AddEmpty(typeof (Player));
		field.SetValue(null, typeMap);

		return new (original, field);

		void AddEmpty(Type t)
		{
			var e = CreateInstance(sortedDictType);
			mapAdd.Invoke(typeMap, [t, e]);
		}
	}
}
