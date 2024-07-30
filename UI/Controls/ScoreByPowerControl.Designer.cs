namespace DCM.UI.Controls
{
	partial class ScoreByPowerControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.BestGamesTabControl = new System.Windows.Forms.TabControl();
			this.BestGameTabPage = new System.Windows.Forms.TabPage();
			this.BestAustriaTabPage = new System.Windows.Forms.TabPage();
			this.BestEnglandTabPage = new System.Windows.Forms.TabPage();
			this.BestFranceTabPage = new System.Windows.Forms.TabPage();
			this.BestGermanyTabPage = new System.Windows.Forms.TabPage();
			this.BestItalyTabPage = new System.Windows.Forms.TabPage();
			this.BestRussiaTabPage = new System.Windows.Forms.TabPage();
			this.BestTurkeyTabPage = new System.Windows.Forms.TabPage();
			this.PrintButton = new System.Windows.Forms.Button();
			this.BestPowersDataGridView = new System.Windows.Forms.DataGridView();
			this.BestGamesTabControl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.BestPowersDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// BestGamesTabControl
			// 
			this.BestGamesTabControl.Controls.Add(this.BestGameTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestAustriaTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestEnglandTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestFranceTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestGermanyTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestItalyTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestRussiaTabPage);
			this.BestGamesTabControl.Controls.Add(this.BestTurkeyTabPage);
			this.BestGamesTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.BestGamesTabControl.Location = new System.Drawing.Point(8, 4);
			this.BestGamesTabControl.Name = "BestGamesTabControl";
			this.BestGamesTabControl.SelectedIndex = 0;
			this.BestGamesTabControl.Size = new System.Drawing.Size(527, 437);
			this.BestGamesTabControl.TabIndex = 7;
			this.BestGamesTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.BestGamesTabControl_DrawItem);
			this.BestGamesTabControl.SelectedIndexChanged += new System.EventHandler(this.BestGamesTabControl_SelectedIndexChanged);
			// 
			// BestGameTabPage
			// 
			this.BestGameTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.BestGameTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestGameTabPage.Name = "BestGameTabPage";
			this.BestGameTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestGameTabPage.TabIndex = 7;
			this.BestGameTabPage.Text = "OVERALL";
			// 
			// BestAustriaTabPage
			// 
			this.BestAustriaTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.BestAustriaTabPage.ForeColor = System.Drawing.Color.Black;
			this.BestAustriaTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestAustriaTabPage.Name = "BestAustriaTabPage";
			this.BestAustriaTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.BestAustriaTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestAustriaTabPage.TabIndex = 0;
			this.BestAustriaTabPage.Text = "AUSTRIA";
			// 
			// BestEnglandTabPage
			// 
			this.BestEnglandTabPage.BackColor = System.Drawing.Color.RoyalBlue;
			this.BestEnglandTabPage.ForeColor = System.Drawing.Color.White;
			this.BestEnglandTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestEnglandTabPage.Name = "BestEnglandTabPage";
			this.BestEnglandTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.BestEnglandTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestEnglandTabPage.TabIndex = 1;
			this.BestEnglandTabPage.Text = "ENGLAND";
			// 
			// BestFranceTabPage
			// 
			this.BestFranceTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestFranceTabPage.Name = "BestFranceTabPage";
			this.BestFranceTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestFranceTabPage.TabIndex = 2;
			this.BestFranceTabPage.Text = "FRANCE";
			this.BestFranceTabPage.UseVisualStyleBackColor = true;
			// 
			// BestGermanyTabPage
			// 
			this.BestGermanyTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestGermanyTabPage.Name = "BestGermanyTabPage";
			this.BestGermanyTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestGermanyTabPage.TabIndex = 3;
			this.BestGermanyTabPage.Text = "GERMANY";
			this.BestGermanyTabPage.UseVisualStyleBackColor = true;
			// 
			// BestItalyTabPage
			// 
			this.BestItalyTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestItalyTabPage.Name = "BestItalyTabPage";
			this.BestItalyTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestItalyTabPage.TabIndex = 4;
			this.BestItalyTabPage.Text = "ITALY";
			this.BestItalyTabPage.UseVisualStyleBackColor = true;
			// 
			// BestRussiaTabPage
			// 
			this.BestRussiaTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestRussiaTabPage.Name = "BestRussiaTabPage";
			this.BestRussiaTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestRussiaTabPage.TabIndex = 5;
			this.BestRussiaTabPage.Text = "RUSSIA";
			this.BestRussiaTabPage.UseVisualStyleBackColor = true;
			// 
			// BestTurkeyTabPage
			// 
			this.BestTurkeyTabPage.Location = new System.Drawing.Point(4, 22);
			this.BestTurkeyTabPage.Name = "BestTurkeyTabPage";
			this.BestTurkeyTabPage.Size = new System.Drawing.Size(519, 411);
			this.BestTurkeyTabPage.TabIndex = 6;
			this.BestTurkeyTabPage.Text = "TURKEY";
			this.BestTurkeyTabPage.UseVisualStyleBackColor = true;
			// 
			// PrintButton
			// 
			this.PrintButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PrintButton.Location = new System.Drawing.Point(433, 401);
			this.PrintButton.Name = "PrintButton";
			this.PrintButton.Size = new System.Drawing.Size(75, 23);
			this.PrintButton.TabIndex = 8;
			this.PrintButton.Text = "Print…";
			this.PrintButton.UseVisualStyleBackColor = true;
			this.PrintButton.Click += new System.EventHandler(this.PrintButton_Click);
			// 
			// BestPowersDataGridView
			// 
			this.BestPowersDataGridView.AllowUserToAddRows = false;
			this.BestPowersDataGridView.AllowUserToDeleteRows = false;
			this.BestPowersDataGridView.AllowUserToResizeColumns = false;
			this.BestPowersDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.BestPowersDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.BestPowersDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.BestPowersDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.BestPowersDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.BestPowersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.BestPowersDataGridView.EnableHeadersVisualStyles = false;
			this.BestPowersDataGridView.Location = new System.Drawing.Point(33, 49);
			this.BestPowersDataGridView.Name = "BestPowersDataGridView";
			this.BestPowersDataGridView.ReadOnly = true;
			this.BestPowersDataGridView.RowHeadersVisible = false;
			this.BestPowersDataGridView.RowTemplate.Height = 16;
			this.BestPowersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.BestPowersDataGridView.ShowCellToolTips = false;
			this.BestPowersDataGridView.Size = new System.Drawing.Size(475, 346);
			this.BestPowersDataGridView.TabIndex = 6;
			this.BestPowersDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BestPowersDataGridView_CellContentDoubleClick);
			this.BestPowersDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.BestPowersDataGridView_DataBindingComplete);
			// 
			// ScoreByPowerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PrintButton);
			this.Controls.Add(this.BestPowersDataGridView);
			this.Controls.Add(this.BestGamesTabControl);
			this.Name = "ScoreByPowerControl";
			this.Size = new System.Drawing.Size(545, 444);
			this.BestGamesTabControl.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.BestPowersDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl BestGamesTabControl;
		private System.Windows.Forms.TabPage BestAustriaTabPage;
		private System.Windows.Forms.TabPage BestEnglandTabPage;
		private System.Windows.Forms.TabPage BestFranceTabPage;
		private System.Windows.Forms.TabPage BestGermanyTabPage;
		private System.Windows.Forms.TabPage BestItalyTabPage;
		private System.Windows.Forms.TabPage BestRussiaTabPage;
		private System.Windows.Forms.TabPage BestTurkeyTabPage;
		private System.Windows.Forms.TabPage BestGameTabPage;
		private System.Windows.Forms.DataGridView BestPowersDataGridView;
		private System.Windows.Forms.Button PrintButton;
	}
}
