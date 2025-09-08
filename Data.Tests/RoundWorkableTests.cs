using System;
using System.Reflection;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

using static System.Activator;

[PublicAPI]
public sealed class RoundWorkableTests
{
    private static void SetPrivate(object target, string field, object? value)
        => target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(target, value);

    private static void SetNonPublic(object target, string prop, object? value)
    {
        var p = target.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        p!.SetValue(target, value);
    }

    [Fact]
    public void Workable_True_When_No_Games()
    {
        using (SeedEmptyCache())
        {
        var t = new Tournament { Id = 1, Name = "T", TotalRounds = 3 };
        var r1 = new Round { Id = 10, Number = 1 };
        var r2 = new Round { Id = 11, Number = 2 };
        var r3 = new Round { Id = 12, Number = 3 };
        SetNonPublic(r1, "TournamentId", t.Id); SetPrivate(r1, "<Tournament>k__BackingField", t);
        SetNonPublic(r2, "TournamentId", t.Id); SetPrivate(r2, "<Tournament>k__BackingField", t);
        SetNonPublic(r3, "TournamentId", t.Id); SetPrivate(r3, "<Tournament>k__BackingField", t);
        // No games assigned to any round
        // Round.Workable clause includes "|| Games.Length is 0"
        Assert.True(r1.Workable);
        Assert.True(r2.Workable);
        Assert.True(r3.Workable);
        }
    }

    private sealed record CacheScope(object Original, FieldInfo Field) : IDisposable { public void Dispose() => Field.SetValue(null, Original); }

    private static CacheScope SeedEmptyCache()
    {
        return SeedRoundsCache();
    }

    private static CacheScope SeedRoundsCache(Tournament? tournament = null, params Round[] rounds)
    {
        var cacheType = typeof(global::Data.Data).GetNestedType("Cache", BindingFlags.NonPublic) ?? throw new InvalidOperationException("Cache type not found");
        var field = cacheType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException("Cache._data field not found");
        var original = field.GetValue(null) ?? throw new NullReferenceException();
        var typeMapType = original.GetType();
        var sortedDictType = typeMapType.GetGenericArguments()[1];
        var typeMap = CreateInstance(typeMapType)!;
        var mapAdd = typeMapType.GetMethod("Add")!;
        void AddEmpty(Type t) => mapAdd.Invoke(typeMap, new object?[] { t, CreateInstance(sortedDictType) });
        // Seed Tournament and Rounds maps appropriately
        if (tournament is not null)
        {
            var sdTournament = CreateInstance(sortedDictType)!;
            sortedDictType.GetMethod("Add")!.Invoke(sdTournament, new object?[] { tournament.PrimaryKey, tournament });
            mapAdd.Invoke(typeMap, new object?[] { typeof(Tournament), sdTournament });
        }
        else
        {
            AddEmpty(typeof(Tournament));
        }
        // Rounds
        if (rounds is { Length: > 0 })
        {
            var sdRound = CreateInstance(sortedDictType)!;
            var sdAdd = sortedDictType.GetMethod("Add")!;
            foreach (var r in rounds)
                sdAdd.Invoke(sdRound, new object?[] { r.PrimaryKey, r });
            mapAdd.Invoke(typeMap, new object?[] { typeof(Round), sdRound });
        }
        else
        {
            AddEmpty(typeof(Round));
        }
        AddEmpty(typeof(Game));
        AddEmpty(typeof(ScoringSystem));
        AddEmpty(typeof(RoundPlayer));
        AddEmpty(typeof(TournamentPlayer));
        AddEmpty(typeof(Group));
        AddEmpty(typeof(GroupPlayer));
        AddEmpty(typeof(Team));
        AddEmpty(typeof(TeamPlayer));
        AddEmpty(typeof(PlayerConflict));
        AddEmpty(typeof(Player));
        field.SetValue(null, typeMap);
        return new CacheScope(original, field);
    }

    [Fact]
    public void Workable_True_For_Latest_Round_With_Unfinished_Game()
    {
        var t = new Tournament { Id = 2, Name = "T", TotalRounds = 2 };
        var r1 = new Round { Id = 20, Number = 1 };
        var r2 = new Round { Id = 21, Number = 2 };
        SetNonPublic(r1, "TournamentId", t.Id); SetPrivate(r1, "<Tournament>k__BackingField", t);
        SetNonPublic(r2, "TournamentId", t.Id); SetPrivate(r2, "<Tournament>k__BackingField", t);
        using (SeedRoundsCache(t, r1, r2))
        {
        // r2 has a seeded (not finished) game -> Workable should be true for r2 as it's the latest round
        var gSeeded = new Game { Status = Game.Statuses.Seeded };
        typeof(Round).GetField("_games", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(r2, new[] { gSeeded });
        Assert.True(r2.Workable);
        // r1 has only finished games and is not the latest round -> Workable should be false
        var gFinished = new Game { Status = Game.Statuses.Finished };
        typeof(Round).GetField("_games", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(r1, new[] { gFinished });
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
        SetNonPublic(r1, "TournamentId", t.Id); SetPrivate(r1, "<Tournament>k__BackingField", t);
        SetNonPublic(r2, "TournamentId", t.Id); SetPrivate(r2, "<Tournament>k__BackingField", t);
        // All games in r2 finished and r2.Number == TotalRounds -> Workable false
        var gFinished = new Game { Status = Game.Statuses.Finished };
        typeof(Round).GetField("_games", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(r2, new[] { gFinished });
        Assert.False(r2.Workable);
        }
    }
}
