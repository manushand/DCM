using static System.Math;
using static System.Drawing.StringAlignment;

namespace PC.Controls;

using static PC;
using static ScoringSystem.DrawRules;

internal sealed partial class GameControl : UserControl
{
	private const string WinText = "Win",
						 LossText = "Loss",
						 SoloText = "Solo",
						 DrawText = "Draw",
						 SurvText = "Survived",
						 ElimText = "Eliminated",
						 ConcText = "Concession";

	private static readonly Dictionary<Color, SolidBrush> Brushes = [];

	private static readonly StringFormat Centered = new () { LineAlignment = Center, Alignment = Center };

	[DesignerSerializationVisibility(Hidden)]
	internal ScoringSystem ScoringSystem
	{
		private get => field.NotNone;
		set
		{
			field = value;
			SetEnabled(value.UsesGameResult || TournamentScoringSystem?.UsesGameResult is true, [..ResultComboBoxes]);
			SetEnabled(value.UsesCenterCount || TournamentScoringSystem?.UsesCenterCount is true, [..CentersComboBoxes]);
			SetEnabled(value.UsesYearsPlayed || TournamentScoringSystem?.UsesYearsPlayed is true, [..YearsComboBoxes]);
			AllComboBoxes.ForSome(static box => !box.Enabled, static box => box.Deselect());
			SetOtherScoreLabel(value.OtherScoreAlias);
			OtherTextBoxes.ForEach(box =>
								   {
									   box.Enabled = value.UsesOtherScore || TournamentScoringSystem?.UsesOtherScore is true;
									   if (!box.Enabled)
										   box.Text = null;
								   });
		}
	} = ScoringSystem.None;

	/// <summary>
	///     Holds the main Scoring System for a Tournament (in a test game situation on the ScoringSystemInfoForm,
	///     this is left null) so that all necessary ComboBoxes are enabled even if the game overrides the scoring
	///     system with one that doesn't think it needs certain things (i.e., game result, center count, or year).
	/// </summary>
	[DesignerSerializationVisibility(Hidden)]
	internal ScoringSystem? TournamentScoringSystem { private get; set; }

	internal bool HasData => AllComboBoxes.Any(static box => box.SelectedItem is not null);

	internal bool AllFilledIn => GamePlayers?.All(static gamePlayer => gamePlayer.PlayComplete) is true
							  && FinalGameDataValidation(out _);

	[DesignerSerializationVisibility(Hidden)]
	internal int NumberOfWinners => ResultComboBoxes.Count(static comboBox => comboBox.SelectedIndex.As<Results>() is Win);

	[DesignerSerializationVisibility(Hidden)]
	internal Callback? FormEnableCallback { private get; set; }

	[DesignerSerializationVisibility(Hidden)]
	internal bool Active
	{
		private get;
		set
		{
			field =
				SoloConcededCheckBox.Enabled =
					value;
			AllComboBoxes.ForEach(box => box.Enabled = value);
		}
	}

	private List<GamePlayer>? GamePlayers { get; set; }

	private List<ComboBox> ResultComboBoxes { get; }

	private List<ComboBox> CentersComboBoxes { get; }

	private List<ComboBox> YearsComboBoxes { get; }

	private List<TextBox> OtherTextBoxes { get; }

	private List<ComboBox> AllComboBoxes { get; }

	internal GameControl()
	{
		InitializeComponent();
		CentersComboBoxes = CentersPanel.PowerControls<ComboBox>();
		ResultComboBoxes = ResultPanel.PowerControls<ComboBox>();
		YearsComboBoxes = YearsPanel.PowerControls<ComboBox>();
		//	IMPORTANT: Keep them loading into AllComboBoxes in THIS ORDER
		AllComboBoxes = [..CentersComboBoxes, ..ResultComboBoxes, ..YearsComboBoxes];
		OtherTextBoxes = OtherPanel.PowerControls<TextBox>();
	}

	private void GameControl_Load(object sender,
								  EventArgs e)
	{
		FillResultBoxes();
		FillCenterComboBoxes();
		//	Fill YearsComboBoxes this way, not with FillYearsComboBoxes(), because it needs a non-null ScoringSystem.
		YearsComboBoxes.ForEach(static comboBox => comboBox.FillRange(1901, LatestFinalGameYear));
		//	Coloring the Labels isn't necessary since this is done in the Form designer
		//	but is done here anyway to be sure they always match the PowerColor array.
		//	Note that this only colors labels whose .Tag is set, and it must be set to
		//	a PowerName.
		foreach (var label in Controls.OfType<Label>().Where(static label => label.Tag is not null))
		{
			var style = $"{label.Tag}".As<Powers>().CellStyle();
			(label.BackColor, label.ForeColor) = (style.BackColor, style.ForeColor);
		}
	}

