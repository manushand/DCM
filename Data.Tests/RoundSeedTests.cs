using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class RoundSeedTests
{
	[Fact]
	public void Seed_Throws_When_PlayerCount_Not_Multiple_Of_7()
	{
		var r = new Round { Id = 1, Number = 1, };
		// Make Tournament association minimal to avoid DB/cache: TournamentId set via reflection
		typeof(Round).GetProperty("TournamentId", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
			.SetValue(r, 1);
		// Provide a dummy tournament to back Round.Tournament property to avoid accidental loads
		var t = new Tournament { Id = 1, Name = "T", TotalRounds = 1 };
		typeof(Round).GetField("<Tournament>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
			.SetValue(r, t);

		// Seed empty cache maps to prevent any DB-backed loads during the precondition check
		using (SeedCache())
		{
			var list = new List<RoundPlayer> { new RoundPlayer(), new RoundPlayer(), new RoundPlayer() }; // 3 is not multiple of 7
			var ex = Assert.Throws<ArgumentOutOfRangeException>(() => r.Seed(list, assignPowers: false));
			Assert.Contains("Invalid number of roundPlayers", ex.Message);
		}
	}

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable { public void Dispose() => Field.SetValue(null, Original); }

	private static CacheScope SeedCache()
	{
		var dataType = typeof(Data);
		var cacheType = dataType.GetNestedType("Cache", BindingFlags.NonPublic)!;
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static)!;
		var original = field.GetValue(null)!;
		var typeMapType = original.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var typeMap = System.Activator.CreateInstance(typeMapType)!;
		var mapAdd = typeMapType.GetMethod("Add")!;
		void AddEmpty(System.Type t) => mapAdd.Invoke(typeMap, new object?[] { t, System.Activator.CreateInstance(sortedDictType) });
		AddEmpty(typeof(Tournament));
		AddEmpty(typeof(Round));
		AddEmpty(typeof(Game));
		AddEmpty(typeof(GamePlayer));
		AddEmpty(typeof(RoundPlayer));
		AddEmpty(typeof(TournamentPlayer));
		AddEmpty(typeof(Group));
		AddEmpty(typeof(GroupPlayer));
		AddEmpty(typeof(Team));
		AddEmpty(typeof(TeamPlayer));
		AddEmpty(typeof(PlayerConflict));
		AddEmpty(typeof(Player));
		AddEmpty(typeof(ScoringSystem));
		field.SetValue(null, typeMap);
		return new CacheScope(original, field);
	}
}
