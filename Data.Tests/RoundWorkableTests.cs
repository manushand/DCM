namespace Data.Tests;

[PublicAPI]
public sealed class RoundWorkableTests : TestBase
{
	[Fact]
	public void Workable_True_When_No_Games()
	{
		using (SeedEmptyCache())
		{
			var t = new Tournament { Id = 1, Name = "T", TotalRounds = 3 };
			var r1 = new Round { Id = 10, Number = 1 };
			var r2 = new Round { Id = 11, Number = 2 };
			var r3 = new Round { Id = 12, Number = 3 };
			SetProperty(r1, "TournamentId", t.Id);
			SetField(r1, "<Tournament>k__BackingField", t);
			SetProperty(r2, "TournamentId", t.Id);
			SetField(r2, "<Tournament>k__BackingField", t);
			SetProperty(r3, "TournamentId", t.Id);
			SetField(r3, "<Tournament>k__BackingField", t);
			// No games assigned to any round.
			// Round.Workable clause includes the check for "Games.Length is 0"
			Assert.True(r1.Workable);
			Assert.True(r2.Workable);
			Assert.True(r3.Workable);
		}
	}

	private static CacheScope SeedEmptyCache()
		=> SeedRoundsCache();

	private static CacheScope SeedRoundsCache(Tournament? tournament = null,
											  params Round[] rounds)
	{
		var original = DataField.GetValue(null).OrThrow();
		var typeMapType = original.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var typeMap = CreateInstance(typeMapType).OrThrow();
		var mapAdd = typeMapType.GetMethod("Add").OrThrow();
		// Seed Tournament and Rounds maps appropriately
		if (tournament is not null)
		{
			var sdTournament = CreateInstance(sortedDictType).OrThrow();
			sortedDictType.GetMethod("Add")
						  .OrThrow()
						  .Invoke(sdTournament, [tournament.PrimaryKey, tournament]);
			mapAdd.Invoke(typeMap, [typeof (Tournament), sdTournament]);
		}
		else
			AddEmpty(typeMap, typeof (Tournament));
		// Rounds
		if (rounds is { Length: > 0 })
		{
			var sdRound = CreateInstance(sortedDictType).OrThrow();
			var sdAdd = sortedDictType.GetMethod("Add").OrThrow();
			foreach (var r in rounds)
				sdAdd.Invoke(sdRound, [r.PrimaryKey, r]);
			mapAdd.Invoke(typeMap, [typeof (Round), sdRound]);
		}
		else
			AddEmpty(typeMap, typeof (Round));
		AddEmpties(typeMap);
		DataField.SetValue(null, typeMap);
		return new (original, DataField);
	}

	[Fact]
	public void Workable_True_For_Latest_Round_With_Unfinished_Game()
	{
		var t = new Tournament { Id = 2, Name = "T", TotalRounds = 2 };
		var r1 = new Round { Id = 20, Number = 1 };
		var r2 = new Round { Id = 21, Number = 2 };
		SetProperty(r1, "TournamentId", t.Id);
		SetField(r1, "<Tournament>k__BackingField", t);
		SetProperty(r2, "TournamentId", t.Id);
		SetField(r2, "<Tournament>k__BackingField", t);
		using (SeedRoundsCache(t, r1, r2))
		{
			// r2 has a seeded (not finished) game -> Workable should be true for r2 as it's the latest round
			var gSeeded = new Game { Status = Seeded };
			SetField(r2, "_games", new[] { gSeeded });
			Assert.True(r2.Workable);
			// r1 has only finished games and is not the latest round -> Workable should be false
			var gFinished = new Game { Status = Finished };
			SetField(r1, "_games", new[] { gFinished });
			Assert.False(r1.Workable);
		}
	}

	[Fact]
	public void Workable_False_For_Latest_Round_When_All_Games_Finished_And_Number_Equals_Total()
	{
		using (SeedEmptyCache())
		{
			var t = new Tournament { Id = 3, Name = "T", TotalRounds = 2 };
			var r1 = new Round { Id = 30, Number = 1 };
			var r2 = new Round { Id = 31, Number = 2 };
			SetProperty(r1, "TournamentId", t.Id);
			SetField(r1, "<Tournament>k__BackingField", t);
			SetProperty(r2, "TournamentId", t.Id);
			SetField(r2, "<Tournament>k__BackingField", t);
			// All games in r2 finished and r2.Number == TotalRounds -> Workable false
			var gFinished = new Game { Status = Finished };
			SetField(r2, "_games", new[] { gFinished });
			Assert.False(r2.Workable);
		}
	}
}
