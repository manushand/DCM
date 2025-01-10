using JetBrains.Annotations;

namespace API;

using static Data.Data;

[PublicAPI]
internal class Tournament : Rest<Tournament, Data.Tournament>
{
	protected override dynamic Detail => new
										 {
											 Date = $"{Data.Date:d}",
											 Description = Data.Description.NullIfEmpty(),
											 Data.TotalRounds,
											 Seeding = new
													   {
														   Data.AssignPowers,
														   Data.GroupPowers,
														   Data.PlayerConflict,
														   Data.PowerConflict,
														   Data.ProgressiveScoreConflict,
														   TeamConflict = Data.TeamSize is 0
																			  ? (int?)null
																			  : Data.TeamConflict
													   },
											 Scoring = new
													   {
														   Data.ScoringSystemId,
														   Data.UnplayedScore,
														   Data.MinimumRounds,
														   Data.RoundsToDrop,
														   DropBeforeFinalRound = Data.RoundsToDrop is 0
																					  ? (bool?)null
																					  : Data.DropBeforeFinalRound,
														   Data.RoundsToScale,
														   ScalePercentage = Data.RoundsToScale is 0
																				 ? (int?)null
																				 : Data.ScalePercentage
													   },
											 TeamTournament = Data.TeamSize is 0
																  ? null
																  : new
																	{
																		Data.TeamSize,
																		Data.TeamConflict,
																		Data.TeamsPlayMultipleRounds,
																		TeamRound = Data.TeamsPlayMultipleRounds
																						? (int?)null
																						: Data.TeamRound,
																		Data.PlayerCanJoinManyTeams
																	}
										 };

	private bool IsEvent => Data.IsEvent;

	new public static IEnumerable<Tournament> GetAll()
		=> Rest<Tournament, Data.Tournament>.GetAll()
											.Where(static tournament => tournament.IsEvent);

	public static IResult GetRegistered(int id)
	{
		var record = Lookup(id);
		return record is null
				   ? Results.NotFound()
				   : Results.Ok(record.Data
									  .TournamentPlayers
									  .Select(static tp => new TournamentPlayer { Data = tp.Player, Rounds = tp.Rounds }));
	}

	public static IResult GetUnregistered(int id)
	{
		var record = Lookup(id);
		if (record is null)
			return Results.NotFound();
		var registered = record.Data.TournamentPlayers.Select(static tp => tp.PlayerId).ToArray();
		return Results.Ok(Player.GetAll()
								.Where(player => !registered.Contains(player.Id)));
	}

	protected override void Update(dynamic record)
	{
		Data.Date = record.Date;
		Data.Description = record.Description;
		Data.TotalRounds = record.TotalRounds;

		Data.TeamSize = record.TeamTournament.TeamSize;
		if (record.Seeding.TeamSize is not 0)
		{
			Data.TeamSize = record.TeamTournament.TeamSize;
			Data.TeamsPlayMultipleRounds = record.TeamTournament.TeamsPlayMultipleRounds;
			Data.PlayerCanJoinManyTeams = record.TeamTournament.PlayerCanJoinManyTeams;
			Data.TeamRound = record.TeamTournament.TeamRound;
		}

		Data.AssignPowers = record.Seeding.AssignPowers;
		Data.GroupPowers = record.Seeding.GroupPowers;
		Data.PlayerConflict = record.Seeding.PlayerConflict;
		Data.PowerConflict = record.Seeding.PowerConflict;
		Data.ProgressiveScoreConflict = record.Seeding.ProgressiveScoreConflict;

		Data.ScoringSystem = ReadById<Data.ScoringSystem>(record.Scoring.ScoringSystemId);
		Data.UnplayedScore = record.Scoring.UnplayedScore;
		Data.MinimumRounds = record.Scoring.MinimumRounds;
		Data.RoundsToDrop = record.Scoring.RoundsToDrop;
		Data.DropBeforeFinalRound = record.Scoring.DropBeforeFinalRound;
		Data.RoundsToScale = record.Scoring.RoundsToScale;
		Data.ScalePercentage = record.Scoring.RoundsToScale;
	}

	[PublicAPI]
	internal sealed class TournamentPlayer : Player
	{
		required public int[] Rounds { get; init; }
	}
}
