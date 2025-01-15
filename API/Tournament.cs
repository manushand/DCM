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
		var tournament = Lookup(id)?.Data;
		return tournament is null
				   ? Results.NotFound()
				   : Results.Ok(tournament.TournamentPlayers
										  .Select(static tp => new TournamentPlayer { Data = tp.Player, Rounds = tp.Rounds }));
	}

	public static IResult GetUnregistered(int id)
	{
		var tournament = Lookup(id)?.Data;
		if (tournament is null)
			return Results.NotFound();
		var registered = tournament.TournamentPlayers.Select(static tp => tp.PlayerId)
								   .ToArray();
		return Results.Ok(Player.GetAll()
								.Where(player => !registered.Contains(player.Id)));
	}

	public static IResult RegisterPlayer(int id, int playerId, int[] rounds)
	{
		var tournament = Lookup(id)?.Data;
		var player = ReadById<Data.Player>(playerId);
		if (tournament is null || player.IsNone)
			return Results.NotFound();
		if (rounds.Any(round => round < 1 || round > tournament.TotalRounds))
			return Results.BadRequest();
		var tournamentPlayer = tournament.TournamentPlayers
										 .SingleOrDefault(tournamentPlayer => tournamentPlayer.PlayerId == playerId);
		var roundList = Enumerable.Range(1, tournament.TotalRounds)
								  .ToArray();
		if (tournamentPlayer is null)
			tournamentPlayer = CreateOne(new Data.TournamentPlayer { Tournament = tournament, Player = player });
		else if (roundList.Any(roundNumber => !rounds.Contains(roundNumber)
										   && tournament.Games
														.Any(game => game.Round.Number == roundNumber
																  && game.GamePlayers.Any(gamePlayer => gamePlayer.PlayerId == playerId))))
			return Results.Conflict();
		foreach (var roundNumber in roundList)
			if (rounds.Contains(roundNumber))
				tournamentPlayer.RegisterForRound(roundNumber);
			else
				tournamentPlayer.UnregisterForRound(roundNumber);
		UpdateOne(tournamentPlayer);
		return Results.Ok(tournamentPlayer);
	}

	public static IResult UnregisterPlayer(int id, int playerId)
	{
		var tournament = Lookup(id)?.Data;
		var player = ReadById<Data.Player>(playerId);
		if (tournament is null || player.IsNone)
			return Results.NotFound();
		var tournamentPlayer = tournament.TournamentPlayers.SingleOrDefault(tournamentPlayer => tournamentPlayer.PlayerId == playerId);
		if (tournamentPlayer is null)
			return Results.NoContent();
		if (tournament.Games.Any(game => game.GamePlayers.Any(gamePlayer => gamePlayer.PlayerId == playerId)))
			return Results.Conflict();
		Delete(tournament.Rounds.SelectMany(round => round.RoundPlayers.Where(roundPlayer => roundPlayer.PlayerId == playerId)));
		Delete(tournamentPlayer);
		return Results.Ok();
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
