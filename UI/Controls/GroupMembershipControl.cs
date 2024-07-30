namespace DCM.UI.Controls;

internal sealed partial class GroupMembershipControl : UserControl
{
	private Group? _group;

	internal Group Group
	{
		private get => _group.OrThrow();
		set
		{
			_group = value;
			JoinButton.Enabled =
				MembershipsButton.Enabled =
					false;
			FillMembershipLists();
		}
	}

	internal GroupMembershipControl()
		=> InitializeComponent();

	internal void ClearMemberList()
		=> MemberListBox.Items.Clear();

	private void JoinButton_Click(object sender,
								  EventArgs e)
	{
		if (NonMemberListBox.SelectedItems.Count > 0)
		{
			var joiningPlayers = NonMemberListBox.GetMultiSelected<Player>();
			CreateMany(joiningPlayers.Select(player => new GroupPlayer { Group = Group, Player = player }));
			SkipHandlers = true;
			NonMemberListBox.ClearSelected();
			SkipHandlers = false;
			FillMembershipLists();
			joiningPlayers.ForEach(MemberListBox.SelectedItems.Add);
		}
		else if (MemberListBox.SelectedItems.Count > 0)
		{
			var leavingPlayers = MemberListBox.GetMultiSelected<Player>();
			var playerIds = leavingPlayers.Ids()
										  .ToArray();
			Delete<GroupPlayer>(groupPlayer => groupPlayer.GroupId == Group.Id
											&& playerIds.Contains(groupPlayer.PlayerId));
			SkipHandlers = true;
			MemberListBox.ClearSelected();
			SkipHandlers = false;
			FillMembershipLists();
			leavingPlayers.ForEach(NonMemberListBox.SelectedItems.Add);
		}
	}

	private void FillMembershipLists(object? sender = null,
									 EventArgs? e = null)
	{
		if (_group is null)
		{
			MemberListBox.Items
						 .Clear();
			NonMemberListBox.Items
							.Clear();
			MembersLabel.Text = "Members";
			return;
		}
		var members = Group.Players
						   .Sorted(LastNameRadioButton.Checked)
						   .ToArray();
		var memberCount = members.Length;
		var memberPlayerIds = members.Ids();
		var selectedMember = MemberListBox.GetSelected<Player>();
		MemberListBox.FillWithRecords(members);
		if (selectedMember is not null)
			MemberListBox.SelectedItem = MemberListBox.Find(selectedMember);
		MembersLabel.Text = "Member".Pluralize(memberCount, true);

		var selectedNonMember = NonMemberListBox.GetSelected<Player>();
		var nonMembers = ReadMany<Player>(player => !memberPlayerIds.Contains(player.Id)).Sorted(LastNameRadioButton.Checked);
		NonMemberListBox.FillWith(nonMembers);
		if (selectedNonMember is not null)
			NonMemberListBox.SelectedItem = NonMemberListBox.Find(selectedNonMember);
	}

	private void MembershipsButton_Click(object sender,
										 EventArgs e)
		=> MessageBox.Show((MemberListBox.GetSelected<Player>()
						 ?? NonMemberListBox.GetSelected<Player>().OrThrow()).GroupMemberships,
						   "Group Memberships",
						   OK,
						   None);

	private void NonMemberListBox_SelectedIndexChanged(object sender,
													   EventArgs e)
	{
		if (SkipHandlers)
			return;
		SkipHandlers = true;
		MemberListBox.ClearSelected();
		SkipHandlers = false;
		JoinButton.Enabled =
			MembershipsButton.Enabled =
				NonMemberListBox.SelectedItem is not null;
		JoinButton.Text = "◀───── Join";
	}

	private void MemberListBox_SelectedIndexChanged(object sender,
													EventArgs e)
	{
		if (SkipHandlers)
			return;
		SkipHandlers = true;
		NonMemberListBox.ClearSelected();
		SkipHandlers = false;
		JoinButton.Enabled =
			MembershipsButton.Enabled =
				MemberListBox.SelectedItem is not null;
		JoinButton.Text = "Leave ─────▶";
	}

	private void GroupMembershipControl_Load(object sender,
											 EventArgs e)
		=> LastNameRadioButton.Checked = true;
}
