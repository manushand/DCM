using PC.Controls;

namespace PC.Forms;

internal sealed partial class ScoresForm : Form
{
	#region Public interface

	#region Constructor

	internal ScoresForm(Tournament tournament)
	{
		InitializeComponent();
		Tournament = tournament;
	}

	#endregion

	#endregion

	#region Private implementation

	#region Data

	private Tournament Tournament { get; }

	#endregion

	#region Event handlers

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

	#endregion

	#endregion
}
