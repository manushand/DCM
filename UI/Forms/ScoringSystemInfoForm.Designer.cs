namespace DCM.UI.Forms
{
	using Controls;

	internal partial class ScoringSystemInfoForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScoringSystemInfoForm));
			this.SystemNameLabel = new System.Windows.Forms.Label();
			this.NameTextBox = new System.Windows.Forms.TextBox();
			this.WinLossCheckBox = new System.Windows.Forms.CheckBox();
			this.DiasCheckBox = new System.Windows.Forms.CheckBox();
			this.CenterCountCheckBox = new System.Windows.Forms.CheckBox();
			this.YearsPlayedCheckBox = new System.Windows.Forms.CheckBox();
			this.FinalGameYearLabel = new System.Windows.Forms.Label();
			this.FinalGameYearComboBox = new System.Windows.Forms.ComboBox();
			this.FormulaTabs = new System.Windows.Forms.TabControl();
			this.FinalScoreFormulaTabPage = new System.Windows.Forms.TabPage();
			this.FinalScoreFormulaTextBox = new System.Windows.Forms.RichTextBox();
			this.ProvisionalScoreTabPage = new System.Windows.Forms.TabPage();
			this.ProvisionalScoreFormulaTextBox = new System.Windows.Forms.RichTextBox();
			this.PlayerAnteFormulaTabPage = new System.Windows.Forms.TabPage();
			this.PlayerAnteFormulaTextBox = new System.Windows.Forms.RichTextBox();
			this.TestGameGroupBox = new System.Windows.Forms.GroupBox();
			this.NewTestButton = new System.Windows.Forms.Button();
			this.RunTestButton = new System.Windows.Forms.Button();
			this.CancelFormButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.SignificantDigitsLabel = new System.Windows.Forms.Label();
			this.SignificantDigitsComboBox = new System.Windows.Forms.ComboBox();
			this.CopyButton = new System.Windows.Forms.Button();
			this.AllowDrawsCheckBox = new System.Windows.Forms.CheckBox();
			this.FormulaTypeLabel = new System.Windows.Forms.Label();
			this.FormulaTypeComboBox = new System.Windows.Forms.ComboBox();
			this.TotalPointsFixedCheckBox = new System.Windows.Forms.CheckBox();
			this.PointsPerGameTextBox = new System.Windows.Forms.TextBox();
			this.PointsPerGameLabel = new System.Windows.Forms.Label();
			this.OtherScoringCheckBox = new System.Windows.Forms.CheckBox();
			this.OtherAliasLabel = new System.Windows.Forms.Label();
			this.OtherAliasTextBox = new System.Windows.Forms.TextBox();
			this.UsesProvisionalScoreCheckBox = new System.Windows.Forms.CheckBox();
			this.UsesPlayerAnteCheckBox = new System.Windows.Forms.CheckBox();
			this.GameControl = new GameControl();
			this.FormulaTabs.SuspendLayout();
			this.FinalScoreFormulaTabPage.SuspendLayout();
			this.ProvisionalScoreTabPage.SuspendLayout();
			this.PlayerAnteFormulaTabPage.SuspendLayout();
			this.TestGameGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// SystemNameLabel
			// 
			this.SystemNameLabel.AutoSize = true;
			this.SystemNameLabel.Location = new System.Drawing.Point(22, 13);
			this.SystemNameLabel.Name = "SystemNameLabel";
			this.SystemNameLabel.Size = new System.Drawing.Size(38, 13);
			this.SystemNameLabel.TabIndex = 0;
			this.SystemNameLabel.Text = "Name:";
			// 
			// NameTextBox
			// 
			this.NameTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NameTextBox.Location = new System.Drawing.Point(61, 10);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(200, 22);
			this.NameTextBox.TabIndex = 1;
			// 
			// WinLossCheckBox
			// 
			this.WinLossCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.WinLossCheckBox.Location = new System.Drawing.Point(586, 61);
			this.WinLossCheckBox.Name = "WinLossCheckBox";
			this.WinLossCheckBox.Size = new System.Drawing.Size(133, 17);
			this.WinLossCheckBox.TabIndex = 3;
			this.WinLossCheckBox.Text = "Game Result Scoring? ";
			this.WinLossCheckBox.UseVisualStyleBackColor = true;
			this.WinLossCheckBox.CheckedChanged += new System.EventHandler(this.WinLossCheckBox_CheckedChanged);
			// 
			// DiasCheckBox
			// 
			this.DiasCheckBox.AutoSize = true;
			this.DiasCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.DiasCheckBox.Location = new System.Drawing.Point(906, 61);
			this.DiasCheckBox.Name = "DiasCheckBox";
			this.DiasCheckBox.Size = new System.Drawing.Size(57, 17);
			this.DiasCheckBox.TabIndex = 4;
			this.DiasCheckBox.Text = "DIAS?";
			this.DiasCheckBox.UseVisualStyleBackColor = true;
			this.DiasCheckBox.CheckedChanged += new System.EventHandler(this.DiasCheckBox_CheckedChanged);
			// 
			// CenterCountCheckBox
			// 
			this.CenterCountCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CenterCountCheckBox.Location = new System.Drawing.Point(586, 36);
			this.CenterCountCheckBox.Name = "CenterCountCheckBox";
			this.CenterCountCheckBox.Size = new System.Drawing.Size(133, 17);
			this.CenterCountCheckBox.TabIndex = 5;
			this.CenterCountCheckBox.Text = "Center Count Scoring? \r\n";
			this.CenterCountCheckBox.UseVisualStyleBackColor = true;
			this.CenterCountCheckBox.CheckedChanged += new System.EventHandler(this.CenterCountCheckBox_CheckedChanged);
			// 
			// YearsPlayedCheckBox
			// 
			this.YearsPlayedCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.YearsPlayedCheckBox.Location = new System.Drawing.Point(586, 86);
			this.YearsPlayedCheckBox.Name = "YearsPlayedCheckBox";
			this.YearsPlayedCheckBox.Size = new System.Drawing.Size(133, 17);
			this.YearsPlayedCheckBox.TabIndex = 6;
			this.YearsPlayedCheckBox.Text = "Years Played Scoring?";
			this.YearsPlayedCheckBox.UseVisualStyleBackColor = true;
			this.YearsPlayedCheckBox.CheckedChanged += new System.EventHandler(this.YearsPlayedCheckBox_CheckedChanged);
			// 
			// FinalGameYearLabel
			// 
			this.FinalGameYearLabel.AutoSize = true;
			this.FinalGameYearLabel.Location = new System.Drawing.Point(800, 87);
			this.FinalGameYearLabel.Name = "FinalGameYearLabel";
			this.FinalGameYearLabel.Size = new System.Drawing.Size(88, 13);
			this.FinalGameYearLabel.TabIndex = 7;
			this.FinalGameYearLabel.Text = "Final Game Year:";
			// 
			// FinalGameYearComboBox
			// 
			this.FinalGameYearComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FinalGameYearComboBox.FormattingEnabled = true;
			this.FinalGameYearComboBox.Location = new System.Drawing.Point(890, 84);
			this.FinalGameYearComboBox.Name = "FinalGameYearComboBox";
			this.FinalGameYearComboBox.Size = new System.Drawing.Size(73, 21);
			this.FinalGameYearComboBox.TabIndex = 8;
			this.FinalGameYearComboBox.SelectedIndexChanged += new System.EventHandler(this.FinalGameYearComboBox_SelectedIndexChanged);
			// 
			// FormulaTabs
			// 
			this.FormulaTabs.Controls.Add(this.FinalScoreFormulaTabPage);
			this.FormulaTabs.Controls.Add(this.ProvisionalScoreTabPage);
			this.FormulaTabs.Controls.Add(this.PlayerAnteFormulaTabPage);
			this.FormulaTabs.Location = new System.Drawing.Point(16, 38);
			this.FormulaTabs.Name = "FormulaTabs";
			this.FormulaTabs.SelectedIndex = 0;
			this.FormulaTabs.Size = new System.Drawing.Size(557, 393);
			this.FormulaTabs.TabIndex = 9;
			// 
			// FinalScoreFormulaTabPage
			// 
			this.FinalScoreFormulaTabPage.Controls.Add(this.FinalScoreFormulaTextBox);
			this.FinalScoreFormulaTabPage.Location = new System.Drawing.Point(4, 22);
			this.FinalScoreFormulaTabPage.Name = "FinalScoreFormulaTabPage";
			this.FinalScoreFormulaTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.FinalScoreFormulaTabPage.Size = new System.Drawing.Size(549, 367);
			this.FinalScoreFormulaTabPage.TabIndex = 0;
			this.FinalScoreFormulaTabPage.Text = "Final Score Formula";
			this.FinalScoreFormulaTabPage.UseVisualStyleBackColor = true;
			// 
			// FinalScoreFormulaTextBox
			// 
			this.FinalScoreFormulaTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FinalScoreFormulaTextBox.Location = new System.Drawing.Point(0, 0);
			this.FinalScoreFormulaTextBox.Name = "FinalScoreFormulaTextBox";
			this.FinalScoreFormulaTextBox.Size = new System.Drawing.Size(549, 367);
			this.FinalScoreFormulaTextBox.TabIndex = 0;
			this.FinalScoreFormulaTextBox.Text = "";
			// 
			// ProvisionalScoreTabPage
			// 
			this.ProvisionalScoreTabPage.Controls.Add(this.ProvisionalScoreFormulaTextBox);
			this.ProvisionalScoreTabPage.Location = new System.Drawing.Point(4, 22);
			this.ProvisionalScoreTabPage.Name = "ProvisionalScoreTabPage";
			this.ProvisionalScoreTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.ProvisionalScoreTabPage.Size = new System.Drawing.Size(549, 367);
			this.ProvisionalScoreTabPage.TabIndex = 1;
			this.ProvisionalScoreTabPage.Text = "Provisional Score Formula";
			this.ProvisionalScoreTabPage.UseVisualStyleBackColor = true;
			// 
			// ProvisionalScoreFormulaTextBox
			// 
			this.ProvisionalScoreFormulaTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ProvisionalScoreFormulaTextBox.Location = new System.Drawing.Point(0, 1);
			this.ProvisionalScoreFormulaTextBox.Name = "ProvisionalScoreFormulaTextBox";
			this.ProvisionalScoreFormulaTextBox.Size = new System.Drawing.Size(549, 370);
			this.ProvisionalScoreFormulaTextBox.TabIndex = 0;
			this.ProvisionalScoreFormulaTextBox.Text = "";
			// 
			// PlayerAnteFormulaTabPage
			// 
			this.PlayerAnteFormulaTabPage.Controls.Add(this.PlayerAnteFormulaTextBox);
			this.PlayerAnteFormulaTabPage.Location = new System.Drawing.Point(4, 22);
			this.PlayerAnteFormulaTabPage.Name = "PlayerAnteFormulaTabPage";
			this.PlayerAnteFormulaTabPage.Size = new System.Drawing.Size(549, 367);
			this.PlayerAnteFormulaTabPage.TabIndex = 2;
			this.PlayerAnteFormulaTabPage.Text = "Player Ante Formula";
			this.PlayerAnteFormulaTabPage.UseVisualStyleBackColor = true;
			// 
			// PlayerAnteFormulaTextBox
			// 
			this.PlayerAnteFormulaTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PlayerAnteFormulaTextBox.Location = new System.Drawing.Point(0, 0);
			this.PlayerAnteFormulaTextBox.Name = "PlayerAnteFormulaTextBox";
			this.PlayerAnteFormulaTextBox.Size = new System.Drawing.Size(549, 367);
			this.PlayerAnteFormulaTextBox.TabIndex = 1;
			this.PlayerAnteFormulaTextBox.Text = "";
			// 
			// TestGameGroupBox
			// 
			this.TestGameGroupBox.Controls.Add(this.NewTestButton);
			this.TestGameGroupBox.Controls.Add(this.RunTestButton);
			this.TestGameGroupBox.Controls.Add(this.GameControl);
			this.TestGameGroupBox.Location = new System.Drawing.Point(579, 138);
			this.TestGameGroupBox.Name = "TestGameGroupBox";
			this.TestGameGroupBox.Size = new System.Drawing.Size(396, 293);
			this.TestGameGroupBox.TabIndex = 11;
			this.TestGameGroupBox.TabStop = false;
			this.TestGameGroupBox.Text = "Test Game";
			// 
			// NewTestButton
			// 
			this.NewTestButton.Location = new System.Drawing.Point(84, 256);
			this.NewTestButton.Name = "NewTestButton";
			this.NewTestButton.Size = new System.Drawing.Size(113, 23);
			this.NewTestButton.TabIndex = 79;
			this.NewTestButton.Text = "New Random Game";
			this.NewTestButton.UseVisualStyleBackColor = true;
			this.NewTestButton.Click += new System.EventHandler(this.NewTestButton_Click);
			// 
			// RunTestButton
			// 
			this.RunTestButton.Location = new System.Drawing.Point(201, 256);
			this.RunTestButton.Name = "RunTestButton";
			this.RunTestButton.Size = new System.Drawing.Size(113, 23);
			this.RunTestButton.TabIndex = 78;
			this.RunTestButton.Text = "Score This Game";
			this.RunTestButton.UseVisualStyleBackColor = true;
			this.RunTestButton.Click += new System.EventHandler(this.RunTestButton_Click);
			// 
			// GameControl
			// 
			this.GameControl.Location = new System.Drawing.Point(7, 14);
			this.GameControl.Margin = new System.Windows.Forms.Padding(0);
			this.GameControl.Name = "GameControl";
			this.GameControl.Size = new System.Drawing.Size(380, 268);
			this.GameControl.TabIndex = 0;
			// 
			// CancelFormButton
			// 
			this.CancelFormButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelFormButton.Location = new System.Drawing.Point(447, 446);
			this.CancelFormButton.Name = "CancelFormButton";
			this.CancelFormButton.Size = new System.Drawing.Size(125, 23);
			this.CancelFormButton.TabIndex = 37;
			this.CancelFormButton.Text = "Cancel";
			this.CancelFormButton.UseVisualStyleBackColor = true;
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(312, 446);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(125, 23);
			this.OkButton.TabIndex = 36;
			this.OkButton.Text = " OK";
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
			// 
			// SignificantDigitsLabel
			// 
			this.SignificantDigitsLabel.AutoSize = true;
			this.SignificantDigitsLabel.Location = new System.Drawing.Point(751, 37);
			this.SignificantDigitsLabel.Name = "SignificantDigitsLabel";
			this.SignificantDigitsLabel.Size = new System.Drawing.Size(166, 13);
			this.SignificantDigitsLabel.TabIndex = 41;
			this.SignificantDigitsLabel.Text = "Significant Digits in Game Scores:";
			// 
			// SignificantDigitsComboBox
			// 
			this.SignificantDigitsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SignificantDigitsComboBox.FormattingEnabled = true;
			this.SignificantDigitsComboBox.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
			this.SignificantDigitsComboBox.Location = new System.Drawing.Point(920, 34);
			this.SignificantDigitsComboBox.Name = "SignificantDigitsComboBox";
			this.SignificantDigitsComboBox.Size = new System.Drawing.Size(43, 21);
			this.SignificantDigitsComboBox.TabIndex = 42;
			this.SignificantDigitsComboBox.SelectedIndexChanged += new System.EventHandler(this.SignificantDigitsComboBox_SelectedIndexChanged);
			// 
			// CopyButton
			// 
			this.CopyButton.Location = new System.Drawing.Point(267, 10);
			this.CopyButton.Name = "CopyButton";
			this.CopyButton.Size = new System.Drawing.Size(47, 22);
			this.CopyButton.TabIndex = 43;
			this.CopyButton.Text = "Copy";
			this.CopyButton.UseVisualStyleBackColor = true;
			this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
			// 
			// AllowDrawsCheckBox
			// 
			this.AllowDrawsCheckBox.AutoSize = true;
			this.AllowDrawsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.AllowDrawsCheckBox.Location = new System.Drawing.Point(800, 61);
			this.AllowDrawsCheckBox.Name = "AllowDrawsCheckBox";
			this.AllowDrawsCheckBox.Size = new System.Drawing.Size(90, 17);
			this.AllowDrawsCheckBox.TabIndex = 44;
			this.AllowDrawsCheckBox.Text = "Allow Draws?";
			this.AllowDrawsCheckBox.UseVisualStyleBackColor = true;
			this.AllowDrawsCheckBox.CheckedChanged += new System.EventHandler(this.AllowDrawsCheckBox_CheckedChanged);
			// 
			// FormulaTypeLabel
			// 
			this.FormulaTypeLabel.AutoSize = true;
			this.FormulaTypeLabel.Location = new System.Drawing.Point(400, 36);
			this.FormulaTypeLabel.Name = "FormulaTypeLabel";
			this.FormulaTypeLabel.Size = new System.Drawing.Size(98, 13);
			this.FormulaTypeLabel.TabIndex = 45;
			this.FormulaTypeLabel.Text = "Formula Language:";
			// 
			// FormulaTypeComboBox
			// 
			this.FormulaTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FormulaTypeComboBox.FormattingEnabled = true;
			this.FormulaTypeComboBox.Items.AddRange(new object[] {
            "DCM",
            "C#"});
			this.FormulaTypeComboBox.Location = new System.Drawing.Point(500, 34);
			this.FormulaTypeComboBox.Name = "FormulaTypeComboBox";
			this.FormulaTypeComboBox.Size = new System.Drawing.Size(67, 21);
			this.FormulaTypeComboBox.TabIndex = 46;
			// 
			// TotalPointsFixedCheckBox
			// 
			this.TotalPointsFixedCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.TotalPointsFixedCheckBox.Location = new System.Drawing.Point(586, 11);
			this.TotalPointsFixedCheckBox.Margin = new System.Windows.Forms.Padding(1);
			this.TotalPointsFixedCheckBox.Name = "TotalPointsFixedCheckBox";
			this.TotalPointsFixedCheckBox.Size = new System.Drawing.Size(133, 17);
			this.TotalPointsFixedCheckBox.TabIndex = 48;
			this.TotalPointsFixedCheckBox.Text = "Fixed Total Points?";
			this.TotalPointsFixedCheckBox.UseVisualStyleBackColor = true;
			this.TotalPointsFixedCheckBox.CheckedChanged += new System.EventHandler(this.TotalPointsFixedCheckBox_CheckedChanged);
			// 
			// PointsPerGameTextBox
			// 
			this.PointsPerGameTextBox.Location = new System.Drawing.Point(920, 9);
			this.PointsPerGameTextBox.Margin = new System.Windows.Forms.Padding(1);
			this.PointsPerGameTextBox.Name = "PointsPerGameTextBox";
			this.PointsPerGameTextBox.Size = new System.Drawing.Size(43, 20);
			this.PointsPerGameTextBox.TabIndex = 49;
			// 
			// PointsPerGameLabel
			// 
			this.PointsPerGameLabel.AutoSize = true;
			this.PointsPerGameLabel.Location = new System.Drawing.Point(751, 13);
			this.PointsPerGameLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this.PointsPerGameLabel.Name = "PointsPerGameLabel";
			this.PointsPerGameLabel.Size = new System.Drawing.Size(160, 13);
			this.PointsPerGameLabel.TabIndex = 50;
			this.PointsPerGameLabel.Text = "Total Points Awarded per Game:";
			// 
			// OtherScoringCheckBox
			// 
			this.OtherScoringCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.OtherScoringCheckBox.Location = new System.Drawing.Point(586, 111);
			this.OtherScoringCheckBox.Name = "OtherScoringCheckBox";
			this.OtherScoringCheckBox.Size = new System.Drawing.Size(133, 17);
			this.OtherScoringCheckBox.TabIndex = 51;
			this.OtherScoringCheckBox.Text = "Other Points Scoring?";
			this.OtherScoringCheckBox.UseVisualStyleBackColor = true;
			this.OtherScoringCheckBox.CheckedChanged += new System.EventHandler(this.OtherScoringCheckBox_CheckedChanged);
			// 
			// OtherAliasLabel
			// 
			this.OtherAliasLabel.AutoSize = true;
			this.OtherAliasLabel.Location = new System.Drawing.Point(752, 112);
			this.OtherAliasLabel.Name = "OtherAliasLabel";
			this.OtherAliasLabel.Size = new System.Drawing.Size(107, 13);
			this.OtherAliasLabel.TabIndex = 52;
			this.OtherAliasLabel.Text = "Alias for Other Score:";
			// 
			// OtherAliasTextBox
			// 
			this.OtherAliasTextBox.Location = new System.Drawing.Point(864, 109);
			this.OtherAliasTextBox.Name = "OtherAliasTextBox";
			this.OtherAliasTextBox.Size = new System.Drawing.Size(99, 20);
			this.OtherAliasTextBox.TabIndex = 53;
			this.OtherAliasTextBox.TextChanged += new System.EventHandler(this.OtherAliasTextBox_TextChanged);
			// 
			// UsesProvisionalScoreCheckBox
			// 
			this.UsesProvisionalScoreCheckBox.AutoSize = true;
			this.UsesProvisionalScoreCheckBox.Location = new System.Drawing.Point(324, 12);
			this.UsesProvisionalScoreCheckBox.Name = "UsesProvisionalScoreCheckBox";
			this.UsesProvisionalScoreCheckBox.Size = new System.Drawing.Size(135, 17);
			this.UsesProvisionalScoreCheckBox.TabIndex = 54;
			this.UsesProvisionalScoreCheckBox.Text = "Uses Provisional Score";
			this.UsesProvisionalScoreCheckBox.UseVisualStyleBackColor = true;
			this.UsesProvisionalScoreCheckBox.CheckedChanged += new System.EventHandler(this.UsesProvisionalScoreCheckBox_CheckedChanged);
			// 
			// UsesPlayerAnteCheckBox
			// 
			this.UsesPlayerAnteCheckBox.AutoSize = true;
			this.UsesPlayerAnteCheckBox.Location = new System.Drawing.Point(466, 12);
			this.UsesPlayerAnteCheckBox.Name = "UsesPlayerAnteCheckBox";
			this.UsesPlayerAnteCheckBox.Size = new System.Drawing.Size(107, 17);
			this.UsesPlayerAnteCheckBox.TabIndex = 55;
			this.UsesPlayerAnteCheckBox.Text = "Uses Player Ante";
			this.UsesPlayerAnteCheckBox.UseVisualStyleBackColor = true;
			this.UsesPlayerAnteCheckBox.CheckedChanged += new System.EventHandler(this.UsesPlayerAnteCheckBox_CheckedChanged);
			// 
			// ScoringSystemInfoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelFormButton;
			this.ClientSize = new System.Drawing.Size(993, 485);
			this.Controls.Add(this.UsesPlayerAnteCheckBox);
			this.Controls.Add(this.UsesProvisionalScoreCheckBox);
			this.Controls.Add(this.OtherAliasTextBox);
			this.Controls.Add(this.OtherAliasLabel);
			this.Controls.Add(this.OtherScoringCheckBox);
			this.Controls.Add(this.PointsPerGameLabel);
			this.Controls.Add(this.PointsPerGameTextBox);
			this.Controls.Add(this.TotalPointsFixedCheckBox);
			this.Controls.Add(this.FormulaTypeComboBox);
			this.Controls.Add(this.FormulaTypeLabel);
			this.Controls.Add(this.AllowDrawsCheckBox);
			this.Controls.Add(this.CopyButton);
			this.Controls.Add(this.SignificantDigitsComboBox);
			this.Controls.Add(this.SignificantDigitsLabel);
			this.Controls.Add(this.CancelFormButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.TestGameGroupBox);
			this.Controls.Add(this.FormulaTabs);
			this.Controls.Add(this.FinalGameYearComboBox);
			this.Controls.Add(this.FinalGameYearLabel);
			this.Controls.Add(this.YearsPlayedCheckBox);
			this.Controls.Add(this.CenterCountCheckBox);
			this.Controls.Add(this.DiasCheckBox);
			this.Controls.Add(this.WinLossCheckBox);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.SystemNameLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScoringSystemInfoForm";
			this.ShowIcon = false;
			this.Text = "Scoring System Details";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScoringSystemDetailsForm_FormClosing);
			this.Load += new System.EventHandler(this.ScoringSystemInfoForm_Load);
			this.FormulaTabs.ResumeLayout(false);
			this.FinalScoreFormulaTabPage.ResumeLayout(false);
			this.ProvisionalScoreTabPage.ResumeLayout(false);
			this.PlayerAnteFormulaTabPage.ResumeLayout(false);
			this.TestGameGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label SystemNameLabel;
		private TextBox NameTextBox;
		private CheckBox WinLossCheckBox;
		private CheckBox DiasCheckBox;
		private CheckBox CenterCountCheckBox;
		private CheckBox YearsPlayedCheckBox;
		private Label FinalGameYearLabel;
		private ComboBox FinalGameYearComboBox;
		private TabControl FormulaTabs;
		private TabPage FinalScoreFormulaTabPage;
		private TabPage ProvisionalScoreTabPage;
		private GroupBox TestGameGroupBox;
		private Button CancelFormButton;
		private Button OkButton;
		private Label SignificantDigitsLabel;
		private ComboBox SignificantDigitsComboBox;
		private Button CopyButton;
		private CheckBox AllowDrawsCheckBox;
		private Label FormulaTypeLabel;
		private ComboBox FormulaTypeComboBox;
		private RichTextBox FinalScoreFormulaTextBox;
		private RichTextBox ProvisionalScoreFormulaTextBox;
		private GameControl GameControl;
		private Button RunTestButton;
		private Button NewTestButton;
		private CheckBox TotalPointsFixedCheckBox;
		private TextBox PointsPerGameTextBox;
		private Label PointsPerGameLabel;
		private CheckBox OtherScoringCheckBox;
		private Label OtherAliasLabel;
		private TextBox OtherAliasTextBox;
		private TabPage PlayerAnteFormulaTabPage;
		private RichTextBox PlayerAnteFormulaTextBox;
		private CheckBox UsesProvisionalScoreCheckBox;
		private CheckBox UsesPlayerAnteCheckBox;
	}
}
