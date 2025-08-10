﻿namespace PC.Forms
{
	internal sealed partial class EventListForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventListForm));
			this.TournamentNameLabel = new System.Windows.Forms.Label();
			this.TournamentComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			//
			// TournamentNameLabel
			//
			this.TournamentNameLabel.AutoSize = true;
			this.TournamentNameLabel.Location = new System.Drawing.Point(14, 16);
			this.TournamentNameLabel.Name = "TournamentNameLabel";
			this.TournamentNameLabel.Size = new System.Drawing.Size(67, 13);
			this.TournamentNameLabel.TabIndex = 0;
			this.TournamentNameLabel.Text = "Event:";
			//
			// TournamentComboBox
			//
			this.TournamentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TournamentComboBox.FormattingEnabled = true;
			this.TournamentComboBox.Location = new System.Drawing.Point(60, 13);
			this.TournamentComboBox.Name = "TournamentComboBox";
			this.TournamentComboBox.Size = new System.Drawing.Size(310, 21);
			this.TournamentComboBox.TabIndex = 1;
			this.TournamentComboBox.SelectedIndexChanged += new System.EventHandler(this.TournamentComboBox_SelectedIndexChanged);
			//
			// TournamentListForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(401, 46);
			this.Controls.Add(this.TournamentComboBox);
			this.Controls.Add(this.TournamentNameLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EventListForm";
			this.ShowIcon = false;
			this.Text = "Open Event";
			this.Load += new System.EventHandler(this.TournamentListForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Label TournamentNameLabel;
		private ComboBox TournamentComboBox;
	}
}
