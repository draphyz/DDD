using Conditions;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    /// <summary>
    /// Represents a license number attributed to Belgian health facilities by the National Institute for Health and Disability Insurance (INAMI/RIZIV).
    /// </summary>
    public class BelgianHealthFacilityLicenseNumber : HealthFacilityLicenseNumber
    {

        #region Constructors

        public BelgianHealthFacilityLicenseNumber(string number) : base(number)
        {
            Condition.Requires(number, nameof(number))
                     .Evaluate(n => n.IsNumeric() && (n.Length == 11 || n.Length == 8));
        }

        #endregion Constructors

        #region Methods

        public static BelgianHealthFacilityLicenseNumber CreateIfNotEmpty(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return null;
            return new BelgianHealthFacilityLicenseNumber(number);
        }

        #endregion Methods

    }
}
