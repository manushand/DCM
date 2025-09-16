global using System;
global using JetBrains.Annotations;
global using Xunit;
global using static System.Activator;
global using static System.Reflection.BindingFlags;
//
global using DCM;
global using Data.Tests.Helpers;
global using static Data.Game.Statuses;
global using static Data.GamePlayer;
global using static Data.GamePlayer.Powers;
global using static Data.GamePlayer.Results;
global using static Data.Tournament.PowerGroups;
using System.Collections.Generic;
//
using System.Reflection;

namespace Data.Tests;

public abstract class TestBase
{
	protected sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	protected static CacheScope SeedCache(Action<object> fill)
	{
		var cacheType = typeof (Data).GetNestedType("Cache", NonPublic)
									 .OrThrow("Cache type not found");
		var field = cacheType.GetField("_data", NonPublic | Static)
							 .OrThrow("Cache._data field not found");
		var original = field.GetValue(null)
							.OrThrow();
		var typeMapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var typeMap = CreateInstance(typeMapType).OrThrow();
		fill(typeMap);
		field.SetValue(null, typeMap);
		return new (original, field);
	}

	protected static void SetField(object target, string field, object? value)
		=> target.GetType()
				 .GetField(field, Instance | NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	protected static void SetProperty(object target, string prop, object? value)
		=> target.GetType()
				 .GetProperty(prop, Instance | Public | NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	private static string GetPrimaryKey(object record)
		=> (string)record.GetType()
						 .GetProperty("PrimaryKey", Instance | Public)
						 .OrThrow($"PrimaryKey property not found on {record.GetType().Name}")
						 .GetValue(record)
						 .OrThrow();

	protected static void AddOne(object typeMap,
								 Type type,
								 object record)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		sortedDictType.GetMethod("Add")?.Invoke(sd, [GetPrimaryKey(record), record]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	protected static void AddMany(object typeMap,
								  Type type,
								  params object[] records)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		var sdAdd = sortedDictType.GetMethod("Add");
		foreach (var r in records)
			sdAdd?.Invoke(sd, [GetPrimaryKey(r), r]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	protected static void AddEmpty(object typeMap,
								   Type type)
	{
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		typeMapType.GetMethod("Add")
				   .OrThrow()
				   .Invoke(typeMap, [type, sd]);
	}

	protected static void AddEmpties(object typeMap)
	{
		var typeMapType = typeMap.GetType();
		var contains = typeMapType.GetMethod("ContainsKey")
								  .OrThrow();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var mapAdd = typeMapType.GetMethod("Add")
								.OrThrow();
		AddAnEmpty(typeof (Round));
		AddAnEmpty(typeof (Tournament));
		AddAnEmpty(typeof (ScoringSystem));
		AddAnEmpty(typeof (Game));
		AddAnEmpty(typeof (RoundPlayer));
		AddAnEmpty(typeof (TournamentPlayer));
		AddAnEmpty(typeof (Group));
		AddAnEmpty(typeof (GroupPlayer));
		// Do not add empty maps for Team, TeamPlayer, Player here because tests already add them
		AddAnEmpty(typeof (PlayerConflict));

		void AddAnEmpty(Type t)
		{
			if (!(bool)contains.Invoke(typeMap, [t])
							   .OrThrow())
				mapAdd.Invoke(typeMap, [t, CreateInstance(sortedDictType)]);
		}
	}

	protected static Game CreateGameWithSystem(ScoringSystem system,
											   Game.Statuses status)
	{
		var t = new Tournament { Id = 1, Name = "T" };
		var r = new Round { Id = 2, Number = 1 };
		SetProperty(r, "TournamentId", t.Id);
		SetField(r, "<Tournament>k__BackingField", t);
		return new ()
			   {
				   Id = 3,
				   Round = r,
				   Status = status,
				   ScoringSystem = system
			   };
	}

	protected static TeamPlayer NewTeamPlayerViaLoad(int teamId,
													 int playerId)
	{
		var tp = new TeamPlayer();
		var values = new Dictionary<string, object?>
					 {
						 ["TeamId"] = teamId,
						 ["PlayerId"] = playerId
					 };
		using var reader = new FakeDbDataReader("TeamPlayer", values);
		tp.Load(reader);
		return tp;
	}
}