	internal void ClearGame()
		=> LoadGame(gamePlayers: null);

	internal void LoadGame(ScoringSystem scoringSystem)
	{
		LoadGame(scoringSystem.TestGamePlayers);
		SkipHandlers(() => SoloConcededCheckBox.Enabled = true);
	}

	internal void LoadGame(List<GamePlayer>? gamePlayers)
	{
		//	If the list of game players is null (or empty), just clear the boxes and return.
		GamePlayers = gamePlayers;
		if (GamePlayers?.Count is null or 0)
		{
			SkipHandlers(() => AllComboBoxes.ForEach(static box => box.Deselect()));
			Active = false;
			return;
		}
		//	Load the game data with callbacks and event handlers turned off.
		SkippingCallbacks = true;
		SkipHandlers(() =>
					 {
						 CentersComboBoxes.ForEach(static comboBox => comboBox.FillRange(0, 34));
						 AllComboBoxes.ForEach(static box => box.Deselect());
						 var concessionPossible = ScoringSystem is { UsesGameResult: true, UsesCenterCount: true }
											   && GamePlayers.Max(static player => player.Centers) < 18;
						 SoloConcededCheckBox.Enabled = concessionPossible
													 && Active;
						 SoloConcededCheckBox.Checked = concessionPossible
													 && GamePlayers.Count(static player => player.Result is Win) is 1;
						 var held = GamePlayers.Select(static player => new
																		{
																			player.Result,
																			player.Centers,
																			player.Years,
																			player.Other
																		})
											   .ToArray();
						 held.Apply((player, index) =>
									{
										ResultComboBoxes[index].SelectedIndex = player.Result
																					  .AsInteger();
										CentersComboBoxes[index].SelectedIndex = player.Centers ?? -1;
										YearsComboBoxes[index].SelectedIndex = player.Years - 1 ?? -1;
										OtherTextBoxes[index].Text = ScoringSystem.UsesOtherScore
																		 ? ScoringSystem.FormattedScore(player.Other, true)
																		 : null;
									});
						 SetOtherScoreLabel(ScoringSystem.OtherScoreAlias);
					 });
		if (ScoringSystem.UsesGameResult)
			if (ScoringSystem.UsesCenterCount)
				//	Touch each Centers box (with event handling back on) to give the Result box
				//	the correct flavor (Solo, Concession, Draw, Survived, or Eliminated).
				CentersComboBoxes.ForEach(box => CentersComboBox_SelectedIndexChanged(box));
			else
			{
				var numWinners = ResultComboBoxes.Count(static box => box.SelectedIndex.As<Results>() is Win);
				//	Must use .Items[box.SelectedIndex] = x; because .SelectedItem = x; does not work
				ResultComboBoxes.ForEach(box => box.Items[box.SelectedIndex] = box.SelectedIndex.As<Results>() is Loss
																				   ? LossText
																				   : numWinners is 1
																					   ? SoloText
																					   : ScoringSystem.DrawPermissions is None
																						   ? WinText
																						   : DrawText);
			}
		//	Turn callbacks on again, and force a callback now that all data is loaded.
		SkippingCallbacks = false;
		RunGameDataChangedCallback();
	}

	[GeneratedRegex(@"\s+")]
	private static partial Regex SpacesRegex();
	private static readonly Regex Spaces = SpacesRegex();

	internal void SetOtherScoreLabel(string otherScoreAlias)
		=> OtherScoreLabel.Text = otherScoreAlias.Length is 0
									  ? "──"
									  : Spaces.Replace(otherScoreAlias, NewLine).ToUpper();

	internal List<GamePlayer>? GetPlayerData()
		=> AllComboBoxes.All(static comboBox => comboBox.SelectedItem is null)
			   ? null
			   : [..Seven.Select(power => PlayerData(power.As<Powers>(),
													 ResultComboBoxes[power],
													 CentersComboBoxes[power],
													 YearsComboBoxes[power],
													 OtherTextBoxes[power]))];

	private static GamePlayer PlayerData(Powers power,
										 ListControl winLossComboBox,
										 ListControl centersComboBox,
										 ListControl yearsComboBox,
										 TextBoxBase otherTextBox)
		=> new ()
		   {
			   Power = power,
			   Result = winLossComboBox.Enabled
							? winLossComboBox.SelectedIndex
											 .As<Results>()
							: Unknown,
			   Centers = centersComboBox.Enabled
							 ? centersComboBox.SelectedIndex is -1
								   ? null
								   : centersComboBox.Text
													.AsInteger()
							 : null,
			   Years = yearsComboBox.Enabled
						   ? yearsComboBox.SelectedIndex is -1
								 ? null
								 : yearsComboBox.Text
												.AsInteger() - 1900
						   : null,
			   Other = otherTextBox is { Enabled: true, TextLength: not 0 }
						   ? otherTextBox.Text
										 .AsDouble()
						   : 0
		   };

