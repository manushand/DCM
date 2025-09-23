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
	private protected static readonly Type CacheType = typeof (Data).GetNestedType("Cache", NonPublic)
																	.OrThrow("Cache type not found");
	private protected static readonly FieldInfo DataField = CacheType.GetField("_data", NonPublic | Static)
																	 .OrThrow("Cache._data field not found");
	private protected static readonly FieldInfo StoresField = CacheType.GetField("Stores", NonPublic | Static)
																	   .OrThrow("Cache.Stores not found");

	private protected sealed record CacheScope(object Original, FieldInfo Field) : IDisposable
	{
		public void Dispose() => Field.SetValue(null, Original);
	}

	private protected static CacheScope SeedCache(Action<object> fill)
	{
		var original = DataField.GetValue(null)
								.OrThrow();
		var typeMapType = original.GetType(); // Dictionary<Type, SortedDictionary<string, IRecord>>
		var typeMap = CreateInstance(typeMapType).OrThrow();
		fill(typeMap);
		DataField.SetValue(null, typeMap);
		return new (original, DataField);
	}

	private protected static void SetField(object target,
										   string field,
										   object? value)
		=> target.GetType()
				 .GetField(field, Instance | NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	private protected static void SetProperty(object target,
											  string prop,
											  object? value)
		=> target.GetType()
				 .GetProperty(prop, Instance | Public | NonPublic)
				 .OrThrow()
				 .SetValue(target, value);

	//	TODO: These next three methods could be extensions on Data.CacheType
	private static string GetPrimaryKey(object record)
		=> (string)record.GetType()
						 .GetProperty("PrimaryKey", Instance | Public)
						 .OrThrow($"PrimaryKey property not found on {record.GetType().Name}")
						 .GetValue(record)
						 .OrThrow();

	private protected static void Add<T>(object typeMap,
										 params T[] records)
		where T : class, IRecord
	{
		var type = typeof (T);
		var typeMapType = typeMap.GetType();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var sd = CreateInstance(sortedDictType).OrThrow();
		var sdAdd = sortedDictType.GetMethod("Add");
		if (records.Length is 0)
			records = [(T)CreateInstance(type).OrThrow()];
		foreach (var r in records)
			sdAdd?.Invoke(sd, [GetPrimaryKey(r.OrThrow()), r]);
		typeMapType.GetMethod("Add")?.Invoke(typeMap, [type, sd]);
	}

	private protected static void AddEmpties(object typeMap)
	{
		var typeMapType = typeMap.GetType();
		var contains = typeMapType.GetMethod("ContainsKey")
								  .OrThrow();
		var sortedDictType = typeMapType.GetGenericArguments()[1];
		var mapAdd = typeMapType.GetMethod("Add")
								.OrThrow();
		AddAnEmpty<Round>();
		AddAnEmpty<Tournament>();
		AddAnEmpty<Tournament>();
		AddAnEmpty<Game>();
		AddAnEmpty<RoundPlayer>();
		AddAnEmpty<TournamentPlayer>();
		AddAnEmpty<Group>();
		AddAnEmpty<GroupPlayer>();
		// Do not add empty maps for Team, TeamPlayer, Player here because tests already add them
		AddAnEmpty<PlayerConflict>();

		void AddAnEmpty<T>()
				where T : class, IRecord
		{
			var t = typeof (T);
			if (!(bool)contains.Invoke(typeMap, [t])
							   .OrThrow())
				mapAdd.Invoke(typeMap, [t, CreateInstance(sortedDictType)]);
		}
	}

	private protected static Game CreateGameWithSystem(ScoringSystem system,
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

	private protected static TeamPlayer NewTeamPlayerViaLoad(int teamId,
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
