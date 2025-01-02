namespace API;

internal static class API
{
	internal static string? NullIfEmpty(this string? s)
		=> string.IsNullOrWhiteSpace(s) ? null : s;

	internal static ICollection<T>? NullIfEmpty<T>(this ICollection<T> i)
		=> i.Count is 0 ? null : i;
}
