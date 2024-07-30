﻿namespace DCM.UI.Forms;

internal sealed partial class ConflictsForm : Form
{
	#region Form event handler

	private void ConflictsForm_Load(object sender,
									EventArgs e)
	{
		//	TODO: add ability to order by last name??
		PlayerNameComboBox.FillWithSorted<Player>();
		IncreaseButton.Visible =
			DecreaseButton.Visible =
				false;
		NewConflictLabel.Visible =
			NewConflictNameComboBox.Visible =
				Player is not null;
		PlayerNameComboBox.SetSelectedItem(Player);
	}

	#endregion

	#region Utility method

	private void UpdateConflictInfo()
	{
		var player = Player.OrThrow();
		SkipHandlers = true;
		var playerId = player.Id;
		var conflicts = player.PlayerConflicts;
		var conflictedIds = conflicts.SelectMany(static conflict => conflict.PlayerIds)
									 .ToArray();
		//	TODO: This sorts by first name only
		NewConflictNameComboBox.FillWithSortedPlayers(other => !conflictedIds.Contains(other.Id)
															   //	This && clause is overkill
															&& other.Id != playerId);
		ConflictsDataGridView.FillWith(conflicts.Select(conflict => new ConflictedPlayer(conflict, playerId))
												.OrderBy(static conflictedPlayer => conflictedPlayer.Player.Name)
												.ToArray());
		SkipHandlers = false;
	}

	#endregion

	#region ConflictedPlayer struct

	//	NOTE: Don't put this in a separate file in another part of this partial class;
	//	If you do, VS will think it has a visual Form and will set up a designer file.

	private readonly record struct ConflictedPlayer
	{
		public Player Player { get; }

		[PublicAPI]
		public string ConflictValue { get; }

		private PlayerConflict PlayerConflict { get; }

		internal ConflictedPlayer(PlayerConflict playerConflict,
								  int currentPlayerId)
		{
			PlayerConflict = playerConflict;
			Player = PlayerConflict.PlayerConflictedWith(currentPlayerId);
			ConflictValue = playerConflict.Value
										  .Points();
		}

		internal void ModifyConflict(bool decrease)
		{
			PlayerConflict.Value += decrease
										? -1
										: +1;
			if (PlayerConflict.Value is 0)
				Delete(PlayerConflict);
			else
				UpdateOne(PlayerConflict);
		}
	}

	#endregion

	#region Fields and Properties

	private Player? _player;

	private Player? Player
	{
		get => _player;
		set
		{
			_player = value;
			if (value is null)
				return;
			NewConflictLabel.Visible =
				NewConflictNameComboBox.Visible =
					true;
			UpdateConflictInfo();
			ConflictsDataGridView.Deselect();
			IncreaseButton.Visible =
				DecreaseButton.Visible =
					false;
		}
	}

	private ConflictedPlayer? Conflictee { get; set; }

	#endregion

	#region Constructors

	public ConflictsForm()
		=> InitializeComponent();

	internal ConflictsForm(Player player) : this()
		=> Player = player;

	#endregion

	#region Control event handlers

	private void PlayerNameComboBox_SelectedIndexChanged(object sender,
														 EventArgs e)
		=> Player = PlayerNameComboBox.GetSelected<Player>();

	private void ModifyConflictButton_Click(object sender,
											EventArgs e)
	{
		var conflictee = Conflictee.OrThrow();
		var selected = ConflictsDataGridView.SelectedRows[0]
											.Index;
		conflictee.ModifyConflict(sender == DecreaseButton);
		UpdateConflictInfo();
		SkipHandlers = true;
		if (ConflictsDataGridView.RowCount > selected
		 && ConflictsDataGridView.GetAtIndex<ConflictedPlayer>(selected).Player.Is(conflictee.Player))
			ConflictsDataGridView.CurrentCell = ConflictsDataGridView.Rows[selected]
																	 .Cells[0];
		SkipHandlers = false;
		ConflictsDataGridView_SelectionChanged();
	}

	private void NewConflictNameComboBox_SelectedIndexChanged(object sender,
															  EventArgs e)
	{
		var currentPlayer = Player.OrThrow();
		if (NewConflictNameComboBox.SelectedItem is not Player player)
			return;
		currentPlayer.AddPlayerConflict(player);
		UpdateConflictInfo();
		ConflictsDataGridView.SelectRowWhere<ConflictedPlayer>(conflicted => conflicted.Player.Is(player));
		ConflictsDataGridView_SelectionChanged();
	}

	private void ConflictsDataGridView_DataBindingComplete(object sender,
														   DataGridViewBindingCompleteEventArgs e)
		=> ConflictsDataGridView.FillColumn(0);

	private void ConflictsDataGridView_SelectionChanged(object? sender = null,
														EventArgs? e = null)
	{
		if (SkipHandlers)
			return;
		Conflictee = (ConflictedPlayer?)ConflictsDataGridView.CurrentRow?.DataBoundItem;
		IncreaseButton.Visible =
			DecreaseButton.Visible =
				Conflictee is not null;
	}

	#endregion
}
