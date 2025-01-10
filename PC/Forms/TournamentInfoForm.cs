namespace PC.Forms;

internal sealed partial class TournamentInfoForm : Form
{
	[DesignerSerializationVisibility(Hidden)]
	internal Tournament Tournament { get; private set; }

	internal static readonly TournamentInfoForm None = new ();

	public TournamentInfoForm() : this(Tournament.None) { }

	internal TournamentInfoForm(Tournament tournament)
	{
		InitializeComponent();
		Tournament = tournament;
	}

	private void TournamentInfoForm_Load(object sender,
										 EventArgs e)
	{
		TournamentControl.LoadControl(this);
		TeamsControl.LoadControl(this);
		SetTeamsTabVisibility();
		DetailsTabControl.SelectedIndex = Tournament.HasTeamTournament
													.AsInteger();
	}

	internal void SetFormTitle()
		=> Text = $"{Tournament} ─ {(Tournament.HasTeamTournament ? "Teams and " : null)}Details";

	internal void SetTeamsTabVisibility()
	{
		if (DetailsTabControl.TabCount == 2 - Tournament.HasTeamTournament.AsInteger())
			DetailsTabControl.AddOrRemove(TeamsTabPage, Tournament.HasTeamTournament);
		SetFormTitle();
	}

	internal void CloseForm()
	{
		Close();
		DialogResult = DialogResult.OK;
	}

	private void TournamentInfoForm_FormClosing(object sender,
												FormClosingEventArgs e)
	{
		if (DialogResult is DialogResult.Cancel && Tournament.Id > 0)
			Tournament = ReadOne(Tournament, false).OrThrow();
	}
}
