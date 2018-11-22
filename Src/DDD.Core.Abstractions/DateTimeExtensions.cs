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
        /// Converts the value of the current DateTime object to its equivalent short date string representation using the culture-specific format information.
        /// </summary>
        public static string ToShortDateString(this DateTime instance, IFormatProvider provider)
        {
            return instance.ToString("d", provider);
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent short date string representation using the french culture-specific format information.
        /// </summary>
        public static string ToFrenchShortDateString(this DateTime instance)
        {
            return instance.ToString("d", new CultureInfo("fr-FR"));
        }

        #endregion Methods

    }
}
