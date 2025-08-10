namespace PC.Forms;

internal sealed partial class GroupGamesForm : Form
{
	#region Public interface

	#region Constructor

	internal GroupGamesForm(Group group)
	{
		InitializeComponent();
		Group = group;
		GameControl.GameDataChangedCallback = GameDataUpdated;
		var scoringSystem =
			GameControl.ScoringSystem =
				Group.ScoringSystem;
		HostRound = Group.HostRound;
		PlayerComboBoxes = PlayersPanel.PowerControls<ComboBox>();
		ScoreLabels = ScoresPanel.PowerControls<Label>();
		ScoreDisplayControls = [ScoreColumnHeaderLabel, ScoreTotalBarLabel, TotalScoreTextLabel, ScoresPanel, TotalScoreLabel];
		DeleteGameButton.Enabled = false;
		if (scoringSystem.PointsPerGame is 0 || scoringSystem.UsesPlayerAnte)
			RescoreGamesFrom(default);
	}

	#endregion

	#endregion

	#region Private implementation

	#region Type

	[PublicAPI]
	private sealed record GroupGame : IRecord
	{
		public string Date => $"{DateTime:d}";

		public string Name => Game.Name;

		[Browsable(false)]
		internal int Id => Game.Id;

		[Browsable(false)]
		internal DateTime DateTime => Game.Date;

		[Browsable(false)]
		internal readonly Game Game;

		internal GroupGame(Game game)
			=> Game = game;
	}

	#endregion

	#region Data

	private Game Game { get; set; } = Game.None;
	private bool AllFilledIn { get; set; }
	private Group Group { get; }
	private Round HostRound { get; }
	private Control[] ScoreDisplayControls { get; }
	private List<Label> ScoreLabels { get; }
	private List<ComboBox> PlayerComboBoxes { get; }

	private List<GamePlayer> GamePlayers => [..Game.GamePlayers];
	private bool AllPlayersAssigned => PlayerComboBoxes.All(static box => box.SelectedItem is not null);

	#endregion

	#region Event handlers

	private void GroupGamesForm_Load(object sender,
									 EventArgs e)
	{
		Text = $"{Group} ─ Group Games";
		FirstNameRegistrationTabRadioButton.Checked = true;
		ShowControls(false);
		NameRegistrationTabRadioButton_CheckedChanged();
		FillGameList();
	}

	private void NameRegistrationTabRadioButton_CheckedChanged(object? sender = null,
															   EventArgs? e = null)
	{
		Player[] players = [..Group.Players
								   .OrderBy(player => FirstNameRegistrationTabRadioButton.Checked
														  ? player.Name
														  : player.LastFirst)];
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
		if (SkippingHandlers || playerComboBox.SelectedItem is null)
			return;
		var playerId = playerComboBox.GetSelected<Player>()
									 .Id;
		PlayerComboBoxes.ForSome(otherBox => otherBox != playerComboBox
										  && otherBox.SelectedItem is not null
										  && otherBox.GetSelected<Player>().Id == playerId,
								 static box => box.Deselect());
		SetVisible(AllPlayersAssigned, GameStatusLabel, GameStatusComboBox);
		//  TODO - Is there some reason this cannot be "if (AllPlayersAssigned)"?
		if (!GameStatusLabel.Visible)
			return;
		GameStatusComboBox.SelectedIndex = default;
		if (Game.IsNone && AllPlayersAssigned)
			GameStatusComboBox_SelectedIndexChanged();
	}

