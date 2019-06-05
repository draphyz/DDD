namespace DDD.HealthcareDelivery.Domain.Facilities
{
    /// <summary>
    /// Represents a facility that provides ongoing basic care in medicine and surgery with the possibility to stay overnight.
    /// </summary>
    public class Hospital : HealthFacility
    {

        #region Constructors

        public Hospital(int identifier,
                        string name,
                        HealthFacilityLicenseNumber licenseNumber = null)
            : base(identifier, name, licenseNumber)
        {
        }

        protected Hospital() { }

        #endregion Constructors
    }
}
