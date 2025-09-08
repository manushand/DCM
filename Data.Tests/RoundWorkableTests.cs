using System;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;
using static System.Activator;

namespace Data.Tests;

using DCM;

[PublicAPI]
public sealed class RoundWorkableTests
{
	private static void SetPrivate(object target, string field, object? value)
		=> target.GetType()
				 .GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	private static void SetNonPublic(object target, string prop, object? value)
		=> target.GetType()
				 .GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	[Fact]
	public void Workable_True_When_No_Games()
	{
		using (SeedEmptyCache())
		{
			var t = new Tournament { Id = 1, Name = "T", TotalRounds = 3 };
			var r1 = new Round { Id = 10, Number = 1 };
			var r2 = new Round { Id = 11, Number = 2 };
			var r3 = new Round { Id = 12, Number = 3 };
			SetNonPublic(r1, "TournamentId", t.Id);
			SetPrivate(r1, "<Tournament>k__BackingField", t);
			SetNonPublic(r2, "TournamentId", t.Id);
			SetPrivate(r2, "<Tournament>k__BackingField", t);
			SetNonPublic(r3, "TournamentId", t.Id);
			SetPrivate(r3, "<Tournament>k__BackingField", t);
			// No games assigned to any round.
			// Round.Workable clause includes "|| Games.Length is 0"
			Assert.True(r1.Workable);
			Assert.True(r2.Workable);
			Assert.True(r3.Workable);
		}
	}

	private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	private static CacheScope SeedEmptyCache()
		=> SeedRoundsCache();

	private static CacheScope SeedRoundsCache(Tournament? tournament = null, params Round[] rounds)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", BindingFlags.NonPublic).OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static).OrThrow("Cache._data field not found");
		var original = field.GetValue(null).OrThrow();
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
			AddEmpty(typeof (Tournament));
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
			AddEmpty(typeof (Round));
		AddEmpty(typeof (Game));
		AddEmpty(typeof (ScoringSystem));
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

		void AddEmpty(Type t) => mapAdd.Invoke(typeMap, [t, CreateInstance(sortedDictType)]);
	}

	[Fact]
	public void Workable_True_For_Latest_Round_With_Unfinished_Game()
	{
		var t = new Tournament { Id = 2, Name = "T", TotalRounds = 2 };
		var r1 = new Round { Id = 20, Number = 1 };
		var r2 = new Round { Id = 21, Number = 2 };
		SetNonPublic(r1, "TournamentId", t.Id);
		SetPrivate(r1, "<Tournament>k__BackingField", t);
		SetNonPublic(r2, "TournamentId", t.Id);
		SetPrivate(r2, "<Tournament>k__BackingField", t);
		using (SeedRoundsCache(t, r1, r2))
		{
			// r2 has a seeded (not finished) game -> Workable should be true for r2 as it's the latest round
			var gSeeded = new Game { Status = Game.Statuses.Seeded };
			typeof (Round).GetField("_games", BindingFlags.Instance | BindingFlags.NonPublic)
						  .OrThrow()
						  .SetValue(r2, new[] { gSeeded });
			Assert.True(r2.Workable);
			// r1 has only finished games and is not the latest round -> Workable should be false
			var gFinished = new Game { Status = Game.Statuses.Finished };
			typeof (Round).GetField("_games", BindingFlags.Instance | BindingFlags.NonPublic)
						  .OrThrow()
						  .SetValue(r1, new[] { gFinished });
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
			SetNonPublic(r1, "TournamentId", t.Id);
			SetPrivate(r1, "<Tournament>k__BackingField", t);
			SetNonPublic(r2, "TournamentId", t.Id);
			SetPrivate(r2, "<Tournament>k__BackingField", t);
			// All games in r2 finished and r2.Number == TotalRounds -> Workable false
			var gFinished = new Game { Status = Game.Statuses.Finished };
			typeof (Round).GetField("_games", BindingFlags.Instance | BindingFlags.NonPublic)
						  .OrThrow()
						  .SetValue(r2, new[] { gFinished });
			Assert.False(r2.Workable);
		}
	}
}
