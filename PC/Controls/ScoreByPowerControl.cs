﻿namespace PC.Controls;

internal sealed partial class ScoreByPowerControl : UserControl, IScoreControl
{
	#region Public interface

	#region Constructor

	internal ScoreByPowerControl()
	{
		InitializeComponent();
		BestGamesTabControl.TabPages
						   .Clear();
		// foreach (PowerNames power in Enum.GetValues<PowerNames>()) puts TBD at the end!
		for (var power = TBD; power <= Turkey; ++power)
			BestGamesTabControl.TabPages
							   .Add(new TabPage
									{
										Text = $" {(power is TBD ? "OVERALL" : power.InCaps)} ",
										BackColor = power.CellStyle.BackColor
									});
	}

	#endregion

	#region Method

	public void LoadControl(Tournament tournament)
	{
		//	Nothing to do if we're already loaded
		if (!Tournament.IsNone)
			return;
		Tournament = tournament;
		var tournamentSystem = tournament.ScoringSystem;
		List<GamePlayer> gamePlayers = [];
		foreach (var game in Tournament.FinishedGames)
		{
			//	TODO: is there any way around this?
			//	Some (rare) games are skipped, and not considered for Best Game Awards.
			//	These are those that use a ScoringSystem that .UsesOtherScore and that
			//	are run in a Tournament whose main ScoringSystem also does so, and when
			//	the .OtherScoreAlias differs in the two tournaments.
			var systemDiffers = game.ScoringSystemId != Tournament.ScoringSystemId;
			if (systemDiffers)
			{
				var gameSystem = game.ScoringSystem;
				if (gameSystem.UsesOtherScore
				&&  tournamentSystem.UsesOtherScore
				&&  !gameSystem.OtherScoreAlias.Matches(tournamentSystem.OtherScoreAlias))
					continue;
			}
			//	Calculation of the Best Game score is always done using the Tournament's default scoring system.
			if (systemDiffers || !game.Scored)
				game.CalculateScores(tournamentSystem);
			gamePlayers.AddRange(game.GamePlayers);
		}
		BestGames.FillWith(gamePlayers.Where(static gamePlayer => gamePlayer.Player.IsHuman)
									  .Select(static gamePlayer => new BestGame(gamePlayer)));
		foreach (var gamer in BestGames)
		{
			gamer.OverallRank = BestGames.Count(better => better.GameScore > gamer.GameScore) + 1;
			gamer.PowerRank = BestGames.Count(better => better.Power == gamer.Power
													 && better.GameScore > gamer.GameScore) + 1;
		}
		BestGamesTabControl.SelectedIndex = 0;
		BestGamesTabControl_SelectedIndexChanged();
	}

	#endregion

	#endregion

	#region Private implementation

	#region Type

	//	Do not make this a struct; it changes behavior.
	[PublicAPI]
	private sealed record BestGame(GamePlayer GamePlayer) : IRecord
	{
		[DisplayName("#")]
		public string OverallRankForDisplay => OverallRank.Dotted;

		[DisplayName("#")]
		public string PowerRankForDisplay => PowerRank.Dotted;

		public Player Player => GamePlayer.Player;

		public string Score => Game.Tournament
								   .ScoringSystem
								   .FormattedScore(GameScore);

		[DisplayName(nameof (Power))]
		public string PowerName => GamePlayer.PowerName;

		public string? Centers => GamePlayer.Centers?.ToString();

		public string? Year => GamePlayer.Years?.ToString();

		[DisplayName("Round─Game")]
		public string Round => $"{Game.Round}─{Game.Letter}";

		internal int OverallRank;
		internal int PowerRank;

		internal Game Game => GamePlayer.Game;
		internal Powers Power => GamePlayer.Power;
		internal double GameScore => GamePlayer.FinalScore;

		private GamePlayer GamePlayer { get; } = GamePlayer;
	}

	#endregion

	#region Data

	private Tournament Tournament { get; set; } = Tournament.None;

	private List<BestGame> BestGames { get; } = [];

	#endregion

	#region Event handlers

	private void BestGamesTabControl_DrawItem(object sender,
											  DrawItemEventArgs e)
	{
		var style = (e.Index.As<Powers>() - 1).CellStyle;
		e.Graphics.FillRectangle(new SolidBrush(style.BackColor), e.Bounds);
		e.Bounds.Offset(1, e.State is DrawItemState.Selected
							   ? -2
							   : +1);
		TextRenderer.DrawText(e.Graphics,
							  BestGamesTabControl.TabPages[e.Index]
												 .Text,
							  BoldFonts.GetOrSet(Font, BoldFont),
							  e.Bounds,
							  style.ForeColor);
	}

	private void BestGamesTabControl_SelectedIndexChanged(object? sender = null,
														  EventArgs? e = null)
	{
		var index = BestGamesTabControl.SelectedIndex;
		if (index is -1)
			return;
		var overall = index is 0;
		BestPowersDataGridView.FillWith(BestGames.Where(game => overall
															 || index - 1 == game.Power.AsInteger())
												 .OrderBy(game => overall
																	  ? game.OverallRank
																	  : game.PowerRank));
	}

	private void BestPowersDataGridView_DataBindingComplete(object sender,
															DataGridViewBindingCompleteEventArgs e)
	{
		BestPowersDataGridView.FillColumn(2);
		ForRange(0, BestPowersDataGridView.ColumnCount, index => BestPowersDataGridView.AlignColumn(index > 3
																										? MiddleCenter
																										: index is 2
																											? MiddleLeft
																											: MiddleRight,
																									index));
		var overall = BestGamesTabControl.SelectedIndex is 0;
		BestPowersDataGridView.Columns[0].Visible =
			BestPowersDataGridView.Columns[4].Visible =
				overall;
		BestPowersDataGridView.Columns[1].Visible = !overall;
		BestPowersDataGridView.Columns[5].Visible = Tournament.ScoringSystem
															  .UsesCenterCount;
		BestPowersDataGridView.Columns[6].Visible = Tournament.ScoringSystem
															  .UsesYearsPlayed;
		if (overall)
			BestPowersDataGridView.PowerCells(4);
	}

	private void PrintButton_Click(object sender,
								   EventArgs e)
		=> BestPowersDataGridView.Print(Tournament.Name,
										$"Best {BestGamesTabControl.SelectedTab.OrThrow().Text.Trim()} Scores");

	private void BestPowersDataGridView_CellContentDoubleClick(object sender,
															   DataGridViewCellEventArgs e)
	{
		var bestGame = BestPowersDataGridView.GetSelected<BestGame>();
		Show<GamesForm>(() => new (bestGame.Player, bestGame.Game));
	}

	#endregion

	#endregion
}