	private void RunGameDataChangedCallback()
		=> GameDataChangedCallback?.Invoke(AllFilledIn);

	private void ResultComboBox_SelectedIndexChanged(object sender,
													 EventArgs? e = null)
	{
		var resultComboBox = (ComboBox)sender;
		resultComboBox.UpdateShadowLabel();
		if (GamePlayers is not null)
			GamePlayers[PowerOffsetFor(resultComboBox)].Result = resultComboBox.SelectedIndex
																			   .As<Results>();
		if (SkippingHandlers)
			return;
		++UpdateDepth;
		//	If win/loss is win, make sure year is latest
		if (resultComboBox.SelectedIndex.As<Results>() is Win && ScoringSystem.UsesYearsPlayed)
			YearsBoxFor(resultComboBox).SelectedIndex = YearsComboBoxes.Max(static comboBox => comboBox.SelectedIndex);
		//	Set the Win text to either Solo or Draw for everyone.
		var winners = NumberOfWinners;
		SkipHandlers(() =>
					 {
						 ResultComboBoxes.ForEach(comboBox =>
												  {
													  //	If draws are allowed, set multiple winners' text to Draw, else Solo
													  if (ScoringSystem.DrawsAllowed)
														  comboBox.Items[1] = winners is 0
																		   || winners is 1 && comboBox.SelectedIndex.As<Results>() is Win
																				  ? SoloText
																				  : DrawText;
													  //	If there's one winner and this isn't it, he's a loser.
													  if (winners is 1 && comboBox.SelectedIndex < 1)
														  comboBox.SelectedIndex = 0;
												  });
						 if (!ScoringSystem.UsesCenterCount)
							 return;
						 var centersComboBox = CentersBoxFor(resultComboBox);
						 switch (resultComboBox.SelectedIndex)
						 {
						 case 0 when ScoringSystem.DrawsIncludeAllSurvivors && !SoloConcededCheckBox.Checked:
							 //	If win/loss is loss and this is not a conceded solo, set centers to 0
							 resultComboBox.Items[centersComboBox.SelectedIndex = 0] = ElimText;
							 var centersBox = CentersBoxFor(resultComboBox);
							 if (centersBox.SelectedIndex > 17)
								 // TODO: that HAD BEEN ((string)resultComboBox.Items[1] is SoloText)
								 // TODO: but this way seems better (but does it work...need to check)
								 foreach (var resultBox in ResultComboBoxes)
								 {
									 //	Those with an unknown type-of-LOSS get a completely unknown result...
									 centersBox = CentersBoxFor(resultBox);
									 if (centersBox.SelectedItem is null)
										 resultBox.Deselect();
									 //	...and survivors become drawers
									 else if (centersBox.SelectedIndex > 0)
										 resultBox.Items[resultBox.SelectedIndex = 1] = DrawText;
								 }
							 break;
						 case 0:
							 resultComboBox.Items[0] = SurvText;
							 //	If this claims to be a conceded solo, and only
							 //	one power has centers, set that power to Win.
							 if (SoloConcededCheckBox.Checked
							 && CentersComboBoxes.Count(box => box != centersComboBox && box.SelectedIndex > 0) is 1)
								 ResultBoxFor(CentersComboBoxes.Single(box => box != centersComboBox && box.SelectedIndex > 0)).SelectedIndex = 1;
							 break;
						 case 1 when centersComboBox.SelectedIndex is 0:
							 //	If win/loss is win and centers is 0, clear the centers (can't be true!)
							 centersComboBox.Deselect();
							 goto case 1;
						 case 1:
							 //	If some other power holds 18+, empty his center count (can't be true!)
							 CentersComboBoxes.ForSome(comboBox => comboBox != centersComboBox && comboBox.SelectedIndex > 17,
													   static comboBox => comboBox.Deselect());
							 if (SoloConcededCheckBox.Checked)
								 ResultComboBoxes.ForEach(comboBox => comboBox.SelectedIndex = (comboBox == resultComboBox).AsInteger());
							 break;
						 case -1:
							 break;
						 default:
							 throw new (); //	TODO
						 }

						 //	Change win/loss dropdown text to "SOLO" if no victor or for a single victor, otherwise to "DRAW"
						 var numWinners = NumberOfWinners;
						 foreach (var resultBox in ResultComboBoxes)
						 {
							 var centersBox = CentersBoxFor(resultBox);
							 resultBox.Items[1] = ScoringSystem.DrawPermissions is not None
												  || numWinners is 0
												  || numWinners is 1 && resultBox.SelectedIndex is 1
												  || SoloConcededCheckBox.Checked
													  ? centersBox.SelectedIndex < 18
															? SoloConcededCheckBox.Checked
																  ? ConcText
																  : DrawText
															: SoloText
													  : DrawText;
							 resultBox.Items[0] = centersBox.SelectedIndex is 0
													  ? ElimText
													  : numWinners is 1
														  ? SurvText
														  : LossText;
						 }
					 });
		--UpdateDepth;
	}

