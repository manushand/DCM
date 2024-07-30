namespace DCM.UI.Controls;

internal interface IScoreControl
{
	int Width { get; }

	void LoadControl(Tournament tournament);
}
