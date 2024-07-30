namespace DCM.UI.Controls
{
	partial class GroupMembershipControl
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
			this.OrderByPanel = new System.Windows.Forms.Panel();
			this.LastNameRadioButton = new System.Windows.Forms.RadioButton();
			this.FirstNameRadioButton = new System.Windows.Forms.RadioButton();
			this.OrderPlayersByLabel = new System.Windows.Forms.Label();
			this.MembershipsButton = new System.Windows.Forms.Button();
			this.JoinButton = new System.Windows.Forms.Button();
			this.MembersLabel = new System.Windows.Forms.Label();
			this.NonMemberLabel = new System.Windows.Forms.Label();
			this.MemberListBox = new System.Windows.Forms.ListBox();
			this.NonMemberListBox = new System.Windows.Forms.ListBox();
			this.OrderByPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// OrderByPanel
			// 
			this.OrderByPanel.Controls.Add(this.LastNameRadioButton);
			this.OrderByPanel.Controls.Add(this.FirstNameRadioButton);
			this.OrderByPanel.Controls.Add(this.OrderPlayersByLabel);
			this.OrderByPanel.Location = new System.Drawing.Point(264, 329);
			this.OrderByPanel.Name = "OrderByPanel";
			this.OrderByPanel.Size = new System.Drawing.Size(243, 30);
			this.OrderByPanel.TabIndex = 25;
			// 
			// LastNameRadioButton
			// 
			this.LastNameRadioButton.AutoSize = true;
			this.LastNameRadioButton.Location = new System.Drawing.Point(165, 6);
			this.LastNameRadioButton.Name = "LastNameRadioButton";
			this.LastNameRadioButton.Size = new System.Drawing.Size(76, 17);
			this.LastNameRadioButton.TabIndex = 17;
			this.LastNameRadioButton.TabStop = true;
			this.LastNameRadioButton.Text = "Last Name";
			this.LastNameRadioButton.UseVisualStyleBackColor = true;
			this.LastNameRadioButton.Click += new System.EventHandler(this.FillMembershipLists);
			// 
			// FirstNameRadioButton
			// 
			this.FirstNameRadioButton.AutoSize = true;
			this.FirstNameRadioButton.Location = new System.Drawing.Point(91, 6);
			this.FirstNameRadioButton.Name = "FirstNameRadioButton";
			this.FirstNameRadioButton.Size = new System.Drawing.Size(75, 17);
			this.FirstNameRadioButton.TabIndex = 16;
			this.FirstNameRadioButton.TabStop = true;
			this.FirstNameRadioButton.Text = "First Name";
			this.FirstNameRadioButton.UseVisualStyleBackColor = true;
			this.FirstNameRadioButton.Click += new System.EventHandler(this.FillMembershipLists);
			// 
			// OrderPlayersByLabel
			// 
			this.OrderPlayersByLabel.AutoSize = true;
			this.OrderPlayersByLabel.Location = new System.Drawing.Point(2, 8);
			this.OrderPlayersByLabel.Name = "OrderPlayersByLabel";
			this.OrderPlayersByLabel.Size = new System.Drawing.Size(88, 13);
			this.OrderPlayersByLabel.TabIndex = 15;
			this.OrderPlayersByLabel.Text = "Order Players By:";
			// 
			// MembershipsButton
			// 
			this.MembershipsButton.Location = new System.Drawing.Point(146, 197);
			this.MembershipsButton.Name = "MembershipsButton";
			this.MembershipsButton.Size = new System.Drawing.Size(102, 23);
			this.MembershipsButton.TabIndex = 24;
			this.MembershipsButton.Text = "List Memberships";
			this.MembershipsButton.UseVisualStyleBackColor = true;
			this.MembershipsButton.Click += new System.EventHandler(this.MembershipsButton_Click);
			// 
			// JoinButton
			// 
			this.JoinButton.Location = new System.Drawing.Point(146, 141);
			this.JoinButton.Name = "JoinButton";
			this.JoinButton.Size = new System.Drawing.Size(102, 23);
			this.JoinButton.TabIndex = 23;
			this.JoinButton.Text = "◀───── Join Group";
			this.JoinButton.UseVisualStyleBackColor = true;
			this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
			// 
			// MembersLabel
			// 
			this.MembersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MembersLabel.Location = new System.Drawing.Point(3, 3);
			this.MembersLabel.Name = "MembersLabel";
			this.MembersLabel.Size = new System.Drawing.Size(140, 18);
			this.MembersLabel.TabIndex = 22;
			this.MembersLabel.Text = "Members";
			this.MembersLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// NonMemberLabel
			// 
			this.NonMemberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NonMemberLabel.Location = new System.Drawing.Point(253, 3);
			this.NonMemberLabel.Name = "NonMemberLabel";
			this.NonMemberLabel.Size = new System.Drawing.Size(264, 18);
			this.NonMemberLabel.TabIndex = 21;
			this.NonMemberLabel.Text = "Non-Members";
			this.NonMemberLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MemberListBox
			// 
			this.MemberListBox.FormattingEnabled = true;
			this.MemberListBox.Location = new System.Drawing.Point(3, 24);
			this.MemberListBox.Name = "MemberListBox";
			this.MemberListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.MemberListBox.Size = new System.Drawing.Size(140, 303);
			this.MemberListBox.TabIndex = 20;
			this.MemberListBox.SelectedIndexChanged += new System.EventHandler(this.MemberListBox_SelectedIndexChanged);
			this.MemberListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.JoinButton_Click);
			// 
			// NonMemberListBox
			// 
			this.NonMemberListBox.ColumnWidth = 132;
			this.NonMemberListBox.FormattingEnabled = true;
			this.NonMemberListBox.Location = new System.Drawing.Point(253, 24);
			this.NonMemberListBox.MultiColumn = true;
			this.NonMemberListBox.Name = "NonMemberListBox";
			this.NonMemberListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.NonMemberListBox.Size = new System.Drawing.Size(264, 303);
			this.NonMemberListBox.TabIndex = 19;
			this.NonMemberListBox.SelectedIndexChanged += new System.EventHandler(this.NonMemberListBox_SelectedIndexChanged);
			this.NonMemberListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.JoinButton_Click);
			// 
			// GroupMembershipControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.OrderByPanel);
			this.Controls.Add(this.MembershipsButton);
			this.Controls.Add(this.JoinButton);
			this.Controls.Add(this.MembersLabel);
			this.Controls.Add(this.NonMemberLabel);
			this.Controls.Add(this.MemberListBox);
			this.Controls.Add(this.NonMemberListBox);
			this.Name = "GroupMembershipControl";
			this.Size = new System.Drawing.Size(520, 362);
			this.Load += new System.EventHandler(this.GroupMembershipControl_Load);
			this.OrderByPanel.ResumeLayout(false);
			this.OrderByPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel OrderByPanel;
		private System.Windows.Forms.RadioButton LastNameRadioButton;
		private System.Windows.Forms.RadioButton FirstNameRadioButton;
		private System.Windows.Forms.Label OrderPlayersByLabel;
		private System.Windows.Forms.Button MembershipsButton;
		private System.Windows.Forms.Button JoinButton;
		private System.Windows.Forms.Label MembersLabel;
		private System.Windows.Forms.Label NonMemberLabel;
		private System.Windows.Forms.ListBox MemberListBox;
		private System.Windows.Forms.ListBox NonMemberListBox;
	}
}
