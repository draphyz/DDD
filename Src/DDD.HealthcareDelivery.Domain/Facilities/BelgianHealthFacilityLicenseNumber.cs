namespace DDD.HealthcareDelivery.Domain.Facilities
{
    /// <summary>
    /// Represents a license number attributed to Belgian health facilities (INAMI).
    /// </summary>
    public class BelgianHealthFacilityLicenseNumber : HealthFacilityLicenseNumber
    {
        #region Constructors

        public BelgianHealthFacilityLicenseNumber(string number) : base(number)
        {
        }

        #endregion Constructors
    }
}
