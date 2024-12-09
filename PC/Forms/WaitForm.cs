using static System.Diagnostics.Stopwatch;

namespace PC.Forms;

internal sealed partial class WaitForm : Form
{
	private readonly Func<int> _function;

	[DesignerSerializationVisibility(Hidden)]
	internal int Result { get; private set; }

	[DesignerSerializationVisibility(Hidden)]
	internal long? ElapsedMilliseconds { get; private set; }

	internal WaitForm(string caption,
					  Func<int> function)
	{
		InitializeComponent();
		Text =
			ActivityLabel.Text =
				$"{caption}…";
		_function = function;
	}

	private void WaitForm_Load(object sender,
							   EventArgs e)
	{
		var watch = Settings.ShowTimingData
						? StartNew()
						: null;
		Task.Run(_function)
			.ContinueWith(task =>
						  {
							  watch?.Stop();
							  ElapsedMilliseconds = watch?.ElapsedMilliseconds;
							  Result = task.Result;
							  Close();
						  }, TaskScheduler.FromCurrentSynchronizationContext());
	}
}
