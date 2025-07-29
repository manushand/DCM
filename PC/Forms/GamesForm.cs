namespace PC.Forms;

internal sealed partial class GamesForm : Form
{
	private Game[] Games { get; }

	private Round Round { get; } = Round.None;

	private Player Player { get; } = Player.None;

	private Game Game
	{
		get => field.NotNone;
		set;
	} = Game.None;

	private int? GameNumber { get; }

	private List<ComboBox> PlayerNameComboBoxes { get; }

	private List<LinkLabel> ConflictLabels { get; }

	private List<Label> ScoreLabels { get; }

	private List<GamePlayer> GamePlayers => [..Game.GamePlayers];

	private bool AnyPowerUnassigned => GamePlayers.Any(static player => player.Power is TBD);

	private Control[] ScoreDisplayControls => [ScoreColumnHeaderLabel, ScoreTotalBarLabel, TotalScoreTextLabel, ScoresPanel, TotalScoreLabel];

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
		=> (Round, GameNumber) = (round, gameNumber);

	internal GamesForm(Player player,
					   params Game[] games) : this(games.Length is not 0 ? games : player.Games)
		=> Player = player;

	//	This static method is internal because it is also used by GroupGamesForm.
	//	TODO: A better way to do this would be to have both forms inherit from a common base class.
	internal static void FillFinalScores(Game game,
										 List<Label> scoreLabels,
										 Label totalScoreLabel,
										 ToolTip toolTip)
	{
		var format = game.ScoringSystem.ScoreFormat;
		var gamePlayers = game.GamePlayers.ToArray();
		var isGroup = !game.Tournament.IsEvent;
		var scoreOrRating = isGroup
								? "Rating"
								: "Score";
		var usesAnte = isGroup && game.ScoringSystem.UsesPlayerAnte;
		scoreLabels.Apply((scoreLabel, i) =>
						  {
							  var gamePlayer = gamePlayers[i];
							  var score = gamePlayer.FinalScore;
							  scoreLabel.Text = score.ToString(format);
							  var preGame = game.Round
												.PreRoundScore(gamePlayer);
							  var postGame = isGroup
												 ? game.Tournament
													   .Group
													   .RatePlayer(gamePlayer.Player, game, includeTheBeforeGame: true)?
													   .Rating ?? 0
												 : preGame + score;
							  List<string> details = [$"Pre-Game {scoreOrRating}: {preGame}"];
							  var ante = gamePlayer.PlayerAnte;
							  if (usesAnte)
								  details.AddRange($"Player Ante: {ante}",
												   $"Game Score: {ante + score}");
							  if (isGroup)
								  details.Add($"Rating Change: {postGame - preGame}");
							  details.Add($"Post-Game {scoreOrRating}: {postGame}");
							  toolTip.SetToolTip(scoreLabels[i], details.BulletList($"{gamePlayer.Player}"));
						  });
		totalScoreLabel.Text = gamePlayers.Sum(static gamePlayer => gamePlayer.FinalScore)
										  .ToString(format);
	}

	private void GamesForm_Load(object sender,
								EventArgs e)
	{
		Text = Round.IsNone
				   ? $"{Player} ─ Games"
				   : $"{Round.Tournament} ─ Round {Round} Games";
		SkipHandlers(() =>
        {
			//	TODO - Do NOT change this to a .FillWith call.  It blows up.  Not sure why.
			GamesTabControl.TabPages
						   .Clear();
			Games.ForEach(game => GamesTabControl.TabPages
												 .Add(Round.IsNone
														  ? game.FullName
														  : $"Game {game.Number}"));
        });
		GameControl.GameDataChangedCallback = GameDataUpdated;
		//	Set the active tab, which will fire an event setting "Game".
		GamesTabControl.ActivateTab(Player.IsNone
										? GameNumber ?? (Games.FirstOrDefault(static game => game.Status < Finished) ?? Games[0]).Number - 1
										: 0);
	}

