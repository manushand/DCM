namespace DCM.UI.Forms;

internal sealed partial class GroupGamesForm : Form
{
	private Game? _game;
	private Group Group { get; }

	private Tournament HostTournament { get; }

	private Round HostRound { get; }

	private Game Game => _game.OrThrow();

	private List<GamePlayer> GamePlayers => [..Game.GamePlayers];

	private List<ComboBox> PlayerComboBoxes { get; }

	private bool AllPlayersAssigned => PlayerComboBoxes.All(static box => box.SelectedItem is not null);

	private List<Label> ScoreLabels { get; }

	private bool AllFilledIn { get; set; }

	internal GroupGamesForm(Group group)
	{
		InitializeComponent();
		Group = group;
		GameControl.GameDataChangedCallback = GameDataUpdated;
		var scoringSystem =
			GameControl.ScoringSystem =
				Group.ScoringSystem
					 .OrThrow();
		HostTournament = Group.Tournament
					  ?? CreateOne(new Tournament
								   {
									   Name = $"{Group} Group Games",
									   Group = Group
								   });
		HostRound = HostTournament.Rounds
								  .SingleOrDefault()
				 ?? HostTournament.AddRound();
		PlayerComboBoxes = PlayersPanel.PowerControls<ComboBox>();
		ScoreLabels = ScoresPanel.PowerControls<Label>();
		DeleteGameButton.Enabled = false;
		if (scoringSystem.PointsPerGame is 0 || scoringSystem.UsesPlayerAnte)
			RescoreGamesFrom(default);
	}

	private void GroupGamesForm_Load(object sender,
									 EventArgs e)
	{
		Text = $"{Group} ─ Group Games";
		FirstNameRegistrationTabRadioButton.Checked = true;
		ShowControls(false);
		NameRegistrationTabRadioButton_CheckedChanged();
		FillGameList();
	}

	private void FillGameList()
	{
		GroupGamesDataGridView.FillWith(Group.Games
											 .Select(static game => new GroupGame(game))
											 .Reverse()
											 .ToArray());
		ShowControls(_game is not null);
		if (_game is null)
		{
			GroupGamesDataGridView.Deselect();
			NewGameButton.Text = "New Game";
			return;
		}
		DeleteGameButton.Text = "Delete Game";
		GroupGamesDataGridView.Rows
							  .Cast<DataGridViewRow>()
							  .Single(each => each.GetFromRow<GroupGame>().Id == Game.Id)
							  .Selected = true;
	}

	private void NameRegistrationTabRadioButton_CheckedChanged(object? sender = null,
															   EventArgs? e = null)
	{
		var players = Group.Players
						   .OrderBy(player => FirstNameRegistrationTabRadioButton.Checked
												  ? player.Name
												  : player.LastFirst)
						   .ToArray();
		PlayerComboBoxes.ForEach(box =>
								 {
									 var selected = box.SelectedItem;
									 box.FillWithRecords(players);
									 box.SelectedItem = selected;
								 });
	}

	private void PlayerComboBox_SelectedIndexChanged(object sender,
													 EventArgs e)
	{
		var playerComboBox = (ComboBox)sender;
		playerComboBox.UpdateShadowLabel();
		if (SkipHandlers || playerComboBox.SelectedItem is null)
			return;
		var playerId = playerComboBox.GetSelected<Player>()
									 .Id;
		PlayerComboBoxes.ForSome(otherBox => otherBox != playerComboBox
										  && otherBox.SelectedItem is not null
										  && otherBox.GetSelected<Player>().Id == playerId,
								 static box => box.Deselect());
		GameStatusLabel.Visible =
			GameStatusComboBox.Visible =
				AllPlayersAssigned;
		if (GameStatusLabel.Visible)
			GameStatusComboBox.SelectedIndex = 0;
	}

	private void ShowControls(bool visible)
	{
		Width = GroupGamesDataGridView.Width + 45 + (visible
														 ? GameControl.Width + PlayerColumnHeaderLabel.Width
																			 + ScoreColumnHeaderLabel.Width
																			 + 25 //	 TODO: check
														 : 0);
		GameControl.Visible =
			PlayerColumnHeaderLabel.Visible =
				GameStatusLabel.Visible =
					GameStatusComboBox.Visible =
						GameNameLabel.Visible =
							GameNameTextBox.Visible =
								GameDateLabel.Visible =
									GameDateTimePicker.Visible =
										visible;
		PlayerComboBoxes.ForEach(box => box.Enabled = _game is null);
		var scored = _game?.Status is Finished;
		ScoreColumnHeaderLabel.Visible =
			ScoreTotalBarLabel.Visible =
				TotalScoreTextLabel.Visible =
					TotalScoreLabel.Visible =
						scored;
		ScoreLabels.ForEach(label => label.Visible = scored);
		OrderByNamePanel.Visible = GameStatusComboBox.SelectedIndex < 1;
	}

