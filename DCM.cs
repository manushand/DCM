global using System.Collections;
global using System.ComponentModel;
global using System.Data.Common;
global using System.Net.Mail;
global using System.Reflection;
global using System.Text.RegularExpressions;
global using static System.Environment;
global using static System.String;
global using static System.Windows.Forms.MessageBoxButtons;
global using static System.Windows.Forms.MessageBoxIcon;
global using static System.Windows.Forms.DataGridViewContentAlignment;
global using JetBrains.Annotations;
global using DCM.DB;
global using DCM.UI.Forms;
global using static DCM.DCM;
global using static DCM.DB.Game;
global using static DCM.DB.Game.Statuses;
global using static DCM.DB.GamePlayer;
global using static DCM.DB.GamePlayer.Results;
global using static DCM.Scoring;
global using static DCM.Scoring.PowerNames;
global using Group = DCM.DB.Group;
//
using System.Data;
using DGVPrinterHelper;
using static System.Threading.Tasks.Task;
using static DGVPrinterHelper.DGVPrinter;

namespace DCM;

internal static partial class DCM
{
	#region Fields and Properties

	internal const string Version = "23.2.16";

	//	TODO: Don't the French end in 1905 or 1906?
	internal const int EarliestFinalGameYear = 1907; //	TODO: could be private to ScoringSystemInfoForm
	internal const int LatestFinalGameYear = 1918; //	== 12 options
	internal const char Comma = ',';
	internal const char Colon = ':';
	internal const char Semicolon = ';';

	internal static readonly SortedDictionary<PowerNames, DataGridViewCellStyle> PowerColors =
		//	TODO: All these should be made Bold here (instead of wherever it's happening)
		new ()
		{
			[Austria] = new ()
						{
							BackColor = Color.Red,
							ForeColor = Color.White
						},
			[England] = new ()
						{
							BackColor = Color.RoyalBlue,
							ForeColor = Color.White
						},
			[France] = new ()
					   {
						   BackColor = Color.SkyBlue,
						   ForeColor = Color.Black
					   },
			[Germany] = new ()
						{
							BackColor = Color.Black,
							ForeColor = Color.White
						},
			[Italy] = new ()
					  {
						  BackColor = Color.Lime,
						  ForeColor = Color.Black
					  },
			[Russia] = new ()
					   {
						   BackColor = Color.White,
						   ForeColor = Color.Black
					   },
			[Turkey] = new ()
					   {
						   BackColor = Color.Yellow,
						   ForeColor = Color.Black
					   },
			[TBD] = new ()
					{
						BackColor = SystemColors.Control,
						ForeColor = SystemColors.WindowText
					}
		};

	internal static readonly Properties.Settings Settings = Properties.Settings.Default;
	internal static readonly Dictionary<Font, Font> BoldFonts = [];
	internal static bool SkipHandlers;

	private const string Null = nameof (Null);
	private static readonly Random Random = new ();
	private static readonly char[] EmailSplitter = [Comma, Semicolon];
	private static readonly object[] EmptyEventArgs = [EventArgs.Empty];
	private static readonly string[] DatabaseFileExtensions = [".dcm", ".mdb", ".accdb"];
	private static readonly string[] SmtpHostNotSet = ["SMTP (email) host is not set."];
	private static readonly string Bullet = $"{NewLine}    • ";
	private static readonly string FileExtensionPatterns = Join(';', DatabaseFileExtensions.Select(static ext => $"*{ext}"));
	private static readonly string DatabaseFileDialogFilter = $"DCM Data File ({FileExtensionPatterns})|{FileExtensionPatterns}";

	#endregion

	#region Methods

	#region Database connection and CRUD methods

	internal static bool OpenDatabase(string? dbFileName = null)
	{
		while (dbFileName is null || !Database.Connect(dbFileName))
		{
			using var dialog = new OpenFileDialog();
			dialog.Title = "Choose DCM Data File";
			dialog.Filter = DatabaseFileDialogFilter;
			if (dialog.ShowDialog() is not DialogResult.OK)
				return false;
			dbFileName = dialog.FileName;
		}

		if (dbFileName == Settings.DatabaseFile)
			return true;
		Cache.Flush();
		Settings.DatabaseFile = dbFileName;
		SetEvent();
		return true;
	}

