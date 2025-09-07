namespace PC.Forms;

internal sealed partial class EventInfoForm : Form
{
	#region Exposed field and property

	internal static readonly EventInfoForm None = new ();

	[DesignerSerializationVisibility(Hidden)]
	internal Tournament Event { get; private set; }

	#endregion

	#region Constructors

	public EventInfoForm() : this(Tournament.None) { }

	internal EventInfoForm(Tournament @event)
	{
		InitializeComponent();
		Event = @event;
	}

	#endregion

	#region Exposed methods

	internal void SetFormTitle()
		=> Text = $"{(Event.IsNone ? "New Event" : Event.Name)} ─ {(Event.HasTeamTournament ? "Teams and " : null)}Details";

	internal void SetTeamsTabVisibility()
	{
		if (DetailsTabControl.TabCount == 2 - Event.HasTeamTournament.AsInteger)
			DetailsTabControl.AddOrRemove(TeamsTabPage, Event.HasTeamTournament);
		SetFormTitle();
	}

	internal void CloseForm()
	{
		Close();
		DialogResult = DialogResult.OK;
	}

	#endregion

	#region Event handler methods

	private void TournamentInfoForm_Load(object sender,
										 EventArgs e)
	{
		EventControl.LoadControl(this);
		TeamsControl.LoadControl(this);
		SetTeamsTabVisibility();
		DetailsTabControl.SelectedIndex = Event.HasTeamTournament
											   .AsInteger;
	}

	private void TournamentInfoForm_FormClosing(object sender,
												FormClosingEventArgs e)
	{
		if (DialogResult is DialogResult.Cancel && Event.Id > 0)
			Event = ReadOne(Event, false).OrThrow();
	}

	#endregion
}
