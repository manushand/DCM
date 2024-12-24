namespace PC.Forms;

using static ScoringSystem.DrawRules;

internal sealed partial class ScoringSystemInfoForm : Form
{
	//	TODO: Don't the French end in 1905 or 1906?
	private const int EarliestFinalGameYear = 1907;
	private ScoringSystem ScoringSystem { get; set; }

	internal ScoringSystemInfoForm(ScoringSystem scoringSystem)
	{
		InitializeComponent();
		GameControl.FormEnableCallback = SetButtonUse;
		ScoringSystem =
			GameControl.ScoringSystem =
				scoringSystem;
	}

	private void ScoringSystemInfoForm_Load(object sender,
											EventArgs e)
	{
		while (FormulaTabs.TabCount > 1)
			FormulaTabs.TabPages
					   .RemoveAt(1);
		FinalGameYearComboBox.FillRange(EarliestFinalGameYear, LatestFinalGameYear);
		FinalGameYearComboBox.Items
							 .Insert(0, "NONE");
		if (ScoringSystem.Id is 0)
		{
			FinalScoreFormulaTextBox.Text = "// (REQUIRED)";
			CopyButton.Hide();
			OtherScoringCheckBox.Checked = false;
			SignificantDigitsComboBox.SelectedIndex =
				FormulaTypeComboBox.SelectedIndex =
					FinalGameYearComboBox.SelectedIndex =
						0;
			//	Calhamer standard defaults
			AllowDrawsCheckBox.Checked =
				DiasCheckBox.Checked =
					true;
		}
		else
		{
			NameTextBox.Text = ScoringSystem.Name;
			WinLossCheckBox.Checked = ScoringSystem.UsesGameResult;
			CenterCountCheckBox.Checked = ScoringSystem.UsesCenterCount;
			YearsPlayedCheckBox.Checked = ScoringSystem.UsesYearsPlayed;
			AllowDrawsCheckBox.Checked = ScoringSystem.DrawsAllowed;
			DiasCheckBox.Checked = ScoringSystem.DrawsIncludeAllSurvivors;
			FinalGameYearComboBox.SelectedIndex = ScoringSystem.FinalGameYear - EarliestFinalGameYear + 1 ?? 0;
			FormulaTypeComboBox.SelectedIndex = ScoringSystem.UsesCompiledFormulas.AsInteger();
			ProvisionalScoreFormulaTextBox.Text = ScoringSystem.ProvisionalScoreFormula;
			FinalScoreFormulaTextBox.Text = ScoringSystem.FinalScoreFormula;
			PlayerAnteFormulaTextBox.Text = ScoringSystem.PlayerAnteFormula;
			SignificantDigitsComboBox.SelectedIndex = ScoringSystem.SignificantDigits;
			TotalPointsFixedCheckBox.Checked = ScoringSystem.PointsPerGame is not null;
			PointsPerGameTextBox.Text = ScoringSystem.PointsPerGame?.ToString();
			OtherScoringCheckBox.Checked = ScoringSystem.OtherScoreAlias.Length is not 0;
			OtherAliasTextBox.Text = ScoringSystem.OtherScoreAlias;
			UsesProvisionalScoreCheckBox.Checked = ScoringSystem.UsesProvisionalScore;
			UsesPlayerAnteCheckBox.Checked = ScoringSystem.UsesPlayerAnte;
		}
		WinLossCheckBox_CheckedChanged();
		CenterCountCheckBox_CheckedChanged();
		YearsPlayedCheckBox_CheckedChanged();
		FinalGameYearComboBox_SelectedIndexChanged();
		TotalPointsFixedCheckBox_CheckedChanged();
		OtherScoringCheckBox_CheckedChanged();
		GameControl.LoadGame(ScoringSystem);
	}

	private void WinLossCheckBox_CheckedChanged(object? sender = null,
												EventArgs? e = null)
	{
		ScoringSystem.UsesGameResult =
			AllowDrawsCheckBox.Visible =
				WinLossCheckBox.Checked;
		GameControl.SetResultComboBoxUsability(WinLossCheckBox.Checked);
		DiasCheckBox.Visible = WinLossCheckBox.Checked
							&& CenterCountCheckBox.Checked
							&& AllowDrawsCheckBox.Checked;
		SetTestButtonUsability();
		GameControl.SetConcessionCheckBoxUsability();
	}

	private void CenterCountCheckBox_CheckedChanged(object? sender = null,
													EventArgs? e = null)
	{
		ScoringSystem.UsesCenterCount = CenterCountCheckBox.Checked;
		GameControl.SetCentersComboBoxUsability(CenterCountCheckBox.Checked);
		DiasCheckBox.Visible = WinLossCheckBox.Checked
							&& CenterCountCheckBox.Checked
							&& AllowDrawsCheckBox.Checked;
		SetTestButtonUsability();
		GameControl.SetConcessionCheckBoxUsability();
	}

