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
							Add(map, t);
							Add(map, r);
							Add(map, g);
							Add<GamePlayer>(map);
							Add<ScoringSystem>(map);
							Add<RoundPlayer>(map);
							Add<TournamentPlayer>(map);
							Add<Group>(map);
							Add<GroupPlayer>(map);
							Add<Team>(map);
							Add<TeamPlayer>(map);
							Add<PlayerConflict>(map);
							Add<Player>(map);
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
							Add(map, t);
							Add(map, r);
							Add(map, g);
							Add(map, gp1, gp2);
							Add(map, p1, p2);
							Add<ScoringSystem>(map);
							Add<RoundPlayer>(map);
							Add<TournamentPlayer>(map);
							Add<Group>(map);
							Add<GroupPlayer>(map);
							Add<Team>(map);
							Add<TeamPlayer>(map);
							Add<PlayerConflict>(map);
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
							Add(map, t);
							Add(map, r);
							Add(map, g);
							Add(map, gp1, gp2);
							Add(map, p1, p2);
							Add<ScoringSystem>(map);
							Add<RoundPlayer>(map);
							Add<TournamentPlayer>(map);
							Add<Group>(map);
							Add<GroupPlayer>(map);
							Add<Team>(map);
							Add<TeamPlayer>(map);
							Add<PlayerConflict>(map);
						}))
			Assert.Equal([111, 222], g.PlayerIds);
	}
}
