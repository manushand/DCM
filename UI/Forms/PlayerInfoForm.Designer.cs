namespace DCM.UI.Forms
{
	partial class PlayerInfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerInfoForm));
			this.LastNameTextBox = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.FirstNameLabel = new System.Windows.Forms.Label();
			this.EmailAddressTextBox = new System.Windows.Forms.TextBox();
			this.LastNameLabel = new System.Windows.Forms.Label();
			this.EmailLabel = new System.Windows.Forms.Label();
			this.FirstNameTextBox = new System.Windows.Forms.TextBox();
			this.CancelFormButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// LastNameTextBox
			// 
			this.LastNameTextBox.Location = new System.Drawing.Point(74, 35);
			this.LastNameTextBox.Name = "LastNameTextBox";
			this.LastNameTextBox.Size = new System.Drawing.Size(212, 20);
			this.LastNameTextBox.TabIndex = 12;
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(36, 88);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(116, 30);
			this.OkButton.TabIndex = 14;
			this.OkButton.Text = "OK";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// FirstNameLabel
			// 
			this.FirstNameLabel.AutoSize = true;
			this.FirstNameLabel.Location = new System.Drawing.Point(8, 12);
			this.FirstNameLabel.Name = "FirstNameLabel";
			this.FirstNameLabel.Size = new System.Drawing.Size(60, 13);
			this.FirstNameLabel.TabIndex = 8;
			this.FirstNameLabel.Text = "First Name:";
			// 
			// EmailAddressTextBox
			// 
			this.EmailAddressTextBox.Location = new System.Drawing.Point(74, 62);
			this.EmailAddressTextBox.Name = "EmailAddressTextBox";
			this.EmailAddressTextBox.Size = new System.Drawing.Size(212, 20);
			this.EmailAddressTextBox.TabIndex = 13;
			// 
			// LastNameLabel
			// 
			this.LastNameLabel.AutoSize = true;
			this.LastNameLabel.Location = new System.Drawing.Point(8, 38);
			this.LastNameLabel.Name = "LastNameLabel";
			this.LastNameLabel.Size = new System.Drawing.Size(61, 13);
			this.LastNameLabel.TabIndex = 9;
			this.LastNameLabel.Text = "Last Name:";
			// 
			// EmailLabel
			// 
			this.EmailLabel.AutoSize = true;
			this.EmailLabel.Location = new System.Drawing.Point(8, 65);
			this.EmailLabel.Name = "EmailLabel";
			this.EmailLabel.Size = new System.Drawing.Size(35, 13);
			this.EmailLabel.TabIndex = 10;
			this.EmailLabel.Text = "Email:";
			// 
			// FirstNameTextBox
			// 
			this.FirstNameTextBox.Location = new System.Drawing.Point(74, 9);
			this.FirstNameTextBox.Name = "FirstNameTextBox";
			this.FirstNameTextBox.Size = new System.Drawing.Size(212, 20);
			this.FirstNameTextBox.TabIndex = 11;
			// 
			// CancelFormButton
			// 
			this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelFormButton.Location = new System.Drawing.Point(158, 88);
			this.CancelFormButton.Name = "CancelFormButton";
			this.CancelFormButton.Size = new System.Drawing.Size(116, 30);
			this.CancelFormButton.TabIndex = 15;
			this.CancelFormButton.Text = "Cancel";
			this.CancelFormButton.UseVisualStyleBackColor = true;
			// 
			// PlayerInfoForm
			// 
			this.AcceptButton = this.OkButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelFormButton;
			this.ClientSize = new System.Drawing.Size(301, 123);
			this.Controls.Add(this.CancelFormButton);
			this.Controls.Add(this.LastNameTextBox);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.FirstNameLabel);
			this.Controls.Add(this.EmailAddressTextBox);
			this.Controls.Add(this.LastNameLabel);
			this.Controls.Add(this.EmailLabel);
			this.Controls.Add(this.FirstNameTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PlayerInfoForm";
			this.ShowIcon = false;
			this.Text = "Player Details";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TextBox LastNameTextBox;
		private Button OkButton;
		private Label FirstNameLabel;
		private TextBox EmailAddressTextBox;
		private Label LastNameLabel;
		private Label EmailLabel;
		private TextBox FirstNameTextBox;
		private Button CancelFormButton;
	}
}