	private void NewGameButton_Click(object? sender = null,
									 EventArgs? e = null)
	{
		//	New Game
		ShowControls(true);
		GroupGamesDataGridView.Deselect();
		GroupGamesDataGridView.Enabled =
			NewGameButton.Enabled =
				ScoreColumnHeaderLabel.Visible =
					ScoreTotalBarLabel.Visible =
						TotalScoreTextLabel.Visible =
							TotalScoreLabel.Visible =
								GameInErrorButton.Visible =
									false;

		GameControl.Active = true;
		ScoreLabels.ForEach(static label => label.Visible = false);
		_game = null;
		GameControl.ClearGame();
		GameNameTextBox.Text = null;
		PlayerComboBoxes.ForEach(static box =>
								 {
									 box.Deselect();
									 box.Enabled = true;
								 });
		GameDateTimePicker.Value = DateTime.Today;
		OrderByNamePanel.Visible =
			DeleteGameButton.Enabled =
				true;
		DeleteGameButton.Text = "Cancel New Game";
		GameStatusLabel.Visible =
			GameStatusComboBox.Visible =
				AllPlayersAssigned;
	}

	private void DeleteGame()
	{
		Delete(Game.GamePlayers);
		Delete(Game);
		_game = null;
		FillGameList();
	}

	private void GameStatusComboBox_SelectedIndexChanged(object sender,
														 EventArgs e)
	{
		if (SkipHandlers)
			return;
		switch (GameStatusComboBox.SelectedIndex)
		{
		case 0 when _game is not null:
			if (MessageBox.Show("Do you really want to re-seed this group game?",
								"Confirm Game Re-Seeding",
								YesNo,
								Question) is DialogResult.No)
				return;
			DeleteGame();
			NewGameButton_Click();
			break;
		case 2 when _game is null || !AllFilledIn:
			SkipHandlers = true;
			GameStatusComboBox.SelectedIndex = 1;
			SkipHandlers = false;
			if (_game is null)
				goto CreateGame;
			break;
		case 1 when _game is null:
		CreateGame:
			_game = CreateOne(new Game
							  {
								  Round = HostRound,
								  Number = Group.Games.Length + 1,
								  Name = GameNameTextBox.Text
														.Trim(),
								  Date = GameDateTimePicker.Value
							  });
			CreateMany(Range(0, 7).Select(index => new GamePlayer
												   {
													   Game = Game,
													   Player = PlayerComboBoxes[index].GetSelected<Player>(),
													   Power = index.As<PowerNames>(),
													   Result = Unknown
												   }));
			FillGameList();
			NewGameButton.Enabled =
				DeleteGameButton.Enabled =
					GroupGamesDataGridView.Enabled =
						true;
			DeleteGameButton.Text = "Delete Game";
			GameControl.LoadGame(GamePlayers);
			break;
		case 0:
		case 1:
		case 2:
			break;
		default:
			throw new (); //	TODO
		}
		ScoresPanel.Visible =
			ScoreColumnHeaderLabel.Visible =
				ScoreTotalBarLabel.Visible =
					TotalScoreTextLabel.Visible =
						TotalScoreLabel.Visible =
							GameStatusComboBox.SelectedIndex is 2;
		GameControl.Active = GameStatusComboBox.SelectedIndex is 1;
		var seeding = GameStatusComboBox.SelectedIndex is 0;
		OrderByNamePanel.Visible = seeding;
		PlayerComboBoxes.ForEach(box => box.Enabled = seeding);
		if (GameStatusComboBox.SelectedIndex is 0)
			return;
		Game.Status = GameStatusComboBox.SelectedIndex
										.As<Statuses>();
		UpdateOne(Game);
		//  If the game just got changed to Finished and the scoring system
		//  uses a player ante, then scores have not yet been calculated.
		if (Game.Status is Finished && Group.ScoringSystem?.UsesPlayerAnte is true)
			GameDataUpdated(true);
	}

	private void RescoreGamesFrom(DateTime gameDate)
	{
		foreach (var game in Group.Games
								  .Where(scoreable => scoreable.Date >= gameDate))
			game.Scored = false;
	}

