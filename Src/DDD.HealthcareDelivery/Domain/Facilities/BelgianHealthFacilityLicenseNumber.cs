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

        /// <summary>
        /// Computes the check digit based on the 6 first digits.
        /// </summary>
        public static int ComputeCheckDigit(string number)
        {
            Condition.Requires(number, nameof(number)).IsLongerOrEqual(6);
            var identifier = int.Parse(number.Substring(0, 6));
            var modulus = 97;
            return modulus - (identifier % modulus);
        }

        public static BelgianHealthFacilityLicenseNumber CreateIfNotEmpty(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return null;
            return new BelgianHealthFacilityLicenseNumber(number);
        }

        /// <summary>
        /// Returns the check digit based on the 6 first digits.
        /// </summary>
        public int CheckDigit() => int.Parse(this.Number.Substring(6, 2));


        /// <summary>
        /// Returns the unique identifier of the health facility.
        /// </summary>
        public int FacilityUniqueIdentifier() => int.Parse(this.Number.Substring(0, 6));

        #endregion Methods
    }
}
