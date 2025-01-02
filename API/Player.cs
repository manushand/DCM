using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Player : Rest<Player, Data.Player>
{
	public string FirstName
	{
		get => Record.FirstName;
		private set => Record.FirstName = value;
	}
	public string LastName
	{
		get => Record.LastName;
		private set => Record.LastName = value;
	}

	protected override dynamic Detail => new
										 {
											 EmailAddresses = Record.EmailAddresses.NullIfEmpty()
										 };

	protected override void Update(dynamic record)
	{
		Record.FirstName = record.FirstName;
		Record.LastName = record.LastName;
		Record.EmailAddress = string.Join(",", record.Details.EmailAddresses);
	}
}
