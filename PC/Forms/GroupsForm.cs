namespace PC.Forms;

internal sealed partial class GroupsForm : Form
{
	private Group Group
	{
		get;
		set
		{
			field = value;
			FillGroupList();
			var enabled = value != Group.None;
			if (enabled)
				GroupMembershipControl.Group = value;
			SetEnabled(enabled, EditButton, DissolveButton);
		}
	} = Group.None;

	public GroupsForm()
		=> InitializeComponent();

	private void GroupsForm_Load(object sender,
								 EventArgs e)
		=> Group = Group.None;

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
		var hostRound = group.HostRound;
		if (!hostRound.IsNone)
		{
			var games = group.Games;
			Delete(games.SelectMany(static game => game.GamePlayers));
			Delete(games);
			Delete(hostRound);
			Delete(hostRound.Tournament);
		}
		GroupMembershipControl.ClearMemberList();
		Delete(group.Players);
		Delete(group);
		Group = Group.None;
	}

	private void EditButton_Click(object sender,
								  EventArgs e)
	{
		var group = Group.OrThrow();
		Show<GroupInfoForm>(() => new (group, true));
		Group = ReadOne(group, false).OrThrow();
	}

	private void NewGroupButton_Click(object sender,
									  EventArgs e)
		=> Show<GroupInfoForm>(form =>
							   {
								   if (form.DialogResult is not DialogResult.Cancel)
									   Group = form.Group;
							   });
}
