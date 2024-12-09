namespace PC.Controls;

internal interface IScoreControl
{
	int Width { get; }

	void LoadControl(Tournament tournament);
}
