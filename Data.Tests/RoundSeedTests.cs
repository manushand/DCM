using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

using DCM;

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
		typeof (Round).GetProperty("TournamentId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
					  .OrThrow()
					  .SetValue(r, 1);
		// Provide a dummy tournament to back Round.Tournament property to avoid accidental loads
		var t = new Tournament
				{
					Id = 1,
					Name = "T",
					TotalRounds = 1
				};
		typeof (Round).GetField("<Tournament>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)
					  .OrThrow()
					  .SetValue(r, t);

		// Seed empty cache maps to prevent any DB-backed loads during the precondition check
		using (SeedCache())
		{
			var list = new List<RoundPlayer> { new (), new (), new () }; // 3 is not multiple of 7
			var ex = Assert.Throws<ArgumentOutOfRangeException>(() => r.Seed(list, false));
			Assert.Contains("Invalid number of roundPlayers", ex.Message);
		}
	}

	private static CacheScope SeedCache()
	{
		var dataType = typeof (Data);
		var cacheType = dataType.GetNestedType("Cache", BindingFlags.NonPublic)
								.OrThrow();
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static)
							 .OrThrow();
		var original = field.GetValue(null)
							.OrThrow();
		var typeMapType = original.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var typeMap = Activator.CreateInstance(typeMapType);
		var mapAdd = typeMapType.GetMethod("Add")
								.OrThrow();
		AddEmpty(typeof (Tournament));
		AddEmpty(typeof (Round));
		AddEmpty(typeof (Game));
		AddEmpty(typeof (GamePlayer));
		AddEmpty(typeof (RoundPlayer));
		AddEmpty(typeof (TournamentPlayer));
		AddEmpty(typeof (Group));
		AddEmpty(typeof (GroupPlayer));
		AddEmpty(typeof (Team));
		AddEmpty(typeof (TeamPlayer));
		AddEmpty(typeof (PlayerConflict));
		AddEmpty(typeof (Player));
		AddEmpty(typeof (ScoringSystem));
		field.SetValue(null, typeMap);
		return new (original, field);

		void AddEmpty(Type t) => mapAdd.Invoke(typeMap, [t, Activator.CreateInstance(sortedDictType)]);
	}
}
