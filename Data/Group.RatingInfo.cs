using System.ComponentModel;

namespace Data;

public sealed partial class Group
{
	[PublicAPI]
	public sealed class RatingInfo : IRecord
	{
		[Browsable(false)]
		public readonly double Rating;

		[DisplayName(nameof (Rank))]
		public string DisplayRank { get; private set; } = Empty;

		public Player Player { get; }

		[DisplayName(nameof (Rating))]
		public string DisplayRating { get; }

		public int Games { get; }

		[Browsable(false)]
		public int Rank
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
