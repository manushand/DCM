namespace DCM.UI.Forms
{
	partial class ConflictsForm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.ConflictsLabel = new System.Windows.Forms.Label();
			this.PlayerNameComboBox = new System.Windows.Forms.ComboBox();
			this.IncreaseButton = new System.Windows.Forms.Button();
			this.DecreaseButton = new System.Windows.Forms.Button();
			this.NewConflictNameComboBox = new System.Windows.Forms.ComboBox();
			this.NewConflictLabel = new System.Windows.Forms.Label();
			this.ConflictsDataGridView = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.ConflictsDataGridView)).BeginInit();
			this.SuspendLayout();
			//
			// ConflictsLabel
			//
			this.ConflictsLabel.AutoSize = true;
			this.ConflictsLabel.Location = new System.Drawing.Point(35, 31);
			this.ConflictsLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.ConflictsLabel.Name = "ConflictsLabel";
			this.ConflictsLabel.Size = new System.Drawing.Size(173, 32);
			this.ConflictsLabel.TabIndex = 0;
			this.ConflictsLabel.Text = "Conflicts for:";
			//
			// PlayerNameComboBox
			//
			this.PlayerNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PlayerNameComboBox.FormattingEnabled = true;
			this.PlayerNameComboBox.Location = new System.Drawing.Point(224, 24);
			this.PlayerNameComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.PlayerNameComboBox.Name = "PlayerNameComboBox";
			this.PlayerNameComboBox.Size = new System.Drawing.Size(580, 39);
			this.PlayerNameComboBox.TabIndex = 1;
			this.PlayerNameComboBox.SelectedIndexChanged += new System.EventHandler(this.PlayerNameComboBox_SelectedIndexChanged);
			//
			// IncreaseButton
			//
			this.IncreaseButton.Location = new System.Drawing.Point(440, 556);
			this.IncreaseButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.IncreaseButton.Name = "IncreaseButton";
			this.IncreaseButton.Size = new System.Drawing.Size(200, 55);
			this.IncreaseButton.TabIndex = 3;
			this.IncreaseButton.Text = "Increase ▲";
			this.IncreaseButton.UseVisualStyleBackColor = true;
			this.IncreaseButton.Click += new System.EventHandler(this.ModifyConflictButton_Click);
			//
			// DecreaseButton
			//
			this.DecreaseButton.Location = new System.Drawing.Point(224, 556);
			this.DecreaseButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.DecreaseButton.Name = "DecreaseButton";
			this.DecreaseButton.Size = new System.Drawing.Size(200, 55);
			this.DecreaseButton.TabIndex = 4;
			this.DecreaseButton.Text = "▼ Decrease";
			this.DecreaseButton.UseVisualStyleBackColor = true;
			this.DecreaseButton.Click += new System.EventHandler(this.ModifyConflictButton_Click);
			//
			// NewConflictNameComboBox
			//
			this.NewConflictNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.NewConflictNameComboBox.FormattingEnabled = true;
			this.NewConflictNameComboBox.Location = new System.Drawing.Point(224, 625);
			this.NewConflictNameComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.NewConflictNameComboBox.Name = "NewConflictNameComboBox";
			this.NewConflictNameComboBox.Size = new System.Drawing.Size(580, 39);
			this.NewConflictNameComboBox.TabIndex = 5;
			this.NewConflictNameComboBox.SelectedIndexChanged += new System.EventHandler(this.NewConflictNameComboBox_SelectedIndexChanged);
			//
			// NewConflictLabel
			//
			this.NewConflictLabel.AutoSize = true;
			this.NewConflictLabel.Location = new System.Drawing.Point(35, 632);
			this.NewConflictLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.NewConflictLabel.Name = "NewConflictLabel";
			this.NewConflictLabel.Size = new System.Drawing.Size(182, 32);
			this.NewConflictLabel.TabIndex = 6;
			this.NewConflictLabel.Text = "New Conflict:";
			//
			// ConflictsDataGridView
			//
			this.ConflictsDataGridView.AllowUserToAddRows = false;
			this.ConflictsDataGridView.AllowUserToDeleteRows = false;
			this.ConflictsDataGridView.AllowUserToResizeColumns = false;
			this.ConflictsDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ConflictsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.ConflictsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ConflictsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.ConflictsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.ConflictsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ConflictsDataGridView.ColumnHeadersVisible = false;
			this.ConflictsDataGridView.Location = new System.Drawing.Point(43, 88);
			this.ConflictsDataGridView.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.ConflictsDataGridView.MultiSelect = false;
			this.ConflictsDataGridView.Name = "ConflictsDataGridView";
			this.ConflictsDataGridView.ReadOnly = true;
			this.ConflictsDataGridView.RowHeadersVisible = false;
			this.ConflictsDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
			this.ConflictsDataGridView.RowTemplate.Height = 16;
			this.ConflictsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ConflictsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ConflictsDataGridView.ShowCellToolTips = false;
			this.ConflictsDataGridView.Size = new System.Drawing.Size(768, 453);
			this.ConflictsDataGridView.TabIndex = 7;
			this.ConflictsDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.ConflictsDataGridView_DataBindingComplete);
			this.ConflictsDataGridView.SelectionChanged += new System.EventHandler(this.ConflictsDataGridView_SelectionChanged);
			//
			// ConflictsForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(856, 703);
			this.Controls.Add(this.ConflictsDataGridView);
			this.Controls.Add(this.NewConflictLabel);
			this.Controls.Add(this.NewConflictNameComboBox);
			this.Controls.Add(this.DecreaseButton);
			this.Controls.Add(this.IncreaseButton);
			this.Controls.Add(this.PlayerNameComboBox);
			this.Controls.Add(this.ConflictsLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConflictsForm";
			this.ShowIcon = false;
			this.Text = "Player Conflicts";
			this.Load += new System.EventHandler(this.ConflictsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.ConflictsDataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label ConflictsLabel;
		private ComboBox PlayerNameComboBox;
		private Button IncreaseButton;
		private Button DecreaseButton;
		private ComboBox NewConflictNameComboBox;
		private Label NewConflictLabel;
		private DataGridView ConflictsDataGridView;
	}
}