	private void CentersComboBox_SelectedIndexChanged(object sender,
													  EventArgs? e = null)
	{
		var centersComboBox = (ComboBox)sender;
		centersComboBox.UpdateShadowLabel();
		if (GamePlayers is not null)
			GamePlayers[PowerOffsetFor(centersComboBox)].Centers = centersComboBox.SelectedItem is null
																	   ? null
																	   : centersComboBox.SelectedIndex;
		if (SkippingHandlers || centersComboBox.SelectedItem is null)
			return;
		++UpdateDepth;
		var maxCenters = CentersComboBoxes.Max(static comboBox => comboBox.SelectedIndex);
		SkipHandlers(FillCenterComboBoxes);
		CentersComboBoxes.ForSome(static box => box.Items.Count is 1 && box.SelectedItem is null, static centerBox => centerBox.SelectedIndex = 0);
		if (ScoringSystem.UsesYearsPlayed && centersComboBox.SelectedIndex > 0)
			//	Centers are more than 0, so set year to final year
			YearsBoxFor(centersComboBox).SelectedIndex = YearsComboBoxes.Max(static comboBox => comboBox.SelectedIndex);
		if (ScoringSystem.UsesGameResult)
		{
			var resultComboBox = ResultBoxFor(centersComboBox);
			SkipHandlers(() =>
						 {
							 switch (centersComboBox.SelectedIndex)
							 {
							 case 0:
								 //	If centers going to 0, set win/loss to LOSS (Eliminated)
								 resultComboBox.SelectedIndex = 0;
								 resultComboBox.Items[0] = ElimText;
								 if (SoloConcededCheckBox.Checked && CentersComboBoxes.Count(static box => box.SelectedIndex > 0) is 1)
									 ResultComboBoxes.ForEach(resultBox => resultBox.SelectedIndex = (CentersBoxFor(resultBox).SelectedIndex > 0).AsInteger());
								 else if (NumberOfWinners is 1)
									 ResultComboBoxes.Single(static box => box.SelectedIndex is 1).Items[1] = maxCenters > 17
																												  ? SoloText
																												  : ConcText;
								 break;
							 case > 17:
								 //	If centers are 18+, set this win/loss to WIN and all others to LOSS
								 //	and reset any other power's centers if they had claimed 18+.
								 var ourCount = centersComboBox.SelectedIndex;
								 ResultComboBoxes.ForEach(comboBox => comboBox.SelectedIndex = (comboBox == resultComboBox).AsInteger());
								 CentersComboBoxes.ForSome(static box => box.SelectedIndex > 17, static comboBox => comboBox.Deselect());
								 centersComboBox.SelectedIndex = ourCount;
								 resultComboBox.Items[1] = SoloText;
								 //	Losers don't yet know what kind of loss they experienced.
								 foreach (var centerBox in CentersComboBoxes)
									 ResultBoxFor(centerBox).Items[0] = centerBox.SelectedItem is null
																			? LossText
																			: centerBox.SelectedIndex is 0
																				? ElimText
																				: SurvText;
								 break;
							 default:
								 if (NumberOfWinners is 1 && resultComboBox.SelectedIndex is 1)
									 resultComboBox.Items[1] = ConcText;
								 else if (ScoringSystem.DrawsIncludeAllSurvivors)
									 //	If DIAS and no other power claims a solo, set box to WIN
									 if (CentersComboBoxes.All(static comboBox => comboBox.SelectedIndex < 18)
									 && (NumberOfWinners is not 1 || !SoloConcededCheckBox.Checked))
									 {
										 //	If we HAD said SOLO, everyone with centers is back in a draw
										 //	TODO: This seems like a bad way to check this, ...?
										 if (resultComboBox.SelectedItem is not null // this null check is necessary to prevent ker-blam
										 &&  resultComboBox.GetSelected<string>() is SoloText)
											 ResultComboBoxes.ForSome(box => CentersBoxFor(box).SelectedIndex > 0,
																	  static resultBox => resultBox.SelectedIndex = 1);
										 resultComboBox.SelectedIndex = 1;
										 //	TODO: Is the line below needed? Isn't this already the case based on the box checking?
										 ResultComboBoxes.ForEach(box => box.Items[1] = SoloConcededCheckBox.Checked
																							? ConcText
																							: DrawText);
									 }
									 else
										 resultComboBox.Items[0] = SurvText;
								 break;
							 }
						 });
			SoloConcededCheckBox.Enabled = Active && maxCenters < 18;
			if (maxCenters > 17)
				SoloConcededCheckBox.Checked = false;
		}
		--UpdateDepth;
	}

