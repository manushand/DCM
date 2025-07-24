using System.Numerics;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using static System.Convert;
using static System.Environment;
using static System.String;
using static System.StringComparison;

namespace DCM;

public static partial class DCM
{
	#region Constant values

	public const int LatestFinalGameYear = 1918;
	public const char Comma = ',';
	public const char Colon = ':';
	public const char Semicolon = ';';

	#endregion

	#region Extension methods

	private static readonly string Bullet = $"{NewLine}    • ";
	private static readonly char[] EmailSplitter = [Comma, Semicolon];
	private static readonly Random Random = new ();

	public static T OrThrow<T>(this T? @this,
							   string? message = null)
		where T : class
		=> @this ?? throw new InvalidOperationException(message);

	public static void FillWith<T>(this List<T> @this,
								   IEnumerable<T> items)
	{
		@this.Clear();
		@this.AddRange(items);
	}

	public static void ForEach<T>([InstantHandle] this IEnumerable<T> @this,
								  Action<T> action)
		=> @this.ToList()
				.ForEach(action);

	public static void ForSome<T>([InstantHandle] this IEnumerable<T> @this,
								  Func<T, bool> func,
								  Action<T> action)
		=> @this.Where(func)
				.ForEach(action);

	public static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this,
												TKey key,
												Func<TKey, TValue> func)
		=> @this.TryGetValue(key, out var result)
			   ? result
			   : @this[key] = func(key);

	public static string BulletList(this IEnumerable<object> @this,
									string intro)
		=> $"{intro}:{Bullet}{Join(Bullet, @this)}";

	public static string InCaps(this Enum @this)
		=> $"{@this}".ToUpper();

	public static char Abbreviation(this Enum @this)
		=> $"{@this}".First();

	public static T As<T>(this int @this)
		where T : Enum
		=> (T)Enum.ToObject(typeof (T), @this);

	public static double AsDouble(this string @this)
		=> double.Parse(@this);

	public static int? AsNullableInteger(this string @this)
		=> @this.Length is 0
			   ? null
			   : @this.AsInteger();

	public static int AsInteger(this object @this)
		=> ToInt32(@this);

	public static int AsInteger(this string @this)
		=> int.Parse(@this);

	public static int AsInteger(this Enum @this)
		=> (int)ChangeType(@this, typeof (int));

	public static string Dotted(this int @this)
		=> $"{@this}.";

	public static int NegatedIf(this int @this,
								bool negator)
		=> negator
			   ? -@this
			   : @this;

	public static string Points<T>(this T @this)
		where T : INumber<T>
		=> $"{"pt".Pluralize(@this.AsInteger(), true)}.";

	public static bool NotEquals(this double @this,
								 double other)
		=> !@this.Equals(other);

	public static bool IsBetween(this double @this,
								 double lowerBound,
								 double upperBound)
		=> lowerBound <= upperBound
		&& @this >= lowerBound
		&& @this <= upperBound;

	public static void Apply<T>(this IEnumerable<T> @this,
								Action<T, int> func)
		where T : class
		=> @this.Select(static (item, index) => (item, index))
				.ForEach(tuple => func(tuple.item, tuple.index));

	public static bool Matches(this string @this,
							   string other)
		=> @this.Equals(other, InvariantCultureIgnoreCase);

	public static string Pluralize<T>(this string @this,
									  IEnumerable<T> items,
									  bool provideCount = false)
		=> @this.Pluralize(items.Count(), provideCount);

	public static string Pluralize(this string @this,
								   int count,
								   bool provideCount = false)
		=> $"{(provideCount ? $"{count} " : null)}{@this}{(count is 1 ? null : 's')}";

	public static bool IsValidEmail(this string @this)
		=> EmailAddressFormat.IsMatch(@this.Trim());

	public static T As<T>(this string @this)
		where T : Enum
		=> (T)Enum.Parse(typeof (T), @this, true);

	public static bool Starts(this string @this,
							  string start,
							  bool symbol = false)
		=> @this.StartsWith(start, InvariantCultureIgnoreCase)
		&& (symbol || @this.Length == start.Length || !char.IsLetterOrDigit(@this[start.Length]));

	public static string[] SplitEmailAddresses(this string @this)
		=> [..@this.Split(EmailSplitter)
				   .Select(static email => email.Trim())
				   .Where(static email => email.Length is not 0)];

	#endregion

	#region Other utility methods

	public static void ForRange(int start,
								int count,
								Action<int> action)
		=> Range(start, count).ForEach(action);

	public static IEnumerable<int> Range(int start,
										 int count)
		=> Enumerable.Range(start, count);

	public static int RandomNumber(int maxValue = int.MaxValue)
		=> Random.Next(maxValue);

	#endregion

	#region Generated Regular Expressions

	//	TODO: there's a lot of debate about what the best way to validate an email address is
	//	TODO: I have usually used try { new MailAddress(text); } but it likes things I don't.
	[GeneratedRegex("^(" +
					@"[\dA-Z]" +              //	Start with a digit or alphabetic character.
					@"([\+\-_\.][\dA-Z]+)*" + //	No continuous or ending +-_. chars in email.
					")+" +
					@"@(([\dA-Z][-\w]*[\dA-Z]*\.)+[\dA-Z]{2,17})$",
					RegexOptions.IgnoreCase)]
	private static partial Regex EmailAddressRegex();
	private static readonly Regex EmailAddressFormat = EmailAddressRegex();

	#endregion
}
