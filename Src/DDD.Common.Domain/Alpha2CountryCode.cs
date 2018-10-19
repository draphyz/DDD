using Conditions;

namespace DDD.Common.Domain
{
    using Core;

    /// <summary>
    /// Represents a two-letter country code as defined in ISO 3166-1.
    /// </summary>
    public class Alpha2CountryCode : CountryCode
    {

        #region Constructors

        public Alpha2CountryCode(string code) : base(code)
        {
            Condition.Requires(code, nameof(code))
                     .HasLength(2)
                     .Evaluate(c => c.IsAlphabetic());
        }

        #endregion Constructors

        #region Methods

        public static Alpha2CountryCode CreateIfNotEmpty(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            return new Alpha2CountryCode(code);
        }

        #endregion Methods

    }
}
