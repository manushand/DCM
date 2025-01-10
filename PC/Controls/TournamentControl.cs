using static System.Math;

namespace PC.Controls;

using static Tournament;

internal sealed partial class TournamentControl : UserControl
{
	private TournamentInfoForm TournamentInfoForm { get; set; } = TournamentInfoForm.None;

	private Tournament Tournament => TournamentInfoForm.Tournament;

	internal TournamentControl()
	{
		InitializeComponent();
		PowerGroupComboBox.Items.AddRange(Enum.GetNames<PowerGroups>()
											  .Select(static object (name) => ((typeof (PowerGroups).GetField(name)
																									?.GetCustomAttribute(typeof (PowerGroupingsAttribute))
																					as PowerGroupingsAttribute)
																				  ?.Text).OrThrow())
											  .ToArray());
	}

	private void TournamentControl_Load(object sender,
										EventArgs e)
		=> TotalRoundsComboBox.FillRange(2, 9); //	TODO: should this be 7 instead of 9 (for the 1 << x in registration)

	internal void LoadControl(TournamentInfoForm tournamentInfoForm)
	{
		SkipHandlers(() =>
        {
			TournamentInfoForm = tournamentInfoForm;
			ScoringSystemComboBox.FillWithSorted<ScoringSystem>();
			MinimumRoundsComboBox.FillRange(1, 9);
			if (Tournament.Id is 0)
			{
				Text = "New Tournament ─ Settings";
				CopyButton.Hide();
				Tournament.TotalRounds = 2;
				Tournament.MinimumRounds = 1;
				TotalRoundsComboBox.SelectedIndex =
					MinimumRoundsComboBox.SelectedIndex =
						PowerAssignmentComboBox.SelectedIndex =
							PowerGroupComboBox.SelectedIndex =
								TeamSizeComboBox.SelectedIndex =
									default;
				UnplayedScoreTextBox.Text = "0";
				PlayerConflictTextBox.Text =
					PowerConflictTextBox.Text =
						"1";
				DateTimePicker.Value = DateTime.Today;
			}
			else
			{
				ScoringSystemComboBox.SetSelectedItem(Tournament.ScoringSystem);
				TournamentInfoForm.SetFormTitle();
				NameTextBox.Text = Tournament.Name;
				DescriptionTextBox.Text = Tournament.Description;
				TotalRoundsComboBox.SelectedIndex = Tournament.TotalRounds - 2;
				MinimumRoundsComboBox.SelectedIndex = Tournament.MinimumRounds - 1;
				UnplayedScoreTextBox.Text = $"{Tournament.UnplayedScore}";
				DropCheckBox.Checked = Tournament.RoundsToDrop is not 0;
				RoundsToDropComboBox.SelectedIndex = Abs(Tournament.RoundsToDrop) - 1;
				DropWhenComboBox.SelectedIndex = Tournament.DropBeforeFinalRound
														   .AsInteger();
				ScaleCheckBox.Checked = Tournament.RoundsToScale is not 0;
				RoundsToScaleComboBox.SelectedIndex = Abs(Tournament.RoundsToScale) - 1;
				ScaleFactorTextBox.Text = $"{Tournament.ScalePercentage}";
				PowerGroupComboBox.SelectedIndex = Tournament.GroupPowers
															 .AsInteger();
				PowerAssignmentComboBox.SelectedIndex = (!Tournament.AssignPowers).AsInteger();
				PlayerConflictTextBox.Text = $"{Tournament.PlayerConflict}";
				PowerConflictTextBox.Text = $"{Tournament.PowerConflict}";
				ScoreConflictTextBox.Text = $"{Tournament.ScoreConflict}";
				TeamSizeComboBox.SelectedIndex = Max(Tournament.TeamSize - 2, 0);
				DateTimePicker.Value = Tournament.Date;
				if (!Tournament.HasTeamTournament)
					return;
				TeamMemberConflictTextBox.Text = $"{Tournament.TeamConflict}";
				TeamRoundComboBox.SelectedIndex = Tournament.TeamRound - 1;
				TeamScoringComboBox.SelectedIndex = Tournament.TeamsPlayMultipleRounds
															  .AsInteger();
				MultiTeamMembershipCheckBox.Checked = Tournament.PlayerCanJoinManyTeams;
			}
        });
		//	Run the handlers that were skipped
		if (!DropCheckBox.Checked)
			DropCheckBox_CheckedChanged();
		if (!ScaleCheckBox.Checked)
			ScaleCheckBox_CheckedChanged();
		TeamSizeComboBox_SelectedIndexChanged();
		SetScoreConflictDetails();
	}

