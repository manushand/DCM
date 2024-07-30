using static System.Diagnostics.Stopwatch;

namespace DCM.UI.Forms;

internal sealed partial class WaitForm : Form
{
	internal int Result { get; private set; }
	internal decimal? ElapsedMilliseconds { get; private set; }

	private Func<int> Function { get; }

	internal WaitForm(string caption,
					  Func<int> function)
	{
		InitializeComponent();
		Text =
			ActivityLabel.Text =
				$"{caption}…";
		Function = function;
	}

	private void WaitForm_Load(object sender,
							   EventArgs e)
	{
		var watch = Settings.ShowTimingData
						? StartNew()
						: null;
		Task.Factory
			.StartNew(Function)
			.ContinueWith(task =>
						  {
							  watch?.Stop();
							  ElapsedMilliseconds = watch?.ElapsedMilliseconds;
							  Result = task.Result;
							  Close();
						  }, TaskScheduler.FromCurrentSynchronizationContext());
	}
}