	private void NewGameButton_Click(object? sender = null,
									 EventArgs? e = null)
	{
		//	New Game
		ShowControls(true);
		GroupGamesDataGridView.Deselect();
		Disable(GroupGamesDataGridView, NewGameButton);
		Conceal([..ScoreDisplayControls, GameInErrorButton]);
		GameControl.Active = true;
		Game = Game.None;
		GameControl.ClearGame();
		GameNameTextBox.Clear();
		PlayerComboBoxes.ForEach(static box =>
								 {
									 box.Deselect();
									 box.Enabled = true;
								 });
		GameDateTimePicker.Value = DateTime.Today;
		Display(OrderByNamePanel);
		Enable(DeleteGameButton);
		DeleteGameButton.Text = "Cancel New Game";
		SetVisible(AllPlayersAssigned, GameStatusLabel, GameStatusComboBox);
	}

	private void GameStatusComboBox_SelectedIndexChanged(object? sender = null,
														 EventArgs? e = null)
	{
		if (SkippingHandlers)
			return;
		var status = GameStatusComboBox.SelectedIndex.As<Statuses>();
		switch (status)
		{
		case Seeded when !Game.IsNone:
			if (MessageBox.Show("Do you really want to re-seed this group game?",
								"Confirm Game Re-Seeding",
								YesNo,
								Question) is DialogResult.No)
				return;
			DeleteGame();
			NewGameButton_Click();
			break;
		case Finished when Game.IsNone || !AllFilledIn:
			SkipHandlers(() => GameStatusComboBox.SelectedIndex = Underway.AsInteger());
			if (Game.IsNone)
				goto CreateGame;
			break;
		case Seeded or Underway when Game.IsNone:
		CreateGame:
			Game = CreateOne(new Game
							 {
								 Round = HostRound,
								 Number = Group.Games.Length + 1,
								 Name = GameNameTextBox.Text
													   .Trim(),
								 Status = status,
								 Date = GameDateTimePicker.Value
							 });
			CreateMany([..Seven.Select(index => new GamePlayer
												{
													Game = Game,
													Player = PlayerComboBoxes[index].GetSelected<Player>(),
													Power = index.As<Powers>(),
													Result = Unknown
												})]);
			FillGameList();
			Enable(NewGameButton, DeleteGameButton, GroupGamesDataGridView);
			DeleteGameButton.Text = "Delete Game";
			GameControl.LoadGame(GamePlayers);
			break;
		case Seeded:
		case Underway:
		case Finished:
			break;
		default:
			throw new (); //	TODO
		}
		status = GameStatusComboBox.SelectedIndex
								   .As<Statuses>();
		SetVisible(status is Finished, ScoreDisplayControls);
		GameControl.Active = status is Underway;
		var seeding = status is Seeded;
		OrderByNamePanel.Visible = seeding;
		SetEnabled(seeding, PlayerComboBoxes);
		//	Games in Seeded status aren't (yet) recorded in the database.
		if (seeding)
			return;
		Game.Status = status;
		UpdateOne(Game);
		//  If the game just got changed to Finished and the scoring system
		//  uses a player ante, then scores have not yet been calculated.
		if (status is Finished && Group.ScoringSystem.UsesPlayerAnte)
			GameDataUpdated(true);
	}

	private void GroupGamesDataGridView_CellClick(object? sender = null,
												  DataGridViewCellEventArgs? e = null)
	{
		Game = GroupGamesDataGridView.GetSelected<GroupGame>()
									 .Game;
		SkipHandlers(() =>
					 {
						 GameDateTimePicker.Value = Game.Date;
						 GameNameTextBox.Text = Game.Name;
						 GameStatusComboBox.SelectedIndex = Game.Status
																.AsInteger();
						 PlayerComboBoxes.Apply((box, index) => box.SetSelectedItem(GamePlayers[index].Player));
					 });
		ShowControls(true);
		GameControl.ClearGame();
		GameControl.LoadGame(GamePlayers);
		GameControl.Active = GameStatusComboBox.SelectedIndex.As<Statuses>() is Underway;
		DeleteGameButton.Enabled = true;
	}

	private void GroupGamesDataGridView_DataBindingComplete(object sender,
															DataGridViewBindingCompleteEventArgs e)
		=> GroupGamesDataGridView.FillColumn(1);

