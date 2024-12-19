namespace PC.Forms;

internal sealed partial class GroupRatingsForm
{
	[PublicAPI]
	public sealed record RatingInfo : IRecord // ...only to admit use of DataGridView.GetSelected<RatingInfo>()
	{
		internal static ScoringSystem ScoringSystem { set; private get; } = ScoringSystem.None;

		public string Rank { get; private set; } = Empty;
		public Player Player { get; }
		public string Rating => ScoringSystem.FormattedScore(RatingPoints);
		public int Games { get; }

		internal int Ranking
		{
			set => Rank = value.Dotted();
		}

		internal readonly double RatingPoints;

		internal RatingInfo(Player player,
							double ratingPoints,
							int games)
			=> (Player, RatingPoints, Games) = (player, ratingPoints, games);
	}
}
