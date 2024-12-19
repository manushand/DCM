global using System.Data.Common;
global using JetBrains.Annotations;
global using static System.Reflection.BindingFlags;
global using static System.String;
//
global using DCM;
global using static DCM.DCM;
global using static DCM.DCM.PowerNames;
global using static Data.Data;
global using static Data.Game.Statuses;
global using static Data.GamePlayer.Results;
global using static Data.Tournament.PowerGroups;
//
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Reflection;
using Microsoft.Data.SqlClient;
using static System.Environment;
using static Microsoft.Win32.RegistryKey;
using static Microsoft.Win32.RegistryHive;
using static Microsoft.Win32.RegistryView;

namespace Data;

using static Tournament;

public static partial class Data
{
	private const string Null = nameof (Null);
	private static string _selectIdentityCommandText = Empty;

	public enum DatabaseTypes : sbyte
	{
		None = default,
		Access = 1,
		SqlServer = 2
	}

	public static bool ConnectToAccessDatabase(string databaseFile)
	{
		foreach (var provider in Providers)
			try
			{
				_connection = provider(databaseFile);
				OpenConnection();
				CloseConnection();
				_selectIdentityCommandText = "SELECT @@IDENTITY";
				return true;
			}
			catch // (Exception exception)
			{
				//	TODO: log?
			}
		return false;
	}

	public static void ConnectToSqlServerDatabase(string connectionString)
	{
		//	NOTE: This method may throw.  Don't worry; the exception is caught by the caller
		_connection = new SqlConnection(connectionString);
		OpenConnection();
		CloseConnection();
		_selectIdentityCommandText = "SELECT SCOPE_IDENTITY()";
	}

	internal static void StartTransaction()
	{
		if (_transaction is not null)
			throw new InvalidOperationException(); // TODO
		OpenConnection();
		_transaction = Connection.BeginTransaction();
	}

	internal static void EndTransaction()
	{
		_transaction.OrThrow()
					.Commit();
		_transaction = null;
		_localTransaction = false;
		CloseConnection();
	}

	public static void CheckDriver()
	{
		var baseKey = OpenBaseKey(LocalMachine,
								  Is64BitOperatingSystem
									  ? Registry64
									  : Registry32);
		if (RegistryKeyExists(Is64BitProcess))
			return;
		var mismatch = RegistryKeyExists(!Is64BitProcess);
		throw new ($"{(mismatch ? "The wrong" : "No")} database engine is installed for .accdb file support.{NewLine}" +
				   $"{(Is64BitProcess ? null : $"{NewLine}The DCM will work with .mdb files only.{NewLine}{NewLine}")}" +
				   $"If you wish it to work with .accdb files, {(mismatch ? "uninstall your Microsoft Access database engine then " : null)}" +
				   $"install the {(Is64BitProcess ? 64 : 32)} bit driver from https://www.microsoft.com/en-us/download/details.aspx?id=54920");

		bool RegistryKeyExists(bool is64Bit)
			=> OfficeVersionFolders.Any(version => baseKey.OpenSubKey($@"SOFTWARE\{(is64Bit ? null : @"Wow6432Node\")}Microsoft\Office\{version}\Access Connectivity Engine\InstallRoot") is not null);
	}

	public static bool Any<T>(Func<T, bool>? func = null)
		where T : IRecord
		=> Cache.Exists(func);

	public static T CreateOne<T>(T record)
		where T : class, IRecord
		=> CreateMany(record).Single();

	public static IEnumerable<T> CreateMany<T>(params T[] records)
		where T : IRecord
	{
		BeginLocalTransaction();
		using (var command = Command())
			foreach (var record in records)
			{
				command.CommandText = InsertStatement(record);
				if (command.ExecuteNonQuery() is 0)
					throw new ($"Record insertion failed: {command.CommandText}");
				if (record is not IdInfoRecord idInfoRecord)
					continue;
				command.CommandText = _selectIdentityCommandText;
				idInfoRecord.Id = (int)command.ExecuteScalar().OrThrow();
			}
		CommitLocalTransaction();
		Cache.AddRange(records);
		return records;

		static string InsertStatement(T record)
		{
			var assignments = record is IInfoRecord infoRecord // 10 record types (7 that are not also IInfoRecord)
								  ? infoRecord.FieldValues
											  .Split(FieldValuesLineSplitter)
											  .ToList()
								  : [];
			if (record is LinkRecord linkRecord) //	6 types, including 3 that are also IInfoRecord
				assignments.AddRange(linkRecord.KeyFieldAssignments);
			var list = assignments.Distinct()
								  .Select(static assignment => assignment.Split('=', 2))
								  .ToArray();
			return $"""
			        INSERT INTO {TableName<T>()} ({Join(Comma, list.Select(static a => a[0]))})
			        VALUES ({Join(Comma, list.Select(static a => a[1]))});
			        """;
		}
	}

