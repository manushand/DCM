namespace PC.Controls;

internal sealed partial class GroupMembershipControl : UserControl
{
	[DesignerSerializationVisibility(Hidden)]
	internal Group Group
	{
		private get;
		set
		{
			field = value;
			SetEnabled(false, JoinButton, MembershipsButton);
			FillMembershipLists();
		}
	} = Group.None;

	internal GroupMembershipControl()
		=> InitializeComponent();

	internal void ClearMemberList()
		=> MemberListBox.Items.Clear();

	private void JoinButton_Click(object sender,
								  EventArgs e)
	{
		if (NonMemberListBox.SelectedItems.Count is not 0)
		{
			var joiningPlayers = NonMemberListBox.GetMultiSelected<Player>();
			CreateMany(joiningPlayers.Select(player => new GroupPlayer { Group = Group, Player = player }).ToArray());
			SkipHandlers(NonMemberListBox.ClearSelected);
			FillMembershipLists();
			joiningPlayers.ForEach(MemberListBox.SelectedItems.Add);
		}
		else if (MemberListBox.SelectedItems.Count is not 0)
		{
			var leavingPlayers = MemberListBox.GetMultiSelected<Player>();
			int[] playerIds = [..leavingPlayers.Ids()];
			Delete<GroupPlayer>(groupPlayer => groupPlayer.GroupId == Group.Id
											&& playerIds.Contains(groupPlayer.PlayerId));
			SkipHandlers(MemberListBox.ClearSelected);
			FillMembershipLists();
			leavingPlayers.ForEach(NonMemberListBox.SelectedItems.Add);
		}
	}

	private void FillMembershipLists(object? sender = null,
									 EventArgs? e = null)
	{
		if (Group.IsNone)
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
		if (SkippingHandlers)
			return;
		SkipHandlers(MemberListBox.ClearSelected);
		SetEnabled(NonMemberListBox.SelectedItem is not null, JoinButton, MembershipsButton);
		JoinButton.Text = "◀───── Join";
	}

	private void MemberListBox_SelectedIndexChanged(object sender,
													EventArgs e)
	{
		if (SkippingHandlers)
			return;
		SkipHandlers(NonMemberListBox.ClearSelected);
		SetEnabled(MemberListBox.SelectedItem is not null, JoinButton, MembershipsButton);
		JoinButton.Text = "Leave ─────▶";
	}

	private void GroupMembershipControl_Load(object sender,
											 EventArgs e)
		=> LastNameRadioButton.Checked = true;
}
