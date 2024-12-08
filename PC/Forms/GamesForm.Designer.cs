using PC.Controls;

namespace PC.Forms
{
	partial class GamesForm
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
			this.components = new System.ComponentModel.Container();
			this.GamesTabControl = new System.Windows.Forms.TabControl();
			this.GameInfoTabPage = new System.Windows.Forms.TabPage();
			this.ScoringSystemDefaultLabel = new System.Windows.Forms.Label();
			this.ScoringSystemComboBox = new System.Windows.Forms.ComboBox();
			this.ScoringSystemLabel = new System.Windows.Forms.Label();
			this.GameStatusLabel = new System.Windows.Forms.Label();
			this.GameStatusComboBox = new System.Windows.Forms.ComboBox();
			this.ScoreColumnHeaderLabel = new System.Windows.Forms.Label();
			this.TotalScoreTextLabel = new System.Windows.Forms.Label();
			this.ScoreTotalBarLabel = new System.Windows.Forms.Label();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.TotalScoreLabel = new System.Windows.Forms.Label();
			this.GameInErrorButton = new System.Windows.Forms.Button();
			this.PlayerAssignmentAdviceLabel = new System.Windows.Forms.Label();
			this.ConflictsPanel = new System.Windows.Forms.Panel();
			this.TurkeyConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.RussiaConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.ItalyConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.GermanyConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.FranceConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.EnglandConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.AustriaConflictLinkLabel = new System.Windows.Forms.LinkLabel();
			this.TotalConflictsLabel = new System.Windows.Forms.Label();
			this.ConflictsTotalBarLabel = new System.Windows.Forms.Label();
			this.ConflictsColumnHeaderLabel = new System.Windows.Forms.Label();
			this.PlayerPanel = new System.Windows.Forms.Panel();
			this.TurkeyPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.RussiaPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.ItalyPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.GermanyPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.FrancePlayerComboBox = new System.Windows.Forms.ComboBox();
			this.EnglandPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.AustriaPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.PlayersColumnHeaderLabel = new System.Windows.Forms.Label();
			this.ScoresPanel = new System.Windows.Forms.Panel();
			this.AustriaScoreLabel = new System.Windows.Forms.Label();
			this.EnglandScoreLabel = new System.Windows.Forms.Label();
			this.FranceScoreLabel = new System.Windows.Forms.Label();
			this.GermanyScoreLabel = new System.Windows.Forms.Label();
			this.ItalyScoreLabel = new System.Windows.Forms.Label();
			this.RussiaScoreLabel = new System.Windows.Forms.Label();
			this.TurkeyScoreLabel = new System.Windows.Forms.Label();
			this.GameControl = new GameControl();
			this.GamesTabControl.SuspendLayout();
			this.ConflictsPanel.SuspendLayout();
			this.PlayerPanel.SuspendLayout();
			this.ScoresPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// GamesTabControl
			// 
			this.GamesTabControl.Controls.Add(this.GameInfoTabPage);
			this.GamesTabControl.Location = new System.Drawing.Point(10, 9);
			this.GamesTabControl.Margin = new System.Windows.Forms.Padding(1);
			this.GamesTabControl.Name = "GamesTabControl";
			this.GamesTabControl.SelectedIndex = 0;
			this.GamesTabControl.Size = new System.Drawing.Size(695, 352);
			this.GamesTabControl.TabIndex = 0;
			this.GamesTabControl.SelectedIndexChanged += new System.EventHandler(this.GamesTabControl_SelectedIndexChanged);
			// 
			// GameInfoTabPage
			// 
			this.GameInfoTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.GameInfoTabPage.Location = new System.Drawing.Point(4, 22);
			this.GameInfoTabPage.Margin = new System.Windows.Forms.Padding(1);
			this.GameInfoTabPage.Name = "GameInfoTabPage";
			this.GameInfoTabPage.Padding = new System.Windows.Forms.Padding(1);
			this.GameInfoTabPage.Size = new System.Drawing.Size(687, 326);
			this.GameInfoTabPage.TabIndex = 0;
			this.GameInfoTabPage.Text = "Game 1";
			// 
			// ScoringSystemDefaultLabel
			// 
			this.ScoringSystemDefaultLabel.AutoSize = true;
			this.ScoringSystemDefaultLabel.Location = new System.Drawing.Point(347, 53);
			this.ScoringSystemDefaultLabel.Name = "ScoringSystemDefaultLabel";
			this.ScoringSystemDefaultLabel.Size = new System.Drawing.Size(45, 13);
			this.ScoringSystemDefaultLabel.TabIndex = 28;
			this.ScoringSystemDefaultLabel.Text = "(default)";
			// 
			// ScoringSystemComboBox
			// 
			this.ScoringSystemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ScoringSystemComboBox.FormattingEnabled = true;
			this.ScoringSystemComboBox.Location = new System.Drawing.Point(150, 50);
			this.ScoringSystemComboBox.Name = "ScoringSystemComboBox";
			this.ScoringSystemComboBox.Size = new System.Drawing.Size(191, 21);
			this.ScoringSystemComboBox.TabIndex = 27;
			this.ScoringSystemComboBox.SelectedIndexChanged += new System.EventHandler(this.ScoringSystemComboBox_SelectedIndexChanged);
			this.ScoringSystemComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// ScoringSystemLabel
			// 
			this.ScoringSystemLabel.AutoSize = true;
			this.ScoringSystemLabel.Location = new System.Drawing.Point(29, 53);
			this.ScoringSystemLabel.Name = "ScoringSystemLabel";
			this.ScoringSystemLabel.Size = new System.Drawing.Size(114, 13);
			this.ScoringSystemLabel.TabIndex = 26;
			this.ScoringSystemLabel.Text = "Game Scoring System:";
			// 
			// GameStatusLabel
			// 
			this.GameStatusLabel.AutoSize = true;
			this.GameStatusLabel.Location = new System.Drawing.Point(503, 53);
			this.GameStatusLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.GameStatusLabel.Name = "GameStatusLabel";
			this.GameStatusLabel.Size = new System.Drawing.Size(71, 13);
			this.GameStatusLabel.TabIndex = 29;
			this.GameStatusLabel.Text = "Game Status:";
			// 
			// GameStatusComboBox
			// 
			this.GameStatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GameStatusComboBox.FormattingEnabled = true;
			this.GameStatusComboBox.Items.AddRange(new object[] {
            "        Seeded",
            "◯ = Underway",
            "✔ = Finished"});
			this.GameStatusComboBox.Location = new System.Drawing.Point(576, 50);
			this.GameStatusComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.GameStatusComboBox.Name = "GameStatusComboBox";
			this.GameStatusComboBox.Size = new System.Drawing.Size(104, 21);
			this.GameStatusComboBox.TabIndex = 30;
			this.GameStatusComboBox.SelectedIndexChanged += new System.EventHandler(this.GameStatusComboBox_SelectedIndexChanged);
			// 
			// ScoreColumnHeaderLabel
			// 
			this.ScoreColumnHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScoreColumnHeaderLabel.Location = new System.Drawing.Point(636, 107);
			this.ScoreColumnHeaderLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ScoreColumnHeaderLabel.Name = "ScoreColumnHeaderLabel";
			this.ScoreColumnHeaderLabel.Size = new System.Drawing.Size(49, 13);
			this.ScoreColumnHeaderLabel.TabIndex = 40;
			this.ScoreColumnHeaderLabel.Text = "SCORE";
			this.ScoreColumnHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TotalScoreTextLabel
			// 
			this.TotalScoreTextLabel.AutoSize = true;
			this.TotalScoreTextLabel.Location = new System.Drawing.Point(520, 326);
			this.TotalScoreTextLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TotalScoreTextLabel.Name = "TotalScoreTextLabel";
			this.TotalScoreTextLabel.Size = new System.Drawing.Size(111, 13);
			this.TotalScoreTextLabel.TabIndex = 49;
			this.TotalScoreTextLabel.Text = "Total Points Awarded:";
			this.TotalScoreTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ScoreTotalBarLabel
			// 
			this.ScoreTotalBarLabel.AutoSize = true;
			this.ScoreTotalBarLabel.Location = new System.Drawing.Point(638, 308);
			this.ScoreTotalBarLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ScoreTotalBarLabel.Name = "ScoreTotalBarLabel";
			this.ScoreTotalBarLabel.Size = new System.Drawing.Size(47, 13);
			this.ScoreTotalBarLabel.TabIndex = 52;
			this.ScoreTotalBarLabel.Text = "─────";
			// 
			// ToolTip
			// 
			this.ToolTip.IsBalloon = true;
			// 
			// TotalScoreLabel
			// 
			this.TotalScoreLabel.Location = new System.Drawing.Point(637, 325);
			this.TotalScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TotalScoreLabel.Name = "TotalScoreLabel";
			this.TotalScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.TotalScoreLabel.TabIndex = 67;
			this.TotalScoreLabel.Text = "0";
			this.TotalScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// GameInErrorButton
			// 
			this.GameInErrorButton.BackColor = System.Drawing.Color.Firebrick;
			this.GameInErrorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GameInErrorButton.ForeColor = System.Drawing.Color.Yellow;
			this.GameInErrorButton.Location = new System.Drawing.Point(358, 321);
			this.GameInErrorButton.Name = "GameInErrorButton";
			this.GameInErrorButton.Size = new System.Drawing.Size(91, 23);
			this.GameInErrorButton.TabIndex = 69;
			this.GameInErrorButton.Text = "ERROR";
			this.GameInErrorButton.UseVisualStyleBackColor = false;
			this.GameInErrorButton.Click += new System.EventHandler(this.GameInErrorButton_Click);
			// 
			// PlayerAssignmentAdviceLabel
			// 
			this.PlayerAssignmentAdviceLabel.AutoSize = true;
			this.PlayerAssignmentAdviceLabel.Location = new System.Drawing.Point(104, 77);
			this.PlayerAssignmentAdviceLabel.Name = "PlayerAssignmentAdviceLabel";
			this.PlayerAssignmentAdviceLabel.Size = new System.Drawing.Size(150, 26);
			this.PlayerAssignmentAdviceLabel.TabIndex = 70;
			this.PlayerAssignmentAdviceLabel.Text = "Once all powers are assigned,\r\nuse Swap Two to fix mistakes.";
			this.PlayerAssignmentAdviceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ConflictsPanel
			// 
			this.ConflictsPanel.Controls.Add(this.TurkeyConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.RussiaConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.ItalyConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.GermanyConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.FranceConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.EnglandConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.AustriaConflictLinkLabel);
			this.ConflictsPanel.Controls.Add(this.TotalConflictsLabel);
			this.ConflictsPanel.Controls.Add(this.ConflictsTotalBarLabel);
			this.ConflictsPanel.Controls.Add(this.ConflictsColumnHeaderLabel);
			this.ConflictsPanel.Location = new System.Drawing.Point(32, 107);
			this.ConflictsPanel.Name = "ConflictsPanel";
			this.ConflictsPanel.Size = new System.Drawing.Size(72, 246);
			this.ConflictsPanel.TabIndex = 71;
			// 
			// TurkeyConflictLinkLabel
			// 
			this.TurkeyConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.TurkeyConflictLinkLabel.Location = new System.Drawing.Point(5, 181);
			this.TurkeyConflictLinkLabel.Name = "TurkeyConflictLinkLabel";
			this.TurkeyConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.TurkeyConflictLinkLabel.TabIndex = 76;
			this.TurkeyConflictLinkLabel.TabStop = true;
			this.TurkeyConflictLinkLabel.Text = "0 pts.";
			this.TurkeyConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RussiaConflictLinkLabel
			// 
			this.RussiaConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.RussiaConflictLinkLabel.Location = new System.Drawing.Point(5, 154);
			this.RussiaConflictLinkLabel.Name = "RussiaConflictLinkLabel";
			this.RussiaConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.RussiaConflictLinkLabel.TabIndex = 75;
			this.RussiaConflictLinkLabel.TabStop = true;
			this.RussiaConflictLinkLabel.Text = "0 pts.";
			this.RussiaConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ItalyConflictLinkLabel
			// 
			this.ItalyConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.ItalyConflictLinkLabel.Location = new System.Drawing.Point(5, 127);
			this.ItalyConflictLinkLabel.Name = "ItalyConflictLinkLabel";
			this.ItalyConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.ItalyConflictLinkLabel.TabIndex = 74;
			this.ItalyConflictLinkLabel.TabStop = true;
			this.ItalyConflictLinkLabel.Text = "0 pts.";
			this.ItalyConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// GermanyConflictLinkLabel
			// 
			this.GermanyConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.GermanyConflictLinkLabel.Location = new System.Drawing.Point(5, 100);
			this.GermanyConflictLinkLabel.Name = "GermanyConflictLinkLabel";
			this.GermanyConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.GermanyConflictLinkLabel.TabIndex = 73;
			this.GermanyConflictLinkLabel.TabStop = true;
			this.GermanyConflictLinkLabel.Text = "0 pts.";
			this.GermanyConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FranceConflictLinkLabel
			// 
			this.FranceConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.FranceConflictLinkLabel.Location = new System.Drawing.Point(5, 73);
			this.FranceConflictLinkLabel.Name = "FranceConflictLinkLabel";
			this.FranceConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.FranceConflictLinkLabel.TabIndex = 72;
			this.FranceConflictLinkLabel.TabStop = true;
			this.FranceConflictLinkLabel.Text = "0 pts.";
			this.FranceConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// EnglandConflictLinkLabel
			// 
			this.EnglandConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.EnglandConflictLinkLabel.Location = new System.Drawing.Point(5, 46);
			this.EnglandConflictLinkLabel.Name = "EnglandConflictLinkLabel";
			this.EnglandConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.EnglandConflictLinkLabel.TabIndex = 71;
			this.EnglandConflictLinkLabel.TabStop = true;
			this.EnglandConflictLinkLabel.Text = "0 pts.";
			this.EnglandConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// AustriaConflictLinkLabel
			// 
			this.AustriaConflictLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AustriaConflictLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.AustriaConflictLinkLabel.Location = new System.Drawing.Point(5, 19);
			this.AustriaConflictLinkLabel.Name = "AustriaConflictLinkLabel";
			this.AustriaConflictLinkLabel.Size = new System.Drawing.Size(60, 21);
			this.AustriaConflictLinkLabel.TabIndex = 70;
			this.AustriaConflictLinkLabel.TabStop = true;
			this.AustriaConflictLinkLabel.Text = "0 pts.";
			this.AustriaConflictLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TotalConflictsLabel
			// 
			this.TotalConflictsLabel.Location = new System.Drawing.Point(6, 218);
			this.TotalConflictsLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TotalConflictsLabel.Name = "TotalConflictsLabel";
			this.TotalConflictsLabel.Size = new System.Drawing.Size(59, 15);
			this.TotalConflictsLabel.TabIndex = 77;
			this.TotalConflictsLabel.Text = "0 pts.";
			this.TotalConflictsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ConflictsTotalBarLabel
			// 
			this.ConflictsTotalBarLabel.Location = new System.Drawing.Point(5, 201);
			this.ConflictsTotalBarLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ConflictsTotalBarLabel.Name = "ConflictsTotalBarLabel";
			this.ConflictsTotalBarLabel.Size = new System.Drawing.Size(60, 13);
			this.ConflictsTotalBarLabel.TabIndex = 69;
			this.ConflictsTotalBarLabel.Text = "───────";
			this.ConflictsTotalBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ConflictsColumnHeaderLabel
			// 
			this.ConflictsColumnHeaderLabel.AutoSize = true;
			this.ConflictsColumnHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ConflictsColumnHeaderLabel.Location = new System.Drawing.Point(0, 0);
			this.ConflictsColumnHeaderLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ConflictsColumnHeaderLabel.Name = "ConflictsColumnHeaderLabel";
			this.ConflictsColumnHeaderLabel.Size = new System.Drawing.Size(75, 13);
			this.ConflictsColumnHeaderLabel.TabIndex = 32;
			this.ConflictsColumnHeaderLabel.Text = "CONFLICTS";
			// 
			// PlayerPanel
			// 
			this.PlayerPanel.Controls.Add(this.TurkeyPlayerComboBox);
			this.PlayerPanel.Controls.Add(this.RussiaPlayerComboBox);
			this.PlayerPanel.Controls.Add(this.ItalyPlayerComboBox);
			this.PlayerPanel.Controls.Add(this.GermanyPlayerComboBox);
			this.PlayerPanel.Controls.Add(this.FrancePlayerComboBox);
			this.PlayerPanel.Controls.Add(this.EnglandPlayerComboBox);
			this.PlayerPanel.Controls.Add(this.AustriaPlayerComboBox);
			this.PlayerPanel.Controls.Add(this.PlayersColumnHeaderLabel);
			this.PlayerPanel.Location = new System.Drawing.Point(107, 106);
			this.PlayerPanel.Name = "PlayerPanel";
			this.PlayerPanel.Size = new System.Drawing.Size(147, 206);
			this.PlayerPanel.TabIndex = 72;
			// 
			// TurkeyPlayerComboBox
			// 
			this.TurkeyPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TurkeyPlayerComboBox.FormattingEnabled = true;
			this.TurkeyPlayerComboBox.Location = new System.Drawing.Point(3, 182);
			this.TurkeyPlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.TurkeyPlayerComboBox.Name = "TurkeyPlayerComboBox";
			this.TurkeyPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.TurkeyPlayerComboBox.TabIndex = 24;
			this.TurkeyPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.TurkeyPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// RussiaPlayerComboBox
			// 
			this.RussiaPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RussiaPlayerComboBox.FormattingEnabled = true;
			this.RussiaPlayerComboBox.Location = new System.Drawing.Point(3, 155);
			this.RussiaPlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.RussiaPlayerComboBox.Name = "RussiaPlayerComboBox";
			this.RussiaPlayerComboBox.Size = new System.Drawing.Size(140, 21);
			this.RussiaPlayerComboBox.TabIndex = 23;
			this.RussiaPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.RussiaPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// ItalyPlayerComboBox
			// 
			this.ItalyPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItalyPlayerComboBox.FormattingEnabled = true;
			this.ItalyPlayerComboBox.Location = new System.Drawing.Point(3, 128);
			this.ItalyPlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.ItalyPlayerComboBox.Name = "ItalyPlayerComboBox";
			this.ItalyPlayerComboBox.Size = new System.Drawing.Size(140, 21);
			this.ItalyPlayerComboBox.TabIndex = 22;
			this.ItalyPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.ItalyPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// GermanyPlayerComboBox
			// 
			this.GermanyPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GermanyPlayerComboBox.FormattingEnabled = true;
			this.GermanyPlayerComboBox.Location = new System.Drawing.Point(3, 101);
			this.GermanyPlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.GermanyPlayerComboBox.Name = "GermanyPlayerComboBox";
			this.GermanyPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.GermanyPlayerComboBox.TabIndex = 21;
			this.GermanyPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.GermanyPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// FrancePlayerComboBox
			// 
			this.FrancePlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FrancePlayerComboBox.FormattingEnabled = true;
			this.FrancePlayerComboBox.Location = new System.Drawing.Point(3, 74);
			this.FrancePlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.FrancePlayerComboBox.Name = "FrancePlayerComboBox";
			this.FrancePlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.FrancePlayerComboBox.TabIndex = 20;
			this.FrancePlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.FrancePlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// EnglandPlayerComboBox
			// 
			this.EnglandPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnglandPlayerComboBox.FormattingEnabled = true;
			this.EnglandPlayerComboBox.Location = new System.Drawing.Point(3, 47);
			this.EnglandPlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.EnglandPlayerComboBox.Name = "EnglandPlayerComboBox";
			this.EnglandPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.EnglandPlayerComboBox.TabIndex = 19;
			this.EnglandPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.EnglandPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// AustriaPlayerComboBox
			// 
			this.AustriaPlayerComboBox.BackColor = System.Drawing.SystemColors.HighlightText;
			this.AustriaPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AustriaPlayerComboBox.FormattingEnabled = true;
			this.AustriaPlayerComboBox.Location = new System.Drawing.Point(3, 20);
			this.AustriaPlayerComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.AustriaPlayerComboBox.Name = "AustriaPlayerComboBox";
			this.AustriaPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.AustriaPlayerComboBox.TabIndex = 18;
			this.AustriaPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.AustriaPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// PlayersColumnHeaderLabel
			// 
			this.PlayersColumnHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PlayersColumnHeaderLabel.Location = new System.Drawing.Point(3, 1);
			this.PlayersColumnHeaderLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.PlayersColumnHeaderLabel.Name = "PlayersColumnHeaderLabel";
			this.PlayersColumnHeaderLabel.Size = new System.Drawing.Size(140, 13);
			this.PlayersColumnHeaderLabel.TabIndex = 17;
			this.PlayersColumnHeaderLabel.Text = "PLAYER";
			this.PlayersColumnHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ScoresPanel
			// 
			this.ScoresPanel.Controls.Add(this.AustriaScoreLabel);
			this.ScoresPanel.Controls.Add(this.EnglandScoreLabel);
			this.ScoresPanel.Controls.Add(this.FranceScoreLabel);
			this.ScoresPanel.Controls.Add(this.GermanyScoreLabel);
			this.ScoresPanel.Controls.Add(this.ItalyScoreLabel);
			this.ScoresPanel.Controls.Add(this.RussiaScoreLabel);
			this.ScoresPanel.Controls.Add(this.TurkeyScoreLabel);
			this.ScoresPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScoresPanel.ForeColor = System.Drawing.Color.Blue;
			this.ScoresPanel.Location = new System.Drawing.Point(636, 124);
			this.ScoresPanel.Name = "ScoresPanel";
			this.ScoresPanel.Size = new System.Drawing.Size(49, 188);
			this.ScoresPanel.TabIndex = 73;
			// 
			// AustriaScoreLabel
			// 
			this.AustriaScoreLabel.Location = new System.Drawing.Point(1, 5);
			this.AustriaScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.AustriaScoreLabel.Name = "AustriaScoreLabel";
			this.AustriaScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.AustriaScoreLabel.TabIndex = 67;
			this.AustriaScoreLabel.Text = "0";
			this.AustriaScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// EnglandScoreLabel
			// 
			this.EnglandScoreLabel.Location = new System.Drawing.Point(1, 32);
			this.EnglandScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.EnglandScoreLabel.Name = "EnglandScoreLabel";
			this.EnglandScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.EnglandScoreLabel.TabIndex = 68;
			this.EnglandScoreLabel.Text = "0";
			this.EnglandScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FranceScoreLabel
			// 
			this.FranceScoreLabel.Location = new System.Drawing.Point(1, 60);
			this.FranceScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.FranceScoreLabel.Name = "FranceScoreLabel";
			this.FranceScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.FranceScoreLabel.TabIndex = 69;
			this.FranceScoreLabel.Text = "0";
			this.FranceScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// GermanyScoreLabel
			// 
			this.GermanyScoreLabel.Location = new System.Drawing.Point(1, 86);
			this.GermanyScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.GermanyScoreLabel.Name = "GermanyScoreLabel";
			this.GermanyScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.GermanyScoreLabel.TabIndex = 70;
			this.GermanyScoreLabel.Text = "0";
			this.GermanyScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ItalyScoreLabel
			// 
			this.ItalyScoreLabel.Location = new System.Drawing.Point(1, 113);
			this.ItalyScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ItalyScoreLabel.Name = "ItalyScoreLabel";
			this.ItalyScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.ItalyScoreLabel.TabIndex = 71;
			this.ItalyScoreLabel.Text = "0";
			this.ItalyScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RussiaScoreLabel
			// 
			this.RussiaScoreLabel.Location = new System.Drawing.Point(2, 140);
			this.RussiaScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.RussiaScoreLabel.Name = "RussiaScoreLabel";
			this.RussiaScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.RussiaScoreLabel.TabIndex = 72;
			this.RussiaScoreLabel.Text = "0";
			this.RussiaScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TurkeyScoreLabel
			// 
			this.TurkeyScoreLabel.Location = new System.Drawing.Point(2, 167);
			this.TurkeyScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TurkeyScoreLabel.Name = "TurkeyScoreLabel";
			this.TurkeyScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.TurkeyScoreLabel.TabIndex = 73;
			this.TurkeyScoreLabel.Text = "0";
			this.TurkeyScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// GameControl
			// 
			this.GameControl.Location = new System.Drawing.Point(254, 77);
			this.GameControl.Margin = new System.Windows.Forms.Padding(0);
			this.GameControl.Name = "GameControl";
			this.GameControl.Size = new System.Drawing.Size(381, 235);
			this.GameControl.TabIndex = 1;
			// 
			// GamesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(716, 371);
			this.Controls.Add(this.ScoresPanel);
			this.Controls.Add(this.PlayerPanel);
			this.Controls.Add(this.ConflictsPanel);
			this.Controls.Add(this.PlayerAssignmentAdviceLabel);
			this.Controls.Add(this.GameInErrorButton);
			this.Controls.Add(this.TotalScoreLabel);
			this.Controls.Add(this.ScoreTotalBarLabel);
			this.Controls.Add(this.TotalScoreTextLabel);
			this.Controls.Add(this.ScoreColumnHeaderLabel);
			this.Controls.Add(this.GameControl);
			this.Controls.Add(this.GameStatusComboBox);
			this.Controls.Add(this.GameStatusLabel);
			this.Controls.Add(this.ScoringSystemDefaultLabel);
			this.Controls.Add(this.ScoringSystemComboBox);
			this.Controls.Add(this.ScoringSystemLabel);
			this.Controls.Add(this.GamesTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GamesForm";
			this.ShowIcon = false;
			this.Text = "Round 1 Games";
			this.Load += new System.EventHandler(this.GamesForm_Load);
			this.GamesTabControl.ResumeLayout(false);
			this.ConflictsPanel.ResumeLayout(false);
			this.ConflictsPanel.PerformLayout();
			this.PlayerPanel.ResumeLayout(false);
			this.ScoresPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TabControl GamesTabControl;
		private TabPage GameInfoTabPage;
		private Label ScoringSystemDefaultLabel;
		private ComboBox ScoringSystemComboBox;
		private Label ScoringSystemLabel;
		private Label GameStatusLabel;
		private ComboBox GameStatusComboBox;
		private Label ScoreColumnHeaderLabel;
		private GameControl GameControl;
		private Label TotalScoreTextLabel;
		private Label ScoreTotalBarLabel;
		private ToolTip ToolTip;
		private Label TotalScoreLabel;
		private Button GameInErrorButton;
		private Label PlayerAssignmentAdviceLabel;
		private Panel ConflictsPanel;
		private Panel PlayerPanel;
		private Panel ScoresPanel;
		private Label AustriaScoreLabel;
		private Label EnglandScoreLabel;
		private Label FranceScoreLabel;
		private Label GermanyScoreLabel;
		private Label ItalyScoreLabel;
		private Label RussiaScoreLabel;
		private Label TurkeyScoreLabel;
		private LinkLabel TurkeyConflictLinkLabel;
		private LinkLabel RussiaConflictLinkLabel;
		private LinkLabel ItalyConflictLinkLabel;
		private LinkLabel GermanyConflictLinkLabel;
		private LinkLabel FranceConflictLinkLabel;
		private LinkLabel EnglandConflictLinkLabel;
		private LinkLabel AustriaConflictLinkLabel;
		private Label TotalConflictsLabel;
		private Label ConflictsTotalBarLabel;
		private Label ConflictsColumnHeaderLabel;
		private ComboBox TurkeyPlayerComboBox;
		private ComboBox RussiaPlayerComboBox;
		private ComboBox ItalyPlayerComboBox;
		private ComboBox GermanyPlayerComboBox;
		private ComboBox FrancePlayerComboBox;
		private ComboBox EnglandPlayerComboBox;
		private ComboBox AustriaPlayerComboBox;
		private Label PlayersColumnHeaderLabel;
	}
}
