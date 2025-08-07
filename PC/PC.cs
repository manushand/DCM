global using System.Collections;
global using System.ComponentModel;
global using System.Net.Mail;
global using System.Reflection;
global using System.Text.RegularExpressions;
global using static System.ComponentModel.DesignerSerializationVisibility;
global using static System.Environment;
global using static System.String;
global using static System.Windows.Forms.MessageBoxButtons;
global using static System.Windows.Forms.MessageBoxIcon;
global using static System.Windows.Forms.DataGridViewContentAlignment;
global using JetBrains.Annotations;
//
global using DCM;
global using Data;
global using PC.Forms;
global using static PC.PC;
global using static DCM.DCM;
global using static Data.Data;
global using static Data.Game;
global using static Data.Game.Statuses;
global using static Data.GamePlayer;
global using static Data.GamePlayer.Powers;
global using static Data.GamePlayer.Results;
global using Group = Data.Group;
//
using System.Net;
using DGVPrinterHelper;
using static System.IO.Directory;
using static System.Reflection.BindingFlags;
using static System.Threading.Tasks.Task;
using static DGVPrinterHelper.DGVPrinter;

namespace PC;

using Properties;
using static Data.Data;
using static DatabaseTypes;

internal static class PC
{
	#region Fields and Properties

	internal static readonly string Version = Assembly.GetEntryAssembly()
													  ?.GetCustomAttribute<AssemblyFileVersionAttribute>()
													  ?.Version
													  ?? "??";

	internal static readonly Settings Settings = Settings.Default;
	internal static readonly Dictionary<Font, Font> BoldFonts = [];

	internal static DatabaseTypes DatabaseType
	{
		get => Settings.DatabaseType.As<DatabaseTypes>();
		private set => Settings.DatabaseType = value.AsInteger();
	}
	internal static bool SkippingHandlers { get; private set; }
	internal static int[] Seven => [..Enumerable.Range(0, 7).OrderBy(RandomNumber)];