	internal static string? GetDatabaseFile()
	{
		//	If a db file has been saved in Settings, use that.
		if (File.Exists(Settings.DatabaseFile))
			return Settings.DatabaseFile;

		//	Otherwise, if there is one and only one file in
		//	the CurrentDirectory with one of the extensions
		//	we like, use it. Otherwise, make the user surf.
		var files = Directory.GetFiles(Directory.GetCurrentDirectory())
							 .Where(static name => DatabaseFileExtensions.Contains(Path.GetExtension(name)))
							 .ToArray();
		return files.Length is 1
				   ? files[0]
				   : null;
	}

	internal static bool CheckDatabaseDriver()
		=> Database.CheckAccessDriver();

	internal static void SaveDatabase()
	{
		using var dialog = new SaveFileDialog();
		dialog.Title = "Save DCM Data File As…";
		dialog.Filter = DatabaseFileDialogFilter;
		dialog.RestoreDirectory = true;
		if (dialog.ShowDialog() is not DialogResult.OK)
			return;
		File.Copy(Settings.DatabaseFile, dialog.FileName);
		Settings.DatabaseFile = dialog.FileName;
		Settings.Save();
	}

	internal static void BeginTransaction()
		=> Database.StartTransaction();

	internal static void CommitTransaction()
		=> Database.EndTransaction();

	internal static void ClearDatabase()
	{
		BeginTransaction();
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
		CommitTransaction();
		Cache.Flush();
		if (Settings.EventId > 0)
			SetEvent();
	}

