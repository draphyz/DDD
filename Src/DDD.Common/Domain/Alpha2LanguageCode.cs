using Conditions;

namespace DDD.Common.Domain
{
    /// <summary>
    /// Represents a two-letter language code as defined in ISO 639.
    /// </summary>
    public class Alpha2LanguageCode : LanguageCode
    {

        #region Constructors

        public Alpha2LanguageCode(string code) : base(code)
        {
            Condition.Requires(code, nameof(code))
                     .HasLength(2)
                     .Evaluate(c => c.IsAlphabetic());
        }

        #endregion Constructors

    }
}
