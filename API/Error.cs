namespace API;

[PublicAPI]
internal sealed class Error(Exception exception)
{
	public string Cause { get; } = exception.Message;
	public string[] Details { get; } = exception.Data[nameof (Details)] as string[] ?? [];

	internal static Exception Exception(string message,
										params IEnumerable<string?> details)
	{
		var exception = new Exception(message);
		exception.Data.Add(nameof (Details), details.Where(static text => text?.Length > 0));
		return exception;
	}
}
