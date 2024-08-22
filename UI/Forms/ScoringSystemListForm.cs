namespace DCM.UI.Forms;

internal sealed partial class ScoringSystemListForm : Form
{
	private const string UsedIn = "Used In…";

	public ScoringSystemListForm()
		=> InitializeComponent();

	private void ScoringSystemListForm_Load(object sender,
											EventArgs e)
		=> FillSystemList();

	private void FillSystemList(ScoringSystem? scoringSystem = null)
	{
		ScoringSystemListBox.FillWithSorted<ScoringSystem>();
		if (scoringSystem is not null)
			ScoringSystemListBox.SelectedItem = ScoringSystemListBox.Find(scoringSystem);
		OpenButton.Visible =
			DeleteButton.Visible =
				ScoringSystemListBox.SelectedItem is not null;
	}

	private void ScoringSystemListBox_MouseDoubleClick(object sender,
													   MouseEventArgs e)
	{
		if (ScoringSystemListBox.SelectedItem is not null)
			OpenButton_Click();
	}

	private void ScoringSystemListBox_SelectedIndexChanged(object sender,
														   EventArgs e)
	{
		var scoringSystem = ScoringSystemListBox.GetSelected<ScoringSystem>();
		if (scoringSystem is null)
			return;
		OpenButton.Visible =
			DeleteButton.Visible =
				true;
		DeleteButton.Text = scoringSystem.Tournaments.Count is 0
                                ? "Delete…"
								: UsedIn;
	}

	private void OpenButton_Click(object? sender = null,
								  EventArgs? e = null)
	{
		var scoringSystem = ScoringSystemListBox.GetSelected<ScoringSystem>()
												.OrThrow();
		Show<ScoringSystemInfoForm>(() => new (scoringSystem),
									_ => FillSystemList(scoringSystem));
	}

	private void NewButton_Click(object sender,
								 EventArgs e)
	{
		var newSystem = new ScoringSystem();
		Show<ScoringSystemInfoForm>(() => new (newSystem),
									form =>
									{
										if (form.DialogResult is DialogResult.OK)
											FillSystemList(newSystem);
									});
	}

	private void DeleteButton_Click(object sender,
									EventArgs e)
	{
		var scoringSystem = ScoringSystemListBox.GetSelected<ScoringSystem>()
												.OrThrow();
		if (DeleteButton.Text is UsedIn)
		{
			var tournaments = scoringSystem.Tournaments;
			var groups = tournaments.Select(static tournament => tournament.Group)
									.OfType<Group>()
									.ToList();
			var groupCount = groups.Count;
			var usedInGroups = groupCount > 0;
			tournaments = [..tournaments.Where(static tournament => tournament.Group is null)];
			var tournamentCount = tournaments.Count;
			var message = $"The {scoringSystem} scoring system is used ";
			if (tournamentCount > 0)
			{
				message += $"in {ThisOrThese(tournamentCount, "tournament")}:{tournaments.BulletList()}";
				if (usedInGroups)
					message += $"{NewLine}{NewLine}and ";
			}
			if (usedInGroups)
				message += $"by {ThisOrThese(groupCount, "group")}:{groups.BulletList()}";
			MessageBox.Show(message,
							"Scoring System Uses",
							OK,
							Information);

			static string ThisOrThese(int count,
									  string what)
				=> $"{(count is 1 ? "this" : "these")} {what.Pluralize(count)}";
		}
		else if (MessageBox.Show($"Really delete the {scoringSystem} scoring system?",
								 "Confirm Scoring System Deletion",
								 YesNo,
								 Question) is DialogResult.Yes)
		{
			Delete(scoringSystem);
			FillSystemList();
		}
	}
}
