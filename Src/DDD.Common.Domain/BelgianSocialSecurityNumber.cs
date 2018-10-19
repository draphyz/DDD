using Conditions;

namespace DDD.Common.Domain
{
    using Core;

    /// <summary>
    /// Represents the Belgian social security number (NISS).
    /// </summary>
    public class BelgianSocialSecurityNumber : SocialSecurityNumber
    {

        #region Constructors

        public BelgianSocialSecurityNumber(string number) : base(number)
        {
            Condition.Requires(Number, nameof(number))
                     .HasLength(11)
                     .Evaluate(c => c.IsNumeric());
        }

        #endregion Constructors

        #region Methods

        public static BelgianSocialSecurityNumber CreateIfNotEmpty(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return null;
            return new BelgianSocialSecurityNumber(number);
        }

        #endregion Methods

    }
}