	private void YearsComboBox_SelectedIndexChanged(object sender,
													EventArgs e)
	{
		var yearsComboBox = (ComboBox)sender;
		yearsComboBox.UpdateShadowLabel();
		if (GamePlayers is not null)
			GamePlayers[PowerOffsetFor(yearsComboBox)].Years = yearsComboBox.SelectedIndex is -1
																   ? null
																   : yearsComboBox.SelectedIndex + 1;
		++UpdateDepth;
		var resultComboBox = ResultBoxFor(yearsComboBox);
		var centersComboBox = CentersBoxFor(yearsComboBox);
		if ((ScoringSystem.UsesGameResult || ScoringSystem.UsesCenterCount)
		&& (resultComboBox.SelectedItem is not null || centersComboBox.SelectedItem is not null))
		{
			var won = ScoringSystem.UsesGameResult
						  ? resultComboBox.SelectedIndex is 1
						  : centersComboBox.SelectedIndex > 0;
			var ourYear = yearsComboBox.SelectedIndex;
			ResultComboBoxes.ForEach(resultBox =>
									 {
										 var yearBox = YearsBoxFor(resultBox);
										 var centerBox = CentersBoxFor(resultBox);
										 //	If setting the year of a winner, make sure losers are no later
										 //	than this year, and that all winners/survivors share this same year.
										 if (won
										 && (yearBox.SelectedIndex > ourYear || resultBox.SelectedIndex is 1 || centerBox.SelectedIndex > 0)
											 //	...or if setting the year of a loser, make sure all winners' years are this year or later
										 ||  !won && resultBox.SelectedIndex is 1 && yearBox.SelectedIndex < ourYear)
											 yearBox.SelectedIndex = ourYear;
									 });
		}
		--UpdateDepth;
	}

	private void SoloConcededCheckBox_CheckedChanged(object sender,
													 EventArgs e)
	{
		if (SkippingHandlers)
			return;
		//	If a solo is mandated and more than one claim to win, un-claim all of them
		var numWinners = NumberOfWinners;
		SkipHandlers(() =>
					 {
						 if (numWinners > 1)
							 ResultComboBoxes.ForEach(static comboBox => comboBox.SelectedIndex *= -1);
						 FillCenterComboBoxes();
					 });
		ResultComboBoxes.ForEach(comboBox => comboBox.Items[1] = SoloConcededCheckBox.Checked
																	 ? ConcText
																	 : numWinners is 1 && comboBox.SelectedIndex is 1
																		 ? SoloText
																		 : DrawText);
	}

	private void ComboBox_EnabledChanged(object sender,
										 EventArgs e)
		=> sender.ToggleEnabled();

	internal bool FinalGameDataValidation(out string? error)
	{
		error = AllComboBoxes.Any(static comboBox => comboBox is { Enabled: true, SelectedItem: null })
					? "Game data is incomplete."
					: null;
		if (!ScoringSystem.UsesCenterCount)
			return error is null;
		var totalCenters = CentersComboBoxes.Sum(static box => box.SelectedIndex);
		if (totalCenters is < 22 or > 34)
		{
			error = "Total number of owned supply centers must be between 22 and 34.";
			return false;
		}
		//	Validate center count to years played.
		if (ScoringSystem.UsesYearsPlayed)
			//	By the end of 1901, England, Italy, Russia, and Turkey must have at least
			//	one center minimum; France, Germany, or Austria may have been eliminated.
			if (YearsComboBoxes.Any(box => box.SelectedIndex is 0
										&& CentersBoxFor(box).SelectedIndex is 0
										&& box != AustriaYearsComboBox
										&& box != FranceYearsComboBox
										&& box != GermanyYearsComboBox))
				error = "Power impossibly eliminated in 1901.";
			//	Only one power (max) can be eliminated in 1901.
			else if (YearsComboBoxes.Count(box => box.SelectedIndex is 0
											   && CentersBoxFor(box).SelectedIndex is 0) > 1)
				error = "Impossibly too many powers eliminated in 1901.";
			//	Two powers (minimum) must survive 1902, and if it's only
			//	two then it has to be one of six specific power-pairs.
			else if (TooMany1902Eliminations())
				error = "Impossibly too many powers eliminated in 1902.";
			//	By the end of 1901, each power could have 3+3=6, except Russia, who could have 4+4=8.
			//	By the end of 1902, each power could have 6+6=12, except Russia, who could have 8+8=16.
			//	By the end of 1903, a non-Russian power can have 30 [(12+3)*2], and Russia can have 34.
			//	After 1903, all powers can own any number of centers.  Check that the center count of
			//	any power that only survived until 1903 or earlier is realistic.
			else if (YearsComboBoxes.Any(box => box.SelectedIndex < 3
											 && CentersBoxFor(box).SelectedIndex > (box == RussiaYearsComboBox
																						? 8
																						: 6)
																				 * (box.SelectedIndex is 2
																						? 5
																						: box.SelectedIndex + 1)))
				error = "Impossibly too many centers for a pre-1904 survival.";
		if (!ScoringSystem.UsesGameResult)
			return error is null;
		var soleWinner = NumberOfWinners is 1;
		if ((soleWinner && CentersComboBoxes.Max(static comboBox => comboBox.SelectedIndex) < 18) != SoloConcededCheckBox.Checked)
			error = soleWinner
						? "Solo must be marked conceded."
						: "Indicated concession not evident.";
		return error is null;

		bool TooMany1902Eliminations()
		{
			var eliminated = YearsComboBoxes.Where(box => box.SelectedIndex is 1
													   && CentersBoxFor(box).SelectedIndex is 0)
											.ToArray();
			//	If there are fewer than five eliminations (i.e., 3+ survivors), it's all good.
			//	If there are six or more eliminations (i.e., 22+ center solo?), it's all bad.
			if (eliminated.Length is not 5)
				return eliminated.Length > 5;
			//	Exactly five eliminations means two survivors.  They must be A(FGR) or R(FGI).
			var austriaSurvived = eliminated.Contains(AustriaYearsComboBox);
			var russiaSurvived = eliminated.Contains(RussiaYearsComboBox);
			return !austriaSurvived && !russiaSurvived                              //	AR is good
				|| (!austriaSurvived || !russiaSurvived)                            //	no A and no R are bad
				&& !eliminated.Contains(FranceYearsComboBox)                        //	(AR)F is good
				&& !eliminated.Contains(GermanyYearsComboBox)                       //	(AR)G is good
				&& (austriaSurvived || !eliminated.Contains(ItalyYearsComboBox));   //	and RI is good
		}
	}

