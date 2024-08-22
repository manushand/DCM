namespace DCM.UI.Forms;

internal sealed partial class PlayerListForm : Form
{
	private const string GamesText = "Games";

	private const string EmailLabelToolTipText = "Separate multiple addresses using comma or semicolon.";

	private static readonly string FirstNameLabelToolTipText = $"If player First Name does not begin with a letter,{NewLine}" +
															   "tournament and group rankings will be hidden.";

	private Player? _player;

	private Player Player => _player.OrThrow();

	private string? SavedEmail { get; set; }

	public PlayerListForm()
		=> InitializeComponent();

	private void PlayerListForm_Load(object sender,
									 EventArgs e)
	{
		ToolTip.SetToolTip(FirstNameLabel, FirstNameLabelToolTipText);
		ToolTip.SetToolTip(EmailLabel, EmailLabelToolTipText);
		LastNameRadioButton.Checked = true;
		AddPlayerButton.Enabled = false;
		Refill();
		PlayerListBox_SelectedIndexChanged(); //	TODO: Seems to be needed to disable buttons??
	}

	private void Refill(object? sender = null,
						EventArgs? e = null)
	{
		PlayerListBox.FillWithSortedPlayers(LastNameRadioButton.Checked);
		PlayerListBox.SelectedItem = PlayerListBox.Find(_player);
		EmailLabel.Enabled =
			EmailAddressTextBox.Enabled =
				false;
	}

	private void NewPlayerTextBoxes_GotFocus(object sender,
											 EventArgs e)
	{
		PlayerListBox.ClearSelected();
		AddPlayerButton.Enabled = true;
	}

	private void PlayerListBox_SelectedIndexChanged(object? sender = null,
													EventArgs? e = null)
	{
		_player = PlayerListBox.GetSelected<Player>();
		EditButton.Enabled =
			GroupsButton.Enabled =
				RemoveButton.Enabled =
					_player is not null;
		ConflictsButton.Enabled = _player is not null && PlayerListBox.Items.Count > 1;
		RemoveButton.Text = _player?.Games.Any() is false
								? "Remove"
								: GamesText;
		if (_player is null)
			return;
		FirstNameTextBox.Text =
			LastNameTextBox.Text =
				EmailAddressTextBox.Text =
					SavedEmail =
						null;
		AddPlayerButton.Enabled = false;
	}

	private void AddPlayerButton_Click(object sender,
									   EventArgs e)
	{
		var firstName = FirstNameTextBox.Text
										.Trim();
		var lastName = LastNameTextBox.Text
									  .Trim();
		if ((firstName.Length | lastName.Length) is 0)
		{
			MessageBox.Show("Player First and Last Names are required.",
							"Player Name Required",
							OK,
							Error);
			return;
		}
		var name = $"{firstName} {lastName}";
		if (ReadMany<Player>(player => player.Name.Matches(name)).Any()
		 && MessageBox.Show($"Player named {name} already exists.  Add another of same name?",
							"Confirm Duplicate Player Name",
							YesNo,
							Question) is DialogResult.No)
			return;
		var addresses = EmailAddressTextBox.Text
										   .SplitEmailAddresses();
		if (addresses.Length != 0)
		{
			var badAddress = addresses.FirstOrDefault(static email => !email.IsValidEmail());
			if (badAddress is not null)
			{
				MessageBox.Show($"Invalid email address ({badAddress}) provided.",
								"Invalid Player Email",
								OK,
								Error);
				return;
			}
			if ((from email in addresses
				 let playersWithThisEmail = ReadMany<Player>(player => player.EmailAddresses
                                                                             .Any(email.Matches))
                                                                             .ToArray()
				 where playersWithThisEmail.Length is not 0
                   && MessageBox.Show($"The email address {email} is already being used by:{playersWithThisEmail.BulletList()}" +
									   "Add another player with this same address?",
									   "Confirm Duplicate Player Email",
									   YesNo,
									   Question) is DialogResult.No
				 select true).Any())
				return;
		}
		_player = CreateOne(new Player
							{
								FirstName = firstName,
								LastName = lastName,
								EmailAddress = Join(';', addresses)
							});
		SavedEmail = null;
		Refill();
	}

	private void RemoveButton_Click(object sender,
									EventArgs e)
	{
		if (Player.Games.Any())
		{
			Show<GamesForm>(() => new (Player));
			return;
		}
		if (MessageBox.Show($"Really remove {Player}?",
							"Confirm Player Removal",
							YesNo,
							Question) is DialogResult.No)
			return;
		Delete(Player.LinksOfType<RoundPlayer>());
		foreach (var teamPlayer in Player.TeamPlayers)
		{
			Delete(teamPlayer);
			//	TODO: Here we are deleting emptied teams.  Good?  I think so.
			if (teamPlayer.Team.Players.Length is 0)
				Delete(teamPlayer.Team);
		}
		Delete(Player.LinksOfType<TournamentPlayer>());
		foreach (var groupPlayer in Player.LinksOfType<GroupPlayer>())
		{
			Delete(groupPlayer);
			//	TODO: Here we are deleting emptied groups.  Good?  I think so, but not as much so as above.
			if (!groupPlayer.Group.Players.Any())
				Delete(groupPlayer.Group);
		}
		Delete(Player.PlayerConflicts);
		Delete(Player);
		_player = null;
		Refill();
	}

	private void EditButton_Click(object sender,
								  EventArgs e)
		=> Show<PlayerInfoForm>(() => new (Player),
								form =>
								{
									if (form.DialogResult is DialogResult.OK)
										Refill();
								});

	private void GroupsButton_Click(object sender,
									EventArgs e)
	{
		if (MessageBox.Show($"{Player.GroupMemberships}{NewLine}{NewLine}Do you want to manage Player Groups?",
							"Group Memberships",
							YesNo,
							Question) is DialogResult.Yes)
			Show<GroupsForm>();
	}

	private void ConflictsButton_Click(object sender,
									   EventArgs e)
		=> Show<ConflictsForm>(() => new (Player));

	private void FirstNameTextBox_KeyUp(object sender,
										EventArgs e)
	{
		var wasEnabled = EmailAddressTextBox.Enabled;
		var nowEnabled =
			EmailLabel.Enabled =
				EmailAddressTextBox.Enabled =
					char.IsLetter(FirstNameTextBox.Text.Trim().FirstOrDefault());
		if (!nowEnabled)
		{
			if (EmailAddressTextBox.TextLength > 0)
				SavedEmail = EmailAddressTextBox.Text;
			EmailAddressTextBox.Text = null;
		}
		else if (!wasEnabled)
			EmailAddressTextBox.Text = SavedEmail;
	}
}