	private void YearsPlayedCheckBox_CheckedChanged(object? sender = null,
													EventArgs? e = null)
	{
		ScoringSystem.UsesYearsPlayed =
			FinalGameYearComboBox.Visible =
				FinalGameYearLabel.Visible =
					YearsPlayedCheckBox.Checked;
		GameControl.SetYearsComboBoxUsability(YearsPlayedCheckBox.Checked);
		SetTestButtonUsability();
	}

	private void FinalGameYearComboBox_SelectedIndexChanged(object? sender = null,
															EventArgs? e = null)
	{
		ScoringSystem.FinalGameYear = FinalGameYearComboBox.SelectedIndex > 0
										  ? EarliestFinalGameYear + FinalGameYearComboBox.SelectedIndex - 1
										  : null;
		GameControl.FillYearComboBoxes();
	}

	[GeneratedRegex("^[A-Z_][\\w\\s]*$", RegexOptions.IgnoreCase)]
	private static partial Regex AliasRegex();
	private static readonly Regex AliasFormat = AliasRegex();

	private bool ValidateSystem(out string? error)
	{
		error = null;
		LoadSystemFromForm();
		if (!WinLossCheckBox.Checked && !CenterCountCheckBox.Checked && !YearsPlayedCheckBox.Checked)
			error = "At least one scoring method must be used.";
		else if (ScoringSystem.FinalScoreFormulaMissing)
			error = "A final score formula is required.";
		else if (UsesPlayerAnteCheckBox.Checked && !ScoringSystem.UsesPlayerAnte)
			error = "A player ante formula is required.";
		else if (UsesProvisionalScoreCheckBox.Checked && !ScoringSystem.UsesProvisionalScore)
			error = "A provisional score formula is required.";
		else if (TotalPointsFixedCheckBox.Checked && ScoringSystem.PointsPerGame is null)
			error = "Points per game must be supplied as an integer.";
		else if (OtherScoringCheckBox.Checked
			 && !AliasFormat.IsMatch(OtherAliasTextBox.Text))
			error = "Formula alias for Other Score must be a legal alias (after spaces are removed).";
		else if (ScoringSystem.TestGamePlayers is not null)
			return GameControl.FinalGameDataValidation(out error);
		return error is null;

		void LoadSystemFromForm()
		{
			ScoringSystem.Name = NameTextBox.Text
											.Trim();
			ScoringSystem.PlayerAnteFormula = UsesPlayerAnteCheckBox.Checked
												  ? PlayerAnteFormulaTextBox.Text
												  : Empty;
			ScoringSystem.ProvisionalScoreFormula = UsesProvisionalScoreCheckBox.Checked
														? ProvisionalScoreFormulaTextBox.Text
														: Empty;
			ScoringSystem.FinalScoreFormula = FinalScoreFormulaTextBox.Text;
			ScoringSystem.SetCompiledFormulae(FormulaTypeComboBox.SelectedIndex is 1);
			ScoringSystem.DrawPermissions = AllowDrawsCheckBox.Checked
												? DiasCheckBox.Checked
													  ? DIAS
													  : All
												: None;
			ScoringSystem.PointsPerGame = TotalPointsFixedCheckBox.Checked
											  ? int.TryParse(PointsPerGameTextBox.Text, out var pts)
													? pts
													: null
											  : null;
			ScoringSystem.OtherScoreAlias = OtherScoringCheckBox.Checked
												? OtherAliasTextBox.Text
												: Empty;
			ScoringSystem.TestGamePlayers = GameControl.GetPlayerData();
		}
	}

	private void OkButton_Click(object sender,
								EventArgs e)
	{
		if (ValidateSystem(out var error))
		{
			var existingSystem = ReadByName(ScoringSystem);
			if (existingSystem is not null && !existingSystem.Is(ScoringSystem))
				error = "A scoring system with this same name exists.";
			else if (ScoringSystem.Name.Length is 0)
				error = "Scoring system must be named.";
			else if (ScoringSystem.TestGamePlayers is not null && !RunTest(out _))
				error = "Scoring system fails on test game data.";
		}
		if (error is null)
		{
			if (ScoringSystem.Id is 0)
				CreateOne(ScoringSystem);
			else
				UpdateOne(ScoringSystem);
			DialogResult = DialogResult.OK;
			//	TODO: check for errors
			ScoringSystem.Games.ForSome(static game => game.Scored, static game => game.Scored = false);
			Close();
		}
		else
			MessageBox.Show(error,
							"Invalid Scoring System Details",
							OK,
							Error);
	}

	private void AllowDrawsCheckBox_CheckedChanged(object sender,
												   EventArgs e)
	{
		DiasCheckBox.Visible = AllowDrawsCheckBox.Checked;
		DiasCheckBox.Checked = true;
		var numWinners = GameControl.NumberOfWinners;
		ScoringSystem.DrawPermissions = AllowDrawsCheckBox.Checked
											? DIAS
											: None;
		GameControl.SetWinType(AllowDrawsCheckBox.Checked, numWinners);
	}

