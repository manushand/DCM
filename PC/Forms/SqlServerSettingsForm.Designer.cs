namespace PC.Forms;

partial class SqlServerSettingsForm
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
		if (disposing && (components != null))
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
		ServerNameLabel = new Label();
		ServerNameTextBox = new TextBox();
		DatabaseLabel = new Label();
		DatabaseTextBox = new TextBox();
		UsernameLabel = new Label();
		UsernameTextBox = new TextBox();
		PasswordLabel = new Label();
		PasswordTextBox = new TextBox();
		ConnectButton = new Button();
		SuspendLayout();
		// 
		// ServerNameLabel
		// 
		ServerNameLabel.Location = new Point(17, 24);
		ServerNameLabel.Name = "ServerNameLabel";
		ServerNameLabel.Size = new Size(82, 19);
		ServerNameLabel.TabIndex = 0;
		ServerNameLabel.Text = "Server Name:";
		ServerNameLabel.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// ServerNameTextBox
		// 
		ServerNameTextBox.Location = new Point(105, 20);
		ServerNameTextBox.Name = "ServerNameTextBox";
		ServerNameTextBox.Size = new Size(286, 23);
		ServerNameTextBox.TabIndex = 1;
		// 
		// DatabaseLabel
		// 
		DatabaseLabel.Location = new Point(222, 48);
		DatabaseLabel.Name = "DatabaseLabel";
		DatabaseLabel.Size = new Size(63, 23);
		DatabaseLabel.TabIndex = 2;
		DatabaseLabel.Text = "Database:";
		DatabaseLabel.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// DatabaseTextBox
		// 
		DatabaseTextBox.Location = new Point(291, 49);
		DatabaseTextBox.Name = "DatabaseTextBox";
		DatabaseTextBox.Size = new Size(100, 23);
		DatabaseTextBox.TabIndex = 7;
		// 
		// UsernameLabel
		// 
		UsernameLabel.Location = new Point(19, 49);
		UsernameLabel.Name = "UsernameLabel";
		UsernameLabel.Size = new Size(80, 20);
		UsernameLabel.TabIndex = 4;
		UsernameLabel.Text = "Username:";
		UsernameLabel.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// UsernameTextBox
		// 
		UsernameTextBox.Location = new Point(105, 49);
		UsernameTextBox.Name = "UsernameTextBox";
		UsernameTextBox.Size = new Size(100, 23);
		UsernameTextBox.TabIndex = 3;
		// 
		// PasswordLabel
		// 
		PasswordLabel.Location = new Point(19, 81);
		PasswordLabel.Name = "PasswordLabel";
		PasswordLabel.Size = new Size(80, 23);
		PasswordLabel.TabIndex = 6;
		PasswordLabel.Text = "Password:";
		PasswordLabel.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// PasswordTextBox
		// 
		PasswordTextBox.Location = new Point(105, 81);
		PasswordTextBox.Name = "PasswordTextBox";
		PasswordTextBox.Size = new Size(100, 23);
		PasswordTextBox.TabIndex = 5;
		// 
		// ConnectButton
		// 
		ConnectButton.Location = new Point(262, 81);
		ConnectButton.Name = "ConnectButton";
		ConnectButton.Size = new Size(129, 23);
		ConnectButton.TabIndex = 8;
		ConnectButton.Text = "Connect";
		ConnectButton.UseVisualStyleBackColor = true;
		ConnectButton.Click += ConnectButton_Click;
		// 
		// SqlServerSettingsForm
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(422, 121);
		Controls.Add(ConnectButton);
		Controls.Add(PasswordTextBox);
		Controls.Add(PasswordLabel);
		Controls.Add(UsernameTextBox);
		Controls.Add(UsernameLabel);
		Controls.Add(DatabaseTextBox);
		Controls.Add(DatabaseLabel);
		Controls.Add(ServerNameTextBox);
		Controls.Add(ServerNameLabel);
		Name = "SqlServerSettingsForm";
		ShowIcon = false;
		Text = "SQL Server Settings";
		Load += SqlServerSettingsForm_Load;
		ResumeLayout(false);
		PerformLayout();
	}

	private System.Windows.Forms.Button ConnectButton;

	private System.Windows.Forms.Label UsernameLabel;
	private System.Windows.Forms.TextBox UsernameTextBox;
	private System.Windows.Forms.Label PasswordLabel;
	private System.Windows.Forms.TextBox PasswordTextBox;

	private System.Windows.Forms.Label DatabaseLabel;
	private System.Windows.Forms.TextBox DatabaseTextBox;

	private System.Windows.Forms.TextBox ServerNameTextBox;

	private System.Windows.Forms.Label ServerNameLabel;

	#endregion
}
