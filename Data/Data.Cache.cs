using System.Reflection;

namespace Data;

using CacheType = Dictionary<Type, SortedDictionary<string, IRecord>>;

public static partial class Data
{
	private static class Cache
	{
		#region Public interface

		#region Methods

		internal static void Restore(string store)
			=> _data = Stores.GetOrSet(store, static _ => []);

		internal static void Flush()
			=> _data.Clear();

		internal static bool ContainsKey<T>()
			where T : IRecord
			=> _data.ContainsKey(typeof (T));

		internal static void Add<T>(T record)
			where T : IRecord
			=> Get<T>()[record.PrimaryKey] = record;

		internal static void AddRange<T>(IEnumerable<T> records)
			where T : IRecord
			=> records.ForEach(Add);

		internal static void Remove<T>(string key)
			where T : IRecord
			=> Get<T>().Remove(key);

		internal static void Remove<T>(params T[] records)
			where T : IRecord
			=> records.ForEach(static record => Remove<T>(record.PrimaryKey));

		internal static bool Exists<T>(Func<T, bool>? func = null)
			where T : IRecord
			=> FetchAll<T>().Any(record => func?.Invoke(record) is not false);

		internal static IEnumerable<T> FetchAll<T>()
			where T : IRecord
			=> Get<T>().Values
					   .Cast<T>(); // Adding .ToArray() here slows seeding down a lot

		internal static T? FetchOne<T>(Func<T, bool> func)
			where T : IRecord
			=> FetchAll<T>().SingleOrDefault(func);

		internal static T? FetchOne<T>(T record)
			where T : class, IRecord
			=> Get<T>().TryGetValue(record.PrimaryKey, out var iRecord)
				   ? (T)iRecord
				   : null;

		internal static IEnumerable<T> FetchMany<T>(Func<T, bool> func)
			where T : IRecord
			=> FetchAll<T>().Where(func);

		#endregion

		#endregion

		#region Private implementation

		#region Data

		private static CacheType _data = [];
		private static readonly Dictionary<string, CacheType> Stores = [];
		private static readonly Dictionary<Type[], MethodInfo> LoadMethods = [];
		private static readonly MethodInfo LoadMethod = typeof (Cache).GetMethod(nameof (Load), Static | NonPublic)
																	  .OrThrow();

		#endregion

		#region Methods

		private static SortedDictionary<string, IRecord> Get<T>()
			where T : IRecord
		{
			var type = typeof (T);
			if (!ContainsKey<T>())
				LoadMethods.GetOrSet([type], LoadMethod.MakeGenericMethod)
						   .Invoke(null, null);
			return _data[type];
		}

		private static void Load<T>()
			where T : class, IRecord, new()
			=> _data[typeof (T)] = new (Read<T>().Cast<IRecord>()
												 .ToDictionary(static record => record.PrimaryKey));

		#endregion

		#endregion
	}
}
