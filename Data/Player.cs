namespace Data;

public sealed class Player : IdentityRecord<Player>
{
	#region Public interface

	#region Data

	public string EmailAddress = Empty;
	public string FirstName = Empty;
	public string LastName = Empty;

	public override string Name => $"{FirstName} {LastName}";

	public string LastFirst => $"{LastName} {FirstName}";

	public bool IsHuman => char.IsLetter(FirstName.First());

	public ICollection<string> EmailAddresses => EmailAddress.AsEmailAddresses;

	public IEnumerable<Game> Games => LinksOfType<GamePlayer>().Select(static gamePlayer => gamePlayer.Game)
															   .OrderBy(static game => (game.Date, game.Round.Number));

	public PlayerConflict[] PlayerConflicts => [..ReadMany<PlayerConflict>(playerConflict => playerConflict.Involves(Id))];

	public Group[] Groups => [..LinksOfType<GroupPlayer>().Select(static groupPlayer => groupPlayer.Group)];

	public string GroupMemberships
	{
		get
		{
			var groups = Groups;
			var groupCount = groups.Length;
			return groupCount is 0
					   ? $"{Name} is not a member of any groups."
					   : groups.BulletList($"{Name} is a member of the following {"group".Pluralize(groupCount)}");
		}
	}

	#endregion

	#region Methods

	public T[] LinksOfType<T>()
		where T : LinkRecord, new()
		=> [..ReadMany<T>(linkRecord => linkRecord.PlayerId == Id)];

	public IEnumerable<Team> Teams(Tournament tournament)
		=> LinksOfType<TeamPlayer>().Select(static teamPlayer => teamPlayer.Team)
									.Where(team => tournament.IsNone || team.TournamentId == tournament.Id);

	public void AddPlayerConflict(Player player)
		=> CreateOne(new PlayerConflict(Id, player.Id));

	internal IEnumerable<Player> TournamentTeamPlayers(Tournament tournament)
		=> Teams(tournament).SelectMany(static team => team.Players)
							.Where(IsNot);

	#endregion

	#region IInfoRecord implementation

	#region IRecord implementation

	public override void Load(DbDataReader record)
	{
		record.CheckDataType<Player>();
		Id = record.Integer(nameof (Id));
		FirstName = record.String(nameof (FirstName));
		LastName = record.String(nameof (LastName));
		EmailAddress = record.String(nameof (EmailAddress));
	}

	#endregion

	public override string FieldValues => Format($$"""
												   [{{nameof (FirstName)}}] = {0},
												   [{{nameof (LastName)}}] = {1},
												   [{{nameof (EmailAddress)}}] = {2}
												   """,
												 FirstName.ForSql(),
												 LastName.ForSql(),
												 EmailAddress.ForSql());

	#endregion

	#endregion
}
