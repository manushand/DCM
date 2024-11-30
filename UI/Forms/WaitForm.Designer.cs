namespace DCM.UI.Forms
{
	partial class WaitForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaitForm));
			this.ActivityLabel = new System.Windows.Forms.Label();
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// ActivityLabel
			// 
			this.ActivityLabel.AutoSize = true;
			this.ActivityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ActivityLabel.Location = new System.Drawing.Point(12, 9);
			this.ActivityLabel.Name = "ActivityLabel";
			this.ActivityLabel.Size = new System.Drawing.Size(60, 13);
			this.ActivityLabel.TabIndex = 0;
			this.ActivityLabel.Text = "Seeding…";
			// 
			// ProgressBar
			// 
			this.ProgressBar.Location = new System.Drawing.Point(15, 34);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(393, 23);
			this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.ProgressBar.TabIndex = 1;
			// 
			// WaitForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.ClientSize = new System.Drawing.Size(424, 71);
			this.ControlBox = false;
			this.Controls.Add(this.ProgressBar);
			this.Controls.Add(this.ActivityLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WaitForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WaitForm";
			this.Load += new System.EventHandler(this.WaitForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label ActivityLabel;
		private ProgressBar ProgressBar;
	}
}