	private int PowerOffsetFor(ComboBox comboBox)
		=> AllComboBoxes.IndexOf(comboBox) % 7;

	private ComboBox CentersBoxFor(ComboBox comboBox)
		=> AllComboBoxes[0 + PowerOffsetFor(comboBox)];

	private ComboBox ResultBoxFor(ComboBox comboBox)
		=> AllComboBoxes[7 + PowerOffsetFor(comboBox)];

	private ComboBox YearsBoxFor(ComboBox comboBox)
		=> AllComboBoxes[14 + PowerOffsetFor(comboBox)];

	private void FillResultBoxes()
		=> ResultComboBoxes.ForEach(static comboBox => comboBox.FillWith(LossText, WinText));

	private void FillCenterComboBoxes()
		=> CentersComboBoxes.ForEach(comboBox =>
									 {
										 var index = comboBox.SelectedIndex;
										 var available = Min(34 >> SoloConcededCheckBox.Checked.AsInteger(),
															 34 - CentersComboBoxes.Sum(static box => Max(0, box.SelectedIndex))
																+ Max(0, index));
										 //	The Max(0...) call below isn't just superfluous.
										 //	It speeds us and is needed for exception prevention
										 //	during generation of ScoringSystem test game data.
										 comboBox.FillRange(0, Max(0, available));
										 comboBox.SelectedIndex = index < comboBox.Items.Count
																	  ? index
																	  : -1;
									 });

	internal void FillYearComboBoxes()
		=> YearsComboBoxes.ForEach(comboBox =>
								   {
									   comboBox.FillRange(1901, ScoringSystem.FinalGameYear - 1 ?? LatestFinalGameYear);
									   var index = comboBox.SelectedIndex;
									   comboBox.SelectedIndex = index < comboBox.Items.Count
																	? index
																	: -1;
								   });

	private static void SetComboBoxUsability(List<ComboBox> boxes,
											 bool enabled)
		=> boxes.ForEach(comboBox =>
						 {
							 comboBox.Enabled = enabled;
							 comboBox.Deselect();
						 });

	internal void SetResultComboBoxUsability(bool enabled)
		=> SetComboBoxUsability(ResultComboBoxes, enabled);

	internal void SetCentersComboBoxUsability(bool enabled)
		=> SetComboBoxUsability(CentersComboBoxes, enabled);

	internal void SetYearsComboBoxUsability(bool enabled)
		=> SetComboBoxUsability(YearsComboBoxes, enabled);

	internal void SetOtherTextBoxUsability(bool enabled)
		=> OtherTextBoxes.ForEach(textBox =>
								  {
									  textBox.Enabled = enabled;
									  textBox.Clear();
								  });

	private void CenteredComboBox_DrawItem(object sender,
										   DrawItemEventArgs e)
	{
		e.DrawBackground();
		if (e.Index is -1)
			return;
		var box = (ComboBox)sender;
		e.Graphics
		 .DrawString($"{box.Items[e.Index]}",
					 box.Font,
					 e.State.HasFlag(DrawItemState.Selected)
						 ? SystemBrushes.HighlightText
						 : Brushes.GetOrSet(box.ForeColor, static foreColor => new (foreColor)),
					 e.Bounds,
					 Centered);
	}

