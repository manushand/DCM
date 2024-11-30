namespace DCM.UI.Controls
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.StartFirstRoundButton = new System.Windows.Forms.Button();
			this.OrderByNamePanel = new System.Windows.Forms.Panel();
			this.LastNameRegistrationTabRadioButton = new System.Windows.Forms.RadioButton();
			this.FirstNameRegistrationTabRadioButton = new System.Windows.Forms.RadioButton();
			this.OrderByNameLabel = new System.Windows.Forms.Label();
			this.RegisteredPlayersLabel = new System.Windows.Forms.Label();
			this.UnregisteredPlayersLabel = new System.Windows.Forms.Label();
			this.RoundsRegisteredGroupBox = new System.Windows.Forms.GroupBox();
			this.Round9RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round8RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round7RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round6RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round5RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round4RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round3RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round2RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.Round1RegistrationCheckBox = new System.Windows.Forms.CheckBox();
			this.RegisteredDataGridView = new System.Windows.Forms.DataGridView();
			this.RegisterPlayerButton = new System.Windows.Forms.Button();
			this.UnregisteredListBox = new System.Windows.Forms.ListBox();
			this.NewPlayerButton = new System.Windows.Forms.Button();
			this.OrderByNamePanel.SuspendLayout();
			this.RoundsRegisteredGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RegisteredDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// StartFirstRoundButton
			// 
			this.StartFirstRoundButton.BackColor = System.Drawing.Color.LightGreen;
			this.StartFirstRoundButton.Location = new System.Drawing.Point(723, 2);
			this.StartFirstRoundButton.Name = "StartFirstRoundButton";
			this.StartFirstRoundButton.Size = new System.Drawing.Size(61, 52);
			this.StartFirstRoundButton.TabIndex = 67;
			this.StartFirstRoundButton.Text = "  Start   ▶\r\n  First   ▶\r\nRound ▶";
			this.StartFirstRoundButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.StartFirstRoundButton.UseVisualStyleBackColor = false;
			this.StartFirstRoundButton.Click += new System.EventHandler(this.StartFirstRoundButton_Click);
			// 
			// OrderByNamePanel
			// 
			this.OrderByNamePanel.Controls.Add(this.LastNameRegistrationTabRadioButton);
			this.OrderByNamePanel.Controls.Add(this.FirstNameRegistrationTabRadioButton);
			this.OrderByNamePanel.Controls.Add(this.OrderByNameLabel);
			this.OrderByNamePanel.Location = new System.Drawing.Point(194, 152);
			this.OrderByNamePanel.Name = "OrderByNamePanel";
			this.OrderByNamePanel.Size = new System.Drawing.Size(243, 30);
			this.OrderByNamePanel.TabIndex = 66;
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
			// RegisteredPlayersLabel
			// 
			this.RegisteredPlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RegisteredPlayersLabel.Location = new System.Drawing.Point(443, 6);
			this.RegisteredPlayersLabel.Name = "RegisteredPlayersLabel";
			this.RegisteredPlayersLabel.Size = new System.Drawing.Size(273, 18);
			this.RegisteredPlayersLabel.TabIndex = 65;
			this.RegisteredPlayersLabel.Text = "Registered Players";
			this.RegisteredPlayersLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// UnregisteredPlayersLabel
			// 
			this.UnregisteredPlayersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UnregisteredPlayersLabel.Location = new System.Drawing.Point(6, 6);
			this.UnregisteredPlayersLabel.Name = "UnregisteredPlayersLabel";
			this.UnregisteredPlayersLabel.Size = new System.Drawing.Size(179, 18);
			this.UnregisteredPlayersLabel.TabIndex = 64;
			this.UnregisteredPlayersLabel.Text = "Unregistered Players";
			this.UnregisteredPlayersLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// RoundsRegisteredGroupBox
			// 
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round9RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round8RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round7RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round6RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round5RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round4RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round3RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round2RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Controls.Add(this.Round1RegistrationCheckBox);
			this.RoundsRegisteredGroupBox.Location = new System.Drawing.Point(194, 274);
			this.RoundsRegisteredGroupBox.Name = "RoundsRegisteredGroupBox";
			this.RoundsRegisteredGroupBox.Size = new System.Drawing.Size(243, 101);
			this.RoundsRegisteredGroupBox.TabIndex = 63;
			this.RoundsRegisteredGroupBox.TabStop = false;
			this.RoundsRegisteredGroupBox.Text = "Player Name";
			// 
			// Round9RegistrationCheckBox
			// 
			this.Round9RegistrationCheckBox.AutoSize = true;
			this.Round9RegistrationCheckBox.Location = new System.Drawing.Point(166, 71);
			this.Round9RegistrationCheckBox.Name = "Round9RegistrationCheckBox";
			this.Round9RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round9RegistrationCheckBox.TabIndex = 8;
			this.Round9RegistrationCheckBox.Text = "Round 9";
			this.Round9RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round9RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round8RegistrationCheckBox
			// 
			this.Round8RegistrationCheckBox.AutoSize = true;
			this.Round8RegistrationCheckBox.Location = new System.Drawing.Point(92, 71);
			this.Round8RegistrationCheckBox.Name = "Round8RegistrationCheckBox";
			this.Round8RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round8RegistrationCheckBox.TabIndex = 7;
			this.Round8RegistrationCheckBox.Text = "Round 8";
			this.Round8RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round8RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round7RegistrationCheckBox
			// 
			this.Round7RegistrationCheckBox.AutoSize = true;
			this.Round7RegistrationCheckBox.Location = new System.Drawing.Point(18, 71);
			this.Round7RegistrationCheckBox.Name = "Round7RegistrationCheckBox";
			this.Round7RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round7RegistrationCheckBox.TabIndex = 6;
			this.Round7RegistrationCheckBox.Text = "Round 7";
			this.Round7RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round7RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round6RegistrationCheckBox
			// 
			this.Round6RegistrationCheckBox.AutoSize = true;
			this.Round6RegistrationCheckBox.Location = new System.Drawing.Point(166, 47);
			this.Round6RegistrationCheckBox.Name = "Round6RegistrationCheckBox";
			this.Round6RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round6RegistrationCheckBox.TabIndex = 5;
			this.Round6RegistrationCheckBox.Text = "Round 6";
			this.Round6RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round6RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round5RegistrationCheckBox
			// 
			this.Round5RegistrationCheckBox.AutoSize = true;
			this.Round5RegistrationCheckBox.Location = new System.Drawing.Point(92, 47);
			this.Round5RegistrationCheckBox.Name = "Round5RegistrationCheckBox";
			this.Round5RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round5RegistrationCheckBox.TabIndex = 4;
			this.Round5RegistrationCheckBox.Text = "Round 5";
			this.Round5RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round5RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round4RegistrationCheckBox
			// 
			this.Round4RegistrationCheckBox.AutoSize = true;
			this.Round4RegistrationCheckBox.Location = new System.Drawing.Point(18, 47);
			this.Round4RegistrationCheckBox.Name = "Round4RegistrationCheckBox";
			this.Round4RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round4RegistrationCheckBox.TabIndex = 3;
			this.Round4RegistrationCheckBox.Text = "Round 4";
			this.Round4RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round4RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round3RegistrationCheckBox
			// 
			this.Round3RegistrationCheckBox.AutoSize = true;
			this.Round3RegistrationCheckBox.Location = new System.Drawing.Point(166, 23);
			this.Round3RegistrationCheckBox.Name = "Round3RegistrationCheckBox";
			this.Round3RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round3RegistrationCheckBox.TabIndex = 2;
			this.Round3RegistrationCheckBox.Text = "Round 3";
			this.Round3RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round3RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round2RegistrationCheckBox
			// 
			this.Round2RegistrationCheckBox.AutoSize = true;
			this.Round2RegistrationCheckBox.Location = new System.Drawing.Point(92, 23);
			this.Round2RegistrationCheckBox.Name = "Round2RegistrationCheckBox";
			this.Round2RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round2RegistrationCheckBox.TabIndex = 1;
			this.Round2RegistrationCheckBox.Text = "Round 2";
			this.Round2RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round2RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// Round1RegistrationCheckBox
			// 
			this.Round1RegistrationCheckBox.AutoSize = true;
			this.Round1RegistrationCheckBox.Location = new System.Drawing.Point(18, 23);
			this.Round1RegistrationCheckBox.Name = "Round1RegistrationCheckBox";
			this.Round1RegistrationCheckBox.Size = new System.Drawing.Size(67, 17);
			this.Round1RegistrationCheckBox.TabIndex = 0;
			this.Round1RegistrationCheckBox.Text = "Round 1";
			this.Round1RegistrationCheckBox.UseVisualStyleBackColor = true;
			this.Round1RegistrationCheckBox.CheckedChanged += new System.EventHandler(this.RoundRegistrationCheckBox_CheckedChanged);
			// 
			// RegisteredDataGridView
			// 
			this.RegisteredDataGridView.AllowUserToAddRows = false;
			this.RegisteredDataGridView.AllowUserToDeleteRows = false;
			this.RegisteredDataGridView.AllowUserToResizeColumns = false;
			this.RegisteredDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.RegisteredDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.RegisteredDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.RegisteredDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.RegisteredDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.RegisteredDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RegisteredDataGridView.ColumnHeadersVisible = false;
			this.RegisteredDataGridView.Location = new System.Drawing.Point(443, 27);
			this.RegisteredDataGridView.MultiSelect = false;
			this.RegisteredDataGridView.Name = "RegisteredDataGridView";
			this.RegisteredDataGridView.ReadOnly = true;
			this.RegisteredDataGridView.RowHeadersVisible = false;
			this.RegisteredDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.RegisteredDataGridView.RowTemplate.Height = 16;
			this.RegisteredDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.RegisteredDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RegisteredDataGridView.ShowCellToolTips = false;
			this.RegisteredDataGridView.Size = new System.Drawing.Size(273, 496);
			this.RegisteredDataGridView.TabIndex = 62;
			this.RegisteredDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RegisterPlayerButton_Click);
			this.RegisteredDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.RegisteredDataGridView_DataBindingComplete);
			this.RegisteredDataGridView.SelectionChanged += new System.EventHandler(this.RegisteredDataGridView_SelectionChanged);
			// 
			// RegisterPlayerButton
			// 
			this.RegisterPlayerButton.Location = new System.Drawing.Point(194, 230);
			this.RegisterPlayerButton.Name = "RegisterPlayerButton";
			this.RegisterPlayerButton.Size = new System.Drawing.Size(243, 23);
			this.RegisterPlayerButton.TabIndex = 61;
			this.RegisterPlayerButton.Text = "Register This Player  ───────▶";
			this.RegisterPlayerButton.UseVisualStyleBackColor = true;
			this.RegisterPlayerButton.Click += new System.EventHandler(this.RegisterPlayerButton_Click);
			// 
			// UnregisteredListBox
			// 
			this.UnregisteredListBox.FormattingEnabled = true;
			this.UnregisteredListBox.Location = new System.Drawing.Point(10, 27);
			this.UnregisteredListBox.Name = "UnregisteredListBox";
			this.UnregisteredListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.UnregisteredListBox.Size = new System.Drawing.Size(179, 498);
			this.UnregisteredListBox.TabIndex = 60;
			this.UnregisteredListBox.SelectedIndexChanged += new System.EventHandler(this.UnregisteredListBox_SelectedIndexChanged);
			this.UnregisteredListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RegisterPlayerButton_Click);
			// 
			// NewPlayerButton
			// 
			this.NewPlayerButton.Location = new System.Drawing.Point(194, 48);
			this.NewPlayerButton.Name = "AddNewPlayerButton";
			this.NewPlayerButton.Size = new System.Drawing.Size(243, 23);
			this.NewPlayerButton.TabIndex = 68;
			this.NewPlayerButton.Text = "Add and Register a New Player";
			this.NewPlayerButton.UseVisualStyleBackColor = true;
			this.NewPlayerButton.Click += new System.EventHandler(this.NewPlayerButton_Click);
			// 
			// RegistrationControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.NewPlayerButton);
			this.Controls.Add(this.StartFirstRoundButton);
			this.Controls.Add(this.OrderByNamePanel);
			this.Controls.Add(this.RegisteredPlayersLabel);
			this.Controls.Add(this.UnregisteredPlayersLabel);
			this.Controls.Add(this.RoundsRegisteredGroupBox);
			this.Controls.Add(this.RegisteredDataGridView);
			this.Controls.Add(this.RegisterPlayerButton);
			this.Controls.Add(this.UnregisteredListBox);
			this.Margin = new System.Windows.Forms.Padding(1);
			this.Name = "RegistrationControl";
			this.Size = new System.Drawing.Size(792, 533);
			this.OrderByNamePanel.ResumeLayout(false);
			this.OrderByNamePanel.PerformLayout();
			this.RoundsRegisteredGroupBox.ResumeLayout(false);
			this.RoundsRegisteredGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RegisteredDataGridView)).EndInit();
			this.ResumeLayout(false);

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
