namespace PC;

file static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.Run(new MainForm());
	}
}