	private static readonly SortedDictionary<Powers, DataGridViewCellStyle> PowerColors
		= new ()
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
							 BackColor = Color.SkyBlue
						 },
			  [Germany] = new ()
						  {
							  BackColor = Color.Black,
							  ForeColor = Color.White
						  },
			  [Italy] = new ()
						{
							BackColor = Color.Lime
						},
			  [Russia] = new (),
			  [Turkey] = new ()
						 {
							 BackColor = Color.Yellow
						 },
			  [TBD] = new ()
					  {
						  BackColor = SystemColors.Control,
						  ForeColor = SystemColors.WindowText
					  }
		  };
	private static readonly MethodInfo? TabSelectionChangedMethod = typeof (TabControl).GetMethod("OnSelectedIndexChanged", NonPublic | Instance);
	private static readonly object[] EmptyEventArgs = [EventArgs.Empty];
	private static readonly string[] DatabaseFileExtensions = [".dcm", ".mdb", ".accdb"];
	private static readonly string[] SmtpHostNotSet = ["SMTP (email) host is not set."];
	private static readonly string FileExtensionPatterns = Join(';', DatabaseFileExtensions.Select(static ext => $"*{ext}"));
	private static readonly string DatabaseFileDialogFilter = $"DCM Data File ({FileExtensionPatterns})|{FileExtensionPatterns}";

	#endregion

	#region Constructor

	static PC()
	{
		var font = BoldFonts.GetOrSet(Control.DefaultFont, BoldFont);
		PowerColors.Values.ForEach(value => (value.Font, value.Alignment) = (font, MiddleCenter));
	}

	#endregion

	#region Methods

	#region Data connection and CRUD methods

	internal static DatabaseTypes OpenDatabase()
	{
		switch (DatabaseType)
		{
		case Access:
			//  Be sure the proper database driver is installed on this host computer.
			try
			{
				CheckDriver();
				//	Get the db file name from saved settings if possible.
				var dbFileName = GetAccessDatabaseFile();
				if (OpenAccessDatabase(dbFileName))
					return DatabaseType;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Data Driver Error", OK, Error);
			}
			break;
		case SqlServer:
			var connectionString = Settings.DatabaseConnectionString;
			if (OpenSqlServerDatabase(connectionString))
				return DatabaseType;
			break;
		case None:
			return DatabaseType;
		default:
			throw new NotImplementedException($"Invalid Database Type ({DatabaseType}");
		}
		DatabaseType = default;
		Settings.Save();
		return default;
	}

	internal static bool OpenAccessDatabase(string? dbFileName = null)
	{
		while (dbFileName is null || !ConnectToAccessDatabase(dbFileName))
		{
			if (dbFileName is not null)
				MessageBox.Show("Failed to connect to database file.",
								"Data Connection Failed");
			using OpenFileDialog dialog = new ();
			dialog.Title = "Choose DCM Data File";
			dialog.Filter = DatabaseFileDialogFilter;
			dialog.FileName = Settings.DatabaseFile;
			if (dialog.ShowDialog() is not DialogResult.OK)
				return false;
			dbFileName = dialog.FileName;
		}
		if (DatabaseType is Access && dbFileName == Settings.DatabaseFile)
			return true;
		FlushCache();
		DatabaseType = Access;
		Settings.DatabaseFile = dbFileName;
		SetEvent();
		return true;
	}

	private static string? GetAccessDatabaseFile()
	{
		//	If a db file has been saved in Settings, use that.
		if (File.Exists(Settings.DatabaseFile))
			return Settings.DatabaseFile;

		//	Otherwise, if there is one and only one file in
		//	the CurrentDirectory with one of the extensions
		//	we like, use it. Otherwise, make the user surf.
		var files = GetFiles(GetCurrentDirectory()).Where(static name => DatabaseFileExtensions.Contains(Path.GetExtension(name)))
												   .ToArray();
		return files.Length is 1
				   ? files.Single()
				   : null;
	}

	internal static void SaveAccessDatabase()
	{
		using SaveFileDialog dialog = new ();
		dialog.Title = "Save DCM Data File As…";
		dialog.Filter = DatabaseFileDialogFilter;
		dialog.RestoreDirectory = true;
		if (dialog.ShowDialog() is not DialogResult.OK)
			return;
		File.Copy(Settings.DatabaseFile, dialog.FileName);
		Settings.DatabaseFile = dialog.FileName;
		Settings.Save();
	}

	internal static bool OpenSqlServerDatabase(string connectionString)
	{
		try
		{
			ConnectToSqlServerDatabase(connectionString);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Could not connect to SQL Server: {ex.Message}", "Data Connection Failed", OK, Error);
			return false;
		}
		if (DatabaseType is SqlServer && Settings.DatabaseConnectionString == connectionString)
			return true;
		FlushCache();
		DatabaseType = SqlServer;
		Settings.DatabaseConnectionString = connectionString;
		SetEvent();
		return true;
	}

	internal static void ClearDatabase()
	{
		DeleteAllData();
		if (Settings.EventId > 0)
			SetEvent();
	}

	internal static void SetEvent(IdInfoRecord? eventRecord = null)
	{
		if (eventRecord is not null and not IdInfoRecord.IEvent)
			throw new ArgumentException($"Invalid EventRecord type ({eventRecord.GetType().Name}).");
		Settings.EventId = eventRecord?.Id ?? 0;
		Settings.EventIsGroup = eventRecord is Group;
		Settings.Save();
	}

	#endregion

	#region Extension methods

	private static readonly Dictionary<string, Label> ShadowLabels = [];

	internal static List<T> PowerControls<T>(this Panel panel)
		where T : Control
		=> [..panel.Controls
				   .OfType<T>()
				   .OrderBy(static control => control.Name)];

	private static IEnumerable<T> GetFromRow<T>(this IEnumerable rows)
		where T : IRecord
		=> rows.Cast<DataGridViewRow>()
			   .Select(static row => row.GetFromRow<T>());

	internal static T GetFromRow<T>(this DataGridViewRow dataGridViewRow)
		=> (T)dataGridViewRow.DataBoundItem.OrThrow();

	extension(TabControl tabControl)
	{
		internal void AddOrRemove(TabPage tabPage,
								  bool add,
								  int? position = null)
		{
			if (add)
				tabControl.TabPages.Insert(position ?? tabControl.TabCount, tabPage);
			else
				tabControl.TabPages.Remove(tabPage);
		}

		internal void ActivateTab(int tabNumber)
		{
			if (tabControl.SelectedIndex == tabNumber)
				TabSelectionChangedMethod?.Invoke(tabControl, EmptyEventArgs);
			else
				tabControl.SelectedIndex = tabNumber;
		}
	}

	extension(ComboBox comboBox)
	{
		internal void FillRange(int start,
								int end)
			=> comboBox.FillWith(Enumerable.Range(start, ++end - start)
										   .Cast<object>());

		internal void SetSelectedItem<T>(T? record)
			where T : IdentityRecord<T>, new()
			=> comboBox.SelectedItem = comboBox.Items
											   .Cast<T>()
											   .SingleOrDefault(t => t.Id == (record?.Id ?? 0));

		internal T GetSelected<T>()
			=> (T)comboBox.SelectedItem.OrThrow();

		internal void Deselect()
			=> comboBox.SelectedItem = null;

		/// <summary>
		///     Any ComboBox that will use the ToggleEnabled method in its EnabledChanged event
		///     method should call this method in its SelectedIndexChanged event method.
		/// </summary>
		internal void UpdateShadowLabel()
			=> comboBox.UpdateShadowLabel(null);

		private void UpdateShadowLabel(Label? label)
		{
			if (label is null && !ShadowLabels.TryGetValue(comboBox.ShadowLabelName, out label))
				return;
			label.Text = comboBox.Text;
			label.TextAlign = comboBox.SelectedItem is IRecord
								  ? ContentAlignment.MiddleLeft
								  : ContentAlignment.MiddleCenter;
		}
	}

	extension(ListControl listControl)
	{
		internal void FillWithSortedPlayers(bool byLastName)
			=> listControl.FillWith(ReadAll<Player>().Sorted(byLastName));

		internal void FillWithSortedPlayers([InstantHandle] Func<Player, bool> func)
			=> listControl.FillWith(ReadMany(func).Sorted());

		internal void FillWithSorted<T>()
			where T : IdInfoRecord
			=> listControl.FillWith(ReadAll<T>().Order());

		internal void FillWithRecords<T>([InstantHandle] IEnumerable<T> items)
			where T : IRecord
			=> listControl.FillWith((IEnumerable<object>)items);

		internal void FillWith([InstantHandle] IEnumerable<object> items)
			=> listControl.FillWith([..items]);

		internal void FillWith(params object[] elements)
		{
			//	This is just a neat little trick taking advantage of the fact that although the two types
			//	don't share an interface, they have identical method signatures for what we want to do.
			var both = ((dynamic)listControl).Items;
			both.Clear();
			both.AddRange(elements);
		}
	}

	extension(ListBox listBox)
	{
		internal void Clear()
			=> listBox.Items
					  .Clear();

		internal T? GetSelected<T>()
			where T : class
			=> listBox.SelectedItem as T;

		internal List<T> GetMultiSelected<T>()
			where T : class
			=> [..listBox.SelectedItems.Cast<T>()];

		internal IEnumerable<T> GetAll<T>()
			where T : IRecord
			=> listBox.Items
					  .Cast<T>();

		internal T? Find<T>(T? record)
			where T : IdInfoRecord
			=> record is null
				   ? null
				   : listBox.GetAll<T>()
							.SingleOrDefault(t => t.Is(record));
	}

	extension(DataGridView dataGridView)
	{
		internal IEnumerable<T> GetAll<T>()
			where T : IRecord
			=> dataGridView.Rows.GetFromRow<T>();

		internal IEnumerable<T> GetMultiSelected<T>()
			where T : IRecord
			=> dataGridView.SelectedRows
						   .GetFromRow<T>();

		internal T GetSelected<T>()
			where T : IRecord
			=> dataGridView.GetMultiSelected<T>()
						   .Single();

		internal T GetAtIndex<T>(int index)
			=> dataGridView.Rows[index]
						   .GetFromRow<T>();

		internal void SelectRowWhere<T>(Func<T, bool> func)
			=> dataGridView.CurrentCell = dataGridView.Rows
													  .Cast<DataGridViewRow>()
													  .Single(row => func(row.GetFromRow<T>()))
													  .Cells[0];

		internal void FillWith(object dataSource)
		{
			dataGridView.AutoGenerateColumns = true;
			dataGridView.AutoSize = false;
			dataGridView.DataSource = dataSource is IEnumerable<object> list
										  ? list.ToList()
										  : dataSource;
		}

		internal void Deselect()
		{
			//	Some of this may seem like overkill, but it seems it is safest to do all of this
			dataGridView.ClearSelection();
			dataGridView.CurrentRow?.Selected = false;
			dataGridView.CurrentCell = null;
		}

		internal void FillColumn(int fillColumnNumber)
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

		internal void AlignColumn(DataGridViewContentAlignment alignment,
								  params int[] columns)
			=> columns.ForEach(column => dataGridView.Columns[column].DefaultCellStyle.Alignment = alignment);

		internal void PowerCells(int column)
		{
			dataGridView.Columns[column].HeaderCell.Style.Alignment = MiddleCenter;
			foreach (DataGridViewRow row in dataGridView.Rows)
				row.Cells[column].Style = $"{row.Cells[column].Value}".As<Powers>().CellStyle;
		}

		internal void Print(string title,
							string subtitle)
		{
			//	TODO: centering the table cramps the titles and footer text
			DGVPrinter printer = new ()
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
				cellStyle.Font = new (cellStyle.Font.FontFamily, 14); // TODO: seems not to do anything
				cellStyle.WrapMode = DataGridViewTriState.False;
			}
			printer.PrintDialogSettings.AllowPrintToFile = true;
			printer.PrintDataGridView(dataGridView);
		}
	}

	extension(DataGridViewCellStyle style)
	{
		internal string StyleTag => $"""style="color: {style.ForeColor}; background-color: {style.BackColor};" """;
	}

	extension(Control control)
	{
		private string ShadowLabelName => $"{(control.Parent?.Name).OrThrow()}.{control.Name}.{typeof (Label)}";

		//	This method is an event handler for the EnabledChanged event of a ComboBox.
		//	It makes the ComboBox's label (if it exists) more visible or less visible.
		internal void ComboBox_EnabledChanged(object @object,
											  EventArgs e)
		{
			var comboBox = (ComboBox)@object;
			/*
			 Another almost-as-good way to make a disabled ComboBox more visible is:
					if (box.Enabled) box.DropDownStyle = DropDownList else DropDown;
			*/
			var name = comboBox.ShadowLabelName;
			ShadowLabels.TryGetValue(name, out var label);
			comboBox.Visible = comboBox.Enabled;
			//	Checking for .IsDisposed is important.  Re-create the Label if it's been Disposed.
			if (comboBox.Enabled && (label?.IsDisposed ?? true))
			{
				label?.Hide();
				return;
			}
			//	I think (?) this if-statement is overkill, but in case I'm wrong, it can't hurt.
			var parent = comboBox.Parent.OrThrow();
			if (label is not null && parent.Contains(label))
				parent.Controls.Remove(label);
			label =
				ShadowLabels[name] =
					new ()
					{
						Name = name,
						Location = comboBox.Location,
						Size = comboBox.Size,
						Visible = !comboBox.Visible,
						ForeColor = SystemColors.WindowText,
						Font = comboBox.Font.Bold // TODO: Probably never true, so this may actually waste not save cycles
								   ? comboBox.Font
								   : BoldFonts.GetOrSet(comboBox.Font, BoldFont),
						BorderStyle = BorderStyle.FixedSingle
					};
			comboBox.UpdateShadowLabel(label);
			(comboBox.Parent?.Controls).OrThrow().Add(label);
			//	It took me hours to figure out that this next line was
			//	what was needed to get the label showing on some Forms!
			//	And, of course, I thought "maybe it needs BringToFront"
			//	more than once while continuing to fight it other ways.
			label.BringToFront();
		}
	}

	extension(Powers power)
	{
		internal DataGridViewCellStyle CellStyle => PowerColors[power];
	}

	#endregion

	#region UI utility methods

	internal static void SkipHandlers([InstantHandle] Action action)
	{
		SkippingHandlers = true;
		action();
		SkippingHandlers = false;
	}

	internal static void Show<T>([InstantHandle] Action<T>? after = null)
		where T : Form, new()
		=> Show(static () => new (), after);

	internal static void Show<T>([InstantHandle] Func<T> constructor,
								 [InstantHandle] Action<T>? after = null)
		where T : Form
	{
		using var form = constructor();
		form.StartPosition = FormStartPosition.CenterScreen;
		form.ShowDialog();
		after?.Invoke(form);
	}

	internal static Font BoldFont(Font font)
		=> new (font, FontStyle.Bold);

	internal static void SetVisible(bool visible,
									[InstantHandle] params IEnumerable<Control> controls)
		=> controls.ForEach(control => control.Visible = visible);

	internal static void SetEnabled(bool enabled,
									[InstantHandle] params IEnumerable<Control> controls)
		=> controls.ForEach(control => control.Enabled = enabled);

	#endregion

	#region Email methods

	internal static MailMessage WriteEmail(string subject,
										   string body,
										   Player? toPlayer = null,
										   string? fromName = null)
	{
		MailMessage message = new ()
							  {
								  From = new (Settings.FromEmailAddress, fromName ?? Settings.FromEmailName),
								  Subject = subject,
								  Body = body,
								  IsBodyHtml = true
							  };
		if (toPlayer is null || Settings.TestEmailOnly)
		{
			const string toTestOnlyInfo = """
                                          <h4 style="text-align:center; color:red;">
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
			toPlayer.EmailAddresses
					.Select(email => new MailAddress(email, toPlayer.Name))
					.ForEach(message.To.Add);
		return message;
	}

	internal static string[] SendEmail([InstantHandle] params MailMessage[] messages)
	{
		var host = Settings.SmtpHost;
		if (host.Length is 0)
			return SmtpHostNotSet;
		using SmtpClient client = new (host, Settings.SmtpPort);
		client.Credentials = new NetworkCredential(Settings.SmtpUsername, Settings.SmtpPassword);
		client.EnableSsl = Settings.SmtpSsl;
		var tasks = messages.ToDictionary(static message => message.To, client.SendMailAsync);
		WaitAll(tasks.Values);
		return [..tasks.Where(static t => t.Value.IsFaulted)
					   .Select(static task => $"Problem sending email to {task.Key}: {task.Value.Exception?.Message ?? "UNKNOWN"}")];
	}

	internal static string[] SendTestEmail(string subject,
										   string body)
		=> SendEmail(WriteEmail(subject, body));

	#endregion

	#endregion
}
