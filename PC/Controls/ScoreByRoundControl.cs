namespace PC.Controls;

internal sealed partial class ScoreByRoundControl : UserControl, IScoreControl
{
	private Tournament Tournament { get; set; } = Tournament.None;

	public ScoreByRoundControl()
		=> InitializeComponent();

	public void LoadControl(Tournament tournament)
	{
		//	Nothing to do if we're already loaded
		if (!Tournament.IsNone)
			return;
		Tournament = tournament;
		RoundScoresTabControl.TabPages
							 .Clear();
		var roundNumbers = Tournament.FinishedGames
									 .SelectSorted(static game => game.Round.Number)
									 .Distinct();
		roundNumbers.ForEach(roundNumber => RoundScoresTabControl.TabPages
																 .Add($"Round {roundNumber}"));
		Tournament.FinishedGames
				  .ForEach(static game => game.CalculateScores());
		RoundScoresTabControl.SelectedIndex = RoundScoresTabControl.TabCount - 1;
		RoundScoresTabControl_SelectedIndexChanged();
	}

	private void RoundScoresTabControl_SelectedIndexChanged(object? sender = null,
															EventArgs? e = null)
	{
		var tabPage = RoundScoresTabControl.SelectedTab;
		if (tabPage is null)
			//	Not an exception; happens when TabPages.Clear();
			return;
		var roundNumber = tabPage.Text
								 .Split()
								 .Last()
								 .AsInteger();
		var roundScores = Tournament.FinishedGames
									.Where(game => game.Round.Number == roundNumber)
									.SelectMany(static game => game.GamePlayers)
									.Where(static gamePlayer => gamePlayer.Player.IsHuman)
									.OrderByDescending(static gamePlayer => gamePlayer.FinalScore)
									.Select(static gamePlayer => new RoundScore(gamePlayer))
									.ToList();
		//	Ties are not broken in individual round scores
		roundScores.ForEach(score => score.Rank = roundScores.Count(better => better.FinalScore > score.FinalScore) + 1);
		ScoresByRoundDataGridView.FillWith(roundScores);
	}

	private void ScoresByRoundDataGridView_DataBindingComplete(object sender,
															   DataGridViewBindingCompleteEventArgs e)
	{
		ScoresByRoundDataGridView.FillColumn(1);
		ScoresByRoundDataGridView.AlignColumn(MiddleCenter, 2);
		ScoresByRoundDataGridView.PowerCells(3);
		ScoresByRoundDataGridView.AlignColumn(MiddleRight, 0, 4);
	}

	private void PrintButton_Click(object sender,
								   EventArgs e)
		=> ScoresByRoundDataGridView.Print(Tournament.Name,
										   $"{RoundScoresTabControl.SelectedTab.OrThrow().Text} Scores");

	private void ScoresByRoundDataGridView_CellContentDoubleClick(object sender,
																  DataGridViewCellEventArgs e)
	{
		var roundScore = ScoresByRoundDataGridView.GetSelected<RoundScore>();
		Show<GamesForm>(() => new (roundScore.Player, roundScore.Game));
	}

	#region RoundScore record

	//	Do not make this a struct; it changes behavior.
	[PublicAPI]
	private sealed record RoundScore : IRecord
	{
		[DisplayName("#")]
		public string RoundRank => Rank.Dotted;

		public Player Player => _gamePlayer.Player;

		[DisplayName(nameof (Game))]
		public int GameNumber => Game.Number;

		public string Power => _gamePlayer.PowerName;

		public string Score => Game.ScoringSystem
								   .FormattedScore(FinalScore);

		internal double FinalScore => _gamePlayer.FinalScore;
		internal int Rank { private get; set; }
		internal Game Game => _gamePlayer.Game;

		private readonly GamePlayer _gamePlayer;

		internal RoundScore(GamePlayer gamePlayer)
			=> _gamePlayer = gamePlayer;
	}

	#endregion
}
