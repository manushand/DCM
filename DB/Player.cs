namespace DCM.DB;

internal sealed class Player : IdentityRecord
{
	internal string EmailAddress = Empty;
	internal string FirstName = Empty;
	internal string LastName = Empty;

	internal override string Name => $"{FirstName} {LastName}";

	internal string LastFirst => $"{LastName} {FirstName}";

	internal bool IsHuman => char.IsLetter(FirstName.First());

	internal IEnumerable<string> EmailAddresses => EmailAddress.SplitEmailAddresses();

	internal IEnumerable<Game> Games => LinksOfType<GamePlayer>().Select(static gamePlayer => gamePlayer.Game)
																 .OrderBy(static game => (game.Date, game.Round.Number));

	internal PlayerConflict[] PlayerConflicts => [..ReadMany<PlayerConflict>(playerConflict => playerConflict.PlayerIds.Contains(Id))];

	internal IEnumerable<TeamPlayer> TeamPlayers => LinksOfType<TeamPlayer>();

	internal Group[] Groups => [..LinksOfType<GroupPlayer>().Select(static groupPlayer => groupPlayer.Group)];

	internal string GroupMemberships
	{
		get
		{
			var groups = Groups;
			var groupCount = groups.Length;
			return groupCount is 0
					   ? $"{Name} is not a member of any groups."
					   : $"{Name} is a member of the following {"group".Pluralize(groupCount)}:{groups.BulletList()}";
		}
	}

	internal IEnumerable<T> LinksOfType<T>()
		where T : LinkRecord, new()
		=> ReadMany<T>(linkRecord => linkRecord.PlayerId == Id);

	internal IEnumerable<Team> Teams(int? tournamentId = null)
		=> TeamPlayers.Select(static teamPlayer => teamPlayer.Team)
					  .Where(team => tournamentId is null || team.TournamentId == tournamentId);

	internal IEnumerable<Player> TournamentTeamPlayers(int? tournamentId = null)
		=> Teams(tournamentId).SelectMany(static team => team.Players)
							  .Where(IsNot);

	internal void AddPlayerConflict(Player player)
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
