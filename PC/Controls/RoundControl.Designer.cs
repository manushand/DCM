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
			DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			UnseedGameButton = new Button();
			SwapButton = new Button();
			ReplaceButton = new Button();
			ChangeRoundButton = new Button();
			OrderPlayersByPanel = new Panel();
			LastNameRadioButton = new RadioButton();
			FirstNameRadioButton = new RadioButton();
			OrderByLabel = new Label();
			MoveGameDownButton = new Button();
			MoveGameUpButton = new Button();
			ScoringSystemDefaultLabel = new Label();
			ScoringSystemComboBox = new ComboBox();
			ScoringSystemLabel = new Label();
			ViewGamesButton = new Button();
			StartGamesButton = new Button();
			GamesAndPlayersLabel = new Label();
			PlayersRegisteredLabel = new Label();
			SeedingAssignsPowersCheckBox = new CheckBox();
			SeededPlayerCountLabel = new Label();
			RegisteredCountLabel = new Label();
			UnregisteredCountLabel = new Label();
			UnseedButton = new Button();
			SeedSomeButton = new Button();
			SeedAllButton = new Button();
			UnregisterAllButton = new Button();
			RegisterAllButton = new Button();
			RegisterForRoundButton = new Button();
			SeededDataGridView = new DataGridView();
			WhichPlayersTabControl = new TabControl();
			TournamentPlayersTabPage = new TabPage();
			AllPlayersTabPage = new TabPage();
			SortByScoreCheckBox = new CheckBox();
			UnregisteredDataGridView = new DataGridView();
			RegisteredDataGridView = new DataGridView();
			RoundLockedLabel = new Label();
			PrintButton = new Button();
			EmailButton = new Button();
			FindPlayerTextBox = new TextBox();
			FindPlayerLabel = new Label();
			NewPlayerButton = new Button();
			OrderPlayersByPanel.SuspendLayout();
			((ISupportInitialize)SeededDataGridView).BeginInit();
			WhichPlayersTabControl.SuspendLayout();
			((ISupportInitialize)UnregisteredDataGridView).BeginInit();
			((ISupportInitialize)RegisteredDataGridView).BeginInit();
			SuspendLayout();
			// 
			// UnseedGameButton
			// 
			UnseedGameButton.Location = new Point(573, 375);
			UnseedGameButton.Margin = new Padding(4, 3, 4, 3);
			UnseedGameButton.Name = "UnseedGameButton";
			UnseedGameButton.Size = new Size(124, 27);
			UnseedGameButton.TabIndex = 91;
			UnseedGameButton.Text = "◀─ Unseed Game";
			UnseedGameButton.UseVisualStyleBackColor = true;
			UnseedGameButton.Click += UnseedGameButton_Click;
			// 
			// SwapButton
			// 
			SwapButton.Location = new Point(574, 270);
			SwapButton.Margin = new Padding(4, 3, 4, 3);
			SwapButton.Name = "SwapButton";
			SwapButton.Size = new Size(122, 40);
			SwapButton.TabIndex = 89;
			SwapButton.Text = "Swap ┏━▶\r\nTwo   ┗━▶";
			SwapButton.UseVisualStyleBackColor = true;
			SwapButton.Click += SwapButton_Click;
			// 
			// ReplaceButton
			// 
			ReplaceButton.Location = new Point(574, 315);
			ReplaceButton.Margin = new Padding(4, 3, 4, 3);
			ReplaceButton.Name = "ReplaceButton";
			ReplaceButton.Size = new Size(122, 27);
			ReplaceButton.TabIndex = 90;
			ReplaceButton.Text = "◀─ Replace ─▶";
			ReplaceButton.UseVisualStyleBackColor = true;
			ReplaceButton.Click += ReplaceButton_Click;
			// 
			// ChangeRoundButton
			// 
			ChangeRoundButton.BackColor = Color.LightGreen;
			ChangeRoundButton.Location = new Point(1038, 3);
			ChangeRoundButton.Margin = new Padding(4, 3, 4, 3);
			ChangeRoundButton.Name = "ChangeRoundButton";
			ChangeRoundButton.Size = new Size(71, 60);
			ChangeRoundButton.TabIndex = 87;
			ChangeRoundButton.Text = "  Start   ▶\r\n Next   ▶\r\nRound ▶";
			ChangeRoundButton.TextAlign = ContentAlignment.MiddleRight;
			ChangeRoundButton.UseVisualStyleBackColor = false;
			ChangeRoundButton.Click += ChangeRoundButton_Click;
			// 
			// OrderPlayersByPanel
			// 
			OrderPlayersByPanel.Controls.Add(LastNameRadioButton);
			OrderPlayersByPanel.Controls.Add(FirstNameRadioButton);
			OrderPlayersByPanel.Controls.Add(OrderByLabel);
			OrderPlayersByPanel.Location = new Point(117, 579);
			OrderPlayersByPanel.Margin = new Padding(4, 3, 4, 3);
			OrderPlayersByPanel.Name = "OrderPlayersByPanel";
			OrderPlayersByPanel.Size = new Size(306, 29);
			OrderPlayersByPanel.TabIndex = 86;
			// 
			// LastNameRadioButton
			// 
			LastNameRadioButton.AutoSize = true;
			LastNameRadioButton.Location = new Point(211, 5);
			LastNameRadioButton.Margin = new Padding(4, 3, 4, 3);
			LastNameRadioButton.Name = "LastNameRadioButton";
			LastNameRadioButton.Size = new Size(81, 19);
			LastNameRadioButton.TabIndex = 17;
			LastNameRadioButton.TabStop = true;
			LastNameRadioButton.Text = "Last Name";
			LastNameRadioButton.UseVisualStyleBackColor = true;
			LastNameRadioButton.CheckedChanged += NameSortControl_CheckedChanged;
			// 
			// FirstNameRadioButton
			// 
			FirstNameRadioButton.AutoSize = true;
			FirstNameRadioButton.Location = new Point(121, 5);
			FirstNameRadioButton.Margin = new Padding(4, 3, 4, 3);
			FirstNameRadioButton.Name = "FirstNameRadioButton";
			FirstNameRadioButton.Size = new Size(82, 19);
			FirstNameRadioButton.TabIndex = 16;
			FirstNameRadioButton.TabStop = true;
			FirstNameRadioButton.Text = "First Name";
			FirstNameRadioButton.UseVisualStyleBackColor = true;
			FirstNameRadioButton.CheckedChanged += NameSortControl_CheckedChanged;
			// 
			// OrderByLabel
			// 
			OrderByLabel.AutoSize = true;
			OrderByLabel.Location = new Point(12, 7);
			OrderByLabel.Margin = new Padding(4, 0, 4, 0);
			OrderByLabel.Name = "OrderByLabel";
			OrderByLabel.Size = new Size(96, 15);
			OrderByLabel.TabIndex = 15;
			OrderByLabel.Text = "Order Players By:";
			// 
			// MoveGameDownButton
			// 
			MoveGameDownButton.Location = new Point(1037, 315);
			MoveGameDownButton.Margin = new Padding(4, 3, 4, 3);
			MoveGameDownButton.Name = "MoveGameDownButton";
			MoveGameDownButton.Size = new Size(72, 40);
			MoveGameDownButton.TabIndex = 85;
			MoveGameDownButton.Text = "Game\r\n▼";
			MoveGameDownButton.UseVisualStyleBackColor = true;
			MoveGameDownButton.Click += MoveGameButton_Click;
			// 
			// MoveGameUpButton
			// 
			MoveGameUpButton.Location = new Point(1037, 270);
			MoveGameUpButton.Margin = new Padding(4, 3, 4, 3);
			MoveGameUpButton.Name = "MoveGameUpButton";
			MoveGameUpButton.Size = new Size(72, 40);
			MoveGameUpButton.TabIndex = 84;
			MoveGameUpButton.Text = "▲\r\nGame";
			MoveGameUpButton.UseVisualStyleBackColor = true;
			MoveGameUpButton.Click += MoveGameButton_Click;
			// 
			// ScoringSystemDefaultLabel
			// 
			ScoringSystemDefaultLabel.AutoSize = true;
			ScoringSystemDefaultLabel.Location = new Point(387, 9);
			ScoringSystemDefaultLabel.Margin = new Padding(4, 0, 4, 0);
			ScoringSystemDefaultLabel.Name = "ScoringSystemDefaultLabel";
			ScoringSystemDefaultLabel.Size = new Size(52, 15);
			ScoringSystemDefaultLabel.TabIndex = 83;
			ScoringSystemDefaultLabel.Text = "(default)";
			// 
			// ScoringSystemComboBox
			// 
			ScoringSystemComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			ScoringSystemComboBox.FormattingEnabled = true;
			ScoringSystemComboBox.Location = new Point(156, 6);
			ScoringSystemComboBox.Margin = new Padding(4, 3, 4, 3);
			ScoringSystemComboBox.Name = "ScoringSystemComboBox";
			ScoringSystemComboBox.Size = new Size(222, 23);
			ScoringSystemComboBox.TabIndex = 82;
			ScoringSystemComboBox.SelectedIndexChanged += ScoringSystemComboBox_SelectedIndexChanged;
			ScoringSystemComboBox.EnabledChanged += ComboBox_EnabledChanged;
			// 
			// ScoringSystemLabel
			// 
			ScoringSystemLabel.AutoSize = true;
			ScoringSystemLabel.Location = new Point(16, 9);
			ScoringSystemLabel.Margin = new Padding(4, 0, 4, 0);
			ScoringSystemLabel.Name = "ScoringSystemLabel";
			ScoringSystemLabel.Size = new Size(129, 15);
			ScoringSystemLabel.TabIndex = 81;
			ScoringSystemLabel.Text = "Round Scoring System:";
			// 
			// ViewGamesButton
			// 
			ViewGamesButton.Location = new Point(707, 580);
			ViewGamesButton.Margin = new Padding(4, 3, 4, 3);
			ViewGamesButton.Name = "ViewGamesButton";
			ViewGamesButton.Size = new Size(323, 27);
			ViewGamesButton.TabIndex = 80;
			ViewGamesButton.Text = "View Games…";
			ViewGamesButton.UseVisualStyleBackColor = true;
			ViewGamesButton.Click += ViewGamesButton_Click;
			// 
			// StartGamesButton
			// 
			StartGamesButton.Location = new Point(704, 3);
			StartGamesButton.Margin = new Padding(4, 3, 4, 3);
			StartGamesButton.Name = "StartGamesButton";
			StartGamesButton.Size = new Size(327, 27);
			StartGamesButton.TabIndex = 78;
			StartGamesButton.Text = "Start Games…";
			StartGamesButton.UseVisualStyleBackColor = true;
			StartGamesButton.Click += StartGamesButton_Click;
			// 
			// GamesAndPlayersLabel
			// 
			GamesAndPlayersLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			GamesAndPlayersLabel.Location = new Point(700, 40);
			GamesAndPlayersLabel.Margin = new Padding(4, 0, 4, 0);
			GamesAndPlayersLabel.Name = "GamesAndPlayersLabel";
			GamesAndPlayersLabel.Size = new Size(330, 27);
			GamesAndPlayersLabel.TabIndex = 77;
			GamesAndPlayersLabel.Text = "Games and Player Assignments for Round 1";
			GamesAndPlayersLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// PlayersRegisteredLabel
			// 
			PlayersRegisteredLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			PlayersRegisteredLabel.Location = new Point(343, 40);
			PlayersRegisteredLabel.Margin = new Padding(4, 0, 4, 0);
			PlayersRegisteredLabel.Name = "PlayersRegisteredLabel";
			PlayersRegisteredLabel.Size = new Size(223, 27);
			PlayersRegisteredLabel.TabIndex = 76;
			PlayersRegisteredLabel.Text = "Players Registered for Round 1";
			PlayersRegisteredLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// SeedingAssignsPowersCheckBox
			// 
			SeedingAssignsPowersCheckBox.AutoSize = true;
			SeedingAssignsPowersCheckBox.CheckAlign = ContentAlignment.TopLeft;
			SeedingAssignsPowersCheckBox.Location = new Point(595, 92);
			SeedingAssignsPowersCheckBox.Margin = new Padding(4, 3, 4, 3);
			SeedingAssignsPowersCheckBox.Name = "SeedingAssignsPowersCheckBox";
			SeedingAssignsPowersCheckBox.Size = new Size(68, 49);
			SeedingAssignsPowersCheckBox.TabIndex = 75;
			SeedingAssignsPowersCheckBox.Text = "Seeding\r\nAssigns\r\nPowers";
			SeedingAssignsPowersCheckBox.UseVisualStyleBackColor = true;
			// 
			// SeededPlayerCountLabel
			// 
			SeededPlayerCountLabel.Location = new Point(704, 546);
			SeededPlayerCountLabel.Margin = new Padding(4, 0, 4, 0);
			SeededPlayerCountLabel.Name = "SeededPlayerCountLabel";
			SeededPlayerCountLabel.Size = new Size(327, 27);
			SeededPlayerCountLabel.TabIndex = 74;
			SeededPlayerCountLabel.Text = "nnn Players in xx Games";
			SeededPlayerCountLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// RegisteredCountLabel
			// 
			RegisteredCountLabel.Location = new Point(343, 546);
			RegisteredCountLabel.Margin = new Padding(4, 0, 4, 0);
			RegisteredCountLabel.Name = "RegisteredCountLabel";
			RegisteredCountLabel.Size = new Size(223, 27);
			RegisteredCountLabel.TabIndex = 73;
			RegisteredCountLabel.Text = "nnn Players Listed";
			RegisteredCountLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// UnregisteredCountLabel
			// 
			UnregisteredCountLabel.Location = new Point(18, 546);
			UnregisteredCountLabel.Margin = new Padding(4, 0, 4, 0);
			UnregisteredCountLabel.Name = "UnregisteredCountLabel";
			UnregisteredCountLabel.Size = new Size(223, 27);
			UnregisteredCountLabel.TabIndex = 72;
			UnregisteredCountLabel.Text = "nnn Players Listed";
			UnregisteredCountLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// UnseedButton
			// 
			UnseedButton.Location = new Point(573, 408);
			UnseedButton.Margin = new Padding(4, 3, 4, 3);
			UnseedButton.Name = "UnseedButton";
			UnseedButton.Size = new Size(124, 27);
			UnseedButton.TabIndex = 71;
			UnseedButton.Text = "◀─── Unseed All";
			UnseedButton.UseVisualStyleBackColor = true;
			UnseedButton.Click += UnseedButton_Click;
			// 
			// SeedSomeButton
			// 
			SeedSomeButton.Location = new Point(573, 213);
			SeedSomeButton.Margin = new Padding(4, 3, 4, 3);
			SeedSomeButton.Name = "SeedSomeButton";
			SeedSomeButton.Size = new Size(124, 27);
			SeedSomeButton.TabIndex = 70;
			SeedSomeButton.Text = "Seed Some ───▶";
			SeedSomeButton.UseVisualStyleBackColor = true;
			SeedSomeButton.Click += SeedButton_Click;
			// 
			// SeedAllButton
			// 
			SeedAllButton.Location = new Point(573, 180);
			SeedAllButton.Margin = new Padding(4, 3, 4, 3);
			SeedAllButton.Name = "SeedAllButton";
			SeedAllButton.Size = new Size(124, 27);
			SeedAllButton.TabIndex = 69;
			SeedAllButton.Text = "Seed All ─────▶";
			SeedAllButton.UseVisualStyleBackColor = true;
			SeedAllButton.Click += SeedButton_Click;
			// 
			// UnregisterAllButton
			// 
			UnregisterAllButton.Location = new Point(247, 408);
			UnregisterAllButton.Margin = new Padding(4, 3, 4, 3);
			UnregisterAllButton.Name = "UnregisterAllButton";
			UnregisterAllButton.Size = new Size(89, 27);
			UnregisterAllButton.TabIndex = 68;
			UnregisterAllButton.Text = "◀── All";
			UnregisterAllButton.UseVisualStyleBackColor = true;
			UnregisterAllButton.Click += UnregisterAllButton_Click;
			// 
			// RegisterAllButton
			// 
			RegisterAllButton.Location = new Point(247, 375);
			RegisterAllButton.Margin = new Padding(4, 3, 4, 3);
			RegisterAllButton.Name = "RegisterAllButton";
			RegisterAllButton.Size = new Size(89, 27);
			RegisterAllButton.TabIndex = 67;
			RegisterAllButton.Text = "All ──▶";
			RegisterAllButton.UseVisualStyleBackColor = true;
			RegisterAllButton.Click += RegisterAllButton_Click;
			// 
			// RegisterForRoundButton
			// 
			RegisterForRoundButton.Location = new Point(247, 213);
			RegisterForRoundButton.Margin = new Padding(4, 3, 4, 3);
			RegisterForRoundButton.Name = "RegisterForRoundButton";
			RegisterForRoundButton.Size = new Size(89, 27);
			RegisterForRoundButton.TabIndex = 66;
			RegisterForRoundButton.Text = "Register ─▶";
			RegisterForRoundButton.UseVisualStyleBackColor = true;
			RegisterForRoundButton.Click += RegisterButton_Click;
			// 
			// SeededDataGridView
			// 
			SeededDataGridView.AllowUserToAddRows = false;
			SeededDataGridView.AllowUserToDeleteRows = false;
			SeededDataGridView.AllowUserToResizeColumns = false;
			SeededDataGridView.AllowUserToResizeRows = false;
			SeededDataGridView.BackgroundColor = SystemColors.Window;
			SeededDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle1.BackColor = SystemColors.Control;
			dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
			dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
			dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
			SeededDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			SeededDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			SeededDataGridView.EnableHeadersVisualStyles = false;
			SeededDataGridView.Location = new Point(704, 74);
			SeededDataGridView.Margin = new Padding(4, 3, 4, 3);
			SeededDataGridView.Name = "SeededDataGridView";
			SeededDataGridView.ReadOnly = true;
			SeededDataGridView.RowHeadersVisible = false;
			SeededDataGridView.RowTemplate.Height = 16;
			SeededDataGridView.RowTemplate.ReadOnly = true;
			SeededDataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
			SeededDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			SeededDataGridView.ShowCellToolTips = false;
			SeededDataGridView.Size = new Size(327, 470);
			SeededDataGridView.TabIndex = 65;
			SeededDataGridView.CellDoubleClick += ViewGamesButton_Click;
			SeededDataGridView.DataBindingComplete += SeededDataGridView_DataBindingComplete;
			SeededDataGridView.SelectionChanged += SeededDataGridView_SelectionChanged;
			// 
			// WhichPlayersTabControl
			// 
			WhichPlayersTabControl.Controls.Add(TournamentPlayersTabPage);
			WhichPlayersTabControl.Controls.Add(AllPlayersTabPage);
			WhichPlayersTabControl.Location = new Point(12, 48);
			WhichPlayersTabControl.Margin = new Padding(1);
			WhichPlayersTabControl.Name = "WhichPlayersTabControl";
			WhichPlayersTabControl.SelectedIndex = 0;
			WhichPlayersTabControl.Size = new Size(232, 496);
			WhichPlayersTabControl.TabIndex = 88;
			WhichPlayersTabControl.SelectedIndexChanged += WhichPlayersTabControl_SelectedIndexChanged;
			// 
			// TournamentPlayersTabPage
			// 
			TournamentPlayersTabPage.Location = new Point(4, 24);
			TournamentPlayersTabPage.Margin = new Padding(1);
			TournamentPlayersTabPage.Name = "TournamentPlayersTabPage";
			TournamentPlayersTabPage.Padding = new Padding(1);
			TournamentPlayersTabPage.Size = new Size(224, 468);
			TournamentPlayersTabPage.TabIndex = 0;
			TournamentPlayersTabPage.Text = "Tournament Players";
			TournamentPlayersTabPage.UseVisualStyleBackColor = true;
			// 
			// AllPlayersTabPage
			// 
			AllPlayersTabPage.Location = new Point(4, 24);
			AllPlayersTabPage.Margin = new Padding(1);
			AllPlayersTabPage.Name = "AllPlayersTabPage";
			AllPlayersTabPage.Padding = new Padding(1);
			AllPlayersTabPage.Size = new Size(224, 468);
			AllPlayersTabPage.TabIndex = 1;
			AllPlayersTabPage.Text = "All Players";
			AllPlayersTabPage.UseVisualStyleBackColor = true;
			// 
			// SortByScoreCheckBox
			// 
			SortByScoreCheckBox.AutoSize = true;
			SortByScoreCheckBox.Checked = true;
			SortByScoreCheckBox.CheckState = CheckState.Checked;
			SortByScoreCheckBox.Location = new Point(424, 585);
			SortByScoreCheckBox.Margin = new Padding(4, 3, 4, 3);
			SortByScoreCheckBox.Name = "SortByScoreCheckBox";
			SortByScoreCheckBox.Size = new Size(55, 19);
			SortByScoreCheckBox.TabIndex = 92;
			SortByScoreCheckBox.Text = "Score";
			SortByScoreCheckBox.UseVisualStyleBackColor = true;
			SortByScoreCheckBox.Click += NameSortControl_CheckedChanged;
			// 
			// UnregisteredDataGridView
			// 
			UnregisteredDataGridView.AllowUserToAddRows = false;
			UnregisteredDataGridView.AllowUserToDeleteRows = false;
			UnregisteredDataGridView.AllowUserToResizeColumns = false;
			UnregisteredDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle2.BackColor = SystemColors.ControlLight;
			UnregisteredDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			UnregisteredDataGridView.BackgroundColor = SystemColors.Window;
			UnregisteredDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
			UnregisteredDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			UnregisteredDataGridView.ColumnHeadersVisible = false;
			UnregisteredDataGridView.Location = new Point(16, 74);
			UnregisteredDataGridView.Margin = new Padding(4, 3, 4, 3);
			UnregisteredDataGridView.Name = "UnregisteredDataGridView";
			UnregisteredDataGridView.ReadOnly = true;
			UnregisteredDataGridView.RowHeadersVisible = false;
			UnregisteredDataGridView.RowTemplate.Height = 16;
			UnregisteredDataGridView.RowTemplate.ReadOnly = true;
			UnregisteredDataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
			UnregisteredDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			UnregisteredDataGridView.ShowCellToolTips = false;
			UnregisteredDataGridView.Size = new Size(223, 467);
			UnregisteredDataGridView.TabIndex = 0;
			UnregisteredDataGridView.CellDoubleClick += RegisterButton_Click;
			UnregisteredDataGridView.DataBindingComplete += RegistrableDataGridView_DataBindingComplete;
			UnregisteredDataGridView.SelectionChanged += RegistrableDataGridView_SelectionChanged;
			// 
			// RegisteredDataGridView
			// 
			RegisteredDataGridView.AllowUserToAddRows = false;
			RegisteredDataGridView.AllowUserToDeleteRows = false;
			RegisteredDataGridView.AllowUserToResizeColumns = false;
			RegisteredDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle3.BackColor = SystemColors.ControlLight;
			RegisteredDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
			RegisteredDataGridView.BackgroundColor = SystemColors.Window;
			RegisteredDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
			RegisteredDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			RegisteredDataGridView.ColumnHeadersVisible = false;
			RegisteredDataGridView.Location = new Point(346, 74);
			RegisteredDataGridView.Margin = new Padding(4, 3, 4, 3);
			RegisteredDataGridView.Name = "RegisteredDataGridView";
			RegisteredDataGridView.ReadOnly = true;
			RegisteredDataGridView.RowHeadersVisible = false;
			RegisteredDataGridView.RowTemplate.Height = 16;
			RegisteredDataGridView.RowTemplate.ReadOnly = true;
			RegisteredDataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
			RegisteredDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			RegisteredDataGridView.ShowCellToolTips = false;
			RegisteredDataGridView.Size = new Size(219, 467);
			RegisteredDataGridView.TabIndex = 93;
			RegisteredDataGridView.DataBindingComplete += RegistrableDataGridView_DataBindingComplete;
			RegisteredDataGridView.SelectionChanged += RegistrableDataGridView_SelectionChanged;
			// 
			// RoundLockedLabel
			// 
			RoundLockedLabel.AutoSize = true;
			RoundLockedLabel.BackColor = Color.LightGreen;
			RoundLockedLabel.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			RoundLockedLabel.ForeColor = Color.Red;
			RoundLockedLabel.Location = new Point(456, 3);
			RoundLockedLabel.Margin = new Padding(4, 0, 4, 0);
			RoundLockedLabel.Name = "RoundLockedLabel";
			RoundLockedLabel.Size = new Size(180, 24);
			RoundLockedLabel.TabIndex = 94;
			RoundLockedLabel.Text = "Round is Finished";
			// 
			// PrintButton
			// 
			PrintButton.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			PrintButton.Location = new Point(1037, 498);
			PrintButton.Margin = new Padding(4, 3, 4, 3);
			PrintButton.Name = "PrintButton";
			PrintButton.Size = new Size(72, 27);
			PrintButton.TabIndex = 95;
			PrintButton.Text = "Print…";
			PrintButton.UseVisualStyleBackColor = true;
			PrintButton.Click += PrintButton_Click;
			// 
			// EmailButton
			// 
			EmailButton.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			EmailButton.Location = new Point(1037, 465);
			EmailButton.Margin = new Padding(4, 3, 4, 3);
			EmailButton.Name = "EmailButton";
			EmailButton.Size = new Size(72, 27);
			EmailButton.TabIndex = 96;
			EmailButton.Text = "Email…";
			EmailButton.UseVisualStyleBackColor = true;
			// 
			// FindPlayerTextBox
			// 
			FindPlayerTextBox.Location = new Point(1038, 159);
			FindPlayerTextBox.Margin = new Padding(4, 3, 4, 3);
			FindPlayerTextBox.Name = "FindPlayerTextBox";
			FindPlayerTextBox.Size = new Size(70, 23);
			FindPlayerTextBox.TabIndex = 97;
			FindPlayerTextBox.TextChanged += FindPlayerTextBox_TextChanged;
			// 
			// FindPlayerLabel
			// 
			FindPlayerLabel.Location = new Point(1036, 92);
			FindPlayerLabel.Margin = new Padding(4, 0, 4, 0);
			FindPlayerLabel.Name = "FindPlayerLabel";
			FindPlayerLabel.Size = new Size(72, 65);
			FindPlayerLabel.TabIndex = 98;
			FindPlayerLabel.Text = "Find Assigned Player By Name:";
			// 
			// NewPlayerButton
			// 
			NewPlayerButton.Location = new Point(247, 92);
			NewPlayerButton.Margin = new Padding(4, 3, 4, 3);
			NewPlayerButton.Name = "NewPlayerButton";
			NewPlayerButton.Size = new Size(88, 27);
			NewPlayerButton.TabIndex = 99;
			NewPlayerButton.Text = "New Player";
			NewPlayerButton.UseVisualStyleBackColor = true;
			NewPlayerButton.Click += NewPlayerButton_Click;
			// 
			// RoundControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(NewPlayerButton);
			Controls.Add(FindPlayerLabel);
			Controls.Add(FindPlayerTextBox);
			Controls.Add(EmailButton);
			Controls.Add(PrintButton);
			Controls.Add(UnregisteredDataGridView);
			Controls.Add(RoundLockedLabel);
			Controls.Add(RegisteredDataGridView);
			Controls.Add(SortByScoreCheckBox);
			Controls.Add(UnseedGameButton);
			Controls.Add(SwapButton);
			Controls.Add(ReplaceButton);
			Controls.Add(ChangeRoundButton);
			Controls.Add(OrderPlayersByPanel);
			Controls.Add(MoveGameDownButton);
			Controls.Add(MoveGameUpButton);
			Controls.Add(ScoringSystemDefaultLabel);
			Controls.Add(ScoringSystemComboBox);
			Controls.Add(ScoringSystemLabel);
			Controls.Add(ViewGamesButton);
			Controls.Add(StartGamesButton);
			Controls.Add(GamesAndPlayersLabel);
			Controls.Add(PlayersRegisteredLabel);
			Controls.Add(SeedingAssignsPowersCheckBox);
			Controls.Add(SeededPlayerCountLabel);
			Controls.Add(RegisteredCountLabel);
			Controls.Add(UnregisteredCountLabel);
			Controls.Add(UnseedButton);
			Controls.Add(SeedSomeButton);
			Controls.Add(SeedAllButton);
			Controls.Add(UnregisterAllButton);
			Controls.Add(RegisterAllButton);
			Controls.Add(RegisterForRoundButton);
			Controls.Add(SeededDataGridView);
			Controls.Add(WhichPlayersTabControl);
			Margin = new Padding(1);
			Name = "RoundControl";
			Size = new Size(1116, 617);
			OrderPlayersByPanel.ResumeLayout(false);
			OrderPlayersByPanel.PerformLayout();
			((ISupportInitialize)SeededDataGridView).EndInit();
			WhichPlayersTabControl.ResumeLayout(false);
			((ISupportInitialize)UnregisteredDataGridView).EndInit();
			((ISupportInitialize)RegisteredDataGridView).EndInit();
			ResumeLayout(false);
			PerformLayout();

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
