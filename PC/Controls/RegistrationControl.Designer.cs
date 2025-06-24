namespace PC.Controls
{
	partial class RegistrationControl
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
			StartFirstRoundButton = new Button();
			OrderByNamePanel = new Panel();
			LastNameRegistrationTabRadioButton = new RadioButton();
			FirstNameRegistrationTabRadioButton = new RadioButton();
			OrderByNameLabel = new Label();
			RegisteredPlayersLabel = new Label();
			UnregisteredPlayersLabel = new Label();
			RoundsRegisteredGroupBox = new GroupBox();
			Round9RegistrationCheckBox = new CheckBox();
			Round8RegistrationCheckBox = new CheckBox();
			Round7RegistrationCheckBox = new CheckBox();
			Round6RegistrationCheckBox = new CheckBox();
			Round5RegistrationCheckBox = new CheckBox();
			Round4RegistrationCheckBox = new CheckBox();
			Round3RegistrationCheckBox = new CheckBox();
			Round2RegistrationCheckBox = new CheckBox();
			Round1RegistrationCheckBox = new CheckBox();
			RegisteredDataGridView = new DataGridView();
			RegisterPlayerButton = new Button();
			UnregisteredListBox = new ListBox();
			NewPlayerButton = new Button();
			OrderByNamePanel.SuspendLayout();
			RoundsRegisteredGroupBox.SuspendLayout();
			((ISupportInitialize)RegisteredDataGridView).BeginInit();
			SuspendLayout();
			// 
			// StartFirstRoundButton
			// 
			StartFirstRoundButton.BackColor = Color.LightGreen;
			StartFirstRoundButton.Location = new Point(844, 2);
			StartFirstRoundButton.Margin = new Padding(4, 3, 4, 3);
			StartFirstRoundButton.Name = "StartFirstRoundButton";
			StartFirstRoundButton.Size = new Size(71, 60);
			StartFirstRoundButton.TabIndex = 67;
			StartFirstRoundButton.Text = "  Start   ▶\r\n  First   ▶\r\nRound ▶";
			StartFirstRoundButton.TextAlign = ContentAlignment.MiddleRight;
			StartFirstRoundButton.UseVisualStyleBackColor = false;
			StartFirstRoundButton.Click += StartFirstRoundButton_Click;
			// 
			// OrderByNamePanel
			// 
			OrderByNamePanel.Controls.Add(LastNameRegistrationTabRadioButton);
			OrderByNamePanel.Controls.Add(FirstNameRegistrationTabRadioButton);
			OrderByNamePanel.Controls.Add(OrderByNameLabel);
			OrderByNamePanel.Location = new Point(226, 175);
			OrderByNamePanel.Margin = new Padding(4, 3, 4, 3);
			OrderByNamePanel.Name = "OrderByNamePanel";
			OrderByNamePanel.Size = new Size(284, 35);
			OrderByNamePanel.TabIndex = 66;
			// 
			// LastNameRegistrationTabRadioButton
			// 
			LastNameRegistrationTabRadioButton.AutoSize = true;
			LastNameRegistrationTabRadioButton.Location = new Point(194, 7);
			LastNameRegistrationTabRadioButton.Margin = new Padding(4, 3, 4, 3);
			LastNameRegistrationTabRadioButton.Name = "LastNameRegistrationTabRadioButton";
			LastNameRegistrationTabRadioButton.Size = new Size(81, 19);
			LastNameRegistrationTabRadioButton.TabIndex = 17;
			LastNameRegistrationTabRadioButton.TabStop = true;
			LastNameRegistrationTabRadioButton.Text = "Last Name";
			LastNameRegistrationTabRadioButton.UseVisualStyleBackColor = true;
			LastNameRegistrationTabRadioButton.CheckedChanged += NameRegistrationTabRadioButton_CheckedChanged;
			// 
			// FirstNameRegistrationTabRadioButton
			// 
			FirstNameRegistrationTabRadioButton.AutoSize = true;
			FirstNameRegistrationTabRadioButton.Location = new Point(108, 7);
			FirstNameRegistrationTabRadioButton.Margin = new Padding(4, 3, 4, 3);
			FirstNameRegistrationTabRadioButton.Name = "FirstNameRegistrationTabRadioButton";
			FirstNameRegistrationTabRadioButton.Size = new Size(82, 19);
			FirstNameRegistrationTabRadioButton.TabIndex = 16;
			FirstNameRegistrationTabRadioButton.TabStop = true;
			FirstNameRegistrationTabRadioButton.Text = "First Name";
			FirstNameRegistrationTabRadioButton.UseVisualStyleBackColor = true;
			FirstNameRegistrationTabRadioButton.CheckedChanged += NameRegistrationTabRadioButton_CheckedChanged;
			// 
			// OrderByNameLabel
			// 
			OrderByNameLabel.AutoSize = true;
			OrderByNameLabel.Location = new Point(1, 8);
			OrderByNameLabel.Margin = new Padding(4, 0, 4, 0);
			OrderByNameLabel.Name = "OrderByNameLabel";
			OrderByNameLabel.Size = new Size(96, 15);
			OrderByNameLabel.TabIndex = 15;
			OrderByNameLabel.Text = "Order Players By:";
			// 
			// RegisteredPlayersLabel
			// 
			RegisteredPlayersLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			RegisteredPlayersLabel.Location = new Point(517, 7);
			RegisteredPlayersLabel.Margin = new Padding(4, 0, 4, 0);
			RegisteredPlayersLabel.Name = "RegisteredPlayersLabel";
			RegisteredPlayersLabel.Size = new Size(318, 21);
			RegisteredPlayersLabel.TabIndex = 65;
			RegisteredPlayersLabel.Text = "Registered Players";
			RegisteredPlayersLabel.TextAlign = ContentAlignment.TopCenter;
			// 
			// UnregisteredPlayersLabel
			// 
			UnregisteredPlayersLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			UnregisteredPlayersLabel.Location = new Point(7, 7);
			UnregisteredPlayersLabel.Margin = new Padding(4, 0, 4, 0);
			UnregisteredPlayersLabel.Name = "UnregisteredPlayersLabel";
			UnregisteredPlayersLabel.Size = new Size(209, 21);
			UnregisteredPlayersLabel.TabIndex = 64;
			UnregisteredPlayersLabel.Text = "Unregistered Players";
			UnregisteredPlayersLabel.TextAlign = ContentAlignment.TopCenter;
			// 
			// RoundsRegisteredGroupBox
			// 
			RoundsRegisteredGroupBox.Controls.Add(Round9RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round8RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round7RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round6RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round5RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round4RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round3RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round2RegistrationCheckBox);
			RoundsRegisteredGroupBox.Controls.Add(Round1RegistrationCheckBox);
			RoundsRegisteredGroupBox.Location = new Point(226, 316);
			RoundsRegisteredGroupBox.Margin = new Padding(4, 3, 4, 3);
			RoundsRegisteredGroupBox.Name = "RoundsRegisteredGroupBox";
			RoundsRegisteredGroupBox.Padding = new Padding(4, 3, 4, 3);
			RoundsRegisteredGroupBox.Size = new Size(284, 117);
			RoundsRegisteredGroupBox.TabIndex = 63;
			RoundsRegisteredGroupBox.TabStop = false;
			RoundsRegisteredGroupBox.Text = "Player Name";
			// 
			// Round9RegistrationCheckBox
			// 
			Round9RegistrationCheckBox.AutoSize = true;
			Round9RegistrationCheckBox.Location = new Point(194, 82);
			Round9RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round9RegistrationCheckBox.Name = "Round9RegistrationCheckBox";
			Round9RegistrationCheckBox.Size = new Size(70, 19);
			Round9RegistrationCheckBox.TabIndex = 8;
			Round9RegistrationCheckBox.Text = "Round 9";
			Round9RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round9RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round8RegistrationCheckBox
			// 
			Round8RegistrationCheckBox.AutoSize = true;
			Round8RegistrationCheckBox.Location = new Point(107, 82);
			Round8RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round8RegistrationCheckBox.Name = "Round8RegistrationCheckBox";
			Round8RegistrationCheckBox.Size = new Size(70, 19);
			Round8RegistrationCheckBox.TabIndex = 7;
			Round8RegistrationCheckBox.Text = "Round 8";
			Round8RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round8RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round7RegistrationCheckBox
			// 
			Round7RegistrationCheckBox.AutoSize = true;
			Round7RegistrationCheckBox.Location = new Point(21, 82);
			Round7RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round7RegistrationCheckBox.Name = "Round7RegistrationCheckBox";
			Round7RegistrationCheckBox.Size = new Size(70, 19);
			Round7RegistrationCheckBox.TabIndex = 6;
			Round7RegistrationCheckBox.Text = "Round 7";
			Round7RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round7RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round6RegistrationCheckBox
			// 
			Round6RegistrationCheckBox.AutoSize = true;
			Round6RegistrationCheckBox.Location = new Point(194, 54);
			Round6RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round6RegistrationCheckBox.Name = "Round6RegistrationCheckBox";
			Round6RegistrationCheckBox.Size = new Size(70, 19);
			Round6RegistrationCheckBox.TabIndex = 5;
			Round6RegistrationCheckBox.Text = "Round 6";
			Round6RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round6RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round5RegistrationCheckBox
			// 
			Round5RegistrationCheckBox.AutoSize = true;
			Round5RegistrationCheckBox.Location = new Point(107, 54);
			Round5RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round5RegistrationCheckBox.Name = "Round5RegistrationCheckBox";
			Round5RegistrationCheckBox.Size = new Size(70, 19);
			Round5RegistrationCheckBox.TabIndex = 4;
			Round5RegistrationCheckBox.Text = "Round 5";
			Round5RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round5RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round4RegistrationCheckBox
			// 
			Round4RegistrationCheckBox.AutoSize = true;
			Round4RegistrationCheckBox.Location = new Point(21, 54);
			Round4RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round4RegistrationCheckBox.Name = "Round4RegistrationCheckBox";
			Round4RegistrationCheckBox.Size = new Size(70, 19);
			Round4RegistrationCheckBox.TabIndex = 3;
			Round4RegistrationCheckBox.Text = "Round 4";
			Round4RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round4RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round3RegistrationCheckBox
			// 
			Round3RegistrationCheckBox.AutoSize = true;
			Round3RegistrationCheckBox.Location = new Point(194, 27);
			Round3RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round3RegistrationCheckBox.Name = "Round3RegistrationCheckBox";
			Round3RegistrationCheckBox.Size = new Size(70, 19);
			Round3RegistrationCheckBox.TabIndex = 2;
			Round3RegistrationCheckBox.Text = "Round 3";
			Round3RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round3RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round2RegistrationCheckBox
			// 
			Round2RegistrationCheckBox.AutoSize = true;
			Round2RegistrationCheckBox.Location = new Point(107, 27);
			Round2RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round2RegistrationCheckBox.Name = "Round2RegistrationCheckBox";
			Round2RegistrationCheckBox.Size = new Size(70, 19);
			Round2RegistrationCheckBox.TabIndex = 1;
			Round2RegistrationCheckBox.Text = "Round 2";
			Round2RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round2RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// Round1RegistrationCheckBox
			// 
			Round1RegistrationCheckBox.AutoSize = true;
			Round1RegistrationCheckBox.Location = new Point(21, 27);
			Round1RegistrationCheckBox.Margin = new Padding(4, 3, 4, 3);
			Round1RegistrationCheckBox.Name = "Round1RegistrationCheckBox";
			Round1RegistrationCheckBox.Size = new Size(70, 19);
			Round1RegistrationCheckBox.TabIndex = 0;
			Round1RegistrationCheckBox.Text = "Round 1";
			Round1RegistrationCheckBox.UseVisualStyleBackColor = true;
			Round1RegistrationCheckBox.CheckedChanged += RoundRegistrationCheckBox_CheckedChanged;
			// 
			// RegisteredDataGridView
			// 
			RegisteredDataGridView.AllowUserToAddRows = false;
			RegisteredDataGridView.AllowUserToDeleteRows = false;
			RegisteredDataGridView.AllowUserToResizeColumns = false;
			RegisteredDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = SystemColors.ControlLight;
			RegisteredDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			RegisteredDataGridView.BackgroundColor = SystemColors.Window;
			RegisteredDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.BackColor = SystemColors.Control;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
			RegisteredDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			RegisteredDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			RegisteredDataGridView.ColumnHeadersVisible = false;
			RegisteredDataGridView.Location = new Point(517, 31);
			RegisteredDataGridView.Margin = new Padding(4, 3, 4, 3);
			RegisteredDataGridView.MultiSelect = false;
			RegisteredDataGridView.Name = "RegisteredDataGridView";
			RegisteredDataGridView.ReadOnly = true;
			RegisteredDataGridView.RowHeadersVisible = false;
			RegisteredDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			RegisteredDataGridView.RowTemplate.Height = 16;
			RegisteredDataGridView.RowTemplate.Resizable = DataGridViewTriState.False;
			RegisteredDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			RegisteredDataGridView.ShowCellToolTips = false;
			RegisteredDataGridView.Size = new Size(318, 572);
			RegisteredDataGridView.TabIndex = 62;
			RegisteredDataGridView.CellContentDoubleClick += RegisterPlayerButton_Click;
			RegisteredDataGridView.DataBindingComplete += RegisteredDataGridView_DataBindingComplete;
			RegisteredDataGridView.SelectionChanged += RegisteredDataGridView_SelectionChanged;
			// 
			// RegisterPlayerButton
			// 
			RegisterPlayerButton.Location = new Point(226, 265);
			RegisterPlayerButton.Margin = new Padding(4, 3, 4, 3);
			RegisterPlayerButton.Name = "RegisterPlayerButton";
			RegisterPlayerButton.Size = new Size(284, 27);
			RegisterPlayerButton.TabIndex = 61;
			RegisterPlayerButton.Text = "Register This Player  ───────▶";
			RegisterPlayerButton.UseVisualStyleBackColor = true;
			RegisterPlayerButton.Click += RegisterPlayerButton_Click;
			// 
			// UnregisteredListBox
			// 
			UnregisteredListBox.FormattingEnabled = true;
			UnregisteredListBox.Location = new Point(12, 31);
			UnregisteredListBox.Margin = new Padding(4, 3, 4, 3);
			UnregisteredListBox.Name = "UnregisteredListBox";
			UnregisteredListBox.Size = new Size(208, 574);
			UnregisteredListBox.TabIndex = 60;
			UnregisteredListBox.SelectedIndexChanged += UnregisteredListBox_SelectedIndexChanged;
			UnregisteredListBox.MouseDoubleClick += RegisterPlayerButton_Click;
			// 
			// NewPlayerButton
			// 
			NewPlayerButton.Location = new Point(226, 55);
			NewPlayerButton.Margin = new Padding(4, 3, 4, 3);
			NewPlayerButton.Name = "NewPlayerButton";
			NewPlayerButton.Size = new Size(284, 27);
			NewPlayerButton.TabIndex = 68;
			NewPlayerButton.Text = "Add and Register a New Player";
			NewPlayerButton.UseVisualStyleBackColor = true;
			NewPlayerButton.Click += NewPlayerButton_Click;
			// 
			// RegistrationControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(NewPlayerButton);
			Controls.Add(StartFirstRoundButton);
			Controls.Add(OrderByNamePanel);
			Controls.Add(RegisteredPlayersLabel);
			Controls.Add(UnregisteredPlayersLabel);
			Controls.Add(RoundsRegisteredGroupBox);
			Controls.Add(RegisteredDataGridView);
			Controls.Add(RegisterPlayerButton);
			Controls.Add(UnregisteredListBox);
			Margin = new Padding(1);
			Name = "RegistrationControl";
			Size = new Size(924, 615);
			OrderByNamePanel.ResumeLayout(false);
			OrderByNamePanel.PerformLayout();
			RoundsRegisteredGroupBox.ResumeLayout(false);
			RoundsRegisteredGroupBox.PerformLayout();
			((ISupportInitialize)RegisteredDataGridView).EndInit();
			ResumeLayout(false);

		}

		#endregion

		private Button StartFirstRoundButton;
		private Panel OrderByNamePanel;
		private RadioButton LastNameRegistrationTabRadioButton;
		private RadioButton FirstNameRegistrationTabRadioButton;
		private Label OrderByNameLabel;
		private Label RegisteredPlayersLabel;
		private Label UnregisteredPlayersLabel;
		private GroupBox RoundsRegisteredGroupBox;
		private CheckBox Round9RegistrationCheckBox;
		private CheckBox Round8RegistrationCheckBox;
		private CheckBox Round7RegistrationCheckBox;
		private CheckBox Round6RegistrationCheckBox;
		private CheckBox Round5RegistrationCheckBox;
		private CheckBox Round4RegistrationCheckBox;
		private CheckBox Round3RegistrationCheckBox;
		private CheckBox Round2RegistrationCheckBox;
		private CheckBox Round1RegistrationCheckBox;
		private DataGridView RegisteredDataGridView;
		private Button RegisterPlayerButton;
		private ListBox UnregisteredListBox;
		private Button NewPlayerButton;
	}
}
