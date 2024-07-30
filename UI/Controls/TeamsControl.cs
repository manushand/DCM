using static Microsoft.VisualBasic.Interaction;

namespace DCM.UI.Controls;

internal sealed partial class TeamsControl : UserControl
{
	private Team? _team;
	private TournamentInfoForm? _tournamentInfoForm;

	private TournamentInfoForm TournamentInfoForm => _tournamentInfoForm.OrThrow();

	private Tournament Tournament => TournamentInfoForm.Tournament;

	private Team Team => _team.OrThrow();

	internal TeamsControl()
		=> InitializeComponent();

	//	TODO: Add some code that stops people from creating new teams
	//	TODO: if the tournament has already advanced too far.

	internal void LoadControl(TournamentInfoForm tournamentInfoForm)
	{
		_tournamentInfoForm = tournamentInfoForm;
		LastNameRadioButton.Checked = true;
		JoinButton.Enabled =
			FormTeamButton.Enabled =
				false;
		FillTeamList();
		TeamSizeLabel.Text = $"Maximum{NewLine}Team Size: {Tournament.TeamSize}";
	}

	private void FillTeamList()
	{
		TeamListBox.FillWithRecords(Tournament.Teams.Order());
		if (_team is not null)
			TeamListBox.SelectedItem = TeamListBox.Find(Team);
		FillMembershipLists();
		if (_team is null)
			SelectNoTeam();
	}

	private void FillMembershipLists(object? sender = null,
									 EventArgs? e = null)
	{
		MemberListBox.Items.Clear();
		if (_team is null)
			SelectNoTeam();
		else
		{
			var members = Team.Players
							  .Sorted(LastNameRadioButton.Checked)
							  .ToArray();
			var memberCount = members.Length;
			var selectedMember = MemberListBox.GetSelected<Player>();
			MemberListBox.FillWithRecords(members);
			if (selectedMember is not null)
				MemberListBox.SelectedItem = MemberListBox.Find(selectedMember);
			MembersLabel.Text = "Member".Pluralize(memberCount, true);
		}

		//	TODO: Hmm, looks like we only include tournament-registered players in the non-member list?
		var memberIds = MemberListBox.GetAll<Player>()
									 .Ids();
		var selectedNonMember = NonMemberListBox.GetSelected<Player>();
		var candidatePlayers = WhichPlayersTabControl.SelectedIndex is 0
								   ? ReadMany<TournamentPlayer>(tp => tp.TournamentId == Tournament.Id).Select(static tp => tp.Player)
								   : ReadAll<Player>();
		var nonMembers = candidatePlayers.Where(player => (Tournament.PlayerCanJoinManyTeams
														|| !player.Teams(Tournament.Id).Any())
													   && !memberIds.Contains(player.Id))
										 .Sorted(LastNameRadioButton.Checked);
		NonMemberListBox.FillWith(nonMembers);
		if (selectedNonMember is not null)
			NonMemberListBox.SelectedItem = NonMemberListBox.Find(selectedNonMember);
	}

	private void NewTeamNameTextBox_Enter(object sender,
										  EventArgs e)
	{
		TeamListBox.ClearSelected();
		_team = null;
		SelectNoTeam();
		FormTeamButton.Enabled = true;
	}

	private void SelectNoTeam()
	{
		RenameButton.Enabled =
			DissolveButton.Enabled =
				false;
		MemberListBox.Items.Clear();
		NonMemberListBox.Items.Clear();
		MembersLabel.Text = "Members";
	}

	private void JoinButton_Click(object sender,
								  EventArgs e)
	{
		if (NonMemberListBox.SelectedItem is Player joiningPlayer)
		{
			SkipHandlers = true;
			NonMemberListBox.ClearSelected();
			SkipHandlers = false;
			Team.AddPlayer(joiningPlayer);
			FillMembershipLists();
			MemberListBox.SelectedItem = MemberListBox.Find(joiningPlayer);
		}
		else if (MemberListBox.SelectedItem is Player leavingPlayer)
		{
			SkipHandlers = true;
			MemberListBox.ClearSelected();
			SkipHandlers = false;
			Delete(Team.TeamPlayers.ByPlayerId(leavingPlayer.Id));
			FillMembershipLists();
			NonMemberListBox.SelectedItem = NonMemberListBox.Find(leavingPlayer);
		}
	}

	private void NonMemberListBox_SelectedIndexChanged(object sender,
													   EventArgs e)
	{
		if (SkipHandlers || _team is null)
			return;
		SkipHandlers = true;
		MemberListBox.ClearSelected();
		SkipHandlers = false;
		JoinButton.Enabled = MemberListBox.Items.Count < Tournament.TeamSize
						  && NonMemberListBox.SelectedItem is not null;
		JoinButton.Text = "◀─── Join Team";
	}

	private void MemberListBox_SelectedIndexChanged(object sender,
													EventArgs e)
	{
		if (SkipHandlers)
			return;
		SkipHandlers = true;
		NonMemberListBox.ClearSelected();
		SkipHandlers = false;
		JoinButton.Enabled = MemberListBox.SelectedItem is not null;
		JoinButton.Text = "Leave Team ─▶";
	}

	private void TeamListBox_SelectedIndexChanged(object sender,
												  EventArgs e)
	{
		if (TeamListBox.SelectedItem is not Team team)
			return;
		NewTeamNameTextBox.Text = null;
		FormTeamButton.Enabled = false;
		_team = team;
		SkipHandlers = true;
		MemberListBox.ClearSelected();
		NonMemberListBox.ClearSelected();
		SkipHandlers = false;
		FillMembershipLists();
		RenameButton.Enabled =
			DissolveButton.Enabled =
				true;
		JoinButton.Enabled = false;
	}

	private void FormTeamButton_Click(object sender,
									  EventArgs e)
	{
		var teamName = NewTeamNameTextBox.Text
										 .Trim();
		if (teamName.Length is 0)
			return;
		if (Tournament.Teams.Any(team => team.Name.Matches(teamName)))
		{
			MessageBox.Show("A Team with that name already exists.",
							"Team Exists",
							OK,
							Error);
			return;
		}
		_team = CreateOne(new Team
						  {
							  TournamentId = Tournament.Id,
							  Name = teamName
						  });
		FillTeamList();
	}

	private void DissolveButton_Click(object sender,
									  EventArgs e)
	{
		if (MessageBox.Show($"Really dissolve {Team}?",
							"Confirm Team Dissolution",
							YesNo,
							Question) is DialogResult.No)
			return;
		Delete(Team.TeamPlayers);
		Delete(Team);
		_team = null;
		FillTeamList();
	}

	private void RenameButton_Click(object sender,
									EventArgs e)
	{
		var teamName = InputBox("Please provide the new name for this team.", $"Rename {Team}", Team.Name).Trim();

		//	Cancel produces an empty string
		if (teamName.Length is 0)
			return;
		if (Tournament.Teams.Any(team => team.Name.Matches(teamName) && team.IsNot(Team)))
			MessageBox.Show("Another team with that name already exists.",
							"Invalid Team Name",
							OK,
							Error);
		else
		{
			Team.Name = teamName;
			UpdateOne(Team);
			FillTeamList();
		}
	}
}
