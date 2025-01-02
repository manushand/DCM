using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Tournament : Rest<Tournament, Data.Tournament>
{
	protected override dynamic Detail => new
										 {
											 Date = $"{Record.Date:d}",
											 Description = Record.Description.NullIfEmpty(),
											 Record.TotalRounds,
											 Seeding = new
													   {
														   Record.AssignPowers,
														   Record.GroupPowers,
														   Record.PlayerConflict,
														   Record.PowerConflict,
														   Record.ProgressiveScoreConflict,
														   TeamConflict = Record.TeamSize is 0
																			  ? (int?)null
																			  : Record.TeamConflict
													   },
											 Scoring = new
													   {
														   Record.ScoringSystemId,
														   Record.UnplayedScore,
														   Record.MinimumRounds,
														   Record.RoundsToDrop,
														   DropBeforeFinalRound = Record.RoundsToDrop is 0
																					  ? (bool?)null
																					  : Record.DropBeforeFinalRound,
														   Record.RoundsToScale,
														   ScalePercentage = Record.RoundsToScale is 0
																				 ? (int?)null
																				 : Record.ScalePercentage
													   },
											 TeamTournament = Record.TeamSize is 0
																  ? null
																  : new
																	{
																		Record.TeamSize,
																		Record.TeamConflict,
																		Record.TeamsPlayMultipleRounds,
																		TeamRound = Record.TeamsPlayMultipleRounds
																						? (int?)null
																						: Record.TeamRound,
																		Record.PlayerCanJoinManyTeams
																	}
										 };

	private bool IsEvent => Record.IsEvent;

	new internal static IEnumerable<Tournament> GetAll()
		=> Rest<Tournament, Data.Tournament>.GetAll()
											.Where(static tournament => tournament.IsEvent);

	public static IResult GetRegistered(int id)
	{
		var record = Lookup(id);
		return record is null
				   ? Results.NotFound()
				   : Results.Ok(record.Record
									  .TournamentPlayers
									  .Select(static tp => new TournamentPlayer { Record = tp.Player, Rounds = tp.Rounds }));
	}

	public static IResult GetUnregistered(int id)
	{
		var record = Lookup(id);
		if (record is null)
			return Results.NotFound();
		var registered = record.Record.TournamentPlayers.Select(static tp => tp.PlayerId).ToArray();
		return Results.Ok(Player.GetAll()
								.Where(player => !registered.Contains(player.Id)));
	}

	[PublicAPI]
	internal sealed class TournamentPlayer : Player
	{
		required public int[] Rounds { get; init; }
	}
}
