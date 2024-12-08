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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.TournamentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenTournamentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.NewTournamentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DeleteTournamentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PlayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ManagePlayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PlayerConflictsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ConfigurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ShowTimingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.EmailConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ReportsConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DatabaseConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveAsDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CheckDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ClearDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.MainFormMenuStrip = new System.Windows.Forms.MenuStrip();
			this.GroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.OpenGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ManageGroupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ScoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ScoringSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.HelpAboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.TournamentNameLabel = new System.Windows.Forms.Label();
			this.ButtonPanel = new System.Windows.Forms.Panel();
			this.RightButton = new System.Windows.Forms.Button();
			this.MiddleButton = new System.Windows.Forms.Button();
			this.LeftButton = new System.Windows.Forms.Button();
			this.MainFormMenuStrip.SuspendLayout();
			this.ButtonPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// TournamentToolStripMenuItem
			//
			this.TournamentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenTournamentMenuItem,
            this.NewTournamentToolStripMenuItem,
            this.DeleteTournamentMenuItem});
			this.TournamentToolStripMenuItem.Name = "TournamentToolStripMenuItem";
			this.TournamentToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
			this.TournamentToolStripMenuItem.Text = "&Tournament";
			//
			// OpenTournamentMenuItem
			//
			this.OpenTournamentMenuItem.Name = "OpenTournamentMenuItem";
			this.OpenTournamentMenuItem.Size = new System.Drawing.Size(116, 22);
			this.OpenTournamentMenuItem.Text = "&Open…";
			this.OpenTournamentMenuItem.Click += new System.EventHandler(this.OpenTournamentMenuItem_Click);
			//
			// NewTournamentToolStripMenuItem
			//
			this.NewTournamentToolStripMenuItem.Name = "NewTournamentToolStripMenuItem";
			this.NewTournamentToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.NewTournamentToolStripMenuItem.Text = "&New…";
			this.NewTournamentToolStripMenuItem.Click += new System.EventHandler(this.NewTournamentMenuItem_Click);
			//
			// DeleteTournamentMenuItem
			//
			this.DeleteTournamentMenuItem.Name = "DeleteTournamentMenuItem";
			this.DeleteTournamentMenuItem.Size = new System.Drawing.Size(116, 22);
			this.DeleteTournamentMenuItem.Text = "&Delete…";
			this.DeleteTournamentMenuItem.Click += new System.EventHandler(this.DeleteTournamentMenuItem_Click);
			//
			// PlayersToolStripMenuItem
			//
			this.PlayersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ManagePlayersToolStripMenuItem,
            this.PlayerConflictsToolStripMenuItem});
			this.PlayersToolStripMenuItem.Name = "PlayersToolStripMenuItem";
			this.PlayersToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.PlayersToolStripMenuItem.Text = "&Players";
			//
			// ManagePlayersToolStripMenuItem
			//
			this.ManagePlayersToolStripMenuItem.Name = "ManagePlayersToolStripMenuItem";
			this.ManagePlayersToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.ManagePlayersToolStripMenuItem.Text = "&Manage…";
			this.ManagePlayersToolStripMenuItem.Click += new System.EventHandler(this.PlayerManagementMenuItem_Click);
			//
			// PlayerConflictsToolStripMenuItem
			//
			this.PlayerConflictsToolStripMenuItem.Name = "PlayerConflictsToolStripMenuItem";
			this.PlayerConflictsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
			this.PlayerConflictsToolStripMenuItem.Text = "&Conflicts…";
			this.PlayerConflictsToolStripMenuItem.Click += new System.EventHandler(this.PlayerConflictsMenuItem_Click);
			//
			// ConfigurationMenuItem
			//
			this.ConfigurationMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowTimingToolStripMenuItem,
            this.EmailConfigurationToolStripMenuItem,
            this.ReportsConfigurationToolStripMenuItem,
            this.DatabaseConfigurationToolStripMenuItem});
			this.ConfigurationMenuItem.Name = "ConfigurationMenuItem";
			this.ConfigurationMenuItem.Size = new System.Drawing.Size(93, 20);
			this.ConfigurationMenuItem.Text = "&Configuration";
			//
			// ShowTimingToolStripMenuItem
			//
			this.ShowTimingToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ShowTimingToolStripMenuItem.Name = "ShowTimingToolStripMenuItem";
			this.ShowTimingToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.ShowTimingToolStripMenuItem.Text = "Show &Timing";
			this.ShowTimingToolStripMenuItem.ToolTipText = "Display elapsed time taken by seeding and game scoring";
			this.ShowTimingToolStripMenuItem.Click += new System.EventHandler(this.ShowTimingToolStripMenuItem_Click);
			//
			// EmailConfigurationToolStripMenuItem
			//
			this.EmailConfigurationToolStripMenuItem.Name = "EmailConfigurationToolStripMenuItem";
			this.EmailConfigurationToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.EmailConfigurationToolStripMenuItem.Text = "&Email…";
			this.EmailConfigurationToolStripMenuItem.Click += new System.EventHandler(this.EmailSetupToolStripMenuItem_Click);
			//
			// ReportsConfigurationToolStripMenuItem
			//
			this.ReportsConfigurationToolStripMenuItem.Name = "ReportsConfigurationToolStripMenuItem";
			this.ReportsConfigurationToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.ReportsConfigurationToolStripMenuItem.Text = "&Reports…";
			//
			// DatabaseConfigurationToolStripMenuItem
			//
			this.DatabaseConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDatabaseToolStripMenuItem,
            this.SaveAsDatabaseToolStripMenuItem,
            this.CheckDatabaseToolStripMenuItem,
            this.ClearDatabaseToolStripMenuItem});
			this.DatabaseConfigurationToolStripMenuItem.Name = "DatabaseConfigurationToolStripMenuItem";
			this.DatabaseConfigurationToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.DatabaseConfigurationToolStripMenuItem.Text = "&Data";
			//
			// OpenDatabaseToolStripMenuItem
			//
			this.OpenDatabaseToolStripMenuItem.Name = "OpenDatabaseToolStripMenuItem";
			this.OpenDatabaseToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.OpenDatabaseToolStripMenuItem.Text = "&Open…";
			this.OpenDatabaseToolStripMenuItem.Click += new System.EventHandler(this.DatabaseOpenToolStripMenuItem_Click);
			//
			// SaveAsDatabaseToolStripMenuItem
			//
			this.SaveAsDatabaseToolStripMenuItem.Name = "SaveAsDatabaseToolStripMenuItem";
			this.SaveAsDatabaseToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.SaveAsDatabaseToolStripMenuItem.Text = "&Save As…";
			this.SaveAsDatabaseToolStripMenuItem.Click += new System.EventHandler(this.DatabaseSaveAsToolStripMenuItem_Click);
			//
			// CheckDatabaseToolStripMenuItem
			//
			this.CheckDatabaseToolStripMenuItem.Name = "CheckDatabaseToolStripMenuItem";
			this.CheckDatabaseToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.CheckDatabaseToolStripMenuItem.Text = "&Accessibility";
			this.CheckDatabaseToolStripMenuItem.Click += new System.EventHandler(this.DatabaseCheckToolStripMenuItem_Click);
			//
			// ClearDatabaseToolStripMenuItem
			//
			this.ClearDatabaseToolStripMenuItem.Name = "ClearDatabaseToolStripMenuItem";
			this.ClearDatabaseToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.ClearDatabaseToolStripMenuItem.Text = "&Clear…";
			this.ClearDatabaseToolStripMenuItem.Click += new System.EventHandler(this.DatabaseClearToolStripMenuItem_Click);
			//
			// MainFormMenuStrip
			//
			this.MainFormMenuStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
			this.MainFormMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PlayersToolStripMenuItem,
            this.GroupToolStripMenuItem,
            this.ScoringToolStripMenuItem,
            this.TournamentToolStripMenuItem,
            this.ConfigurationMenuItem,
            this.HelpToolStripMenuItem});
			this.MainFormMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.MainFormMenuStrip.Name = "MainFormMenuStrip";
			this.MainFormMenuStrip.Size = new System.Drawing.Size(450, 24);
			this.MainFormMenuStrip.TabIndex = 5;
			//
			// GroupToolStripMenuItem
			//
			this.GroupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenGroupMenuItem,
            this.ManageGroupsToolStripMenuItem});
			this.GroupToolStripMenuItem.Name = "GroupToolStripMenuItem";
			this.GroupToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.GroupToolStripMenuItem.Text = "&Group";
			//
			// OpenGroupMenuItem
			//
			this.OpenGroupMenuItem.Name = "OpenGroupMenuItem";
			this.OpenGroupMenuItem.Size = new System.Drawing.Size(126, 22);
			this.OpenGroupMenuItem.Text = "&Open…";
			this.OpenGroupMenuItem.Click += new System.EventHandler(this.OpenGroupMenuItem_Click);
			//
			// ManageGroupsToolStripMenuItem
			//
			this.ManageGroupsToolStripMenuItem.Name = "ManageGroupsToolStripMenuItem";
			this.ManageGroupsToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.ManageGroupsToolStripMenuItem.Text = "&Manage…";
			this.ManageGroupsToolStripMenuItem.Click += new System.EventHandler(this.GroupManagementMenuItem_Click);
			//
			// ScoringToolStripMenuItem
			//
			this.ScoringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScoringSystemsToolStripMenuItem});
			this.ScoringToolStripMenuItem.Name = "ScoringToolStripMenuItem";
			this.ScoringToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.ScoringToolStripMenuItem.Text = "&Scoring";
			//
			// ScoringSystemsToolStripMenuItem
			//
			this.ScoringSystemsToolStripMenuItem.Name = "ScoringSystemsToolStripMenuItem";
			this.ScoringSystemsToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
			this.ScoringSystemsToolStripMenuItem.Text = "&Systems…";
			this.ScoringSystemsToolStripMenuItem.Click += new System.EventHandler(this.ScoringSystemsMenuItem_Click);
			//
			// HelpToolStripMenuItem
			//
			this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpTopicsToolStripMenuItem,
            this.HelpAboutToolStripMenuItem1});
			this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
			this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.HelpToolStripMenuItem.Text = "&Help";
			//
			// HelpTopicsToolStripMenuItem
			//
			this.HelpTopicsToolStripMenuItem.Name = "HelpTopicsToolStripMenuItem";
			this.HelpTopicsToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.HelpTopicsToolStripMenuItem.Text = "&Topics…";
			this.HelpTopicsToolStripMenuItem.Click += new System.EventHandler(this.HelpTopicsToolStripMenuItem_Click);
			//
			// HelpAboutToolStripMenuItem1
			//
			this.HelpAboutToolStripMenuItem1.Name = "HelpAboutToolStripMenuItem1";
			this.HelpAboutToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.HelpAboutToolStripMenuItem1.Text = "&About…";
			this.HelpAboutToolStripMenuItem1.Click += new System.EventHandler(this.HelpAboutToolStripMenuItem_Click);
			//
			// TournamentNameLabel
			//
			this.TournamentNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TournamentNameLabel.Location = new System.Drawing.Point(6, 24);
			this.TournamentNameLabel.Name = "TournamentNameLabel";
			this.TournamentNameLabel.Size = new System.Drawing.Size(437, 58);
			this.TournamentNameLabel.TabIndex = 7;
			this.TournamentNameLabel.Text = "DCM version yy.mm.dd\r\nStab You Soon!";
			this.TournamentNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// ButtonPanel
			//
			this.ButtonPanel.Controls.Add(this.RightButton);
			this.ButtonPanel.Controls.Add(this.MiddleButton);
			this.ButtonPanel.Controls.Add(this.LeftButton);
			this.ButtonPanel.Location = new System.Drawing.Point(10, 85);
			this.ButtonPanel.Name = "ButtonPanel";
			this.ButtonPanel.Size = new System.Drawing.Size(428, 44);
			this.ButtonPanel.TabIndex = 9;
			//
			// ScoresButton
			//
			this.RightButton.Location = new System.Drawing.Point(286, 1);
			this.RightButton.Margin = new System.Windows.Forms.Padding(2);
			this.RightButton.Name = "RightButton";
			this.RightButton.Size = new System.Drawing.Size(137, 41);
			this.RightButton.TabIndex = 11;
			this.RightButton.Text = "Scores";
			this.RightButton.UseVisualStyleBackColor = true;
			this.RightButton.Click += new System.EventHandler(this.RightButton_Click);
			//
			// RoundsButton
			//
			this.MiddleButton.Location = new System.Drawing.Point(145, 1);
			this.MiddleButton.Margin = new System.Windows.Forms.Padding(2);
			this.MiddleButton.Name = "MiddleButton";
			this.MiddleButton.Size = new System.Drawing.Size(137, 41);
			this.MiddleButton.TabIndex = 10;
			this.MiddleButton.Text = "Rounds";
			this.MiddleButton.UseVisualStyleBackColor = true;
			this.MiddleButton.Click += new System.EventHandler(this.MiddleButton_Click);
			//
			// DetailsButton
			//
			this.LeftButton.Location = new System.Drawing.Point(4, 1);
			this.LeftButton.Margin = new System.Windows.Forms.Padding(2);
			this.LeftButton.Name = "LeftButton";
			this.LeftButton.Size = new System.Drawing.Size(137, 41);
			this.LeftButton.TabIndex = 9;
			this.LeftButton.Text = "Details";
			this.LeftButton.UseVisualStyleBackColor = true;
			this.LeftButton.Click += new System.EventHandler(this.LeftButton_Click);
			//
			// MainForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(450, 140);
			this.Controls.Add(this.ButtonPanel);
			this.Controls.Add(this.TournamentNameLabel);
			this.Controls.Add(this.MainFormMenuStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Diplomacy Competition Manager";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.MainFormMenuStrip.ResumeLayout(false);
			this.MainFormMenuStrip.PerformLayout();
			this.ButtonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private ToolStripMenuItem TournamentToolStripMenuItem;
		private ToolStripMenuItem NewTournamentToolStripMenuItem;
		private ToolStripMenuItem OpenTournamentMenuItem;
		private ToolStripMenuItem DeleteTournamentMenuItem;
		private ToolStripMenuItem PlayersToolStripMenuItem;
		private ToolStripMenuItem ManagePlayersToolStripMenuItem;
		private ToolStripMenuItem ConfigurationMenuItem;
		private MenuStrip MainFormMenuStrip;
		private Label TournamentNameLabel;
		private ToolStripMenuItem PlayerConflictsToolStripMenuItem;
		private ToolStripMenuItem EmailConfigurationToolStripMenuItem;
		private ToolStripMenuItem GroupToolStripMenuItem;
		private ToolStripMenuItem OpenGroupMenuItem;
		private ToolStripMenuItem ReportsConfigurationToolStripMenuItem;
		private ToolStripMenuItem HelpToolStripMenuItem;
		private ToolStripMenuItem HelpAboutToolStripMenuItem1;
		private ToolStripMenuItem DatabaseConfigurationToolStripMenuItem;
        private ToolStripMenuItem HelpTopicsToolStripMenuItem;
		private ToolStripMenuItem ManageGroupsToolStripMenuItem;
		private ToolStripMenuItem ScoringToolStripMenuItem;
		private ToolStripMenuItem ScoringSystemsToolStripMenuItem;
		private ToolStripMenuItem OpenDatabaseToolStripMenuItem;
		private ToolStripMenuItem SaveAsDatabaseToolStripMenuItem;
		private ToolStripMenuItem ClearDatabaseToolStripMenuItem;
		private ToolStripMenuItem CheckDatabaseToolStripMenuItem;
		private ToolStripMenuItem ShowTimingToolStripMenuItem;
		private Panel ButtonPanel;
		private Button RightButton;
		private Button MiddleButton;
		private Button LeftButton;
	}
}
