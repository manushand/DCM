﻿namespace DCM.UI.Forms;

internal sealed partial class GamesForm : Form
{
	private readonly Player? _player;
	private readonly Round? _round;

	private Game? _game;

	private Game[] Games { get; }

	private Round Round => _round.OrThrow();

	private Player Player => _player.OrThrow();

	private Game Game => _game.OrThrow();

	private int? GameNumber { get; }

	private List<ComboBox> PlayerNameComboBoxes { get; }

	private List<LinkLabel> ConflictLabels { get; }

	private List<Label> ScoreLabels { get; }

	private List<GamePlayer> GamePlayers => [..Game.GamePlayers];

	private bool AnyPowerUnassigned => GamePlayers.Any(static player => player.Power is TBD);

	private GamesForm(IEnumerable<Game> games)
	{
		Games = [..games];
		InitializeComponent();
		ConflictLabels = ConflictsPanel.PowerControls<LinkLabel>();
		ScoreLabels = ScoresPanel.PowerControls<Label>();
		PlayerNameComboBoxes = PlayerPanel.PowerControls<ComboBox>();
	}

	internal GamesForm(Round round,
					   int? gameNumber = null) : this(round.Games)
	{
		_round = round;
		GameNumber = gameNumber;
	}

	internal GamesForm(Player player,
					   params Game[] games) : this(games.Length > 0 ? games : player.Games)
		=> _player = player;

	private void GamesForm_Load(object sender,
								EventArgs e)
	{
		Text = _round is null
				   ? $"{Player} ─ Games"
				   : $"{Round.Tournament} ─ Round {Round} Games";
		SkipHandlers = true;
		//	TODO - Do NOT change this to a .FillWith call.  It blows up.  Not sure why.
		GamesTabControl.TabPages
					   .Clear();
		Games.ForEach(game => GamesTabControl.TabPages
											 .Add(_round is null
													  ? game.FullName
													  : $"Game {game.Number}"));
		SkipHandlers = false;
		GameControl.GameDataChangedCallback = GameDataUpdated;
		//	Set the active tab and be sure the event runs.
		GamesTabControl.ActivateTab(_player is null
										? GameNumber ?? (Games.FirstOrDefault(static game => game.Status < Finished) ?? Games[0]).Number - 1
										: 0);
	}

	private void GamesTabControl_SelectedIndexChanged(object sender,
													  EventArgs e)
	{
		if (SkipHandlers)
			return;
		_game = Games[GamesTabControl.SelectedIndex];
		GameControl.ClearGame();
		GameControl.TournamentScoringSystem = _game.Tournament
												   .ScoringSystem;
		GameControl.ScoringSystem = Game.ScoringSystem;
		SkipHandlers = true;
		GameStatusComboBox.SelectedIndex = Game.Status
											   .AsInteger();
		ScoringSystemComboBox.FillWithSorted<ScoringSystem>();
		ScoringSystemComboBox.SetSelectedItem(Game.ScoringSystem);
		var players = GamePlayers.SelectSorted(static gamePlayer => gamePlayer.Player) //	TODO: does this not have/need a by last name option?
								 .ToArray();
		foreach (var (box, power) in PlayerNameComboBoxes.Select(static (box, power) => (box, power.As<PowerNames>())))
		{
			box.FillWithRecords(players);
			box.Enabled = AnyPowerUnassigned;
			box.SetSelectedItem(GamePlayers.SingleOrDefault(gp => gp.Power == power)?.Player);
		}
		SkipHandlers = false;
		PlayerAssignmentAdviceLabel.Visible = AnyPowerUnassigned;
		GameControl.Active = Game.Status is Underway
						  && !AnyPowerUnassigned;
		//	TODO: disable other things if not underway??
		FillConflicts();
		GameControl.LoadGame(GamePlayers);
		SetScoringSystemChangeability();
	}

	private void SetScoringSystemChangeability()
		=> ScoringSystemComboBox.Enabled = Game.Status < Finished;

	private void ComboBox_EnabledChanged(object sender,
										 EventArgs e)
		=> sender.ToggleEnabled();

	private void GameDataUpdated(bool allFilledIn)
	{
		if (SkipHandlers)
			return;
		UpdateMany(GamePlayers);
		ScoreColumnHeaderLabel.Visible =
			ScoreTotalBarLabel.Visible =
				TotalScoreTextLabel.Visible =
					TotalScoreLabel.Visible =
						allFilledIn;
		ScoreLabels.ForEach(label => label.Visible = allFilledIn);
		//	If all GamePlayers are Completely filled in, but we were told NOT allFilledIn,
		//	this means that FinalGameDataValidation failed. Show the GameInErrorButton.
		GameInErrorButton.Visible = !allFilledIn
								 && GamePlayers.All(static gamePlayer => gamePlayer.PlayComplete);
		if (!allFilledIn)
			return;
		//	Fill in the scores
		if (Game.CalculateScores(out var errors))
			Game.FillFinalScores(ScoreLabels, TotalScoreLabel, ToolTip);
		else
			MessageBox.Show(errors.OfType<string>().BulletList(),
							"Game in Error",
							OK,
							Warning);
	}

