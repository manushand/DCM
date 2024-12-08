namespace PC.Forms;

internal sealed partial class RoundInfoForm : Form
{
	internal static readonly RoundInfoForm None = new (Tournament.None);
	internal readonly Tournament Tournament;

	internal RoundInfoForm(Tournament tournament)
	{
		InitializeComponent();
		Tournament = tournament;
	}

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

	private void SetFormTitle()
		=> Text = $"{Tournament} ─ {(Tournament.Rounds.Length is 0 ? null : "Rounds and ")}Registration";

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

	public void StartNewRound()
	{
		var roundNumber = RoundsTabControl.SelectedIndex + 1;
		if (MessageBox.Show($"Start Round {roundNumber}?{NewLine}{NewLine}Pre-registration for that round will be closed.",
							"Confirm Start Next Round",
							YesNo,
							Question) is DialogResult.No)
			return;
		var round = CreateOne(new Round
							  {
								  Tournament = Tournament,
								  Number = roundNumber
							  });
		CreateMany(Tournament.TournamentPlayers
							 .Where(tournamentPlayer => tournamentPlayer.RegisteredForRound(roundNumber))
							 .Select(tournamentPlayer => new RoundPlayer
														 {
															 Round = round,
															 Player = tournamentPlayer.Player
														 })
							 .ToArray());
		RoundsTabControl.TabPages
						.Add($"Round {roundNumber}");
		RegistrationControl.StartRound();
		RoundsTabControl.ActivateTab(roundNumber);
		SetFormTitle();
	}

	public void DiscardRound()
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
}
