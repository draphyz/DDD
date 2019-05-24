using Conditions;

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
            Condition.Requires(value, nameof(value))
                     .HasLength(2)
                     .Evaluate(c => c.IsAlphabetic());
        }

        #endregion Constructors

    }
}
