namespace DCM.UI.Controls
{
	partial class GameControl
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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PowerColumnHeaderLabel = new System.Windows.Forms.Label();
			this.CentersPanel = new System.Windows.Forms.Panel();
			this.CentersHeaderLabel = new System.Windows.Forms.Label();
			this.TurkeyCentersComboBox = new System.Windows.Forms.ComboBox();
			this.RussiaCentersComboBox = new System.Windows.Forms.ComboBox();
			this.ItalyCentersComboBox = new System.Windows.Forms.ComboBox();
			this.GermanyCentersComboBox = new System.Windows.Forms.ComboBox();
			this.FranceCentersComboBox = new System.Windows.Forms.ComboBox();
			this.EnglandCentersComboBox = new System.Windows.Forms.ComboBox();
			this.AustriaCentersComboBox = new System.Windows.Forms.ComboBox();
			this.ResultPanel = new System.Windows.Forms.Panel();
			this.ResultHeaderLabel = new System.Windows.Forms.Label();
			this.TurkeyResultComboBox = new System.Windows.Forms.ComboBox();
			this.RussiaResultComboBox = new System.Windows.Forms.ComboBox();
			this.ItalyResultComboBox = new System.Windows.Forms.ComboBox();
			this.GermanyResultComboBox = new System.Windows.Forms.ComboBox();
			this.AustriaResultComboBox = new System.Windows.Forms.ComboBox();
			this.EnglandResultComboBox = new System.Windows.Forms.ComboBox();
			this.FranceResultComboBox = new System.Windows.Forms.ComboBox();
			this.YearsPanel = new System.Windows.Forms.Panel();
			this.YearsHeaderLabel = new System.Windows.Forms.Label();
			this.TurkeyYearsComboBox = new System.Windows.Forms.ComboBox();
			this.RussiaYearsComboBox = new System.Windows.Forms.ComboBox();
			this.ItalyYearsComboBox = new System.Windows.Forms.ComboBox();
			this.GermanyYearsComboBox = new System.Windows.Forms.ComboBox();
			this.FranceYearsComboBox = new System.Windows.Forms.ComboBox();
			this.EnglandYearsComboBox = new System.Windows.Forms.ComboBox();
			this.AustriaYearsComboBox = new System.Windows.Forms.ComboBox();
			this.SoloConcededCheckBox = new System.Windows.Forms.CheckBox();
			this.AustriaLabel = new System.Windows.Forms.Label();
			this.EnglandLabel = new System.Windows.Forms.Label();
			this.FranceLabel = new System.Windows.Forms.Label();
			this.GermanyLabel = new System.Windows.Forms.Label();
			this.ItalyLabel = new System.Windows.Forms.Label();
			this.RussiaLabel = new System.Windows.Forms.Label();
			this.TurkeyLabel = new System.Windows.Forms.Label();
			this.OtherPanel = new System.Windows.Forms.Panel();
			this.TurkeyOtherTextBox = new System.Windows.Forms.TextBox();
			this.RussiaOtherTextBox = new System.Windows.Forms.TextBox();
			this.ItalyOtherTextBox = new System.Windows.Forms.TextBox();
			this.GermanyOtherTextBox = new System.Windows.Forms.TextBox();
			this.FranceOtherTextBox = new System.Windows.Forms.TextBox();
			this.EnglandOtherTextBox = new System.Windows.Forms.TextBox();
			this.AustriaOtherTextBox = new System.Windows.Forms.TextBox();
			this.OtherScoreLabel = new System.Windows.Forms.Label();
			this.CentersPanel.SuspendLayout();
			this.ResultPanel.SuspendLayout();
			this.YearsPanel.SuspendLayout();
			this.OtherPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// PowerColumnHeaderLabel
			//
			this.PowerColumnHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PowerColumnHeaderLabel.Location = new System.Drawing.Point(3, 28);
			this.PowerColumnHeaderLabel.Name = "PowerColumnHeaderLabel";
			this.PowerColumnHeaderLabel.Size = new System.Drawing.Size(69, 16);
			this.PowerColumnHeaderLabel.TabIndex = 84;
			this.PowerColumnHeaderLabel.Text = "POWER";
			this.PowerColumnHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// CentersPanel
			//
			this.CentersPanel.Controls.Add(this.CentersHeaderLabel);
			this.CentersPanel.Controls.Add(this.TurkeyCentersComboBox);
			this.CentersPanel.Controls.Add(this.RussiaCentersComboBox);
			this.CentersPanel.Controls.Add(this.ItalyCentersComboBox);
			this.CentersPanel.Controls.Add(this.GermanyCentersComboBox);
			this.CentersPanel.Controls.Add(this.FranceCentersComboBox);
			this.CentersPanel.Controls.Add(this.EnglandCentersComboBox);
			this.CentersPanel.Controls.Add(this.AustriaCentersComboBox);
			this.CentersPanel.Location = new System.Drawing.Point(75, 26);
			this.CentersPanel.Name = "CentersPanel";
			this.CentersPanel.Size = new System.Drawing.Size(65, 209);
			this.CentersPanel.TabIndex = 85;
			//
			// CentersHeaderLabel
			//
			this.CentersHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CentersHeaderLabel.Location = new System.Drawing.Point(1, 2);
			this.CentersHeaderLabel.Name = "CentersHeaderLabel";
			this.CentersHeaderLabel.Size = new System.Drawing.Size(65, 16);
			this.CentersHeaderLabel.TabIndex = 78;
			this.CentersHeaderLabel.Text = "CENTERS";
			this.CentersHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// TurkeyCentersComboBox
			//
			this.TurkeyCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.TurkeyCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TurkeyCentersComboBox.FormattingEnabled = true;
			this.TurkeyCentersComboBox.Location = new System.Drawing.Point(3, 185);
			this.TurkeyCentersComboBox.Name = "TurkeyCentersComboBox";
			this.TurkeyCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.TurkeyCentersComboBox.TabIndex = 77;
			this.TurkeyCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.TurkeyCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.TurkeyCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// RussiaCentersComboBox
			//
			this.RussiaCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.RussiaCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RussiaCentersComboBox.FormattingEnabled = true;
			this.RussiaCentersComboBox.Location = new System.Drawing.Point(3, 158);
			this.RussiaCentersComboBox.Name = "RussiaCentersComboBox";
			this.RussiaCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.RussiaCentersComboBox.TabIndex = 76;
			this.RussiaCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.RussiaCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.RussiaCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// ItalyCentersComboBox
			//
			this.ItalyCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ItalyCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItalyCentersComboBox.FormattingEnabled = true;
			this.ItalyCentersComboBox.Location = new System.Drawing.Point(3, 131);
			this.ItalyCentersComboBox.Name = "ItalyCentersComboBox";
			this.ItalyCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.ItalyCentersComboBox.TabIndex = 75;
			this.ItalyCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.ItalyCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.ItalyCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// GermanyCentersComboBox
			//
			this.GermanyCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.GermanyCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GermanyCentersComboBox.FormattingEnabled = true;
			this.GermanyCentersComboBox.Location = new System.Drawing.Point(3, 104);
			this.GermanyCentersComboBox.Name = "GermanyCentersComboBox";
			this.GermanyCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.GermanyCentersComboBox.TabIndex = 74;
			this.GermanyCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.GermanyCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.GermanyCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// FranceCentersComboBox
			//
			this.FranceCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.FranceCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FranceCentersComboBox.FormattingEnabled = true;
			this.FranceCentersComboBox.Location = new System.Drawing.Point(3, 77);
			this.FranceCentersComboBox.Name = "FranceCentersComboBox";
			this.FranceCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.FranceCentersComboBox.TabIndex = 73;
			this.FranceCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.FranceCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.FranceCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// EnglandCentersComboBox
			//
			this.EnglandCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.EnglandCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnglandCentersComboBox.FormattingEnabled = true;
			this.EnglandCentersComboBox.Location = new System.Drawing.Point(3, 50);
			this.EnglandCentersComboBox.Name = "EnglandCentersComboBox";
			this.EnglandCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.EnglandCentersComboBox.TabIndex = 72;
			this.EnglandCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.EnglandCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.EnglandCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// AustriaCentersComboBox
			//
			this.AustriaCentersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.AustriaCentersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AustriaCentersComboBox.FormattingEnabled = true;
			this.AustriaCentersComboBox.Location = new System.Drawing.Point(3, 23);
			this.AustriaCentersComboBox.Name = "AustriaCentersComboBox";
			this.AustriaCentersComboBox.Size = new System.Drawing.Size(59, 21);
			this.AustriaCentersComboBox.TabIndex = 71;
			this.AustriaCentersComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.AustriaCentersComboBox.SelectedIndexChanged += new System.EventHandler(this.CentersComboBox_SelectedIndexChanged);
			this.AustriaCentersComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// ResultPanel
			//
			this.ResultPanel.Controls.Add(this.ResultHeaderLabel);
			this.ResultPanel.Controls.Add(this.TurkeyResultComboBox);
			this.ResultPanel.Controls.Add(this.RussiaResultComboBox);
			this.ResultPanel.Controls.Add(this.ItalyResultComboBox);
			this.ResultPanel.Controls.Add(this.GermanyResultComboBox);
			this.ResultPanel.Controls.Add(this.AustriaResultComboBox);
			this.ResultPanel.Controls.Add(this.EnglandResultComboBox);
			this.ResultPanel.Controls.Add(this.FranceResultComboBox);
			this.ResultPanel.Location = new System.Drawing.Point(139, 26);
			this.ResultPanel.Name = "ResultPanel";
			this.ResultPanel.Size = new System.Drawing.Size(90, 209);
			this.ResultPanel.TabIndex = 86;
			//
			// ResultHeaderLabel
			//
			this.ResultHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ResultHeaderLabel.Location = new System.Drawing.Point(0, 2);
			this.ResultHeaderLabel.Name = "ResultHeaderLabel";
			this.ResultHeaderLabel.Size = new System.Drawing.Size(90, 16);
			this.ResultHeaderLabel.TabIndex = 77;
			this.ResultHeaderLabel.Text = "RESULT";
			this.ResultHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// TurkeyResultComboBox
			//
			this.TurkeyResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.TurkeyResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TurkeyResultComboBox.FormattingEnabled = true;
			this.TurkeyResultComboBox.Location = new System.Drawing.Point(3, 185);
			this.TurkeyResultComboBox.Name = "TurkeyResultComboBox";
			this.TurkeyResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.TurkeyResultComboBox.TabIndex = 76;
			this.TurkeyResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.TurkeyResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.TurkeyResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// RussiaResultComboBox
			//
			this.RussiaResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.RussiaResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RussiaResultComboBox.FormattingEnabled = true;
			this.RussiaResultComboBox.Location = new System.Drawing.Point(3, 158);
			this.RussiaResultComboBox.Name = "RussiaResultComboBox";
			this.RussiaResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.RussiaResultComboBox.TabIndex = 75;
			this.RussiaResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.RussiaResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.RussiaResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// ItalyResultComboBox
			//
			this.ItalyResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ItalyResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItalyResultComboBox.FormattingEnabled = true;
			this.ItalyResultComboBox.Location = new System.Drawing.Point(3, 131);
			this.ItalyResultComboBox.Name = "ItalyResultComboBox";
			this.ItalyResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.ItalyResultComboBox.TabIndex = 74;
			this.ItalyResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.ItalyResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.ItalyResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// GermanyResultComboBox
			//
			this.GermanyResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.GermanyResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GermanyResultComboBox.FormattingEnabled = true;
			this.GermanyResultComboBox.Location = new System.Drawing.Point(3, 104);
			this.GermanyResultComboBox.Name = "GermanyResultComboBox";
			this.GermanyResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.GermanyResultComboBox.TabIndex = 73;
			this.GermanyResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.GermanyResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.GermanyResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// AustriaResultComboBox
			//
			this.AustriaResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.AustriaResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AustriaResultComboBox.FormattingEnabled = true;
			this.AustriaResultComboBox.Location = new System.Drawing.Point(3, 23);
			this.AustriaResultComboBox.Name = "AustriaResultComboBox";
			this.AustriaResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.AustriaResultComboBox.TabIndex = 70;
			this.AustriaResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.AustriaResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.AustriaResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// EnglandResultComboBox
			//
			this.EnglandResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.EnglandResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnglandResultComboBox.FormattingEnabled = true;
			this.EnglandResultComboBox.Location = new System.Drawing.Point(3, 50);
			this.EnglandResultComboBox.Name = "EnglandResultComboBox";
			this.EnglandResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.EnglandResultComboBox.TabIndex = 71;
			this.EnglandResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.EnglandResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.EnglandResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// FranceResultComboBox
			//
			this.FranceResultComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.FranceResultComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FranceResultComboBox.FormattingEnabled = true;
			this.FranceResultComboBox.Location = new System.Drawing.Point(3, 77);
			this.FranceResultComboBox.Name = "FranceResultComboBox";
			this.FranceResultComboBox.Size = new System.Drawing.Size(86, 21);
			this.FranceResultComboBox.TabIndex = 72;
			this.FranceResultComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.FranceResultComboBox.SelectedIndexChanged += new System.EventHandler(this.ResultComboBox_SelectedIndexChanged);
			this.FranceResultComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// YearsPanel
			//
			this.YearsPanel.Controls.Add(this.YearsHeaderLabel);
			this.YearsPanel.Controls.Add(this.TurkeyYearsComboBox);
			this.YearsPanel.Controls.Add(this.RussiaYearsComboBox);
			this.YearsPanel.Controls.Add(this.ItalyYearsComboBox);
			this.YearsPanel.Controls.Add(this.GermanyYearsComboBox);
			this.YearsPanel.Controls.Add(this.FranceYearsComboBox);
			this.YearsPanel.Controls.Add(this.EnglandYearsComboBox);
			this.YearsPanel.Controls.Add(this.AustriaYearsComboBox);
			this.YearsPanel.Location = new System.Drawing.Point(230, 26);
			this.YearsPanel.Name = "YearsPanel";
			this.YearsPanel.Size = new System.Drawing.Size(80, 209);
			this.YearsPanel.TabIndex = 87;
			//
			// YearsHeaderLabel
			//
			this.YearsHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.YearsHeaderLabel.Location = new System.Drawing.Point(-3, 2);
			this.YearsHeaderLabel.Name = "YearsHeaderLabel";
			this.YearsHeaderLabel.Size = new System.Drawing.Size(83, 16);
			this.YearsHeaderLabel.TabIndex = 84;
			this.YearsHeaderLabel.Text = "FINAL YEAR";
			this.YearsHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// TurkeyYearsComboBox
			//
			this.TurkeyYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.TurkeyYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TurkeyYearsComboBox.FormattingEnabled = true;
			this.TurkeyYearsComboBox.Location = new System.Drawing.Point(3, 185);
			this.TurkeyYearsComboBox.Name = "TurkeyYearsComboBox";
			this.TurkeyYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.TurkeyYearsComboBox.TabIndex = 83;
			this.TurkeyYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.TurkeyYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.TurkeyYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// RussiaYearsComboBox
			//
			this.RussiaYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.RussiaYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RussiaYearsComboBox.FormattingEnabled = true;
			this.RussiaYearsComboBox.Location = new System.Drawing.Point(3, 158);
			this.RussiaYearsComboBox.Name = "RussiaYearsComboBox";
			this.RussiaYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.RussiaYearsComboBox.TabIndex = 82;
			this.RussiaYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.RussiaYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.RussiaYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// ItalyYearsComboBox
			//
			this.ItalyYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ItalyYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItalyYearsComboBox.FormattingEnabled = true;
			this.ItalyYearsComboBox.Location = new System.Drawing.Point(3, 131);
			this.ItalyYearsComboBox.Name = "ItalyYearsComboBox";
			this.ItalyYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.ItalyYearsComboBox.TabIndex = 81;
			this.ItalyYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.ItalyYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.ItalyYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// GermanyYearsComboBox
			//
			this.GermanyYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.GermanyYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GermanyYearsComboBox.FormattingEnabled = true;
			this.GermanyYearsComboBox.Location = new System.Drawing.Point(3, 104);
			this.GermanyYearsComboBox.Name = "GermanyYearsComboBox";
			this.GermanyYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.GermanyYearsComboBox.TabIndex = 80;
			this.GermanyYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.GermanyYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.GermanyYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// FranceYearsComboBox
			//
			this.FranceYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.FranceYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FranceYearsComboBox.FormattingEnabled = true;
			this.FranceYearsComboBox.Location = new System.Drawing.Point(3, 77);
			this.FranceYearsComboBox.Name = "FranceYearsComboBox";
			this.FranceYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.FranceYearsComboBox.TabIndex = 79;
			this.FranceYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.FranceYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.FranceYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// EnglandYearsComboBox
			//
			this.EnglandYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.EnglandYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EnglandYearsComboBox.FormattingEnabled = true;
			this.EnglandYearsComboBox.Location = new System.Drawing.Point(3, 50);
			this.EnglandYearsComboBox.Name = "EnglandYearsComboBox";
			this.EnglandYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.EnglandYearsComboBox.TabIndex = 78;
			this.EnglandYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.EnglandYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.EnglandYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// AustriaYearsComboBox
			//
			this.AustriaYearsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.AustriaYearsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AustriaYearsComboBox.FormattingEnabled = true;
			this.AustriaYearsComboBox.Location = new System.Drawing.Point(3, 23);
			this.AustriaYearsComboBox.Name = "AustriaYearsComboBox";
			this.AustriaYearsComboBox.Size = new System.Drawing.Size(73, 21);
			this.AustriaYearsComboBox.TabIndex = 77;
			this.AustriaYearsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CenteredComboBox_DrawItem);
			this.AustriaYearsComboBox.SelectedIndexChanged += new System.EventHandler(this.YearsComboBox_SelectedIndexChanged);
			this.AustriaYearsComboBox.EnabledChanged += new System.EventHandler(this.ComboBox_EnabledChanged);
			//
			// SoloConcededCheckBox
			//
			this.SoloConcededCheckBox.AutoSize = true;
			this.SoloConcededCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SoloConcededCheckBox.Location = new System.Drawing.Point(124, 3);
			this.SoloConcededCheckBox.Name = "SoloConcededCheckBox";
			this.SoloConcededCheckBox.Size = new System.Drawing.Size(127, 17);
			this.SoloConcededCheckBox.TabIndex = 76;
			this.SoloConcededCheckBox.Text = "Solo was Conceded?";
			this.SoloConcededCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.SoloConcededCheckBox.UseVisualStyleBackColor = true;
			this.SoloConcededCheckBox.CheckedChanged += new System.EventHandler(this.SoloConcededCheckBox_CheckedChanged);
			//
			// AustriaLabel
			//
			this.AustriaLabel.BackColor = System.Drawing.Color.Red;
			this.AustriaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AustriaLabel.ForeColor = System.Drawing.Color.White;
			this.AustriaLabel.Location = new System.Drawing.Point(3, 49);
			this.AustriaLabel.Name = "AustriaLabel";
			this.AustriaLabel.Size = new System.Drawing.Size(68, 21);
			this.AustriaLabel.TabIndex = 77;
			this.AustriaLabel.Tag = "Austria";
			this.AustriaLabel.Text = "AUSTRIA";
			this.AustriaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// EnglandLabel
			//
			this.EnglandLabel.BackColor = System.Drawing.Color.RoyalBlue;
			this.EnglandLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EnglandLabel.ForeColor = System.Drawing.Color.White;
			this.EnglandLabel.Location = new System.Drawing.Point(3, 76);
			this.EnglandLabel.Name = "EnglandLabel";
			this.EnglandLabel.Size = new System.Drawing.Size(68, 21);
			this.EnglandLabel.TabIndex = 78;
			this.EnglandLabel.Tag = "England";
			this.EnglandLabel.Text = "ENGLAND";
			this.EnglandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// FranceLabel
			//
			this.FranceLabel.BackColor = System.Drawing.Color.SkyBlue;
			this.FranceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FranceLabel.Location = new System.Drawing.Point(4, 103);
			this.FranceLabel.Name = "FranceLabel";
			this.FranceLabel.Size = new System.Drawing.Size(68, 21);
			this.FranceLabel.TabIndex = 79;
			this.FranceLabel.Tag = "France";
			this.FranceLabel.Text = "FRANCE";
			this.FranceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// GermanyLabel
			//
			this.GermanyLabel.BackColor = System.Drawing.Color.Black;
			this.GermanyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GermanyLabel.ForeColor = System.Drawing.Color.White;
			this.GermanyLabel.Location = new System.Drawing.Point(4, 130);
			this.GermanyLabel.Name = "GermanyLabel";
			this.GermanyLabel.Size = new System.Drawing.Size(68, 21);
			this.GermanyLabel.TabIndex = 80;
			this.GermanyLabel.Tag = "Germany";
			this.GermanyLabel.Text = "GERMANY";
			this.GermanyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// ItalyLabel
			//
			this.ItalyLabel.BackColor = System.Drawing.Color.Lime;
			this.ItalyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ItalyLabel.Location = new System.Drawing.Point(4, 157);
			this.ItalyLabel.Name = "ItalyLabel";
			this.ItalyLabel.Size = new System.Drawing.Size(68, 21);
			this.ItalyLabel.TabIndex = 81;
			this.ItalyLabel.Tag = "Italy";
			this.ItalyLabel.Text = "ITALY";
			this.ItalyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// RussiaLabel
			//
			this.RussiaLabel.BackColor = System.Drawing.Color.White;
			this.RussiaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RussiaLabel.Location = new System.Drawing.Point(4, 184);
			this.RussiaLabel.Name = "RussiaLabel";
			this.RussiaLabel.Size = new System.Drawing.Size(68, 21);
			this.RussiaLabel.TabIndex = 82;
			this.RussiaLabel.Tag = "Russia";
			this.RussiaLabel.Text = "RUSSIA";
			this.RussiaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// TurkeyLabel
			//
			this.TurkeyLabel.BackColor = System.Drawing.Color.Yellow;
			this.TurkeyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TurkeyLabel.Location = new System.Drawing.Point(4, 211);
			this.TurkeyLabel.Name = "TurkeyLabel";
			this.TurkeyLabel.Size = new System.Drawing.Size(68, 21);
			this.TurkeyLabel.TabIndex = 83;
			this.TurkeyLabel.Tag = "Turkey";
			this.TurkeyLabel.Text = "TURKEY";
			this.TurkeyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// OtherPanel
			//
			this.OtherPanel.Controls.Add(this.TurkeyOtherTextBox);
			this.OtherPanel.Controls.Add(this.RussiaOtherTextBox);
			this.OtherPanel.Controls.Add(this.ItalyOtherTextBox);
			this.OtherPanel.Controls.Add(this.GermanyOtherTextBox);
			this.OtherPanel.Controls.Add(this.FranceOtherTextBox);
			this.OtherPanel.Controls.Add(this.EnglandOtherTextBox);
			this.OtherPanel.Controls.Add(this.AustriaOtherTextBox);
			this.OtherPanel.Controls.Add(this.OtherScoreLabel);
			this.OtherPanel.Location = new System.Drawing.Point(312, 3);
			this.OtherPanel.Name = "OtherPanel";
			this.OtherPanel.Size = new System.Drawing.Size(69, 232);
			this.OtherPanel.TabIndex = 88;
			//
			// TurkeyOtherTextBox
			//
			this.TurkeyOtherTextBox.Location = new System.Drawing.Point(0, 208);
			this.TurkeyOtherTextBox.Name = "TurkeyOtherTextBox";
			this.TurkeyOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.TurkeyOtherTextBox.TabIndex = 92;
			this.TurkeyOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.TurkeyOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// RussiaOtherTextBox
			//
			this.RussiaOtherTextBox.Location = new System.Drawing.Point(0, 181);
			this.RussiaOtherTextBox.Name = "RussiaOtherTextBox";
			this.RussiaOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.RussiaOtherTextBox.TabIndex = 91;
			this.RussiaOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.RussiaOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// ItalyOtherTextBox
			//
			this.ItalyOtherTextBox.Location = new System.Drawing.Point(0, 155);
			this.ItalyOtherTextBox.Name = "ItalyOtherTextBox";
			this.ItalyOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.ItalyOtherTextBox.TabIndex = 90;
			this.ItalyOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.ItalyOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// GermanyOtherTextBox
			//
			this.GermanyOtherTextBox.Location = new System.Drawing.Point(0, 127);
			this.GermanyOtherTextBox.Name = "GermanyOtherTextBox";
			this.GermanyOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.GermanyOtherTextBox.TabIndex = 89;
			this.GermanyOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.GermanyOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// FranceOtherTextBox
			//
			this.FranceOtherTextBox.Location = new System.Drawing.Point(0, 101);
			this.FranceOtherTextBox.Name = "FranceOtherTextBox";
			this.FranceOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.FranceOtherTextBox.TabIndex = 88;
			this.FranceOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.FranceOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// EnglandOtherTextBox
			//
			this.EnglandOtherTextBox.Location = new System.Drawing.Point(0, 74);
			this.EnglandOtherTextBox.Name = "EnglandOtherTextBox";
			this.EnglandOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.EnglandOtherTextBox.TabIndex = 87;
			this.EnglandOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.EnglandOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// AustriaOtherTextBox
			//
			this.AustriaOtherTextBox.Location = new System.Drawing.Point(0, 47);
			this.AustriaOtherTextBox.Name = "AustriaOtherTextBox";
			this.AustriaOtherTextBox.Size = new System.Drawing.Size(68, 20);
			this.AustriaOtherTextBox.TabIndex = 86;
			this.AustriaOtherTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.AustriaOtherTextBox.TextChanged += new System.EventHandler(this.OtherTextBox_TextChanged);
			//
			// OtherScoreLabel
			//
			this.OtherScoreLabel.AutoEllipsis = true;
			this.OtherScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OtherScoreLabel.Location = new System.Drawing.Point(0, 2);
			this.OtherScoreLabel.Name = "OtherScoreLabel";
			this.OtherScoreLabel.Size = new System.Drawing.Size(69, 39);
			this.OtherScoreLabel.TabIndex = 85;
			this.OtherScoreLabel.Text = "OTHER";
			this.OtherScoreLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			//
			// GameControl
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.OtherPanel);
			this.Controls.Add(this.YearsPanel);
			this.Controls.Add(this.ResultPanel);
			this.Controls.Add(this.CentersPanel);
			this.Controls.Add(this.PowerColumnHeaderLabel);
			this.Controls.Add(this.TurkeyLabel);
			this.Controls.Add(this.RussiaLabel);
			this.Controls.Add(this.ItalyLabel);
			this.Controls.Add(this.GermanyLabel);
			this.Controls.Add(this.FranceLabel);
			this.Controls.Add(this.EnglandLabel);
			this.Controls.Add(this.AustriaLabel);
			this.Controls.Add(this.SoloConcededCheckBox);
			this.Margin = new System.Windows.Forms.Padding(1);
			this.Name = "GameControl";
			this.Size = new System.Drawing.Size(381, 235);
			this.Load += new System.EventHandler(this.GameControl_Load);
			this.EnabledChanged += new System.EventHandler(this.GameControl_EnabledChanged);
			this.CentersPanel.ResumeLayout(false);
			this.ResultPanel.ResumeLayout(false);
			this.YearsPanel.ResumeLayout(false);
			this.OtherPanel.ResumeLayout(false);
			this.OtherPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private Label AustriaLabel;
		private Label EnglandLabel;
		private Label FranceLabel;
		private Label GermanyLabel;
		private Label ItalyLabel;
		private Label RussiaLabel;
		private Label TurkeyLabel;
		private Label PowerColumnHeaderLabel;
		private Panel CentersPanel;
		private ComboBox TurkeyCentersComboBox;
		private ComboBox RussiaCentersComboBox;
		private ComboBox ItalyCentersComboBox;
		private ComboBox GermanyCentersComboBox;
		private ComboBox FranceCentersComboBox;
		private ComboBox EnglandCentersComboBox;
		private ComboBox AustriaCentersComboBox;
		private Panel ResultPanel;
		private ComboBox TurkeyResultComboBox;
		private ComboBox RussiaResultComboBox;
		private ComboBox ItalyResultComboBox;
		private ComboBox GermanyResultComboBox;
		private ComboBox AustriaResultComboBox;
		private ComboBox EnglandResultComboBox;
		private ComboBox FranceResultComboBox;
		private Panel YearsPanel;
		private CheckBox SoloConcededCheckBox;
		private ComboBox TurkeyYearsComboBox;
		private ComboBox RussiaYearsComboBox;
		private ComboBox ItalyYearsComboBox;
		private ComboBox GermanyYearsComboBox;
		private ComboBox FranceYearsComboBox;
		private ComboBox EnglandYearsComboBox;
		private ComboBox AustriaYearsComboBox;
		private Label CentersHeaderLabel;
		private Label ResultHeaderLabel;
		private Label YearsHeaderLabel;
		private Panel OtherPanel;
		private TextBox TurkeyOtherTextBox;
		private TextBox RussiaOtherTextBox;
		private TextBox ItalyOtherTextBox;
		private TextBox GermanyOtherTextBox;
		private TextBox FranceOtherTextBox;
		private TextBox EnglandOtherTextBox;
		private TextBox AustriaOtherTextBox;
		private Label OtherScoreLabel;
	}
}
