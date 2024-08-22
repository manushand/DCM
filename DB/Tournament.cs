using static System.Math;

namespace DCM.DB;

using static Tournament.PowerGroups;

internal sealed class Tournament : IdentityRecord
{
	internal enum PowerGroups : byte
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
			[None] = [ "F", "R", "I", "G", "A", "T", "E" ],
			[EastWest] = [ "AGI", "EF", "RT" ],
			[Corners] = [ "AGI", "ET", "FR" ],
			[Naval] = [ "AG", "EIT", "FR" ],
			[LandSea] = [ "AI", "EFG", "RT" ],
			[FleetNear] = [ "AI", "EF", "GRT" ],
			[Lepanto] = [ "AIT", "EG", "FR" ]
		};

	private Group? _group;
	private ScoringSystem? _scoringSystem;

	internal bool AssignPowers;
	internal DateTime Date;
	internal string Description = Empty;
	internal bool DropBeforeFinalRound;
	internal PowerGroups GroupPowers;
	internal int MinimumRounds;
	internal bool PlayerCanJoinManyTeams;
	internal int PlayerConflict;
	internal int PowerConflict;
	internal bool ProgressiveScoreConflict;
	internal int RoundsToDrop;
	internal int RoundsToScale;
	internal int ScalePercentage;
	internal int ScoreConflict;
	internal int TeamConflict;
	internal int TeamRound;
	internal int TeamSize;
	internal bool TeamsPlayMultipleRounds;
	internal int TotalRounds;
	internal int UnplayedScore; //	TODO: this is an int in db; change to decimal?

	internal int? GroupId { get; private set; }
	internal int ScoringSystemId { get; private set; }

	internal ScoringSystem ScoringSystem
	{
		get => _scoringSystem ??= ReadById<ScoringSystem>(ScoringSystemId).OrThrow();
		set
		{
			_scoringSystem = value;
			ScoringSystemId = value.Id;
			Rounds.ForSome(round => round.ScoringSystemId == ScoringSystemId, round => round.ScoringSystem = value);
		}
	}

	internal Group? Group
	{
		get => GroupId is null
				   ? null
				   : _group ??= ReadById<Group>(GroupId.Value).OrThrow();
		init => (_group, GroupId) = (value, value?.Id);
	}

	internal bool HasTeamTournament => TeamSize > 0;

	internal Round[] Rounds => [..ReadMany<Round>(round => round.TournamentId == Id).OrderBy(static round => round.Number)];

	internal Game[] Games => [..Rounds.SelectMany(static round => round.Games)];

	internal Game[] FinishedGames => [..Games.Where(static game => game.Status is Finished)];

	internal TournamentPlayer[] TournamentPlayers => [..ReadMany<TournamentPlayer>(tournamentPlayer => tournamentPlayer.TournamentId == Id)];

	internal Team[] Teams => [..ReadMany<Team>(team => team.TournamentId == Id)];

	internal Round AddRound()
		=> CreateOne(new Round { Tournament = this });

	internal void AddPlayer(Player player)
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
