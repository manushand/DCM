using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Data.Tests.Helpers;

internal sealed class FakeDbConnection : DbConnection
{
	private readonly string _connectionString;
	private readonly Func<string, int> _executeNonQuery;
	private readonly Func<object?> _executeScalar;
	private readonly Func<CommandBehavior, DbDataReader> _executeReader;
	public readonly List<string> ExecutedSql = new();
	public bool OpenCalled { get; private set; }
	public bool CloseCalled { get; private set; }

	public FakeDbConnection(string connectionString,
		Func<string, int>? executeNonQuery = null,
		Func<object?>? executeScalar = null,
		Func<CommandBehavior, DbDataReader>? executeReader = null)
	{
		_connectionString = connectionString;
		_executeNonQuery = sql => (executeNonQuery?.Invoke(sql) ?? 1);
		_executeScalar = executeScalar ?? (() => 1);
		_executeReader = executeReader ?? (_ => new FakeEmptyReader());
	}

	public override string ConnectionString { get => _connectionString; set { } }
	public override string Database => "Fake";
	public override string DataSource => "Fake";
	public override string ServerVersion => "1.0";
	public override ConnectionState State => OpenCalled ? ConnectionState.Open : ConnectionState.Closed;

	protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => new FakeDbTransaction(this);
	public override void ChangeDatabase(string databaseName) { }
	public override void Close() { CloseCalled = true; }
	public override void Open() { OpenCalled = true; }
	protected override DbCommand CreateDbCommand() => new FakeDbCommand(this, ExecutedSql, _executeNonQuery, _executeScalar, _executeReader);
}

internal sealed class FakeDbTransaction : DbTransaction
{
	private readonly DbConnection _conn;
	public FakeDbTransaction(DbConnection conn) => _conn = conn;
	public override void Commit() { }
	protected override DbConnection DbConnection => _conn;
	public override IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;
	public override void Rollback() { }
}

internal sealed class FakeDbCommand : DbCommand
{
	private readonly FakeDbConnection _conn;
	private readonly List<string> _log;
	private readonly Func<string, int> _executeNonQuery;
	private readonly Func<object?> _executeScalar;
	private readonly Func<CommandBehavior, DbDataReader> _executeReader;
	public FakeDbCommand(FakeDbConnection conn, List<string> log,
		Func<string, int> execNonQuery,
		Func<object?> execScalar,
		Func<CommandBehavior, DbDataReader> execReader)
	{
		_conn = conn;
		_log = log;
		_executeNonQuery = execNonQuery;
		_executeScalar = execScalar;
		_executeReader = execReader;
	}

	public override string CommandText { get; set; } = string.Empty;
	public override int CommandTimeout { get; set; }
	public override CommandType CommandType { get; set; }
	public override UpdateRowSource UpdatedRowSource { get; set; }
	protected override DbConnection DbConnection { get => _conn; set { } }
	protected override DbParameterCollection DbParameterCollection { get; } = new FakeParameterCollection();
	protected override DbTransaction DbTransaction { get; set; } = null!;
	public override bool DesignTimeVisible { get; set; }
	public override void Cancel() { }
	public override int ExecuteNonQuery()
	{
		_log.Add(CommandText);
		return _executeNonQuery(CommandText);
	}
	public override object ExecuteScalar()
	{
		_log.Add(CommandText);
		return _executeScalar();
	}
	public override void Prepare() { }
	protected override DbParameter CreateDbParameter() => new FakeParameter();
	protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
	{
		_log.Add(CommandText);
		return _executeReader(behavior);
	}
}

internal sealed class FakeEmptyReader : DbDataReader
{
	public override bool Read() => false;
	public override int FieldCount => 0;
	public override object this[int ordinal] => throw new IndexOutOfRangeException();
	public override object this[string name] => throw new IndexOutOfRangeException();
	public override bool HasRows => false;
	public override bool IsClosed => false;
	public override int RecordsAffected => 0;
	public override int Depth => 0;
	public override bool NextResult() => false;
	public override int GetOrdinal(string name) => -1;
	public override object GetValue(int ordinal) => throw new IndexOutOfRangeException();
	public override bool IsDBNull(int ordinal) => true;
	public override string GetString(int ordinal) => string.Empty;
	public override string GetName(int ordinal) => string.Empty;
	public override Type GetFieldType(int ordinal) => typeof(object);
	public override System.Collections.IEnumerator GetEnumerator() => Array.Empty<int>().GetEnumerator();
	public override bool GetBoolean(int ordinal) => false;
	public override byte GetByte(int ordinal) => 0;
	public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => 0;
	public override char GetChar(int ordinal) => '\0';
	public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => 0;
	public override string GetDataTypeName(int ordinal) => "";
	public override DateTime GetDateTime(int ordinal) => default;
	public override decimal GetDecimal(int ordinal) => default;
	public override double GetDouble(int ordinal) => default;
	public override float GetFloat(int ordinal) => default;
	public override Guid GetGuid(int ordinal) => Guid.Empty;
	public override short GetInt16(int ordinal) => 0;
	public override int GetInt32(int ordinal) => 0;
	public override long GetInt64(int ordinal) => 0;
	public override int GetValues(object[] values) => 0;
}

internal sealed class FakeParameterCollection : DbParameterCollection
{
	private readonly List<DbParameter> _list = new();
	public override int Add(object value) { _list.Add((DbParameter)value); return _list.Count - 1; }
	public override void AddRange(Array values) { foreach (var v in values) Add(v!); }
	public override void Clear() => _list.Clear();
	public override bool Contains(object value) => _list.Contains((DbParameter)value);
	public override bool Contains(string value) => _list.Exists(p => p.ParameterName == value);
	public override void CopyTo(Array array, int index) => _list.ToArray().CopyTo(array, index);
	public override int Count => _list.Count;
	public override System.Collections.IEnumerator GetEnumerator() => _list.GetEnumerator();
	protected override DbParameter GetParameter(int index) => _list[index];
	protected override DbParameter GetParameter(string parameterName) => _list.Find(p => p.ParameterName == parameterName)!;
	public override int IndexOf(object value) => _list.IndexOf((DbParameter)value);
	public override int IndexOf(string parameterName) => _list.FindIndex(p => p.ParameterName == parameterName);
	public override void Insert(int index, object value) => _list.Insert(index, (DbParameter)value);
	public override bool IsFixedSize => false;
	public override bool IsReadOnly => false;
	public override bool IsSynchronized => false;
	public override void Remove(object value) => _list.Remove((DbParameter)value);
	public override void RemoveAt(int index) => _list.RemoveAt(index);
	public override void RemoveAt(string parameterName) => _list.RemoveAll(p => p.ParameterName == parameterName);
	protected override void SetParameter(int index, DbParameter value) => _list[index] = value;
	protected override void SetParameter(string parameterName, DbParameter value)
	{
		var idx = IndexOf(parameterName);
		if (idx >= 0) _list[idx] = value; else _list.Add(value);
	}
	public override object SyncRoot => this;
}

internal sealed class FakeParameter : DbParameter
{
	public override DbType DbType { get; set; }
	public override ParameterDirection Direction { get; set; }
	public override bool IsNullable { get; set; }
	public override string ParameterName { get; set; } = string.Empty;
	public override string SourceColumn { get; set; } = string.Empty;
	public override object? Value { get; set; }
	public override bool SourceColumnNullMapping { get; set; }
	public override int Size { get; set; }
	public override void ResetDbType() { }
}
