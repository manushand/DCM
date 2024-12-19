using static System.Math;

namespace Data;

public sealed class Tournament : IdentityRecord<Tournament>, IdInfoRecord.IEvent
{
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class PowerGroupingsAttribute(params string[] groups) : Attribute
	{
		public string[] Groups { get; } = groups;
	}

	[PublicAPI] // to prevent R# complaints of unused enum values
	public enum PowerGroups : byte
	{
		[PowerGroupings("F", "R", "I", "G", "A", "T", "E")]
		None,
		[PowerGroupings("AGI", "EF", "RT")]
		EastWest,
		[PowerGroupings("AGI", "ET", "FR")]
		Corners,
		[PowerGroupings("AG", "EIT", "FR")]
		Naval,
		[PowerGroupings("AI", "EFG", "RT")]
		LandSea,
		[PowerGroupings("AI", "EF", "GRT")]
		FleetNear,
		[PowerGroupings("AIT", "EG", "FR")]
		Lepanto
	}

	public DateTime Date;
	public PowerGroups GroupPowers;
	public string Description = Empty;
	public bool AssignPowers;
	public bool DropBeforeFinalRound;
	public bool PlayerCanJoinManyTeams;
	public bool ProgressiveScoreConflict;
	public bool TeamsPlayMultipleRounds;
	public int MinimumRounds;
	public int PlayerConflict;
	public int PowerConflict;
	public int RoundsToDrop;
	public int RoundsToScale;
	public int ScalePercentage;
	public int ScoreConflict;
	public int TeamConflict;
	public int TeamRound;
	public int TeamSize;
	public int TotalRounds;
	public int UnplayedScore; // TODO: this is an int in db; change to double?

	public int ScoringSystemId { get; private set; }
	public int? GroupId { get; private set; }

	public ScoringSystem ScoringSystem
	{
		get => field == ScoringSystem.None && ScoringSystemId > 0
				   ? field = ReadById<ScoringSystem>(ScoringSystemId).OrThrow()
				   : field;
		set
		{
			field = value;
			Rounds.ForSome(round => round.ScoringSystemId == ScoringSystemId, round => round.ScoringSystem = value);
			ScoringSystemId = value.Id;
		}
	} = ScoringSystem.None;

	public Group Group
	{
		get => GroupId is null
				   ? Group.None
				   : field;
		init => (field, GroupId) = (value, value.Id);
	} = Group.None;

	public bool IsEvent => Group != Group.None;

	public bool HasTeamTournament => TeamSize > 0;

	public Round[] Rounds => [..ReadMany<Round>(round => round.TournamentId == Id).OrderBy(static round => round.Number)];

	public Game[] Games => [..Rounds.SelectMany(static round => round.Games)];

	public Game[] FinishedGames => [..Games.Where(static game => game.Status is Finished)];

	public TournamentPlayer[] TournamentPlayers => [..ReadMany<TournamentPlayer>(tournamentPlayer => tournamentPlayer.TournamentId == Id)];

	public Team[] Teams => [..ReadMany<Team>(team => team.TournamentId == Id)];

	//  HostRound for Group games (which are modeled as a single-round Tournament)
	public Round HostRound => Rounds.SingleOrDefault() ?? CreateOne(new Round { Tournament = this });

	public void AddPlayer(Player player)
		=> CreateOne(new TournamentPlayer { Tournament = this, Player = player });

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<Tournament>();
		Id = record.Integer(nameof (Id));
		Name = record.String(nameof (Name));
		Date = record.NullableDate(nameof (Date)) ?? DateTime.Today;
		Description = record.String(nameof (Description));
		ScoringSystemId = record.Integer(nameof (ScoringSystemId));
		TeamConflict = record.Integer(nameof (TeamConflict));
		PlayerConflict = record.Integer(nameof (PlayerConflict));
		PowerConflict = record.Integer(nameof (PowerConflict));
		TotalRounds = record.Integer(nameof (TotalRounds));
		MinimumRounds = record.Integer(nameof (MinimumRounds));
		AssignPowers = record.Boolean(nameof (AssignPowers));
		GroupPowers = record.IntegerAs<PowerGroups>(nameof (GroupPowers));
		UnplayedScore = record.Integer(nameof (UnplayedScore));
		//	Four fields hold two pieces of data each.  Sneaky!
		var roundsToDrop = record.Integer(nameof (RoundsToDrop));
		RoundsToDrop = Abs(roundsToDrop);
		DropBeforeFinalRound = roundsToDrop < 0;
		var scalePercentage = record.Decimal(nameof (ScalePercentage));
		RoundsToScale = (int)scalePercentage;
		ScalePercentage = (int)(scalePercentage % 1m * 100);
		var teamSize = record.Integer(nameof (TeamSize));
		TeamSize = Abs(teamSize);
		PlayerCanJoinManyTeams = teamSize < 0;
		var teamRound = record.Integer(nameof (TeamRound));
		TeamRound = Abs(teamRound);
		TeamsPlayMultipleRounds = teamRound < 0;
		var scoreConflict = record.Integer(nameof (ScoreConflict));
		ScoreConflict = Abs(scoreConflict);
		ProgressiveScoreConflict = scoreConflict < 0;
		GroupId = record.NullableInteger(nameof (GroupId));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (Name)}}] = {0},
	                                           [{{nameof (Description)}}] = {1},
	                                           [{{nameof (Date)}}] = {2},
	                                           [{{nameof (ScoringSystemId)}}] = {3},
	                                           [{{nameof (TeamConflict)}}] = {4},
	                                           [{{nameof (PlayerConflict)}}] = {5},
	                                           [{{nameof (PowerConflict)}}] = {6},
	                                           [{{nameof (TotalRounds)}}] = {7},
	                                           [{{nameof (MinimumRounds)}}] = {8},
	                                           [{{nameof (AssignPowers)}}] = {9},
	                                           [{{nameof (GroupPowers)}}] = {10},
	                                           [{{nameof (UnplayedScore)}}] = {11},
	                                           [{{nameof (RoundsToDrop)}}] = {12},
	                                           [{{nameof (ScalePercentage)}}] = {13}.{14},
	                                           [{{nameof (TeamSize)}}] = {15},
	                                           [{{nameof (TeamRound)}}] = {16},
	                                           [{{nameof (ScoreConflict)}}] = {17},
	                                           [{{nameof (GroupId)}}] = {18}
	                                           """;

	public override string FieldValues => Format(FieldValuesFormat,
												 Name.ForSql(),
												 Description.ForSql(),
												 Date.ForSql(),
												 ScoringSystemId,
												 TeamConflict,
												 PlayerConflict,
												 PowerConflict,
												 TotalRounds,
												 MinimumRounds,
												 AssignPowers.ForSql(),
												 GroupPowers.ForSql(),
												 UnplayedScore,
												 RoundsToDrop.NegatedIf(DropBeforeFinalRound),
												 RoundsToScale, ScalePercentage,
												 TeamSize.NegatedIf(PlayerCanJoinManyTeams),
												 TeamRound.NegatedIf(TeamsPlayMultipleRounds),
												 ScoreConflict.NegatedIf(ProgressiveScoreConflict),
												 GroupId.ForSql());

	#endregion
}
