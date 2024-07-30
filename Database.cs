using System.Data.Odbc;
using System.Data.OleDb;
using Microsoft.Win32;

namespace DCM;

internal static class Database
{
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

    private static readonly string[] OfficeVersionFolders = [ "14.0", "16.0" ];

	private static DbConnection? _connection;

	private static DbTransaction? _transaction;

	private static bool _localTransaction;

	private static DbConnection Connection => _connection.OrThrow();

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

	internal static DbCommand Command(string? sql = null)
		=> Connection switch
		   {
			   OdbcConnection odbcConnection => new OdbcCommand(sql, odbcConnection, _transaction as OdbcTransaction),
			   OleDbConnection oleConnection => new OleDbCommand(sql, oleConnection, _transaction as OleDbTransaction),
			   _                             => throw new ()
		   };

	internal static void OpenConnection()
	{
		if (_transaction is null)
			Connection.Open();
	}

	internal static void CloseConnection()
	{
		if (_transaction is null)
			Connection.Close();
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

	internal static void BeginLocalTransaction()
    {
		_localTransaction = _transaction is null;
		if (_localTransaction)
			StartTransaction();
		else
			OpenConnection();
	}

	internal static void CommitLocalTransaction()
    {
		if (_localTransaction)
			EndTransaction();
		else
			CloseConnection();
	}

	internal static void Execute([InstantHandle] IEnumerable<string> statements)
		=> Execute([..statements]);

	internal static void Execute(params string[] statements)
	{
		using var command = Command();
		foreach (var statement in statements)
		{
			command.CommandText = statement;
			if (command.ExecuteNonQuery() is 0)
				throw new (); //	TODO
		}
	}

	internal static bool CheckAccessDriver()
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
}
