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
        //  Be sure the proper Access database driver is installed on this host computer.
		if (!CheckDatabaseDriver())
			return;

        //	If a db file name is on the command line, use that.  Otherwise, get it from saved settings if possible.
		var dbFileName = args.FirstOrDefault() ?? GetDatabaseFile();

		//	TODO: if (dbFileName is null || !File.Exists(dbFileName))
		//	TODO: ask if they want to create a new empty DCM.dcm file or go surfing (the code below)

		if (OpenDatabase(dbFileName))
			Run(new MainForm());
	}
}