	private void TotalRoundsComboBox_SelectedIndexChanged(object sender,
														  EventArgs e)
	{
		var totalRounds =
			Tournament.TotalRounds =
				TotalRoundsComboBox.SelectedIndex + 2;
		SetEnabled(true, MinimumRoundsComboBox, RoundsToDropComboBox, RoundsToScaleComboBox);
		//	Hold the ComboBox's SelectedIndex before refilling it
		var minimumRounds = MinimumRoundsComboBox.SelectedIndex;
		MinimumRoundsComboBox.FillRange(1, totalRounds);
		if (minimumRounds > -1)
		{
			Tournament.MinimumRounds = Min(Tournament.MinimumRounds, totalRounds);
			MinimumRoundsComboBox.SelectedIndex = Tournament.MinimumRounds - 1;
		}
		//	Hold the ComboBox's SelectedIndex before refilling it
		var dropCount = RoundsToDropComboBox.SelectedIndex;
		RoundsToDropComboBox.FillRange(1, totalRounds - 1);
		if (dropCount > -1)
		{
			Tournament.RoundsToDrop = Min(Tournament.RoundsToDrop, totalRounds - 1);
			dropCount = RoundsToDropComboBox.SelectedIndex = Tournament.RoundsToDrop - 1;
		}
		//	Hold the ComboBox's SelectedIndex before refilling it
		var scaleCount = RoundsToScaleComboBox.SelectedIndex;
		RoundsToScaleComboBox.FillRange(1, totalRounds - Max(1, dropCount));
		if (scaleCount > -1)
		{
			Tournament.RoundsToScale = Min(Tournament.RoundsToScale, RoundsToScaleComboBox.Items.Count - 1);
			RoundsToScaleComboBox.SelectedIndex = Tournament.RoundsToScale - 1;
		}
		//	Hold the ComboBox's SelectedIndex before refilling it
		var teamRound = TeamRoundComboBox.SelectedIndex;
		TeamRoundComboBox.FillRange(1, totalRounds);
		if (!Tournament.HasTeamTournament || teamRound < 0)
			return;
		Tournament.TeamRound = Min(Tournament.TeamRound, totalRounds);
		TeamRoundComboBox.SelectedIndex = Tournament.TeamRound - 1;
	}

	private void PowerAssignmentComboBox_SelectedIndexChanged(object sender,
															  EventArgs e)
		=> Tournament.AssignPowers =
			   PowerGroupLabel.Visible =
				   PowerGroupComboBox.Visible =
					   RepeatingPowerLabel.Visible =
						   PowerConflictTextBox.Visible =
							   PowerConflictPointsLabel.Visible =
								   PowerAssignmentComboBox.SelectedIndex is 0;

