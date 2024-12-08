namespace PC.Controls;

internal sealed partial class RoundControl /* to Major Tom */ : UserControl
{
	private RoundInfoForm RoundInfoForm
	{
		get => field == RoundInfoForm.None ? throw new NullReferenceException(nameof (RoundInfoForm)) : field;
		set;
	} = RoundInfoForm.None;

	private Round Round
	{
		get => field == Round.None ? throw new NullReferenceException(nameof (Round)) : field;
		set;
	} = Round.None;

	private Tournament Tournament => RoundInfoForm.Tournament;

	internal RoundControl()
		=> InitializeComponent();

	internal void LoadControl(RoundInfoForm roundInfoForm)
	{
		//	If SmtpHost is null, email isn't available.
		EmailButton.Visible = Settings.SmtpHost is not null;
		RoundInfoForm = roundInfoForm;
		SkipHandlers(() =>
					 {
						 TournamentPlayersTabPage.Select();
						 FirstNameRadioButton.Checked = true;
					 });
	}

	internal void Activate(int roundNumber)
	{
		Round = Tournament.Rounds[roundNumber - 1];
		RoundLockedLabel.Visible = !Round.Workable;
		if (RoundLockedLabel.Visible)
			RoundLockedLabel.Text = $"Round is {(Round.Games.All(static game => game.Status is Finished)
													 ? nameof (Finished)
													 : nameof (Underway))}";
		GamesAndPlayersLabel.Text = $"Games and Player Assignments for Round {Round}";
		PlayersRegisteredLabel.Text = $"Players Registered for Round {Round}";

		SortByScoreCheckBox.Visible = Round.Number > 1;
		FillPlayerLists();

		ScoringSystemComboBox.FillWithSorted<ScoringSystem>();
		ScoringSystemComboBox.SetSelectedItem(Round.ScoringSystem);
		ScoringSystemComboBox_SelectedIndexChanged(); //	TODO: Is this needed??

		SetButtonUsability();
	}

	private void SetButtonUsability()
	{
		ChangeRoundButton.Hide();
		if (Round.Workable)
			//	Current round can only be discarded when there are no started games.
			if (!Round.GamesStarted)
			{
				ChangeRoundButton.Show();
				ChangeRoundButton.Enabled = true;
				ChangeRoundButton.Text = $"Discard{NewLine}❌    This{NewLine}Round";
				ChangeRoundButton.BackColor = Color.LightCoral;
			}
			//	Next round can ony be started when there are some started games and
			//	no seeded games in this round and when this isn't the final round.
			else if (Round.Number < Tournament.TotalRounds)
			{
				ChangeRoundButton.Visible = !Round.GamesSeeded;
				ChangeRoundButton.Text = $"Start   ▶{NewLine}Next   ▶{NewLine}Round ▶";
				ChangeRoundButton.BackColor = Color.LightGreen;
			}
		SeedingAssignsPowersCheckBox.Visible =
			SeedingAssignsPowersCheckBox.Checked =
				Tournament.AssignPowers && Round.Workable;
		NewPlayerButton.Visible =
			ScoringSystemComboBox.Enabled =
				Round.Workable;
		StartGamesButton.Enabled = Round.GamesSeeded;
		var (unregisteredCount, unregisteredSelected,
			registeredCount, registeredSelected,
			seededCount, seededSelected) = Round.Workable
											   ? (UnregisteredDataGridView.RowCount, UnregisteredDataGridView.SelectedRows.Count,
												  RegisteredDataGridView.RowCount, RegisteredDataGridView.SelectedRows.Count,
												  SeededDataGridView.RowCount, SeededDataGridView.SelectedRows.Count)
											   : (0, 0, 0, 0, 0, 0);

		FindPlayerLabel.Visible =
			FindPlayerTextBox.Visible =
				PrintButton.Enabled =
					SeededDataGridView.RowCount > 0; //	Don't change this to seededCount > 0; we want the count even if !Workable
		RegisterForRoundButton.Enabled = unregisteredSelected > 0
									  || registeredSelected > 0;
		RegisterForRoundButton.Text = registeredSelected > 0
										  ? "◀─ Sit Out"
										  : "Register ─▶";
		RegisterAllButton.Enabled = unregisteredCount > 0;
		UnregisterAllButton.Enabled = registeredCount > 0;
		SeedAllButton.Enabled = registeredCount > 0 && registeredCount % 7 is 0;
		SeedSomeButton.Visible = registeredSelected > 0 && registeredSelected % 7 is 0;
		if (SeedSomeButton.Visible)
			SeedSomeButton.Text = $"Seed {"Game".Pluralize(registeredSelected / 7, true)} ─▶";
		SwapButton.Enabled = seededSelected is 2; //	TODO: and the selected games aren't finished?
		ReplaceButton.Enabled = registeredSelected is 1
							 && seededSelected is 1;
		UnseedButton.Enabled = Round.GamesSeeded;
		UnseedGameButton.Visible = seededSelected is 1
								&& SeededDataGridView.GetSelected<GamePlayer>().Game.Status is Seeded;
		MoveGameUpButton.Enabled = seededSelected is 1 //	TODO: or more than 1 in same game?
								&& SeededDataGridView.SelectedRows[0].Index > 6;
		MoveGameDownButton.Enabled = seededSelected is 1 //	TODO: or more than 1 in same game?
								  && SeededDataGridView.SelectedRows[0].Index < seededCount - 7;
		ViewGamesButton.Enabled = SeededDataGridView.RowCount > 0;
		foreach (var dataGridView in new[] { UnregisteredDataGridView, RegisteredDataGridView })
		{
			var countSelected = dataGridView.SelectedRows.Count;
			var label = dataGridView == UnregisteredDataGridView
							? UnregisteredCountLabel
							: RegisteredCountLabel;
			label.Text = $"{"Player".Pluralize(dataGridView.RowCount, true)} Listed{(countSelected is 0 ? null : $", {countSelected} Selected")}";
		}
		SortByScoreCheckBox.Visible = Round.Number > 1;
	}

	private void FillPlayerLists(ListsToFill listsToFill = ListsToFill.All)
	{
		if (SkippingHandlers)
			return;
		var games = Round.Games;
		var gamePlayers = games.SelectMany(static game => game.GamePlayers)
							   .ToArray();
		int[] gamePlayerIds = [..gamePlayers.Select(static gamePlayer => gamePlayer.PlayerId)];
		var roundPlayerIds = Round.RoundPlayers
								  .Select(static roundPlayer => roundPlayer.PlayerId)
								  .Where(id => !gamePlayerIds.Contains(id))
								  .ToList();
		roundPlayerIds.AddRange(Tournament.TournamentPlayers
										  .Where(tp => tp.RegisteredForRound(Round.Number))
										  .Select(static tp => tp.PlayerId));
		//	This next assignment SHOULD be unnecessary overkill, but can't hurt.
		roundPlayerIds = [..roundPlayerIds.Where(id => !gamePlayerIds.Contains(id))
										  .Distinct()]; //	Just in case!
		SkipHandlers(() =>
        {
			if (listsToFill.HasFlag(ListsToFill.Unregistered))
			{
				List<int>? tournamentPlayerIds;
				if (WhichPlayersTabControl.SelectedIndex is 0)
				{
					//	Everyone pre-registered for any round in the tournament
					tournamentPlayerIds = [..Tournament.TournamentPlayers.Select(static tp => tp.PlayerId)];
					//	And everyone who has been a RoundPlayer in any round of the tournament
					tournamentPlayerIds.AddRange(Tournament.Rounds
														   .SelectMany(static round => round.RoundPlayers)
														   .Select(static rp => rp.PlayerId));
				}
				else
					//	Everyone at all
					tournamentPlayerIds = null;

				var unregisteredPlayers = ReadMany<Player>(player => (tournamentPlayerIds?.Contains(player.Id) ?? true)
																	 && !gamePlayerIds.Contains(player.Id)
																	 && !roundPlayerIds.Contains(player.Id))
										  .Select(player => new SeedablePlayer(Tournament,
																			   player,
																			   tournamentPlayerIds is null
																				   ? 0
																				   : null))
										  .OrderByDescending(player => SortByScoreCheckBox.Checked
																		   ? player.ScoreBeforeRound
																		   : 0)
										  .ThenBy(player => FirstNameRadioButton.Checked
																? player.Player.Name
																: player.Player.LastFirst)
										  .ToArray();
				UnregisteredDataGridView.DataSource = unregisteredPlayers;
				UnregisteredCountLabel.Text = $"{"Player".Pluralize(unregisteredPlayers, true)} Listed";
				UnregisteredDataGridView.Deselect();
			}

			if (listsToFill.HasFlag(ListsToFill.Registered))
			{
				var playerIds = roundPlayerIds;
				var registeredPlayers = ReadMany<Player>(player => playerIds.Contains(player.Id))
										.Select(player => new SeedablePlayer(Tournament,
																			 player,
																			 Round.Number))
										.OrderByDescending(player => SortByScoreCheckBox.Checked
																		 ? player.ScoreBeforeRound
																		 : default)
										.ThenBy(player => FirstNameRadioButton.Checked
															  ? player.Player.Name
															  : player.Player.LastFirst)
										.ToList();
				RegisteredDataGridView.FillWith(registeredPlayers);
				RegisteredCountLabel.Text = $"{"Player".Pluralize(registeredPlayers, true)} Listed";
				RegisteredDataGridView.Deselect();
			}

			if (!listsToFill.HasFlag(ListsToFill.Seeded))
				return;
			var seededPlayers = gamePlayers.Order()
										   .Select(static gamePlayer => new
																		{
																			Game = gamePlayer.GameNumber,
																			gamePlayer.Player,
																			Power = gamePlayer.PowerName,
																			gamePlayer.Status
																		})
										   .ToList();
			SeededDataGridView.FillWith(seededPlayers);
			SeededPlayerCountLabel.Text = $"{seededPlayers.Count} Players in {games.Length} Games; Total Conflict {Round.Conflict.Points()}";
			SeededDataGridView.Deselect();
        });
	}

	private void ScoringSystemComboBox_SelectedIndexChanged(object? sender = null,
															EventArgs? e = null)
	{
		var scoringSystem = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		if (scoringSystem.Id == Round.ScoringSystemId)
			return;
		var finishedGames = Round.FinishedGames;
		var answer = CheckScoringSystemChange(Round.ScoringSystem, scoringSystem, finishedGames, true);
		if (answer is DialogResult.Cancel)
		{
			SkipHandlers(() => ScoringSystemComboBox.SetSelectedItem(Round.ScoringSystem));
			return;
		}
		//	Before changing the Round's system, if finished games that would go to the new
		//	default are instead to stay with their current system, create a Dictionary that
		//	retains their current systems, so that we can put them back on.
		var retainedSystems = answer is DialogResult.Yes
								  ? null
								  : finishedGames.ToDictionary(static game => game.Id, static game => game.ScoringSystem);
		//	Now make the update to the Round.  This should be done before updating the "lacking"
		//	games so that when setting a game's ScoringSystem (if retained) it doesn't revert to
		//	null because it is the default for its Round (according to the un-updated Round).
		Round.ScoringSystem = scoringSystem;
		UpdateOne(Round);
		ScoringSystemComboBox.UpdateShadowLabel();
		ScoringSystemDefaultLabel.Visible = Round.ScoringSystemIsDefault;
		if (finishedGames.Length is 0)
			return;
		//	Now it is safe to do what must be done to any games affected by the change.
		//	Either put them back to Underway status, or return their ScoringSystems to them.
		finishedGames.ForEach(retainedSystems is null
								  ? static game => game.Status = Underway
								  : game => game.ScoringSystem = retainedSystems[game.Id]);
		UpdateMany(finishedGames);
	}

	private void SeededDataGridView_DataBindingComplete(object sender,
														DataGridViewBindingCompleteEventArgs e)
	{
		SeededDataGridView.FillColumn(0);   //	Why this is 0 not 1, I don't know, but making it 1 will cut off names with ellipses
		SeededDataGridView.AlignColumn(MiddleCenter, 0, 3); //	Game Number, Status
		SeededDataGridView.AlignColumn(MiddleLeft, 1);      //	Player Name
		SeededDataGridView.PowerCells(2);                   //	Power Name
		foreach (DataGridViewRow row in SeededDataGridView.Rows)
			row.DefaultCellStyle.BackColor = (row.Index / 7 & 1) is 0
												 ? SystemColors.Window
												 : SystemColors.Info;
	}

	private void NameSortControl_CheckedChanged(object sender,
												EventArgs e)
	{
		if (SkippingHandlers)
			return;
		//	Remember which players were selected, if any
		int[] unregisteredIds = [..UnregisteredDataGridView.GetMultiSelected<SeedablePlayer>()
														   .Select(static seedable => seedable.Id)];
		int[] registeredIds = [..RegisteredDataGridView.GetMultiSelected<SeedablePlayer>()
													   .Select(static seedable => seedable.Id)];
		FillPlayerLists(ListsToFill.Unseeded);
		//	Re-select the players who were selected before the refill
		SkipHandlers(() =>
        {
			foreach (DataGridViewRow row in UnregisteredDataGridView.Rows)
				row.Selected = unregisteredIds.Contains(UnregisteredDataGridView.GetAtIndex<SeedablePlayer>(row.Index).Id);
			foreach (DataGridViewRow row in RegisteredDataGridView.Rows)
				row.Selected = registeredIds.Contains(RegisteredDataGridView.GetAtIndex<SeedablePlayer>(row.Index).Id);
        });
	}

	private void RegistrableDataGridView_SelectionChanged(object sender,
														  EventArgs e)
	{
		if (SkippingHandlers)
			return;
		SkipHandlers(() =>
        {
			var view = (DataGridView)sender;
			(Round.Workable
				 ? view == UnregisteredDataGridView
					   ? RegisteredDataGridView
					   : UnregisteredDataGridView
				 : view).Deselect();
        });
		SetButtonUsability();
	}

	private void RegisterButton_Click(object sender,
									  EventArgs e)
	{
		if (UnregisteredDataGridView.SelectedRows.Count is 0)
			UnregisterPlayers(RegisteredDataGridView.GetMultiSelected<SeedablePlayer>());
		else
			RegisterPlayers(UnregisteredDataGridView.GetMultiSelected<SeedablePlayer>());
	}

	private void RegisterAllButton_Click(object sender,
										 EventArgs e)
		=> RegisterPlayers();

	private void UnregisterAllButton_Click(object sender,
										   EventArgs e)
		=> UnregisterPlayers();

	private void RegisterPlayers(IEnumerable<SeedablePlayer>? players = null)
	{
		var registeringPlayers = (players ?? UnregisteredDataGridView.GetAll<SeedablePlayer>()).ToArray();
		registeringPlayers.ForEach(seedable => Round.AddPlayer(seedable.Player));
		FillPlayerLists(ListsToFill.Unseeded);
		if (players is null)
			UnregisterAllButton.Focus();
		else
		{
			int[] playerIds = [..registeringPlayers.Select(static player => player.Id)];
			foreach (DataGridViewRow row in RegisteredDataGridView.Rows)
				row.Selected = playerIds.Contains(row.GetFromRow<SeedablePlayer>().Id);
			RegisterForRoundButton.Focus();
		}
		SetButtonUsability();
	}

	private void UnregisterPlayers(IEnumerable<SeedablePlayer>? players = null)
	{
		var unregisteringPlayers = (players ?? RegisteredDataGridView.GetAll<SeedablePlayer>()).ToArray();
		int[] playerIds = [..unregisteringPlayers.Select(static player => player.Id)];
		var preregistered = Tournament.TournamentPlayers
									  .Where(tp => playerIds.Contains(tp.PlayerId)
												&& tp.RegisteredForRound(Round.Number))
									  .ToArray();
		var numberPreregistered = preregistered.Length;
		if (numberPreregistered > 0
		&&  MessageBox.Show($"The {(numberPreregistered is 1
										? "player below is"
										: $"{numberPreregistered} players below are")} preregistered for this round:{preregistered.Select(static tp => tp.Player)
																																  .BulletList()}{NewLine}{NewLine}Cancel preregistration?",
							"Confirm Preregistration Cancellation",
							YesNo,
							Warning) is DialogResult.No)
			return;
		preregistered.ForEach(tournamentPlayer => tournamentPlayer.UnregisterForRound(Round.Number));
		UpdateMany(preregistered);
		Delete(unregisteringPlayers.Select(seedable => Round.RoundPlayers.ByPlayerId(seedable.Id)));
		FillPlayerLists(ListsToFill.Unseeded);
		if (players is null)
		{
			foreach (DataGridViewRow row in UnregisteredDataGridView.Rows)
				row.Selected = playerIds.Contains(row.GetFromRow<SeedablePlayer>().Id);
			RegisterForRoundButton.Focus();
		}
		else
			RegisterAllButton.Focus();
		SetButtonUsability();
	}

	private void SeededDataGridView_SelectionChanged(object sender,
													 EventArgs e)
	{
		if (SkippingHandlers)
			return;
		SkipHandlers(() =>
        {
			FindPlayerTextBox.Clear();
			UnregisteredDataGridView.Deselect();
        });
		SetButtonUsability();
	}

	private void MoveGameButton_Click(object sender,
									  EventArgs e)
	{
		//	TODO: ask if an email should go out to the players telling them they are moving tables?
		var selectedRow = SeededDataGridView.CurrentRow.OrThrow();
		var whichRow = selectedRow.Index;
		var firstIndex = whichRow / 7;
		var direction = sender == MoveGameUpButton
							? -1
							: +1;
		var otherIndex = firstIndex + direction;
		var movingGame = Round.Games[firstIndex];
		var otherGame = Round.Games[otherIndex];
		//	Yes, do this as three updates, NOT a delete/create because the
		//	GamePlayers do not move with the Games if the Games get new Ids
		movingGame.Number = 0;
		UpdateOne(movingGame);
		movingGame.Number = otherGame.Number;
		otherGame.Number -= direction;
		UpdateMany(otherGame, movingGame);
		FillPlayerLists(ListsToFill.Seeded);
		whichRow += direction * 7;
		//	NOTE: Setting .CurrentCell is the only thing that seems to work.
		//	Don't set Row[x].Selected = true or Cell[x].Selected = true, etc.
		SeededDataGridView.CurrentCell = SeededDataGridView.Rows[whichRow]
														   .Cells[0];
	}

	private void SwapButton_Click(object sender,
								  EventArgs e)
	{
		if (SeededDataGridView.SelectedRows.Count is not 2)
			throw new InvalidOperationException(); //	TODO
		var selected = SeededDataGridView.SelectedRows
										 .Cast<DataGridViewRow>()
										 .Select(static row => row.Index)
										 .ToArray();
		var gamePlayers = SeededDataGridView.GetMultiSelected<GamePlayer>()
											.ToArray();
		//	TODO: No idea why, but if the swapping players are in different games, the swap must be a Delete+Create
		var sameGame = SeededDataGridView.SelectedRows[0].Index / 7 == SeededDataGridView.SelectedRows[1].Index / 7;
		if (!sameGame)
			Delete(gamePlayers);
		//	TODO: Are we okay just swapping the .Player?  DURING seeding, we have to swap .Game and .Power instead....
		(gamePlayers[0].Player, gamePlayers[1].Player) = (gamePlayers[1].Player, gamePlayers[0].Player);
		if (sameGame)
			UpdateMany(gamePlayers);
		else
			CreateMany(gamePlayers);
		//	We need to (re-)run PrepareForSeeding and recalculate the conflict for all players in affected games.
		foreach (var gamePlayer in gamePlayers)
		{
			gamePlayer.PrepareForSeeding();
			gamePlayer.Game.GamePlayers.ForEach(static participant => participant.CalculateConflict());
			if (sameGame)
				break;
		}
		FillPlayerLists(ListsToFill.Seeded);
		//	NOTE: I know I said that didn't work in the method above, but it works here.  Weird!
		selected.ForEach(rowNumber => SeededDataGridView.Rows[rowNumber].Selected = true);
	}

	private void ReplaceButton_Click(object sender,
									 EventArgs e)
	{
		if (SeededDataGridView.SelectedRows.Count is not 1
		|| RegisteredDataGridView.SelectedRows.Count is not 1)
			throw new InvalidOperationException(); //	TODO
		var whichRow = SeededDataGridView.SelectedRows
										 .Cast<DataGridViewRow>()
										 .Select(static row => row.Index)
										 .Single();
		var registeredPlayer = RegisteredDataGridView.GetSelected<SeedablePlayer>();
		var gamePlayer = SeededDataGridView.GetAtIndex<GamePlayer>(whichRow);
		var formerPrimaryKey = gamePlayer.PrimaryKey;
		var gamePlayerId = gamePlayer.PlayerId;
		gamePlayer.Player = registeredPlayer.Player;
		//	If for some reason the seeded player has no RoundPlayer
		//	record (legacy only!), create the RoundPlayer record.
		if (Round.RoundPlayers.All(roundPlayer => roundPlayer.PlayerId != gamePlayerId))
			Round.AddPlayer(gamePlayer.Player);
		UpdateOne(gamePlayer, formerPrimaryKey);
		FillPlayerLists(ListsToFill.Seedable);
		foreach (DataGridViewRow row in RegisteredDataGridView.Rows)
			row.Selected = row.GetFromRow<SeedablePlayer>().Id == gamePlayerId;
		SeededDataGridView.CurrentCell = SeededDataGridView.Rows[whichRow]
														   .Cells[0];
	}

	#region Seeding methods

	private void SeedButton_Click(object sender,
								  EventArgs e)
	{
		var startedGames = Round.StartedGames
								.Length;
		var selected = new int[RegisteredDataGridView.SelectedRows.Count];
		RegisteredDataGridView.SelectedRows
							  .Cast<DataGridViewRow>()
							  .Select(static row => row.Index)
							  .ToArray()
							  .CopyTo(selected, 0);
		var roundPlayerIds = RegisteredDataGridView.GetAll<SeedablePlayer>()
												   .Where((_, index) => sender == SeedAllButton
																	 || selected.Contains(index))
												   .Select(static seededPlayer => seededPlayer.Id)
												   .ToArray();
		var roundPlayers = Round.RoundPlayers
								.Where(roundPlayer => roundPlayerIds.Contains(roundPlayer.PlayerId))
								.ToList();
		if (roundPlayers.Count % 7 > 0)
			throw new InvalidOperationException(); //	TODO
		var seededGameCount = roundPlayers.Count / 7;
		var preseededGameCount = Round.SeededGames
									  .Length;
		var totalSeededGameCount = preseededGameCount + seededGameCount;

		int seededGamesConflict;
		long? elapsedMilliseconds;
		using (var form = new WaitForm($"Seeding {"Game".Pluralize(totalSeededGameCount, true)}",
									   () => Round.Seed(roundPlayers, SeedingAssignsPowersCheckBox.Checked)))
		{
			form.ShowDialog(this);
			seededGamesConflict = form.Result;
			elapsedMilliseconds = form.ElapsedMilliseconds;
		}
		var allGamesConflict = Round.Conflict;
		var extraInfo = startedGames is 0
							? null
							: $"{NewLine}{NewLine}Conflict for the seeded {GameCount(totalSeededGameCount, false)}: {seededGamesConflict.Points()}";
		FillPlayerLists(ListsToFill.Seedable);
		SetButtonUsability();
		var timingData = elapsedMilliseconds is null
							 ? null
							 : $"{NewLine}{NewLine}Time to seed: {elapsedMilliseconds / 1000m} sec.";
		var preseedInfo = preseededGameCount is 0
							  ? null
							  : $" and re-seeded {GameCount(preseededGameCount)}";
		MessageBox.Show($"Seeded {GameCount(seededGameCount)}{preseedInfo}.{extraInfo}{NewLine}{NewLine}" +
						$"Total conflict for round: {allGamesConflict.Points()}{timingData}",
						"Seeding complete",
						OK,
						Information);

		static string GameCount(int count,
								bool includeCount = true)
			=> "game".Pluralize(count, includeCount);
	}

	#endregion

	private void WhichPlayersTabControl_SelectedIndexChanged(object sender,
															 EventArgs e)
	{
		int[] playerIds = [..UnregisteredDataGridView.GetMultiSelected<SeedablePlayer>()
													 .Select(static player => player.Id)];
		FillPlayerLists(ListsToFill.Unregistered);
		foreach (DataGridViewRow row in UnregisteredDataGridView.Rows)
			row.Selected = playerIds.Contains(UnregisteredDataGridView.GetAtIndex<SeedablePlayer>(row.Index).Id);
	}

	private void ViewGamesButton_Click(object sender,
									   EventArgs e)
	{
		var selectedGame = SeededDataGridView.SelectedRows.Count is 0
							   ? (int?)null
							   : SeededDataGridView.SelectedRows[0].Index / 7;
		Show<GamesForm>(() => new (Round, selectedGame),
						_ =>
						{
							FillPlayerLists(ListsToFill.Seeded);
							if (selectedGame is not null)
								SeededDataGridView.FirstDisplayedScrollingRowIndex = selectedGame.Value * 7;
						});
	}

	private void StartGamesButton_Click(object sender,
										EventArgs e)
	{
		var unstartedGames = Round.SeededGames
								  .ToList();
		if (unstartedGames.Count is 0)
			return;
		var emailToSend = Settings.AssignmentEmailTemplate?.Length > 0;
		var games = "game".Pluralize(unstartedGames);
		var howMany = unstartedGames.Count is 1
						  ? "the"
						  : "all";
		var choice = MessageBox.Show(emailToSend
										 ? $"Email board assignments to players in {howMany} starting {games}?{NewLine}{NewLine}" +
										   $"Click Yes to start the {games} and email assignments to the players.{NewLine}" +
										   $"Click No to start the {games} without sending player emails.{NewLine}" +
										   $"Click Cancel to keep the {games} unstarted."
										 : $"Start {howMany} unstarted {games} in Round {Round}?",
									 "Confirm Game Start",
									 emailToSend
										 ? YesNoCancel
										 : YesNo,
									 Question);
		if (choice is DialogResult.Cancel || choice is DialogResult.No && !emailToSend)
			return;
		unstartedGames.ForEach(static game => game.Status = Underway);
		UpdateMany(unstartedGames);
		FillPlayerLists(ListsToFill.Seeded);
		SetButtonUsability();
		if (emailToSend)
			EmailAssignments();

		void EmailAssignments()
		{
			var template = Settings.AssignmentEmailTemplate
								   .Replace("{TournamentName}", Tournament.Name)
								   .Replace("{RoundNumber}", $"{Round}");
			var messages = new List<MailMessage>();
			foreach (var game in unstartedGames)
			{
				var assignments = """
								  <table style="margin:auto; border: solid 1 black;">
								  """;
				foreach (var gamePlayer in game.GamePlayers)
				{
					var power = gamePlayer.Power;
					assignments += $"""
									<tr>
									    <td>{gamePlayer.Player}</td>
									    <th {power.CellStyle().Tag()}>{power.InCaps()}</th>
									</tr>
									""";
				}
				assignments += "</table>";
				var tournamentName = Tournament.Name;
				var emailBody = template.Replace("{GameNumber}", $"{game.Number}")
										.Replace("{Assignments}", assignments);
				messages.AddRange(game.GamePlayers
									  .Where(static gamePlayer => gamePlayer.Player.EmailAddress.Length > 0)
									  .Select(gamePlayer => WriteEmail($"Round {Round} Board Assignment",
																	   emailBody.Replace("{PlayerName}", gamePlayer.Player.Name)
																				.Replace("{PowerName}", gamePlayer.Power is TBD
																											? null
																											: $"{gamePlayer.Power}"),
																	   gamePlayer.Player,
																	   tournamentName)));
			}
			//	TODO: maybe put up a "Waiting..." modal here?
			var errors = SendEmail([..messages]);
			//	TODO: ...and take it down here?
			var hasErrors = errors.Length > 0;
			MessageBox.Show(hasErrors
								? $"Errors sending email:{errors.BulletList()}"
								: "Emails sent successfully.",
							$"Assignment Email {(hasErrors ? "Error" : "Success")}",
							OK,
							hasErrors
								? Error
								: Information);
		}
	}

	private void ChangeRoundButton_Click(object sender,
										 EventArgs e)
	{
		if (Round.GamesStarted is false)
			//	Can only discard a round if there are no started games
			RoundInfoForm.DiscardRound();
		else if (Round.GamesSeeded is not true)
			//	Can only start a new round if there are started but no seeded games
			RoundInfoForm.StartNewRound();
	}

	internal void DiscardRound()
		=> SkipHandlers(() =>
						{
							UnseedButton_Click();
							Delete(Round.RoundPlayers);
							Delete(Round);
						});

	private void SeedableDataGridView_DataBindingComplete(object sender,
														  DataGridViewBindingCompleteEventArgs e)
	{
		var view = (DataGridView)sender;
		view.FillColumn(0);
		view.AlignColumn(MiddleRight, 1);
		view.Columns[1].Visible = SortByScoreCheckBox.Visible
								  && SortByScoreCheckBox.Checked;
		view.AlternatingRowsDefaultCellStyle.BackColor = view.Columns[1].Visible
															 ? SystemColors.ControlLight
															 : view.DefaultCellStyle
																   .BackColor;
	}

	private void PrintButton_Click(object sender,
								   EventArgs e)
		=> SeededDataGridView.Print(Tournament.Name,
									$"Seeding for Round {Round}");

	private void ComboBox_EnabledChanged(object sender,
										 EventArgs e)
		=> sender.ToggleEnabled();

	private void FindPlayerTextBox_TextChanged(object sender,
											   EventArgs e)
	{
		var text = FindPlayerTextBox.Text;
		if (text.Length is 0)
			return;
		var row = SeededDataGridView.Rows
									.Cast<DataGridViewRow>()
									.FirstOrDefault(record => Regex.IsMatch(record.GetFromRow<GamePlayer>().Player.Name,
																			text,
																			RegexOptions.IgnoreCase));
		if (row is null)
			return;
		SkipHandlers(() =>
        {
			SeededDataGridView.ClearSelection();
			row.Selected = true;
        });
		SeededDataGridView.FirstDisplayedScrollingRowIndex = row.Index;
		SetButtonUsability();
	}

	private void NewPlayerButton_Click(object sender, EventArgs e)
		=> Show<PlayerInfoForm>(form =>
								{
									var newPlayer = form.Player;
									if (newPlayer.Id > 0)
										RegisterPlayers([new (Tournament, newPlayer, Round.Number)]);
								});

	/// <summary>
	///     Determines if this ScoringSystem (given a set of FinishedGames in a Tournament or Round
	///     using this ScoringSystem) can be changed to another without leaving any of those games in
	///     a "did not report all the things that the new scoring system will need" situation.  In the
	///     case of a change to a main Tournament scoring system, all such games MUST be put back into
	///     the Underway state if the change is made, since the main Tournament system is always used
	///     for the calculation of Best Game (Best Austria, etc.) scores.  In the case of changing a
	///     Round (not Tournament) system, any finished games COULD be left finished and retaining
	///     their scoring system (rather than taking on the proposed new system) since they obviously
	///     are okay for Best Game calculation the way they are.
	/// </summary>
	/// <param name="currentScoringSystem" />
	/// <param name="proposedScoringSystem" />
	/// <param name="finishedGames">
	///     A parameter that should contain all Games to be checked to see if they need to be
	///     updated in some way if the change is approved.  It will be returned populated with only
	///     those games that do.  It is the caller's responsibility to update them appropriately.
	/// </param>
	/// <param name="allowKeepingCurrentSystem">
	///     false if the caller expects only Yes and Cancel (but not No) responses from this method
	/// </param>
	/// <returns>
	///     Cancel, if the user said NOT to make the proposed ScoringSystem change.
	///     Yes, if the change can be made and the "lacking" games (if any) should
	///     be put back into Underway status.
	///     No, if the change can be made and the "lacking" games should be kept in
	///     Finished state and told to retain their current scoring systems.
	/// </returns>
	public static DialogResult CheckScoringSystemChange(ScoringSystem currentScoringSystem,
														ScoringSystem proposedScoringSystem,
														Game[] finishedGames,
														bool allowKeepingCurrentSystem = false)
	{
		//	This if isn't really necessary, since filtering the finishedGames (below) will answer
		//	the same question, but in a tournament with a lot of games, this could be faster.
		if ((currentScoringSystem.UsesGameResult || !proposedScoringSystem.UsesGameResult)
			&& (currentScoringSystem.UsesCenterCount || !proposedScoringSystem.UsesCenterCount)
			&& (currentScoringSystem.UsesYearsPlayed || !proposedScoringSystem.UsesYearsPlayed)
			&& (currentScoringSystem.UsesOtherScore || !proposedScoringSystem.UsesOtherScore))
			return DialogResult.Yes;
		finishedGames =
		[
			..finishedGames.Where(game =>
								  {
									  var system = game.ScoringSystem;
									  return !system.UsesGameResult && proposedScoringSystem.UsesGameResult
											 || !system.UsesCenterCount && proposedScoringSystem.UsesCenterCount
											 || !system.UsesYearsPlayed && proposedScoringSystem.UsesYearsPlayed
											 || !system.UsesOtherScore && proposedScoringSystem.UsesOtherScore;
								  })
		];
		var count = finishedGames.Length;
		if (count is 0)
			return DialogResult.Yes;
		var singular = count is 1;
		var extraText = allowKeepingCurrentSystem
							? $". Do you want to move {(singular ? "this game" : "these games")} back to the Underway status? " +
							  $"(If you answer No, the {"game".Pluralize(count)} will remain Finished " +
							  $"and{(singular ? null : " each")} will retain its current scoring system.)"
							: ", and that will be moved back to Underway status if you proceed. Continue?";
		var answer = MessageBox.Show($"There {(singular ? "is a" : "are")} {"finished game".Pluralize(count, !singular)} that {(singular ? "has" : "have")} " +
									 $"been recorded without sufficient data for this scoring system{extraText}",
									 "Confirm Tournament Scoring System Change",
									 allowKeepingCurrentSystem
										 ? YesNoCancel
										 : YesNo,
									 Warning);
		return !allowKeepingCurrentSystem && answer is DialogResult.No
				   ? DialogResult.Cancel
				   : answer;
	}

	[Flags]
	private enum ListsToFill : byte
	{
		Unregistered = 1, // << 0,
		Registered = 1 << 1,
		Seeded = 1 << 2,
		Unseeded = Unregistered | Registered,
		Seedable = Registered | Seeded,
		All = Unseeded | Seeded
	}

	#region SeedablePlayer class

	//	NOTE: Don't put this in a separate file in another part of this partial class;
	//	If you do, VS will think it has a visual Form and will set up a designer file.

	[PublicAPI]
	private sealed class SeedablePlayer : IRecord
	{
		internal readonly Player Player;

		internal readonly double ScoreBeforeRound;

		public string PlayerName => $"{Player}{(Preregistered ? " ✅" : null)}"; // or ✔ or ✓

		public string Score => ScoreBeforeRound.Points();

		internal int Id => Player.Id;

		private bool Preregistered { get; }

		/// <summary>
		/// </summary>
		/// <param name="tournament" />
		/// <param name="player" />
		/// <param name="roundNumber">
		///     Show a check-mark next to the player name if the player is registered for this round.
		///     0 means show the check-mark if the player is registered for ANY round.
		///     null means do not show the check-mark at all.
		/// </param>
		internal SeedablePlayer(Tournament tournament,
								Player player,
								int? roundNumber)
		{
			Player = player;
			ScoreBeforeRound = roundNumber is 0 or null
								   ? 0
								   : tournament.Rounds[roundNumber.Value - 1].PreRoundScore(player);
			var tournamentPlayer = tournament.TournamentPlayers
											 .SingleOrDefault(tp => tp.PlayerId == player.Id);
			Preregistered = roundNumber is not null
						 && tournamentPlayer is not null
						 && (roundNumber is 0 || tournamentPlayer.RegisteredForRound(roundNumber.Value));
		}
	}

	#endregion

	#region GameNumber struct

	private readonly record struct GameNumber(Game Game, int Number);

	#endregion

	#region Unseeding methods

	private void UnseedButton_Click(object? sender = null,
									EventArgs? e = null)
	{
		if (!SkippingHandlers)
			UnseedGames(Round.SeededGames);
	}

	private void UnseedGameButton_Click(object sender,
										EventArgs e)
	{
		if (SeededDataGridView.SelectedRows.Count is not 1)
			throw new InvalidOperationException(); //	TODO
		UnseedGames(SeededDataGridView.GetSelected<GamePlayer>().Game);
	}

	private void UnseedGames(params Game[] games)
	{
		if (games.Length is 0 || games.Any(static game => game.Status is not Seeded))
			throw new InvalidOperationException(); //	TODO
		if (MessageBox.Show($"Are you sure you want to unseed the {"seeded game".Pluralize(games, true)}?",
							"Confirm Game Unseeding",
							YesNo,
							Question) is DialogResult.No)
			return;
		Delete(games.SelectMany(static game => game.GamePlayers));
		Delete(games);
		//	Renumber remaining games as necessary
		var movingGames = Round.Games
							   .Select(static (game, index) => new GameNumber(game, index + 1))
							   .Where(static a => a.Game.Number > a.Number)
							   .ToList();
		movingGames.ForEach(static moving => moving.Game.Number = moving.Number);
		UpdateMany(movingGames.Select(static moving => moving.Game));
		//	Refill the right two player lists.
		FillPlayerLists(ListsToFill.Seedable);
		SetButtonUsability();
	}

	#endregion
}
