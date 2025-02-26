using PC.Controls;

namespace PC.Forms
{
	partial class TournamentInfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TournamentInfoForm));
			this.DetailsTabControl = new System.Windows.Forms.TabControl();
			this.DetailsTabPage = new System.Windows.Forms.TabPage();
			this.TeamsTabPage = new System.Windows.Forms.TabPage();
			this.DetailsTabControl.SuspendLayout();
			this.DetailsTabPage.SuspendLayout();
			this.TeamsTabPage.SuspendLayout();
			this.TournamentControl = new TournamentControl();
			this.TeamsControl = new TeamsControl();
			this.SuspendLayout();
			//
			// DetailsTabControl
			//
			this.DetailsTabControl.Controls.Add(this.DetailsTabPage);
			this.DetailsTabControl.Controls.Add(this.TeamsTabPage);
			this.DetailsTabControl.Location = new System.Drawing.Point(10, 13);
			this.DetailsTabControl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.DetailsTabControl.Name = "DetailsTabControl";
			this.DetailsTabControl.SelectedIndex = 0;
			this.DetailsTabControl.Size = new System.Drawing.Size(712, 412);
			this.DetailsTabControl.TabIndex = 46;
			//
			// DetailsTabPage
			//
			this.DetailsTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.DetailsTabPage.Controls.Add(this.TournamentControl);
			this.DetailsTabPage.Location = new System.Drawing.Point(4, 22);
			this.DetailsTabPage.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.DetailsTabPage.Name = "DetailsTabPage";
			this.DetailsTabPage.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.DetailsTabPage.Size = new System.Drawing.Size(704, 386);
			this.DetailsTabPage.TabIndex = 0;
			this.DetailsTabPage.Text = "Event Settings";
			//
			// TournamentControl
			//
			this.TournamentControl.Location = new System.Drawing.Point(5, 5);
			this.TournamentControl.Margin = new System.Windows.Forms.Padding(0);
			this.TournamentControl.Name = "TournamentControl";
			this.TournamentControl.Size = new System.Drawing.Size(691, 383);
			this.TournamentControl.TabIndex = 0;
			//
			// TeamsTabPage
			//
			this.TeamsTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.TeamsTabPage.Controls.Add(this.TeamsControl);
			this.TeamsTabPage.Location = new System.Drawing.Point(4, 22);
			this.TeamsTabPage.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.TeamsTabPage.Name = "TeamsTabPage";
			this.TeamsTabPage.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
			this.TeamsTabPage.Size = new System.Drawing.Size(704, 386);
			this.TeamsTabPage.TabIndex = 1;
			this.TeamsTabPage.Text = "Tournament Teams";
			//
			// TeamsControl
			//
			this.TeamsControl.Location = new System.Drawing.Point(4, 14);
			this.TeamsControl.Margin = new System.Windows.Forms.Padding(0);
			this.TeamsControl.Name = "TeamsControl";
			this.TeamsControl.Size = new System.Drawing.Size(696, 361);
			this.TeamsControl.TabIndex = 0;
			//
			// TournamentInfoForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(735, 436);
			this.Controls.Add(this.DetailsTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TournamentInfoForm";
			this.ShowIcon = false;
			this.Text = "Event Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TournamentInfoForm_FormClosing);
			this.Load += new System.EventHandler(this.TournamentInfoForm_Load);
			this.DetailsTabControl.ResumeLayout(false);
			this.DetailsTabPage.ResumeLayout(false);
			this.TeamsTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private TabControl DetailsTabControl;
		private TabPage DetailsTabPage;
		private TabPage TeamsTabPage;
		private TournamentControl TournamentControl;
		private TeamsControl TeamsControl;
	}
}
