using System;
using System.Globalization;
using Conditions;

namespace DDD
{
    /// <summary>
    /// Adds extension methods to the <see cref="DateTime" /> class.
    /// </summary>
    public static class DateTimeExtensions
    {

        #region Methods

        /// <summary>
        /// Gets the next date and time.
        /// </summary>
        public static DateTime Next(this DateTime instance)
        {
            return instance.AddTicks(1);
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent short date string representation using the french culture-specific format information.
        /// </summary>
        public static string ToFrenchShortDateString(this DateTime instance)
        {
            return instance.ToString("d", new CultureInfo("fr-FR"));
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
