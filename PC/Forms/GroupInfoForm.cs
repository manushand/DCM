namespace PC.Forms;

internal sealed partial class GroupInfoForm : Form
{
	[DesignerSerializationVisibility(Hidden)]
	internal Group Group { get; private set; } = Group.None;

	private bool DetailsOnly { get; }

	private bool GroupHasGames => Group is { IsNone: false, Games.Length: > 0 };

	public GroupInfoForm()
		=> InitializeComponent();

	internal GroupInfoForm(Group group,
						   bool detailsOnly = false) : this()
		=> (Group, DetailsOnly) = (group, detailsOnly);

	private void GroupInfoForm_Load(object sender,
									EventArgs e)
	{
		Text = Group.IsNone
				   ? "New Group Details"
				   : $"{Group} ─ Details{(DetailsOnly ? null : " and Members")}";
		ScoringSystemComboBox.FillWithSorted<ScoringSystem>();
		if (!GroupHasGames)
			ScoringSystemComboBox.Items
								 .Insert(0, ScoringSystem.None);
		if (Group.IsNone || DetailsOnly)
		{
			Width -= GroupMembershipControl.Width + 20;
			if (Group.IsNone)
			{
				ConflictTextBox.Text = "0";
				ScoringSystemComboBox.SelectedIndex = default;
				ScoringSystemComboBox_SelectedIndexChanged();
				return;
			}
		}
		GroupMembershipControl.Group = Group;
		GroupNameTextBox.Text = Group.Name;
		DescriptionTextBox.Text = Group.Description;
		ConflictTextBox.Text = $"{Group.Conflict}";
		ScoringSystemComboBox.SetSelectedItem(Group.ScoringSystem);
		if (ScoringSystemComboBox.SelectedIndex > 0)
			ScoringSystemComboBox_SelectedIndexChanged();
	}

	private void OkButton_Click(object sender,
								EventArgs e)
	{
		var scoringSystem = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		//	This next "if" should never be true, since the No-Scoring-System option isn't shown if GroupHasGames.
		//	But in case somehow (or someday if that option IS restored when a GroupHasGames) this is happening,
		//	the user should know what will happen if they make the Group one that doesn't have a ScoringSystem.
		//  (Actually, I believe this could happen even now after a setting the system for the first time ever,
		//  which doesn't remove the None option, then creating a game all in the same run of this Form.)
		if (scoringSystem.IsNone
		&&  GroupHasGames
		&&  MessageBox.Show("By removing the scoring system, all Group Games will be deleted. Really proceed?",
							"Delete all Group Games?",
							YesNo,
							Question) is DialogResult.No)
			return;
		var groupName = GroupNameTextBox.Text
										.Trim();
		string error;
		var conflict = 0;
		if (groupName.Length is 0)
			error = "Player Group must have a name.";
		else if (ReadOne<Group>(group => group.Name.Matches(groupName) && group.Id != Group.Id) is not null)
			error = "Another Player Group with that name already exists.";
		else if (ConflictTextBox.TextLength is not 0
			 && !int.TryParse(ConflictTextBox.Text, out conflict))
			error = "Tournament conflict value must be numeric.";
		else
		{
			if (Group.IsNone)
				Group = CreateOne(new Group
								  {
									  Name = groupName,
									  Description = DescriptionTextBox.Text,
									  Conflict = conflict
								  });
			else
			{
				Group.Name = groupName;
				Group.Description = DescriptionTextBox.Text;
				Group.Conflict = conflict;
				UpdateOne(Group);
			}
			Group.ScoringSystem = scoringSystem;
			DialogResult = DialogResult.OK;
			Close();
			return;
		}
		MessageBox.Show(error,
						"Invalid Group Details",
						OK,
						Error);
	}

	private void GroupInfoForm_FormClosing(object sender,
										   FormClosingEventArgs e)
	{
		if (DialogResult is not DialogResult.Cancel)
			return;
		//	Reload from database since any mucking with it changed it in the Cache
		Group = ReadOne(Group, false) ?? Group.None;
		var current = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		e.Cancel = (Group.ScoringSystemId != current.Id
				   || $"{Group.Conflict}" != ConflictTextBox.Text.Trim()
				   || (Group.IsNone ? Empty : Group.Name) != GroupNameTextBox.Text.Trim()
				   || Group.Description != DescriptionTextBox.Text.Trim())
				&& MessageBox.Show("Abandon changes to the Group?",
								   "Changes Will Be Lost",
								   YesNo,
								   Warning) is DialogResult.No;
	}

	private void ScoringSystemComboBox_SelectedIndexChanged(object? sender = null,
															EventArgs? e = null)
	{
		var system = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		var hasSystem = !system.IsNone;
		SetVisible(hasSystem, RatingMethodLabel, RatingMethodDescriptionLabel);
		if (hasSystem)
			RatingMethodDescriptionLabel.Text = system.UsesPlayerAnte || system.PointsPerGame is 0
													? "Sum of Game Scores"
													: "Average of Game Scores";
	}
}
