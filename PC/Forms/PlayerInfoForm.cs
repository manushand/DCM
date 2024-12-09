namespace PC.Forms;

internal sealed partial class PlayerInfoForm : Form
{
	internal Player Player { get; }

	public PlayerInfoForm() : this(null) { }

	internal PlayerInfoForm(Player? player = null)
	{
		InitializeComponent();
		Player = player ?? new Player();
		if (player is null)
			return;
		FirstNameTextBox.Text = player.FirstName;
		LastNameTextBox.Text = player.LastName;
		EmailAddressTextBox.Text = player.EmailAddress;
	}

	private void OkButton_Click(object sender,
								EventArgs e)
	{
		var firstName = FirstNameTextBox.Text
										.Trim();
		var lastName = LastNameTextBox.Text
									  .Trim();
		var name = $"{firstName} {lastName}";
		var players = ReadMany<Player>(player => player.Name.Matches(name)).ToArray();
		if (players.Any(player => player.IsNot(Player))
		 && MessageBox.Show($"Player named {name} already exists.  Use this same name?",
							"Confirm Duplicate Player Name",
							YesNo,
							Question) is DialogResult.No)
			return;
		var emailAddress = EmailAddressTextBox.Text
											  .Trim();
		string? error = null;
		if (firstName.Length * lastName.Length is 0)
			error = "Player First and Last Names are required.";
		else if (emailAddress.Length is not 0)
			if (!emailAddress.IsValidEmail())
				error = "Invalid email address.";
			else if (!emailAddress.Matches(Player.EmailAddress))
			{
				var playersWithThisEmail = ReadMany<Player>(player => player.EmailAddresses
																			.Any(emailAddress.Matches))
                                                                            .ToArray();
				if (playersWithThisEmail.Length > 0
				 && MessageBox.Show($"The email address {emailAddress} is already being used by:{playersWithThisEmail.BulletList()}" +
									"Have this player use the same address?",
									"Confirm Duplicate Player Email",
									YesNo,
									Question) is DialogResult.No)
					return;
			}
		if (error is null)
		{
			Player.FirstName = firstName;
			Player.LastName = lastName;
			Player.EmailAddress = emailAddress;
			if (Player.Id is 0)
				CreateOne(Player);
			else
				UpdateOne(Player);
			Close();
			DialogResult = DialogResult.OK;
			return;
		}
		MessageBox.Show(error,
						"Invalid Player Details",
						OK,
						Error);
	}
}
