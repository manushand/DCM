using PC.Controls;

namespace PC.Forms
{
	partial class GroupGamesForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.OrderByNamePanel = new System.Windows.Forms.Panel();
			this.LastNameRegistrationTabRadioButton = new System.Windows.Forms.RadioButton();
			this.FirstNameRegistrationTabRadioButton = new System.Windows.Forms.RadioButton();
			this.OrderByNameLabel = new System.Windows.Forms.Label();
			this.GameStatusComboBox = new System.Windows.Forms.ComboBox();
			this.GameStatusLabel = new System.Windows.Forms.Label();
			this.NewGameButton = new System.Windows.Forms.Button();
			this.GameNameLabel = new System.Windows.Forms.Label();
			this.GameNameTextBox = new System.Windows.Forms.TextBox();
			this.GameDateLabel = new System.Windows.Forms.Label();
			this.GameDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.GroupGamesDataGridView = new System.Windows.Forms.DataGridView();
			this.DeleteGameButton = new System.Windows.Forms.Button();
			this.GameInErrorButton = new System.Windows.Forms.Button();
			this.TotalScoreLabel = new System.Windows.Forms.Label();
			this.ScoreTotalBarLabel = new System.Windows.Forms.Label();
			this.TotalScoreTextLabel = new System.Windows.Forms.Label();
			this.ScoreColumnHeaderLabel = new System.Windows.Forms.Label();
			this.PlayersPanel = new System.Windows.Forms.Panel();
			this.PlayerColumnHeaderLabel = new System.Windows.Forms.Label();
			this.TurkeyPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.RussiaPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.ItalyPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.GermanyPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.FrancePlayerComboBox = new System.Windows.Forms.ComboBox();
			this.EnglandPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.AustriaPlayerComboBox = new System.Windows.Forms.ComboBox();
			this.ScoresPanel = new System.Windows.Forms.Panel();
			this.AustriaScoreLabel = new System.Windows.Forms.Label();
			this.EnglandScoreLabel = new System.Windows.Forms.Label();
			this.FranceScoreLabel = new System.Windows.Forms.Label();
			this.GermanyScoreLabel = new System.Windows.Forms.Label();
			this.ItalyScoreLabel = new System.Windows.Forms.Label();
			this.RussiaScoreLabel = new System.Windows.Forms.Label();
			this.TurkeyScoreLabel = new System.Windows.Forms.Label();
			this.GameControl = new GameControl();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.OrderByNamePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GroupGamesDataGridView)).BeginInit();
			this.PlayersPanel.SuspendLayout();
			this.ScoresPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// OrderByNamePanel
			// 
			this.OrderByNamePanel.Controls.Add(this.LastNameRegistrationTabRadioButton);
			this.OrderByNamePanel.Controls.Add(this.FirstNameRegistrationTabRadioButton);
			this.OrderByNamePanel.Controls.Add(this.OrderByNameLabel);
			this.OrderByNamePanel.Location = new System.Drawing.Point(268, 275);
			this.OrderByNamePanel.Name = "OrderByNamePanel";
			this.OrderByNamePanel.Size = new System.Drawing.Size(243, 30);
			this.OrderByNamePanel.TabIndex = 67;
			// 
			// LastNameRegistrationTabRadioButton
			// 
			this.LastNameRegistrationTabRadioButton.AutoSize = true;
			this.LastNameRegistrationTabRadioButton.Location = new System.Drawing.Point(166, 6);
			this.LastNameRegistrationTabRadioButton.Name = "LastNameRegistrationTabRadioButton";
			this.LastNameRegistrationTabRadioButton.Size = new System.Drawing.Size(76, 17);
			this.LastNameRegistrationTabRadioButton.TabIndex = 17;
			this.LastNameRegistrationTabRadioButton.TabStop = true;
			this.LastNameRegistrationTabRadioButton.Text = "Last Name";
			this.LastNameRegistrationTabRadioButton.UseVisualStyleBackColor = true;
			this.LastNameRegistrationTabRadioButton.CheckedChanged += new System.EventHandler(this.NameRegistrationTabRadioButton_CheckedChanged);
			// 
			// FirstNameRegistrationTabRadioButton
			// 
			this.FirstNameRegistrationTabRadioButton.AutoSize = true;
			this.FirstNameRegistrationTabRadioButton.Location = new System.Drawing.Point(93, 6);
			this.FirstNameRegistrationTabRadioButton.Name = "FirstNameRegistrationTabRadioButton";
			this.FirstNameRegistrationTabRadioButton.Size = new System.Drawing.Size(75, 17);
			this.FirstNameRegistrationTabRadioButton.TabIndex = 16;
			this.FirstNameRegistrationTabRadioButton.TabStop = true;
			this.FirstNameRegistrationTabRadioButton.Text = "First Name";
			this.FirstNameRegistrationTabRadioButton.UseVisualStyleBackColor = true;
			this.FirstNameRegistrationTabRadioButton.CheckedChanged += new System.EventHandler(this.NameRegistrationTabRadioButton_CheckedChanged);
			// 
			// OrderByNameLabel
			// 
			this.OrderByNameLabel.AutoSize = true;
			this.OrderByNameLabel.Location = new System.Drawing.Point(1, 7);
			this.OrderByNameLabel.Name = "OrderByNameLabel";
			this.OrderByNameLabel.Size = new System.Drawing.Size(88, 13);
			this.OrderByNameLabel.TabIndex = 15;
			this.OrderByNameLabel.Text = "Order Players By:";
			// 
			// GameStatusComboBox
			// 
			this.GameStatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GameStatusComboBox.FormattingEnabled = true;
			this.GameStatusComboBox.Items.AddRange(new object[] {
            "        Seeded",
            "◯ = Underway",
            "✔ = Finished"});
			this.GameStatusComboBox.Location = new System.Drawing.Point(728, 6);
			this.GameStatusComboBox.Margin = new System.Windows.Forms.Padding(1);
			this.GameStatusComboBox.Name = "GameStatusComboBox";
			this.GameStatusComboBox.Size = new System.Drawing.Size(109, 21);
			this.GameStatusComboBox.TabIndex = 87;
			this.GameStatusComboBox.SelectedIndexChanged += new System.EventHandler(this.GameStatusComboBox_SelectedIndexChanged);
			// 
			// GameStatusLabel
			// 
			this.GameStatusLabel.AutoSize = true;
			this.GameStatusLabel.Location = new System.Drawing.Point(653, 10);
			this.GameStatusLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.GameStatusLabel.Name = "GameStatusLabel";
			this.GameStatusLabel.Size = new System.Drawing.Size(71, 13);
			this.GameStatusLabel.TabIndex = 86;
			this.GameStatusLabel.Text = "Game Status:";
			// 
			// NewGameButton
			// 
			this.NewGameButton.Location = new System.Drawing.Point(13, 12);
			this.NewGameButton.Name = "NewGameButton";
			this.NewGameButton.Size = new System.Drawing.Size(118, 23);
			this.NewGameButton.TabIndex = 88;
			this.NewGameButton.Text = "New Game";
			this.NewGameButton.UseVisualStyleBackColor = true;
			this.NewGameButton.Click += new System.EventHandler(this.NewGameButton_Click);
			// 
			// GameNameLabel
			// 
			this.GameNameLabel.AutoSize = true;
			this.GameNameLabel.Location = new System.Drawing.Point(268, 40);
			this.GameNameLabel.Name = "GameNameLabel";
			this.GameNameLabel.Size = new System.Drawing.Size(69, 13);
			this.GameNameLabel.TabIndex = 90;
			this.GameNameLabel.Text = "Game Name:";
			// 
			// GameNameTextBox
			// 
			this.GameNameTextBox.Location = new System.Drawing.Point(337, 36);
			this.GameNameTextBox.Name = "GameNameTextBox";
			this.GameNameTextBox.Size = new System.Drawing.Size(174, 20);
			this.GameNameTextBox.TabIndex = 91;
			this.GameNameTextBox.Leave += new System.EventHandler(this.GameNameTextBox_Leave);
			// 
			// GameDateLabel
			// 
			this.GameDateLabel.AutoSize = true;
			this.GameDateLabel.Location = new System.Drawing.Point(268, 10);
			this.GameDateLabel.Name = "GameDateLabel";
			this.GameDateLabel.Size = new System.Drawing.Size(64, 13);
			this.GameDateLabel.TabIndex = 92;
			this.GameDateLabel.Text = "Game Date:";
			// 
			// GameDateTimePicker
			// 
			this.GameDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.GameDateTimePicker.Location = new System.Drawing.Point(337, 7);
			this.GameDateTimePicker.Name = "GameDateTimePicker";
			this.GameDateTimePicker.Size = new System.Drawing.Size(109, 20);
			this.GameDateTimePicker.TabIndex = 93;
			this.GameDateTimePicker.ValueChanged += new System.EventHandler(this.GameDateTimePicker_ValueChanged);
			// 
			// GroupGamesDataGridView
			// 
			this.GroupGamesDataGridView.AllowUserToAddRows = false;
			this.GroupGamesDataGridView.AllowUserToDeleteRows = false;
			this.GroupGamesDataGridView.AllowUserToResizeColumns = false;
			this.GroupGamesDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.GroupGamesDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.GroupGamesDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.GroupGamesDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.GroupGamesDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.GroupGamesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.GroupGamesDataGridView.EnableHeadersVisualStyles = false;
			this.GroupGamesDataGridView.Location = new System.Drawing.Point(13, 41);
			this.GroupGamesDataGridView.MultiSelect = false;
			this.GroupGamesDataGridView.Name = "GroupGamesDataGridView";
			this.GroupGamesDataGridView.ReadOnly = true;
			this.GroupGamesDataGridView.RowHeadersVisible = false;
			this.GroupGamesDataGridView.RowTemplate.Height = 16;
			this.GroupGamesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.GroupGamesDataGridView.ShowCellToolTips = false;
			this.GroupGamesDataGridView.Size = new System.Drawing.Size(236, 257);
			this.GroupGamesDataGridView.TabIndex = 94;
			this.GroupGamesDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GroupGamesDataGridView_CellClick);
			this.GroupGamesDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.GroupGamesDataGridView_DataBindingComplete);
			// 
			// DeleteGameButton
			// 
			this.DeleteGameButton.Location = new System.Drawing.Point(131, 12);
			this.DeleteGameButton.Name = "DeleteGameButton";
			this.DeleteGameButton.Size = new System.Drawing.Size(118, 23);
			this.DeleteGameButton.TabIndex = 95;
			this.DeleteGameButton.Text = "Delete Game";
			this.DeleteGameButton.UseVisualStyleBackColor = true;
			this.DeleteGameButton.Click += new System.EventHandler(this.DeleteGameButton_Click);
			// 
			// GameInErrorButton
			// 
			this.GameInErrorButton.BackColor = System.Drawing.Color.Firebrick;
			this.GameInErrorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GameInErrorButton.ForeColor = System.Drawing.Color.Yellow;
			this.GameInErrorButton.Location = new System.Drawing.Point(527, 280);
			this.GameInErrorButton.Name = "GameInErrorButton";
			this.GameInErrorButton.Size = new System.Drawing.Size(83, 23);
			this.GameInErrorButton.TabIndex = 107;
			this.GameInErrorButton.Text = "ERROR";
			this.GameInErrorButton.UseVisualStyleBackColor = false;
			this.GameInErrorButton.Click += new System.EventHandler(this.GameInErrorButton_Click);
			// 
			// TotalScoreLabel
			// 
			this.TotalScoreLabel.Location = new System.Drawing.Point(796, 284);
			this.TotalScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TotalScoreLabel.Name = "TotalScoreLabel";
			this.TotalScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.TotalScoreLabel.TabIndex = 106;
			this.TotalScoreLabel.Text = "0";
			this.TotalScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ScoreTotalBarLabel
			// 
			this.ScoreTotalBarLabel.AutoSize = true;
			this.ScoreTotalBarLabel.Location = new System.Drawing.Point(796, 267);
			this.ScoreTotalBarLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ScoreTotalBarLabel.Name = "ScoreTotalBarLabel";
			this.ScoreTotalBarLabel.Size = new System.Drawing.Size(47, 13);
			this.ScoreTotalBarLabel.TabIndex = 98;
			this.ScoreTotalBarLabel.Text = "─────";
			// 
			// TotalScoreTextLabel
			// 
			this.TotalScoreTextLabel.AutoSize = true;
			this.TotalScoreTextLabel.Location = new System.Drawing.Point(678, 285);
			this.TotalScoreTextLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TotalScoreTextLabel.Name = "TotalScoreTextLabel";
			this.TotalScoreTextLabel.Size = new System.Drawing.Size(111, 13);
			this.TotalScoreTextLabel.TabIndex = 97;
			this.TotalScoreTextLabel.Text = "Total Points Awarded:";
			this.TotalScoreTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ScoreColumnHeaderLabel
			// 
			this.ScoreColumnHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScoreColumnHeaderLabel.Location = new System.Drawing.Point(793, 66);
			this.ScoreColumnHeaderLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ScoreColumnHeaderLabel.Name = "ScoreColumnHeaderLabel";
			this.ScoreColumnHeaderLabel.Size = new System.Drawing.Size(50, 13);
			this.ScoreColumnHeaderLabel.TabIndex = 96;
			this.ScoreColumnHeaderLabel.Text = "SCORE";
			this.ScoreColumnHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PlayersPanel
			// 
			this.PlayersPanel.Controls.Add(this.PlayerColumnHeaderLabel);
			this.PlayersPanel.Controls.Add(this.TurkeyPlayerComboBox);
			this.PlayersPanel.Controls.Add(this.RussiaPlayerComboBox);
			this.PlayersPanel.Controls.Add(this.ItalyPlayerComboBox);
			this.PlayersPanel.Controls.Add(this.GermanyPlayerComboBox);
			this.PlayersPanel.Controls.Add(this.FrancePlayerComboBox);
			this.PlayersPanel.Controls.Add(this.EnglandPlayerComboBox);
			this.PlayersPanel.Controls.Add(this.AustriaPlayerComboBox);
			this.PlayersPanel.Location = new System.Drawing.Point(266, 64);
			this.PlayersPanel.Name = "PlayersPanel";
			this.PlayersPanel.Size = new System.Drawing.Size(139, 210);
			this.PlayersPanel.TabIndex = 108;
			// 
			// PlayerColumnHeaderLabel
			// 
			this.PlayerColumnHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PlayerColumnHeaderLabel.Location = new System.Drawing.Point(0, 0);
			this.PlayerColumnHeaderLabel.Name = "PlayerColumnHeaderLabel";
			this.PlayerColumnHeaderLabel.Size = new System.Drawing.Size(139, 16);
			this.PlayerColumnHeaderLabel.TabIndex = 93;
			this.PlayerColumnHeaderLabel.Text = "PLAYER";
			this.PlayerColumnHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TurkeyPlayerComboBox
			// 
			this.TurkeyPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TurkeyPlayerComboBox.FormattingEnabled = true;
			this.TurkeyPlayerComboBox.Location = new System.Drawing.Point(0, 183);
			this.TurkeyPlayerComboBox.Name = "TurkeyPlayerComboBox";
			this.TurkeyPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.TurkeyPlayerComboBox.TabIndex = 92;
			this.TurkeyPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.TurkeyPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// RussiaPlayerComboBox
			// 
			this.RussiaPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RussiaPlayerComboBox.FormattingEnabled = true;
			this.RussiaPlayerComboBox.Location = new System.Drawing.Point(0, 156);
			this.RussiaPlayerComboBox.Name = "RussiaPlayerComboBox";
			this.RussiaPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.RussiaPlayerComboBox.TabIndex = 91;
			this.RussiaPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.RussiaPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// ItalyPlayerComboBox
			// 
			this.ItalyPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItalyPlayerComboBox.FormattingEnabled = true;
			this.ItalyPlayerComboBox.Location = new System.Drawing.Point(0, 129);
			this.ItalyPlayerComboBox.Name = "ItalyPlayerComboBox";
			this.ItalyPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.ItalyPlayerComboBox.TabIndex = 90;
			this.ItalyPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.ItalyPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// GermanyPlayerComboBox
			// 
			this.GermanyPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GermanyPlayerComboBox.FormattingEnabled = true;
			this.GermanyPlayerComboBox.Location = new System.Drawing.Point(0, 102);
			this.GermanyPlayerComboBox.Name = "GermanyPlayerComboBox";
			this.GermanyPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.GermanyPlayerComboBox.TabIndex = 89;
			this.GermanyPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.GermanyPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// FrancePlayerComboBox
			// 
			this.FrancePlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FrancePlayerComboBox.FormattingEnabled = true;
			this.FrancePlayerComboBox.Location = new System.Drawing.Point(0, 75);
			this.FrancePlayerComboBox.Name = "FrancePlayerComboBox";
			this.FrancePlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.FrancePlayerComboBox.TabIndex = 88;
			this.FrancePlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.FrancePlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// EnglandPlayerComboBox
			// 
			this.EnglandPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnglandPlayerComboBox.FormattingEnabled = true;
			this.EnglandPlayerComboBox.Location = new System.Drawing.Point(0, 48);
			this.EnglandPlayerComboBox.Name = "EnglandPlayerComboBox";
			this.EnglandPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.EnglandPlayerComboBox.TabIndex = 87;
			this.EnglandPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.EnglandPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			// 
			// AustriaPlayerComboBox
			// 
			this.AustriaPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AustriaPlayerComboBox.FormattingEnabled = true;
			this.AustriaPlayerComboBox.Location = new System.Drawing.Point(0, 21);
			this.AustriaPlayerComboBox.Name = "AustriaPlayerComboBox";
			this.AustriaPlayerComboBox.Size = new System.Drawing.Size(139, 21);
			this.AustriaPlayerComboBox.TabIndex = 86;
			this.AustriaPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerComboBox_SelectedIndexChanged);
			this.AustriaPlayerComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
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
			this.ScoresPanel.Location = new System.Drawing.Point(793, 85);
			this.ScoresPanel.Name = "ScoresPanel";
			this.ScoresPanel.Size = new System.Drawing.Size(50, 189);
			this.ScoresPanel.TabIndex = 109;
			// 
			// AustriaScoreLabel
			// 
			this.AustriaScoreLabel.Location = new System.Drawing.Point(3, 3);
			this.AustriaScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.AustriaScoreLabel.Name = "AustriaScoreLabel";
			this.AustriaScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.AustriaScoreLabel.TabIndex = 106;
			this.AustriaScoreLabel.Text = "0";
			this.AustriaScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// EnglandScoreLabel
			// 
			this.EnglandScoreLabel.Location = new System.Drawing.Point(3, 30);
			this.EnglandScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.EnglandScoreLabel.Name = "EnglandScoreLabel";
			this.EnglandScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.EnglandScoreLabel.TabIndex = 107;
			this.EnglandScoreLabel.Text = "0";
			this.EnglandScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FranceScoreLabel
			// 
			this.FranceScoreLabel.Location = new System.Drawing.Point(3, 57);
			this.FranceScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.FranceScoreLabel.Name = "FranceScoreLabel";
			this.FranceScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.FranceScoreLabel.TabIndex = 108;
			this.FranceScoreLabel.Text = "0";
			this.FranceScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// GermanyScoreLabel
			// 
			this.GermanyScoreLabel.Location = new System.Drawing.Point(3, 84);
			this.GermanyScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.GermanyScoreLabel.Name = "GermanyScoreLabel";
			this.GermanyScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.GermanyScoreLabel.TabIndex = 109;
			this.GermanyScoreLabel.Text = "0";
			this.GermanyScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ItalyScoreLabel
			// 
			this.ItalyScoreLabel.Location = new System.Drawing.Point(3, 111);
			this.ItalyScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.ItalyScoreLabel.Name = "ItalyScoreLabel";
			this.ItalyScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.ItalyScoreLabel.TabIndex = 110;
			this.ItalyScoreLabel.Text = "0";
			this.ItalyScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// RussiaScoreLabel
			// 
			this.RussiaScoreLabel.Location = new System.Drawing.Point(3, 138);
			this.RussiaScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.RussiaScoreLabel.Name = "RussiaScoreLabel";
			this.RussiaScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.RussiaScoreLabel.TabIndex = 111;
			this.RussiaScoreLabel.Text = "0";
			this.RussiaScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TurkeyScoreLabel
			// 
			this.TurkeyScoreLabel.Location = new System.Drawing.Point(3, 165);
			this.TurkeyScoreLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TurkeyScoreLabel.Name = "TurkeyScoreLabel";
			this.TurkeyScoreLabel.Size = new System.Drawing.Size(45, 15);
			this.TurkeyScoreLabel.TabIndex = 112;
			this.TurkeyScoreLabel.Text = "0";
			this.TurkeyScoreLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// GameControl
			// 
			this.GameControl.Location = new System.Drawing.Point(411, 36);
			this.GameControl.Margin = new System.Windows.Forms.Padding(1);
			this.GameControl.Name = "GameControl";
			this.GameControl.Size = new System.Drawing.Size(383, 235);
			this.GameControl.TabIndex = 1;
			// 
			// ToolTip
			// 
			this.ToolTip.IsBalloon = true;
			// 
			// GroupGamesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(857, 317);
			this.Controls.Add(this.ScoresPanel);
			this.Controls.Add(this.PlayersPanel);
			this.Controls.Add(this.GameInErrorButton);
			this.Controls.Add(this.TotalScoreLabel);
			this.Controls.Add(this.ScoreTotalBarLabel);
			this.Controls.Add(this.TotalScoreTextLabel);
			this.Controls.Add(this.ScoreColumnHeaderLabel);
			this.Controls.Add(this.DeleteGameButton);
			this.Controls.Add(this.GroupGamesDataGridView);
			this.Controls.Add(this.GameDateTimePicker);
			this.Controls.Add(this.GameDateLabel);
			this.Controls.Add(this.GameNameTextBox);
			this.Controls.Add(this.GameNameLabel);
			this.Controls.Add(this.NewGameButton);
			this.Controls.Add(this.GameStatusComboBox);
			this.Controls.Add(this.GameStatusLabel);
			this.Controls.Add(this.OrderByNamePanel);
			this.Controls.Add(this.GameControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupGamesForm";
			this.ShowIcon = false;
			this.Text = "Group Games";
			this.Load += new System.EventHandler(this.GroupGamesForm_Load);
			this.OrderByNamePanel.ResumeLayout(false);
			this.OrderByNamePanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.GroupGamesDataGridView)).EndInit();
			this.PlayersPanel.ResumeLayout(false);
			this.ScoresPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private GameControl GameControl;
		private Panel OrderByNamePanel;
		private RadioButton LastNameRegistrationTabRadioButton;
		private RadioButton FirstNameRegistrationTabRadioButton;
		private Label OrderByNameLabel;
		private ComboBox GameStatusComboBox;
		private Label GameStatusLabel;
		private Button NewGameButton;
		private Label GameNameLabel;
		private TextBox GameNameTextBox;
		private Label GameDateLabel;
		private DateTimePicker GameDateTimePicker;
		private DataGridView GroupGamesDataGridView;
		private Button DeleteGameButton;
		private Button GameInErrorButton;
		private Label TotalScoreLabel;
		private Label ScoreTotalBarLabel;
		private Label TotalScoreTextLabel;
		private Label ScoreColumnHeaderLabel;
		private Panel PlayersPanel;
		private Label PlayerColumnHeaderLabel;
		private ComboBox TurkeyPlayerComboBox;
		private ComboBox RussiaPlayerComboBox;
		private ComboBox ItalyPlayerComboBox;
		private ComboBox GermanyPlayerComboBox;
		private ComboBox FrancePlayerComboBox;
		private ComboBox EnglandPlayerComboBox;
		private ComboBox AustriaPlayerComboBox;
		private Panel ScoresPanel;
		private Label AustriaScoreLabel;
		private Label EnglandScoreLabel;
		private Label FranceScoreLabel;
		private Label GermanyScoreLabel;
		private Label ItalyScoreLabel;
		private Label RussiaScoreLabel;
		private Label TurkeyScoreLabel;
		private ToolTip ToolTip;
	}
}
