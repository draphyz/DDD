using Conditions;

namespace DDD.HealthcareDelivery.Domain.Providers
{
    using Core;

    /// <summary>
    /// Represents the Belgian practitioner license number (INAMI).
    /// </summary>
    public class BelgianPractitionerLicenseNumber : PractitionerLicenseNumber
    {
        #region Constructors

        public BelgianPractitionerLicenseNumber(string number) : base(number)
        {
            Condition.Requires(number, nameof(number))
                     .HasLength(11)
                     .Evaluate(c => c.IsNumeric());
        }

        #endregion Constructors
    }
}
