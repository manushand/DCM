namespace DCM.UI.Forms;

using Controls;

internal sealed partial class ScoresForm : Form
{
	private Tournament Tournament { get; }

	internal ScoresForm(Tournament tournament)
	{
		InitializeComponent();
		Tournament = tournament;
	}

	private void ScoresForm_Load(object sender,
								 EventArgs e)
	{
		Text = $"{Tournament} ─ Scores";
		ScoresTabControl_SelectedIndexChanged();
	}

	private void ScoresTabControl_SelectedIndexChanged(object? sender = null,
													   EventArgs? e = null)
	{
		var scoreControl = ScoresTabControl.SelectedTab
										   .OrThrow()
										   .Controls
										   .OfType<IScoreControl>()
										   .Single();
		scoreControl.LoadControl(Tournament);
		ScoresTabControl.Width = scoreControl.Width + 20;
		Width = ScoresTabControl.Width + 40;
	}
}
