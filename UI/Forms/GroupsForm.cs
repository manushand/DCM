namespace DCM.UI.Forms;

internal sealed partial class GroupsForm : Form
{
	private Group? Group
	{
		get;
		set
		{
			field = value;
			FillGroupList();
			if (value is not null)
				GroupMembershipControl.Group = value;
			SetEnabled(value is not null, EditButton, DissolveButton);
		}
	}

	public GroupsForm()
		=> InitializeComponent();

	private void GroupsForm_Load(object sender,
								 EventArgs e)
		=> Group = null;

	private void FillGroupList()
	{
		GroupListBox.FillWithSorted<Group>();
		GroupListBox.SelectedItem = GroupListBox.Find(Group);
	}

	private void GroupListBox_SelectedIndexChanged(object sender,
												   EventArgs e)
	{
		if (GroupListBox.SelectedItem is Group group && group != Group)
			Group = group;
	}

	private void DissolveButton_Click(object sender,
									  EventArgs e)
	{
		var group = Group.OrThrow();
		if (MessageBox.Show($"Really dissolve {group}?",
							"Confirm Group Dissolution",
							YesNo,
							Question) is DialogResult.No)
			return;
		var tournament = group.Tournament;
		if (tournament is not null)
		{
			var games = group.Games;
			Delete(games.SelectMany(static game => game.GamePlayers));
			Delete(games);
			Delete(tournament.Rounds);
			Delete(tournament);
		}
		GroupMembershipControl.ClearMemberList();
		Delete(group.Players);
		Delete(group);
		Group = null;
	}

	private void EditButton_Click(object sender,
								  EventArgs e)
	{
		var group = Group.OrThrow();
		Show<GroupInfoForm>(() => new (group, true));
		Group = ReadOne(group, false);
	}

	private void NewGroupButton_Click(object sender,
									  EventArgs e)
		=> Show<GroupInfoForm>(form =>
							   {
								   if (form.DialogResult is not DialogResult.Cancel)
									   Group = form.Group;
							   });
}
