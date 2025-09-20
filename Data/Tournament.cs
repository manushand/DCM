using System.Text.RegularExpressions;
using static System.Math;

namespace Data;

public sealed partial class Tournament : IdentityRecord<Tournament>, IdInfoRecord.IEvent
{
	#region Public interface

	#region Attribute and Types

	[AttributeUsage(AttributeTargets.Field)]
	public sealed partial class PowerGroupingsAttribute(PowerGroups name, string groups) : Attribute
	{
		public readonly string Text = $"{GroupName.Replace($"{name}", "$1-$2").ToUpper(),-11}({groups})";
		internal readonly string[] Groups = groups.Split('-');
		private static readonly Regex GroupName = GroupNameRegex();

		[GeneratedRegex("(.)([A-Z])")]
		private static partial Regex GroupNameRegex();
	}

	[PublicAPI] // to prevent R# complaining of unused enum values
	public enum PowerGroups : byte
	{
		[PowerGroupings(None, "F-R-I-G-A-T-E")]
		None,
		[PowerGroupings(Corners, "FR-IGA-TE")]
		Corners,
		[PowerGroupings(EastWest, "AGI-EF-RT")]
		EastWest,
		[PowerGroupings(Naval, "AG-EIT-FR")]
		Naval,
		[PowerGroupings(LandSea, "AI-EFG-RT")]
		LandSea,
		[PowerGroupings(FleetNear, "AI-EF-GRT")]
		FleetNear,
		[PowerGroupings(Lepanto, "AIT-EG-FR")]
		Lepanto
	}

	#endregion

	#region Data

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

	public bool IsEvent => Group.IsNone;
	public bool HasTeamTournament => TeamSize > 0;
	public TournamentPlayer[] TournamentPlayers => [..ReadMany<TournamentPlayer>(tournamentPlayer => tournamentPlayer.TournamentId == Id)];
	public Team[] Teams => [..ReadMany<Team>(team => team.TournamentId == Id)];
	public Round[] Rounds => [..ReadMany<Round>(round => round.TournamentId == Id).Order()];
	public Game[] Games => _games ??= [..Rounds.SelectMany(static round => round.Games)];
	public Game[] FinishedGames => [..Games.Where(static game => game.Status is Finished)];

	public ScoringSystem ScoringSystem
	{
		get => field.Id == ScoringSystemId
				   ? field
				   : field = ReadById<ScoringSystem>(ScoringSystemId);
		set
		{
			field = value;
			Rounds.ForSome(round => round.ScoringSystemId == ScoringSystemId,
						   round => round.ScoringSystem = value);
			ScoringSystemId = value.Id;
		}
	} = ScoringSystem.None;

	public Group Group
	{
		get => GroupId > 0
				   ? field.Id != GroupId
						 ? field = ReadById<Group>(GroupId.Value)
						 : field
				   : Group.None;
		internal init => (field, GroupId) = (value, value.Id);
	} = Group.None;

	#endregion

	#region Methods

	public TournamentPlayer AddPlayer(Player player)
		=> CreateOne(new TournamentPlayer { Tournament = this, Player = player });

	public Round CreateRound()
	{
		var roundNumber = Rounds.Length + 1;
		var round = CreateOne(new Round
							  {
								  Tournament = this,
								  Number = roundNumber
							  });
		CreateMany(TournamentPlayers.Where(tournamentPlayer => tournamentPlayer.RegisteredForRound(roundNumber))
									.Select(tournamentPlayer => new RoundPlayer
																{
																	Round = round,
																	Player = tournamentPlayer.Player
																}));
		return round;
	}

	internal void InvalidateGamesCache()
		=> _games = null;

	#region IInfoRecord implementation

	#region IRecord implementation

	public override void Load(DbDataReader record)
	{
		record.CheckDataType<Tournament>();
		Id = record.Integer(nameof (Id));
		Name = record.String(nameof (Name));
		Date = record.NullableDate(nameof (Date)) ?? DateTime.Today;
		Description = record.String(nameof (Description));
		GroupId = record.NullableInteger(nameof (GroupId));
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
		RoundsToScale = scalePercentage.AsInteger;
		ScalePercentage = (scalePercentage % 1m * 100).AsInteger;
		var teamSize = record.Integer(nameof (TeamSize));
		TeamSize = Abs(teamSize);
		PlayerCanJoinManyTeams = teamSize < 0;
		var teamRound = record.Integer(nameof (TeamRound));
		TeamRound = Abs(teamRound);
		TeamsPlayMultipleRounds = teamRound < 0;
		var scoreConflict = record.Integer(nameof (ScoreConflict));
		ScoreConflict = Abs(scoreConflict);
		ProgressiveScoreConflict = scoreConflict < 0;
	}

	#endregion

	public override string FieldValues => $"""
										   [{nameof (Name)}] = {Name.ForSql()},
										   [{nameof (Description)}] = {Description.ForSql()},
										   [{nameof (Date)}] = {Date.ForSql()},
										   [{nameof (ScoringSystemId)}] = {ScoringSystemId},
										   [{nameof (TeamConflict)}] = {TeamConflict},
										   [{nameof (PlayerConflict)}] = {PlayerConflict},
										   [{nameof (PowerConflict)}] = {PowerConflict},
										   [{nameof (TotalRounds)}] = {TotalRounds},
										   [{nameof (MinimumRounds)}] = {MinimumRounds},
										   [{nameof (AssignPowers)}] = {AssignPowers.ForSql()},
										   [{nameof (GroupPowers)}] = {GroupPowers.ForSql()},
										   [{nameof (UnplayedScore)}] = {UnplayedScore},
										   [{nameof (RoundsToDrop)}] = {RoundsToDrop.NegatedIf(DropBeforeFinalRound)},
										   [{nameof (ScalePercentage)}] = {RoundsToScale}.{ScalePercentage},
										   [{nameof (TeamSize)}] = {TeamSize.NegatedIf(PlayerCanJoinManyTeams)},
										   [{nameof (TeamRound)}] = {TeamRound.NegatedIf(TeamsPlayMultipleRounds)},
										   [{nameof (ScoreConflict)}] = {ScoreConflict.NegatedIf(ProgressiveScoreConflict)},
										   [{nameof (GroupId)}] = {GroupId.ForSql()}
										   """;

	#endregion

	#endregion

	#endregion

	#region Private implementation

	private Game[]? _games;

	#endregion
}
