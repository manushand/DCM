namespace API;

[PublicAPI]
public sealed record Error
{
	public string Cause { get; }
	public string[] Details { get; }

	internal Error(Exception exception)
	{
		Cause = exception.Message;
		Details = exception.Data[nameof (Details)] as string[] ?? [];
	}

	internal static Exception Exception(string message,
										params IEnumerable<string?> details)
	{
		var exception = new Exception(message);
		exception.Data.Add(nameof (Details), details.Where(static text => text?.Length > 0));
		return exception;
	}
}
