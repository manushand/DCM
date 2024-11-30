namespace DCM.UI.Controls
{
	partial class ScoreByRoundControl
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
			this.ScoresByRoundDataGridView = new System.Windows.Forms.DataGridView();
			this.RoundScoresTabControl = new System.Windows.Forms.TabControl();
			this.Round1ScoresTab = new System.Windows.Forms.TabPage();
			this.PrintButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ScoresByRoundDataGridView)).BeginInit();
			this.RoundScoresTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// ScoresByRoundDataGridView
			// 
			this.ScoresByRoundDataGridView.AllowUserToAddRows = false;
			this.ScoresByRoundDataGridView.AllowUserToDeleteRows = false;
			this.ScoresByRoundDataGridView.AllowUserToResizeColumns = false;
			this.ScoresByRoundDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ScoresByRoundDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.ScoresByRoundDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.ScoresByRoundDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.ScoresByRoundDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.ScoresByRoundDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ScoresByRoundDataGridView.EnableHeadersVisualStyles = false;
			this.ScoresByRoundDataGridView.Location = new System.Drawing.Point(14, 29);
			this.ScoresByRoundDataGridView.Name = "ScoresByRoundDataGridView";
			this.ScoresByRoundDataGridView.ReadOnly = true;
			this.ScoresByRoundDataGridView.RowHeadersVisible = false;
			this.ScoresByRoundDataGridView.RowTemplate.Height = 16;
			this.ScoresByRoundDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ScoresByRoundDataGridView.ShowCellToolTips = false;
			this.ScoresByRoundDataGridView.Size = new System.Drawing.Size(342, 365);
			this.ScoresByRoundDataGridView.TabIndex = 4;
			this.ScoresByRoundDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ScoresByRoundDataGridView_CellContentDoubleClick);
			this.ScoresByRoundDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.ScoresByRoundDataGridView_DataBindingComplete);
			// 
			// RoundScoresTabControl
			// 
			this.RoundScoresTabControl.Controls.Add(this.Round1ScoresTab);
			this.RoundScoresTabControl.Location = new System.Drawing.Point(8, 4);
			this.RoundScoresTabControl.Name = "RoundScoresTabControl";
			this.RoundScoresTabControl.SelectedIndex = 0;
			this.RoundScoresTabControl.Size = new System.Drawing.Size(354, 437);
			this.RoundScoresTabControl.TabIndex = 3;
			this.RoundScoresTabControl.SelectedIndexChanged += new System.EventHandler(this.RoundScoresTabControl_SelectedIndexChanged);
			// 
			// Round1ScoresTab
			// 
			this.Round1ScoresTab.BackColor = System.Drawing.SystemColors.Control;
			this.Round1ScoresTab.Location = new System.Drawing.Point(4, 22);
			this.Round1ScoresTab.Name = "Round1ScoresTab";
			this.Round1ScoresTab.Padding = new System.Windows.Forms.Padding(3);
			this.Round1ScoresTab.Size = new System.Drawing.Size(346, 411);
			this.Round1ScoresTab.TabIndex = 1;
			this.Round1ScoresTab.Text = "Round 1";
			// 
			// PrintButton
			// 
			this.PrintButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PrintButton.Location = new System.Drawing.Point(279, 400);
			this.PrintButton.Name = "PrintButton";
			this.PrintButton.Size = new System.Drawing.Size(75, 23);
			this.PrintButton.TabIndex = 5;
			this.PrintButton.Text = "Print…";
			this.PrintButton.UseVisualStyleBackColor = true;
			this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
			// 
			// ScoreByRoundControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PrintButton);
			this.Controls.Add(this.ScoresByRoundDataGridView);
			this.Controls.Add(this.RoundScoresTabControl);
			this.Name = "ScoreByRoundControl";
			this.Size = new System.Drawing.Size(371, 444);
			((System.ComponentModel.ISupportInitialize)(this.ScoresByRoundDataGridView)).EndInit();
			this.RoundScoresTabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DataGridView ScoresByRoundDataGridView;
		private TabControl RoundScoresTabControl;
		private TabPage Round1ScoresTab;
		private Button PrintButton;
	}
}
