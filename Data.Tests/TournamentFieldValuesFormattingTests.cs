using System;
using JetBrains.Annotations;
using Xunit;

namespace Data.Tests;

[PublicAPI]
public sealed class TournamentFieldValuesFormattingTests
{
	[Fact]
	public void FieldValues_Uses_NegatedIf_For_Combined_Flags()
	{
		var t = new Tournament
		{
			Id = 1,
			Name = "WDC",
			Description = "Desc",
			Date = new DateTime(2024, 1, 2),
			TeamConflict = 1,
			PlayerConflict = 2,
			PowerConflict = 3,
			TotalRounds = 4,
			MinimumRounds = 2,
			AssignPowers = true,
			GroupPowers = Tournament.PowerGroups.EastWest,
			UnplayedScore = 7,
			RoundsToDrop = 2,
			DropBeforeFinalRound = true,
			RoundsToScale = 5,
			ScalePercentage = 75,
			TeamSize = 3,
			PlayerCanJoinManyTeams = true,
			TeamRound = 4,
			TeamsPlayMultipleRounds = true,
			ScoreConflict = 5,
			ProgressiveScoreConflict = true,
			Group = Group.None
		};

		var sql = t.FieldValues;
		// Check a sampling of fields and the negated values
		Assert.Contains("[Name] = 'WDC'", sql);
		Assert.Contains("[Date] = '1/2/2024'", sql);
		// NegatedIf checks
		Assert.Contains("[RoundsToDrop] = -2", sql); // DropBeforeFinalRound true => negative
		Assert.Contains("[ScalePercentage] = 5.75", sql); // roundsToScale.ScalePercentage formatting
		Assert.Contains("[TeamSize] = -3", sql); // PlayerCanJoinManyTeams true => negative
		Assert.Contains("[TeamRound] = -4", sql); // TeamsPlayMultipleRounds true => negative
		Assert.Contains("[ScoreConflict] = -5", sql); // ProgressiveScoreConflict true => negative
	}
}
