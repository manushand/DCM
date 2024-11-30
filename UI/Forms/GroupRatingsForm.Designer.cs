namespace DCM.UI.Forms
{
	partial class GroupRatingsForm
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
			this.RatingsDataGridView = new System.Windows.Forms.DataGridView();
			this.RatingsTabControl = new System.Windows.Forms.TabControl();
			this.GroupGamesTabPage = new System.Windows.Forms.TabPage();
			this.SystemGamesTabPage = new System.Windows.Forms.TabPage();
			this.ScorableGamesTabPage = new System.Windows.Forms.TabPage();
			this.RatedGamesLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.RatingsDataGridView)).BeginInit();
			this.RatingsTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// RatingsDataGridView
			// 
			this.RatingsDataGridView.AllowUserToAddRows = false;
			this.RatingsDataGridView.AllowUserToDeleteRows = false;
			this.RatingsDataGridView.AllowUserToResizeColumns = false;
			this.RatingsDataGridView.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
			this.RatingsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.RatingsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.RatingsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			dataGridViewCellStyle2.Alignment = MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.RatingsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.RatingsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RatingsDataGridView.EnableHeadersVisualStyles = false;
			this.RatingsDataGridView.Location = new System.Drawing.Point(43, 95);
			this.RatingsDataGridView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.RatingsDataGridView.MultiSelect = false;
			this.RatingsDataGridView.Name = "RatingsDataGridView";
			this.RatingsDataGridView.ReadOnly = true;
			this.RatingsDataGridView.RowHeadersVisible = false;
			this.RatingsDataGridView.RowHeadersWidth = 102;
			this.RatingsDataGridView.RowTemplate.Height = 16;
			this.RatingsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RatingsDataGridView.ShowCellToolTips = false;
			this.RatingsDataGridView.Size = new System.Drawing.Size(819, 906);
			this.RatingsDataGridView.TabIndex = 0;
			this.RatingsDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RatingsDataGridView_CellContentDoubleClick);
			this.RatingsDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.RatingsDataGridView_DataBindingComplete);
			// 
			// RatingsTabControl
			// 
			this.RatingsTabControl.Controls.Add(this.GroupGamesTabPage);
			this.RatingsTabControl.Controls.Add(this.SystemGamesTabPage);
			this.RatingsTabControl.Controls.Add(this.ScorableGamesTabPage);
			this.RatingsTabControl.Location = new System.Drawing.Point(32, 43);
			this.RatingsTabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.RatingsTabControl.Name = "RatingsTabControl";
			this.RatingsTabControl.SelectedIndex = 0;
			this.RatingsTabControl.Size = new System.Drawing.Size(840, 959);
			this.RatingsTabControl.TabIndex = 1;
			this.RatingsTabControl.SelectedIndexChanged += new System.EventHandler(this.RatingsTabControl_SelectedIndexChanged);
			// 
			// GroupGamesTabPage
			// 
			this.GroupGamesTabPage.Location = new System.Drawing.Point(10, 48);
			this.GroupGamesTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GroupGamesTabPage.Name = "GroupGamesTabPage";
			this.GroupGamesTabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.GroupGamesTabPage.Size = new System.Drawing.Size(820, 901);
			this.GroupGamesTabPage.TabIndex = 0;
			this.GroupGamesTabPage.Text = "Group Games";
			this.GroupGamesTabPage.UseVisualStyleBackColor = true;
			// 
			// SystemGamesTabPage
			// 
			this.SystemGamesTabPage.Location = new System.Drawing.Point(10, 48);
			this.SystemGamesTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SystemGamesTabPage.Name = "SystemGamesTabPage";
			this.SystemGamesTabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SystemGamesTabPage.Size = new System.Drawing.Size(820, 901);
			this.SystemGamesTabPage.TabIndex = 1;
			this.SystemGamesTabPage.Text = "System Games";
			this.SystemGamesTabPage.UseVisualStyleBackColor = true;
			// 
			// ScorableGamesTabPage
			// 
			this.ScorableGamesTabPage.Location = new System.Drawing.Point(10, 48);
			this.ScorableGamesTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ScorableGamesTabPage.Name = "ScorableGamesTabPage";
			this.ScorableGamesTabPage.Size = new System.Drawing.Size(820, 901);
			this.ScorableGamesTabPage.TabIndex = 2;
			this.ScorableGamesTabPage.Text = "All Scorable Games";
			this.ScorableGamesTabPage.UseVisualStyleBackColor = true;
			// 
			// RatedGamesLabel
			// 
			this.RatedGamesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RatedGamesLabel.Location = new System.Drawing.Point(43, 1025);
			this.RatedGamesLabel.Name = "RatedGamesLabel";
			this.RatedGamesLabel.Size = new System.Drawing.Size(819, 86);
			this.RatedGamesLabel.TabIndex = 2;
			this.RatedGamesLabel.Text = "Group Games";
			this.RatedGamesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// GroupRatingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(912, 1142);
			this.Controls.Add(this.RatedGamesLabel);
			this.Controls.Add(this.RatingsDataGridView);
			this.Controls.Add(this.RatingsTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupRatingsForm";
			this.ShowIcon = false;
			this.Text = "Member Ratings";
			this.Load += new System.EventHandler(this.GroupRatingsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.RatingsDataGridView)).EndInit();
			this.RatingsTabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DataGridView RatingsDataGridView;
		private TabControl RatingsTabControl;
		private TabPage GroupGamesTabPage;
		private TabPage SystemGamesTabPage;
		private TabPage ScorableGamesTabPage;
		private Label RatedGamesLabel;
	}
}
