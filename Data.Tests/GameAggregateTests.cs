namespace Data.Tests;

[PublicAPI]
public sealed class GameAggregateTests : TestBase
{
	[Fact]
	public void AveragePreGameScore_Defaults_To_UnplayedScore_When_No_GamePlayers()
	{
		var t = new Tournament { Id = 1, Name = "T", UnplayedScore = 7 };
		var r = new Round { Id = 2, Number = 1 };
		// Wire Round -> Tournament without DB/cache
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		var g = new Game { Id = 3, Round = r };

		// Seed empty GamePlayer cache for this game to ensure enumeration is empty
		using (SeedCache(map =>
						{
							AddOne(map, typeof (Tournament), t);
							AddOne(map, typeof (Round), r);
							AddOne(map, typeof (Game), g);
							AddEmpty(map, typeof (GamePlayer));
							AddEmpty(map, typeof (ScoringSystem));
							AddEmpty(map, typeof (RoundPlayer));
							AddEmpty(map, typeof (TournamentPlayer));
							AddEmpty(map, typeof (Group));
							AddEmpty(map, typeof (GroupPlayer));
							AddEmpty(map, typeof (Team));
							AddEmpty(map, typeof (TeamPlayer));
							AddEmpty(map, typeof (PlayerConflict));
							AddEmpty(map, typeof (Player));
						}))
			Assert.Equal(7, g.AveragePreGameScore);
	}

	[Fact]
	public void Conflict_Sums_GamePlayer_Conflicts()
	{
		var t = new Tournament { Id = 10, Name = "T" };
		var r = new Round { Id = 11, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		var g = new Game { Id = 12, Round = r };
		var p1 = new Player { Id = 101, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 102, FirstName = "B", LastName = "B" };
		var gp1 = new GamePlayer { Game = g, Player = p1, Power = Austria, Result = Unknown };
		var gp2 = new GamePlayer { Game = g, Player = p2, Power = England, Result = Unknown };
		// Set private _conflict fields directly to avoid invoking CalculateConflict
		SetField(gp1, "_conflict", 3);
		SetField(gp2, "_conflict", 4);

		using (SeedCache(map =>
						{
							AddOne(map, typeof (Tournament), t);
							AddOne(map, typeof (Round), r);
							AddOne(map, typeof (Game), g);
							AddMany(map, typeof (GamePlayer), gp1, gp2);
							AddMany(map, typeof (Player), p1, p2);
							AddEmpty(map, typeof (ScoringSystem));
							AddEmpty(map, typeof (RoundPlayer));
							AddEmpty(map, typeof (TournamentPlayer));
							AddEmpty(map, typeof (Group));
							AddEmpty(map, typeof (GroupPlayer));
							AddEmpty(map, typeof (Team));
							AddEmpty(map, typeof (TeamPlayer));
							AddEmpty(map, typeof (PlayerConflict));
						}))
			Assert.Equal(7, g.Conflict);
	}

	[Fact]
	public void PlayerIds_Reflect_GamePlayers()
	{
		var t = new Tournament { Id = 20, Name = "T" };
		var r = new Round { Id = 21, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		var g = new Game { Id = 22, Round = r };
		var p1 = new Player { Id = 111, FirstName = "A", LastName = "A" };
		var p2 = new Player { Id = 222, FirstName = "B", LastName = "B" };
		var gp1 = new GamePlayer { Game = g, Player = p1, Power = Austria, Result = Unknown };
		var gp2 = new GamePlayer { Game = g, Player = p2, Power = England, Result = Unknown };

		using (SeedCache(map =>
						{
							AddOne(map, typeof (Tournament), t);
							AddOne(map, typeof (Round), r);
							AddOne(map, typeof (Game), g);
							AddMany(map, typeof (GamePlayer), gp1, gp2);
							AddMany(map, typeof (Player), p1, p2);
							AddEmpty(map, typeof (ScoringSystem));
							AddEmpty(map, typeof (RoundPlayer));
							AddEmpty(map, typeof (TournamentPlayer));
							AddEmpty(map, typeof (Group));
							AddEmpty(map, typeof (GroupPlayer));
							AddEmpty(map, typeof (Team));
							AddEmpty(map, typeof (TeamPlayer));
							AddEmpty(map, typeof (PlayerConflict));
						}))
			Assert.Equal([111, 222], g.PlayerIds);
	}

	private static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic).OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", NonPublic | Static).OrThrow("Cache._data field not found");
		var original = field.GetValue(null).OrThrow();
		var typeMapType = original.GetType();
		var typeMap = CreateInstance(typeMapType).OrThrow();
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new (original, field);
	}

	private static void AddOne(object typeMap, Type type, object record)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		sortedDictType.GetMethod("Add")?.Invoke(sd, [GetPrimaryKey(record), record]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	private static void AddMany(object typeMap, Type type, params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		var sdAdd = sortedDictType.GetMethod("Add");
		foreach (var r in records)
			sdAdd?.Invoke(sd, [GetPrimaryKey(r), r]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	private static void AddEmpty(object typeMap, Type type)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType);
		typeMapType.GetMethod("Add").OrThrow().Invoke(typeMap, [type, sd]);
	}
}