	private void DiasCheckBox_CheckedChanged(object sender,
											 EventArgs e)
	{
		ScoringSystem.DrawPermissions = DiasCheckBox.Checked
											? DIAS
											: All;
		//	If NO-DIAS, all bets are off.
		if (DiasCheckBox.Checked)
			GameControl.SetDiasOptions();
	}

	private void SignificantDigitsComboBox_SelectedIndexChanged(object sender,
																EventArgs e)
		=> ScoringSystem.SignificantDigits = SignificantDigitsComboBox.SelectedIndex;

	private void CopyButton_Click(object sender,
								  EventArgs e)
	{
		NameTextBox.Text = $"COPY OF {ScoringSystem}";
		ScoringSystem = new ()
						{
							Id = 0,
							Name = NameTextBox.Text,
							DrawPermissions = ScoringSystem.DrawPermissions,
							FinalGameYear = ScoringSystem.FinalGameYear,
							FinalScoreFormula = ScoringSystem.FinalScoreFormula,
							OtherScoreAlias = ScoringSystem.OtherScoreAlias,
							PlayerAnteFormula = ScoringSystem.PlayerAnteFormula,
							PointsPerGame = ScoringSystem.PointsPerGame,
							ProvisionalScoreFormula = ScoringSystem.ProvisionalScoreFormula,
							SignificantDigits = ScoringSystem.SignificantDigits,
							UsesCenterCount = ScoringSystem.UsesCenterCount,
							UsesGameResult = ScoringSystem.UsesGameResult,
							UsesYearsPlayed = ScoringSystem.UsesYearsPlayed,
							TestGamePlayers = ScoringSystem.TestGamePlayers?.ToList()
						};
		CopyButton.Hide();
	}

	private void ScoringSystemDetailsForm_FormClosing(object sender,
													  FormClosingEventArgs e)
	{
		if (DialogResult is DialogResult.Cancel && ScoringSystem.Id > 0)
			//	TODO: maybe ask for confirmation if there are unsaved changes
			//	Re-load system from disk because mucking with ScoringSystem changed it in the Cache
			ReadOne(ScoringSystem, false);
	}

	private void SetButtonUse(bool enabled)
		=> SetEnabled(enabled, OkButton, CancelFormButton, NewTestButton, RunTestButton, CopyButton);

	private void SetTestButtonUsability()
		=> SetEnabled(ScoringSystem.UsesGameResult
					  || ScoringSystem.UsesCenterCount
					  || ScoringSystem.UsesYearsPlayed
					  || ScoringSystem.UsesOtherScore, NewTestButton, RunTestButton);

	private void NewTestButton_Click(object sender,
									 EventArgs e)
		=> GameControl.CreateRandomGame();

	private void RunTestButton_Click(object sender,
									 EventArgs e)
	{
		var success = ValidateSystem(out var text);
		if (success)
			if (ScoringSystem.TestGamePlayers is null)
				text = "No test game data provided.";
			else
			{
				success = RunTest(out var results);
				text = Join(NewLine, results);
			}
		MessageBox.Show(text,
						success
							? "Test Game Scoring Report"
							: "Scoring Failed",
						OK,
						success
							? Information
							: Error);
	}

	private bool RunTest(out List<string?> results)
		=> ScoringSystem.ScoreWithResults(ScoringSystem.TestGamePlayers.OrThrow(),
										  out results);

	private void TotalPointsFixedCheckBox_CheckedChanged(object? sender = null,
														 EventArgs? e = null)
		=> SetVisible(TotalPointsFixedCheckBox.Checked, PointsPerGameLabel, PointsPerGameTextBox);

	private void OtherScoringCheckBox_CheckedChanged(object? sender = null,
													 EventArgs? e = null)
	{
		SetVisible(OtherScoringCheckBox.Checked, OtherAliasLabel, OtherAliasTextBox);
		OtherAliasTextBox_TextChanged();
		GameControl.SetOtherTextBoxUsability(OtherScoringCheckBox.Checked);
	}

	private void OtherAliasTextBox_TextChanged(object? sender = null,
											   EventArgs? e = null)
		=> GameControl.SetOtherScoreLabel(OtherAliasLabel.Visible
											  ? OtherAliasTextBox.Text
											  : Empty);

	private void UsesProvisionalScoreCheckBox_CheckedChanged(object sender,
															 EventArgs e)
		=> FormulaTabs.AddOrRemove(ProvisionalScoreTabPage, UsesProvisionalScoreCheckBox.Checked, 1);

	private void UsesPlayerAnteCheckBox_CheckedChanged(object sender,
													   EventArgs e)
	{
		FormulaTabs.AddOrRemove(PlayerAnteFormulaTabPage, UsesPlayerAnteCheckBox.Checked);
		if (PlayerAnteFormulaTextBox.TextLength is 0)
			PlayerAnteFormulaTextBox.Text = $"{nameof (ScoringSystem.PointsPerGame)} / 7";
	}
}
