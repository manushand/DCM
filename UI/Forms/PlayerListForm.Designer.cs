namespace DCM.UI.Forms
{
	internal sealed partial class PlayerListForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private global::System.ComponentModel.IContainer components = null;

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
			this.PlayerListBox = new System.Windows.Forms.ListBox();
			this.FirstNameLabel = new System.Windows.Forms.Label();
			this.LastNameLabel = new System.Windows.Forms.Label();
			this.EmailLabel = new System.Windows.Forms.Label();
			this.FirstNameTextBox = new System.Windows.Forms.TextBox();
			this.LastNameTextBox = new System.Windows.Forms.TextBox();
			this.EmailAddressTextBox = new System.Windows.Forms.TextBox();
			this.AddPlayerButton = new System.Windows.Forms.Button();
			this.NewPlayerPanel = new System.Windows.Forms.Panel();
			this.EditButton = new System.Windows.Forms.Button();
			this.ConflictsButton = new System.Windows.Forms.Button();
			this.GroupsButton = new System.Windows.Forms.Button();
			this.RemoveButton = new System.Windows.Forms.Button();
			this.OrderPlayersByLabel = new System.Windows.Forms.Label();
			this.OrderPlayersByPanel = new System.Windows.Forms.Panel();
			this.LastNameRadioButton = new System.Windows.Forms.RadioButton();
			this.FirstNameRadioButton = new System.Windows.Forms.RadioButton();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.NewPlayerPanel.SuspendLayout();
			this.OrderPlayersByPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// PlayerListBox
			// 
			this.PlayerListBox.ColumnWidth = 132;
			this.PlayerListBox.FormattingEnabled = true;
			this.PlayerListBox.Location = new System.Drawing.Point(13, 39);
			this.PlayerListBox.MultiColumn = true;
			this.PlayerListBox.Name = "PlayerListBox";
			this.PlayerListBox.Size = new System.Drawing.Size(264, 407);
			this.PlayerListBox.TabIndex = 0;
			this.PlayerListBox.SelectedIndexChanged += new System.EventHandler(this.PlayerListBox_SelectedIndexChanged);
			this.PlayerListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.EditButton_Click);
			// 
			// FirstNameLabel
			// 
			this.FirstNameLabel.AutoSize = true;
			this.FirstNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FirstNameLabel.ForeColor = System.Drawing.Color.Blue;
			this.FirstNameLabel.Location = new System.Drawing.Point(4, 16);
			this.FirstNameLabel.Name = "FirstNameLabel";
			this.FirstNameLabel.Size = new System.Drawing.Size(60, 13);
			this.FirstNameLabel.TabIndex = 1;
			this.FirstNameLabel.Text = "First Name:";
			// 
			// LastNameLabel
			// 
			this.LastNameLabel.AutoSize = true;
			this.LastNameLabel.Location = new System.Drawing.Point(4, 42);
			this.LastNameLabel.Name = "LastNameLabel";
			this.LastNameLabel.Size = new System.Drawing.Size(61, 13);
			this.LastNameLabel.TabIndex = 2;
			this.LastNameLabel.Text = "Last Name:";
			// 
			// EmailLabel
			// 
			this.EmailLabel.AutoSize = true;
			this.EmailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EmailLabel.ForeColor = System.Drawing.Color.Blue;
			this.EmailLabel.Location = new System.Drawing.Point(4, 69);
			this.EmailLabel.Name = "EmailLabel";
			this.EmailLabel.Size = new System.Drawing.Size(46, 13);
			this.EmailLabel.TabIndex = 3;
			this.EmailLabel.Text = "Email(s):";
			// 
			// FirstNameTextBox
			// 
			this.FirstNameTextBox.Location = new System.Drawing.Point(70, 13);
			this.FirstNameTextBox.Name = "FirstNameTextBox";
			this.FirstNameTextBox.Size = new System.Drawing.Size(185, 20);
			this.FirstNameTextBox.TabIndex = 4;
			this.FirstNameTextBox.Enter += new System.EventHandler(this.NewPlayerTextBoxes_GotFocus);
			this.FirstNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FirstNameTextBox_KeyUp);
			// 
			// LastNameTextBox
			// 
			this.LastNameTextBox.Location = new System.Drawing.Point(70, 39);
			this.LastNameTextBox.Name = "LastNameTextBox";
			this.LastNameTextBox.Size = new System.Drawing.Size(185, 20);
			this.LastNameTextBox.TabIndex = 5;
			this.LastNameTextBox.Enter += new System.EventHandler(this.NewPlayerTextBoxes_GotFocus);
			// 
			// EmailAddressTextBox
			// 
			this.EmailAddressTextBox.Location = new System.Drawing.Point(70, 66);
			this.EmailAddressTextBox.Name = "EmailAddressTextBox";
			this.EmailAddressTextBox.Size = new System.Drawing.Size(185, 20);
			this.EmailAddressTextBox.TabIndex = 6;
			this.EmailAddressTextBox.Enter += new System.EventHandler(this.NewPlayerTextBoxes_GotFocus);
			// 
			// AddPlayerButton
			// 
			this.AddPlayerButton.Location = new System.Drawing.Point(70, 92);
			this.AddPlayerButton.Name = "AddPlayerButton";
			this.AddPlayerButton.Size = new System.Drawing.Size(127, 30);
			this.AddPlayerButton.TabIndex = 7;
			this.AddPlayerButton.Text = "Add New Player";
			this.AddPlayerButton.UseVisualStyleBackColor = true;
			this.AddPlayerButton.Click += new System.EventHandler(this.AddPlayerButton_Click);
			// 
			// NewPlayerPanel
			// 
			this.NewPlayerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.NewPlayerPanel.Controls.Add(this.LastNameTextBox);
			this.NewPlayerPanel.Controls.Add(this.AddPlayerButton);
			this.NewPlayerPanel.Controls.Add(this.FirstNameLabel);
			this.NewPlayerPanel.Controls.Add(this.EmailAddressTextBox);
			this.NewPlayerPanel.Controls.Add(this.LastNameLabel);
			this.NewPlayerPanel.Controls.Add(this.EmailLabel);
			this.NewPlayerPanel.Controls.Add(this.FirstNameTextBox);
			this.NewPlayerPanel.Location = new System.Drawing.Point(13, 482);
			this.NewPlayerPanel.Name = "NewPlayerPanel";
			this.NewPlayerPanel.Size = new System.Drawing.Size(264, 132);
			this.NewPlayerPanel.TabIndex = 8;
			// 
			// EditButton
			// 
			this.EditButton.Location = new System.Drawing.Point(13, 453);
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(66, 23);
			this.EditButton.TabIndex = 9;
			this.EditButton.Text = "Edit";
			this.EditButton.UseVisualStyleBackColor = true;
			this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
			// 
			// ConflictsButton
			// 
			this.ConflictsButton.Location = new System.Drawing.Point(145, 453);
			this.ConflictsButton.Name = "ConflictsButton";
			this.ConflictsButton.Size = new System.Drawing.Size(66, 23);
			this.ConflictsButton.TabIndex = 10;
			this.ConflictsButton.Text = "Conflicts";
			this.ConflictsButton.UseVisualStyleBackColor = true;
			this.ConflictsButton.Click += new System.EventHandler(this.ConflictsButton_Click);
			// 
			// GroupsButton
			// 
			this.GroupsButton.Location = new System.Drawing.Point(79, 453);
			this.GroupsButton.Name = "GroupsButton";
			this.GroupsButton.Size = new System.Drawing.Size(66, 23);
			this.GroupsButton.TabIndex = 11;
			this.GroupsButton.Text = "Groups";
			this.GroupsButton.UseVisualStyleBackColor = true;
			this.GroupsButton.Click += new System.EventHandler(this.GroupsButton_Click);
			// 
			// RemoveButton
			// 
			this.RemoveButton.Location = new System.Drawing.Point(211, 453);
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(66, 23);
			this.RemoveButton.TabIndex = 12;
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.UseVisualStyleBackColor = true;
			this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
			// 
			// OrderPlayersByLabel
			// 
			this.OrderPlayersByLabel.AutoSize = true;
			this.OrderPlayersByLabel.Location = new System.Drawing.Point(-1, 8);
			this.OrderPlayersByLabel.Name = "OrderPlayersByLabel";
			this.OrderPlayersByLabel.Size = new System.Drawing.Size(88, 13);
			this.OrderPlayersByLabel.TabIndex = 15;
			this.OrderPlayersByLabel.Text = "Order Players By:";
			// 
			// OrderPlayersByPanel
			// 
			this.OrderPlayersByPanel.Controls.Add(this.LastNameRadioButton);
			this.OrderPlayersByPanel.Controls.Add(this.FirstNameRadioButton);
			this.OrderPlayersByPanel.Controls.Add(this.OrderPlayersByLabel);
			this.OrderPlayersByPanel.Location = new System.Drawing.Point(13, 3);
			this.OrderPlayersByPanel.Name = "OrderPlayersByPanel";
			this.OrderPlayersByPanel.Size = new System.Drawing.Size(264, 30);
			this.OrderPlayersByPanel.TabIndex = 16;
			// 
			// LastNameRadioButton
			// 
			this.LastNameRadioButton.AutoSize = true;
			this.LastNameRadioButton.Location = new System.Drawing.Point(191, 6);
			this.LastNameRadioButton.Name = "LastNameRadioButton";
			this.LastNameRadioButton.Size = new System.Drawing.Size(76, 17);
			this.LastNameRadioButton.TabIndex = 17;
			this.LastNameRadioButton.TabStop = true;
			this.LastNameRadioButton.Text = "Last Name";
			this.LastNameRadioButton.UseVisualStyleBackColor = true;
			this.LastNameRadioButton.CheckedChanged += new System.EventHandler(this.Refill);
			// 
			// FirstNameRadioButton
			// 
			this.FirstNameRadioButton.AutoSize = true;
			this.FirstNameRadioButton.Location = new System.Drawing.Point(101, 6);
			this.FirstNameRadioButton.Name = "FirstNameRadioButton";
			this.FirstNameRadioButton.Size = new System.Drawing.Size(75, 17);
			this.FirstNameRadioButton.TabIndex = 16;
			this.FirstNameRadioButton.TabStop = true;
			this.FirstNameRadioButton.Text = "First Name";
			this.FirstNameRadioButton.UseVisualStyleBackColor = true;
			this.FirstNameRadioButton.CheckedChanged += new System.EventHandler(this.Refill);
			// 
			// ToolTip
			// 
			this.ToolTip.IsBalloon = true;
			// 
			// PlayerListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 629);
			this.Controls.Add(this.OrderPlayersByPanel);
			this.Controls.Add(this.RemoveButton);
			this.Controls.Add(this.GroupsButton);
			this.Controls.Add(this.ConflictsButton);
			this.Controls.Add(this.EditButton);
			this.Controls.Add(this.NewPlayerPanel);
			this.Controls.Add(this.PlayerListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PlayerListForm";
			this.ShowIcon = false;
			this.Text = "Players";
			this.Load += new System.EventHandler(this.PlayerListForm_Load);
			this.NewPlayerPanel.ResumeLayout(false);
			this.NewPlayerPanel.PerformLayout();
			this.OrderPlayersByPanel.ResumeLayout(false);
			this.OrderPlayersByPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private global::System.Windows.Forms.ListBox PlayerListBox;
		private global::System.Windows.Forms.Label FirstNameLabel;
		private global::System.Windows.Forms.Label LastNameLabel;
		private global::System.Windows.Forms.Label EmailLabel;
		private global::System.Windows.Forms.TextBox FirstNameTextBox;
		private global::System.Windows.Forms.TextBox LastNameTextBox;
		private global::System.Windows.Forms.TextBox EmailAddressTextBox;
		private global::System.Windows.Forms.Button AddPlayerButton;
		private global::System.Windows.Forms.Panel NewPlayerPanel;
		private System.Windows.Forms.Button EditButton;
		private System.Windows.Forms.Button ConflictsButton;
		private System.Windows.Forms.Button GroupsButton;
		private System.Windows.Forms.Button RemoveButton;
		private System.Windows.Forms.Label OrderPlayersByLabel;
		private System.Windows.Forms.Panel OrderPlayersByPanel;
		private System.Windows.Forms.RadioButton LastNameRadioButton;
		private System.Windows.Forms.RadioButton FirstNameRadioButton;
		private System.Windows.Forms.ToolTip ToolTip;
	}
}
