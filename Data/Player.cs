namespace Data;

public sealed class Player : IdentityRecord<Player>
{
	public string EmailAddress = Empty;
	public string FirstName = Empty;
	public string LastName = Empty;

	public override string Name => $"{FirstName} {LastName}";

	public string LastFirst => $"{LastName} {FirstName}";

	public bool IsHuman => char.IsLetter(FirstName.First());

	public ICollection<string> EmailAddresses => EmailAddress.SplitEmailAddresses();

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

	public T[] LinksOfType<T>()
		where T : LinkRecord, new()
		=> [..ReadMany<T>(linkRecord => linkRecord.PlayerId == Id)];

	public IEnumerable<Team> Teams(Tournament tournament)
		=> LinksOfType<TeamPlayer>().Select(static teamPlayer => teamPlayer.Team)
									.Where(team => tournament.IsNone || team.TournamentId == tournament.Id);

	internal IEnumerable<Player> TournamentTeamPlayers(Tournament tournament)
		=> Teams(tournament).SelectMany(static team => team.Players)
							.Where(IsNot);

	public void AddPlayerConflict(Player player)
		=> CreateOne(new PlayerConflict(Id, player.Id));

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<Player>();
		Id = record.Integer(nameof (Id));
		FirstName = record.String(nameof (FirstName));
		LastName = record.String(nameof (LastName));
		EmailAddress = record.String(nameof (EmailAddress));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (FirstName)}}] = {0},
	                                           [{{nameof (LastName)}}] = {1},
	                                           [{{nameof (EmailAddress)}}] = {2}
	                                           """;

	public override string FieldValues => Format(FieldValuesFormat,
												 FirstName.ForSql(),
												 LastName.ForSql(),
												 EmailAddress.ForSql());

	#endregion
}
