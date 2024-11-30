namespace DCM.UI.Controls;

internal sealed partial class RegistrationControl : UserControl
{
	private RoundInfoForm RoundInfoForm
	{
		get => field == RoundInfoForm.Empty ? throw new NullReferenceException(nameof (RoundInfoForm)) : field;
		set;
	} = RoundInfoForm.Empty;

	private List<Player> UnregisteredPlayers { get; } = [];

	private List<RegisteredPlayer> RegisteredPlayers { get; } = [];

	private Tournament Tournament => RoundInfoForm.Tournament;

	private List<CheckBox> RegistrationCheckBoxes { get; }

	private int WidthWithStartRoundButton { get; }

	internal RegistrationControl()
	{
		InitializeComponent();
		WidthWithStartRoundButton = Width;
		RegistrationCheckBoxes =
		[
			Round1RegistrationCheckBox, Round2RegistrationCheckBox, Round3RegistrationCheckBox,
			Round4RegistrationCheckBox, Round5RegistrationCheckBox, Round6RegistrationCheckBox,
			Round7RegistrationCheckBox, Round8RegistrationCheckBox, Round9RegistrationCheckBox
		];
	}

	internal void LoadControl(RoundInfoForm roundChanger)
	{
		RoundInfoForm = roundChanger;
		var startedRounds = Tournament.Rounds.Length - 1;
		RegistrationCheckBoxes.Apply((box, roundNumber) =>
									 {
										 box.Visible = roundNumber < Tournament.TotalRounds;
										 box.Enabled = roundNumber > startedRounds;
									 });
		RoundsRegisteredGroupBox.Hide();
		RegisterPlayerButton.Hide();
		LastNameRegistrationTabRadioButton.Checked = true;
	}

	internal void Activate()
	{
		var showStartButton = Tournament.Rounds.Length is 0;
		StartFirstRoundButton.Visible = showStartButton;
		Width = WidthWithStartRoundButton - (showStartButton ? 0 : StartFirstRoundButton.Width + 10);
		FillLists();
		RegisteredDataGridView.Deselect();
	}

	private void FillLists(bool registeredOnly = false)
	{
		if (!registeredOnly)
		{
			//	Don't take the advice to use pattern matching here. Also don't move this line below the coming .Fill().
			var selectedUnregisteredPlayer = UnregisteredListBox.GetSelected<Player>();
			var unregisteredPlayers = UnregisteredPlayers.OrderBy(player => FirstNameRegistrationTabRadioButton.Checked
																				? player.Name
																				: player.LastFirst)
														 .ToArray();
			UnregisteredListBox.FillWithRecords(unregisteredPlayers);
			UnregisteredPlayersLabel.Text = "Unregistered Player".Pluralize(unregisteredPlayers, true);
			if (selectedUnregisteredPlayer is not null)
				UnregisteredListBox.SelectedItem = UnregisteredListBox.Find(selectedUnregisteredPlayer);
		}

		var selectedRegisteredPlayer = (RegisteredDataGridView.CurrentRow?.DataBoundItem as RegisteredPlayer)?.Player;
		var registeredPlayers = RegisteredPlayers.OrderBy(player => FirstNameRegistrationTabRadioButton.Checked
																		? player.Name
																		: player.ByLastName)
												 .ToArray();
		//  TODO - Is Eventless needed??
		SkipHandlers(() => RegisteredDataGridView.FillWith(registeredPlayers));
		RegisteredPlayersLabel.Text = "Registered Player".Pluralize(registeredPlayers, true);
		if (selectedRegisteredPlayer is not null)
			SetRegisteredPlayer(selectedRegisteredPlayer);
	}

	private void UnregisteredListBox_SelectedIndexChanged(object sender,
														  EventArgs e)
	{
		if (SkippingHandlers)
			return;
		if (RegisteredDataGridView.CurrentRow is not null)
			SkipHandlers(RegisteredDataGridView.Deselect);
		RoundsRegisteredGroupBox.Hide();
		RegisterPlayerButton.Show();
		RegisterPlayerButton.Text = "Register This Player  ───────▶";
	}

	private void RegisterPlayerButton_Click(object sender,
											EventArgs? e = null)
	{
		var unregisteredPlayer = sender as Player ?? UnregisteredListBox.SelectedItem as Player;
		if (unregisteredPlayer is not null)
		{
			UnregisteredListBox.ClearSelected();
			Tournament.AddPlayer(unregisteredPlayer);
			UnregisteredPlayers.Remove(unregisteredPlayer);
			RegisteredPlayers.Add(new (unregisteredPlayer));
			FillLists();
			SetRegisteredPlayer(unregisteredPlayer);
			RegisteredDataGridView_SelectionChanged(); //	TODO: Is this necessary? Doesn't the line above handle it?  Seemed not?
			foreach (var roundCheckBox in RegistrationCheckBoxes.Where(static checkbox => checkbox.Visible))
				roundCheckBox.Checked = true;
		}
		else if (RegisteredDataGridView.CurrentRow?.DataBoundItem is RegisteredPlayer registeredPlayer)
		{
			RegisteredDataGridView.Deselect();
			var player = registeredPlayer.Player;
			Delete(Tournament.TournamentPlayers.ByPlayerId(player.Id));
			RegisteredPlayers.Remove(registeredPlayer);
			UnregisteredPlayers.Add(player);
			FillLists();
			UnregisteredListBox.SelectedItem = UnregisteredListBox.Find(player);
		}
	}

