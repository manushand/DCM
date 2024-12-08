using System.Text.RegularExpressions;
using JetBrains.Annotations;
using static System.Environment;
using static System.String;

namespace DCM;

public static partial class DCM
{
	public enum PowerNames : sbyte
	{
		//	IMPORTANT: Values must be -1 through 6
		TBD = -1,
		Austria = 0,
		England = 1,
		France = 2,
		Germany = 3,
		Italy = 4,
		Russia = 5,
		Turkey = 6
	}

	public const int LatestFinalGameYear = 1918;

	public const char Comma = ',';
	public const char Colon = ':';
	public const char Semicolon = ';';

	private static readonly string Bullet = $"{NewLine}    • ";
	private static readonly char[] EmailSplitter = [Comma, Semicolon];
	private static readonly Random Random = new ();

	#region Extension methods

	public static T OrThrow<T>(this T? nullable,
							   string? message = null)
		where T : struct
		=> nullable ?? throw new InvalidOperationException(message);

	public static T OrThrow<T>(this T? nullable,
							   string? message = null)
		where T : class
		=> nullable ?? throw new InvalidOperationException(message);

	public static void FillWith<T>(this List<T> list,
								   IEnumerable<T> items)
	{
		list.Clear();
		list.AddRange(items);
	}

	public static bool Matches(this string text,
							   string other)
		=> text.Equals(other, StringComparison.InvariantCultureIgnoreCase);

	public static void ForEach<T>([InstantHandle] this IEnumerable<T> collection,
								  Action<T> action)
		=> collection.ToList()
					 .ForEach(action);

	public static void ForSome<T>([InstantHandle] this IEnumerable<T> collection,
								  Func<T, bool> func,
								  Action<T> action)
		=> collection.Where(func)
					 .ForEach(action);

	public static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
												TKey key,
												Func<TKey, TValue> func)
		=> dictionary.TryGetValue(key, out var result)
			   ? result
			   : dictionary[key] = func(key);

	public static string BulletList(this IEnumerable<object> items)
		=> $"{Bullet}{Join(Bullet, items)}";

	public static string InCaps(this Enum powerName)
		=> $"{powerName}".ToUpper();

	public static char Abbreviation(this PowerNames powerName)
		=> $"{powerName}".First();

	public static T As<T>(this int value)
		where T : Enum
		=> (T)Enum.ToObject(typeof (T), value);

	public static double AsDouble(this string value)
		=> double.Parse(value);

	public static int? AsNullableInteger(this string value)
		=> value.Length is 0
			   ? null
			   : value.AsInteger();

	public static int AsInteger(this bool value)
		=> Convert.ToInt32(value);

	public static int AsInteger(this string value)
		=> int.Parse(value);

	public static int AsInteger<T>(this T value)
		where T : Enum
		=> (int)Convert.ChangeType(value, typeof (int));

	public static string Dotted(this int value)
		=> $"{value}.";

	public static int NegatedIf(this int value, bool negator)
		=> negator ? -value : value;

	public static string Points(this double number)
		=> ((int)number).Points();

	public static string Points(this int number)
		=> $"{"pt".Pluralize(number, true)}.";

	public static bool NotEquals(this double @this, double other)
		=> !@this.Equals(other);

	public static void Apply<T>(this IEnumerable<T> items,
								Action<T, int> func)
		where T : class
		=> items.Select(static (item, index) => (item, index))
				.ForEach(tuple => func(tuple.item, tuple.index));

	public static string Pluralize<T>(this string what,
									  IEnumerable<T> items,
									  bool provideCount = false)
		=> what.Pluralize(items.Count(), provideCount);

	public static string Pluralize(this string what,
								   int count,
								   bool provideCount = false)
		=> $"{(provideCount ? $"{count} " : null)}{what}{(count is 1 ? null : 's')}";

	//	TODO: there's a lot of debate about what the best way to validate an email address is
	//	TODO: I have usually used try { new MailAddress(text); } but it likes things I don't.
	[GeneratedRegex("^(" +
					@"[\dA-Z]" +              //	Start with a digit or alphabetic character
					@"([\+\-_\.][\dA-Z]+)*" + //	No continuous or ending +-_. chars in email
					")+" +
					@"@(([\dA-Z][-\w]*[\dA-Z]*\.)+[\dA-Z]{2,17})$",
					RegexOptions.IgnoreCase)]
	private static partial Regex EmailAddressRegex();

	private static readonly Regex EmailAddressFormat = EmailAddressRegex();

	public static bool IsValidEmail(this string text)
		=> EmailAddressFormat.IsMatch(text.Trim());

	public static T As<T>(this string text)
		where T : Enum
		=> (T)Enum.Parse(typeof (T), text, true);

	public static bool Starts(this string text,
							  string start,
							  bool symbol = false)
		=> text.StartsWith(start, StringComparison.InvariantCultureIgnoreCase)
		&& (symbol || text.Length == start.Length || !char.IsLetterOrDigit(text[start.Length]));

	public static string[] SplitEmailAddresses(this string addresses)
		=> [..addresses.Split(EmailSplitter)
					   .Select(static email => email.Trim())
					   .Where(static email => email.Length is not 0)];

	#endregion

	#region Other utility methods

	public static void ForRange(int start, int count, Action<int> action)
		=> Range(start, count).ForEach(action);

	public static IEnumerable<int> Range(int start, int count)
		=> Enumerable.Range(start, count);

	public static int RandomNumber(int maxValue = int.MaxValue)
		=> Random.Next(maxValue);

	#endregion
}
