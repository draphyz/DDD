namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    using Common.Domain;

    /// <summary>
    /// Represents a license number attributed to healthcare practitioners.
    /// </summary>
    public abstract class HealthcarePractitionerLicenseNumber : IdentificationNumber
    {

        #region Constructors

        protected HealthcarePractitionerLicenseNumber(string value) : base(value)
        {
        }

        #endregion Constructors

    }
}