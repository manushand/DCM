namespace DCM.UI.Forms
{
	using Controls;

	partial class ScoresForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScoresForm));
			this.ScoresTabControl = new System.Windows.Forms.TabControl();
			this.PlayerScoreTabPage = new System.Windows.Forms.TabPage();
			this.ScoreByPlayerControl = new ScoreByPlayerControl();
			this.RoundScoreTabPage = new System.Windows.Forms.TabPage();
			this.ScoreByRoundControl = new ScoreByRoundControl();
			this.PowerScoreTabPage = new System.Windows.Forms.TabPage();
			this.ScoreByPowerControl = new ScoreByPowerControl();
			this.ScoresTabControl.SuspendLayout();
			this.PlayerScoreTabPage.SuspendLayout();
			this.RoundScoreTabPage.SuspendLayout();
			this.PowerScoreTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// ScoresTabControl
			// 
			this.ScoresTabControl.Controls.Add(this.PlayerScoreTabPage);
			this.ScoresTabControl.Controls.Add(this.RoundScoreTabPage);
			this.ScoresTabControl.Controls.Add(this.PowerScoreTabPage);
			this.ScoresTabControl.Location = new System.Drawing.Point(13, 13);
			this.ScoresTabControl.Name = "ScoresTabControl";
			this.ScoresTabControl.SelectedIndex = 0;
			this.ScoresTabControl.Size = new System.Drawing.Size(687, 478);
			this.ScoresTabControl.TabIndex = 0;
			this.ScoresTabControl.SelectedIndexChanged += new System.EventHandler(this.ScoresTabControl_SelectedIndexChanged);
			// 
			// PlayerScoreTabPage
			// 
			this.PlayerScoreTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.PlayerScoreTabPage.Controls.Add(this.ScoreByPlayerControl);
			this.PlayerScoreTabPage.Location = new System.Drawing.Point(4, 22);
			this.PlayerScoreTabPage.Name = "PlayerScoreTabPage";
			this.PlayerScoreTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.PlayerScoreTabPage.Size = new System.Drawing.Size(679, 452);
			this.PlayerScoreTabPage.TabIndex = 0;
			this.PlayerScoreTabPage.Text = "Individual Scores";
			// 
			// ScoreByPlayerControl
			// 
			this.ScoreByPlayerControl.Location = new System.Drawing.Point(3, 3);
			this.ScoreByPlayerControl.Name = "ScoreByPlayerControl";
			this.ScoreByPlayerControl.Size = new System.Drawing.Size(679, 446);
			this.ScoreByPlayerControl.TabIndex = 0;
			// 
			// RoundScoreTabPage
			// 
			this.RoundScoreTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.RoundScoreTabPage.Controls.Add(this.ScoreByRoundControl);
			this.RoundScoreTabPage.Location = new System.Drawing.Point(4, 22);
			this.RoundScoreTabPage.Name = "RoundScoreTabPage";
			this.RoundScoreTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.RoundScoreTabPage.Size = new System.Drawing.Size(679, 452);
			this.RoundScoreTabPage.TabIndex = 1;
			this.RoundScoreTabPage.Text = "Scores By Round";
			// 
			// ScoreByRoundControl
			// 
			this.ScoreByRoundControl.Location = new System.Drawing.Point(6, 6);
			this.ScoreByRoundControl.Name = "ScoreByRoundControl";
			this.ScoreByRoundControl.Size = new System.Drawing.Size(375, 446);
			this.ScoreByRoundControl.TabIndex = 0;
			// 
			// PowerScoreTabPage
			// 
			this.PowerScoreTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.PowerScoreTabPage.Controls.Add(this.ScoreByPowerControl);
			this.PowerScoreTabPage.Location = new System.Drawing.Point(4, 22);
			this.PowerScoreTabPage.Name = "PowerScoreTabPage";
			this.PowerScoreTabPage.Size = new System.Drawing.Size(679, 452);
			this.PowerScoreTabPage.TabIndex = 2;
			this.PowerScoreTabPage.Text = "Best Games";
			// 
			// ScoreByPowerControl
			// 
			this.ScoreByPowerControl.Location = new System.Drawing.Point(3, 3);
			this.ScoreByPowerControl.Name = "ScoreByPowerControl";
			this.ScoreByPowerControl.Size = new System.Drawing.Size(540, 449);
			this.ScoreByPowerControl.TabIndex = 0;
			// 
			// ScoresForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(714, 503);
			this.Controls.Add(this.ScoresTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScoresForm";
			this.ShowIcon = false;
			this.Text = "Scores";
			this.Load += new System.EventHandler(this.ScoresForm_Load);
			this.ScoresTabControl.ResumeLayout(false);
			this.PlayerScoreTabPage.ResumeLayout(false);
			this.RoundScoreTabPage.ResumeLayout(false);
			this.PowerScoreTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private TabControl ScoresTabControl;
		private TabPage PlayerScoreTabPage;
		private TabPage RoundScoreTabPage;
		private TabPage PowerScoreTabPage;
		private ScoreByPlayerControl ScoreByPlayerControl;
		private ScoreByRoundControl ScoreByRoundControl;
		private ScoreByPowerControl ScoreByPowerControl;
	}
}
