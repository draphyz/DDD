using System;
using System.Globalization;

namespace DDD
{
    /// <summary>
    /// Adds extension methods to the <see cref="DateTime" /> class.
    /// </summary>
    public static class DateTimeExtensions
    {

        #region Methods

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent short date string representation using the french culture-specific format information.
        /// </summary>
        public static string ToFrenchShortDateString(this DateTime instance)
        {
            return instance.ToString("d", new CultureInfo("fr-FR"));
        }

        /// <summary>
        /// Converts the value of the current nullable DateTime object to its equivalent not nullable representation.
        /// </summary>
        public static DateTime ToNotNullable(this DateTime? instance)
        {
            if (instance == null) return default;
            return instance.Value;
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent nullable representation.
        /// </summary>
        public static DateTime? ToNullable(this DateTime instance)
        {
            if (instance == default) return null;
            return instance;
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent short date string representation using the culture-specific format information.
        /// </summary>
        public static string ToShortDateString(this DateTime instance, IFormatProvider provider)
        {
            return instance.ToString("d", provider);
        }

        #endregion Methods

    }
}
