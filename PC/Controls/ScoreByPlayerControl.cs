﻿namespace PC.Controls;

internal sealed partial class ScoreByPlayerControl : UserControl, IScoreControl
{
	#region Public interface

	#region Constructor

	internal ScoreByPlayerControl()
		=> InitializeComponent();

	#endregion

	#region Method

	public void LoadControl(Tournament tournament)
	{
		//	Nothing to do if we're already loaded
		if (!Tournament.IsNone)
			return;
		Tournament =
			TournamentScore.Tournament =
				tournament;
		ScoreTournament();
		LegendLabel.Text = Join(null,
								LegendItems.Where(pair => TournamentScores.Any(score => score.HasNote(pair.Key)))
										   .Select(static pair => $"  {pair.Key} ─ {pair.Value}  "));
		FinalScoresTabControl.TabPages
							 .Clear();
		FinalScoresTabControl.Visible = Tournament.MinimumRounds > 1;
		if (TournamentScores.Any(static score => score.Qualified))
			FinalScoresTabControl.TabPages
								 .Add(QualifiedPlayersTabPage);
		if (TournamentScores.Any(static score => !score.Qualified))
			FinalScoresTabControl.TabPages
								 .Add(UnqualifiedPlayersTabPage);
		FinalScoresTabControl.SelectedIndex = default;
		FinalScoresTabControl_SelectedIndexChanged();

		void ScoreTournament()
		{
			TournamentScore[] tournamentScores = [..Tournament.FinishedGames
															  .SelectMany(static game => game.GamePlayers)
															  .Select<GamePlayer, Player>(static gamePlayer => gamePlayer.Player)
															  .Distinct()
															  .Where(static player => player.IsHuman)
															  .Select(static player => new TournamentScore(player))];
			foreach (var qualified in new[] { true, false })
			{
				TournamentScore[] scorers = [..tournamentScores.Where(score => score.Qualified == qualified)];
				double[] uniqueScores = [..scorers.Select(static score => score.Score)
												  .Distinct()
												  .OrderDescending()];
				var rank = 0;
				foreach (var tied in uniqueScores.Select(score => scorers.Where(total => total.Score.Equals(score))
																		 .ToArray()))
				{
					if (tied.Length is 1)
					{
						tied.Single().Rank = ++rank;
						continue;
					}
					//	Break ties and set rank accordingly:
					//	1. highest total score in games that EVERY TIED player played in together;
					//	2. highest single game score in any round of the tournament;
					//	3. give up and call any remaining player(s) tied.
					int[] playerIds = [..tied.Select(static scorer => scorer.Player.Id)];
					var commonGames = Tournament.FinishedGames
												.Where(game => playerIds.All(game.PlayerIds
																				 .Contains));
					TournamentScore[] tiebreaker = [..tied.Modify(scorer => scorer.Breaker = (commonGames.Select(game => game.GamePlayers
																															.ByPlayerId(scorer.Player.Id)
																															.FinalScore)
																										 .DefaultIfEmpty()
																										 .Sum(),
																							  scorer.BestRoundScore))
														  .OrderByDescending(static scores => scores.Breaker)];
					tiebreaker.Apply((scorer, index) => scorer.Rank = ++rank + tiebreaker.Skip(++index)
																						 .Count(a => a.Breaker == scorer.Breaker));
				}
			}
			TournamentScores.FillWith(tournamentScores);
		}
	}

	#endregion

	#endregion

	#region Private implementation

	#region Type

	//	Do not make this a struct; it changes behavior.
	[PublicAPI]
	private sealed record TournamentScore : IRecord
	{
		[DisplayName("#")]
		public string DisplayRank => Rank.Dotted;

		public Player Player { get; }

		[DisplayName("Score")]
		public string DisplayScore => Formatted(Score);

		[DisplayName("Round 1")]
		public string Round1Score => this[0];

		[DisplayName("Round 2")]
		public string Round2Score => this[1];

