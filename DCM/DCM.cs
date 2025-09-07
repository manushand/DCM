using System.Numerics;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using static System.Convert;
using static System.Environment;
using static System.Math;
using static System.String;
using static System.StringComparison;

namespace DCM;

public static partial class DCM
{
	#region Constants

	public const int LatestFinalGameYear = 1918;
	public const char Comma = ',';
	public const char Colon = ':';
	public const char Semicolon = ';';

	public static readonly Func<int, int, IEnumerable<int>> Range = Enumerable.Range;

	#endregion

	#region Extensions

	public static T OrThrow<T>(this T? @this,
							   string? message = null)
		where T : class
		=> @this ?? throw new InvalidOperationException(message);

	public static void FillWith<T>(this List<T> @this,
								   [InstantHandle] IEnumerable<T> items)
	{
		@this.Clear();
		@this.AddRange(items);
	}

	public static TValue GetOrSet<TKey, TValue>(this IDictionary<TKey, TValue> @this,
												TKey key,
												[InstantHandle] Func<TKey, TValue> func)
		=> @this.TryGetValue(key, out var result)
			   ? result
			   : @this[key] = func(key);

	extension<T>(T @this)
	{
		public int AsInteger => ToInt32(@this switch
										{
											decimal @decimal => Truncate(@decimal),
											double @double   => Truncate(@double),
											string @string   => int.Parse(@string),
											_                => @this
										});
	}

	extension<T>(T @this)
		where T : INumber<T>
	{
		public string Points => $"{"pt".Pluralize(@this.AsInteger, true)}.";
	}

	extension<T>([InstantHandle] IEnumerable<T> @this)
	{
		public void ForEach([InstantHandle] Action<T> action)
			=> @this.ToList()
					.ForEach(action);

		public void ForSome([InstantHandle] Func<T, bool> func,
							[InstantHandle] Action<T> action)
			=> @this.Where(func)
					.ForEach(action);

		public IEnumerable<T> Modify([InstantHandle] Action<T> action)
			=> @this.Select(item =>
							{
								action(item);
								return item;
							});

		public void Apply([InstantHandle] Action<T, int> action)
			=> @this.Select(static (item, index) => (item, index))
					.ForEach(tuple => action(tuple.item, tuple.index));

		public string BulletList(string intro)
			=> $"{intro}:{Bullet}{Join(Bullet, @this)}";
	}

	extension(string @this)
	{
		public string[] AsEmailAddresses => [..@this.Split(EmailSplitter)
													.Select(static email => email.Trim())
													.Where(static email => email.Length is not 0)];

		public bool IsValidEmail => EmailAddressFormat.IsMatch(@this.Trim());

		public double AsDouble => double.Parse(@this);

		public int? AsNullableInteger => @this.Length is 0
											 ? null
											 : @this.AsInteger;

		public bool Matches(string other)
			=> @this.Equals(other, InvariantCultureIgnoreCase);

		public string Pluralize<T>(IReadOnlyCollection<T> items,
								   bool provideCount = false)
			=> @this.Pluralize(items.Count, provideCount);

		public string Pluralize(int count,
								bool provideCount = false)
			=> $"{(provideCount ? $"{count} " : null)}{@this}{(count is 1 ? null : 's')}";

		public T As<T>()
			where T : Enum
			=> (T)Enum.Parse(typeof (T), @this, true);

		public bool Starts(string start,
						   bool symbol = false)
			=> @this.StartsWith(start, InvariantCultureIgnoreCase)
			&& (symbol || @this.Length == start.Length || !char.IsLetterOrDigit(@this[start.Length]));
	}

	extension(Enum @this)
	{
		public string InCaps => $"{@this}".ToUpper();

		public char Abbreviation => $"{@this}".First();
	}

	extension(double @this)
	{
		public bool DoesNotEqual(double other)
			=> !@this.Equals(other);

		public bool IsBetween(double lowerBound,
							  double upperBound)
			=> @this >= lowerBound
			&& @this <= upperBound;
	}

	extension(int @this)
	{
		public string Dotted => $"{@this}.";

		public T As<T>()
			where T : Enum
			=> (T)Enum.ToObject(typeof (T), @this);

		public int NegatedIf(bool negator)
			=> negator
				   ? -@this
				   : @this;
	}

	#endregion

	#region Utility methods

	public static void ForRange(int start,
								int count,
								[InstantHandle] Action<int> action)
		=> Range(start, count).ForEach(action);

	public static int RandomNumber(int maxValue = int.MaxValue)
		=> Random.Next(maxValue);

	#endregion

	#region Private fields and method

	private static readonly string Bullet = $"{NewLine}    • ";
	private static readonly char[] EmailSplitter = [Comma, Semicolon];
	private static readonly Random Random = new ();
	private static readonly Regex EmailAddressFormat = EmailAddressRegex();

	//	TODO: There's a lot of debate about what the best way to validate an email address is.
	//		  I have usually used try { new MailAddress(text); } but it likes things I don't.
	[GeneratedRegex("^(" +
					@"[\dA-Z]" +              // Start with a digit or alphabetic character.
					@"([\+\-_\.][\dA-Z]+)*" + // No continuous or ending +-_. chars in email.
					")+" +
					@"@(([\dA-Z][-\w]*[\dA-Z]*\.)+[\dA-Z]{2,17})$",
					RegexOptions.IgnoreCase)]
	//	BUG: This compiles, but Rider (but not VisualStudio!) claims that it does not have an implementation part.
	//		 https://youtrack.jetbrains.com/issue/RIDER-129206
	private static partial Regex EmailAddressRegex();

	#endregion
}
