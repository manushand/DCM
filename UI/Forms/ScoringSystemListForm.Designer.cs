namespace DCM.UI.Forms
{
	partial class ScoringSystemListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScoringSystemListForm));
			this.ScoringSystemListBox = new System.Windows.Forms.ListBox();
			this.OpenButton = new System.Windows.Forms.Button();
			this.NewButton = new System.Windows.Forms.Button();
			this.DeleteButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ScoringSystemListBox
			// 
			this.ScoringSystemListBox.FormattingEnabled = true;
			this.ScoringSystemListBox.Location = new System.Drawing.Point(10, 41);
			this.ScoringSystemListBox.Margin = new System.Windows.Forms.Padding(1);
			this.ScoringSystemListBox.Name = "ScoringSystemListBox";
			this.ScoringSystemListBox.Size = new System.Drawing.Size(262, 225);
			this.ScoringSystemListBox.TabIndex = 0;
			this.ScoringSystemListBox.SelectedIndexChanged += new System.EventHandler(this.ScoringSystemListBox_SelectedIndexChanged);
			this.ScoringSystemListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ScoringSystemListBox_MouseDoubleClick);
			// 
			// OpenButton
			// 
			this.OpenButton.Location = new System.Drawing.Point(9, 10);
			this.OpenButton.Margin = new System.Windows.Forms.Padding(1);
			this.OpenButton.Name = "OpenButton";
			this.OpenButton.Size = new System.Drawing.Size(86, 23);
			this.OpenButton.TabIndex = 1;
			this.OpenButton.Text = "Details…";
			this.OpenButton.UseVisualStyleBackColor = true;
			this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
			// 
			// NewButton
			// 
			this.NewButton.Location = new System.Drawing.Point(97, 10);
			this.NewButton.Margin = new System.Windows.Forms.Padding(1);
			this.NewButton.Name = "NewButton";
			this.NewButton.Size = new System.Drawing.Size(86, 23);
			this.NewButton.TabIndex = 2;
			this.NewButton.Text = "New…";
			this.NewButton.UseVisualStyleBackColor = true;
			this.NewButton.Click += new System.EventHandler(this.NewButton_Click);
			// 
			// DeleteButton
			// 
			this.DeleteButton.Location = new System.Drawing.Point(185, 10);
			this.DeleteButton.Margin = new System.Windows.Forms.Padding(1);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(86, 23);
			this.DeleteButton.TabIndex = 3;
			this.DeleteButton.Text = "Delete…";
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// ScoringSystemListForm
			// 
			this.AcceptButton = this.OpenButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 276);
			this.Controls.Add(this.DeleteButton);
			this.Controls.Add(this.NewButton);
			this.Controls.Add(this.OpenButton);
			this.Controls.Add(this.ScoringSystemListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScoringSystemListForm";
			this.ShowIcon = false;
			this.Text = "Scoring Systems";
			this.Load += new System.EventHandler(this.ScoringSystemListForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private ListBox ScoringSystemListBox;
		private Button OpenButton;
		private Button NewButton;
		private Button DeleteButton;
	}
}
