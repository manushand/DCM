using static System.Windows.Forms.Application;

namespace DCM;

file static class Program
{
	static Program()
	{
		EnableVisualStyles();
		SetCompatibleTextRenderingDefault(false);
	}

	[STAThread]
	private static void Main(string[] args)
	{
		//  Be sure the proper database driver is installed on this host computer.
		if (!CheckDriver())
			return;

		//	If a db file name is on the command line, use that.  Otherwise, get it from saved settings if possible.
		var dbFileName = args.FirstOrDefault() ?? GetDatabaseFile();

		//	TODO: if (dbFileName is null || !File.Exists(dbFileName))
		//	TODO: ask if they want to create a new empty DCM.dcm file or go surfing (the code below)

		if (OpenDatabase(dbFileName))
			Run(new MainForm());
	}
}
