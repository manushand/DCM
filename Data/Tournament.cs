using static System.Math;

namespace Data;

public sealed class Tournament : IdentityRecord
{
	public static readonly Tournament None = new ();

	public enum PowerGroups : byte
	{
		None,      //	F-R-I-G-A-T-E
		EastWest,  //	AGI-EF-RT
		Corners,   //	AGI-ET-FT
		Naval,     //	AG-EIT-FR
		LandSea,   //	AI-EFG-RT
		FleetNear, //	AI-EF-GRT
		Lepanto    //	AIT-EG-FR
	}

	private static readonly SortedDictionary<PowerGroups, string[]> PowerInitialGroups =
		new ()
		{
			[PowerGroups.None] = ["F", "R", "I", "G", "A", "T", "E"],
			[EastWest] = ["AGI", "EF", "RT"],
			[Corners] = ["AGI", "ET", "FR"],
			[Naval] = ["AG", "EIT", "FR"],
			[LandSea] = ["AI", "EFG", "RT"],
			[FleetNear] = ["AI", "EF", "GRT"],
			[Lepanto] = ["AIT", "EG", "FR"]
		};

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
	public int UnplayedScore; //	TODO: this is an int in db; change to double?

	public int? GroupId { get; private set; }
	public int ScoringSystemId { get; private set; }

	public ScoringSystem ScoringSystem
	{
		get => field == ScoringSystem.None
				   ? field = ReadById<ScoringSystem>(ScoringSystemId).OrThrow()
				   : field;
		set
		{
			field = value;
			ScoringSystemId = value.Id;
			Rounds.ForSome(round => round.ScoringSystemId == ScoringSystemId, round => round.ScoringSystem = value);
		}
	} = ScoringSystem.None;

	public Group? Group
	{
		get => GroupId is null
				   ? null
				   : field ??= ReadById<Group>(GroupId.Value).OrThrow();
		init => (field, GroupId) = (value, value?.Id);
	}

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

	//	TODO: is this the best location for this method?
	internal bool SharePowerGroup(PowerNames power1,
								  PowerNames power2)
	{
		var powerGroupings = PowerInitialGroups[GroupPowers];
		return GroupContaining(power1) == GroupContaining(power2);

		string GroupContaining(PowerNames power)
			=> powerGroupings.Single(group => group.Contains(power.Abbreviation()));
	}

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
