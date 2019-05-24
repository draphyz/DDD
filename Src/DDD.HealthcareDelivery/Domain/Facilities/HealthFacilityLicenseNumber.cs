namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Common.Domain;

    public abstract class HealthFacilityLicenseNumber : IdentificationNumber
    {

        #region Constructors

        protected HealthFacilityLicenseNumber(string value) : base(value)
        {
        }

        #endregion Constructors

    }
}