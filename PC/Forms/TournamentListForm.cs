namespace PC.Forms;

internal sealed partial class TournamentListForm : Form
{
	[DesignerSerializationVisibility(Hidden)]
	internal Tournament? Tournament { get; private set; }

	private bool Delete { get; }

	public TournamentListForm()
		=> InitializeComponent();

	internal TournamentListForm(bool delete) : this()
		=> Delete = delete;

	private void TournamentListForm_Load(object sender,
										 EventArgs e)
	{
		Text = Delete
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
		if (Delete)
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
}
