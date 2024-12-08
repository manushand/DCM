using PC.Controls;

namespace PC.Forms
{
	partial class GroupInfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupInfoForm));
			this.ScoringSystemComboBox = new System.Windows.Forms.ComboBox();
			this.ScoringSystemLabel = new System.Windows.Forms.Label();
			this.ConflictTextBox = new System.Windows.Forms.TextBox();
			this.GroupNameTextBox = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.MemberConflictLabel = new System.Windows.Forms.Label();
			this.GroupNameLabel = new System.Windows.Forms.Label();
			this.CancelFormButton = new System.Windows.Forms.Button();
			this.DescriptionTextBox = new System.Windows.Forms.TextBox();
			this.DescriptionLabel = new System.Windows.Forms.Label();
			this.RatingMethodLabel = new System.Windows.Forms.Label();
			this.RatingMethodDescriptionLabel = new System.Windows.Forms.Label();
			this.GroupMembershipControl = new GroupMembershipControl();
			this.SuspendLayout();
			// 
			// ScoringSystemComboBox
			// 
			this.ScoringSystemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ScoringSystemComboBox.FormattingEnabled = true;
			this.ScoringSystemComboBox.Location = new System.Drawing.Point(264, 682);
			this.ScoringSystemComboBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.ScoringSystemComboBox.Name = "ScoringSystemComboBox";
			this.ScoringSystemComboBox.Size = new System.Drawing.Size(431, 39);
			this.ScoringSystemComboBox.TabIndex = 95;
			this.ScoringSystemComboBox.SelectedIndexChanged += new System.EventHandler(this.ScoringSystemComboBox_SelectedIndexChanged);
			// 
			// ScoringSystemLabel
			// 
			this.ScoringSystemLabel.AutoSize = true;
			this.ScoringSystemLabel.Location = new System.Drawing.Point(29, 689);
			this.ScoringSystemLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.ScoringSystemLabel.Name = "ScoringSystemLabel";
			this.ScoringSystemLabel.Size = new System.Drawing.Size(207, 32);
			this.ScoringSystemLabel.TabIndex = 94;
			this.ScoringSystemLabel.Text = "Rating System:";
			// 
			// ConflictTextBox
			// 
			this.ConflictTextBox.Location = new System.Drawing.Point(605, 612);
			this.ConflictTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.ConflictTextBox.Name = "ConflictTextBox";
			this.ConflictTextBox.Size = new System.Drawing.Size(89, 38);
			this.ConflictTextBox.TabIndex = 92;
			// 
			// GroupNameTextBox
			// 
			this.GroupNameTextBox.Location = new System.Drawing.Point(149, 26);
			this.GroupNameTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.GroupNameTextBox.Name = "GroupNameTextBox";
			this.GroupNameTextBox.Size = new System.Drawing.Size(545, 38);
			this.GroupNameTextBox.TabIndex = 91;
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(176, 830);
			this.OkButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(200, 55);
			this.OkButton.TabIndex = 93;
			this.OkButton.Text = "OK";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// MemberConflictLabel
			// 
			this.MemberConflictLabel.AutoSize = true;
			this.MemberConflictLabel.Location = new System.Drawing.Point(29, 620);
			this.MemberConflictLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.MemberConflictLabel.Name = "MemberConflictLabel";
			this.MemberConflictLabel.Size = new System.Drawing.Size(580, 32);
			this.MemberConflictLabel.TabIndex = 90;
			this.MemberConflictLabel.Text = "Default Member Conflict in Tournament Play:";
			// 
			// GroupNameLabel
			// 
			this.GroupNameLabel.AutoSize = true;
			this.GroupNameLabel.Location = new System.Drawing.Point(32, 33);
			this.GroupNameLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.GroupNameLabel.Name = "GroupNameLabel";
			this.GroupNameLabel.Size = new System.Drawing.Size(98, 32);
			this.GroupNameLabel.TabIndex = 89;
			this.GroupNameLabel.Text = "Name:";
			// 
			// CancelFormButton
			// 
			this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelFormButton.Location = new System.Drawing.Point(392, 830);
			this.CancelFormButton.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.CancelFormButton.Name = "CancelFormButton";
			this.CancelFormButton.Size = new System.Drawing.Size(200, 55);
			this.CancelFormButton.TabIndex = 98;
			this.CancelFormButton.Text = "Cancel";
			this.CancelFormButton.UseVisualStyleBackColor = true;
			// 
			// DescriptionTextBox
			// 
			this.DescriptionTextBox.Location = new System.Drawing.Point(37, 143);
			this.DescriptionTextBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.DescriptionTextBox.Multiline = true;
			this.DescriptionTextBox.Name = "DescriptionTextBox";
			this.DescriptionTextBox.Size = new System.Drawing.Size(657, 433);
			this.DescriptionTextBox.TabIndex = 100;
			// 
			// DescriptionLabel
			// 
			this.DescriptionLabel.AutoSize = true;
			this.DescriptionLabel.Location = new System.Drawing.Point(32, 98);
			this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.DescriptionLabel.Name = "DescriptionLabel";
			this.DescriptionLabel.Size = new System.Drawing.Size(166, 32);
			this.DescriptionLabel.TabIndex = 101;
			this.DescriptionLabel.Text = "Description:";
			// 
			// GroupMembershipControl
			// 
			this.GroupMembershipControl.Location = new System.Drawing.Point(755, 26);
			this.GroupMembershipControl.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
			this.GroupMembershipControl.Name = "GroupMembershipControl";
			this.GroupMembershipControl.Size = new System.Drawing.Size(1389, 863);
			this.GroupMembershipControl.TabIndex = 99;
			// 
			// RatingMethodLabel
			// 
			this.RatingMethodLabel.AutoSize = true;
			this.RatingMethodLabel.Location = new System.Drawing.Point(32, 756);
			this.RatingMethodLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.RatingMethodLabel.Name = "RatingMethodLabel";
			this.RatingMethodLabel.Size = new System.Drawing.Size(208, 32);
			this.RatingMethodLabel.TabIndex = 102;
			this.RatingMethodLabel.Text = "Rating Method:";
			// 
			// RatingMethodDescriptionLabel
			// 
			this.RatingMethodDescriptionLabel.AutoSize = true;
			this.RatingMethodDescriptionLabel.Location = new System.Drawing.Point(258, 756);
			this.RatingMethodDescriptionLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.RatingMethodDescriptionLabel.Name = "RatingMethodDescriptionLabel";
			this.RatingMethodDescriptionLabel.Size = new System.Drawing.Size(283, 32);
			this.RatingMethodDescriptionLabel.TabIndex = 103;
			this.RatingMethodDescriptionLabel.Text = "Sum of Game Scores";
			// 
			// GroupInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelFormButton;
			this.ClientSize = new System.Drawing.Size(2173, 916);
			this.Controls.Add(this.RatingMethodDescriptionLabel);
			this.Controls.Add(this.RatingMethodLabel);
			this.Controls.Add(this.DescriptionLabel);
			this.Controls.Add(this.DescriptionTextBox);
			this.Controls.Add(this.GroupMembershipControl);
			this.Controls.Add(this.CancelFormButton);
			this.Controls.Add(this.ScoringSystemComboBox);
			this.Controls.Add(this.ScoringSystemLabel);
			this.Controls.Add(this.ConflictTextBox);
			this.Controls.Add(this.GroupNameTextBox);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.MemberConflictLabel);
			this.Controls.Add(this.GroupNameLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupInfoForm";
			this.ShowIcon = false;
			this.Text = "Group Details";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GroupInfoForm_FormClosing);
			this.Load += new System.EventHandler(this.GroupInfoForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ComboBox ScoringSystemComboBox;
		private Label ScoringSystemLabel;
		private TextBox ConflictTextBox;
		private TextBox GroupNameTextBox;
		private Button OkButton;
		private Label MemberConflictLabel;
		private Label GroupNameLabel;
		private Button CancelFormButton;
		private GroupMembershipControl GroupMembershipControl;
		private TextBox DescriptionTextBox;
		private Label DescriptionLabel;
		private Label RatingMethodLabel;
		private Label RatingMethodDescriptionLabel;
	}
}
