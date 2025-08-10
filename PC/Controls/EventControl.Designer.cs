namespace PC.Controls
{
	partial class EventControl
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
			this.components = new System.ComponentModel.Container();
			this.DescriptionTextBox = new System.Windows.Forms.RichTextBox();
			this.CopyButton = new System.Windows.Forms.Button();
			this.ScalePercentLabel = new System.Windows.Forms.Label();
			this.ScaleCheckBox = new System.Windows.Forms.CheckBox();
			this.DropCheckBox = new System.Windows.Forms.CheckBox();
			this.DropWhenComboBox = new System.Windows.Forms.ComboBox();
			this.CancelFormButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.TeamDetailsGroupBox = new System.Windows.Forms.GroupBox();
			this.MultiTeamMembershipCheckBox = new System.Windows.Forms.CheckBox();
			this.TeamRoundComboBox = new System.Windows.Forms.ComboBox();
			this.TeamSizeComboBox = new System.Windows.Forms.ComboBox();
			this.TeamTournamentLabel = new System.Windows.Forms.Label();
			this.TeamRoundInfoLabel = new System.Windows.Forms.Label();
			this.TeamMemberConflictLabel = new System.Windows.Forms.Label();
			this.TeamScoringComboBox = new System.Windows.Forms.ComboBox();
			this.TeamMemberConflictTextBox = new System.Windows.Forms.TextBox();
			this.TeamScoringLabel = new System.Windows.Forms.Label();
			this.ScaleFactorTextBox = new System.Windows.Forms.TextBox();
			this.RoundsToScaleLabel = new System.Windows.Forms.Label();
			this.RoundsToScaleComboBox = new System.Windows.Forms.ComboBox();
			this.UnplayedScoreTextBox = new System.Windows.Forms.TextBox();
			this.UnplayedScoreLabel = new System.Windows.Forms.Label();
			this.RoundsToDropLabel = new System.Windows.Forms.Label();
			this.RoundsToDropComboBox = new System.Windows.Forms.ComboBox();
			this.MinimumToQualifyLabel = new System.Windows.Forms.Label();
			this.MinimumRoundsComboBox = new System.Windows.Forms.ComboBox();
			this.MinimumRoundsLabel = new System.Windows.Forms.Label();
			this.TotalRoundsComboBox = new System.Windows.Forms.ComboBox();
			this.TotalRoundsLabel = new System.Windows.Forms.Label();
			this.PowerConflictTextBox = new System.Windows.Forms.TextBox();
			this.RepeatingPowerLabel = new System.Windows.Forms.Label();
			this.PlayerConflictTextBox = new System.Windows.Forms.TextBox();
			this.PlayerConflictLabel = new System.Windows.Forms.Label();
			this.PowerGroupComboBox = new System.Windows.Forms.ComboBox();
			this.PowerGroupLabel = new System.Windows.Forms.Label();
			this.PowerAssignmentComboBox = new System.Windows.Forms.ComboBox();
			this.PowerAssignmentLabel = new System.Windows.Forms.Label();
			this.ScoringSystemComboBox = new System.Windows.Forms.ComboBox();
			this.ScoringSystemLabel = new System.Windows.Forms.Label();
			this.DescriptionLabel = new System.Windows.Forms.Label();
			this.NameTextBox = new System.Windows.Forms.TextBox();
			this.TournamentNameLabel = new System.Windows.Forms.Label();
			this.ScoreConflictTextBox = new System.Windows.Forms.TextBox();
			this.ProgressiveScoreConflictCheckBox = new System.Windows.Forms.CheckBox();
			this.ScoreConflictLabel = new System.Windows.Forms.Label();
			this.ScoreConflictCheckBox = new System.Windows.Forms.CheckBox();
			this.PlayerConflictPointsLabel = new System.Windows.Forms.Label();
			this.PowerConflictPointsLabel = new System.Windows.Forms.Label();
			this.DateLabel = new System.Windows.Forms.Label();
			this.DateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.TeamDetailsGroupBox.SuspendLayout();
			this.SuspendLayout();
			//
			// DescriptionTextBox
			//
			this.DescriptionTextBox.Location = new System.Drawing.Point(12, 65);
			this.DescriptionTextBox.Name = "DescriptionTextBox";
			this.DescriptionTextBox.Size = new System.Drawing.Size(351, 149);
			this.DescriptionTextBox.TabIndex = 115;
			this.DescriptionTextBox.Text = "";
			//
			// CopyButton
			//
			this.CopyButton.Location = new System.Drawing.Point(296, 12);
			this.CopyButton.Name = "CopyButton";
			this.CopyButton.Size = new System.Drawing.Size(66, 22);
			this.CopyButton.TabIndex = 114;
			this.CopyButton.Text = "Copy";
			this.CopyButton.UseVisualStyleBackColor = true;
			this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
			//
			// ScalePercentLabel
			//
			this.ScalePercentLabel.AutoSize = true;
			this.ScalePercentLabel.Location = new System.Drawing.Point(307, 312);
			this.ScalePercentLabel.Name = "ScalePercentLabel";
			this.ScalePercentLabel.Size = new System.Drawing.Size(43, 13);
			this.ScalePercentLabel.TabIndex = 113;
			this.ScalePercentLabel.Text = "percent";
			//
			// ScaleCheckBox
			//
			this.ScaleCheckBox.AutoSize = true;
			this.ScaleCheckBox.Location = new System.Drawing.Point(16, 311);
			this.ScaleCheckBox.Name = "ScaleCheckBox";
			this.ScaleCheckBox.Size = new System.Drawing.Size(109, 17);
			this.ScaleCheckBox.TabIndex = 112;
			this.ScaleCheckBox.Text = "Scale next lowest";
			this.ScaleCheckBox.UseVisualStyleBackColor = true;
			this.ScaleCheckBox.CheckedChanged += new System.EventHandler(this.ScaleCheckBox_CheckedChanged);
			//
			// DropCheckBox
			//
			this.DropCheckBox.AutoSize = true;
			this.DropCheckBox.Location = new System.Drawing.Point(16, 284);
			this.DropCheckBox.Name = "DropCheckBox";
			this.DropCheckBox.Size = new System.Drawing.Size(100, 17);
			this.DropCheckBox.TabIndex = 111;
			this.DropCheckBox.Text = "Drop the lowest";
			this.DropCheckBox.UseVisualStyleBackColor = true;
			this.DropCheckBox.CheckedChanged += new System.EventHandler(this.DropCheckBox_CheckedChanged);
			//
			// DropWhenComboBox
			//
			this.DropWhenComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DropWhenComboBox.FormattingEnabled = true;
			this.DropWhenComboBox.Items.AddRange(new object[] {
            "Only After Final Round",
            "Before and After Final Round"});
			this.DropWhenComboBox.Location = new System.Drawing.Point(201, 282);
			this.DropWhenComboBox.Name = "DropWhenComboBox";
			this.DropWhenComboBox.Size = new System.Drawing.Size(162, 21);
			this.DropWhenComboBox.TabIndex = 110;
			this.DropWhenComboBox.SelectedIndexChanged += new System.EventHandler(this.DropWhenComboBox_SelectedIndexChanged);
			//
			// CancelFormButton
			//
			this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelFormButton.Location = new System.Drawing.Point(369, 346);
			this.CancelFormButton.Name = "CancelFormButton";
			this.CancelFormButton.Size = new System.Drawing.Size(125, 23);
			this.CancelFormButton.TabIndex = 109;
			this.CancelFormButton.Text = "Cancel";
			this.CancelFormButton.UseVisualStyleBackColor = true;
			//
			// OkButton
			//
			this.OkButton.Location = new System.Drawing.Point(238, 346);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(125, 23);
			this.OkButton.TabIndex = 108;
			this.OkButton.Text = " OK";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			//
			// TeamDetailsGroupBox
			//
			this.TeamDetailsGroupBox.Controls.Add(this.MultiTeamMembershipCheckBox);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamRoundComboBox);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamSizeComboBox);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamTournamentLabel);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamRoundInfoLabel);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamMemberConflictLabel);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamScoringComboBox);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamMemberConflictTextBox);
			this.TeamDetailsGroupBox.Controls.Add(this.TeamScoringLabel);
			this.TeamDetailsGroupBox.Location = new System.Drawing.Point(369, 175);
			this.TeamDetailsGroupBox.Name = "TeamDetailsGroupBox";
			this.TeamDetailsGroupBox.Size = new System.Drawing.Size(309, 164);
			this.TeamDetailsGroupBox.TabIndex = 107;
			this.TeamDetailsGroupBox.TabStop = false;
			this.TeamDetailsGroupBox.Text = "Team Details";
			//
			// MultiTeamMembershipCheckBox
			//
			this.MultiTeamMembershipCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.MultiTeamMembershipCheckBox.Location = new System.Drawing.Point(8, 136);
			this.MultiTeamMembershipCheckBox.Name = "MultiTeamMembershipCheckBox";
			this.MultiTeamMembershipCheckBox.Size = new System.Drawing.Size(280, 17);
			this.MultiTeamMembershipCheckBox.TabIndex = 35;
			this.MultiTeamMembershipCheckBox.Text = "Allow Players to Join and Play for Multiple Teams?";
			this.MultiTeamMembershipCheckBox.UseVisualStyleBackColor = true;
			//
			// TeamRoundComboBox
			//
			this.TeamRoundComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TeamRoundComboBox.FormattingEnabled = true;
			this.TeamRoundComboBox.Location = new System.Drawing.Point(245, 107);
			this.TeamRoundComboBox.Name = "TeamRoundComboBox";
			this.TeamRoundComboBox.Size = new System.Drawing.Size(46, 21);
			this.TeamRoundComboBox.TabIndex = 34;
			this.TeamRoundComboBox.SelectedIndexChanged += new System.EventHandler(this.TeamRoundComboBox_SelectedIndexChanged);
			//
			// TeamSizeComboBox
			//
			this.TeamSizeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TeamSizeComboBox.FormattingEnabled = true;
			this.TeamSizeComboBox.Items.AddRange(new object[] {
            "No Team Tournament",
            "Three-Member Teams",
            "Four-Member Teams",
            "Five-Member Teams",
            "Six-Member Teams",
            "Seven-Member Teams"});
			this.TeamSizeComboBox.Location = new System.Drawing.Point(110, 23);
			this.TeamSizeComboBox.Name = "TeamSizeComboBox";
			this.TeamSizeComboBox.Size = new System.Drawing.Size(181, 21);
			this.TeamSizeComboBox.TabIndex = 1;
			this.TeamSizeComboBox.SelectedIndexChanged += new System.EventHandler(this.TeamSizeComboBox_SelectedIndexChanged);
			//
			// TeamTournamentLabel
			//
			this.TeamTournamentLabel.AutoSize = true;
			this.TeamTournamentLabel.Location = new System.Drawing.Point(7, 26);
			this.TeamTournamentLabel.Name = "TeamTournamentLabel";
			this.TeamTournamentLabel.Size = new System.Drawing.Size(97, 13);
			this.TeamTournamentLabel.TabIndex = 0;
			this.TeamTournamentLabel.Text = "Team Tournament:";
			//
			// TeamRoundInfoLabel
			//
			this.TeamRoundInfoLabel.AutoSize = true;
			this.TeamRoundInfoLabel.Location = new System.Drawing.Point(7, 110);
			this.TeamRoundInfoLabel.Name = "TeamRoundInfoLabel";
			this.TeamRoundInfoLabel.Size = new System.Drawing.Size(200, 13);
			this.TeamRoundInfoLabel.TabIndex = 4;
			this.TeamRoundInfoLabel.Text = "Round Number to Use for Team Scoring:";
			//
			// TeamMemberConflictLabel
			//
			this.TeamMemberConflictLabel.AutoSize = true;
			this.TeamMemberConflictLabel.Location = new System.Drawing.Point(7, 54);
			this.TeamMemberConflictLabel.Name = "TeamMemberConflictLabel";
			this.TeamMemberConflictLabel.Size = new System.Drawing.Size(242, 13);
			this.TeamMemberConflictLabel.TabIndex = 16;
			this.TeamMemberConflictLabel.Text = "Conflict for playing in a game with a team member:";
			//
			// TeamScoringComboBox
			//
			this.TeamScoringComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TeamScoringComboBox.FormattingEnabled = true;
			this.TeamScoringComboBox.Items.AddRange(new object[] {
            "Teams Score in a Single Round",
            "Sum Games Without Interplay"});
			this.TeamScoringComboBox.Location = new System.Drawing.Point(110, 79);
			this.TeamScoringComboBox.Name = "TeamScoringComboBox";
			this.TeamScoringComboBox.Size = new System.Drawing.Size(181, 21);
			this.TeamScoringComboBox.TabIndex = 3;
			this.TeamScoringComboBox.SelectedIndexChanged += new System.EventHandler(this.TeamScoringComboBox_SelectedIndexChanged);
			//
			// TeamMemberConflictTextBox
			//
			this.TeamMemberConflictTextBox.Location = new System.Drawing.Point(263, 51);
			this.TeamMemberConflictTextBox.Name = "TeamMemberConflictTextBox";
			this.TeamMemberConflictTextBox.Size = new System.Drawing.Size(28, 20);
			this.TeamMemberConflictTextBox.TabIndex = 17;
			this.TeamMemberConflictTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			// TeamScoringLabel
			//
			this.TeamScoringLabel.AutoSize = true;
			this.TeamScoringLabel.Location = new System.Drawing.Point(7, 82);
			this.TeamScoringLabel.Name = "TeamScoringLabel";
			this.TeamScoringLabel.Size = new System.Drawing.Size(76, 13);
			this.TeamScoringLabel.TabIndex = 2;
			this.TeamScoringLabel.Text = "Team Scoring:";
			//
			// ScaleFactorTextBox
			//
			this.ScaleFactorTextBox.Location = new System.Drawing.Point(259, 308);
			this.ScaleFactorTextBox.Name = "ScaleFactorTextBox";
			this.ScaleFactorTextBox.Size = new System.Drawing.Size(43, 20);
			this.ScaleFactorTextBox.TabIndex = 106;
			this.ScaleFactorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			// RoundsToScaleLabel
			//
			this.RoundsToScaleLabel.AutoSize = true;
			this.RoundsToScaleLabel.Location = new System.Drawing.Point(162, 312);
			this.RoundsToScaleLabel.Name = "RoundsToScaleLabel";
			this.RoundsToScaleLabel.Size = new System.Drawing.Size(88, 13);
			this.RoundsToScaleLabel.TabIndex = 105;
			this.RoundsToScaleLabel.Text = "round score(s) by";
			//
			// RoundsToScaleComboBox
			//
			this.RoundsToScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RoundsToScaleComboBox.FormattingEnabled = true;
			this.RoundsToScaleComboBox.Location = new System.Drawing.Point(127, 308);
			this.RoundsToScaleComboBox.Name = "RoundsToScaleComboBox";
			this.RoundsToScaleComboBox.Size = new System.Drawing.Size(35, 21);
			this.RoundsToScaleComboBox.TabIndex = 104;
			this.RoundsToScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.RoundsToScaleComboBox_SelectedIndexChanged);
			//
			// UnplayedScoreTextBox
			//
			this.UnplayedScoreTextBox.Location = new System.Drawing.Point(320, 226);
			this.UnplayedScoreTextBox.Name = "UnplayedScoreTextBox";
			this.UnplayedScoreTextBox.Size = new System.Drawing.Size(42, 20);
			this.UnplayedScoreTextBox.TabIndex = 103;
			this.UnplayedScoreTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			// UnplayedScoreLabel
			//
			this.UnplayedScoreLabel.AutoSize = true;
			this.UnplayedScoreLabel.Location = new System.Drawing.Point(187, 229);
			this.UnplayedScoreLabel.Name = "UnplayedScoreLabel";
			this.UnplayedScoreLabel.Size = new System.Drawing.Size(129, 13);
			this.UnplayedScoreLabel.TabIndex = 102;
			this.UnplayedScoreLabel.Text = "Score for unplayed round:";
			//
			// RoundsToDropLabel
			//
			this.RoundsToDropLabel.AutoSize = true;
			this.RoundsToDropLabel.Location = new System.Drawing.Point(162, 285);
			this.RoundsToDropLabel.Name = "RoundsToDropLabel";
			this.RoundsToDropLabel.Size = new System.Drawing.Size(38, 13);
			this.RoundsToDropLabel.TabIndex = 101;
			this.RoundsToDropLabel.Text = "scores";
			//
			// RoundsToDropComboBox
			//
			this.RoundsToDropComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RoundsToDropComboBox.FormattingEnabled = true;
			this.RoundsToDropComboBox.Location = new System.Drawing.Point(127, 282);
			this.RoundsToDropComboBox.Name = "RoundsToDropComboBox";
			this.RoundsToDropComboBox.Size = new System.Drawing.Size(35, 21);
			this.RoundsToDropComboBox.TabIndex = 100;
			this.RoundsToDropComboBox.SelectedIndexChanged += new System.EventHandler(this.RoundsToDropComboBox_SelectedIndexChanged);
			//
			// MinimumToQualifyLabel
			//
			this.MinimumToQualifyLabel.AutoSize = true;
			this.MinimumToQualifyLabel.Location = new System.Drawing.Point(162, 257);
			this.MinimumToQualifyLabel.Name = "MinimumToQualifyLabel";
			this.MinimumToQualifyLabel.Size = new System.Drawing.Size(175, 13);
			this.MinimumToQualifyLabel.TabIndex = 99;
			this.MinimumToQualifyLabel.Text = "or more rounds to qualify for ranking";
			//
			// MinimumRoundsComboBox
			//
			this.MinimumRoundsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MinimumRoundsComboBox.FormattingEnabled = true;
			this.MinimumRoundsComboBox.Location = new System.Drawing.Point(127, 254);
			this.MinimumRoundsComboBox.Name = "MinimumRoundsComboBox";
			this.MinimumRoundsComboBox.Size = new System.Drawing.Size(35, 21);
			this.MinimumRoundsComboBox.TabIndex = 98;
			this.MinimumRoundsComboBox.SelectedIndexChanged += new System.EventHandler(this.MinimumRoundsComboBox_SelectedIndexChanged);
			//
			// MinimumRoundsLabel
			//
			this.MinimumRoundsLabel.AutoSize = true;
			this.MinimumRoundsLabel.Location = new System.Drawing.Point(13, 257);
			this.MinimumRoundsLabel.Name = "MinimumRoundsLabel";
			this.MinimumRoundsLabel.Size = new System.Drawing.Size(94, 13);
			this.MinimumRoundsLabel.TabIndex = 97;
			this.MinimumRoundsLabel.Text = "Player must play in";
			//
			// TotalRoundsComboBox
			//
			this.TotalRoundsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TotalRoundsComboBox.FormattingEnabled = true;
			this.TotalRoundsComboBox.Location = new System.Drawing.Point(127, 226);
			this.TotalRoundsComboBox.Name = "TotalRoundsComboBox";
			this.TotalRoundsComboBox.Size = new System.Drawing.Size(35, 21);
			this.TotalRoundsComboBox.TabIndex = 96;
			this.TotalRoundsComboBox.SelectedIndexChanged += new System.EventHandler(this.TotalRoundsComboBox_SelectedIndexChanged);
			//
			// TotalRoundsLabel
			//
			this.TotalRoundsLabel.AutoSize = true;
			this.TotalRoundsLabel.Location = new System.Drawing.Point(13, 229);
			this.TotalRoundsLabel.Name = "TotalRoundsLabel";
			this.TotalRoundsLabel.Size = new System.Drawing.Size(94, 13);
			this.TotalRoundsLabel.TabIndex = 95;
			this.TotalRoundsLabel.Text = "Number of rounds:";
			//
			// PowerConflictTextBox
			//
			this.PowerConflictTextBox.Location = new System.Drawing.Point(632, 147);
			this.PowerConflictTextBox.Name = "PowerConflictTextBox";
			this.PowerConflictTextBox.Size = new System.Drawing.Size(28, 20);
			this.PowerConflictTextBox.TabIndex = 94;
			this.PowerConflictTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			// RepeatingPowerLabel
			//
			this.RepeatingPowerLabel.AutoSize = true;
			this.RepeatingPowerLabel.Location = new System.Drawing.Point(376, 150);
			this.RepeatingPowerLabel.Name = "RepeatingPowerLabel";
			this.RepeatingPowerLabel.Size = new System.Drawing.Size(255, 13);
			this.RepeatingPowerLabel.TabIndex = 93;
			this.RepeatingPowerLabel.Text = "Conflict for playing in a power group more than once:";
			//
			// PlayerConflictTextBox
			//
			this.PlayerConflictTextBox.Location = new System.Drawing.Point(632, 121);
			this.PlayerConflictTextBox.Name = "PlayerConflictTextBox";
			this.PlayerConflictTextBox.Size = new System.Drawing.Size(28, 20);
			this.PlayerConflictTextBox.TabIndex = 92;
			this.PlayerConflictTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			// PlayerConflictLabel
			//
			this.PlayerConflictLabel.AutoSize = true;
			this.PlayerConflictLabel.Location = new System.Drawing.Point(376, 124);
			this.PlayerConflictLabel.Name = "PlayerConflictLabel";
			this.PlayerConflictLabel.Size = new System.Drawing.Size(250, 13);
			this.PlayerConflictLabel.TabIndex = 91;
			this.PlayerConflictLabel.Text = "Conflict for playing the same player more than once:";
			//
			// PowerGroupComboBox
			//
			this.PowerGroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PowerGroupComboBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PowerGroupComboBox.FormattingEnabled = true;
			this.PowerGroupComboBox.Location = new System.Drawing.Point(479, 65);
			this.PowerGroupComboBox.Name = "PowerGroupComboBox";
			this.PowerGroupComboBox.Size = new System.Drawing.Size(199, 22);
			this.PowerGroupComboBox.TabIndex = 90;
			this.PowerGroupComboBox.SelectedIndexChanged += new System.EventHandler(this.PowerGroupComboBox_SelectedIndexChanged);
			//
			// PowerGroupLabel
			//
			this.PowerGroupLabel.AutoSize = true;
			this.PowerGroupLabel.Location = new System.Drawing.Point(376, 68);
			this.PowerGroupLabel.Name = "PowerGroupLabel";
			this.PowerGroupLabel.Size = new System.Drawing.Size(77, 13);
			this.PowerGroupLabel.TabIndex = 89;
			this.PowerGroupLabel.Text = "Power Groups:";
			//
			// PowerAssignmentComboBox
			//
			this.PowerAssignmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PowerAssignmentComboBox.FormattingEnabled = true;
			this.PowerAssignmentComboBox.Items.AddRange(new object[] {
            "Automatic at Seeding",
            "Determined at Tables"});
			this.PowerAssignmentComboBox.Location = new System.Drawing.Point(479, 39);
			this.PowerAssignmentComboBox.Name = "PowerAssignmentComboBox";
			this.PowerAssignmentComboBox.Size = new System.Drawing.Size(199, 21);
			this.PowerAssignmentComboBox.TabIndex = 88;
			this.PowerAssignmentComboBox.SelectedIndexChanged += new System.EventHandler(this.PowerAssignmentComboBox_SelectedIndexChanged);
			//
			// PowerAssignmentLabel
			//
			this.PowerAssignmentLabel.AutoSize = true;
			this.PowerAssignmentLabel.Location = new System.Drawing.Point(376, 42);
			this.PowerAssignmentLabel.Name = "PowerAssignmentLabel";
			this.PowerAssignmentLabel.Size = new System.Drawing.Size(97, 13);
			this.PowerAssignmentLabel.TabIndex = 87;
			this.PowerAssignmentLabel.Text = "Power Assignment:";
			//
			// ScoringSystemComboBox
			//
			this.ScoringSystemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ScoringSystemComboBox.FormattingEnabled = true;
			this.ScoringSystemComboBox.Location = new System.Drawing.Point(479, 13);
			this.ScoringSystemComboBox.Name = "ScoringSystemComboBox";
			this.ScoringSystemComboBox.Size = new System.Drawing.Size(199, 21);
			this.ScoringSystemComboBox.TabIndex = 85;
			this.ScoringSystemComboBox.SelectedIndexChanged += new System.EventHandler(this.ScoringSystemComboBox_SelectedIndexChanged);
			//
			// ScoringSystemLabel
			//
			this.ScoringSystemLabel.AutoSize = true;
			this.ScoringSystemLabel.Location = new System.Drawing.Point(376, 16);
			this.ScoringSystemLabel.Name = "ScoringSystemLabel";
			this.ScoringSystemLabel.Size = new System.Drawing.Size(83, 13);
			this.ScoringSystemLabel.TabIndex = 84;
			this.ScoringSystemLabel.Text = "Scoring System:";
			//
			// DescriptionLabel
			//
			this.DescriptionLabel.AutoSize = true;
			this.DescriptionLabel.Location = new System.Drawing.Point(13, 42);
			this.DescriptionLabel.Name = "DescriptionLabel";
			this.DescriptionLabel.Size = new System.Drawing.Size(63, 13);
			this.DescriptionLabel.TabIndex = 83;
			this.DescriptionLabel.Text = "Description:";
			//
			// NameTextBox
			//
			this.NameTextBox.Location = new System.Drawing.Point(53, 13);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(241, 20);
			this.NameTextBox.TabIndex = 82;
			//
			// TournamentNameLabel
			//
			this.TournamentNameLabel.AutoSize = true;
			this.TournamentNameLabel.Location = new System.Drawing.Point(13, 17);
			this.TournamentNameLabel.Name = "TournamentNameLabel";
			this.TournamentNameLabel.Size = new System.Drawing.Size(38, 13);
			this.TournamentNameLabel.TabIndex = 81;
			this.TournamentNameLabel.Text = "Name:";
			//
			// ScoreConflictTextBox
			//
			this.ScoreConflictTextBox.Location = new System.Drawing.Point(490, 96);
			this.ScoreConflictTextBox.Name = "ScoreConflictTextBox";
			this.ScoreConflictTextBox.Size = new System.Drawing.Size(30, 20);
			this.ScoreConflictTextBox.TabIndex = 117;
			//
			// ProgressiveScoreConflictCheckBox
			//
			this.ProgressiveScoreConflictCheckBox.AutoSize = true;
			this.ProgressiveScoreConflictCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ProgressiveScoreConflictCheckBox.Location = new System.Drawing.Point(592, 98);
			this.ProgressiveScoreConflictCheckBox.Name = "ProgressiveScoreConflictCheckBox";
			this.ProgressiveScoreConflictCheckBox.Size = new System.Drawing.Size(87, 17);
			this.ProgressiveScoreConflictCheckBox.TabIndex = 118;
			this.ProgressiveScoreConflictCheckBox.Text = "Progressive?";
			this.ProgressiveScoreConflictCheckBox.UseVisualStyleBackColor = true;
			//
			// ScoreConflictLabel
			//
			this.ScoreConflictLabel.AutoSize = true;
			this.ScoreConflictLabel.Location = new System.Drawing.Point(524, 99);
			this.ScoreConflictLabel.Name = "ScoreConflictLabel";
			this.ScoreConflictLabel.Size = new System.Drawing.Size(71, 13);
			this.ScoreConflictLabel.TabIndex = 119;
			this.ScoreConflictLabel.Text = "pts. from avg.";
			//
			// ScoreConflictCheckBox
			//
			this.ScoreConflictCheckBox.AutoSize = true;
			this.ScoreConflictCheckBox.Location = new System.Drawing.Point(379, 98);
			this.ScoreConflictCheckBox.Name = "ScoreConflictCheckBox";
			this.ScoreConflictCheckBox.Size = new System.Drawing.Size(111, 17);
			this.ScoreConflictCheckBox.TabIndex = 120;
			this.ScoreConflictCheckBox.Text = "1 conflict pt. each";
			this.ScoreConflictCheckBox.UseVisualStyleBackColor = true;
			this.ScoreConflictCheckBox.CheckedChanged += new System.EventHandler(this.ScoreConflictCheckBox_CheckedChanged);
			//
			// PlayerConflictPointsLabel
			//
			this.PlayerConflictPointsLabel.AutoSize = true;
			this.PlayerConflictPointsLabel.Location = new System.Drawing.Point(661, 124);
			this.PlayerConflictPointsLabel.Name = "PlayerConflictPointsLabel";
			this.PlayerConflictPointsLabel.Size = new System.Drawing.Size(24, 13);
			this.PlayerConflictPointsLabel.TabIndex = 121;
			this.PlayerConflictPointsLabel.Text = "pts.";
			//
			// PowerConflictPointsLabel
			//
			this.PowerConflictPointsLabel.AutoSize = true;
			this.PowerConflictPointsLabel.Location = new System.Drawing.Point(661, 150);
			this.PowerConflictPointsLabel.Name = "PowerConflictPointsLabel";
			this.PowerConflictPointsLabel.Size = new System.Drawing.Size(24, 13);
			this.PowerConflictPointsLabel.TabIndex = 122;
			this.PowerConflictPointsLabel.Text = "pts.";
			//
			// DateLabel
			//
			this.DateLabel.AutoSize = true;
			this.DateLabel.Location = new System.Drawing.Point(245, 42);
			this.DateLabel.Name = "DateLabel";
			this.DateLabel.Size = new System.Drawing.Size(33, 13);
			this.DateLabel.TabIndex = 123;
			this.DateLabel.Text = "Date:";
			//
			// DateTimePicker
			//
			this.DateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.DateTimePicker.Location = new System.Drawing.Point(279, 39);
			this.DateTimePicker.Name = "DateTimePicker";
			this.DateTimePicker.Size = new System.Drawing.Size(83, 20);
			this.DateTimePicker.TabIndex = 124;
			//
			// TournamentControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.DateTimePicker);
			this.Controls.Add(this.DateLabel);
			this.Controls.Add(this.PowerConflictPointsLabel);
			this.Controls.Add(this.PlayerConflictPointsLabel);
			this.Controls.Add(this.ScoreConflictCheckBox);
			this.Controls.Add(this.ScoreConflictLabel);
			this.Controls.Add(this.ProgressiveScoreConflictCheckBox);
			this.Controls.Add(this.ScoreConflictTextBox);
			this.Controls.Add(this.DescriptionTextBox);
			this.Controls.Add(this.CopyButton);
			this.Controls.Add(this.ScalePercentLabel);
			this.Controls.Add(this.ScaleCheckBox);
			this.Controls.Add(this.DropCheckBox);
			this.Controls.Add(this.DropWhenComboBox);
			this.Controls.Add(this.CancelFormButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.TeamDetailsGroupBox);
			this.Controls.Add(this.ScaleFactorTextBox);
			this.Controls.Add(this.RoundsToScaleLabel);
			this.Controls.Add(this.RoundsToScaleComboBox);
			this.Controls.Add(this.UnplayedScoreTextBox);
			this.Controls.Add(this.UnplayedScoreLabel);
			this.Controls.Add(this.RoundsToDropLabel);
			this.Controls.Add(this.RoundsToDropComboBox);
			this.Controls.Add(this.MinimumToQualifyLabel);
			this.Controls.Add(this.MinimumRoundsComboBox);
			this.Controls.Add(this.MinimumRoundsLabel);
			this.Controls.Add(this.TotalRoundsComboBox);
			this.Controls.Add(this.TotalRoundsLabel);
			this.Controls.Add(this.PowerConflictTextBox);
			this.Controls.Add(this.RepeatingPowerLabel);
			this.Controls.Add(this.PlayerConflictTextBox);
			this.Controls.Add(this.PlayerConflictLabel);
			this.Controls.Add(this.PowerGroupComboBox);
			this.Controls.Add(this.PowerGroupLabel);
			this.Controls.Add(this.PowerAssignmentComboBox);
			this.Controls.Add(this.PowerAssignmentLabel);
			this.Controls.Add(this.ScoringSystemComboBox);
			this.Controls.Add(this.ScoringSystemLabel);
			this.Controls.Add(this.DescriptionLabel);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.TournamentNameLabel);
			this.Margin = new System.Windows.Forms.Padding(1);
			this.Name = "EventControl";
			this.Size = new System.Drawing.Size(691, 384);
			this.Load += new System.EventHandler(this.TournamentControl_Load);
			this.TeamDetailsGroupBox.ResumeLayout(false);
			this.TeamDetailsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RichTextBox DescriptionTextBox;
		private Button CopyButton;
		private Label ScalePercentLabel;
		private CheckBox ScaleCheckBox;
		private CheckBox DropCheckBox;
		private ComboBox DropWhenComboBox;
		private Button CancelFormButton;
		private Button OkButton;
		private GroupBox TeamDetailsGroupBox;
		private CheckBox MultiTeamMembershipCheckBox;
		private ComboBox TeamRoundComboBox;
		private ComboBox TeamSizeComboBox;
		private Label TeamTournamentLabel;
		private Label TeamRoundInfoLabel;
		private Label TeamMemberConflictLabel;
		private ComboBox TeamScoringComboBox;
		private TextBox TeamMemberConflictTextBox;
		private Label TeamScoringLabel;
		private TextBox ScaleFactorTextBox;
		private Label RoundsToScaleLabel;
		private ComboBox RoundsToScaleComboBox;
		private TextBox UnplayedScoreTextBox;
		private Label UnplayedScoreLabel;
		private Label RoundsToDropLabel;
		private ComboBox RoundsToDropComboBox;
		private Label MinimumToQualifyLabel;
		private ComboBox MinimumRoundsComboBox;
		private Label MinimumRoundsLabel;
		private ComboBox TotalRoundsComboBox;
		private Label TotalRoundsLabel;
		private TextBox PowerConflictTextBox;
		private Label RepeatingPowerLabel;
		private TextBox PlayerConflictTextBox;
		private Label PlayerConflictLabel;
		private ComboBox PowerGroupComboBox;
		private Label PowerGroupLabel;
		private ComboBox PowerAssignmentComboBox;
		private Label PowerAssignmentLabel;
		private ComboBox ScoringSystemComboBox;
		private Label ScoringSystemLabel;
		private Label DescriptionLabel;
		private TextBox NameTextBox;
		private Label TournamentNameLabel;
		private TextBox ScoreConflictTextBox;
		private CheckBox ProgressiveScoreConflictCheckBox;
		private Label ScoreConflictLabel;
		private CheckBox ScoreConflictCheckBox;
		private Label PlayerConflictPointsLabel;
		private Label PowerConflictPointsLabel;
		private Label DateLabel;
		private DateTimePicker DateTimePicker;
		private ToolTip ToolTip;
	}
}
