using JetBrains.Annotations;

namespace API;

[PublicAPI]
internal class Player : Rest<Player, Data.Player>
{
	public string FirstName
	{
		get => Data.FirstName;
		private set => Data.FirstName = value;
	}
	public string LastName
	{
		get => Data.LastName;
		private set => Data.LastName = value;
	}

	protected override dynamic Detail => new
										 {
											 EmailAddresses = Data.EmailAddresses.NullIfEmpty()
										 };

	protected override void Update(dynamic record)
	{
		Data.FirstName = record.FirstName;
		Data.LastName = record.LastName;
		Data.EmailAddress = string.Join(",", record.Details.EmailAddresses);
	}
}
