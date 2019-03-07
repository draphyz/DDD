using Conditions;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    /// <summary>
    /// Represents a license number attributed to Belgian healthcare practitioners by the Belgian National Institute for Health and Disability Insurance (INAMI/RIZIV).
    /// </summary>
    public class BelgianHealthcarePractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
    {
        #region Constructors

        public BelgianHealthcarePractitionerLicenseNumber(string number) : base(number)
        {
            Condition.Requires(number, nameof(number))
                     .HasLength(11)
                     .Evaluate(c => c.IsNumeric());
        }

        #endregion Constructors
    }
}
