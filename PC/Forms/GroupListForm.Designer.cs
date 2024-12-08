namespace PC.Forms
{
	partial class GroupListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupListForm));
			this.GroupComboBox = new System.Windows.Forms.ComboBox();
			this.GroupNameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// GroupComboBox
			// 
			this.GroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GroupComboBox.FormattingEnabled = true;
			this.GroupComboBox.Location = new System.Drawing.Point(55, 12);
			this.GroupComboBox.Name = "GroupComboBox";
			this.GroupComboBox.Size = new System.Drawing.Size(289, 21);
			this.GroupComboBox.TabIndex = 3;
			this.GroupComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupComboBox_SelectedIndexChanged);
			// 
			// GroupNameLabel
			// 
			this.GroupNameLabel.AutoSize = true;
			this.GroupNameLabel.Location = new System.Drawing.Point(10, 15);
			this.GroupNameLabel.Name = "GroupNameLabel";
			this.GroupNameLabel.Size = new System.Drawing.Size(39, 13);
			this.GroupNameLabel.TabIndex = 2;
			this.GroupNameLabel.Text = "Group:";
			// 
			// GroupListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(369, 45);
			this.Controls.Add(this.GroupComboBox);
			this.Controls.Add(this.GroupNameLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupListForm";
			this.ShowIcon = false;
			this.Text = "Open Group";
			this.Load += new System.EventHandler(this.GroupListForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ComboBox GroupComboBox;
		private Label GroupNameLabel;
	}
}
