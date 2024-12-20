namespace PC.Forms
{
	internal sealed partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components is not null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			TournamentToolStripMenuItem = new ToolStripMenuItem();
			OpenTournamentMenuItem = new ToolStripMenuItem();
			NewTournamentToolStripMenuItem = new ToolStripMenuItem();
			DeleteTournamentMenuItem = new ToolStripMenuItem();
			PlayersToolStripMenuItem = new ToolStripMenuItem();
			ManagePlayersToolStripMenuItem = new ToolStripMenuItem();
			PlayerConflictsToolStripMenuItem = new ToolStripMenuItem();
			ConfigurationMenuItem = new ToolStripMenuItem();
			ShowTimingToolStripMenuItem = new ToolStripMenuItem();
			EmailConfigurationToolStripMenuItem = new ToolStripMenuItem();
			ReportsConfigurationToolStripMenuItem = new ToolStripMenuItem();
			MainFormMenuStrip = new MenuStrip();
			GroupToolStripMenuItem = new ToolStripMenuItem();
			OpenGroupMenuItem = new ToolStripMenuItem();
			ManageGroupsToolStripMenuItem = new ToolStripMenuItem();
			ScoringToolStripMenuItem = new ToolStripMenuItem();
			ScoringSystemsToolStripMenuItem = new ToolStripMenuItem();
			HelpToolStripMenuItem = new ToolStripMenuItem();
			HelpTopicsToolStripMenuItem = new ToolStripMenuItem();
			HelpAboutToolStripMenuItem1 = new ToolStripMenuItem();
			TournamentNameLabel = new Label();
			ButtonPanel = new Panel();
			RightButton = new Button();
			MiddleButton = new Button();
			LeftButton = new Button();
			DatabaseToolStripMenuItem = new ToolStripMenuItem();
			SqlServerToolStripMenuItem = new ToolStripMenuItem();
			AccessToolStripMenuItem = new ToolStripMenuItem();
			AccessOpenToolStripMenuItem = new ToolStripMenuItem();
			SaveAsDatabaseToolStripMenuItem = new ToolStripMenuItem();
			CheckDatabaseToolStripMenuItem = new ToolStripMenuItem();
			ClearDatabaseToolStripMenuItem = new ToolStripMenuItem();
			MainFormMenuStrip.SuspendLayout();
			ButtonPanel.SuspendLayout();
			SuspendLayout();
			//
			// TournamentToolStripMenuItem
			//
			TournamentToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenTournamentMenuItem, NewTournamentToolStripMenuItem, DeleteTournamentMenuItem });
			TournamentToolStripMenuItem.Name = "TournamentToolStripMenuItem";
			TournamentToolStripMenuItem.Size = new Size(84, 20);
			TournamentToolStripMenuItem.Text = "&Tournament";
			//
			// OpenTournamentMenuItem
			//
			OpenTournamentMenuItem.Name = "OpenTournamentMenuItem";
			OpenTournamentMenuItem.Size = new Size(116, 22);
			OpenTournamentMenuItem.Text = "&Open…";
			OpenTournamentMenuItem.Click += OpenTournamentMenuItem_Click;
			//
			// NewTournamentToolStripMenuItem
			//
			NewTournamentToolStripMenuItem.Name = "NewTournamentToolStripMenuItem";
			NewTournamentToolStripMenuItem.Size = new Size(116, 22);
			NewTournamentToolStripMenuItem.Text = "&New…";
			NewTournamentToolStripMenuItem.Click += NewTournamentMenuItem_Click;
			//
			// DeleteTournamentMenuItem
			//
			DeleteTournamentMenuItem.Name = "DeleteTournamentMenuItem";
			DeleteTournamentMenuItem.Size = new Size(116, 22);
			DeleteTournamentMenuItem.Text = "&Delete…";
			DeleteTournamentMenuItem.Click += DeleteTournamentMenuItem_Click;
			//
			// PlayersToolStripMenuItem
			//
			PlayersToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ManagePlayersToolStripMenuItem, PlayerConflictsToolStripMenuItem });
			PlayersToolStripMenuItem.Name = "PlayersToolStripMenuItem";
			PlayersToolStripMenuItem.Size = new Size(56, 20);
			PlayersToolStripMenuItem.Text = "&Players";
			//
			// ManagePlayersToolStripMenuItem
			//
			ManagePlayersToolStripMenuItem.Name = "ManagePlayersToolStripMenuItem";
			ManagePlayersToolStripMenuItem.Size = new Size(130, 22);
			ManagePlayersToolStripMenuItem.Text = "&Manage…";
			ManagePlayersToolStripMenuItem.Click += PlayerManagementMenuItem_Click;
			//
			// PlayerConflictsToolStripMenuItem
			//
			PlayerConflictsToolStripMenuItem.Name = "PlayerConflictsToolStripMenuItem";
			PlayerConflictsToolStripMenuItem.Size = new Size(130, 22);
			PlayerConflictsToolStripMenuItem.Text = "&Conflicts…";
			PlayerConflictsToolStripMenuItem.Click += PlayerConflictsMenuItem_Click;
			//
			// ConfigurationMenuItem
			//
			ConfigurationMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ShowTimingToolStripMenuItem, EmailConfigurationToolStripMenuItem, DatabaseToolStripMenuItem, ReportsConfigurationToolStripMenuItem });
			ConfigurationMenuItem.Name = "ConfigurationMenuItem";
			ConfigurationMenuItem.Size = new Size(93, 20);
			ConfigurationMenuItem.Text = "&Configuration";
			//
			// ShowTimingToolStripMenuItem
			//
			ShowTimingToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
			ShowTimingToolStripMenuItem.Name = "ShowTimingToolStripMenuItem";
			ShowTimingToolStripMenuItem.Size = new Size(180, 22);
			ShowTimingToolStripMenuItem.Text = "Show &Timing";
			ShowTimingToolStripMenuItem.ToolTipText = "Display elapsed time taken by seeding and game scoring";
			ShowTimingToolStripMenuItem.Click += ShowTimingToolStripMenuItem_Click;
			//
			// EmailConfigurationToolStripMenuItem
			//
			EmailConfigurationToolStripMenuItem.Name = "EmailConfigurationToolStripMenuItem";
			EmailConfigurationToolStripMenuItem.Size = new Size(180, 22);
			EmailConfigurationToolStripMenuItem.Text = "&Email…";
			EmailConfigurationToolStripMenuItem.Click += EmailSetupToolStripMenuItem_Click;
			//
			// ReportsConfigurationToolStripMenuItem
			//
			ReportsConfigurationToolStripMenuItem.Name = "ReportsConfigurationToolStripMenuItem";
			ReportsConfigurationToolStripMenuItem.Size = new Size(180, 22);
			ReportsConfigurationToolStripMenuItem.Text = "&Reports…";
			//
			// MainFormMenuStrip
			//
			MainFormMenuStrip.ImageScalingSize = new Size(40, 40);
			MainFormMenuStrip.Items.AddRange(new ToolStripItem[] { PlayersToolStripMenuItem, GroupToolStripMenuItem, ScoringToolStripMenuItem, TournamentToolStripMenuItem, ConfigurationMenuItem, HelpToolStripMenuItem });
			MainFormMenuStrip.Location = new Point(0, 0);
			MainFormMenuStrip.Name = "MainFormMenuStrip";
			MainFormMenuStrip.Padding = new Padding(7, 2, 0, 2);
			MainFormMenuStrip.Size = new Size(525, 24);
			MainFormMenuStrip.TabIndex = 5;
			//
			// GroupToolStripMenuItem
			//
			GroupToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenGroupMenuItem, ManageGroupsToolStripMenuItem });
			GroupToolStripMenuItem.Name = "GroupToolStripMenuItem";
			GroupToolStripMenuItem.Size = new Size(52, 20);
			GroupToolStripMenuItem.Text = "&Group";
			//
			// OpenGroupMenuItem
			//
			OpenGroupMenuItem.Name = "OpenGroupMenuItem";
			OpenGroupMenuItem.Size = new Size(126, 22);
			OpenGroupMenuItem.Text = "&Open…";
			OpenGroupMenuItem.Click += OpenGroupMenuItem_Click;
			//
			// ManageGroupsToolStripMenuItem
			//
			ManageGroupsToolStripMenuItem.Name = "ManageGroupsToolStripMenuItem";
			ManageGroupsToolStripMenuItem.Size = new Size(126, 22);
			ManageGroupsToolStripMenuItem.Text = "&Manage…";
			ManageGroupsToolStripMenuItem.Click += GroupManagementMenuItem_Click;
			//
			// ScoringToolStripMenuItem
			//
			ScoringToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ScoringSystemsToolStripMenuItem });
			ScoringToolStripMenuItem.Name = "ScoringToolStripMenuItem";
			ScoringToolStripMenuItem.Size = new Size(59, 20);
			ScoringToolStripMenuItem.Text = "&Scoring";
			//
			// ScoringSystemsToolStripMenuItem
			//
			ScoringSystemsToolStripMenuItem.Name = "ScoringSystemsToolStripMenuItem";
			ScoringSystemsToolStripMenuItem.Size = new Size(126, 22);
			ScoringSystemsToolStripMenuItem.Text = "&Systems…";
			ScoringSystemsToolStripMenuItem.Click += ScoringSystemsMenuItem_Click;
			//
			// HelpToolStripMenuItem
			//
			HelpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { HelpTopicsToolStripMenuItem, HelpAboutToolStripMenuItem1 });
			HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
			HelpToolStripMenuItem.Size = new Size(44, 20);
			HelpToolStripMenuItem.Text = "&Help";
			//
			// HelpTopicsToolStripMenuItem
			//
			HelpTopicsToolStripMenuItem.Name = "HelpTopicsToolStripMenuItem";
			HelpTopicsToolStripMenuItem.Size = new Size(180, 22);
			HelpTopicsToolStripMenuItem.Text = "&Topics…";
			HelpTopicsToolStripMenuItem.Click += HelpTopicsToolStripMenuItem_Click;
			//
			// HelpAboutToolStripMenuItem1
			//
			HelpAboutToolStripMenuItem1.Name = "HelpAboutToolStripMenuItem1";
			HelpAboutToolStripMenuItem1.Size = new Size(180, 22);
			HelpAboutToolStripMenuItem1.Text = "&About…";
			HelpAboutToolStripMenuItem1.Click += HelpAboutToolStripMenuItem_Click;
			//
			// TournamentNameLabel
			//
			TournamentNameLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			TournamentNameLabel.Location = new Point(7, 28);
			TournamentNameLabel.Margin = new Padding(4, 0, 4, 0);
			TournamentNameLabel.Name = "TournamentNameLabel";
			TournamentNameLabel.Size = new Size(510, 67);
			TournamentNameLabel.TabIndex = 7;
			TournamentNameLabel.Text = "DCM version yy.mm.dd\r\nStab You Soon!";
			TournamentNameLabel.TextAlign = ContentAlignment.MiddleCenter;
			//
			// ButtonPanel
			//
			ButtonPanel.Controls.Add(RightButton);
			ButtonPanel.Controls.Add(MiddleButton);
			ButtonPanel.Controls.Add(LeftButton);
			ButtonPanel.Location = new Point(12, 98);
			ButtonPanel.Margin = new Padding(4, 3, 4, 3);
			ButtonPanel.Name = "ButtonPanel";
			ButtonPanel.Size = new Size(499, 51);
			ButtonPanel.TabIndex = 9;
			//
			// RightButton
			//
			RightButton.Location = new Point(334, 1);
			RightButton.Margin = new Padding(2);
			RightButton.Name = "RightButton";
			RightButton.Size = new Size(160, 47);
			RightButton.TabIndex = 11;
			RightButton.Text = "Scores";
			RightButton.UseVisualStyleBackColor = true;
			RightButton.Click += RightButton_Click;
			//
			// MiddleButton
			//
			MiddleButton.Location = new Point(169, 1);
			MiddleButton.Margin = new Padding(2);
			MiddleButton.Name = "MiddleButton";
			MiddleButton.Size = new Size(160, 47);
			MiddleButton.TabIndex = 10;
			MiddleButton.Text = "Rounds";
			MiddleButton.UseVisualStyleBackColor = true;
			MiddleButton.Click += MiddleButton_Click;
			//
			// LeftButton
			//
			LeftButton.Location = new Point(5, 1);
			LeftButton.Margin = new Padding(2);
			LeftButton.Name = "LeftButton";
			LeftButton.Size = new Size(160, 47);
			LeftButton.TabIndex = 9;
			LeftButton.Text = "Details";
			LeftButton.UseVisualStyleBackColor = true;
			LeftButton.Click += LeftButton_Click;
			//
			// databaseToolStripMenuItem
			//
			DatabaseToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { AccessToolStripMenuItem, SqlServerToolStripMenuItem, ClearDatabaseToolStripMenuItem });
			DatabaseToolStripMenuItem.Name = "DatabaseToolStripMenuItem";
			DatabaseToolStripMenuItem.Size = new Size(180, 22);
			DatabaseToolStripMenuItem.Text = "&Database";
			//
			// SqlServerToolStripMenuItem
			//
			SqlServerToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
			SqlServerToolStripMenuItem.Name = "SqlServerToolStripMenuItem";
			SqlServerToolStripMenuItem.Size = new Size(180, 22);
			SqlServerToolStripMenuItem.Text = "&SQL Server…";
			SqlServerToolStripMenuItem.Click += SqlServerToolStripMenuItem_Click;
			//
			// DatabaseConfigurationToolStripMenuItem
			//
			AccessToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { AccessOpenToolStripMenuItem, SaveAsDatabaseToolStripMenuItem, CheckDatabaseToolStripMenuItem });
			AccessToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
			AccessToolStripMenuItem.Name = "AccessToolStripMenuItem";
			AccessToolStripMenuItem.Size = new Size(180, 22);
			AccessToolStripMenuItem.Text = "&Access";
			//
			// OpenDatabaseToolStripMenuItem
			//
			AccessOpenToolStripMenuItem.Name = "AccessOpenToolStripMenuItem";
			AccessOpenToolStripMenuItem.Size = new Size(180, 22);
			AccessOpenToolStripMenuItem.Text = "&Open…";
			AccessOpenToolStripMenuItem.Click += DatabaseOpenToolStripMenuItem_Click;
			//
			// SaveAsDatabaseToolStripMenuItem
			//
			SaveAsDatabaseToolStripMenuItem.Name = "SaveAsDatabaseToolStripMenuItem";
			SaveAsDatabaseToolStripMenuItem.Size = new Size(180, 22);
			SaveAsDatabaseToolStripMenuItem.Text = "&Save As…";
			SaveAsDatabaseToolStripMenuItem.Click += DatabaseSaveAsToolStripMenuItem_Click;
			//
			// CheckDatabaseToolStripMenuItem
			//
			CheckDatabaseToolStripMenuItem.Name = "CheckDatabaseToolStripMenuItem";
			CheckDatabaseToolStripMenuItem.Size = new Size(180, 22);
			CheckDatabaseToolStripMenuItem.Text = "&Accessibility";
			CheckDatabaseToolStripMenuItem.Click += DatabaseCheckToolStripMenuItem_Click;
			//
			// ClearDatabaseToolStripMenuItem
			//
			ClearDatabaseToolStripMenuItem.Name = "ClearDatabaseToolStripMenuItem";
			ClearDatabaseToolStripMenuItem.Size = new Size(180, 22);
			ClearDatabaseToolStripMenuItem.Text = "&Clear…";
			ClearDatabaseToolStripMenuItem.Click += DatabaseClearToolStripMenuItem_Click;
			//
			// MainForm
			//
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(525, 162);
			Controls.Add(ButtonPanel);
			Controls.Add(TournamentNameLabel);
			Controls.Add(MainFormMenuStrip);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Margin = new Padding(2);
			MaximizeBox = false;
			Name = "MainForm";
			Text = "Diplomacy Competition Manager";
			Load += MainForm_Load;
			MainFormMenuStrip.ResumeLayout(false);
			MainFormMenuStrip.PerformLayout();
			ButtonPanel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private ToolStripMenuItem TournamentToolStripMenuItem;
		private ToolStripMenuItem NewTournamentToolStripMenuItem;
		private ToolStripMenuItem OpenTournamentMenuItem;
		private ToolStripMenuItem DeleteTournamentMenuItem;
		private ToolStripMenuItem PlayersToolStripMenuItem;
		private ToolStripMenuItem ManagePlayersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ConfigurationMenuItem;
		private MenuStrip MainFormMenuStrip;
		private Label TournamentNameLabel;
		private ToolStripMenuItem PlayerConflictsToolStripMenuItem;
		private ToolStripMenuItem EmailConfigurationToolStripMenuItem;
		private ToolStripMenuItem GroupToolStripMenuItem;
		private ToolStripMenuItem OpenGroupMenuItem;
		private ToolStripMenuItem ReportsConfigurationToolStripMenuItem;
		private ToolStripMenuItem HelpToolStripMenuItem;
		private ToolStripMenuItem HelpAboutToolStripMenuItem1;
        private ToolStripMenuItem HelpTopicsToolStripMenuItem;
		private ToolStripMenuItem ManageGroupsToolStripMenuItem;
		private ToolStripMenuItem ScoringToolStripMenuItem;
		private ToolStripMenuItem ScoringSystemsToolStripMenuItem;
		private ToolStripMenuItem ShowTimingToolStripMenuItem;
		private Panel ButtonPanel;
		private Button RightButton;
		private Button MiddleButton;
		private Button LeftButton;
		private ToolStripMenuItem DatabaseToolStripMenuItem;
		private ToolStripMenuItem AccessToolStripMenuItem;
		private ToolStripMenuItem AccessOpenToolStripMenuItem;
		private ToolStripMenuItem SaveAsDatabaseToolStripMenuItem;
		private ToolStripMenuItem CheckDatabaseToolStripMenuItem;
		private ToolStripMenuItem SqlServerToolStripMenuItem;
		private ToolStripMenuItem ClearDatabaseToolStripMenuItem;
	}
}