	internal static IEnumerable<T> CreateMany<T>(IEnumerable<T> records)
		where T : IRecord
		=> CreateMany([..records]);

	public static T? ReadOne<T>(Func<T, bool> func)
		where T : IRecord
		=> Cache.FetchOne(func);

	public static T? ReadById<T>(int id)
		where T : IdInfoRecord
		=> ReadOne<T>(record => record.Id == id);

	public static T? ReadByName<T>(T record)
		where T : IdInfoRecord
		=> ReadOne<T>(t => t.Name.Matches(record.Name));

	//	Important (for some reason): note that in all cases where we open and close the
	//	database connection, we wait until the connection is closed to update the cache.

	public static T? ReadOne<T>(T record,
								bool fromCache = true)
		where T : class, IRecord, new()
	{
		//	Load Cache for this type if not yet loaded
		if (fromCache || !Cache.ContainsKey<T>())
			return Cache.FetchOne(record);
		var result = Read(record).SingleOrDefault();
		if (result is not null)
			Cache.Add(result);
		return result;
	}

	public static IEnumerable<T> ReadAll<T>()
		where T : IRecord
		=> Cache.FetchAll<T>();

	public static IEnumerable<T> ReadMany<T>(Func<T, bool> func)
		where T : IRecord
		=> Cache.FetchMany(func);

	public static void UpdateOne<T>(T record,
									string? formerPrimaryKey = null)
		where T : IInfoRecord
	{
		OpenConnection();
		var primaryKey = formerPrimaryKey ?? record.PrimaryKey;
		Execute(UpdateStatement(primaryKey, record));
		CloseConnection();
		if (formerPrimaryKey is not null)
			Cache.Remove<T>(formerPrimaryKey);
		Cache.Add(record);
	}

	//	This method cannot be used to change primary key fields on any of the records involved
	public static void UpdateMany<T>(params T[] records)
		where T : IInfoRecord
	{
		BeginLocalTransaction();
		Execute(records.Select(UpdateStatement));
		CommitLocalTransaction();
		Cache.AddRange(records);
	}

	public static void UpdateMany<T>(IEnumerable<T> records)
		where T : IInfoRecord
		=> UpdateMany([..records]);

	public static void Delete<T>(params T[] records)
		where T : IRecord
	{
		BeginLocalTransaction();
		Execute(records.Select(static record => $"{DeleteStatement<T>()}{WhereClause(record)}"));
		CommitLocalTransaction();
		Cache.Remove(records);
	}

	public static void Delete<T>(IEnumerable<T> records)
		where T : IRecord
		=> Delete([..records]);

	public static void Delete<T>(Func<T, bool> func)
		where T : IRecord
		=> Delete(Cache.FetchMany(func));

	public static void Clear()
	{
		StartTransaction();
		DeleteAll<GamePlayer>();
		DeleteAll<Game>();
		DeleteAll<RoundPlayer>();
		DeleteAll<Round>();
		DeleteAll<TeamPlayer>();
		DeleteAll<Team>();
		DeleteAll<GroupPlayer>();
		DeleteAll<Group>();
		DeleteAll<TournamentPlayer>();
		DeleteAll<Tournament>();
		DeleteAll<PlayerConflict>();
		DeleteAll<Player>();
		DeleteAll<ScoringSystem>();
		EndTransaction();
		FlushCache();
	}

	public static void FlushCache()
		=> Cache.Flush();

