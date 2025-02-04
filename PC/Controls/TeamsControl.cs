﻿using static Microsoft.VisualBasic.Interaction;

namespace PC.Controls;

internal sealed partial class TeamsControl : UserControl
{
	private TournamentInfoForm TournamentInfoForm { get; set; } = TournamentInfoForm.None;

	private Tournament Tournament => TournamentInfoForm.Tournament;

	private Team Team { get; set; } = Team.None;

	internal TeamsControl()
		=> InitializeComponent();

	//	TODO: Add some code that stops people from creating new teams
	//	TODO: if the tournament has already advanced too far.

	internal void LoadControl(TournamentInfoForm tournamentInfoForm)
	{
		TournamentInfoForm = tournamentInfoForm;
		LastNameRadioButton.Checked = true;
		SetEnabled(false, JoinButton, FormTeamButton);
		FillTeamList();
		TeamSizeLabel.Text = $"Maximum{NewLine}Team Size: {Tournament.TeamSize}";
	}

	private void FillTeamList()
	{
		TeamListBox.FillWithRecords(Tournament.Teams.Order());
		if (!Team.IsNone)
			TeamListBox.SelectedItem = TeamListBox.Find(Team);
		FillMembershipLists();
		if (Team.IsNone)
			SelectNoTeam();
	}

	private void FillMembershipLists(object? sender = null,
									 EventArgs? e = null)
	{
		MemberListBox.Items.Clear();
		if (Team.IsNone)
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
		var nonMembers = candidatePlayers.Where(player => (Tournament.PlayerCanJoinManyTeams || !player.Teams(Tournament).Any())
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
		Team = Team.None;
		SelectNoTeam();
		FormTeamButton.Enabled = true;
	}

	private void SelectNoTeam()
	{
		SetEnabled(false, RenameButton, DissolveButton);
		MemberListBox.Items.Clear();
		NonMemberListBox.Items.Clear();
		MembersLabel.Text = "Members";
	}

	private void JoinButton_Click(object sender,
								  EventArgs e)
	{
		if (NonMemberListBox.SelectedItem is Player joiningPlayer)
		{
			SkipHandlers(NonMemberListBox.ClearSelected);
			Team.AddPlayer(joiningPlayer);
			FillMembershipLists();
			MemberListBox.SelectedItem = MemberListBox.Find(joiningPlayer);
		}
		else if (MemberListBox.SelectedItem is Player leavingPlayer)
		{
			SkipHandlers(MemberListBox.ClearSelected);
			Delete(Team.TeamPlayers.ByPlayerId(leavingPlayer.Id));
			FillMembershipLists();
			NonMemberListBox.SelectedItem = NonMemberListBox.Find(leavingPlayer);
		}
	}

	private void NonMemberListBox_SelectedIndexChanged(object sender,
													   EventArgs e)
	{
		if (SkippingHandlers || Team.IsNone)
			return;
		SkipHandlers(MemberListBox.ClearSelected);
		JoinButton.Enabled = MemberListBox.Items.Count < Tournament.TeamSize
						  && NonMemberListBox.SelectedItem is not null;
		JoinButton.Text = "◀─── Join Team";
	}

	private void MemberListBox_SelectedIndexChanged(object sender,
													EventArgs e)
	{
		if (SkippingHandlers)
			return;
		SkipHandlers(NonMemberListBox.ClearSelected);
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
		Team = team;
		SkipHandlers(() =>
					 {
						 MemberListBox.ClearSelected();
						 NonMemberListBox.ClearSelected();
					 });
		FillMembershipLists();
		SetEnabled(true, RenameButton, DissolveButton);
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
		Team = CreateOne(new Team
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
		Team = Team.None;
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
