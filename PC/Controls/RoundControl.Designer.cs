namespace PC.Controls
{
	partial class RoundControl
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.UnseedGameButton = new System.Windows.Forms.Button();
			this.SwapButton = new System.Windows.Forms.Button();
			this.ReplaceButton = new System.Windows.Forms.Button();
			this.ChangeRoundButton = new System.Windows.Forms.Button();
			this.OrderPlayersByPanel = new System.Windows.Forms.Panel();
			this.LastNameRadioButton = new System.Windows.Forms.RadioButton();
			this.FirstNameRadioButton = new System.Windows.Forms.RadioButton();
			this.OrderByLabel = new System.Windows.Forms.Label();
			this.MoveGameDownButton = new System.Windows.Forms.Button();
			this.MoveGameUpButton = new System.Windows.Forms.Button();
			this.ScoringSystemDefaultLabel = new System.Windows.Forms.Label();
			this.ScoringSystemComboBox = new System.Windows.Forms.ComboBox();
			this.ScoringSystemLabel = new System.Windows.Forms.Label();
			this.ViewGamesButton = new System.Windows.Forms.Button();
			this.StartGamesButton = new System.Windows.Forms.Button();
			this.GamesAndPlayersLabel = new System.Windows.Forms.Label();
			this.PlayersRegisteredLabel = new System.Windows.Forms.Label();
			this.SeedingAssignsPowersCheckBox = new System.Windows.Forms.CheckBox();
			this.SeededPlayerCountLabel = new System.Windows.Forms.Label();
			this.RegisteredCountLabel = new System.Windows.Forms.Label();
			this.UnregisteredCountLabel = new System.Windows.Forms.Label();
			this.UnseedButton = new System.Windows.Forms.Button();
			this.SeedSomeButton = new System.Windows.Forms.Button();
			this.SeedAllButton = new System.Windows.Forms.Button();
			this.UnregisterAllButton = new System.Windows.Forms.Button();
			this.RegisterAllButton = new System.Windows.Forms.Button();
			this.RegisterForRoundButton = new System.Windows.Forms.Button();
			this.SeededDataGridView = new System.Windows.Forms.DataGridView();
			this.WhichPlayersTabControl = new System.Windows.Forms.TabControl();
			this.TournamentPlayersTabPage = new System.Windows.Forms.TabPage();
			this.AllPlayersTabPage = new System.Windows.Forms.TabPage();
			this.SortByScoreCheckBox = new System.Windows.Forms.CheckBox();
			this.UnregisteredDataGridView = new System.Windows.Forms.DataGridView();
			this.RegisteredDataGridView = new System.Windows.Forms.DataGridView();
			this.RoundLockedLabel = new System.Windows.Forms.Label();
			this.PrintButton = new System.Windows.Forms.Button();
			this.EmailButton = new System.Windows.Forms.Button();
			this.FindPlayerTextBox = new System.Windows.Forms.TextBox();
			this.FindPlayerLabel = new System.Windows.Forms.Label();
			this.NewPlayerButton = new System.Windows.Forms.Button();
			this.OrderPlayersByPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SeededDataGridView)).BeginInit();
			this.WhichPlayersTabControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UnregisteredDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RegisteredDataGridView)).BeginInit();
			this.SuspendLayout();
			//
			// UnseedGameButton
			//
			this.UnseedGameButton.Location = new System.Drawing.Point(491, 325);
			this.UnseedGameButton.Name = "UnseedGameButton";
			this.UnseedGameButton.Size = new System.Drawing.Size(106, 23);
			this.UnseedGameButton.TabIndex = 91;
			this.UnseedGameButton.Text = "◀─ Unseed Game";
			this.UnseedGameButton.UseVisualStyleBackColor = true;
			this.UnseedGameButton.Click += new System.EventHandler(this.UnseedGameButton_Click);
			//
			// SwapButton
			//
			this.SwapButton.Location = new System.Drawing.Point(530, 231);
			this.SwapButton.Name = "SwapButton";
			this.SwapButton.Size = new System.Drawing.Size(67, 37);
			this.SwapButton.TabIndex = 89;
			this.SwapButton.Text = "Swap ┏━▶\r\nTwo   ┗━▶";
			this.SwapButton.UseVisualStyleBackColor = true;
			this.SwapButton.Click += new System.EventHandler(this.SwapButton_Click);
			//
			// ReplaceButton
			//
			this.ReplaceButton.Location = new System.Drawing.Point(492, 273);
			this.ReplaceButton.Name = "ReplaceButton";
			this.ReplaceButton.Size = new System.Drawing.Size(105, 23);
			this.ReplaceButton.TabIndex = 90;
			this.ReplaceButton.Text = "◀─ Replace ─▶";
			this.ReplaceButton.UseVisualStyleBackColor = true;
			this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButton_Click);
			//
			// ChangeRoundButton
			//
			this.ChangeRoundButton.BackColor = System.Drawing.Color.LightGreen;
			this.ChangeRoundButton.Location = new System.Drawing.Point(890, 3);
			this.ChangeRoundButton.Name = "ChangeRoundButton";
			this.ChangeRoundButton.Size = new System.Drawing.Size(61, 52);
			this.ChangeRoundButton.TabIndex = 87;
			this.ChangeRoundButton.Text = "  Start   ▶\r\n Next   ▶\r\nRound ▶";
			this.ChangeRoundButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ChangeRoundButton.UseVisualStyleBackColor = false;
			this.ChangeRoundButton.Click += new System.EventHandler(this.ChangeRoundButton_Click);
			//
			// OrderPlayersByPanel
			//
			this.OrderPlayersByPanel.Controls.Add(this.LastNameRadioButton);
			this.OrderPlayersByPanel.Controls.Add(this.FirstNameRadioButton);
			this.OrderPlayersByPanel.Controls.Add(this.OrderByLabel);
			this.OrderPlayersByPanel.Location = new System.Drawing.Point(100, 502);
			this.OrderPlayersByPanel.Name = "OrderPlayersByPanel";
			this.OrderPlayersByPanel.Size = new System.Drawing.Size(262, 25);
			this.OrderPlayersByPanel.TabIndex = 86;
			//
			// LastNameRadioButton
			//
			this.LastNameRadioButton.AutoSize = true;
			this.LastNameRadioButton.Location = new System.Drawing.Point(181, 4);
			this.LastNameRadioButton.Name = "LastNameRadioButton";
			this.LastNameRadioButton.Size = new System.Drawing.Size(76, 17);
			this.LastNameRadioButton.TabIndex = 17;
			this.LastNameRadioButton.TabStop = true;
			this.LastNameRadioButton.Text = "Last Name";
			this.LastNameRadioButton.UseVisualStyleBackColor = true;
			this.LastNameRadioButton.CheckedChanged += new System.EventHandler(this.NameSortControl_CheckedChanged);
			//
			// FirstNameRadioButton
			//
			this.FirstNameRadioButton.AutoSize = true;
			this.FirstNameRadioButton.Location = new System.Drawing.Point(104, 4);
			this.FirstNameRadioButton.Name = "FirstNameRadioButton";
			this.FirstNameRadioButton.Size = new System.Drawing.Size(75, 17);
			this.FirstNameRadioButton.TabIndex = 16;
			this.FirstNameRadioButton.TabStop = true;
			this.FirstNameRadioButton.Text = "First Name";
			this.FirstNameRadioButton.UseVisualStyleBackColor = true;
			this.FirstNameRadioButton.CheckedChanged += new System.EventHandler(this.NameSortControl_CheckedChanged);
			//
			// OrderByLabel
			//
			this.OrderByLabel.AutoSize = true;
			this.OrderByLabel.Location = new System.Drawing.Point(10, 6);
			this.OrderByLabel.Name = "OrderByLabel";
			this.OrderByLabel.Size = new System.Drawing.Size(88, 13);
			this.OrderByLabel.TabIndex = 15;
			this.OrderByLabel.Text = "Order Players By:";
			//
			// MoveGameDownButton
			//
			this.MoveGameDownButton.Location = new System.Drawing.Point(889, 273);
			this.MoveGameDownButton.Name = "MoveGameDownButton";
			this.MoveGameDownButton.Size = new System.Drawing.Size(62, 35);
			this.MoveGameDownButton.TabIndex = 85;
			this.MoveGameDownButton.Text = "Game\r\n▼";
			this.MoveGameDownButton.UseVisualStyleBackColor = true;
			this.MoveGameDownButton.Click += new System.EventHandler(this.MoveGameButton_Click);
			//
			// MoveGameUpButton
			//
			this.MoveGameUpButton.Location = new System.Drawing.Point(889, 234);
			this.MoveGameUpButton.Name = "MoveGameUpButton";
			this.MoveGameUpButton.Size = new System.Drawing.Size(62, 35);
			this.MoveGameUpButton.TabIndex = 84;
			this.MoveGameUpButton.Text = "▲\r\nGame";
			this.MoveGameUpButton.UseVisualStyleBackColor = true;
			this.MoveGameUpButton.Click += new System.EventHandler(this.MoveGameButton_Click);
			//
			// ScoringSystemDefaultLabel
			//
			this.ScoringSystemDefaultLabel.AutoSize = true;
			this.ScoringSystemDefaultLabel.Location = new System.Drawing.Point(332, 8);
			this.ScoringSystemDefaultLabel.Name = "ScoringSystemDefaultLabel";
			this.ScoringSystemDefaultLabel.Size = new System.Drawing.Size(45, 13);
			this.ScoringSystemDefaultLabel.TabIndex = 83;
			this.ScoringSystemDefaultLabel.Text = "(default)";
			//
			// ScoringSystemComboBox
			//
			this.ScoringSystemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ScoringSystemComboBox.FormattingEnabled = true;
			this.ScoringSystemComboBox.Location = new System.Drawing.Point(134, 5);
			this.ScoringSystemComboBox.Name = "ScoringSystemComboBox";
			this.ScoringSystemComboBox.Size = new System.Drawing.Size(191, 21);
			this.ScoringSystemComboBox.TabIndex = 82;
			this.ScoringSystemComboBox.SelectedIndexChanged += new System.EventHandler(this.ScoringSystemComboBox_SelectedIndexChanged);
			this.ScoringSystemComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// ScoringSystemLabel
			//
			this.ScoringSystemLabel.AutoSize = true;
			this.ScoringSystemLabel.Location = new System.Drawing.Point(14, 8);
			this.ScoringSystemLabel.Name = "ScoringSystemLabel";
			this.ScoringSystemLabel.Size = new System.Drawing.Size(118, 13);
			this.ScoringSystemLabel.TabIndex = 81;
			this.ScoringSystemLabel.Text = "Round Scoring System:";
			//
			// ViewGamesButton
			//
			this.ViewGamesButton.Location = new System.Drawing.Point(606, 503);
			this.ViewGamesButton.Name = "ViewGamesButton";
			this.ViewGamesButton.Size = new System.Drawing.Size(277, 23);
			this.ViewGamesButton.TabIndex = 80;
			this.ViewGamesButton.Text = "View Games…";
			this.ViewGamesButton.UseVisualStyleBackColor = true;
			this.ViewGamesButton.Click += new System.EventHandler(this.ViewGamesButton_Click);
			//
			// StartGamesButton
			//
			this.StartGamesButton.Location = new System.Drawing.Point(603, 3);
			this.StartGamesButton.Name = "StartGamesButton";
			this.StartGamesButton.Size = new System.Drawing.Size(280, 23);
			this.StartGamesButton.TabIndex = 78;
			this.StartGamesButton.Text = "Start Games…";
			this.StartGamesButton.UseVisualStyleBackColor = true;
			this.StartGamesButton.Click += new System.EventHandler(this.StartGamesButton_Click);
			//
			// GamesAndPlayersLabel
			//
			this.GamesAndPlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GamesAndPlayersLabel.Location = new System.Drawing.Point(600, 35);
			this.GamesAndPlayersLabel.Name = "GamesAndPlayersLabel";
			this.GamesAndPlayersLabel.Size = new System.Drawing.Size(283, 23);
			this.GamesAndPlayersLabel.TabIndex = 77;
			this.GamesAndPlayersLabel.Text = "Games and Player Assignments for Round 1";
			this.GamesAndPlayersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// PlayersRegisteredLabel
			//
			this.PlayersRegisteredLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PlayersRegisteredLabel.Location = new System.Drawing.Point(294, 35);
			this.PlayersRegisteredLabel.Name = "PlayersRegisteredLabel";
			this.PlayersRegisteredLabel.Size = new System.Drawing.Size(191, 23);
			this.PlayersRegisteredLabel.TabIndex = 76;
			this.PlayersRegisteredLabel.Text = "Players Registered for Round 1";
			this.PlayersRegisteredLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// SeedingAssignsPowersCheckBox
			//
			this.SeedingAssignsPowersCheckBox.AutoSize = true;
			this.SeedingAssignsPowersCheckBox.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.SeedingAssignsPowersCheckBox.Location = new System.Drawing.Point(510, 80);
			this.SeedingAssignsPowersCheckBox.Name = "SeedingAssignsPowersCheckBox";
			this.SeedingAssignsPowersCheckBox.Size = new System.Drawing.Size(65, 43);
			this.SeedingAssignsPowersCheckBox.TabIndex = 75;
			this.SeedingAssignsPowersCheckBox.Text = "Seeding\r\nAssigns\r\nPowers";
			this.SeedingAssignsPowersCheckBox.UseVisualStyleBackColor = true;
			//
			// SeededPlayerCountLabel
			//
			this.SeededPlayerCountLabel.Location = new System.Drawing.Point(603, 473);
			this.SeededPlayerCountLabel.Name = "SeededPlayerCountLabel";
			this.SeededPlayerCountLabel.Size = new System.Drawing.Size(280, 23);
			this.SeededPlayerCountLabel.TabIndex = 74;
			this.SeededPlayerCountLabel.Text = "nnn Players in xx Games";
			this.SeededPlayerCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// RegisteredCountLabel
			//
			this.RegisteredCountLabel.Location = new System.Drawing.Point(294, 473);
			this.RegisteredCountLabel.Name = "RegisteredCountLabel";
			this.RegisteredCountLabel.Size = new System.Drawing.Size(191, 23);
			this.RegisteredCountLabel.TabIndex = 73;
			this.RegisteredCountLabel.Text = "nnn Players Listed";
			this.RegisteredCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// UnregisteredCountLabel
			//
			this.UnregisteredCountLabel.Location = new System.Drawing.Point(15, 473);
			this.UnregisteredCountLabel.Name = "UnregisteredCountLabel";
			this.UnregisteredCountLabel.Size = new System.Drawing.Size(191, 23);
			this.UnregisteredCountLabel.TabIndex = 72;
			this.UnregisteredCountLabel.Text = "nnn Players Listed";
			this.UnregisteredCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// UnseedButton
			//
			this.UnseedButton.Location = new System.Drawing.Point(491, 354);
			this.UnseedButton.Name = "UnseedButton";
			this.UnseedButton.Size = new System.Drawing.Size(106, 23);
			this.UnseedButton.TabIndex = 71;
			this.UnseedButton.Text = "◀─── Unseed All";
			this.UnseedButton.UseVisualStyleBackColor = true;
			this.UnseedButton.Click += new System.EventHandler(this.UnseedButton_Click);
			//
			// SeedSomeButton
			//
			this.SeedSomeButton.Location = new System.Drawing.Point(491, 185);
			this.SeedSomeButton.Name = "SeedSomeButton";
			this.SeedSomeButton.Size = new System.Drawing.Size(106, 23);
			this.SeedSomeButton.TabIndex = 70;
			this.SeedSomeButton.Text = "Seed Some ───▶";
			this.SeedSomeButton.UseVisualStyleBackColor = true;
			this.SeedSomeButton.Click += new System.EventHandler(this.SeedButton_Click);
			//
			// SeedAllButton
			//
			this.SeedAllButton.Location = new System.Drawing.Point(491, 156);
			this.SeedAllButton.Name = "SeedAllButton";
			this.SeedAllButton.Size = new System.Drawing.Size(106, 23);
			this.SeedAllButton.TabIndex = 69;
			this.SeedAllButton.Text = "Seed All ─────▶";
			this.SeedAllButton.UseVisualStyleBackColor = true;
			this.SeedAllButton.Click += new System.EventHandler(this.SeedButton_Click);
			//
			// UnregisterAllButton
			//
			this.UnregisterAllButton.Location = new System.Drawing.Point(212, 354);
			this.UnregisterAllButton.Name = "UnregisterAllButton";
			this.UnregisterAllButton.Size = new System.Drawing.Size(76, 23);
			this.UnregisterAllButton.TabIndex = 68;
			this.UnregisterAllButton.Text = "◀── All";
			this.UnregisterAllButton.UseVisualStyleBackColor = true;
			this.UnregisterAllButton.Click += new System.EventHandler(this.UnregisterAllButton_Click);
			//
			// RegisterAllButton
			//
			this.RegisterAllButton.Location = new System.Drawing.Point(212, 325);
			this.RegisterAllButton.Name = "RegisterAllButton";
			this.RegisterAllButton.Size = new System.Drawing.Size(76, 23);
			this.RegisterAllButton.TabIndex = 67;
			this.RegisterAllButton.Text = "All ──▶";
			this.RegisterAllButton.UseVisualStyleBackColor = true;
			this.RegisterAllButton.Click += new System.EventHandler(this.RegisterAllButton_Click);
			//
			// RegisterForRoundButton
			//
			this.RegisterForRoundButton.Location = new System.Drawing.Point(212, 185);
			this.RegisterForRoundButton.Name = "RegisterForRoundButton";
			this.RegisterForRoundButton.Size = new System.Drawing.Size(76, 23);
			this.RegisterForRoundButton.TabIndex = 66;
			this.RegisterForRoundButton.Text = "Register ─▶";
			this.RegisterForRoundButton.UseVisualStyleBackColor = true;
			this.RegisterForRoundButton.Click += new System.EventHandler(this.RegisterButton_Click);
			//
			// SeededDataGridView
			//
			this.SeededDataGridView.AllowUserToAddRows = false;
			this.SeededDataGridView.AllowUserToDeleteRows = false;
			this.SeededDataGridView.AllowUserToResizeColumns = false;
			this.SeededDataGridView.AllowUserToResizeRows = false;
			this.SeededDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.SeededDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle1.Alignment = MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.SeededDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.SeededDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.SeededDataGridView.EnableHeadersVisualStyles = false;
			this.SeededDataGridView.Location = new System.Drawing.Point(603, 64);
			this.SeededDataGridView.Name = "SeededDataGridView";
			this.SeededDataGridView.ReadOnly = true;
			this.SeededDataGridView.RowHeadersVisible = false;
			this.SeededDataGridView.RowTemplate.Height = 16;
			this.SeededDataGridView.RowTemplate.ReadOnly = true;
			this.SeededDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.SeededDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.SeededDataGridView.ShowCellToolTips = false;
			this.SeededDataGridView.Size = new System.Drawing.Size(280, 407);
			this.SeededDataGridView.TabIndex = 65;
			this.SeededDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ViewGamesButton_Click);
			this.SeededDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.SeededDataGridView_DataBindingComplete);
			this.SeededDataGridView.SelectionChanged += new System.EventHandler(this.SeededDataGridView_SelectionChanged);
			//
			// WhichPlayersTabControl
			//
			this.WhichPlayersTabControl.Controls.Add(this.TournamentPlayersTabPage);
			this.WhichPlayersTabControl.Controls.Add(this.AllPlayersTabPage);
			this.WhichPlayersTabControl.Location = new System.Drawing.Point(10, 42);
			this.WhichPlayersTabControl.Margin = new System.Windows.Forms.Padding(1);
			this.WhichPlayersTabControl.Name = "WhichPlayersTabControl";
			this.WhichPlayersTabControl.SelectedIndex = 0;
			this.WhichPlayersTabControl.Size = new System.Drawing.Size(199, 430);
			this.WhichPlayersTabControl.TabIndex = 88;
			this.WhichPlayersTabControl.SelectedIndexChanged += new System.EventHandler(this.WhichPlayersTabControl_SelectedIndexChanged);
			//
			// TournamentPlayersTabPage
			//
			this.TournamentPlayersTabPage.Location = new System.Drawing.Point(4, 22);
			this.TournamentPlayersTabPage.Margin = new System.Windows.Forms.Padding(1);
			this.TournamentPlayersTabPage.Name = "TournamentPlayersTabPage";
			this.TournamentPlayersTabPage.Padding = new System.Windows.Forms.Padding(1);
			this.TournamentPlayersTabPage.Size = new System.Drawing.Size(191, 404);
			this.TournamentPlayersTabPage.TabIndex = 0;
			this.TournamentPlayersTabPage.Text = "Tournament Players";
			this.TournamentPlayersTabPage.UseVisualStyleBackColor = true;
			//
			// AllPlayersTabPage
			//
			this.AllPlayersTabPage.Location = new System.Drawing.Point(4, 22);
			this.AllPlayersTabPage.Margin = new System.Windows.Forms.Padding(1);
			this.AllPlayersTabPage.Name = "AllPlayersTabPage";
			this.AllPlayersTabPage.Padding = new System.Windows.Forms.Padding(1);
			this.AllPlayersTabPage.Size = new System.Drawing.Size(191, 404);
			this.AllPlayersTabPage.TabIndex = 1;
			this.AllPlayersTabPage.Text = "All Players";
			this.AllPlayersTabPage.UseVisualStyleBackColor = true;
			//
			// SortByScoreCheckBox
			//
			this.SortByScoreCheckBox.AutoSize = true;
			this.SortByScoreCheckBox.Checked = true;
			this.SortByScoreCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SortByScoreCheckBox.Location = new System.Drawing.Point(363, 507);
			this.SortByScoreCheckBox.Name = "SortByScoreCheckBox";
			this.SortByScoreCheckBox.Size = new System.Drawing.Size(54, 17);
			this.SortByScoreCheckBox.TabIndex = 92;
			this.SortByScoreCheckBox.Text = "Score";
			this.SortByScoreCheckBox.UseVisualStyleBackColor = true;
			this.SortByScoreCheckBox.Click += new System.EventHandler(this.NameSortControl_CheckedChanged);
			//
			// UnregisteredDataGridView
			//
			this.UnregisteredDataGridView.AllowUserToAddRows = false;
			this.UnregisteredDataGridView.AllowUserToDeleteRows = false;
			this.UnregisteredDataGridView.AllowUserToResizeColumns = false;
			this.UnregisteredDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
			this.UnregisteredDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			this.UnregisteredDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.UnregisteredDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.UnregisteredDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.UnregisteredDataGridView.ColumnHeadersVisible = false;
			this.UnregisteredDataGridView.Location = new System.Drawing.Point(14, 64);
			this.UnregisteredDataGridView.Name = "UnregisteredDataGridView";
			this.UnregisteredDataGridView.ReadOnly = true;
			this.UnregisteredDataGridView.RowHeadersVisible = false;
			this.UnregisteredDataGridView.RowTemplate.Height = 16;
			this.UnregisteredDataGridView.RowTemplate.ReadOnly = true;
			this.UnregisteredDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.UnregisteredDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.UnregisteredDataGridView.ShowCellToolTips = false;
			this.UnregisteredDataGridView.Size = new System.Drawing.Size(191, 405);
			this.UnregisteredDataGridView.TabIndex = 0;
			this.UnregisteredDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RegisterButton_Click);
			this.UnregisteredDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.RegistrableDataGridView_DataBindingComplete);
			this.UnregisteredDataGridView.SelectionChanged += new System.EventHandler(this.RegistrableDataGridView_SelectionChanged);
			//
			// RegisteredDataGridView
			//
			this.RegisteredDataGridView.AllowUserToAddRows = false;
			this.RegisteredDataGridView.AllowUserToDeleteRows = false;
			this.RegisteredDataGridView.AllowUserToResizeColumns = false;
			this.RegisteredDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
			this.RegisteredDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
			this.RegisteredDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.RegisteredDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.RegisteredDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RegisteredDataGridView.ColumnHeadersVisible = false;
			this.RegisteredDataGridView.Location = new System.Drawing.Point(297, 64);
			this.RegisteredDataGridView.Name = "RegisteredDataGridView";
			this.RegisteredDataGridView.ReadOnly = true;
			this.RegisteredDataGridView.RowHeadersVisible = false;
			this.RegisteredDataGridView.RowTemplate.Height = 16;
			this.RegisteredDataGridView.RowTemplate.ReadOnly = true;
			this.RegisteredDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.RegisteredDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RegisteredDataGridView.ShowCellToolTips = false;
			this.RegisteredDataGridView.Size = new System.Drawing.Size(188, 405);
			this.RegisteredDataGridView.TabIndex = 93;
			this.RegisteredDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.RegistrableDataGridView_DataBindingComplete);
			this.RegisteredDataGridView.SelectionChanged += new System.EventHandler(this.RegistrableDataGridView_SelectionChanged);
			//
			// RoundLockedLabel
			//
			this.RoundLockedLabel.AutoSize = true;
			this.RoundLockedLabel.BackColor = System.Drawing.Color.LightGreen;
			this.RoundLockedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RoundLockedLabel.ForeColor = System.Drawing.Color.Red;
			this.RoundLockedLabel.Location = new System.Drawing.Point(391, 3);
			this.RoundLockedLabel.Name = "RoundLockedLabel";
			this.RoundLockedLabel.Size = new System.Drawing.Size(180, 24);
			this.RoundLockedLabel.TabIndex = 94;
			this.RoundLockedLabel.Text = "Round is Finished";
			//
			// PrintButton
			//
			this.PrintButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PrintButton.Location = new System.Drawing.Point(889, 432);
			this.PrintButton.Name = "PrintButton";
			this.PrintButton.Size = new System.Drawing.Size(62, 23);
			this.PrintButton.TabIndex = 95;
			this.PrintButton.Text = "Print…";
			this.PrintButton.UseVisualStyleBackColor = true;
			this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
			//
			// EmailButton
			//
			this.EmailButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EmailButton.Location = new System.Drawing.Point(889, 403);
			this.EmailButton.Name = "EmailButton";
			this.EmailButton.Size = new System.Drawing.Size(62, 23);
			this.EmailButton.TabIndex = 96;
			this.EmailButton.Text = "Email…";
			this.EmailButton.UseVisualStyleBackColor = true;
			//
			// FindPlayerTextBox
			//
			this.FindPlayerTextBox.Location = new System.Drawing.Point(890, 138);
			this.FindPlayerTextBox.Name = "FindPlayerTextBox";
			this.FindPlayerTextBox.Size = new System.Drawing.Size(61, 20);
			this.FindPlayerTextBox.TabIndex = 97;
			this.FindPlayerTextBox.TextChanged += new System.EventHandler(this.FindPlayerTextBox_TextChanged);
			//
			// FindPlayerLabel
			//
			this.FindPlayerLabel.Location = new System.Drawing.Point(888, 80);
			this.FindPlayerLabel.Name = "FindPlayerLabel";
			this.FindPlayerLabel.Size = new System.Drawing.Size(62, 56);
			this.FindPlayerLabel.TabIndex = 98;
			this.FindPlayerLabel.Text = "Find Assigned Player By Name:";
			//
			// NewPlayerButton
			//
			this.NewPlayerButton.Location = new System.Drawing.Point(212, 80);
			this.NewPlayerButton.Name = "NewPlayerButton";
			this.NewPlayerButton.Size = new System.Drawing.Size(75, 23);
			this.NewPlayerButton.TabIndex = 99;
			this.NewPlayerButton.Text = "New Player";
			this.NewPlayerButton.UseVisualStyleBackColor = true;
			this.NewPlayerButton.Click += new System.EventHandler(this.NewPlayerButton_Click);
			//
			// RoundControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.NewPlayerButton);
			this.Controls.Add(this.FindPlayerLabel);
			this.Controls.Add(this.FindPlayerTextBox);
			this.Controls.Add(this.EmailButton);
			this.Controls.Add(this.PrintButton);
			this.Controls.Add(this.UnregisteredDataGridView);
			this.Controls.Add(this.RoundLockedLabel);
			this.Controls.Add(this.RegisteredDataGridView);
			this.Controls.Add(this.SortByScoreCheckBox);
			this.Controls.Add(this.UnseedGameButton);
			this.Controls.Add(this.SwapButton);
			this.Controls.Add(this.ReplaceButton);
			this.Controls.Add(this.ChangeRoundButton);
			this.Controls.Add(this.OrderPlayersByPanel);
			this.Controls.Add(this.MoveGameDownButton);
			this.Controls.Add(this.MoveGameUpButton);
			this.Controls.Add(this.ScoringSystemDefaultLabel);
			this.Controls.Add(this.ScoringSystemComboBox);
			this.Controls.Add(this.ScoringSystemLabel);
			this.Controls.Add(this.ViewGamesButton);
			this.Controls.Add(this.StartGamesButton);
			this.Controls.Add(this.GamesAndPlayersLabel);
			this.Controls.Add(this.PlayersRegisteredLabel);
			this.Controls.Add(this.SeedingAssignsPowersCheckBox);
			this.Controls.Add(this.SeededPlayerCountLabel);
			this.Controls.Add(this.RegisteredCountLabel);
			this.Controls.Add(this.UnregisteredCountLabel);
			this.Controls.Add(this.UnseedButton);
			this.Controls.Add(this.SeedSomeButton);
			this.Controls.Add(this.SeedAllButton);
			this.Controls.Add(this.UnregisterAllButton);
			this.Controls.Add(this.RegisterAllButton);
			this.Controls.Add(this.RegisterForRoundButton);
			this.Controls.Add(this.SeededDataGridView);
			this.Controls.Add(this.WhichPlayersTabControl);
			this.Margin = new System.Windows.Forms.Padding(1);
			this.Name = "RoundControl";
			this.Size = new System.Drawing.Size(957, 535);
			this.OrderPlayersByPanel.ResumeLayout(false);
			this.OrderPlayersByPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SeededDataGridView)).EndInit();
			this.WhichPlayersTabControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.UnregisteredDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RegisteredDataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Button UnseedGameButton;
		private Button SwapButton;
		private Button ReplaceButton;
		private Button ChangeRoundButton;
		private Panel OrderPlayersByPanel;
		private Button MoveGameDownButton;
		private Button MoveGameUpButton;
		private Label ScoringSystemDefaultLabel;
		private ComboBox ScoringSystemComboBox;
		private Label ScoringSystemLabel;
		private Button ViewGamesButton;
		private Button StartGamesButton;
		private Label GamesAndPlayersLabel;
		private Label PlayersRegisteredLabel;
		private CheckBox SeedingAssignsPowersCheckBox;
		private Label SeededPlayerCountLabel;
		private Label RegisteredCountLabel;
		private Label UnregisteredCountLabel;
		private Button UnseedButton;
		private Button SeedSomeButton;
		private Button SeedAllButton;
		private Button UnregisterAllButton;
		private Button RegisterAllButton;
		private Button RegisterForRoundButton;
		private DataGridView SeededDataGridView;
		private TabControl WhichPlayersTabControl;
		private TabPage TournamentPlayersTabPage;
		private TabPage AllPlayersTabPage;
		private RadioButton LastNameRadioButton;
		private RadioButton FirstNameRadioButton;
		private Label OrderByLabel;
		private CheckBox SortByScoreCheckBox;
		private DataGridView UnregisteredDataGridView;
		private DataGridView RegisteredDataGridView;
		private Label RoundLockedLabel;
		private Button PrintButton;
		private Button EmailButton;
		private TextBox FindPlayerTextBox;
		private Label FindPlayerLabel;
		private Button NewPlayerButton;
	}
}
