using Conditions;

namespace DDD.Common.Domain
{
    /// <summary>
    /// Represents a two-letter country code as defined in ISO 3166-1.
    /// </summary>
    public class Alpha2CountryCode : CountryCode
    {

        #region Constructors

        public Alpha2CountryCode(string value) : base(value)
        {
            Condition.Requires(value, nameof(value))
                     .HasLength(2)
                     .Evaluate(c => c.IsAlphabetic());
        }

        #endregion Constructors

        #region Methods

        public static Alpha2CountryCode CreateIfNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return new Alpha2CountryCode(value);
        }

        #endregion Methods

    }
}
