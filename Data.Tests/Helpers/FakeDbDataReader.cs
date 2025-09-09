using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Data.Tests.Helpers;

/// <inheritdoc />
/// <summary>
/// Minimal DbDataReader fake that supports the IDataRecord extension methods in Data.Data
/// and the CheckDataType validation. It exposes a single row of values.
/// </summary>
internal sealed class FakeDbDataReader : DbDataReader
{
	private readonly string _baseTableName;
	private readonly Dictionary<string, object?> _values;
	private readonly Dictionary<string, int> _ordinals;
	private readonly List<string> _columns;

	public FakeDbDataReader(string baseTableName, IDictionary<string, object?> values)
	{
		_baseTableName = baseTableName;
		_values = new (values, StringComparer.InvariantCultureIgnoreCase);
		_columns = _values.Keys.ToList();
		_ordinals = _columns
					.Select(static (name, index) => new { name, index })
					.ToDictionary(static x => x.name, static x => x.index, StringComparer.InvariantCultureIgnoreCase);
	}

	public override bool HasRows => true;
	public override int FieldCount => _columns.Count;
	public override bool IsClosed => false;
	public override int RecordsAffected => 1;
	public override int Depth => 0;

	// We don't iterate; consumers read current row values directly via Get* methods.
	public override bool Read() => false;
	public override bool NextResult() => false;
	public override object this[int ordinal] => GetValue(ordinal);
	public override object this[string name] => GetValue(GetOrdinal(name));

	public override string GetName(int ordinal) => _columns[ordinal];

	public override int GetOrdinal(string name)
		=> _ordinals.TryGetValue(name, out var ord) ? ord : throw new IndexOutOfRangeException(name);

	public override object GetValue(int ordinal)
	{
		var name = GetName(ordinal);
		return _values.TryGetValue(name, out var value) && value is not null ? value : DBNull.Value;
	}

	public override bool IsDBNull(int ordinal)
		=> GetValue(ordinal) is DBNull;

	public override string GetString(int ordinal) => (string)(GetValue(ordinal) is DBNull ? string.Empty : GetValue(ordinal));
	public override bool GetBoolean(int ordinal) => Convert.ToBoolean(GetValue(ordinal));
	public override int GetInt32(int ordinal) => Convert.ToInt32(GetValue(ordinal));
	public override double GetDouble(int ordinal) => Convert.ToDouble(GetValue(ordinal));
	public override decimal GetDecimal(int ordinal) => Convert.ToDecimal(GetValue(ordinal));
	public override DateTime GetDateTime(int ordinal) => (DateTime)GetValue(ordinal);
	public override byte GetByte(int ordinal) => Convert.ToByte(GetValue(ordinal));
	public override char GetChar(int ordinal) => Convert.ToChar(GetValue(ordinal));

	public override Type GetFieldType(int ordinal)
	{
		var value = GetValue(ordinal);
		return value is DBNull ? typeof (object) : value.GetType();
	}

	public override DataTable GetSchemaTable()
	{
		var table = new DataTable();
		table.Columns.Add(SchemaTableColumn.ColumnName, typeof (string));
		table.Columns.Add(SchemaTableColumn.ColumnOrdinal, typeof (int));
		table.Columns.Add(SchemaTableColumn.DataType, typeof (Type));
		table.Columns.Add(SchemaTableColumn.AllowDBNull, typeof (bool));
		table.Columns.Add(SchemaTableColumn.BaseTableName, typeof (string));
		for (var i = 0; i < _columns.Count; i++)
		{
			var row = table.NewRow();
			row[SchemaTableColumn.ColumnName] = _columns[i];
			row[SchemaTableColumn.ColumnOrdinal] = i;
			var value = _values[_columns[i]];
			row[SchemaTableColumn.DataType] = value?.GetType() ?? typeof (object);
			row[SchemaTableColumn.AllowDBNull] = value is null;
			row[SchemaTableColumn.BaseTableName] = _baseTableName;
			table.Rows.Add(row);
		}
		return table;
	}

	// Unused members for our tests
	public override int GetValues(object[] values)
	{
		for (var i = 0; i < values.Length && i < FieldCount; i++)
			values[i] = GetValue(i);
		return Math.Min(values.Length, FieldCount);
	}

	public override string GetDataTypeName(int ordinal) => GetFieldType(ordinal).Name;
	public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => throw new NotSupportedException();
	public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => throw new NotSupportedException();
	public override Guid GetGuid(int ordinal) => (Guid)GetValue(ordinal);
	public override short GetInt16(int ordinal) => Convert.ToInt16(GetValue(ordinal));
	public override long GetInt64(int ordinal) => Convert.ToInt64(GetValue(ordinal));
	public override float GetFloat(int ordinal) => Convert.ToSingle(GetValue(ordinal));
	public override System.Collections.IEnumerator GetEnumerator() => _columns.GetEnumerator();
}
