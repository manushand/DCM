using Microsoft.Data.SqlClient;

namespace PC.Forms;

internal sealed partial class SqlServerSettingsForm : Form
{
	public SqlServerSettingsForm()
		=> InitializeComponent();

	private void SqlServerSettingsForm_Load(object sender, EventArgs e)
	{
		var connectionString = Settings.DatabaseConnectionString;
		if (connectionString?.Length is null or 0)
			return;
		var stringBuilder = new SqlConnectionStringBuilder(connectionString);
		ServerNameTextBox.Text = stringBuilder.DataSource;
		DatabaseTextBox.Text = stringBuilder.InitialCatalog;
		UsernameTextBox.Text = stringBuilder.UserID;
		PasswordTextBox.Text = stringBuilder.Password;
	}

	private void ConnectButton_Click(object sender, EventArgs e)
	{
		try
		{
			var stringBuilder = new SqlConnectionStringBuilder
								{
									DataSource = ServerNameTextBox.Text,
									InitialCatalog = DatabaseTextBox.Text,
									UserID = UsernameTextBox.Text,
									Password = PasswordTextBox.Text,
									Encrypt = false
								};
			var connectionString = stringBuilder.ToString();
			if (!OpenSqlServerDatabase(connectionString))
				throw new ("Could not connect to SQL Server.");
			Close();
		}
		catch (Exception exception)
		{
			MessageBox.Show($"Connection failed: {exception.Message}", "SQL Server Connection Error", OK, Error);
		}
	}
}
