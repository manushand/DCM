namespace PC.Forms;

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
		SetVisible(ScoringSystemListBox.SelectedItem is not null, OpenButton, DeleteButton);
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
		OpenButton.Show();
		DeleteButton.Show();
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
									.Where(static group => !group.IsNone)
									.ToList();
			tournaments = [..tournaments.Where(static tournament => tournament.IsEvent)];
			var usedInGroups = groups.Count > 0;
			var message = $"The {scoringSystem} scoring system is used ";
			if (tournaments.Count > 0)
				message += $"in {UsedInList(tournaments, "event")}{(usedInGroups ? $"{NewLine}{NewLine}and " : null)}";
			if (usedInGroups)
				message += $"by {UsedInList(groups, "group")}";
			MessageBox.Show(message,
							"Scoring System Uses",
							OK,
							Information);

			static string UsedInList(IReadOnlyCollection<object> objects,
									 string what)
				=> $"{(objects.Count is 1 ? "this" : "these")} {what.Pluralize(objects)}:{objects.BulletList()}";
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
