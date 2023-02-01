using EnsureThat;

namespace DDD.Common.Domain
{
    /// <summary>
    /// Represents a two-letter language code as defined in ISO 639.
    /// </summary>
    public class Alpha2LanguageCode : LanguageCode
    {

        #region Constructors

        public Alpha2LanguageCode(string value) : base(value)
        {
            Ensure.That(value, nameof(value)).HasLength(2);
            Ensure.That(value, nameof(value)).IsAllLetters();
        }

        protected Alpha2LanguageCode() { }

        #endregion Constructors

    }
}
