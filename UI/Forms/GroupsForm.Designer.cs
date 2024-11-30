namespace DCM.UI.Forms
{
	using Controls;

	partial class GroupsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupsForm));
			this.GroupListBox = new System.Windows.Forms.ListBox();
			this.GroupsLabel = new System.Windows.Forms.Label();
			this.DissolveButton = new System.Windows.Forms.Button();
			this.EditButton = new System.Windows.Forms.Button();
			this.NewGroupButton = new System.Windows.Forms.Button();
			this.GroupMembershipControl = new GroupMembershipControl();
			this.SuspendLayout();
			// 
			// GroupListBox
			// 
			this.GroupListBox.FormattingEnabled = true;
			this.GroupListBox.Location = new System.Drawing.Point(12, 56);
			this.GroupListBox.Name = "GroupListBox";
			this.GroupListBox.Size = new System.Drawing.Size(264, 277);
			this.GroupListBox.TabIndex = 19;
			this.GroupListBox.SelectedIndexChanged += new System.EventHandler(this.GroupListBox_SelectedIndexChanged);
			this.GroupListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.EditButton_Click);
			// 
			// GroupsLabel
			// 
			this.GroupsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GroupsLabel.Location = new System.Drawing.Point(12, 9);
			this.GroupsLabel.Name = "GroupsLabel";
			this.GroupsLabel.Size = new System.Drawing.Size(264, 18);
			this.GroupsLabel.TabIndex = 20;
			this.GroupsLabel.Text = "Groups";
			this.GroupsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// DissolveButton
			// 
			this.DissolveButton.Location = new System.Drawing.Point(195, 30);
			this.DissolveButton.Name = "DissolveButton";
			this.DissolveButton.Size = new System.Drawing.Size(81, 23);
			this.DissolveButton.TabIndex = 25;
			this.DissolveButton.Text = "Dissolve…";
			this.DissolveButton.UseVisualStyleBackColor = true;
			this.DissolveButton.Click += new System.EventHandler(this.DissolveButton_Click);
			// 
			// EditButton
			// 
			this.EditButton.Location = new System.Drawing.Point(99, 30);
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(90, 23);
			this.EditButton.TabIndex = 22;
			this.EditButton.Text = "Edit…";
			this.EditButton.UseVisualStyleBackColor = true;
			this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
			// 
			// NewGroupButton
			// 
			this.NewGroupButton.Location = new System.Drawing.Point(12, 30);
			this.NewGroupButton.Name = "NewGroupButton";
			this.NewGroupButton.Size = new System.Drawing.Size(81, 23);
			this.NewGroupButton.TabIndex = 30;
			this.NewGroupButton.Text = "New…";
			this.NewGroupButton.UseVisualStyleBackColor = true;
			this.NewGroupButton.Click += new System.EventHandler(this.NewGroupButton_Click);
			// 
			// GroupMembershipControl
			// 
			this.GroupMembershipControl.Location = new System.Drawing.Point(281, 7);
			this.GroupMembershipControl.Name = "GroupMembershipControl";
			this.GroupMembershipControl.Size = new System.Drawing.Size(520, 362);
			this.GroupMembershipControl.TabIndex = 31;
			// 
			// GroupsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(813, 376);
			this.Controls.Add(this.GroupMembershipControl);
			this.Controls.Add(this.NewGroupButton);
			this.Controls.Add(this.DissolveButton);
			this.Controls.Add(this.EditButton);
			this.Controls.Add(this.GroupsLabel);
			this.Controls.Add(this.GroupListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GroupsForm";
			this.ShowIcon = false;
			this.Text = "Player Groups";
			this.Load += new System.EventHandler(this.GroupsForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private ListBox GroupListBox;
		private Label GroupsLabel;
		private Button DissolveButton;
		private Button EditButton;
		private Button NewGroupButton;
		private GroupMembershipControl GroupMembershipControl;
	}
}
