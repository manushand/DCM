namespace DCM.UI.Forms;

internal sealed partial class EmailSettingsForm : Form
{
	#region Constructor

	public EmailSettingsForm()
		=> InitializeComponent();

	#endregion

	#region Form event handler

	private void EmailSettingsForm_Load(object sender,
										EventArgs e)
	{
		HostTextBox.Text = Settings.SmtpHost;
		PortTextBox.Text = $"{Settings.SmtpPort}";
		UseSslCheckBox.Checked = Settings.SmtpSsl;
		UsernameTextBox.Text = Settings.SmtpUsername;
		PasswordTextBox.Text = Settings.SmtpPassword;
		TestEmailTextBox.Text = Settings.TestEmailAddress;
		MailFromTextBox.Text = Settings.FromEmailAddress;
		FromNameTextBox.Text = Settings.FromEmailName;
		OnlySendToTestCheckBox.Checked = Settings.TestEmailOnly;
		Settings_Changed();
		AssignmentTemplateTextBox.Text = Settings.AssignmentEmailTemplate;
		AnnouncementTemplateTextBox.Text = Settings.AnnouncementEmailTemplate;
		TemplateTextBox_TextChanged();
		TemplatesTabControl_SelectedIndexChanged();
	}

	#endregion

	#region Fields and properties

	private static readonly string[] TemplateLegend = ["Available", "Fill-Ins:", string.Empty, "{TournamentName}", "{PlayerName}"];

	private static readonly string AssignmentTemplateLegend = Join(NewLine,
																   [..TemplateLegend,
																   "{PowerName}", "{GameNumber}", "{RoundNumber}", "{Assignments}"]);

	private static readonly string AnnouncementTemplateLegend = Join(NewLine, [..TemplateLegend, "{MessageText}"]);

	private bool SettingsSaved => HostTextBox.Text.Matches(Settings.SmtpHost)
							   && PortTextBox.Text == $"{Settings.SmtpPort}"
							   && UseSslCheckBox.Checked == Settings.SmtpSsl
							   && UsernameTextBox.Text.Matches(Settings.SmtpUsername)
							   && PasswordTextBox.Text == Settings.SmtpPassword
							   && TestEmailTextBox.Text.Matches(Settings.TestEmailAddress)
							   && MailFromTextBox.Text.Matches(Settings.FromEmailAddress)
							   && FromNameTextBox.Text.Matches(Settings.FromEmailName)
							   && OnlySendToTestCheckBox.Checked == Settings.TestEmailOnly;

	private bool TemplatesSaved => AssignmentTemplateTextBox.Text.Matches(Settings.AssignmentEmailTemplate)
								&& AnnouncementTemplateTextBox.Text.Matches(Settings.AnnouncementEmailTemplate);

	#endregion

	#region Control event handlers

	private void SaveAndSendButton_Click(object sender,
										 EventArgs e)
	{
		var error = string.Empty;
		var port = 25;
		if (Uri.CheckHostName(HostTextBox.Text) is UriHostNameType.Unknown)
			error = "Host name is invalid.";
		else if (PortTextBox.TextLength > 0 && (!int.TryParse(PortTextBox.Text, out port) || port is < 1 or > 65535))
			error = "Port must be a number in the range 1 to 65535.";
		else if (MailFromTextBox.TextLength is 0 || !MailFromTextBox.Text.IsValidEmail())
			error = "From email address is missing or invalid.";
		else if (TestEmailTextBox.TextLength is 0 || !TestEmailTextBox.Text.IsValidEmail())
			error = "Test email address is missing or invalid.";
		if (error.Length > 0)
		{
			MessageBox.Show(error,
							"Invalid Email Settings",
							OK,
							Error);
			return;
		}
		Settings.SmtpHost = HostTextBox.Text;
		Settings.SmtpPort = port;
		Settings.SmtpSsl = UseSslCheckBox.Checked;
		Settings.SmtpUsername = UsernameTextBox.Text;
		Settings.SmtpPassword = PasswordTextBox.Text;
		Settings.TestEmailAddress = TestEmailTextBox.Text;
		Settings.FromEmailAddress = MailFromTextBox.Text;
		Settings.FromEmailName = FromNameTextBox.Text;
		Settings.TestEmailOnly = OnlySendToTestCheckBox.Checked;
		Settings.Save();
		Settings_Changed();
	}

	private void TemplatesTabControl_SelectedIndexChanged(object? sender = null,
														  EventArgs? e = null)
		//	TODO: Add {Scores} ?
		=> TemplateLegendLabel.Text = TemplatesTabControl.SelectedTab == AssignmentTemplateTabPage
										  ? AssignmentTemplateLegend
										  : AnnouncementTemplateLegend;

	private void TemplateTextBox_TextChanged(object? sender = null,
											 EventArgs? e = null)
		=> SaveOrSendTemplateButton.Text = TemplatesSaved
											   ? "Test-Send This Template"
											   : "Save Templates";

	private void SaveOrSendTemplateButton_Click(object sender,
												EventArgs e)
	{
		if (!TemplatesSaved)
		{
			Settings.AssignmentEmailTemplate = AssignmentTemplateTextBox.Text;
			Settings.AnnouncementEmailTemplate = AnnouncementTemplateTextBox.Text;
			Settings.Save();
			TemplateTextBox_TextChanged();
			return;
		}
		var currentTab = TemplatesTabControl.SelectedTab.OrThrow();
		var result = SendTestEmail($"DCM TEST EMAIL: {currentTab.Text}",
								   currentTab == AssignmentTemplateTabPage
									   ? AssignmentTemplateTextBox.Text
									   : AnnouncementTemplateTextBox.Text);
		var succeeded = result.Length is 0;
		MessageBox.Show(succeeded
							? "Test email sent successfully."
							: Join(NewLine, result),
						"Test Email Result",
						OK,
						succeeded
							? Information
							: Error);
	}

	private void Settings_Changed(object? sender = null,
								  EventArgs? e = null)
		=> SaveButton.Visible = !SettingsSaved && HostTextBox.TextLength > 0;

	#endregion
}
