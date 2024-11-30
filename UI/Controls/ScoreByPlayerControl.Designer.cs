namespace DCM.UI.Controls
{
	partial class ScoreByPlayerControl
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
			this.FinalScoresTabControl = new System.Windows.Forms.TabControl();
			this.QualifiedPlayersTabPage = new System.Windows.Forms.TabPage();
			this.UnqualifiedPlayersTabPage = new System.Windows.Forms.TabPage();
			this.PrintButton = new System.Windows.Forms.Button();
			this.FinalScoresDataGridView = new System.Windows.Forms.DataGridView();
			this.LegendLabel = new System.Windows.Forms.Label();
			this.FinalScoresTabControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.FinalScoresDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// FinalScoresTabControl
			// 
			this.FinalScoresTabControl.Controls.Add(this.QualifiedPlayersTabPage);
			this.FinalScoresTabControl.Controls.Add(this.UnqualifiedPlayersTabPage);
			this.FinalScoresTabControl.Location = new System.Drawing.Point(8, 4);
			this.FinalScoresTabControl.Name = "FinalScoresTabControl";
			this.FinalScoresTabControl.SelectedIndex = 0;
			this.FinalScoresTabControl.Size = new System.Drawing.Size(657, 437);
			this.FinalScoresTabControl.TabIndex = 5;
			this.FinalScoresTabControl.SelectedIndexChanged += new System.EventHandler(this.FinalScoresTabControl_SelectedIndexChanged);
			// 
			// QualifiedPlayersTabPage
			// 
			this.QualifiedPlayersTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.QualifiedPlayersTabPage.Location = new System.Drawing.Point(4, 22);
			this.QualifiedPlayersTabPage.Name = "QualifiedPlayersTabPage";
			this.QualifiedPlayersTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.QualifiedPlayersTabPage.Size = new System.Drawing.Size(649, 411);
			this.QualifiedPlayersTabPage.TabIndex = 0;
			this.QualifiedPlayersTabPage.Text = "Qualified Players";
			// 
			// UnqualifiedPlayersTabPage
			// 
			this.UnqualifiedPlayersTabPage.Location = new System.Drawing.Point(4, 22);
			this.UnqualifiedPlayersTabPage.Name = "UnqualifiedPlayersTabPage";
			this.UnqualifiedPlayersTabPage.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
			this.UnqualifiedPlayersTabPage.Size = new System.Drawing.Size(649, 411);
			this.UnqualifiedPlayersTabPage.TabIndex = 1;
			this.UnqualifiedPlayersTabPage.Text = "Unqualified Players";
			this.UnqualifiedPlayersTabPage.UseVisualStyleBackColor = true;
			// 
			// PrintButton
			// 
			this.PrintButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PrintButton.Location = new System.Drawing.Point(583, 401);
			this.PrintButton.Name = "PrintButton";
			this.PrintButton.Size = new System.Drawing.Size(75, 23);
			this.PrintButton.TabIndex = 0;
			this.PrintButton.Text = "Print…";
			this.PrintButton.UseVisualStyleBackColor = true;
			this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
			// 
			// FinalScoresDataGridView
			// 
			this.FinalScoresDataGridView.AllowUserToAddRows = false;
			this.FinalScoresDataGridView.AllowUserToDeleteRows = false;
			this.FinalScoresDataGridView.AllowUserToResizeColumns = false;
			this.FinalScoresDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.FinalScoresDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.FinalScoresDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.FinalScoresDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.FinalScoresDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.FinalScoresDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.FinalScoresDataGridView.EnableHeadersVisualStyles = false;
			this.FinalScoresDataGridView.Location = new System.Drawing.Point(13, 27);
			this.FinalScoresDataGridView.Name = "FinalScoresDataGridView";
			this.FinalScoresDataGridView.ReadOnly = true;
			this.FinalScoresDataGridView.RowHeadersVisible = false;
			this.FinalScoresDataGridView.RowTemplate.Height = 16;
			this.FinalScoresDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.FinalScoresDataGridView.ShowCellToolTips = false;
			this.FinalScoresDataGridView.Size = new System.Drawing.Size(648, 363);
			this.FinalScoresDataGridView.TabIndex = 4;
			this.FinalScoresDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.FinalScoresDataGridView_CellContentDoubleClick);
			this.FinalScoresDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.FinalScoresDataGridView_DataBindingComplete);
			// 
			// LegendLabel
			// 
			this.LegendLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LegendLabel.Location = new System.Drawing.Point(13, 401);
			this.LegendLabel.Name = "LegendLabel";
			this.LegendLabel.Size = new System.Drawing.Size(567, 23);
			this.LegendLabel.TabIndex = 6;
			this.LegendLabel.Text = "* ─ did not play    † ─ score dropped   ‡ ─ score scaled";
			this.LegendLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ScoreByPlayerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PrintButton);
			this.Controls.Add(this.LegendLabel);
			this.Controls.Add(this.FinalScoresDataGridView);
			this.Controls.Add(this.FinalScoresTabControl);
			this.Name = "ScoreByPlayerControl";
			this.Size = new System.Drawing.Size(673, 444);
			this.FinalScoresTabControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.FinalScoresDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private TabControl FinalScoresTabControl;
		private TabPage QualifiedPlayersTabPage;
		private TabPage UnqualifiedPlayersTabPage;
		private DataGridView FinalScoresDataGridView;
		private Label LegendLabel;
		private Button PrintButton;
	}
}
