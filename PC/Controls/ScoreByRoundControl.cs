namespace PC.Controls;

internal sealed partial class ScoreByRoundControl : UserControl, IScoreControl
{
	#region Public interface

	#region Constructor

	public ScoreByRoundControl()
		=> InitializeComponent();

	#endregion

	#region Method

	public void LoadControl(Tournament tournament)
	{
		//	Nothing to do if we're already loaded
		if (!Tournament.IsNone)
			return;
		Tournament = tournament;
		RoundScoresTabControl.TabPages
							 .Clear();
		Tournament.FinishedGames
				  .Modify(static game => game.CalculateScores())
				  .SelectSorted(static game => game.Round.Number)
				  .Distinct()
				  .ForEach(roundNumber => RoundScoresTabControl.TabPages
															   .Add($"Round {roundNumber}"));
		RoundScoresTabControl.SelectedIndex = RoundScoresTabControl.TabCount - 1;
		RoundScoresTabControl_SelectedIndexChanged();
	}

	#endregion

	#endregion

	#region Private implementation

	#region Type

	//	Do not make this a struct; it changes behavior.
	[PublicAPI]
	private sealed record RoundScore : IRecord
	{
		[DisplayName("#")]
		public string RoundRank => Rank.Dotted;

		public Player Player => GamePlayer.Player;

		[DisplayName(nameof (Game))]
		public int GameNumber => Game.Number;

		public string Power => GamePlayer.PowerName;

		public string Score => Game.ScoringSystem
								   .FormattedScore(FinalScore);

		internal int Rank { private get; set; }

		internal Game Game => GamePlayer.Game;
		internal double FinalScore => GamePlayer.FinalScore;

		private GamePlayer GamePlayer { get; }

		internal RoundScore(GamePlayer gamePlayer)
			=> GamePlayer = gamePlayer;
	}

	#endregion

	#region Data

	private Tournament Tournament { get; set; } = Tournament.None;

	#endregion

	#region Event handlers

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
		ScoresByRoundDataGridView.FillWith(roundScores.Modify(score => score.Rank = roundScores.Count(better => better.FinalScore > score.FinalScore) + 1));
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

	#endregion

	#endregion
}
