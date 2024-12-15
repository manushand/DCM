using static System.Windows.Forms.ToolStripDropDownCloseReason;

namespace PC.Forms;

internal sealed partial class MainForm : Form
{
	private static bool Connected => Settings.DatabaseType.As<DatabaseTypes>() is DatabaseTypes.Access or DatabaseTypes.SqlServer;

	private IdentityRecord? Event
	{
		get
		{
			var eventId = Connected
				? Settings.EventId
				: 0;
			return field is not null || eventId is 0
					   ? field
					   : field = Settings.EventIsGroup
									 ? ReadById<Group>(eventId)
									 : ReadById<Tournament>(eventId);
		}
		set
		{
			field = value;
			SetEvent(field);
			MainForm_Load();
		}
	}

	internal MainForm()
	{
		InitializeComponent();
		ConfigurationMenuItem.DropDown.Closing += static (_, e) => e.Cancel = e.CloseReason is ItemClicked;
		StartPosition = FormStartPosition.CenterScreen;
		OpenDatabase();
	}

	private void MainForm_Load(object? sender = null,
							   EventArgs? e = null)
	{
		Activate();
		PlayersToolStripMenuItem.Visible =
			GroupToolStripMenuItem.Visible =
				ScoringToolStripMenuItem.Visible =
					TournamentToolStripMenuItem.Visible =
						Connected;
		ShowTimingToolStripMenuItem.Checked =
			ScoringSystem.ShowTimingData =
				Settings.ShowTimingData;
		OpenTournamentMenuItem.Enabled =
			DeleteTournamentMenuItem.Enabled =
				Connected && Any<Tournament>(static tournament => tournament.GroupId is null);
		OpenGroupMenuItem.Enabled = Connected && Any<Group>();
		PlayerConflictsToolStripMenuItem.Enabled = Connected && ReadAll<Player>().Count() > 1;
		ButtonPanel.Visible = Connected && Settings.EventId > 0;
		switch (Event)
		{
		case null:
			TournamentNameLabel.Text = $"DCM version {PC.Version}{NewLine}Stab You Soon!";
			break;
		case Group group:
			TournamentNameLabel.Text = group.Name;
			LeftButton.Text = $"Group Details{NewLine}and Members";
			MiddleButton.Text = "Group Games";
			RightButton.Text = "Member Ratings";
			MiddleButton.Enabled = group.ScoringSystemId
										.HasValue;
			RightButton.Enabled = group.FinishedGames.Length > 0;
			if (MiddleButton.Enabled)
				return;
			var needSystem = $"{NewLine}(No Rating System)";
			MiddleButton.Text += needSystem;
			RightButton.Text += needSystem;
			break;
		case Tournament tournament:
			TournamentNameLabel.Text = tournament.Name;
			LeftButton.Text = tournament.HasTeamTournament
								  ? $"Teams and{NewLine}Tournament Settings"
								  : "Tournament Settings";
			var startedRounds = tournament.Rounds
										  .Length;
			MiddleButton.Text = startedRounds is 0
									? "Registration"
									: startedRounds == tournament.TotalRounds
										? "Rounds"
										: $"Rounds and{NewLine}Registration";
			RightButton.Text = "Scores";
			RightButton.Enabled = tournament.Games //	This could be .FinishedGames but it wastes time doing .ToList()
											.Any(static game => game.Status is Finished);
			break;
		default:
			throw new InvalidOperationException(); //	TODO
		}
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
		=> e.Cancel = MessageBox.Show("Really Quit the Diplomacy Competition Manager?",
									  "Confirm Quit",
									  YesNo,
									  Question) is DialogResult.No;

	#region Scoring menu item event handler

	private void ScoringSystemsMenuItem_Click(object sender,
											  EventArgs e)
		=> Show<ScoringSystemListForm>();

	#endregion

	#region Tournament/Group button event handlers

	private void LeftButton_Click(object sender,
								  EventArgs e)
	{
		switch (Event)
		{
		case Tournament tournament:
			Show<TournamentInfoForm>(() => new (tournament),
									 form => Event = form.Tournament);
			break;
		case Group group:
			Show<GroupInfoForm>(() => new (group),
								form => Event = form.Group);
			break;
		default:
			throw new InvalidOperationException();
		}
	}

	private void MiddleButton_Click(object sender,
									EventArgs e)
	{
		switch (Event)
		{
		case Group group:
			Show<GroupGamesForm>(() => new (group));
			break;
		case Tournament tournament:
			/*
			if (tournament.Rounds.Length is 0
			&&	MessageBox.Show($"Start Round 1?{NewLine}{NewLine}Pre-registration for that round will be closed.",
								"Confirm Start First Round",
								YesNo,
								Question) is DialogResult.No)
				return;
			*/
			Show<RoundInfoForm>(() => new (tournament));
			break;
		default:
			throw new InvalidOperationException();
		}
		MainForm_Load(); //	To enable/disable buttons if needed
	}

	private void RightButton_Click(object sender,
								   EventArgs e)
	{
		switch (Event)
		{
		case Group { FinishedGames.Length: > 0 } group:
			Show<GroupRatingsForm>(() => new (group));
			break;
		case Tournament { FinishedGames.Length: > 0 } tournament:
			Show<ScoresForm>(() => new (tournament));
			break;
		default:
			throw new InvalidOperationException(); //	TODO
		}
	}

	#endregion

	#region Player menu item event handlers

	private void PlayerManagementMenuItem_Click(object sender,
												EventArgs e)
		=> Show<PlayerListForm>();

	private void PlayerConflictsMenuItem_Click(object sender,
											   EventArgs e)
		=> Show<ConflictsForm>();

	#endregion

	#region Group menu item event handler

	private void OpenGroupMenuItem_Click(object sender,
										 EventArgs e)
		=> Show<GroupListForm>(form =>
							   {
								   if (form.Group is not null)
									   Event = form.Group;
							   });

	private void GroupManagementMenuItem_Click(object sender,
											   EventArgs e)
	{
		Show<GroupsForm>();
		//	In case the currently open Group was deleted
		if (Event is Group group)
			Event = ReadOne(group);
	}

	#endregion

	#region Tournament menu item event handlers

	private void OpenTournamentMenuItem_Click(object sender,
											  EventArgs e)
		=> Show<TournamentListForm>(form =>
									{
										if (form.Tournament is not null)
											Event = form.Tournament;
									});

	private void NewTournamentMenuItem_Click(object sender,
											 EventArgs e)
		=> Show<TournamentInfoForm>(form =>
									{
										if (form.Tournament.Id > 0)
											Event = form.Tournament;
									});

	private void DeleteTournamentMenuItem_Click(object sender,
												EventArgs e)
	{
		Show<TournamentListForm>(static () => new (true));
		if (Event is Tournament tournament)
			Event = ReadOne(tournament);
	}

	#endregion

	#region Configuration menu item event handlers

	private void ShowTimingToolStripMenuItem_Click(object sender,
												   EventArgs e)
	{
		ShowTimingToolStripMenuItem.Checked =
			Settings.ShowTimingData =
				ScoringSystem.ShowTimingData =
					!Settings.ShowTimingData;
		Settings.Save();
	}

	private void EmailSetupToolStripMenuItem_Click(object sender,
												   EventArgs e)
		=> Show<EmailSettingsForm>();

	private void SqlServerToolStripMenuItem_Click(object sender, EventArgs e)
		=> Show<SqlServerSettingsForm>();

	private void DatabaseOpenToolStripMenuItem_Click(object sender,
													 EventArgs e)
	{
		if (!OpenAccessDatabase())
			return;
		Event = null;
	}

	private void DatabaseSaveAsToolStripMenuItem_Click(object sender,
													   EventArgs e)
		=> SaveAccessDatabase();

	private void DatabaseClearToolStripMenuItem_Click(object sender,
													  EventArgs e)
	{
		var connection = Settings.DatabaseType.As<DatabaseTypes>();
		var host = connection switch
				   {
					   DatabaseTypes.Access    => $"Access file {Settings.DatabaseFile}",
					   DatabaseTypes.SqlServer => "connected SQL Server database",
					   DatabaseTypes.None      => null,
					   _                       => throw new InvalidOperationException()
				   };
		if (host is null
		||  MessageBox.Show($"""
							 Are you absolutely sure you want to clear all data in the {host}?
							 {NewLine}THERE IS NO UNDO!
							 """,
							"Confirm Data Clear",
							YesNo,
							Exclamation) is DialogResult.No)
			return;
		ClearDatabase();
		Event = null;
	}

	private void DatabaseCheckToolStripMenuItem_Click(object sender,
													  EventArgs e)
	{
		try
		{
			CheckDriver();
			MessageBox.Show("Your system is equipped to use both .accdb and .mdb data files (either type may be renamed .dcm).",
							"Data Driver Report",
							OK,
							Information);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Data Driver Error", OK, Error);
		}
	}

	#endregion

	#region Help menu item event handlers

	private void HelpTopicsToolStripMenuItem_Click(object sender,
												   EventArgs e)
	{
		//	TODO
	}

	private void HelpAboutToolStripMenuItem_Click(object sender,
												  EventArgs e)
	{
		const string legalCopyright = "Copyright © 2018";
		const string companyName = "ARMADA (The Association of Rocky Mountain Area Diplomacy Adversaries) and Manus Hand";
		const string comments = "Contact the ARMADA at https://www.armada-dip.com where the latest version of the DCM can be found and downloaded.";

		MessageBox.Show($"Diplomacy Competition Manager ({nameof (DCM)}) is {legalCopyright}-{DateTime.Now.Year} " +
						$"{companyName}.{NewLine}{NewLine}{comments}{NewLine}{NewLine}This version: {PC.Version}",
						$"About the {nameof (DCM)}",
						OK,
						Information);
	}

	#endregion
}