	public static IEnumerable<Player> Sorted(this IEnumerable<Player> players,
											 bool byLastName = false)
		=> players.OrderBy(player => byLastName ? player.LastFirst : player.Name);

	[LinqTunnel]
	public static IEnumerable<T2> SelectSorted<T1, T2>(this IEnumerable<T1> items,
													   Func<T1, T2> func)
		where T1 : IRecord
		where T2 : IComparable<T2>
		=> items.Select(func)
				.Order();

	public static IEnumerable<int> Ids<T>(this IEnumerable<IdentityRecord<T>> records)
		where T : class, new()
		=> records.Select(static record => record.Id);

	internal static IEnumerable<T> WithPlayerId<T>(this IEnumerable<T> linkRecords,
												   int playerId)
		where T : LinkRecord
		=> linkRecords.Where(linkRecord => linkRecord.PlayerId == playerId);

	public static bool HasPlayerId<T>(this IEnumerable<T> linkRecords,
									  int playerId)
		where T : LinkRecord
		=> linkRecords.Any(linkRecord => linkRecord.PlayerId == playerId);

	public static T ByPlayerId<T>(this IEnumerable<T> linkRecords,
								  int playerId)
		where T : LinkRecord
		=> linkRecords.Single(linkRecord => linkRecord.PlayerId == playerId);

	internal static void CheckDataType<T>(this DbDataReader reader)
		where T : IRecord
	{
		if (reader.GetSchemaTable()?
				  .Rows
				  .Cast<DataRow>()
				  .Any(static row => $"{row[nameof (SchemaTableColumn.BaseTableName)]}" != typeof (T).Name) ?? true)
			throw new ArgumentException($"reader has no SchemaTable or is not reading Type {typeof (T).Name}", nameof (reader)); //	TODO
	}

	internal static string ForSql(this string? text)
		=> text?.Length > 0
			   ? $"'{text.Replace("'", "''")}'"
			   : Null;

	internal static string ForSql(this int? value)
		=> value?.ToString() ?? Null;

	internal static int ForSql<T>(this T value)
		where T : Enum
		=> value.AsInteger();

	internal static string ForSql(this DateTime value)
		=> $"'{value:d}'";

	internal static string ForSql(this DateTime? value)
		=> value?.ForSql()
		?? Null;

	internal static int ForSql(this bool value)
		=> value.AsInteger();

	internal static bool Boolean(this IDataRecord record,
								 string columnName)
		=> record.GetBoolean(record.GetOrdinal(columnName));

	internal static string String(this IDataRecord record,
								  string columnName)
	{
		var ordinal = record.GetOrdinal(columnName);
		return record.IsDBNull(ordinal)
				   ? Empty
				   : record.GetString(ordinal);
	}

	internal static double Double(this IDataRecord record,
								  string columnName)
		=> record.GetDouble(record.GetOrdinal(columnName));

	internal static decimal Decimal(this IDataRecord record,
									string columnName)
		=> (decimal)record.GetDouble(record.GetOrdinal(columnName));

	internal static int Integer(this IDataRecord record,
								string columnName)
		=> record.GetInt32(record.GetOrdinal(columnName));

	internal static int? NullableInteger(this IDataRecord record,
										 string columnName)
	{
		var column = record.GetOrdinal(columnName);
		return record.IsDBNull(column)
				   ? null
				   : record.GetInt32(column);
	}

	internal static T IntegerAs<T>(this IDataRecord record,
								   string columnName)
		where T : Enum
		=> record.Integer(columnName)
				 .As<T>();

	internal static DateTime? NullableDate(this IDataRecord record,
										   string columnName)
	{
		var ordinal = record.GetOrdinal(columnName);
		return record.IsDBNull(ordinal)
				   ? null
				   : record.GetDateTime(ordinal);
	}

	internal static bool GroupSharedBy(this PowerGroups groups, PowerNames power1, PowerNames power2)
	{
		var groupings = (typeof (PowerGroups).GetField(groups.ToString())
											 ?.GetCustomAttribute(typeof (PowerGroupingsAttribute)) as PowerGroupingsAttribute)
											 ?.Groups
						?? throw new InvalidOperationException("Missing PowerGroupings attribute");
		return groupings.Single(group => group.Contains(power1.Abbreviation())) == groupings.Single(group => group.Contains(power2.Abbreviation()));
	}