		[DisplayName("Round 3")]
		public string Round3Score => this[2];

		[DisplayName("Round 4")]
		public string Round4Score => this[3];

		[DisplayName("Round 5")]
		public string Round5Score => this[4];

		[DisplayName("Round 6")]
		public string Round6Score => this[5];

		[DisplayName("Round 7")]
		public string Round7Score => this[6];

		[DisplayName("Round 8")]
		public string Round8Score => this[7];

		[DisplayName("Round 9")]
		public string Round9Score => this[8];

		[DisplayName("Centers")]
		public string TotalCenters => $"{_totalCenters}";

		[DisplayName("Years")]
		public string TotalYears => $"{_totalYears}";

		internal static Tournament Tournament
		{
			private get => field.NotNone;
			set
			{
				field = value;
				_roundsToHaveScoreChanges = value.RoundsToDrop + value.RoundsToScale;
				//	If there is no drop, we scale (if called for) as soon as the
				//	player has played more than RoundsToScale rounds.
				//	Otherwise, we drop (then scale if called for) only if he has
				//	played more than TotalRounds - DropBeforeFinalRound - 1.
				_roundsBeforeScoreChanges = value.RoundsToDrop is 0
												? value.RoundsToScale
												: value.TotalRounds
												  - value.DropBeforeFinalRound.AsInteger()
												  - 1;
			}
		} = Tournament.None;

		internal readonly bool Qualified;
		internal readonly double Score;
		internal int Rank;
		internal (double CommonGames, double BestRoundScore) Breaker;

		internal double BestRoundScore => RoundScores.Max();

		private static int _roundsToHaveScoreChanges;
		private static int _roundsBeforeScoreChanges;

		private readonly int _totalCenters;
		private readonly int _totalYears;

		private double[] RoundScores { get; } = new double[9];
		private string?[] ScoreNotes { get; } = new string[9];

		private string this[int index]
			=> $"{ScoreNotes[index]}{Formatted(RoundScores[index])}";

		internal TournamentScore(Player player)
		{
			Player = player;
			var centers = new int[9];
			var years = new int[9];
			var roundsPlayed = 0;
			var scoringRounds = Tournament.Rounds
										  .Length;
			for (var roundNumber = 0; roundNumber < scoringRounds; ++roundNumber)
			{
				//	TODO: This will except if it's ever possible for a player to play more than one game in a round.
				var gamePlayer = Tournament.Rounds[roundNumber]
										   .FinishedGames
										   .SelectMany(static game => game.GamePlayers)
										   .SingleOrDefault(gp => gp.PlayerId == player.Id);
				RoundScores[roundNumber] = gamePlayer?.FinalScore ?? Tournament.UnplayedScore;
				centers[roundNumber] = gamePlayer?.Centers ?? 0;
				years[roundNumber] = gamePlayer?.Years ?? 0;
				if (gamePlayer is null)
					ScoreNotes[roundNumber] = $"{SatOut}";
				else
				{
					++roundsPlayed;
					ScoreNotes[roundNumber] = Empty;
				}
			}
			Qualified = roundsPlayed >= Tournament.MinimumRounds;
			//	Drop then scale the lowest rounds
			if (scoringRounds > _roundsBeforeScoreChanges)
				foreach (var (roundNumber, drop) in RoundScores.Take(scoringRounds)
															   .Select(static (score, roundNumber) => new { score, roundNumber })
															   .OrderBy(static a => a.score)
															   .Take(_roundsToHaveScoreChanges)
															   .Select(static (a, i) => (a.roundNumber, i < Tournament.RoundsToDrop)))
				{
					if (drop)
					{
						RoundScores[roundNumber] = Tournament.UnplayedScore;
						centers[roundNumber] =
							years[roundNumber] =
								0;
					}
					else
						RoundScores[roundNumber] *= Tournament.ScalePercentage / 100d;
					ScoreNotes[roundNumber] += drop
												   ? TooLow
												   : Scaled;
				}
			Score = RoundScores.Sum();
			_totalCenters = centers.Sum();
			_totalYears = years.Sum();
		}

