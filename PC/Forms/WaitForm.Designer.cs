namespace PC.Forms
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
			ActivityLabel = new Label();
			ProgressBar = new ProgressBar();
			SuspendLayout();
			// 
			// ActivityLabel
			// 
			ActivityLabel.AutoSize = true;
			ActivityLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
			ActivityLabel.Location = new Point(14, 10);
			ActivityLabel.Margin = new Padding(4, 0, 4, 0);
			ActivityLabel.Name = "ActivityLabel";
			ActivityLabel.Size = new Size(60, 13);
			ActivityLabel.TabIndex = 0;
			ActivityLabel.Text = "Seeding…";
			// 
			// ProgressBar
			// 
			ProgressBar.Location = new Point(18, 39);
			ProgressBar.Margin = new Padding(4, 3, 4, 3);
			ProgressBar.Name = "ProgressBar";
			ProgressBar.Size = new Size(458, 27);
			ProgressBar.Style = ProgressBarStyle.Marquee;
			ProgressBar.TabIndex = 1;
			// 
			// WaitForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ButtonHighlight;
			ClientSize = new Size(495, 82);
			ControlBox = false;
			Controls.Add(ProgressBar);
			Controls.Add(ActivityLabel);
			FormBorderStyle = FormBorderStyle.None;
			Margin = new Padding(4, 3, 4, 3);
			Name = "WaitForm";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterParent;
			Text = "WaitForm";
			Load += WaitForm_Load;
			ResumeLayout(false);
			PerformLayout();

		}

		#endregion

		private Label ActivityLabel;
		private ProgressBar ProgressBar;
	}
}