	private void PowerGroupComboBox_SelectedIndexChanged(object sender,
														 EventArgs e)
	{
		Tournament.GroupPowers = (PowerGroups)PowerGroupComboBox.SelectedIndex;
		RepeatingPowerLabel.Text = $"Conflict for playing {(Tournament.GroupPowers is PowerGroups.None
																? "the same power"
																: "in a power group")} more than once:";
		ToolTip.SetToolTip(RepeatingPowerLabel,
						   PowerGroupComboBox.SelectedIndex is 0
							   ? null
							   : "Playing the exact same power will get ten times as many conflict points.");
	}

	private void MinimumRoundsComboBox_SelectedIndexChanged(object sender,
															EventArgs e)
		=> Tournament.MinimumRounds = MinimumRoundsComboBox.SelectedIndex + 1;

	private void TeamSizeComboBox_SelectedIndexChanged(object? sender = null,
													   EventArgs? e = null)
	{
		if (SkippingHandlers)
			return;
		var teamSize = TeamSizeComboBox.SelectedIndex;
		if (teamSize > 0)
		{
			teamSize += 2;
			var minimumTeamSize = Tournament.Teams
											.Select(static team => team.Players.Length)
											.DefaultIfEmpty()
											.Max();
			if (teamSize < minimumTeamSize)
			{
				MessageBox.Show($"Teams exist for this tournament which have as many as {minimumTeamSize} members.{NewLine}{NewLine}" +
								$"All such teams will need to be modified to be able to set the team size to {teamSize}.",
								$"Cannot Set Team Size to {teamSize}",
								OK,
								Stop);
				SkipHandlers(() => TeamSizeComboBox.SelectedIndex = Tournament.TeamSize
                                                                  - (Tournament.TeamSize is 0 ? 0 : 2));
				return;
			}
		}
		Tournament.TeamSize = teamSize;
		SetVisible(Tournament.HasTeamTournament,
				   TeamMemberConflictLabel, TeamMemberConflictTextBox, TeamScoringLabel, TeamScoringComboBox,
				   TeamRoundInfoLabel, TeamRoundComboBox, MultiTeamMembershipCheckBox);
		if (Tournament.HasTeamTournament
		&&  TeamScoringComboBox.SelectedItem is null)
			TeamScoringComboBox.SelectedIndex = 0;
		TournamentInfoForm.SetTeamsTabVisibility();
	}

	private void TeamRoundComboBox_SelectedIndexChanged(object? sender = null,
														EventArgs? e = null)
		=> Tournament.TeamRound = (TeamRoundComboBox.SelectedIndex + 1)
								* (TeamScoringComboBox.SelectedIndex is 0 ? 1 : -1);

	private void TeamScoringComboBox_SelectedIndexChanged(object sender,
														  EventArgs e)
	{
		Tournament.TeamsPlayMultipleRounds = TeamScoringComboBox.SelectedIndex is 1;
		TeamRoundInfoLabel.Show();
		TeamRoundComboBox.Show();
		TeamRoundInfoLabel.Text = TeamScoringComboBox.SelectedIndex is 0
									  ? "Round Number to Use for Team Scoring:"
									  : "Highest-Scoring Games to Score per Player:";
		if (TeamRoundComboBox.SelectedItem is not null)
			TeamRoundComboBox_SelectedIndexChanged();
	}

	private void ScoringSystemComboBox_SelectedIndexChanged(object sender,
															EventArgs e)
	{
		if (SkippingHandlers)
			return;
		var scoringSystem = ScoringSystemComboBox.GetSelected<ScoringSystem>();
		var finishedGames = Tournament.FinishedGames;
		if (Tournament.Id > 0
		&&  RoundControl.CheckScoringSystemChange(Tournament.ScoringSystem, scoringSystem, finishedGames) is not DialogResult.Yes)
		{
			SkipHandlers(() => ScoringSystemComboBox.SetSelectedItem(Tournament.ScoringSystem));
			return;
		}
		Tournament.ScoringSystem = scoringSystem;
		if (Tournament.Id > 0)
			UpdateOne(Tournament);
		SetScoreConflictDetails();
		if (finishedGames.Length is 0)
			return;
		finishedGames.ForEach(static game => game.Status = Underway);
		UpdateMany(finishedGames);
	}

	private void DropCheckBox_CheckedChanged(object? sender = null,
											 EventArgs? e = null)
	{
		if (SkippingHandlers)
			return;
		var drop = DropCheckBox.Checked;
		SetVisible(drop, RoundsToDropComboBox, RoundsToDropLabel, DropWhenComboBox);
		DropCheckBox.Text = drop
								? "Drop the lowest"
								: "Drop scores?";
		if (ScaleCheckBox.Checked)
			SetScaleCheckBoxText(drop);
		if (!drop)
		{
			Tournament.RoundsToDrop = 0;
			FillRoundsToScaleComboBox(true);
		}
		else if (RoundsToDropComboBox.SelectedItem is null)
			RoundsToDropComboBox.SelectedIndex = 0;
	}

	private void SetScaleCheckBoxText(bool drop)
		=> ScaleCheckBox.Text = $"Scale {(drop ? "next" : "the")} lowest";

	private void RoundsToDropComboBox_SelectedIndexChanged(object sender,
														   EventArgs e)
	{
		var selected =
			Tournament.RoundsToDrop =
				RoundsToDropComboBox.SelectedIndex + 1;
		RoundsToDropLabel.Text = "score".Pluralize(selected);
		FillRoundsToScaleComboBox(true);
	}

	private void FillRoundsToScaleComboBox(bool restoreSelected)
	{
		var selectedIndex = restoreSelected
								? RoundsToScaleComboBox.SelectedIndex
								: 0;
		RoundsToScaleComboBox.FillRange(1, Tournament.TotalRounds
										 - Tournament.RoundsToDrop
										 - 1);
		if (selectedIndex > -1 && selectedIndex < RoundsToScaleComboBox.Items.Count)
			RoundsToScaleComboBox.SelectedIndex = selectedIndex;
	}

	private void ScaleCheckBox_CheckedChanged(object? sender = null,
											  EventArgs? e = null)
	{
		if (SkippingHandlers)
			return;
		var scale = ScaleCheckBox.Checked;
		SetVisible(scale, RoundsToScaleComboBox, RoundsToScaleLabel, ScaleFactorTextBox, ScalePercentLabel);
		if (scale)
		{
			SetScaleCheckBoxText(DropCheckBox.Checked);
			FillRoundsToScaleComboBox(false);
			return;
		}
		ScaleCheckBox.Text = "Scale scores?";
		Tournament.RoundsToScale = 0;
	}

	private void RoundsToScaleComboBox_SelectedIndexChanged(object sender,
															EventArgs e)
	{
		var selected =
			Tournament.RoundsToScale =
				RoundsToScaleComboBox.SelectedIndex + 1;
		RoundsToScaleLabel.Text = $"{"round score".Pluralize(selected)} by";
	}

	private void DropWhenComboBox_SelectedIndexChanged(object sender,
													   EventArgs e)
		=> Tournament.DropBeforeFinalRound = DropWhenComboBox.SelectedIndex is 1;

	private void CopyButton_Click(object sender,
								  EventArgs e)
	{
		Tournament.Id = 0;
		NameTextBox.Text = $"COPY OF {Tournament}";
		CopyButton.Hide();
	}

	private void OkButton_Click(object sender,
								EventArgs e)
	{
		var unplayedScore = 0;
		var playerConflict = 0;
		var powerConflict = 0;
		var teamConflict = 0;
		var scoreConflict = 0;
		var scaleFactor = 0;
		string? error = null;
		Tournament.Name = NameTextBox.Text
									 .Trim();
		var existingTournament = ReadByName(Tournament);
		if (existingTournament is not null && !existingTournament.Is(Tournament))
			error = "Another tournament with that name already exists.";
		else if (Tournament.Name.Length is 0)
			error = "Tournament must be named.";
		else if (Tournament.ScoringSystemId is 0)
			error = "The tournament scoring system must be specified.";
		else if (!TryGetInteger(UnplayedScoreTextBox, ref unplayedScore))
			error = "Unplayed round score must be a number.";
		else if (!TryGetInteger(ScaleFactorTextBox, ref scaleFactor)
			  || ScaleCheckBox.Checked && scaleFactor is 0)
			error = "Scaling percentage cannot be zero.";
		else if (!TryGetInteger(PlayerConflictTextBox, ref playerConflict))
			error = "Player conflict value must be a number.";
		else if (!TryGetInteger(PowerConflictTextBox, ref powerConflict))
			error = "Power conflict value must be a number.";
		else if (!TryGetInteger(TeamMemberConflictTextBox, ref teamConflict))
			error = "Team member conflict value must be a number.";
		else if (ScoreConflictCheckBox.Checked
			 && (!TryGetInteger(ScoreConflictTextBox, ref scoreConflict, true) || scoreConflict <= 0))
			error = "Score conflict value must be a positive number.";
		if (error is null)
		{
			Tournament.Description = DescriptionTextBox.Text
													   .Trim();
			Tournament.UnplayedScore = unplayedScore;
			Tournament.PlayerConflict = playerConflict;
			Tournament.PowerConflict = powerConflict;
			Tournament.TeamConflict = teamConflict;
			Tournament.ScoreConflict = scoreConflict;
			Tournament.ScalePercentage = scaleFactor;
			Tournament.PlayerCanJoinManyTeams = MultiTeamMembershipCheckBox.Checked;
			Tournament.ProgressiveScoreConflict = ProgressiveScoreConflictCheckBox.Checked;
			Tournament.Date = DateTimePicker.Value;
			Tournament.RoundsToDrop = DropCheckBox.Checked
										  ? RoundsToDropComboBox.SelectedIndex + 1
										  : 0;
			Tournament.RoundsToScale = ScaleCheckBox.Checked
										   ? RoundsToScaleComboBox.SelectedIndex + 1
										   : 0;
			if (Tournament.Id is 0)
				CreateOne(Tournament);
			else
				UpdateOne(Tournament);
			TournamentInfoForm.CloseForm();
			return;
		}
		MessageBox.Show(error,
						"Invalid Tournament Details",
						OK,
						Error);

		static bool TryGetInteger(Control control,
								  ref int number,
								  bool required = false)
		{
			var text = control.Text
							  .Trim();
			return text.Length is 0 && !required
				|| int.TryParse(text, out number);
		}
	}

	private void ScoreConflictCheckBox_CheckedChanged(object? sender = null,
													  EventArgs? e = null)
	{
		var useScoreConflicts = ScoreConflictCheckBox.Checked;
		SetVisible(useScoreConflicts, ScoreConflictTextBox, ScoreConflictLabel, ProgressiveScoreConflictCheckBox);
		ScoreConflictCheckBox.Text = useScoreConflicts
										 ? "1 conflict pt. each"
										 : "Score Conflict?";
	}

	private void SetScoreConflictDetails()
	{
		//	TODO: The restriction below (to allow seed-by-score only in fixed-point-per-game systems) is commented out.  Okay?
		//	ScoreConflictCheckBox.Visible = Tournament.ScoringSystem.PointsPerGame is not null;
		ScoreConflictCheckBox.Checked = Tournament.ScoreConflict is not 0;
		ProgressiveScoreConflictCheckBox.Checked = Tournament.ProgressiveScoreConflict;
		ScoreConflictCheckBox_CheckedChanged(); //	TODO: does the line above do this?
	}
}
