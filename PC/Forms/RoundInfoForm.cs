﻿namespace PC.Forms;

internal sealed partial class RoundInfoForm : Form
{
	#region Public interface

	#region Data

	internal static readonly RoundInfoForm None = new (Tournament.None);
	internal readonly Tournament Tournament;

	#endregion

	#region Constructor

	internal RoundInfoForm(Tournament tournament)
	{
		InitializeComponent();
		Tournament = tournament;
	}

	#endregion

	#region Methods

	internal void StartNewRound()
	{
		var roundNumber = RoundsTabControl.SelectedIndex + 1;
		if (MessageBox.Show($"Start Round {roundNumber}?{NewLine}{NewLine}Pre-registration for that round will be closed.",
							"Confirm Start Next Round",
							YesNo,
							Question) is DialogResult.No)
			return;
		Tournament.CreateRound();
		RoundsTabControl.TabPages
						.Add($"Round {roundNumber}");
		RegistrationControl.StartRound();
		RoundsTabControl.ActivateTab(roundNumber);
		SetFormTitle();
	}

	internal void DiscardRound()
	{
		var roundNumber = RoundsTabControl.SelectedIndex;
		if (MessageBox.Show($"Discard Round {roundNumber} to start fresh?",
							"Confirm Discard Round",
							YesNo,
							Question) is DialogResult.No)
			return;
		RegistrationControl.DiscardRound();
		RoundControl.DiscardRound();
		SetFormTitle();
		RoundsTabControl.TabPages
						.RemoveAt(roundNumber);
		RoundsTabControl.ActivateTab(--roundNumber);
	}

	#endregion

	#endregion

	#region Private implementation

	#region Event handlers

	private void RoundInfoForm_Load(object sender,
									EventArgs e)
	{
		SetFormTitle();
		RegistrationControl.LoadControl(this);
		RoundControl.LoadControl(this);
		var roundNumber = 0;
		SkipHandlers(() =>
        {
			RoundsTabControl.TabPages
							.Clear();
			RoundsTabControl.TabPages
							.Add(RegistrationTabPage);
			roundNumber = Tournament.Rounds
									.Length;
			RegistrationTabPage.Enabled = roundNumber < Tournament.TotalRounds;
			ForRange(1, roundNumber, round => RoundsTabControl.TabPages
															  .Add($"Round {round}"));
        });
		RoundsTabControl.ActivateTab(roundNumber);
	}

	private void RoundsTabControl_SelectedIndexChanged(object sender,
													   EventArgs e)
	{
		if (SkippingHandlers)
			return;
		const int margin = 20;
		if (RoundsTabControl.SelectedIndex is 0)
		{
			RegistrationControl.Activate();
			RoundsTabControl.Width = RegistrationControl.Width + margin;
		}
		else
		{
			var tabControls = RoundsTabControl.SelectedTab
											  .OrThrow()
											  .Controls;
			if (tabControls.Count is 0)
				//	TODO: do we need to set SkipHandlers to true for this?
				tabControls.Add(RoundControl);
			RoundControl.Activate(RoundsTabControl.SelectedIndex);
			RoundsTabControl.Width = RoundControl.Width + margin;
		}
		Width = RoundsTabControl.Width + (margin << 1);
	}

	#endregion

	#region Method

	private void SetFormTitle()
		=> Text = $"{Tournament} ─ {(Tournament.Rounds.Length is 0 ? null : "Rounds and ")}Registration";

	#endregion

	#endregion
}