	//	TODO: this is pretty much duplicated code over in RoundsForm
	private void ScoringSystemComboBox_SelectedIndexChanged(object sender,
															EventArgs e)
	{
		ScoringSystemComboBox.UpdateShadowLabel();
		if (SkipHandlers)
			return;
		var scoringSystem = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		if (Game.ScoringSystemId == scoringSystem.Id)
			return;
		Game.ScoringSystem = scoringSystem;
		UpdateOne(Game);
		//	Enable/disable+empty the needed/unneeded ComboBoxes.
		SkipHandlers = true;
		GameControl.ScoringSystem = scoringSystem;
		SkipHandlers = false;
		ScoringSystemDefaultLabel.Visible = Game.ScoringSystemIsDefault;
		GameDataUpdated(GameControl.AllFilledIn);
	}

	private void FillConflicts()
	{
		ConflictsPanel.Visible = Game.Tournament.Group is null;
		if (!ConflictsPanel.Visible)
			return;
		ConflictsColumnHeaderLabel.Visible =
			TotalConflictsLabel.Visible =
				ConflictsTotalBarLabel.Visible =
					!AnyPowerUnassigned;
		ConflictLabels.ForEach(label =>
							   {
								   label.Text = Empty;
								   label.Enabled = !AnyPowerUnassigned;
							   });
		if (AnyPowerUnassigned)
			return;
		var totalConflict = 0;
		foreach (var gamePlayer in GamePlayers)
		{
			totalConflict += gamePlayer.CalculateConflict(true); //	TODO: was seededPlayers, true
			if (gamePlayer.Power is TBD)
				continue;
			var label = ConflictLabels[gamePlayer.Power.AsInteger()];
			label.Text = gamePlayer.Conflict
								   .Points();
			ToolTip.SetToolTip(label, $"{gamePlayer.Player}:{gamePlayer.ConflictDetails.BulletList()}");
		}
		TotalConflictsLabel.Text = totalConflict.Points();
	}

	private void PlayerComboBox_SelectedIndexChanged(object sender,
													 EventArgs e)
	{
		var playerComboBox = (ComboBox)sender;
		playerComboBox.UpdateShadowLabel();
		if (SkipHandlers)
			return;
		SkipHandlers = true;
		foreach (var box in PlayerNameComboBoxes)
		{
			if (box.SelectedIndex != playerComboBox.SelectedIndex || box == playerComboBox)
				continue;
			var otherGamePlayer = GamePlayers.ByPlayerId(box.GetSelected<Player>().Id);
			box.Deselect();
			otherGamePlayer.Power = TBD;
			UpdateOne(otherGamePlayer);
			break;
		}
		SkipHandlers = false;
		var gamePlayer = GamePlayers.ByPlayerId(playerComboBox.GetSelected<Player>().Id);
		gamePlayer.Power = PlayerNameComboBoxes.IndexOf(playerComboBox)
											   .As<PowerNames>();
		UpdateOne(gamePlayer);
		SkipHandlers = false;
		if (PlayerNameComboBoxes.All(static box => box.SelectedItem is not null))
		{
			PlayerNameComboBoxes.ForEach(static box => box.Enabled = false);
			GameControl.Active = Game.Status is Underway;
			PlayerAssignmentAdviceLabel.Visible = false;
			FillConflicts();
		}
		SkipHandlers = false;
	}

	//	TODO: This method is duplicated verbatim in GroupGamesForm.cs
	private void GameInErrorButton_Click(object sender,
										 EventArgs e)
	{
		GameControl.FinalGameDataValidation(out var error);
		MessageBox.Show(error,
						"Game in Error",
						OK,
						Warning);
	}

	private void GameStatusComboBox_SelectedIndexChanged(object sender,
														 EventArgs e)
	{
		if (SkipHandlers)
			return;
		var newStatus = GameStatusComboBox.SelectedIndex
										  .As<Statuses>();
		switch (newStatus)
		{
		case Finished when !GameControl.AllFilledIn:
			MessageBox.Show("Game details are not complete.",
							"Cannot Finish Game",
							OK,
							Hand);
			GameStatusComboBox.SelectedIndex = Game.Status
												   .AsInteger();
			return;
		case Seeded when GameControl.HasData:
			if (MessageBox.Show($"Are you sure you wish to unstart this game?{NewLine}{NewLine}The game details recorded for players will be erased.",
								"Confirm Loss of Game-Player Details",
								YesNo,
								Warning) is DialogResult.No)
				return;
			GameControl.ClearGame();
			GamePlayers.ForEach(static gamePlayer =>
								{
									gamePlayer.Result = Unknown;
									gamePlayer.Centers =
										gamePlayer.Years =
											null;
								});
			UpdateMany(GamePlayers);
			break;
		case Finished:
		case Underway:
		case Seeded:
			break;
		default:
			throw new NotImplementedException("Unrecognized Statuses value"); //	TODO
		}
		Game.Status = newStatus;
		UpdateOne(Game);
		GameControl.Active = Game.Status is Underway && !AnyPowerUnassigned;
		SetScoringSystemChangeability();
	}
}
