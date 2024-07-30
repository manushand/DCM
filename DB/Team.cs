namespace DCM.DB;

internal sealed class Team : IdentityRecord
{
	internal int TournamentId;

	internal TeamPlayer[] TeamPlayers => [..ReadMany<TeamPlayer>(teamPlayer => teamPlayer.TeamId == Id)];

	internal Player[] Players => [..TeamPlayers.Select(static teamPlayer => teamPlayer.Player)];

	internal void AddPlayer(Player player)
		=> CreateOne(new TeamPlayer { Team = this, Player = player });

	#region IInfoRecord interface implementation

	#region IRecord interface implementation

	public override IRecord Load(DbDataReader record)
	{
		record.CheckDataType<Team>();
		Id = record.Integer(nameof (Id));
		Name = record.String(nameof (Name));
		TournamentId = record.Integer(nameof (TournamentId));
		return this;
	}

	#endregion

	private const string FieldValuesFormat = $$"""
	                                           [{{nameof (Name)}}] = {0},
	                                           [{{nameof (TournamentId)}}] = {1}
	                                           """;

	public override string FieldValues => Format(FieldValuesFormat,
												 Name.ForSql(),
												 TournamentId);

	#endregion
}