	internal static void SetEvent(IdentityRecord? identityRecord = null)
	{
		Settings.EventId = identityRecord?.Id ?? 0;
		Settings.EventIsGroup = identityRecord is Group;
		Settings.Save();
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
		Database.BeginLocalTransaction();
		using (var command = Database.Command())
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
		Database.CommitLocalTransaction();
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
			        VALUES ({Join(Comma, list.Select(static a => a[1]))})
			        """;
		}
	}

	internal static IEnumerable<T> CreateMany<T>(IEnumerable<T> records)
		where T : IRecord
		=> CreateMany([..records]);

    //	It is up to the caller to Add any returned record(s) to the Cache
    internal static List<T> Read<T>(T? record = null)
        where T : class, IRecord, new()
    {
        Database.OpenConnection();
        var records = new List<T>();
        using (var command = Database.Command($"SELECT * FROM {TableName<T>()}{(record is null ? null : WhereClause(record))}"))
        {
            using var reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            while (reader.Read())
                records.Add((T)new T().Load(reader));
        }
        Database.CloseConnection();
        return records;
    }

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
		Database.OpenConnection();
		var primaryKey = formerPrimaryKey ?? record.PrimaryKey;
		Database.Execute(UpdateStatement(primaryKey, record));
		Database.CloseConnection();
		if (formerPrimaryKey is not null)
			Cache.Remove<T>(formerPrimaryKey);
		Cache.Add(record);
	}

	//	This method cannot be used to change primary key fields on any of the records involved
	internal static void UpdateMany<T>(params T[] records)
		where T : IInfoRecord
	{
		Database.BeginLocalTransaction();
		Database.Execute(records.Select(UpdateStatement));
		Database.CommitLocalTransaction();
		Cache.AddRange(records);
	}

	internal static void UpdateMany<T>(IEnumerable<T> records)
		where T : IInfoRecord
		=> UpdateMany([..records]);

	internal static void Delete<T>(params T[] records)
		where T : IRecord
	{
		Database.BeginLocalTransaction();
		Database.Execute(records.Select(static record => $"{DeleteStatement<T>()}{WhereClause(record)}"));
		Database.CommitLocalTransaction();
		Cache.Remove(records);
	}

	internal static void Delete<T>(IEnumerable<T> records)
		where T : IRecord
		=> Delete([..records]);

	internal static void Delete<T>(Func<T, bool> func)
		where T : IRecord
		=> Delete(Cache.FetchMany(func));

    #region Private database properties and methods

	private static readonly string FieldValuesLineSplitter = $"{Comma}{NewLine}";

	private static string TableName<T>()
		where T : IRecord
		=> $"[{typeof (T).Name}]";

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
        => Database.Execute(DeleteStatement<T>());

	#endregion

	#endregion

	#region Extension methods

	internal static T OrThrow<T>(this T? nullable,
								 string? message = null)
		where T : struct
		=> nullable ?? throw new InvalidOperationException(message);

	internal static T OrThrow<T>(this T? nullable,
								 string? message = null)
		where T : class
		=> nullable ?? throw new InvalidOperationException(message);

	internal static void ForEach<T>([InstantHandle] this IEnumerable<T> collection,
									Action<T> action)
		=> collection.ToList().ForEach(action);

	internal static void ForSome<T>([InstantHandle] this IEnumerable<T> collection,
									Func<T, bool> func,
									Action<T> action)
		=> collection.Where(func).ForEach(action);

	internal static IEnumerable<int> Ids(this IEnumerable<IdentityRecord> records)
		=> records.Select(static record => record.Id);

	internal static char Abbreviation(this PowerNames powerName)
		=> $"{powerName}".First();

	internal static string InCaps(this PowerNames powerName)
		=> $"{powerName}".ToUpper();

	internal static void Apply<T>(this IEnumerable<T> items,
								  Action<T, int> func)
		where T : class
		=> items.Select(static (item, index) => (item, index))
				.ForEach(tuple => func(tuple.item, tuple.index));

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

	internal static int ForSql(this bool value)
		=> value.AsInteger();

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

	internal static int AsInteger(this bool value)
		=> Convert.ToInt32(value);

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

	internal static decimal AsDecimal(this string value)
		=> decimal.Parse(value);

	internal static int? AsNullableInteger(this string value)
		=> value.Length is 0
			   ? null
			   : value.AsInteger();

	internal static int AsInteger(this string value)
		=> int.Parse(value);

	internal static int AsInteger<T>(this T value)
		where T : Enum
		=> (int)Convert.ChangeType(value, typeof (int));

	internal static T As<T>(this int value)
		where T : Enum
		=> (T)Enum.ToObject(typeof (T), value);

	internal static string Dotted(this int value)
		=> $"{value}.";

    internal static int NegatedIf(this int value, bool negator)
        => negator ? -value : value;

	internal static string[] SplitEmailAddresses(this string addresses)
		=> [..addresses.Split(EmailSplitter)
					   .Select(static email => email.Trim())
					   .Where(static email => email.Length is not 0)];

    //	TODO: there's a lot of debate about what the best way to validate an email address is
    //	TODO: I have usually used try { new MailAddress(text); } but it likes things I don't.
    [GeneratedRegex(@"^(" +
                    @"[\dA-Z]" +              //	Start with a digit or alphabetical
                    @"([\+\-_\.][\dA-Z]+)*" + //	No continuous or ending +-_. chars in email
                    @")+" +
                    @"@(([\dA-Z][-\w]*[\dA-Z]*\.)+[\dA-Z]{2,17})$",
        RegexOptions.IgnoreCase)]
    private static partial Regex EmailAddressFormat();

	internal static bool IsValidEmail(this string text)
        => EmailAddressFormat().IsMatch(text.Trim());

    internal static T As<T>(this string text)
		where T : Enum
		=> (T)Enum.Parse(typeof (T), text, true);

	internal static bool Starts(this string text,
								string start,
								bool symbol = false)
		=> text.StartsWith(start, StringComparison.InvariantCultureIgnoreCase)
		&& (symbol || text.Length == start.Length || !char.IsLetterOrDigit(text[start.Length]));

	internal static bool Matches(this string text,
								 string other)
		=> text.Equals(other, StringComparison.InvariantCultureIgnoreCase);

	internal static string Pluralize<T>(this string what,
										IEnumerable<T> items,
										bool provideCount = false)
		=> what.Pluralize(items.Count(), provideCount);

	internal static string Pluralize(this string what,
									 int count,
									 bool provideCount = false)
		=> $"{(provideCount ? $"{count} " : null)}{what}{(count is 1 ? null : 's')}";

	internal static void FillWith<T>(this List<T> list,
									 IEnumerable<T> items)
	{
		list.Clear();
		list.AddRange(items);
	}

	internal static IEnumerable<Player> Sorted(this IEnumerable<Player> players,
											   bool byLastName = false)
		=> players.OrderBy(player => byLastName ? player.LastFirst : player.Name);

	internal static void FillRange(this ComboBox comboBox,
								   int start,
								   int end)
		=> comboBox.FillWith(Range(start, ++end - start).Cast<object>());

	internal static void FillWithSortedPlayers(this ListControl listControl,
											   bool byLastName)
		=> listControl.FillWith(ReadAll<Player>().Sorted(byLastName));

	internal static void FillWithSortedPlayers(this ListControl listControl,
											   Func<Player, bool> func)
		=> listControl.FillWith(ReadMany(func).Sorted());

	internal static void FillWithSorted<T>(this ListControl listControl)
		where T : IdentityRecord, new()
		=> listControl.FillWith(ReadAll<T>().Order());

	internal static void FillWithRecords<T>(this ListControl listControl,
											IEnumerable<T> items)
		where T : IRecord
		=> listControl.FillWith((IEnumerable<object>)items);

	internal static void FillWith(this ListControl listControl,
								  IEnumerable<object> items)
		=> listControl.FillWith([..items]);

	internal static void FillWith(this ListControl listControl,
								  params object[] elements)
	{
		switch (listControl)
		{
		case ListBox listBox:
			listBox.Items
				   .Clear();
			listBox.Items
				   .AddRange(elements);
			return;
		case ComboBox comboBox:
			comboBox.Items
					.Clear();
			comboBox.Items
					.AddRange(elements);
			return;
		}
	}

	internal static void SetSelectedItem<T>(this ComboBox comboBox,
											T? record)
		where T : IdentityRecord
		=> comboBox.SelectedItem = comboBox.Items
										   .Cast<T>()
										   .SingleOrDefault(t => t.Id == (record?.Id ?? 0));

	internal static T GetSelected<T>(this ComboBox comboBox)
		=> (T)comboBox.SelectedItem.OrThrow();

	internal static T? GetSelected<T>(this ListBox listBox)
		where T : class
		=> listBox.SelectedItem as T;

	internal static List<T> GetMultiSelected<T>(this ListBox listBox)
		where T : class
		=> [..listBox.SelectedItems
				     .Cast<T>()];

	internal static IEnumerable<T> GetAll<T>(this ListBox listBox)
		where T : IRecord
		=> listBox.Items
				  .Cast<T>();

	internal static T? Find<T>(this ListBox listBox,
							   T? record)
		where T : IdentityRecord
		=> record is null
			   ? null
			   : listBox.GetAll<T>()
						.SingleOrDefault(t => t.Is(record));

	internal static List<T> PowerControls<T>(this Panel panel)
		where T : Control
		=> [..panel.Controls
				   .OfType<T>()
				   .OrderBy(static control => control.Name)];

	private static IEnumerable<T> GetFromRow<T>(IEnumerable rows)
		where T : IRecord
		=> rows.Cast<DataGridViewRow>()
			   .Select(static row => row.GetFromRow<T>());

	internal static IEnumerable<T> GetAll<T>(this DataGridView dataGridView)
		where T : IRecord
		=> GetFromRow<T>(dataGridView.Rows);

	internal static IEnumerable<T> GetMultiSelected<T>(this DataGridView dataGridView)
		where T : IRecord
		=> GetFromRow<T>(dataGridView.SelectedRows);

	internal static T GetSelected<T>(this DataGridView dataGridView)
		where T : IRecord
		=> dataGridView.GetMultiSelected<T>()
					   .Single();

	internal static T GetAtIndex<T>(this DataGridView dataGridView,
									int index)
		=> dataGridView.Rows[index]
					   .GetFromRow<T>();

	internal static T GetFromRow<T>(this DataGridViewRow dataGridViewRow)
		=> (T)dataGridViewRow.DataBoundItem;

	internal static void SelectRowWhere<T>(this DataGridView dataGridView,
										   Func<T, bool> func)
		=> dataGridView.CurrentCell = dataGridView.Rows
												  .Cast<DataGridViewRow>()
												  .Single(row => func(row.GetFromRow<T>()))
												  .Cells[0];

	internal static void FillWith(this DataGridView dataGridView,
								  object dataSource)
	{
		dataGridView.AutoGenerateColumns = true;
		dataGridView.AutoSize = false;
		dataGridView.DataSource = dataSource;
	}

	internal static void Deselect(this ComboBox comboBox)
		=> comboBox.SelectedItem = null;

	internal static void Deselect(this DataGridView dataGridView)
	{
		//	Some of this may seem like overkill, but it seems it is safest to do all of this
		dataGridView.ClearSelection();
		if (dataGridView.CurrentRow is not null)
			dataGridView.CurrentRow.Selected = false;
		dataGridView.CurrentCell = null;
	}

	internal static void FillColumn(this DataGridView dataGridView,
									int fillColumnNumber)
	{
		var columnNumber = 0;
		foreach (DataGridViewColumn column in dataGridView.Columns)
		{
			column.AutoSizeMode = columnNumber++ == fillColumnNumber
									  ? DataGridViewAutoSizeColumnMode.Fill
									  : DataGridViewAutoSizeColumnMode.AllCells;
			column.SortMode = DataGridViewColumnSortMode.NotSortable;
		}
	}

	internal static void AlignColumn(this DataGridView dataGridView,
									 DataGridViewContentAlignment alignment,
									 params int[] columns)
		=> columns.ForEach(column => dataGridView.Columns[column].DefaultCellStyle.Alignment = alignment);

	internal static void PowerCells(this DataGridView dataGridView,
									int column)
	{
		dataGridView.Columns[column].HeaderCell.Style.Alignment = MiddleCenter;
		foreach (DataGridViewRow row in dataGridView.Rows)
			row.Cells[column].Style = $"{row.Cells[column].Value}".As<PowerNames>().CellStyle();
	}

	#region ComboBox-to-Label toggler methods

	private static readonly Dictionary<string, Label> ShadowLabels = [];

	private static string ShadowLabelName(this Control control)
		=> $"{(control.Parent?.Name).OrThrow()}.{control.Name}.{typeof (Label)}";

	/// <summary>
	///     Any ComboBox that will use the ToggleEnabled method in its EnabledChanged event
	///     method should call this method in its SelectedIndexChanged event method.
	/// </summary>
	/// <param name="box"></param>
	internal static void UpdateShadowLabel(this ComboBox box)
		=> box.UpdateShadowLabel(null);

	private static void UpdateShadowLabel(this ComboBox box,
										  Label? label)
	{
		if (label is null && !ShadowLabels.TryGetValue(box.ShadowLabelName(), out label))
			return;
		label.Text = box.Text;
		label.TextAlign = box.SelectedItem is IRecord
							  ? ContentAlignment.MiddleLeft
							  : ContentAlignment.MiddleCenter;
	}

	/// <summary>
	///     This method should be called from a ComboBox's EnabledChange event method to toggle
	///     the box to and from a Label (which is much more visible than a disabled ComboBox).
	/// </summary>
	/// <param name="object"></param>
	internal static void ToggleEnabled(this object @object)
	{
		var box = (ComboBox)@object;
		/*
		 Another almost-as-good way to make a disabled ComboBox more visible is simply:
				box.DropDownStyle = box.Enabled
										? DropDownList
										: DropDown;
		*/
		var name = box.ShadowLabelName();
		ShadowLabels.TryGetValue(name, out var label);
		box.Visible = box.Enabled;
		//	Checking for .IsDisposed is important.  Re-create the Label if it's been Disposed.
		if (box.Enabled && (label?.IsDisposed ?? true))
		{
			if (label is not null)
				label.Visible = false;
			return;
		}
		//	I think (?) this if-statement is overkill, but in case I'm wrong, it can't hurt.
		var parent = box.Parent.OrThrow();
		if (label is not null && parent.Contains(label))
			parent.Controls.Remove(label);
		label = ShadowLabels[name] = new ()
									 {
										 Name = name,
										 Location = box.Location,
										 Size = box.Size,
										 Visible = !box.Visible,
										 ForeColor = SystemColors.WindowText,
										 Font = box.Font.Bold //	TODO: Probably never true, so this may actually waste not save cycles
													? box.Font
													: BoldFonts.GetOrSet(box.Font, BoldFont),
										 BorderStyle = BorderStyle.FixedSingle
									 };
		box.UpdateShadowLabel(label);
		(box.Parent?.Controls).OrThrow().Add(label);
		//	It took me hours to figure out that this next line was
		//	what was needed to get the label showing on some Forms!
		//	And, of course, I thought "maybe it needs BringToFront"
		//	more than once while continuing to fight it other ways.
		label.BringToFront();
	}

	#endregion

	internal static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
												  TKey key,
												  Func<TKey, TValue> func)
		=> dictionary.TryGetValue(key, out var result)
			   ? result
			   : dictionary[key] = func(key);

	internal static void Print(this DataGridView dataGridView,
							   string title,
							   string subtitle)
	{
		//	TODO: centering the table cramps the titles and footer text
		var printer = new DGVPrinter
					  {
						  Title = title,
						  SubTitle = subtitle,
						  SubTitleSpacing = 16,
						  PageNumbers = true,
						  PageNumberInHeader = false,
						  TableAlignment = Alignment.Center,
						  ColumnWidth = ColumnWidthSetting.DataWidth,
						  Footer = $"{nameof (DCM)} © {DateTime.Now.Year} ARMADA",
						  FooterSpacing = 15
					  };
		foreach (var cellStyle in printer.ColumnStyles.Values)
		{
			cellStyle.Font = new (cellStyle.Font.FontFamily, 14); //	TODO: seems not to do anything
			cellStyle.WrapMode = DataGridViewTriState.False;
		}
		printer.PrintDialogSettings.AllowPrintToFile = true;
		printer.PrintDataGridView(dataGridView);
	}

	/// <summary>
	/// </summary>
	/// <param name="items"></param>
	/// <returns>
	///     Each of the objects (as a string) on its own line, preceded by a bullet and a space.
	/// </returns>
	internal static string BulletList(this IEnumerable<object> items)
		=> $"{Bullet}{Join(Bullet, items)}";

	internal static string Points(this decimal number)
		=> ((int)number).Points();

	internal static string Points(this int number)
		=> $"{"pt".Pluralize(number, true)}.";

	internal static void ActivateTab(this TabControl tabControl,
									 int tabNumber)
	{
		if (tabControl.SelectedIndex == tabNumber)
			typeof (TabControl).GetMethod("OnSelectedIndexChanged", BindingFlags.NonPublic | BindingFlags.Instance)
							  ?.Invoke(tabControl, EmptyEventArgs);
		else
			tabControl.SelectedIndex = tabNumber;
	}

	internal static void AddOrRemove(this TabControl tabControl,
									 TabPage tabPage,
									 bool add,
									 int? position = null)
		=> (add
				? page => tabControl.TabPages.Insert(position ?? tabControl.TabCount, page)
				: (Action<TabPage>)tabControl.TabPages.Remove)(tabPage);

	internal static IEnumerable<T> WithPlayerId<T>(this IEnumerable<T> linkRecords,
												   int playerId)
		where T : LinkRecord
		=> linkRecords.Where(linkRecord => linkRecord.PlayerId == playerId);

	internal static bool HasPlayerId<T>(this IEnumerable<T> linkRecords,
										int playerId)
		where T : LinkRecord
		=> linkRecords.Any(linkRecord => linkRecord.PlayerId == playerId);

	internal static T ByPlayerId<T>(this IEnumerable<T> linkRecords,
									int playerId)
		where T : LinkRecord
		=> linkRecords.Single(linkRecord => linkRecord.PlayerId == playerId);

	[LinqTunnel]
	internal static IEnumerable<T2> SelectSorted<T1, T2>(this IEnumerable<T1> items,
														 Func<T1, T2> func)
		where T1 : IRecord
		where T2 : IComparable<T2>
		=> items.Select(func)
				.Order();

	internal static DataGridViewCellStyle CellStyle(this PowerNames power)
		=> PowerColors[power];

	#endregion

	#region UI utility methods

	internal static void Show<T>(Action<T>? after = null)
		where T : Form, new()
		=> Show(static () => new (), after);

	internal static void Show<T>(Func<T> constructor,
								 Action<T>? after = null)
		where T : Form
	{
		using var form = constructor();
		form.StartPosition = FormStartPosition.CenterScreen;
		form.ShowDialog();
		after?.Invoke(form);
	}

	internal static Font BoldFont(Font font)
		=> new (font, FontStyle.Bold);

	#endregion

	#region Other utility methods

	internal static IEnumerable<int> Range(int x, int y)
		=> Enumerable.Range(x, y);

	internal static int RandomNumber(int maxValue = int.MaxValue)
		=> Random.Next(maxValue);

	#endregion

	#region Email methods

	internal static MailMessage WriteEmail(string subject,
										   string body,
										   IEnumerable<Player> toPlayers,
										   string? fromName = null,
										   bool toTestAddressOnly = false)
	{
		var message = new MailMessage
					  {
						  From = new (Settings.FromEmailAddress, fromName ?? Settings.FromEmailName),
						  Subject = subject,
						  Body = body,
						  IsBodyHtml = true
					  };
		if (toTestAddressOnly || Settings.TestEmailOnly)
		{
			const string toTestOnlyInfo = """
			                              <h4 style='text-align:center; color:red;'>
			                              												THIS EMAIL WAS SENT TO THE TEST ADDRESS ONLY!
			                              											</h4>
			                              """;
			var testAddress = Settings.TestEmailAddress;
			if (testAddress.Length is 0)
				throw new InvalidOperationException(); //	TODO
			message.To
				   .Add(testAddress);
			message.Body = toTestOnlyInfo + body;
		}
		else
			toPlayers.SelectMany(static player => player.EmailAddresses
														.Select(email => new MailAddress(email, player.Name)))
					 .ForEach(message.To.Add);
		return message;
	}

	internal static string[] SendEmail(params MailMessage[] messages)
	{
		var host = Settings.SmtpHost;
		if (host.Length is 0)
			return SmtpHostNotSet;
		using var client = new SmtpClient(host, Settings.SmtpPort);
		client.Credentials = new System.Net.NetworkCredential(Settings.SmtpUsername, Settings.SmtpPassword);
		client.EnableSsl = Settings.SmtpSsl;
		var tasks = messages.ToDictionary(static message => message.To, client.SendMailAsync);
		WaitAll([..tasks.Values]);
		return [..tasks.Where(static t => t.Value.IsFaulted)
					   .Select(static task => $"Problem sending email to {task.Key}: {task.Value.Exception?.Message}")];
	}

	internal static string[] SendTestEmail(string subject,
										   string body)
		=> SendEmail(WriteEmail(subject, body, Array.Empty<Player>(), toTestAddressOnly: true));

	#endregion

	#endregion
}
