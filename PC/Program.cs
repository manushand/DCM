using static System.Windows.Forms.Application;

namespace PC;

file static class Program
{
	static Program()
	{
		EnableVisualStyles();
		SetCompatibleTextRenderingDefault(false);
	}

	[STAThread]
	private static void Main()
		=> Run(new MainForm());
}
