namespace DCM.UI.Forms
{
    partial class EmailSettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailSettingsForm));
			this.HostLabel = new System.Windows.Forms.Label();
			this.HostTextBox = new System.Windows.Forms.TextBox();
			this.PortLabel = new System.Windows.Forms.Label();
			this.PortTextBox = new System.Windows.Forms.TextBox();
			this.UseSslCheckBox = new System.Windows.Forms.CheckBox();
			this.UsernameLabel = new System.Windows.Forms.Label();
			this.UsernameTextBox = new System.Windows.Forms.TextBox();
			this.PasswordTextBox = new System.Windows.Forms.TextBox();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.TestEmailTextBox = new System.Windows.Forms.TextBox();
			this.TestEmailLabel = new System.Windows.Forms.Label();
			this.OnlySendToTestCheckBox = new System.Windows.Forms.CheckBox();
			this.SaveButton = new System.Windows.Forms.Button();
			this.MailFromTextBox = new System.Windows.Forms.TextBox();
			this.FromAddressLabel = new System.Windows.Forms.Label();
			this.FromNameTextBox = new System.Windows.Forms.TextBox();
			this.FromNameLabel = new System.Windows.Forms.Label();
			this.NonEventMailingsLabel = new System.Windows.Forms.Label();
			this.TemplatesTabControl = new System.Windows.Forms.TabControl();
			this.AssignmentTemplateTabPage = new System.Windows.Forms.TabPage();
			this.AssignmentTemplateTextBox = new System.Windows.Forms.TextBox();
			this.AnnouncementTemplateTabPage = new System.Windows.Forms.TabPage();
			this.AnnouncementTemplateTextBox = new System.Windows.Forms.TextBox();
			this.TemplateLegendLabel = new System.Windows.Forms.Label();
			this.SaveOrSendTemplateButton = new System.Windows.Forms.Button();
			this.TemplatesTabControl.SuspendLayout();
			this.AssignmentTemplateTabPage.SuspendLayout();
			this.AnnouncementTemplateTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// HostLabel
			// 
			this.HostLabel.AutoSize = true;
			this.HostLabel.Location = new System.Drawing.Point(24, 22);
			this.HostLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.HostLabel.Name = "HostLabel";
			this.HostLabel.Size = new System.Drawing.Size(65, 13);
			this.HostLabel.TabIndex = 0;
			this.HostLabel.Text = "SMTP Host:";
			// 
			// HostTextBox
			// 
			this.HostTextBox.Location = new System.Drawing.Point(88, 22);
			this.HostTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.HostTextBox.Name = "HostTextBox";
			this.HostTextBox.Size = new System.Drawing.Size(158, 20);
			this.HostTextBox.TabIndex = 1;
			this.HostTextBox.TextChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// PortLabel
			// 
			this.PortLabel.AutoSize = true;
			this.PortLabel.Location = new System.Drawing.Point(58, 50);
			this.PortLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.PortLabel.Name = "PortLabel";
			this.PortLabel.Size = new System.Drawing.Size(29, 13);
			this.PortLabel.TabIndex = 2;
			this.PortLabel.Text = "Port:";
			// 
			// PortTextBox
			// 
			this.PortTextBox.Location = new System.Drawing.Point(88, 47);
			this.PortTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.PortTextBox.Name = "PortTextBox";
			this.PortTextBox.Size = new System.Drawing.Size(44, 20);
			this.PortTextBox.TabIndex = 3;
			this.PortTextBox.Leave += new System.EventHandler(this.Settings_Changed);
			// 
			// UseSslCheckBox
			// 
			this.UseSslCheckBox.AutoSize = true;
			this.UseSslCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.UseSslCheckBox.Location = new System.Drawing.Point(172, 49);
			this.UseSslCheckBox.Margin = new System.Windows.Forms.Padding(1);
			this.UseSslCheckBox.Name = "UseSslCheckBox";
			this.UseSslCheckBox.Size = new System.Drawing.Size(74, 17);
			this.UseSslCheckBox.TabIndex = 4;
			this.UseSslCheckBox.Text = "Use SSL?";
			this.UseSslCheckBox.UseVisualStyleBackColor = true;
			this.UseSslCheckBox.CheckedChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// UsernameLabel
			// 
			this.UsernameLabel.AutoSize = true;
			this.UsernameLabel.Location = new System.Drawing.Point(29, 76);
			this.UsernameLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.UsernameLabel.Name = "UsernameLabel";
			this.UsernameLabel.Size = new System.Drawing.Size(58, 13);
			this.UsernameLabel.TabIndex = 5;
			this.UsernameLabel.Text = "Username:";
			// 
			// UsernameTextBox
			// 
			this.UsernameTextBox.Location = new System.Drawing.Point(88, 75);
			this.UsernameTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.UsernameTextBox.Name = "UsernameTextBox";
			this.UsernameTextBox.Size = new System.Drawing.Size(158, 20);
			this.UsernameTextBox.TabIndex = 6;
			this.UsernameTextBox.TextChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// PasswordTextBox
			// 
			this.PasswordTextBox.Location = new System.Drawing.Point(88, 102);
			this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.PasswordTextBox.Name = "PasswordTextBox";
			this.PasswordTextBox.Size = new System.Drawing.Size(158, 20);
			this.PasswordTextBox.TabIndex = 8;
			this.PasswordTextBox.TextChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Location = new System.Drawing.Point(31, 104);
			this.PasswordLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(56, 13);
			this.PasswordLabel.TabIndex = 7;
			this.PasswordLabel.Text = "Password:";
			// 
			// TestEmailTextBox
			// 
			this.TestEmailTextBox.Location = new System.Drawing.Point(88, 208);
			this.TestEmailTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.TestEmailTextBox.Name = "TestEmailTextBox";
			this.TestEmailTextBox.Size = new System.Drawing.Size(158, 20);
			this.TestEmailTextBox.TabIndex = 10;
			this.TestEmailTextBox.TextChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// TestEmailLabel
			// 
			this.TestEmailLabel.AutoSize = true;
			this.TestEmailLabel.Location = new System.Drawing.Point(9, 210);
			this.TestEmailLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.TestEmailLabel.Name = "TestEmailLabel";
			this.TestEmailLabel.Size = new System.Drawing.Size(80, 13);
			this.TestEmailLabel.TabIndex = 9;
			this.TestEmailLabel.Text = "Send Tests To:";
			// 
			// OnlySendToTestCheckBox
			// 
			this.OnlySendToTestCheckBox.AutoSize = true;
			this.OnlySendToTestCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.OnlySendToTestCheckBox.Location = new System.Drawing.Point(136, 240);
			this.OnlySendToTestCheckBox.Margin = new System.Windows.Forms.Padding(1);
			this.OnlySendToTestCheckBox.Name = "OnlySendToTestCheckBox";
			this.OnlySendToTestCheckBox.Size = new System.Drawing.Size(110, 17);
			this.OnlySendToTestCheckBox.TabIndex = 11;
			this.OnlySendToTestCheckBox.Text = "Send Only Tests?";
			this.OnlySendToTestCheckBox.UseVisualStyleBackColor = true;
			this.OnlySendToTestCheckBox.CheckedChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// SaveButton
			// 
			this.SaveButton.Location = new System.Drawing.Point(88, 268);
			this.SaveButton.Margin = new System.Windows.Forms.Padding(1);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(98, 28);
			this.SaveButton.TabIndex = 12;
			this.SaveButton.Text = "Save Settings";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveAndSendButton_Click);
			// 
			// MailFromTextBox
			// 
			this.MailFromTextBox.Location = new System.Drawing.Point(88, 131);
			this.MailFromTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.MailFromTextBox.Name = "MailFromTextBox";
			this.MailFromTextBox.Size = new System.Drawing.Size(158, 20);
			this.MailFromTextBox.TabIndex = 14;
			this.MailFromTextBox.TextChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// FromAddressLabel
			// 
			this.FromAddressLabel.AutoSize = true;
			this.FromAddressLabel.Location = new System.Drawing.Point(12, 133);
			this.FromAddressLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.FromAddressLabel.Name = "FromAddressLabel";
			this.FromAddressLabel.Size = new System.Drawing.Size(74, 13);
			this.FromAddressLabel.TabIndex = 13;
			this.FromAddressLabel.Text = "From Address:";
			// 
			// FromNameTextBox
			// 
			this.FromNameTextBox.Location = new System.Drawing.Point(88, 160);
			this.FromNameTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.FromNameTextBox.Name = "FromNameTextBox";
			this.FromNameTextBox.Size = new System.Drawing.Size(158, 20);
			this.FromNameTextBox.TabIndex = 16;
			this.FromNameTextBox.TextChanged += new System.EventHandler(this.Settings_Changed);
			// 
			// FromNameLabel
			// 
			this.FromNameLabel.AutoSize = true;
			this.FromNameLabel.Location = new System.Drawing.Point(50, 161);
			this.FromNameLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.FromNameLabel.Name = "FromNameLabel";
			this.FromNameLabel.Size = new System.Drawing.Size(38, 13);
			this.FromNameLabel.TabIndex = 15;
			this.FromNameLabel.Text = "Name:";
			// 
			// NonEventMailingsLabel
			// 
			this.NonEventMailingsLabel.AutoSize = true;
			this.NonEventMailingsLabel.Location = new System.Drawing.Point(48, 184);
			this.NonEventMailingsLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.NonEventMailingsLabel.Name = "NonEventMailingsLabel";
			this.NonEventMailingsLabel.Size = new System.Drawing.Size(201, 13);
			this.NonEventMailingsLabel.TabIndex = 17;
			this.NonEventMailingsLabel.Text = "(Used only for non-event-related mailings)";
			// 
			// TemplatesTabControl
			// 
			this.TemplatesTabControl.Controls.Add(this.AssignmentTemplateTabPage);
			this.TemplatesTabControl.Controls.Add(this.AnnouncementTemplateTabPage);
			this.TemplatesTabControl.Location = new System.Drawing.Point(263, 22);
			this.TemplatesTabControl.Name = "TemplatesTabControl";
			this.TemplatesTabControl.SelectedIndex = 0;
			this.TemplatesTabControl.Size = new System.Drawing.Size(406, 235);
			this.TemplatesTabControl.TabIndex = 18;
			this.TemplatesTabControl.SelectedIndexChanged += new System.EventHandler(this.TemplatesTabControl_SelectedIndexChanged);
			// 
			// AssignmentTemplateTabPage
			// 
			this.AssignmentTemplateTabPage.Controls.Add(this.AssignmentTemplateTextBox);
			this.AssignmentTemplateTabPage.Location = new System.Drawing.Point(4, 22);
			this.AssignmentTemplateTabPage.Name = "AssignmentTemplateTabPage";
			this.AssignmentTemplateTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.AssignmentTemplateTabPage.Size = new System.Drawing.Size(398, 209);
			this.AssignmentTemplateTabPage.TabIndex = 0;
			this.AssignmentTemplateTabPage.Text = "Board Assignment";
			this.AssignmentTemplateTabPage.UseVisualStyleBackColor = true;
			// 
			// AssignmentTemplateTextBox
			// 
			this.AssignmentTemplateTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AssignmentTemplateTextBox.Location = new System.Drawing.Point(0, 2);
			this.AssignmentTemplateTextBox.Multiline = true;
			this.AssignmentTemplateTextBox.Name = "AssignmentTemplateTextBox";
			this.AssignmentTemplateTextBox.Size = new System.Drawing.Size(396, 204);
			this.AssignmentTemplateTextBox.TabIndex = 0;
			this.AssignmentTemplateTextBox.TextChanged += new System.EventHandler(this.TemplateTextBox_TextChanged);
			// 
			// AnnouncementTemplateTabPage
			// 
			this.AnnouncementTemplateTabPage.Controls.Add(this.AnnouncementTemplateTextBox);
			this.AnnouncementTemplateTabPage.Location = new System.Drawing.Point(4, 22);
			this.AnnouncementTemplateTabPage.Name = "AnnouncementTemplateTabPage";
			this.AnnouncementTemplateTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.AnnouncementTemplateTabPage.Size = new System.Drawing.Size(398, 209);
			this.AnnouncementTemplateTabPage.TabIndex = 1;
			this.AnnouncementTemplateTabPage.Text = "Announcement";
			this.AnnouncementTemplateTabPage.UseVisualStyleBackColor = true;
			// 
			// AnnouncementTemplateTextBox
			// 
			this.AnnouncementTemplateTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AnnouncementTemplateTextBox.Location = new System.Drawing.Point(0, 2);
			this.AnnouncementTemplateTextBox.Multiline = true;
			this.AnnouncementTemplateTextBox.Name = "AnnouncementTemplateTextBox";
			this.AnnouncementTemplateTextBox.Size = new System.Drawing.Size(395, 204);
			this.AnnouncementTemplateTextBox.TabIndex = 1;
			this.AnnouncementTemplateTextBox.TextChanged += new System.EventHandler(this.TemplateTextBox_TextChanged);
			// 
			// TemplateLegendLabel
			// 
			this.TemplateLegendLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TemplateLegendLabel.Location = new System.Drawing.Point(671, 44);
			this.TemplateLegendLabel.Name = "TemplateLegendLabel";
			this.TemplateLegendLabel.Size = new System.Drawing.Size(125, 209);
			this.TemplateLegendLabel.TabIndex = 19;
			this.TemplateLegendLabel.Text = "Available\r\nMoustaches:\r\n\r\n{PlayerName}\r\n{PowerName}\r\n{GameNumber}\r\n{RoundNumber}";
			this.TemplateLegendLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SaveOrSendTemplateButton
			// 
			this.SaveOrSendTemplateButton.Location = new System.Drawing.Point(395, 268);
			this.SaveOrSendTemplateButton.Name = "SaveOrSendTemplateButton";
			this.SaveOrSendTemplateButton.Size = new System.Drawing.Size(155, 28);
			this.SaveOrSendTemplateButton.TabIndex = 21;
			this.SaveOrSendTemplateButton.Text = "Test-Send This Template";
			this.SaveOrSendTemplateButton.UseVisualStyleBackColor = true;
			this.SaveOrSendTemplateButton.Click += new System.EventHandler(this.SaveOrSendTemplateButton_Click);
			// 
			// EmailSettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(807, 311);
			this.Controls.Add(this.SaveOrSendTemplateButton);
			this.Controls.Add(this.TemplateLegendLabel);
			this.Controls.Add(this.TemplatesTabControl);
			this.Controls.Add(this.NonEventMailingsLabel);
			this.Controls.Add(this.FromNameTextBox);
			this.Controls.Add(this.FromNameLabel);
			this.Controls.Add(this.MailFromTextBox);
			this.Controls.Add(this.FromAddressLabel);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.OnlySendToTestCheckBox);
			this.Controls.Add(this.TestEmailTextBox);
			this.Controls.Add(this.TestEmailLabel);
			this.Controls.Add(this.PasswordTextBox);
			this.Controls.Add(this.PasswordLabel);
			this.Controls.Add(this.UsernameTextBox);
			this.Controls.Add(this.UsernameLabel);
			this.Controls.Add(this.UseSslCheckBox);
			this.Controls.Add(this.PortTextBox);
			this.Controls.Add(this.PortLabel);
			this.Controls.Add(this.HostTextBox);
			this.Controls.Add(this.HostLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EmailSettingsForm";
			this.ShowIcon = false;
			this.Text = "DCM Email Settings";
			this.Load += new System.EventHandler(this.EmailSettingsForm_Load);
			this.TemplatesTabControl.ResumeLayout(false);
			this.AssignmentTemplateTabPage.ResumeLayout(false);
			this.AssignmentTemplateTabPage.PerformLayout();
			this.AnnouncementTemplateTabPage.ResumeLayout(false);
			this.AnnouncementTemplateTabPage.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HostLabel;
        private System.Windows.Forms.TextBox HostTextBox;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.CheckBox UseSslCheckBox;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox TestEmailTextBox;
        private System.Windows.Forms.Label TestEmailLabel;
        private System.Windows.Forms.CheckBox OnlySendToTestCheckBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox MailFromTextBox;
        private System.Windows.Forms.Label FromAddressLabel;
        private System.Windows.Forms.TextBox FromNameTextBox;
        private System.Windows.Forms.Label FromNameLabel;
        private System.Windows.Forms.Label NonEventMailingsLabel;
		private System.Windows.Forms.TabControl TemplatesTabControl;
		private System.Windows.Forms.TabPage AssignmentTemplateTabPage;
		private System.Windows.Forms.TextBox AssignmentTemplateTextBox;
		private System.Windows.Forms.TabPage AnnouncementTemplateTabPage;
		private System.Windows.Forms.Label TemplateLegendLabel;
		private System.Windows.Forms.TextBox AnnouncementTemplateTextBox;
		private System.Windows.Forms.Button SaveOrSendTemplateButton;
	}
}
