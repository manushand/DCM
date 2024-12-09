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
global using static DCM.DCM.PowerNames;
global using static Data.Data;
global using static Data.Game;
global using static Data.Game.Statuses;
global using static Data.GamePlayer;
global using static Data.GamePlayer.Results;
global using Group = Data.Group;
//
using System.Net;
using DGVPrinterHelper;
using static System.IO.Directory;
using static System.Threading.Tasks.Task;
using static DGVPrinterHelper.DGVPrinter;

namespace PC;

using Properties;
using static Data.Data;

internal static class PC
{
	#region Fields and Properties

	internal const string Version = "24.12.07";

	internal static readonly Settings Settings = Settings.Default;
	internal static readonly Dictionary<Font, Font> BoldFonts = [];

	internal static bool SkippingHandlers { get; private set; }
	internal static int[] Seven => [..Range(0, 7).OrderBy(RandomNumber)];

	private static readonly SortedDictionary<PowerNames, DataGridViewCellStyle> PowerColors
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

	internal static bool OpenDatabase(string? dbFileName = null)
	{
		while (dbFileName is null || !Connect(dbFileName))
		{
			if (dbFileName is not null)
				MessageBox.Show("Failed to connect to database file.",
								"Data Connection Failed");
			using var dialog = new OpenFileDialog();
			dialog.Title = "Choose DCM Data File";
			dialog.Filter = DatabaseFileDialogFilter;
			if (dialog.ShowDialog() is not DialogResult.OK)
				return false;
			dbFileName = dialog.FileName;
		}

		if (dbFileName == Settings.DatabaseFile)
			return true;
		FlushCache();
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
		var files = GetFiles(GetCurrentDirectory()).Where(static name => DatabaseFileExtensions.Contains(Path.GetExtension(name)))
												   .ToArray();
		return files.Length is 1
				   ? files.Single()
				   : null;
	}

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

	internal static void ClearDatabase()
	{
		Clear();
		if (Settings.EventId > 0)
			SetEvent();
	}

	internal static void SetEvent(IdentityRecord? identityRecord = null)
	{
		Settings.EventId = identityRecord?.Id ?? 0;
		Settings.EventIsGroup = identityRecord is Group;
		Settings.Save();
	}

	#endregion

	#region Extension methods

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
		if (listControl is not (object @object and (ListBox or ComboBox)))
			return;
		dynamic both = @object;
		both.Items
			.Clear();
		both.Items
			.AddRange(elements);
	}

	internal static void SetSelectedItem<T>(this ComboBox comboBox,
											T? record)
		where T : IdentityRecord
		=> comboBox.SelectedItem = comboBox.Items
										   .Cast<T>()
										   .SingleOrDefault(t => t.Id == (record?.Id ?? 0));

	internal static T GetSelected<T>(this ComboBox comboBox)
		=> (T)comboBox.SelectedItem.OrThrow();

	internal static void Deselect(this ComboBox comboBox)
		=> comboBox.SelectedItem = null;

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
		dataGridView.DataSource = dataSource is IEnumerable<object> list
									  ? list.ToList()
									  : dataSource;
	}

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

	internal static T GetFromRow<T>(this DataGridViewRow dataGridViewRow)
		=> (T)dataGridViewRow.DataBoundItem.OrThrow();

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
			label?.Hide();
			return;
		}
		//	I think (?) this if-statement is overkill, but in case I'm wrong, it can't hurt.
		var parent = box.Parent.OrThrow();
		if (label is not null && parent.Contains(label))
			parent.Controls.Remove(label);
		label =
			ShadowLabels[name] =
				new ()
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
						  Footer = $"{nameof (PC)} © {DateTime.Now.Year} ARMADA",
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

	public static DataGridViewCellStyle CellStyle(this PowerNames power)
		=> PowerColors[power];

	internal static string Tag(this DataGridViewCellStyle style)
		=> $"""style="color: {style.ForeColor}; background-color: {style.BackColor};" """;

	#endregion

	#region UI utility methods

	internal static void SkipHandlers(Action action)
	{
		SkippingHandlers = true;
		action();
		SkippingHandlers = false;
	}

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

	internal static void SetVisible(bool visible, params List<Control> controls)
		=> controls.ForEach(control => control.Visible = visible);

	internal static void SetEnabled(bool enabled, params List<Control> controls)
		=> controls.ForEach(control => control.Enabled = enabled);

	#endregion

	#region Email methods

	internal static MailMessage WriteEmail(string subject,
										   string body,
										   Player? toPlayer = null,
										   string? fromName = null)
	{
		var message = new MailMessage
					  {
						  From = new (Settings.FromEmailAddress, fromName ?? Settings.FromEmailName),
						  Subject = subject,
						  Body = body,
						  IsBodyHtml = true
					  };
		if (toPlayer is null || Settings.TestEmailOnly)
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
			toPlayer.EmailAddresses
					.Select(email => new MailAddress(email, toPlayer.Name))
					.ForEach(message.To.Add);
		return message;
	}

	internal static string[] SendEmail(params MailMessage[] messages)
	{
		var host = Settings.SmtpHost;
		if (host.Length is 0)
			return SmtpHostNotSet;
		using var client = new SmtpClient(host, Settings.SmtpPort);
		client.Credentials = new NetworkCredential(Settings.SmtpUsername, Settings.SmtpPassword);
		client.EnableSsl = Settings.SmtpSsl;
		var tasks = messages.ToDictionary(static message => message.To, client.SendMailAsync);
		WaitAll([..tasks.Values]);
		return [..tasks.Where(static t => t.Value.IsFaulted)
					   .Select(static task => $"Problem sending email to {task.Key}: {task.Value.Exception?.Message ?? "UNKNOWN"}")];
	}

	internal static string[] SendTestEmail(string subject,
										   string body)
		=> SendEmail(WriteEmail(subject, body));

	#endregion

	#endregion
}