	private void GamesTabControl_SelectedIndexChanged(object sender,
													  EventArgs e)
	{
		if (SkippingHandlers)
			return;
		Game = Games[GamesTabControl.SelectedIndex];
		GameControl.ClearGame();
		GameControl.TournamentScoringSystem = Game.Tournament
												  .ScoringSystem;
		GameControl.ScoringSystem = Game.ScoringSystem;
		SkipHandlers(() =>
        {
			GameStatusComboBox.SelectedIndex = Game.Status
												   .AsInteger();
			ScoringSystemComboBox.FillWithSorted<ScoringSystem>();
			ScoringSystemComboBox.SetSelectedItem(Game.ScoringSystem);
			var players = GamePlayers.SelectSorted(static gamePlayer => gamePlayer.Player) //	TODO: does this not have/need a by last name option?
									 .ToArray();
			foreach (var (box, power) in PlayerNameComboBoxes.Select(static (box, power) => (box, power.As<Powers>())))
			{
				box.FillWithRecords(players);
				box.Enabled = AnyPowerUnassigned;
				box.SetSelectedItem(GamePlayers.SingleOrDefault(gp => gp.Power == power)?.Player);
			}
        });
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
		if (SkippingHandlers)
			return;
		UpdateMany(GamePlayers);
		List<string?> errors = [];
		var scored = allFilledIn && Game.Status is Finished && Game.CalculateScores(out errors);
		SetVisible(scored, ScoreDisplayControls);
		//	If all GamePlayers are totally filled in, but we were told NOT allFilledIn,
		//	this means that FinalGameDataValidation failed. Show the GameInErrorButton.
		GameInErrorButton.Visible = !allFilledIn
								 && GamePlayers.All(static gamePlayer => gamePlayer.PlayComplete);
		if (scored)
			FillFinalScores(Game, ScoreLabels, TotalScoreLabel, ToolTip);
		else if (errors.Count > 0)
			MessageBox.Show(errors.OfType<string>().BulletList("Error(s)"),
							"Game in Error",
							OK,
							Warning);
	}

	//	TODO: this is pretty much duplicated code over in RoundsForm
	private void ScoringSystemComboBox_SelectedIndexChanged(object sender,
															EventArgs e)
	{
		ScoringSystemComboBox.UpdateShadowLabel();
		if (SkippingHandlers)
			return;
		var scoringSystem = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		if (Game.ScoringSystemId == scoringSystem.Id)
			return;
		Game.ScoringSystem = scoringSystem;
		UpdateOne(Game);
		//	Enable/disable+empty the needed/unneeded ComboBoxes.
		SkipHandlers(() => GameControl.ScoringSystem = scoringSystem);
		ScoringSystemDefaultLabel.Visible = Game.ScoringSystemIsDefault;
		GameDataUpdated(GameControl.AllFilledIn);
	}

	private void FillConflicts()
	{
		ConflictsPanel.Visible = Game.Tournament.IsEvent;
		if (!ConflictsPanel.Visible)
			return;
		SetVisible(!AnyPowerUnassigned, ConflictsColumnHeaderLabel, TotalConflictsLabel, ConflictsTotalBarLabel);
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
			ToolTip.SetToolTip(label, gamePlayer.ConflictDetails.BulletList($"{gamePlayer.Player}"));
		}
		TotalConflictsLabel.Text = totalConflict.Points();
	}

	private void PlayerComboBox_SelectedIndexChanged(object sender,
													 EventArgs e)
	{
		var playerComboBox = (ComboBox)sender;
		playerComboBox.UpdateShadowLabel();
		if (SkippingHandlers)
			return;
		SkipHandlers(() =>
        {
			foreach (var box in PlayerNameComboBoxes.Where(box => box.SelectedIndex == playerComboBox.SelectedIndex && box != playerComboBox))
			{
				var otherGamePlayer = GamePlayers.ByPlayerId(box.GetSelected<Player>().Id);
				box.Deselect();
				otherGamePlayer.Power = TBD;
				UpdateOne(otherGamePlayer);
				break;
			}
        });
		var gamePlayer = GamePlayers.ByPlayerId(playerComboBox.GetSelected<Player>().Id);
		gamePlayer.Power = PlayerNameComboBoxes.IndexOf(playerComboBox)
											   .As<Powers>();
		UpdateOne(gamePlayer);
		if (PlayerNameComboBoxes.All(static box => box.SelectedItem is not null))
			SkipHandlers(() =>
						 {
							 SetEnabled(false, PlayerNameComboBoxes);
							 GameControl.Active = Game.Status is Underway;
							 PlayerAssignmentAdviceLabel.Hide();
							 FillConflicts();
						 });
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
		if (SkippingHandlers)
			return;
		var newStatus = GameStatusComboBox.SelectedIndex
										  .As<Statuses>();
		switch (newStatus)
		{
		case Seeded when GameControl.HasData:
			if (MessageBox.Show($"Are you sure you wish to unstart this game?{NewLine}{NewLine}The game details recorded for players will be erased.",
								"Confirm Erasure of Game-Player Details",
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
			var issue = !GameControl.AllFilledIn
							? "Game details are not complete."
							: !Game.CalculateScores(out var issues)
								? Join(NewLine, issues)
								: Empty;
			if (issue.Length is 0)
				break;
			MessageBox.Show(issue,
							"Cannot Finish Game",
							OK,
							Hand);
			GameStatusComboBox.SelectedIndex = Game.Status
												   .AsInteger();
			return;
		case Underway:
		case Seeded:
			break;
		default:
			throw new NotImplementedException("Unrecognized Statuses value"); //	TODO
		}
		Game.Status = newStatus;
		UpdateOne(Game);
		SetVisible(newStatus is Finished, ScoreDisplayControls);
		GameControl.Active = Game.Status is Underway && !AnyPowerUnassigned;
		SetScoringSystemChangeability();
	}
}