	internal void SetConcessionCheckBoxUsability()
	{
		SoloConcededCheckBox.Visible = ScoringSystem is { UsesGameResult: true, UsesCenterCount: true };
		if (!SoloConcededCheckBox.Visible)
			SoloConcededCheckBox.Checked = false;
	}

	internal void CreateRandomGame()
	{
		//	This should be impossible, because the button should be disabled
		if (ScoringSystem is { UsesGameResult: false, UsesCenterCount: false, UsesYearsPlayed: false })
			throw new InvalidOperationException();  //	TODO
		//	Disable all the buttons while we create a random game.
		FormEnableCallback?.Invoke(false);
		//	Clear all the game combo boxes and refill their options.
		SkipHandlers(() =>
					 {
						 AllComboBoxes.ForEach(static comboBox => comboBox.Deselect());
						 FillResultBoxes();
						 FillCenterComboBoxes();
						 FillYearComboBoxes();
						 SoloConcededCheckBox.Checked = false;
					 });
		do
		{
			while (ScoringSystem.UsesGameResult && ResultComboBoxes.Any(static comboBox => comboBox.SelectedItem is null)
				|| ScoringSystem.UsesCenterCount && (CentersComboBoxes.Any(static comboBox => comboBox.SelectedItem is null)
												 ||  CentersComboBoxes.Sum(static box => box.SelectedIndex) is not 34))
			{
				var randomOrder = Seven;
				var lastInTurn = randomOrder.Last();
				foreach (var item in randomOrder)
				{
					const int maxCenters = 20;  //	still pretty unrealistic
					if (ScoringSystem.UsesCenterCount)
					{
						var centersBox = CentersComboBoxes[item];
						centersBox.SelectedIndex = item == lastInTurn
													   ? Min(maxCenters, centersBox.Items.Count - 1)
													   : Max(0, RandomNumber(Min(maxCenters, centersBox.Items.Count) + 2) - 2);
					}
					if (item == lastInTurn && CentersComboBoxes.Sum(static box => box.SelectedIndex) < 34)
						continue;
					if (ScoringSystem.UsesGameResult)
						//	Choose a WIN or LOSS even if the box is already set.  Otherwise, the first
						//	one to set to Solo will set all others to Loss and that's all we get
						//	But ensure we have at least one winner at all times.
						ResultComboBoxes[item].SelectedIndex = ResultComboBoxes.All(static box => box.SelectedIndex is 0)
																   ? 1                // = win
																   : RandomNumber(2); // = number of choices (loss, win)
				}
			}

			//	Years can be set once results and/or centers are decided.
			if (ScoringSystem.UsesYearsPlayed)
			{
				//	The 10 below sets the latest game year for these random games to 1911.
				var maxYears = Min(10, YearsComboBoxes[0].Items.Count);
				//	First just set random years. But don't use 1901 (hence the +1 below).
				YearsComboBoxes.ForEach(box => box.SelectedIndex = RandomNumber(maxYears) + 1);
				var finalYear = YearsComboBoxes.Max(static box => box.SelectedIndex);
				//	Make sure all winners or survivors played to final year.
				YearsComboBoxes.ForSome(comboBox => ScoringSystem.UsesGameResult && ResultBoxFor(comboBox).SelectedIndex is 1
												 || ScoringSystem.UsesCenterCount && CentersBoxFor(comboBox).SelectedIndex > 0,
										box => box.SelectedIndex = finalYear);
			}

			if (!ScoringSystem.UsesCenterCount || !ScoringSystem.UsesGameResult)
				continue;

			//	TODO: Can we get into these situations, during MANUAL game filling, where things don't make sense?
			//	TODO: If so, this isn't the right place for this code.
			var minToWin = CentersComboBoxes.Max(static box => box.SelectedIndex) > 17
							   ? 18
							   : 1;
			SkipHandlers(() =>
            {
				if (NumberOfWinners > 1)
					ResultComboBoxes.ForEach(box => box.SelectedIndex = (CentersBoxFor(box).SelectedIndex >= minToWin).AsInteger());

				//	This is separate from the above. Do not make it an else if -- the NumberOfWinners may have changed.
				//	This code, which just sets the ComboBox item text if there is a sole winner is (I think) only needed
				//	here in the test game code; I'm less sure about that for the above code (hence the TODOs up there).
				var concession = minToWin is 1 && RandomNumber(7) is 0;
				if (NumberOfWinners is not 1)
					if (concession)
					{
						SoloConcededCheckBox.Checked = true;
						var bigBoy = CentersComboBoxes.Max(static box => box.SelectedIndex);
						var lucky = ResultBoxFor(CentersComboBoxes.First(box => box.SelectedIndex == bigBoy));
						ResultComboBoxes.ForEach(box => box.SelectedIndex = (box == lucky).AsInteger());
					}
					else
						return;
				//	Must use .Items[box.SelectedIndex] = x; because .SelectedItem = x; does not work
				ResultComboBoxes.ForEach(box => box.Items[box.SelectedIndex] = box.SelectedIndex is 1
																				   ? concession
																						 ? ConcText
																						 : SoloText
																				   : CentersBoxFor(box).SelectedIndex is 0
																					   ? ElimText
																					   : SurvText);
            });
		}
		while (!FinalGameDataValidation(out _));

		//	Because the test game can be changed, ensure the Concession CheckBox is enabled.
		SoloConcededCheckBox.Enabled = true;

		//	All done.
		FormEnableCallback?.Invoke(true);
	}

