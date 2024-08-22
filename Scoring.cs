namespace DCM;

//	This class (and each of its public members) is public so that C# formulas have access to it
[PublicAPI]
public sealed partial class Scoring
{
	public enum PowerNames : sbyte
	{
		//	IMPORTANT: Values must be -1 through 6 to match ComboBox item order
		TBD = -1,
		Austria = 0,
		England = 1,
		France = 2,
		Germany = 3,
		Italy = 4,
		Russia = 5,
		Turkey = 6
	}

	private ScoringSystem ScoringSystem { get; }

	private Dictionary<PowerNames, GamePlayer> GamePlayers { get; }

	internal PowerNames PlayerPower
	{
		set => _player = Powers[value];
	}

	internal Scoring(ScoringSystem scoringSystem,
					 IReadOnlyCollection<GamePlayer> gamePlayers)
	{
		ScoringSystem = scoringSystem;
		GamePlayers = gamePlayers.ToDictionary(static gamePlayer => gamePlayer.Power, static gamePlayer => gamePlayer);
		Powers = gamePlayers.ToDictionary(static gamePlayer => gamePlayer.Power, gamePlayer => new PowerData(this, gamePlayer));
		AllPowerYears = [..gamePlayers.Select(static gamePlayer => gamePlayer.Years ?? 0)];
	}

	internal void UpdateProvisionalScores()
		=> Powers.Keys.ForEach(power => Powers[power].ProvisionalScore = GamePlayers[power].ProvisionalScore);

	internal void UpdatePlayerAntes()
		=> Powers.Keys.ForEach(power => Powers[power].PlayerAnte = GamePlayers[power].PlayerAnte);

	#region Scoring properties

	#region Scoring properties shared by all players

	//	Properties that are public are [UsedImplicitly] because they are exposed for use in C# formulae

	private PowerData? _player;

	//	C# Formula use case: if (Player is Austria) ...
	public PowerData Player => _player.OrThrow();

	public Dictionary<PowerNames, PowerData> Powers { get; }

	public int PointsPerGame => ScoringSystem.PointsPerGame ?? 0;

	//	C# Formula use case: Austria.Centers, England.Scoring, etc.
	public PowerData Austria => Powers[PowerNames.Austria];

	public PowerData England => Powers[PowerNames.England];

	public PowerData France => Powers[PowerNames.France];

	public PowerData Germany => Powers[PowerNames.Germany];

	public PowerData Italy => Powers[PowerNames.Italy];

	public PowerData Russia => Powers[PowerNames.Russia];

	public PowerData Turkey => Powers[PowerNames.Turkey];

	public int Winners => Powers.Count(static power => power.Value.Won);
	public int Losers => Powers.Count(static power => power.Value.Lost);
	public int DrawSize => Winners;
	public int Survivors => Powers.Count(static power => power.Value.Survived);
	public int LeaderCenters => Powers.Max(static power => power.Value.Centers);

	public int FewestCenters => Powers.Where(static power => power.Value.Centers > 0)
									  .Min(static power => power.Value.Centers);

	public int SurvivorsCenters => Powers.Where(static power => power.Value.Lost)
										 .Sum(static power => power.Value.Centers);

	public int WinnersCenters => Powers.Where(static power => power.Value.Won)
									   .Sum(static power => power.Value.Centers);

	public int GameYears => Powers.Max(static power => power.Value.Years);

	public decimal SumOfCentersSquared => Powers.Values
												.Sum(static power => power.CentersSquared);

	public decimal SumOfYears => AllPowerYears.Sum();
	public decimal SumOfProvisionalScores => AllProvisionalScores.Sum();
	public decimal AverageProvisionalScore => AllProvisionalScores.Average();
	public decimal SumOfPlayerAntes => AllPlayerAntes.Sum();
	public decimal AveragePlayerAnte => AllPlayerAntes.Average();
	public decimal LowestPlayerAnte => AllPlayerAntes.Min();
	public decimal HighestPlayerAnte => AllPlayerAntes.Max();
	public decimal SumOfRunningScores => RunningScores.Sum();
	public decimal AverageRunningScore => RunningScores.Average();
	public decimal LowestRunningScore => RunningScores.Min();
	public decimal HighestRunningScore => RunningScores.Max();
	public decimal AverageOfAverageGameScores => AverageGameScores.Average();
	public decimal SumOfOtherScores => AllOtherScores.Sum();
	public decimal SumOfEveryOtherScore => SumOfOtherScores;
	public decimal AverageOtherScore => AllOtherScores.Average();
	public decimal LowestOtherScore => AllOtherScores.Min();
	public decimal HighestOtherScore => AllOtherScores.Max();

	private List<int> AllPowerYears { get; }

	private IEnumerable<decimal> AllProvisionalScores => Powers.Select(static power => power.Value.ProvisionalScore);

	private IEnumerable<decimal> AllPlayerAntes => Powers.Select(static power => power.Value.PlayerAnte);

	private IEnumerable<decimal> RunningScores => Powers.Select(static power => power.Value.RunningScore);

	private IEnumerable<decimal> AverageGameScores => Powers.Select(static power => power.Value.AverageGameScore);

	private IEnumerable<decimal> AllOtherScores => Powers.Select(static power => power.Value.OtherScore);

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
	public decimal EliminationOrder => Player.EliminationOrder;
	public decimal CenterRank => Player.CenterRank;
	public decimal ProvisionalScore => Player.ProvisionalScore;
	public decimal AverageGameScore => Player.AverageGameScore;
	public decimal RunningScore => Player.RunningScore;
	public decimal PlayerAnte => Player.PlayerAnte;
	public decimal OtherScore => Player.OtherScore;

	#endregion

	#endregion
}
