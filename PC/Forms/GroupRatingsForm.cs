namespace PC.Forms;

using static Group;
using static Group.GamesToRate;

internal sealed partial class GroupRatingsForm : Form
{
	private Group Group { get; }

	internal GroupRatingsForm(Group group)
	{
		InitializeComponent();
		Group = group;
	}

	private void GroupRatingsForm_Load(object sender,
									   EventArgs e)
	{
		Text = $"{Group} ─ Member Ratings";
		RatingsTabControl_SelectedIndexChanged();
	}

	private void RatingsDataGridView_DataBindingComplete(object sender,
														 DataGridViewBindingCompleteEventArgs e)
	{
		if (RatingsDataGridView.Columns.Count is 0)
			return;
		RatingsDataGridView.FillColumn(1);
		RatingsDataGridView.AlignColumn(MiddleRight, 0, 2);
		RatingsDataGridView.AlignColumn(MiddleCenter, 3);
	}

	private void RatingsTabControl_SelectedIndexChanged(object? sender = null,
														EventArgs? e = null)
	{
		RatingInfo.ScoringSystem = Group.ScoringSystem;
		var gamesToRate = RatingsTabControl.SelectedIndex
										   .As<GamesToRate>();
		var groupPlayers = Group.Players
								.Where(static player => player.IsHuman)
								.Select(player => Group.RatePlayer(player, gamesToRate: gamesToRate))
								.OfType<RatingRecord>()
								.OrderByDescending(static player => player.Rating)
								.Select(static player => new RatingInfo(player.Player, player.Rating, player.Games))
								.ToList();
		groupPlayers.ForEach(player => player.Ranking = groupPlayers.Count(ratingInfo => ratingInfo.RatingPoints > player.RatingPoints) + 1);
		RatingsDataGridView.FillWith(groupPlayers);
		RatedGamesLabel.Text = gamesToRate switch
							   {
								   GroupGamesOnly                   => $"All {Group} Group Games",
								   GamesUsingGroupSystem            => $"All Games That Used {Group.ScoringSystem}",
								   AllGamesScoreableWithGroupSystem => $"All Games Scoreable Using {Group.ScoringSystem}",
								   _                                => throw new NotImplementedException("Unrecognized GamesToRate value")
							   };
	}

	private void RatingsDataGridView_CellContentDoubleClick(object sender,
															DataGridViewCellEventArgs e)
	{
		var gamesToRate = RatingsTabControl.SelectedIndex
										   .As<GamesToRate>();
		var player = RatingsDataGridView.GetSelected<RatingInfo>()
										.Player;
		Show<GamesForm>(() => new (player, [..player.Games
													.Where(game => Group.IsRatable(game, gamesToRate))
													.OrderBy(static game => game.Date)]));
	}
}
