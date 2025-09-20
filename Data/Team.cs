namespace Data;

public sealed class Team : IdentityRecord<Team>
{
	#region Public interface

	#region Data

	public int TournamentId;

	public TeamPlayer[] TeamPlayers => [..ReadMany<TeamPlayer>(teamPlayer => teamPlayer.TeamId == Id)];

	public Player[] Players => [..TeamPlayers.Select(static teamPlayer => teamPlayer.Player)];

	#endregion

	#region Methods

	public static Team operator +(Team team, Player player)
	{
		CreateOne(new TeamPlayer { Team = team, Player = player });
		return team;
	}

	public static Team operator -(Team team, Player player)
	{
		Delete(team.TeamPlayers.ByPlayerId(player.Id));
		return team;
	}

	#endregion

	#region IInfoRecord implementation

	#region IRecord implementation

	public override void Load(DbDataReader record)
	{
		record.CheckDataType<Team>();
		Id = record.Integer(nameof (Id));
		Name = record.String(nameof (Name));
		TournamentId = record.Integer(nameof (TournamentId));
	}

	#endregion

	public override string FieldValues => Format($$"""
												   [{{nameof (Name)}}] = {0},
												   [{{nameof (TournamentId)}}] = {1}
												   """,
												 Name.ForSql(),
												 TournamentId);

	#endregion

	#endregion
}
