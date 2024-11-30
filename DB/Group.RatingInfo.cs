namespace DCM.DB;

internal sealed partial class Group
{
	[PublicAPI]
	internal sealed class RatingInfo : IRecord
	{
		internal readonly double Rating;

		[DisplayName(nameof (Rank))]
		public string DisplayRank { get; private set; } = string.Empty;

		public Player Player { get; }

		[DisplayName(nameof (Rating))]
		public string DisplayRating { get; }

		public int Games { get; }

		internal int Rank
		{
			set => DisplayRank = value.Dotted();
		}

		internal RatingInfo(Player player,
							double rating,
							ScoringSystem scoringSystem,
							int games)
			=> (Player, Rating, DisplayRating, Games) = (player, rating, scoringSystem.FormattedScore(rating), games);
	}
}
