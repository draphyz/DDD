using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Conditions;
using System.Linq;
using System.Text;

namespace DDD
{
    /// <summary>
    /// Adds extension methods to the <see cref="string" /> class.
    /// </summary>
    public static class StringExtensions
    {

        #region Methods

        /// <summary>
        /// Determines whether the specified string only consists of letters.
        /// </summary>
        public static bool IsAlphabetic(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return instance.ToCharArray().All(char.IsLetter);
        }

        /// <summary>
        /// Determines whether the specified string only consists of letters and/or digits.
        /// </summary>
        public static bool IsAlphanumeric(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return instance.ToCharArray().All(char.IsLetterOrDigit);
        }

        /// <summary>
        /// Determines whether the specified string is formatted using the short date string representation associated with the french culture-specific format information.
        /// </summary>
        /// <param name="instance">The current instance.</param>
        public static bool IsFrenchShortDateString(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return IsShortDateString(instance, CultureInfo.CreateSpecificCulture("fr"));
        }

        /// <summary>
        /// Determines whether the specified string only consists of digits.
        /// </summary>
        public static bool IsNumeric(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return instance.ToCharArray().All(char.IsDigit);
        }

        /// <summary>
        /// Determines whether the specified string is formatted using the short date string representation associated with the culture-specific format information.
        /// </summary>
        /// <param name="instance">The current instance.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        public static bool IsShortDateString(this string instance, IFormatProvider provider)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return DateTime.TryParseExact(instance, "d", provider, DateTimeStyles.None, out _);
        }
        /// <summary>
        /// Returns a string containing a specified number of characters from the left side of a string.
        /// </summary>
        /// <param name="instance">The current instance.</param>
        /// <param name="length">Numeric expression indicating how many characters to return.</param>
        /// <returns>
        /// If the specified length is 0, an empty string is returned. 
        /// If the specified length is greater than or equal to the number of characters in the string, the entire string is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Throws an exception when the specified length is less than zero.</exception>
        public static string Left(this string instance, int length)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            Condition.Requires(length, nameof(length)).IsGreaterOrEqual(0);
            if (length == 0) return string.Empty;
            if (length >= instance.Length) return instance;
            return instance.Substring(0, length);
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string, ignoring or honoring their case.
        /// </summary>
        /// <param name="instance">The current instance.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of oldValue.</param>
        /// <param name="ignoreCase">true to ignore case during the replacement; otherwise, false.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of oldValue are replaced with newValue.</returns>
        public static string Replace(this string instance, string oldValue, string newValue, bool ignoreCase)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            if (ignoreCase) return Regex.Replace(instance, Regex.Escape(oldValue), Regex.Escape(newValue), RegexOptions.IgnoreCase);
            return instance.Replace(oldValue, newValue);
        }

        /// <summary>
        /// Reverses order of characters in a string.
        /// </summary>
        public static string Reverse(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            if (instance.Length <= 1) return instance;
            var sb = new StringBuilder(instance.Length);
            for (var i = instance.Length - 1; i >= 0; i--)
                sb.Append(instance[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string containing a specified number of characters from the right side of a string.
        /// </summary>
        /// <param name="instance">The current instance.</param>
        /// <param name="length">Numeric expression indicating how many characters to return.</param>
        /// <returns>
        /// If the specified length is 0, an empty string is returned. 
        /// If the specified length is greater than or equal to the number of characters in the string, the entire string is returned.
        /// </returns>
        /// <exception cref="ArgumentException">Throws an exception when the specified length is less than zero.</exception>
        public static string Right(this string instance, int length)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            Condition.Requires(length, nameof(length)).IsGreaterOrEqual(0);
            if (length == 0) return string.Empty;
            if (length >= instance.Length) return instance;
            return instance.Substring(instance.Length - length);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. 
        /// A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <typeparam name="TEnum">An enumeration type.</typeparam>
        /// <param name="instance">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">True to ignore case; false to regard case.</param>
        /// <returns>An object of type TEnum whose value is represented by value.</returns>
        public static TEnum ToEnum<TEnum>(this string instance, bool ignoreCase = true) where TEnum : struct
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return (TEnum)Enum.Parse(typeof(TEnum), instance, ignoreCase);
        }

        /// <summary>
        /// Converts a camel case string to snake case (underscore case).
        /// </summary>
        /// <param name="instance">A camel case string.</param>
        /// <returns>The specified string converted to snake case.</returns>
        public static string ToSnakeCase(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return Regex.Replace(instance, "((?<=[a-z])(?=[A-Z]))|((?<=[A-Z])(?=[A-Z][a-z]))", "_");
        }

        /// <summary>
        /// Converts the specified string to title case.
        /// </summary>
        /// <param name="instance">The current instance.</param>
        /// <returns>The specified string converted to title case.</returns>
        /// <remarks>The current implementation of the ToTitleCase method provides an arbitrary casing behavior which is not necessarily linguistically correct.</remarks>
        public static string ToTitleCase(this string instance)
        {
            Condition.Requires(instance, nameof(instance)).IsNotNull();
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(instance.ToLower());
        }

        #endregion Methods

    }
}