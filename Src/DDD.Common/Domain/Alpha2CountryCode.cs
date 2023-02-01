using EnsureThat;

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
            Ensure.That(value, nameof(value)).HasLength(2);
            Ensure.That(value, nameof(value)).IsAllLetters();
        }

        protected Alpha2CountryCode() { }

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