	private void DeleteGameButton_Click(object sender,
										EventArgs e)
	{
		if (Game.IsNone)
		{
			//	Cancel New Game
			ShowControls(false);
			Enable(NewGameButton, GroupGamesDataGridView);
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
		if (SkippingHandlers || Game.IsNone)
			return;
		var newDate = GameDateTimePicker.Value;
		var earlier = Game.Date < newDate
						  ? Game.Date
						  : newDate;
		Game.Date = newDate;
		UpdateOne(Game);
		//	BUG: This doesn't seem to work to change player scores like I would expect....??
		if (Group.ScoringSystem.UsesPlayerAnte)
			RescoreGamesFrom(earlier);
		FillGameList();
		GameDataUpdated(AllFilledIn);
		GroupGamesDataGridView.SelectRowWhere<GroupGame>(groupGame => groupGame.Id == Game.Id);
		GroupGamesDataGridView_CellClick();
	}

	private void GameNameTextBox_Leave(object sender,
									   EventArgs e)
	{
		if (SkippingHandlers || Game.IsNone)
			return;
		Game.Name = GameNameTextBox.Text;
		UpdateOne(Game);
		FillGameList();
	}

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

	#endregion

	#region Methods

	private void FillGameList()
	{
		GroupGamesDataGridView.FillWith(Group.Games
											 .Select(static game => new GroupGame(game))
											 .Reverse());
		ShowControls(!Game.IsNone);
		if (Game.IsNone)
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

	private void ShowControls(bool visible)
	{
		Width = GroupGamesDataGridView.Width + 45 + (visible
														 ? GameControl.Width + PlayerColumnHeaderLabel.Width
																			 + ScoreColumnHeaderLabel.Width
																			 + 25 // TODO: check
														 : 0);
		SetVisible(visible,
				   GameControl, GameStatusComboBox, GameDateTimePicker, GameNameTextBox,
				   PlayerColumnHeaderLabel, GameStatusLabel, GameNameLabel, GameDateLabel);
		SetEnabled(Game.IsNone, PlayerComboBoxes);
		SetVisible(Game.Status is Finished, ScoreDisplayControls);
		OrderByNamePanel.Visible = GameStatusComboBox.SelectedIndex < 1;
	}

	private void DeleteGame()
	{
		Delete(Game.GamePlayers);
		Delete(Game);
		Game = Game.None;
		FillGameList();
	}

	private void RescoreGamesFrom(DateTime gameDate)
		=> Group.Games
				.Where(scoreable => scoreable.Date >= gameDate)
				.ForEach(static game => game.Scored = false);

	private void GameDataUpdated(bool allFilledIn)
	{
		AllFilledIn = allFilledIn;
		if (SkippingHandlers || Game.IsNone)
			return;
		UpdateMany(GamePlayers);
		//	If all GamePlayers are totally filled in, but we were told NOT allFilledIn,
		//	this means that FinalGameDataValidation failed. Show the GameInErrorButton.
		GameInErrorButton.Visible = !allFilledIn
								 && GamePlayers.All(static gamePlayer => gamePlayer.PlayComplete);
		AllFilledIn = allFilledIn && !GameInErrorButton.Visible;
		var showScores = allFilledIn
					  && (Game.Status is Finished || Group.ScoringSystem.UsesPlayerAnte is not true);
		SetVisible(showScores, ScoreDisplayControls);
		if (!showScores)
			return;
		//	Calculate the scores
		if (Game.Scored && Game.ScoringSystemId == Group.ScoringSystemId
		||  Game.CalculateScores(out var errors, Group.ScoringSystem))
			GamesForm.FillFinalScores(Game, ScoreLabels, TotalScoreLabel, ToolTip);
		else
			MessageBox.Show(errors.OfType<string>().BulletList("Error(s)"),
							"Game in Error",
							OK,
							Warning);
	}

	#endregion

	#endregion
}
