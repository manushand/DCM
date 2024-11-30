namespace DCM.DB;

using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Win32;

internal static partial class Database
{
	internal static bool Connect(string databaseFile)
	{
		foreach (var provider in Providers)
			try
			{
				_connection = provider(databaseFile);
				OpenConnection();
				CloseConnection();
				return true;
			}
			catch //	(Exception exception)
			{
				//  MessageBox.Show(exception.Message);
				//	TODO: log?
			}

		MessageBox.Show("Failed to connect to database file.",
						"Database Connection Failed");
		return false;
	}

	internal static void StartTransaction()
	{
		if (_transaction is not null)
			throw new InvalidOperationException(); //	TODO
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

	internal static bool CheckDriver()
	{
		var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
											  Is64BitOperatingSystem
												  ? RegistryView.Registry64
												  : RegistryView.Registry32);
		if (RegistryKeyExists(Is64BitProcess))
			return true;
		var mismatch = RegistryKeyExists(!Is64BitProcess);
		MessageBox.Show($"{(mismatch ? "The wrong" : "No")} database engine is installed for .accdb file support.{NewLine}" +
						$"{(Is64BitProcess ? null : $"{NewLine}The DCM will work with .mdb files only.{NewLine}{NewLine}")}" +
						$"If you wish it to work with .accdb files, {(mismatch ? "uninstall your Microsoft Access database engine then " : null)}" +
						$"install the {(Is64BitProcess ? 64 : 32)} bit driver from https://www.microsoft.com/en-us/download/details.aspx?id=54920",
						"Driver Missing", OK, Warning);
		return false;

		bool RegistryKeyExists(bool is64Bit)
			=> OfficeVersionFolders.Any(version => baseKey.OpenSubKey($@"SOFTWARE\{(is64Bit ? null : @"Wow6432Node\")}Microsoft\Office\{version}\Access Connectivity Engine\InstallRoot") is not null);
	}

	internal static bool Any<T>(Func<T, bool>? func = null)
		where T : IRecord
		=> Cache.Exists(func);

	internal static T CreateOne<T>(T record)
		where T : class, IRecord
		=> CreateMany(record).Single();

	internal static IEnumerable<T> CreateMany<T>(params T[] records)
		where T : IRecord
	{
		BeginLocalTransaction();
		using (var command = Command())
			foreach (var record in records)
			{
				command.CommandText = InsertStatement(record);
				if (command.ExecuteNonQuery() is 0)
					throw new (); //	TODO
				if (record is not IdentityRecord identityRecord)
					continue;
				command.CommandText = "SELECT @@Identity";
				identityRecord.Id = (int)command.ExecuteScalar().OrThrow();
			}

		CommitLocalTransaction();
		Cache.AddRange(records);
		return records;

		static string InsertStatement(T record)
		{
			var assignments = record is IInfoRecord infoRecord //	10 record types (7 that are not also IInfoRecord)
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

	internal static T? ReadOne<T>(Func<T, bool> func)
		where T : IRecord
		=> Cache.FetchOne(func);

	internal static T? ReadById<T>(int id)
		where T : IdentityRecord
		=> ReadOne<T>(record => record.Id == id);

	internal static T? ReadByName<T>(T record)
		where T : IdentityRecord
		=> ReadOne<T>(t => t.Name.Matches(record.Name));

	//	Important (for some reason): note that in all cases where we open and close the
	//	database connection, we wait until the connection is closed to update the cache.

	internal static T? ReadOne<T>(T record,
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

	internal static IEnumerable<T> ReadAll<T>()
		where T : class, IRecord, new()
		=> Cache.FetchAll<T>();

	internal static IEnumerable<T> ReadMany<T>(Func<T, bool> func)
		where T : IRecord
		=> Cache.FetchMany(func);

	internal static void UpdateOne<T>(T record,
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
	internal static void UpdateMany<T>(params T[] records)
		where T : IInfoRecord
	{
		BeginLocalTransaction();
		Execute(records.Select(UpdateStatement));
		CommitLocalTransaction();
		Cache.AddRange(records);
	}

	internal static void UpdateMany<T>(IEnumerable<T> records)
		where T : IInfoRecord
		=> UpdateMany([..records]);

	internal static void Delete<T>(params T[] records)
		where T : IRecord
	{
		BeginLocalTransaction();
		Execute(records.Select(static record => $"{DeleteStatement<T>()}{WhereClause(record)}"));
		CommitLocalTransaction();
		Cache.Remove(records);
	}

	internal static void Delete<T>(IEnumerable<T> records)
		where T : IRecord
		=> Delete([..records]);

	internal static void Delete<T>(Func<T, bool> func)
		where T : IRecord
		=> Delete(Cache.FetchMany(func));

	internal static void Clear()
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

	internal static void FlushCache()
		=> Cache.Flush();

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
