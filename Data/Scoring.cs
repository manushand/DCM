namespace Data;

using static GamePlayer;

//	This class is public (as are most of its members) so that C# formulas have access to it
[PublicAPI]
public sealed partial class Scoring
{
	internal static readonly Scoring None = new (ScoringSystem.None, []);
	internal bool IsNone => ScoringSystem.IsNone;

	private ScoringSystem ScoringSystem { get; }
	private Dictionary<PowerNames, GamePlayer> GamePlayers { get; }

	internal PowerNames PlayerPower
	{
		private get;
		set;
	} = TBD;

	//	For hacking the aliasing into C# scoring formulae by replacing each of the aliases with these.
	internal static readonly string[] OtherScoreAliases =
	[
		//	Ordered by descending length
		nameof (OtherScoreNotValidated),
		nameof (SumOfEveryOtherScore),
		nameof (AverageOtherScore),
		nameof (HighestOtherScore),
		nameof (LowestOtherScore),
		nameof (SumOfOtherScores),
		nameof (OtherScore)
	];

	private Scoring() : this(ScoringSystem.None, []) { }

	internal Scoring(ScoringSystem scoringSystem,
					 IReadOnlyCollection<GamePlayer> gamePlayers)
	{
		ScoringSystem = scoringSystem;
		GamePlayers = gamePlayers.ToDictionary(static gamePlayer => gamePlayer.Power, static gamePlayer => gamePlayer);
		Powers = gamePlayers.ToDictionary(static gamePlayer => gamePlayer.Power, gamePlayer => new PowerData(this, gamePlayer));
	}

	internal void UpdateProvisionalScores()
		=> Powers.ForEach(power => power.Value.ProvisionalScore = GamePlayers[power.Key].ProvisionalScore);

	internal void UpdatePlayerAntes()
		=> Powers.ForEach(power => power.Value.PlayerAnte = GamePlayers[power.Key].PlayerAnte);

	#region Scoring fields and properties

	private IEnumerable<double> AllProvisionalScores => Powers.Select(static power => power.Value.ProvisionalScore);
	private IEnumerable<double> AllPlayerAntes => Powers.Select(static power => power.Value.PlayerAnte);
	private IEnumerable<double> RunningScores => Powers.Select(static power => power.Value.RunningScore);
	private IEnumerable<double> AverageGameScores => Powers.Select(static power => power.Value.AverageGameScore);
	private IEnumerable<double> AllOtherScores => Powers.Select(static power => power.Value.OtherScore);

	#region Scoring properties shared by all players

	public bool OtherScoreNotValidated => PlayerPower is PowerNames.Austria;

	//  C# Formula use case: foreach (var (name, data) in Powers) ...
	public Dictionary<PowerNames, PowerData> Powers { get; }

	//	C# Formula use case: if (Player == Austria) ...
	public PowerData Player => Powers[PlayerPower];

	//	C# Formula use case: Austria.Centers, England.Scoring, etc.
	public PowerData Austria => Powers[PowerNames.Austria];
	public PowerData England => Powers[PowerNames.England];
	public PowerData France => Powers[PowerNames.France];
	public PowerData Germany => Powers[PowerNames.Germany];
	public PowerData Italy => Powers[PowerNames.Italy];
	public PowerData Russia => Powers[PowerNames.Russia];
	public PowerData Turkey => Powers[PowerNames.Turkey];

	public bool SingleWinner => Winners is 1;

	public int PointsPerGame => ScoringSystem.PointsPerGame ?? 0;
	public int Losers => Powers.Count(static power => power.Value.Lost);
	public int Survivors => Powers.Count(static power => power.Value.Survived);
	public int Winners => Powers.Count(static power => power.Value.Won);
	public int DrawSize => Winners;
	public int LeaderCenters => Powers.Max(static power => power.Value.Centers);
	public int FewestCenters => Powers.Where(static power => power.Value.Centers > 0)
									  .Min(static power => power.Value.Centers);
	public int SurvivorsCenters => Powers.Where(static power => power.Value.Lost)
										 .Sum(static power => power.Value.Centers);
	public int WinnersCenters => Powers.Where(static power => power.Value.Won)
									   .Sum(static power => power.Value.Centers);
	public int SumOfCentersSquared => Powers.Sum(static power => power.Value.CentersSquared);
	public int GameYears => Powers.Max(static power => power.Value.Years);
	public int SumOfYears => Powers.Sum(static power => power.Value.Years);
	public int UnplayedYears => (ScoringSystem.FinalGameYear - 1900 ?? GameYears) - Player.Years;

	public double SumOfProvisionalScores => AllProvisionalScores.Sum();
	public double AverageProvisionalScore => AllProvisionalScores.Average();
	public double SumOfPlayerAntes => AllPlayerAntes.Sum();
	public double AveragePlayerAnte => AllPlayerAntes.Average();
	public double LowestPlayerAnte => AllPlayerAntes.Min();
	public double HighestPlayerAnte => AllPlayerAntes.Max();
	public double SumOfRunningScores => RunningScores.Sum();
	public double AverageRunningScore => RunningScores.Average();
	public double LowestRunningScore => RunningScores.Min();
	public double HighestRunningScore => RunningScores.Max();
	public double AverageOfAverageGameScores => AverageGameScores.Average();
	public double SumOfOtherScores => AllOtherScores.Sum();
	public double SumOfEveryOtherScore => SumOfOtherScores;
	public double AverageOtherScore => AllOtherScores.Average();
	public double LowestOtherScore => AllOtherScores.Min();
	public double HighestOtherScore => AllOtherScores.Max();

	#endregion

	#region Scoring properties specific to the scoring player

	public bool Won => Player.Won;
	public bool WonDraw => Player.WonDraw;
	public bool WonAlone => Player.WonAlone;
	public bool WonSolo => Player.WonSolo;
	public bool WonConcession => Player.WonConcession;
	public bool Lost => Player.Lost;
	public bool Eliminated => Player.Eliminated;
	public bool Uneliminated => Player.Uneliminated;
	public bool Survived => Player.Survived;
	public bool SurvivedSolo => Player.SurvivedSolo;
	public bool SurvivedDraw => Player.SurvivedDraw;
	public bool SurvivedConcession => Player.SurvivedConcession;
	public bool LostToSoleWinner => Player.LostToSoleWinner;
	public bool Conceded => Player.Conceded;
	public bool WasLeader => Player.WasLeader;

	public int Centers => Player.Centers;
	public int Years => Player.Years;
	public int CentersSquared => Player.CentersSquared;
	public int WorstCenterRank => Player.WorstCenterRank;
	public int BestCenterRank => Player.BestCenterRank;
	public int CenterRankSharers => Player.CenterRankSharers;
	public int WorstSurvivorRank => Player.WorstSurvivorRank;
	public int BestSurvivorRank => Player.BestSurvivorRank;
	public int SurvivorRankSharers => Player.SurvivorRankSharers;
	public int EliminatedEarlier => Player.EliminatedEarlier;
	public int BestEliminationOrder => Player.BestEliminationOrder;
	public int WorstEliminationOrder => Player.WorstEliminationOrder;
	public int EliminationOrderSharers => Player.EliminationOrderSharers;

	public double EliminationOrder => Player.EliminationOrder;
	public double CenterRank => Player.CenterRank;
	public double ProvisionalScore => Player.ProvisionalScore;
	public double AverageGameScore => Player.AverageGameScore;
	public double RunningScore => Player.RunningScore;
	public double PlayerAnte => Player.PlayerAnte;
	public double OtherScore => Player.OtherScore;

	#endregion

	#endregion
}
