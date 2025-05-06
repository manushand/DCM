namespace PC.Forms;

internal sealed partial class PlayerListForm : Form
{
	private const string GamesText = "Games";

	private const string EmailLabelToolTipText = "Separate multiple addresses using comma or semicolon.";

	private static readonly string FirstNameLabelToolTipText = $"If player First Name does not begin with a letter,{NewLine}" +
															    "event and group rankings will be hidden.";

	private Player Player { get; set; } = Player.None;

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
		PlayerListBox.SelectedItem = PlayerListBox.Find(Player);
		SetEnabled(false, EmailLabel, EmailAddressTextBox);
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
		Player = PlayerListBox.GetSelected<Player>() ?? Player.None;
		SetEnabled(!Player.IsNone, EditButton, GroupsButton, RemoveButton);
		ConflictsButton.Enabled = !Player.IsNone && PlayerListBox.Items.Count > 1;
		RemoveButton.Text = Player.Games.Any() is false
								? "Remove"
								: GamesText;
		if (Player.IsNone)
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
		&&  MessageBox.Show($"Player named {name} already exists.  Add another of same name?",
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

			if (addresses.Select(static email =>
								 {
									 var playersWithThisEmail = ReadMany<Player>(player => player.EmailAddresses.Any(email.Matches)).ToArray();
									 return playersWithThisEmail.Length is not 0
											&& MessageBox.Show($"The email address {email} is already being used by:{playersWithThisEmail.BulletList()}" +
															   "Add another player with this same address?",
															   "Confirm Duplicate Player Email", YesNo, Question) is DialogResult.No;
								 })
						 .Any(static x => x))
				return;
		}
		Player = CreateOne(new Player
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
		//  Someone associated with Games cannot be removed; show which Games they are in.
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
		foreach (var teamPlayer in Player.LinksOfType<TeamPlayer>())
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
		Player = Player.None;
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
		var nowEnabled = char.IsLetter(FirstNameTextBox.Text.Trim().FirstOrDefault());
		SetEnabled(nowEnabled, EmailLabel, EmailAddressTextBox);
		if (!nowEnabled)
		{
			if (EmailAddressTextBox.TextLength is not 0)
				SavedEmail = EmailAddressTextBox.Text;
			EmailAddressTextBox.Clear();
		}
		else if (!wasEnabled)
			EmailAddressTextBox.Text = SavedEmail;
	}
}
