namespace Data.Tests;

[PublicAPI]
public sealed class RoundSeedTests : TestBase
{
	[Fact]
	public void Seed_Throws_When_PlayerCount_Not_Multiple_Of_7()
	{
		var r = new Round
				{
					Id = 1,
					Number = 1
				};
		// Make Tournament association minimal to avoid DB/cache: TournamentId set via reflection
		SetProperty(r, "TournamentId", 1);
		// Provide a dummy tournament to back Round.Tournament property to avoid accidental loads
		var t = new Tournament
				{
					Id = 1,
					Name = "T",
					TotalRounds = 1
				};
		typeof (Round).GetField("<Tournament>k__BackingField", Instance | NonPublic)
					  .OrThrow()
					  .SetValue(r, t);

		// Seed empty cache maps to prevent any DB-backed loads during the precondition check
		using (SeedCache())
		{
			var ex = Assert.Throws<ArgumentOutOfRangeException>(() => r.Seed([new ()], false)); // 1 is not multiple of 7
			Assert.Contains("Invalid number of roundPlayers", ex.Message);
		}
	}

	private static CacheScope SeedCache()
	{
		var dataType = typeof (Data);
		var cacheType = dataType.GetNestedType("Cache", NonPublic)
								.OrThrow();
		var field = cacheType.GetField("_data", NonPublic | Static)
							 .OrThrow();
		var original = field.GetValue(null)
							.OrThrow();
		var typeMap = CreateInstance(original.GetType()).OrThrow();
		AddEmpties(typeMap);
		field.SetValue(null, typeMap);
		return new (original, field);
	}
}
