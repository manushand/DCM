namespace PC.Forms;

internal sealed partial class ConflictsForm : Form
{
	#region Public interface

	#region Constructors

	public ConflictsForm()
		=> InitializeComponent();

	internal ConflictsForm(Player player) : this()
		=> Player = player;

	#endregion

	#endregion

	#region Private implementation

	#region Type

	//	NOTE: Don't move this to a separate file in another part of this partial class;
	//	If you do, VS will think it has a visual Form and will set up a designer file.

	//	Also, do not make this a record struct; that causes it to fail
	//	(and you won't know it till runtime) when sent to .FillWith().
	private sealed record ConflictedPlayer
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
										  .Points;
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

	#region Data

	private Player? Player
	{
		get;
		set
		{
			field = value;
			if (value is null)
				return;
			Display(NewConflictLabel, NewConflictNameComboBox);
			UpdateConflictInfo();
			ConflictsDataGridView.Deselect();
			Conceal(IncreaseButton, DecreaseButton);
		}
	}

	private ConflictedPlayer? Conflictee { get; set; }

	#endregion

	#region Event handlers

	private void ConflictsForm_Load(object sender,
									EventArgs e)
	{
		//	TODO: add ability to order by last name??
		PlayerNameComboBox.FillWithSorted<Player>();
		Conceal(IncreaseButton, DecreaseButton);
		SetVisible(Player is not null, NewConflictLabel, NewConflictNameComboBox);
		PlayerNameComboBox.SetSelectedItem(Player);
	}

	private void PlayerNameComboBox_SelectedIndexChanged(object sender,
														 EventArgs e)
	{
		Player = PlayerNameComboBox.GetSelected<Player>();
		UpdateConflictInfo();
	}

	private void ModifyConflictButton_Click(object sender,
											EventArgs e)
	{
		var conflictee = Conflictee.OrThrow();
		var selected = ConflictsDataGridView.SelectedRows[0]
											.Index;
		conflictee.ModifyConflict(sender == DecreaseButton);
		UpdateConflictInfo();
		SkipHandlers(() =>
        {
			if (ConflictsDataGridView.RowCount > selected
			&&  ConflictsDataGridView.GetAtIndex<ConflictedPlayer>(selected).Player.Is(conflictee.Player))
				ConflictsDataGridView.CurrentCell = ConflictsDataGridView.Rows[selected]
																		 .Cells[0];
        });
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
		if (SkippingHandlers)
			return;
		Conflictee = (ConflictedPlayer?)ConflictsDataGridView.CurrentRow?.DataBoundItem;
		SetVisible(Conflictee is not null, IncreaseButton, DecreaseButton);
	}

	#endregion

	#region Method

	private void UpdateConflictInfo()
		=> SkipHandlers(() =>
						{
							var player = Player.OrThrow();
							var playerId = player.Id;
							var conflicts = player.PlayerConflicts;
							int[] conflictedIds = [..conflicts.SelectMany(static conflict => conflict.ConflictedPlayerIds)];
							//	TODO: This sorts by first name only
							NewConflictNameComboBox.FillWithSortedPlayers(other => !conflictedIds.Contains(other.Id)
																				   && other.Id != playerId);
							ConflictsDataGridView.FillWith(conflicts.Select(conflict => new ConflictedPlayer(conflict, playerId))
																	.OrderBy(static conflictedPlayer => $"{conflictedPlayer.Player}"));
						});

	#endregion

	#endregion
}