	internal void SetWinType(bool allowDraws,
							 int numWinners)
	{
		if (allowDraws)
			//	Turning ON AllowDraws; set win/loss dropdown text to "SOLO"
			//	if there is a sole winner, or to "DRAW" otherwise.
			ResultComboBoxes.ForEach(comboBox => comboBox.Items[1] = numWinners < 2
																  && (numWinners is 0 || comboBox.SelectedIndex is 1)
																		 ? SoloText
																		 : DrawText);
		else
			//	Turning OFF AllowDraws; set win/loss dropdown text to "SOLO"
			//	and change all to Loss if there are more than one winner.
			//	Yes, event handling must be shut off; I tested it.
			SkipHandlers(() => ResultComboBoxes.ForEach(comboBox =>
														{
															comboBox.Items[1] = SoloText;
															if (numWinners > 1)
																comboBox.SelectedIndex = 0;
														}));
	}

	internal void SetDiasOptions()
	{
		var isSolo = ResultComboBoxes.All(static box => box.SelectedItem is not null)
				  && NumberOfWinners is 1;
		foreach (var resultComboBox in ResultComboBoxes)
		{
			var centersComboBox = CentersBoxFor(resultComboBox);
			if (isSolo)
				resultComboBox.Items[0] = centersComboBox.SelectedIndex is 0
											  ? ElimText
											  : SurvText;
			else
				switch (resultComboBox.SelectedIndex)
				{
				//	Losers cannot have centers
				case 0:
					centersComboBox.SelectedIndex = 0;
					break;
				//	Winners cannot have zero centers
				case 1 when centersComboBox.SelectedIndex is 0:
					centersComboBox.Deselect();
					break;
				//	Otherwise, winners are fine
				case 1:
					break;
				//	Players with win/loss not yet known know it based on center count
				case -1:
					resultComboBox.SelectedIndex = Sign(centersComboBox.SelectedIndex);
					break;
				default:
					throw new ArgumentOutOfRangeException(); //	TODO
				}
		}
	}

	private void GameControl_EnabledChanged(object sender,
											EventArgs e)
		=> throw new NotSupportedException($"Coder issue: Use {nameof (GameControl)}.{nameof (Active)} = bool, not {nameof (GameControl)}.Enabled = bool;");

	private void OtherTextBox_TextChanged(object sender,
										  EventArgs e)
	{
		var textBox = (TextBox)sender;
		double other;
		if (textBox.TextLength is 0 || textBox.Text is "-" or "." or "-.")
			other = 0;
		else if (!double.TryParse(textBox.Text, out other))
		{
			MessageBox.Show("Other score must be numeric.",
							"Error in Other Score Entry",
							OK,
							Error);
			textBox.Text = $"{textBox.Text}"[..(textBox.TextLength - 1)];
			return;
		}
		if (GamePlayers is not null)
			GamePlayers[OtherTextBoxes.IndexOf(textBox)].Other = other;
	}

	internal delegate void Callback(bool state);

	#region GameDataChangedCallback support for the parent form

	//	Now this works slickly!  Whenever a combobox is updated, we bump the UpdateDepth
	//	by one, then drop it back down one before returning from the handler.  But since
	//	the handler will often trigger OTHER handlers (or even re-trigger the same one),
	//	the bump/drop count automatically calls the GameDataChangedCallback only when the
	//	UpdateDepth is back to 0.

	private int UpdateDepth
	{
		get;
		set
		{
			field = value;
			if (!SkippingHandlers && !SkippingCallbacks && UpdateDepth is 0)
				RunGameDataChangedCallback();
		}
	}

	[DesignerSerializationVisibility(Hidden)]
	internal Callback? GameDataChangedCallback { private get; set; }

	//	This boolean is used to shut off the callback completely, even when the UpdateDepth is 0.
	private bool SkippingCallbacks { get; set; }

	#endregion
}