	private void GroupGamesDataGridView_CellClick(object? sender = null,
												  DataGridViewCellEventArgs? e = null)
	{
		_game = GroupGamesDataGridView.GetSelected<GroupGame>()
									  .Game;
		SkipHandlers = true;
		GameDateTimePicker.Value = Game.Date;
		GameNameTextBox.Text = Game.Name;
		GameStatusComboBox.SelectedIndex = Game.Status
											   .AsInteger();
		PlayerComboBoxes.Apply((box, index) => box.SetSelectedItem(GamePlayers[index].Player));
		SkipHandlers = false;
		ShowControls(true);
		GameControl.ClearGame();
		GameControl.LoadGame(GamePlayers);
		GameControl.Active = GameStatusComboBox.SelectedIndex is 1;
		DeleteGameButton.Enabled = true;
	}

	private void GroupGamesDataGridView_DataBindingComplete(object sender,
															DataGridViewBindingCompleteEventArgs e)
		=> GroupGamesDataGridView.FillColumn(1);

	private void DeleteGameButton_Click(object sender,
										EventArgs e)
	{
		if (_game is null)
		{
			//	Cancel New Game
			ShowControls(false);
			NewGameButton.Enabled =
				GroupGamesDataGridView.Enabled =
					true;
			DeleteGameButton.Text = "Delete Game";
		}
		else if (MessageBox.Show($"Do you really want to delete this group game dated {Game.Date:d}?",
								 "Confirm Game Deletion",
								 YesNo,
								 Question) is DialogResult.Yes)
			DeleteGame();
		else
			return;
		DeleteGameButton.Enabled = false;
	}

	private void GameDateTimePicker_ValueChanged(object sender,
												 EventArgs e)
	{
		if (SkipHandlers || _game is null)
			return;
		var newDate = GameDateTimePicker.Value;
		var earlier = Game.Date < newDate
						  ? Game.Date
						  : newDate;
		Game.Date = newDate;
		UpdateOne(Game);
		//	BUG: This doesn't seem to work to change player scores like I would expect....??
		if (Group.ScoringSystem?.UsesPlayerAnte is true)
			RescoreGamesFrom(earlier);
		FillGameList();
		GameDataUpdated(AllFilledIn);
		GroupGamesDataGridView.SelectRowWhere<GroupGame>(groupGame => groupGame.Id == Game.Id);
		GroupGamesDataGridView_CellClick();
	}

	private void GameNameTextBox_Leave(object sender,
									   EventArgs e)
	{
		if (SkipHandlers || _game is null)
			return;
		Game.Name = GameNameTextBox.Text;
		UpdateOne(Game);
		FillGameList();
	}

	private void GameDataUpdated(bool allFilledIn)
	{
		AllFilledIn = allFilledIn;
		if (SkipHandlers || _game is null)
			return;
		UpdateMany(GamePlayers);
		//	If all GamePlayers are Completely filled in, but we were told NOT allFilledIn,
		//	this means that FinalGameDataValidation failed. Show the GameInErrorButton.
		GameInErrorButton.Visible = !allFilledIn
								 && GamePlayers.All(static gamePlayer => gamePlayer.PlayComplete);
		AllFilledIn = allFilledIn && !GameInErrorButton.Visible;
		var showScores = allFilledIn
					  && (Game.Status is Finished
					   || Group.ScoringSystem?.UsesPlayerAnte is not true);
		ScoreColumnHeaderLabel.Visible =
			ScoreTotalBarLabel.Visible =
				TotalScoreTextLabel.Visible =
					TotalScoreLabel.Visible =
						showScores;
		ScoreLabels.ForEach(label => label.Visible = showScores);
		if (!showScores)
			return;
		//	Calculate the scores
		if (Game.Scored && Game.ScoringSystemId == Group.ScoringSystemId
		 || Game.CalculateScores(out var errors, Group.ScoringSystem))
			Game.FillFinalScores(ScoreLabels, TotalScoreLabel, ToolTip);
		else
			MessageBox.Show(errors.OfType<string>().BulletList(),
							"Game in Error",
							OK,
							Warning);
	}

	private void ComboBox_EnabledChanged(object sender,
										 EventArgs e)
		=> sender.ToggleEnabled();

	//	TODO: This method is duplicated verbatim in GamesForm.cs
	private void GameInErrorButton_Click(object sender,
										 EventArgs e)
	{
		GameControl.FinalGameDataValidation(out var error);
		MessageBox.Show(error,
						"Game in Error",
						OK,
						Warning);
	}

	#region GroupGame struct

	[PublicAPI]
	private readonly record struct GroupGame(Game Game) : IRecord
	{
		public string Date => $"{DateTime:d}";

		public string Name => Game.Name;

		internal int Id => Game.Id;

		internal DateTime DateTime => Game.Date;
	}

	#endregion
}
