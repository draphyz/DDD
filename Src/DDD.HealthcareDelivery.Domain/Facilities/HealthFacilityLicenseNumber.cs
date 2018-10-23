namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Common.Domain;

    public abstract class HealthFacilityLicenseNumber : IdentificationNumber
    {

        #region Constructors

        protected HealthFacilityLicenseNumber(string number) : base(number)
        {
        }

        #endregion Constructors

    }
}