		internal bool HasNote(char key)
			=> ScoreNotes.Any(note => note?.Contains(key) is true);

		private static string Formatted(double score)
			=> Tournament.ScoringSystem.FormattedScore(score);
	}

	#endregion

	#region Data

	private const char SatOut = '*';
	private const char TooLow = '†';
	private const char Scaled = '‡';

	//	A SORTED(!)Dictionary of only three entries?!  Hey, every little bit helps, right?
	private static readonly SortedDictionary<char, string> LegendItems = new ()
																		 {
																			 [SatOut] = "did not play",
																			 [TooLow] = "score dropped",
																			 [Scaled] = "score scaled"
																		 };

	private Tournament Tournament { get; set; } = Tournament.None;

	private List<TournamentScore> TournamentScores { get; } = [];

	private TournamentScore[] QualifiedScores => [..TournamentScores.Where(static score => score.Qualified).OrderBy(static score => score.Rank)];

	private TournamentScore[] UnqualifiedScores => [..TournamentScores.Where(static score => !score.Qualified).OrderBy(static score => score.Rank)];

	#endregion

	#region Event handlers

	private void FinalScoresTabControl_SelectedIndexChanged(object? sender = null,
															EventArgs? e = null)
	{
		var tabPage = FinalScoresTabControl.SelectedTab;
		if (tabPage is null)
			//	Not an exception; happens when TabPages.Clear();
			return;
		FinalScoresDataGridView.FillWith(tabPage == QualifiedPlayersTabPage
											 ? QualifiedScores
											 : UnqualifiedScores);
		FinalScoresDataGridView.Width = 100
									  + 50 * FinalScoresDataGridView.Columns
																	.Cast<DataGridViewColumn>()
																	.Count(static column => column.Visible);
		LegendLabel.Width = FinalScoresDataGridView.Width - PrintButton.Width;
		var location = LegendLabel.Location;
		location.X += LegendLabel.Width;
		PrintButton.Location = location;
		Width = FinalScoresDataGridView.Width + 20;
	}

	private void FinalScoresDataGridView_DataBindingComplete(object sender,
															 DataGridViewBindingCompleteEventArgs e)
	{
		FinalScoresDataGridView.FillColumn(1);
		FinalScoresDataGridView.AlignColumn(MiddleLeft, 1);
		FinalScoresDataGridView.AlignColumn(MiddleRight, 0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
		int[] scoredRounds = [..Tournament.FinishedGames
										  .Select(static game => game.Round.Number)
										  .Distinct()];
		ForRange(1, 9, roundNumber => FinalScoresDataGridView.Columns[2 + roundNumber].Visible = scoredRounds.Contains(roundNumber));
		FinalScoresDataGridView.Columns[12].Visible = Tournament.ScoringSystem.UsesCenterCount;
		FinalScoresDataGridView.Columns[13].Visible = Tournament.ScoringSystem.UsesYearsPlayed;
	}

	private void PrintButton_Click(object sender,
								   EventArgs e)
	{
		var subtitle = "Player Scores";
		if (FinalScoresTabControl.Visible)
			subtitle = $"{FinalScoresTabControl.SelectedTab.OrThrow().Text.Split().First()} {subtitle}";
		FinalScoresDataGridView.Print(Tournament.Name, subtitle);
	}

	private void FinalScoresDataGridView_CellContentDoubleClick(object sender,
																DataGridViewCellEventArgs e)
	{
		var player = FinalScoresDataGridView.GetSelected<TournamentScore>()
											.Player;
		Show<GamesForm>(() => new (player, [..Tournament.FinishedGames
														.Where(game => game.GamePlayers.HasPlayerId(player.Id))
														.OrderBy(static game => game.Round.Number)]));
	}

	#endregion

	#endregion
}
