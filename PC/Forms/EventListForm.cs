namespace PC.Forms;

internal sealed partial class EventListForm : Form
{
	#region Exposed property

	[DesignerSerializationVisibility(Hidden)]
	internal Tournament? Tournament { get; private set; }

	#endregion

	#region Constructors

	public EventListForm()
		=> InitializeComponent();

	internal EventListForm(bool delete) : this()
		=> _delete = delete;

	#endregion

	#region Private field

	private readonly bool _delete;

	#endregion

	#region Event handler methods

	private void TournamentListForm_Load(object sender,
										 EventArgs e)
	{
		Text = _delete
				   ? "Delete Event"
				   : "Open Event";
		TournamentComboBox.FillWith(ReadMany<Tournament>(static tournament => tournament.GroupId is null)
										.OrderByDescending(static tournament => tournament.Date));
	}

	private void TournamentComboBox_SelectedIndexChanged(object sender,
														 EventArgs e)
	{
		if (TournamentComboBox.SelectedItem is not Tournament tournament)
			return;
		if (_delete)
		{
			if (MessageBox.Show($"Are you absolutely sure you want to delete{NewLine}{tournament}?!?{NewLine}{NewLine}THERE IS NO UNDO!",
								"Confirm Event Deletion",
								YesNo,
								Question) is DialogResult.No)
				return;
			var teams = tournament.Teams;
			var rounds = tournament.Rounds;
			var games = tournament.Games;
			Delete(games.SelectMany(static game => game.GamePlayers));
			Delete(games);
			Delete(rounds.SelectMany(static round => round.RoundPlayers));
			Delete(rounds);
			Delete(teams.SelectMany(static team => team.TeamPlayers));
			Delete(teams);
			Delete(tournament.TournamentPlayers);
			Delete(tournament);
			if (Tournament?.Id == tournament.Id)
				Tournament = null;
		}
		else
			Tournament = tournament;
		Close();
		DialogResult = DialogResult.OK;
	}

	#endregion
}
