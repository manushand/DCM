namespace PC.Controls;

internal sealed partial class GroupMembershipControl : UserControl
{
	#region Public interface

	#region Data

	[DesignerSerializationVisibility(Hidden)]
	internal Group Group
	{
		private get;
		set
		{
			field = value;
			Disable(JoinButton, MembershipsButton);
			FillMembershipLists();
		}
	} = Group.None;

	#endregion

	#region Constructor

	internal GroupMembershipControl()
		=> InitializeComponent();

	#endregion

	#region Method

	internal void ClearMemberList()
		=> MemberListBox.Clear();

	#endregion

	#endregion

	#region Private implementation

	#region Event handlers

	private void GroupMembershipControl_Load(object sender,
											 EventArgs e)
		=> LastNameRadioButton.Checked = true;

	private void JoinButton_Click(object sender,
								  EventArgs e)
	{
		if (NonMemberListBox.SelectedItems.Count is not 0)
		{
			var joiningPlayers = NonMemberListBox.GetMultiSelected<Player>();
			joiningPlayers.ForEach(player => Group += player);
			SkipHandlers(NonMemberListBox.ClearSelected);
			FillMembershipLists();
			joiningPlayers.ForEach(MemberListBox.SelectedItems.Add);
		}
		else if (MemberListBox.SelectedItems.Count is not 0)
		{
			var leavingPlayers = MemberListBox.GetMultiSelected<Player>();
			leavingPlayers.ForEach(player => Group -= player);
			SkipHandlers(MemberListBox.ClearSelected);
			FillMembershipLists();
			leavingPlayers.ForEach(NonMemberListBox.SelectedItems.Add);
		}
	}

	private void MembershipsButton_Click(object sender,
										 EventArgs e)
		=> MessageBox.Show((MemberListBox.GetSelected<Player>() ?? NonMemberListBox.GetSelected<Player>().OrThrow()).GroupMemberships,
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

	#endregion

	#region Other method

	private void FillMembershipLists(object? sender = null,
									 EventArgs? e = null)
	{
		if (Group.IsNone)
		{
			MemberListBox.Clear();
			NonMemberListBox.Clear();
			MembersLabel.Text = "Members";
			return;
		}
		Player[] members = [..Group.Players
								   .Sorted(LastNameRadioButton.Checked)];
		var memberCount = members.Length;
		var memberPlayerIds = members.Ids;
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

	#endregion

	#endregion
}
