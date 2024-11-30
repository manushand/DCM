namespace DCM.UI.Controls
{
	partial class TeamsControl
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.NewTeamGroupBox = new System.Windows.Forms.GroupBox();
			this.NewTeamNameTextBox = new System.Windows.Forms.TextBox();
			this.FormTeamButton = new System.Windows.Forms.Button();
			this.TeamNameLabel = new System.Windows.Forms.Label();
			this.TeamSizeLabel = new System.Windows.Forms.Label();
			this.DissolveButton = new System.Windows.Forms.Button();
			this.RenameButton = new System.Windows.Forms.Button();
			this.TeamsLabel = new System.Windows.Forms.Label();
			this.TeamListBox = new System.Windows.Forms.ListBox();
			this.OrderByPanel = new System.Windows.Forms.Panel();
			this.LastNameRadioButton = new System.Windows.Forms.RadioButton();
			this.FirstNameRadioButton = new System.Windows.Forms.RadioButton();
			this.OrderPlayersByLabel = new System.Windows.Forms.Label();
			this.JoinButton = new System.Windows.Forms.Button();
			this.MembersLabel = new System.Windows.Forms.Label();
			this.MemberListBox = new System.Windows.Forms.ListBox();
			this.NonMemberListBox = new System.Windows.Forms.ListBox();
			this.WhichPlayersTabControl = new System.Windows.Forms.TabControl();
			this.RegisteredPlayersTabPage = new System.Windows.Forms.TabPage();
			this.AllPlayersTabPage = new System.Windows.Forms.TabPage();
			this.NewTeamGroupBox.SuspendLayout();
			this.OrderByPanel.SuspendLayout();
			this.WhichPlayersTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// NewTeamGroupBox
			// 
			this.NewTeamGroupBox.Controls.Add(this.NewTeamNameTextBox);
			this.NewTeamGroupBox.Controls.Add(this.FormTeamButton);
			this.NewTeamGroupBox.Controls.Add(this.TeamNameLabel);
			this.NewTeamGroupBox.Location = new System.Drawing.Point(141, 293);
			this.NewTeamGroupBox.Name = "NewTeamGroupBox";
			this.NewTeamGroupBox.Size = new System.Drawing.Size(458, 54);
			this.NewTeamGroupBox.TabIndex = 64;
			this.NewTeamGroupBox.TabStop = false;
			this.NewTeamGroupBox.Text = "New Team";
			// 
			// NewTeamNameTextBox
			// 
			this.NewTeamNameTextBox.Location = new System.Drawing.Point(51, 19);
			this.NewTeamNameTextBox.Name = "NewTeamNameTextBox";
			this.NewTeamNameTextBox.Size = new System.Drawing.Size(262, 20);
			this.NewTeamNameTextBox.TabIndex = 4;
			this.NewTeamNameTextBox.Enter += new System.EventHandler(this.NewTeamNameTextBox_Enter);
			// 
			// FormTeamButton
			// 
			this.FormTeamButton.Location = new System.Drawing.Point(319, 18);
			this.FormTeamButton.Name = "FormTeamButton";
			this.FormTeamButton.Size = new System.Drawing.Size(124, 20);
			this.FormTeamButton.TabIndex = 7;
			this.FormTeamButton.Text = "Form Team";
			this.FormTeamButton.UseVisualStyleBackColor = true;
			this.FormTeamButton.Click += new System.EventHandler(this.FormTeamButton_Click);
			// 
			// TeamNameLabel
			// 
			this.TeamNameLabel.AutoSize = true;
			this.TeamNameLabel.Location = new System.Drawing.Point(7, 22);
			this.TeamNameLabel.Name = "TeamNameLabel";
			this.TeamNameLabel.Size = new System.Drawing.Size(38, 13);
			this.TeamNameLabel.TabIndex = 1;
			this.TeamNameLabel.Text = "Name:";
			// 
			// TeamSizeLabel
			// 
			this.TeamSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TeamSizeLabel.Location = new System.Drawing.Point(405, 30);
			this.TeamSizeLabel.Name = "TeamSizeLabel";
			this.TeamSizeLabel.Size = new System.Drawing.Size(102, 48);
			this.TeamSizeLabel.TabIndex = 63;
			this.TeamSizeLabel.Text = "Maximum\r\nTeam Size: 7";
			this.TeamSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DissolveButton
			// 
			this.DissolveButton.Location = new System.Drawing.Point(125, 239);
			this.DissolveButton.Name = "DissolveButton";
			this.DissolveButton.Size = new System.Drawing.Size(118, 23);
			this.DissolveButton.TabIndex = 62;
			this.DissolveButton.Text = "Dissolve…";
			this.DissolveButton.UseVisualStyleBackColor = true;
			this.DissolveButton.Click += new System.EventHandler(this.DissolveButton_Click);
			// 
			// RenameButton
			// 
			this.RenameButton.Location = new System.Drawing.Point(10, 239);
			this.RenameButton.Name = "RenameButton";
			this.RenameButton.Size = new System.Drawing.Size(110, 23);
			this.RenameButton.TabIndex = 61;
			this.RenameButton.Text = "Rename…";
			this.RenameButton.UseVisualStyleBackColor = true;
			this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
			// 
			// TeamsLabel
			// 
			this.TeamsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TeamsLabel.Location = new System.Drawing.Point(10, 9);
			this.TeamsLabel.Name = "TeamsLabel";
			this.TeamsLabel.Size = new System.Drawing.Size(233, 18);
			this.TeamsLabel.TabIndex = 60;
			this.TeamsLabel.Text = "Teams";
			this.TeamsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// TeamListBox
			// 
			this.TeamListBox.FormattingEnabled = true;
			this.TeamListBox.Location = new System.Drawing.Point(10, 30);
			this.TeamListBox.Name = "TeamListBox";
			this.TeamListBox.Size = new System.Drawing.Size(236, 199);
			this.TeamListBox.TabIndex = 59;
			this.TeamListBox.SelectedIndexChanged += new System.EventHandler(this.TeamListBox_SelectedIndexChanged);
			// 
			// OrderByPanel
			// 
			this.OrderByPanel.Controls.Add(this.LastNameRadioButton);
			this.OrderByPanel.Controls.Add(this.FirstNameRadioButton);
			this.OrderByPanel.Controls.Add(this.OrderPlayersByLabel);
			this.OrderByPanel.Location = new System.Drawing.Point(322, 240);
			this.OrderByPanel.Name = "OrderByPanel";
			this.OrderByPanel.Size = new System.Drawing.Size(245, 22);
			this.OrderByPanel.TabIndex = 58;
			// 
			// LastNameRadioButton
			// 
			this.LastNameRadioButton.AutoSize = true;
			this.LastNameRadioButton.Location = new System.Drawing.Point(168, 3);
			this.LastNameRadioButton.Name = "LastNameRadioButton";
			this.LastNameRadioButton.Size = new System.Drawing.Size(76, 17);
			this.LastNameRadioButton.TabIndex = 17;
			this.LastNameRadioButton.TabStop = true;
			this.LastNameRadioButton.Text = "Last Name";
			this.LastNameRadioButton.UseVisualStyleBackColor = true;
			this.LastNameRadioButton.Click += new System.EventHandler(this.FillMembershipLists);
			// 
			// FirstNameRadioButton
			// 
			this.FirstNameRadioButton.AutoSize = true;
			this.FirstNameRadioButton.Location = new System.Drawing.Point(92, 2);
			this.FirstNameRadioButton.Name = "FirstNameRadioButton";
			this.FirstNameRadioButton.Size = new System.Drawing.Size(75, 17);
			this.FirstNameRadioButton.TabIndex = 16;
			this.FirstNameRadioButton.TabStop = true;
			this.FirstNameRadioButton.Text = "First Name";
			this.FirstNameRadioButton.UseVisualStyleBackColor = true;
			this.FirstNameRadioButton.Click += new System.EventHandler(this.FillMembershipLists);
			// 
			// OrderPlayersByLabel
			// 
			this.OrderPlayersByLabel.AutoSize = true;
			this.OrderPlayersByLabel.Location = new System.Drawing.Point(2, 4);
			this.OrderPlayersByLabel.Name = "OrderPlayersByLabel";
			this.OrderPlayersByLabel.Size = new System.Drawing.Size(88, 13);
			this.OrderPlayersByLabel.TabIndex = 15;
			this.OrderPlayersByLabel.Text = "Order Players By:";
			// 
			// JoinButton
			// 
			this.JoinButton.Location = new System.Drawing.Point(405, 119);
			this.JoinButton.Name = "JoinButton";
			this.JoinButton.Size = new System.Drawing.Size(102, 23);
			this.JoinButton.TabIndex = 57;
			this.JoinButton.Text = "◀─── Join Team";
			this.JoinButton.UseVisualStyleBackColor = true;
			this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
			// 
			// MembersLabel
			// 
			this.MembersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MembersLabel.Location = new System.Drawing.Point(249, 9);
			this.MembersLabel.Name = "MembersLabel";
			this.MembersLabel.Size = new System.Drawing.Size(150, 18);
			this.MembersLabel.TabIndex = 56;
			this.MembersLabel.Text = "Members";
			this.MembersLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MemberListBox
			// 
			this.MemberListBox.FormattingEnabled = true;
			this.MemberListBox.Location = new System.Drawing.Point(249, 30);
			this.MemberListBox.Name = "MemberListBox";
			this.MemberListBox.Size = new System.Drawing.Size(152, 199);
			this.MemberListBox.TabIndex = 55;
			this.MemberListBox.SelectedIndexChanged += new System.EventHandler(this.MemberListBox_SelectedIndexChanged);
			this.MemberListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.JoinButton_Click);
			// 
			// NonMemberListBox
			// 
			this.NonMemberListBox.FormattingEnabled = true;
			this.NonMemberListBox.Location = new System.Drawing.Point(513, 30);
			this.NonMemberListBox.Name = "NonMemberListBox";
			this.NonMemberListBox.Size = new System.Drawing.Size(170, 199);
			this.NonMemberListBox.TabIndex = 54;
			this.NonMemberListBox.SelectedIndexChanged += new System.EventHandler(this.NonMemberListBox_SelectedIndexChanged);
			this.NonMemberListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RenameButton_Click);
			// 
			// WhichPlayersTabControl
			// 
			this.WhichPlayersTabControl.Controls.Add(this.RegisteredPlayersTabPage);
			this.WhichPlayersTabControl.Controls.Add(this.AllPlayersTabPage);
			this.WhichPlayersTabControl.Location = new System.Drawing.Point(512, 9);
			this.WhichPlayersTabControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.WhichPlayersTabControl.Name = "WhichPlayersTabControl";
			this.WhichPlayersTabControl.SelectedIndex = 0;
			this.WhichPlayersTabControl.Size = new System.Drawing.Size(172, 220);
			this.WhichPlayersTabControl.TabIndex = 65;
			this.WhichPlayersTabControl.SelectedIndexChanged += new System.EventHandler(this.FillMembershipLists);
			// 
			// RegisteredPlayersTabPage
			// 
			this.RegisteredPlayersTabPage.Location = new System.Drawing.Point(4, 22);
			this.RegisteredPlayersTabPage.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.RegisteredPlayersTabPage.Name = "RegisteredPlayersTabPage";
			this.RegisteredPlayersTabPage.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.RegisteredPlayersTabPage.Size = new System.Drawing.Size(164, 194);
			this.RegisteredPlayersTabPage.TabIndex = 0;
			this.RegisteredPlayersTabPage.Text = "Registered Players";
			this.RegisteredPlayersTabPage.UseVisualStyleBackColor = true;
			// 
			// AllPlayersTabPage
			// 
			this.AllPlayersTabPage.Location = new System.Drawing.Point(4, 22);
			this.AllPlayersTabPage.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.AllPlayersTabPage.Name = "AllPlayersTabPage";
			this.AllPlayersTabPage.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.AllPlayersTabPage.Size = new System.Drawing.Size(164, 198);
			this.AllPlayersTabPage.TabIndex = 1;
			this.AllPlayersTabPage.Text = "All Players";
			this.AllPlayersTabPage.UseVisualStyleBackColor = true;
			// 
			// TeamsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.NonMemberListBox);
			this.Controls.Add(this.NewTeamGroupBox);
			this.Controls.Add(this.TeamSizeLabel);
			this.Controls.Add(this.DissolveButton);
			this.Controls.Add(this.RenameButton);
			this.Controls.Add(this.TeamsLabel);
			this.Controls.Add(this.TeamListBox);
			this.Controls.Add(this.OrderByPanel);
			this.Controls.Add(this.JoinButton);
			this.Controls.Add(this.MembersLabel);
			this.Controls.Add(this.MemberListBox);
			this.Controls.Add(this.WhichPlayersTabControl);
			this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.Name = "TeamsControl";
			this.Size = new System.Drawing.Size(696, 361);
			this.NewTeamGroupBox.ResumeLayout(false);
			this.NewTeamGroupBox.PerformLayout();
			this.OrderByPanel.ResumeLayout(false);
			this.OrderByPanel.PerformLayout();
			this.WhichPlayersTabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private GroupBox NewTeamGroupBox;
		private TextBox NewTeamNameTextBox;
		private Button FormTeamButton;
		private Label TeamNameLabel;
		private Label TeamSizeLabel;
		private Button DissolveButton;
		private Button RenameButton;
		private Label TeamsLabel;
		private ListBox TeamListBox;
		private Panel OrderByPanel;
		private RadioButton LastNameRadioButton;
		private RadioButton FirstNameRadioButton;
		private Label OrderPlayersByLabel;
		private Button JoinButton;
		private Label MembersLabel;
		private ListBox MemberListBox;
		private ListBox NonMemberListBox;
		private TabControl WhichPlayersTabControl;
		private TabPage RegisteredPlayersTabPage;
		private TabPage AllPlayersTabPage;
	}
}