	#region Private fields, property, and methods

	private static readonly Func<string, DbConnection>[] Providers =
	[
		//	For ODBC either (32 or 64-bit) of the versions of the Access drivers must be
		//	installed from https://www.microsoft.com/en-us/download/details.aspx?id=13255
		//	and the solution's Build "Prefer 32-bit" must be set to match which driver was
		//	installed. These drivers work for .accdb and .mdb.
		static file => new OdbcConnection($"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};Uid=Admin;Pwd=;Dbq={file}"),
		/*
		//	The OLE ACE provider commented out below has the exact same requirements for
		//	success as the ODBC driver, so there's no point at all in using it.
		file => new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=False;Data Source=" + file),
		*/
		//	For OLE JET neither Access driver is needed but the solution's Build "Prefer 32-bit"
		//	property must be set to yes.  This driver works for .mdb only.
		static file => new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;User Id=admin;Data Source={file}")
	];

	private static readonly string[] OfficeVersionFolders = ["14.0", "16.0"];
	private static readonly string FieldValuesLineSplitter = $"{Comma}{NewLine}";

	private static DbConnection? _connection;
	private static DbTransaction? _transaction;
	private static bool _localTransaction;

	private static DbConnection Connection => _connection.OrThrow();

	private static DbCommand Command(string? sql = null)
		=> Connection switch
		   {
			   OdbcConnection odbcConnection => new OdbcCommand(sql, odbcConnection, _transaction as OdbcTransaction),
			   OleDbConnection oleConnection => new OleDbCommand(sql, oleConnection, _transaction as OleDbTransaction),
			   SqlConnection sqlConnection   => new SqlCommand(sql, sqlConnection, _transaction as SqlTransaction),
			   _                             => throw new ()
		   };

	private static void OpenConnection()
	{
		if (_transaction is null)
			Connection.Open();
	}

	private static void CloseConnection()
	{
		if (_transaction is null)
			Connection.Close();
	}

	private static void BeginLocalTransaction()
	{
		_localTransaction = _transaction is null;
		if (_localTransaction)
			StartTransaction();
		else
			OpenConnection();
	}

	private static void CommitLocalTransaction()
	{
		if (_localTransaction)
			EndTransaction();
		else
			CloseConnection();
	}

	private static void Execute([InstantHandle] IEnumerable<string> statements)
		=> Execute([..statements]);

	private static void Execute(params string[] statements)
	{
		using var command = Command();
		foreach (var statement in statements)
		{
			command.CommandText = statement;
			if (command.ExecuteNonQuery() is 0)
				throw new (); //	TODO
		}
	}

	private static string TableName<T>()
		where T : IRecord
		=> $"[{typeof (T).Name}]";

	//	It is up to the caller to Add any returned record(s) to the Cache
	private static List<T> Read<T>(T? record = null)
		where T : class, IRecord, new()
	{
		OpenConnection();
		var records = new List<T>();
		using (var command = Command($"SELECT * FROM {TableName<T>()}{(record is null ? null : WhereClause(record))}"))
		{
			using var reader = command.ExecuteReader(CommandBehavior.KeyInfo);
			while (reader.Read())
				records.Add((T)new T().Load(reader));
		}
		CloseConnection();
		return records;
	}

	private static string UpdateStatement<T>(T record)
		where T : IInfoRecord
		=> UpdateStatement(record.PrimaryKey, record);

	private static string UpdateStatement<T>(string currentPrimaryKey,
											 T record)
		where T : IInfoRecord
		=> $"UPDATE {TableName<T>()} SET {record.FieldValues}{WhereClause(currentPrimaryKey)}";

	private static string DeleteStatement<T>()
		where T : IRecord
		=> $"DELETE FROM {TableName<T>()}";

	private static string WhereClause(IRecord record)
		=> WhereClause(record.PrimaryKey);

	private static string WhereClause(string primaryKey)
		=> $" WHERE {primaryKey}";

	private static void DeleteAll<T>()
		where T : IRecord
		=> Execute(DeleteStatement<T>());

	#endregion
}
