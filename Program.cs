using static System.Windows.Forms.Application;

namespace DCM;

using UI.Forms;

internal static class Program
{
	static Program()
	{
		var font = BoldFonts.GetOrSet(Control.DefaultFont, BoldFont);
		PowerColors.Values.ForEach(value => (value.Font, value.Alignment) = (font, MiddleCenter));
		EnableVisualStyles();
		SetCompatibleTextRenderingDefault(false);
	}

	[STAThread]
	private static void Main(string[] args)
	{
		if (!CheckDatabaseDriver())
			return;

		var dbFileName = GetDatabaseFile(args);

		//	TODO: if (dbFileName is null || !File.Exists(dbFileName))
		//	TODO: ask if they want to create a new empty DCM.dcm file or go surfing (the code below)

		if (OpenDatabase(dbFileName))
			Run(new MainForm { StartPosition = FormStartPosition.CenterScreen });
	}
}
