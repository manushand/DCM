using DCM.UI.Controls;

namespace DCM.UI.Forms
{
	partial class RoundInfoForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoundInfoForm));
			this.RoundsTabControl = new System.Windows.Forms.TabControl();
			this.RegistrationTabPage = new System.Windows.Forms.TabPage();
			this.RegistrationControl = new RegistrationControl();
			this.Round1TabPage = new System.Windows.Forms.TabPage();
			this.RoundsTabControl.SuspendLayout();
			this.RegistrationTabPage.SuspendLayout();
			this.Round1TabPage.SuspendLayout();
			this.RoundControl = new RoundControl();
			this.SuspendLayout();
			// 
			// RoundsTabControl
			// 
			this.RoundsTabControl.Controls.Add(this.RegistrationTabPage);
			this.RoundsTabControl.Controls.Add(this.Round1TabPage);
			this.RoundsTabControl.Location = new System.Drawing.Point(13, 13);
			this.RoundsTabControl.Name = "RoundsTabControl";
			this.RoundsTabControl.SelectedIndex = 0;
			this.RoundsTabControl.Size = new System.Drawing.Size(970, 571);
			this.RoundsTabControl.TabIndex = 15;
			this.RoundsTabControl.SelectedIndexChanged += new System.EventHandler(this.RoundsTabControl_SelectedIndexChanged);
			// 
			// RegistrationTabPage
			// 
			this.RegistrationTabPage.BackColor = System.Drawing.SystemColors.Control;
			this.RegistrationTabPage.Controls.Add(this.RegistrationControl);
			this.RegistrationTabPage.Location = new System.Drawing.Point(4, 22);
			this.RegistrationTabPage.Name = "RegistrationTabPage";
			this.RegistrationTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.RegistrationTabPage.Size = new System.Drawing.Size(962, 545);
			this.RegistrationTabPage.TabIndex = 0;
			this.RegistrationTabPage.Text = "Registration";
			// 
			// RegistrationControl
			// 
			this.RegistrationControl.Location = new System.Drawing.Point(3, 5);
			this.RegistrationControl.Margin = new System.Windows.Forms.Padding(0);
			this.RegistrationControl.Name = "RegistrationControl";
			this.RegistrationControl.Size = new System.Drawing.Size(792, 533);
			this.RegistrationControl.TabIndex = 0;
			// 
			// Round1TabPage
			// 
			this.Round1TabPage.BackColor = System.Drawing.SystemColors.Control;
			this.Round1TabPage.Controls.Add(this.RoundControl);
			this.Round1TabPage.Location = new System.Drawing.Point(4, 22);
			this.Round1TabPage.Name = "Round1TabPage";
			this.Round1TabPage.Padding = new System.Windows.Forms.Padding(3);
			this.Round1TabPage.Size = new System.Drawing.Size(962, 545);
			this.Round1TabPage.TabIndex = 1;
			this.Round1TabPage.Text = "Round 1";
			// 
			// RoundControl
			// 
			this.RoundControl.Location = new System.Drawing.Point(2, 5);
			this.RoundControl.Margin = new System.Windows.Forms.Padding(0);
			this.RoundControl.Name = "RoundControl";
			this.RoundControl.Size = new System.Drawing.Size(957, 535);
			this.RoundControl.TabIndex = 0;
			// 
			// RoundInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(996, 595);
			this.Controls.Add(this.RoundsTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "RoundInfoForm";
			this.ShowIcon = false;
			this.Text = "Tournament Rounds";
			this.Load += new System.EventHandler(this.RoundInfoForm_Load);
			this.RoundsTabControl.ResumeLayout(false);
			this.RegistrationTabPage.ResumeLayout(false);
			this.Round1TabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl RoundsTabControl;
		private System.Windows.Forms.TabPage RegistrationTabPage;
		private System.Windows.Forms.TabPage Round1TabPage;
		private RegistrationControl RegistrationControl;
		private RoundControl RoundControl;
	}
}