	//	ReSharper disable once SuggestBaseTypeForParameter
	private void SetRegisteredPlayer(Player player)
	{
		var tournamentPlayer = Tournament.TournamentPlayers
										 .ByPlayerId(player.Id);
		RoundsRegisteredGroupBox.Text = player.Name;
		SkipHandlers(() => ForRange(0, Tournament.TotalRounds, round => RegistrationCheckBoxes[round].Checked = tournamentPlayer.RegisteredForRound(round + 1)));
		RegisteredDataGridView.SelectRowWhere<RegisteredPlayer>(rp => rp.Id == player.Id);
	}

	private void RoundRegistrationCheckBox_CheckedChanged(object sender,
														  EventArgs e)
	{
		if (SkippingHandlers)
			return;
		var checkBox = (CheckBox)sender;
		var roundNumber = RegistrationCheckBoxes.IndexOf(checkBox) + 1;
		if (RegisteredDataGridView.CurrentRow?.DataBoundItem is not RegisteredPlayer registeredPlayer)
			return;
		var tournamentPlayer = Tournament.TournamentPlayers.ByPlayerId(registeredPlayer.Id);
		if (checkBox.Checked)
			tournamentPlayer.RegisterForRound(roundNumber);
		else
			tournamentPlayer.UnregisterForRound(roundNumber);
		UpdateOne(tournamentPlayer);
		registeredPlayer.Rounds = tournamentPlayer.RoundsRegistered;
		FillLists(true);
		SetRegisteredPlayer(registeredPlayer.Player);
	}

	private void RegisteredDataGridView_DataBindingComplete(object sender,
															DataGridViewBindingCompleteEventArgs e)
		=> RegisteredDataGridView.FillColumn(default);

	private void RegisteredDataGridView_SelectionChanged(object? sender = null,
														 EventArgs? e = null)
	{
		if (SkippingHandlers)
			return;
		SkipHandlers(UnregisteredListBox.ClearSelected);
		if (RegisteredDataGridView.CurrentRow?.DataBoundItem is not RegisteredPlayer registeredPlayer)
			return;
		RoundsRegisteredGroupBox.Show();
		RegisterPlayerButton.Show();
		RegisterPlayerButton.Text = "◀───────  Unregister This Player";
		SetRegisteredPlayer(registeredPlayer.Player);
	}

	private void NameRegistrationTabRadioButton_CheckedChanged(object sender,
															   EventArgs e)
	{
		RegisteredPlayers.FillWith(Tournament.TournamentPlayers
											 .Select(static tournamentPlayer => new RegisteredPlayer(tournamentPlayer.Player,
																									 tournamentPlayer.RoundsRegistered)));
		var registeredPlayerIds = RegisteredPlayers.Select(static tournamentPlayer => tournamentPlayer.Id);
		UnregisteredPlayers.FillWith(ReadMany<Player>(player => !registeredPlayerIds.Contains(player.Id)));
		FillLists();
	}

	private void StartFirstRoundButton_Click(object sender,
											 EventArgs e)
		=> RoundInfoForm.StartNewRound();

	internal void StartRound()
		=> SetRoundRegistrationStatus(false);

	internal void DiscardRound()
		=> SetRoundRegistrationStatus(true);

	private void SetRoundRegistrationStatus(bool enabled)
		=> RegistrationCheckBoxes[Tournament.Rounds.Length - 1].Enabled = enabled;

	private void NewPlayerButton_Click(object sender, EventArgs e)
		=> Show<PlayerInfoForm>(form =>
								{
									var newPlayer = form.Player;
									if (newPlayer.Id > 0)
										RegisterPlayerButton_Click(newPlayer);
								});

	#region RegisteredPlayer class

	//	NOTE: Don't put this in a separate file in another part of this partial class;
	//	If you do, VS will think it has a visual Form and will set up a designer file.

	private sealed class RegisteredPlayer
	{
		public Player Player { get; }

		[PublicAPI]
		public string Rounds { get; set; }

		internal int Id => Player.Id;

		internal string Name => Player.Name;

		internal string ByLastName => Player.LastFirst;

		internal RegisteredPlayer(Player player,
								  string rounds = "")
			=> (Player, Rounds) = (player, rounds);
	}

	#endregion
}
