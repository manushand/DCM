namespace DCM.DB;

internal sealed partial class Group
{
	[PublicAPI]
	internal sealed class RatingInfo : IRecord
	{
		internal readonly decimal Rating;

		[DisplayName(nameof (Rank))]
		public string DisplayRank { get; private set; } = Empty;

		public Player Player { get; }

		[DisplayName(nameof (Rating))]
		public string DisplayRating { get; }

		public int Games { get; }

		internal int Rank
		{
			set => DisplayRank = value.Dotted();
		}

		internal RatingInfo(Player player,
							decimal rating,
							ScoringSystem scoringSystem,
							int games)
			=> (Player, Rating, DisplayRating, Games) = (player, rating, scoringSystem.FormattedScore(rating), games);
	}
}
