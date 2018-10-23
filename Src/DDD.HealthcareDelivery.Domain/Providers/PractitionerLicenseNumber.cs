namespace DDD.HealthcareDelivery.Domain.Providers
{
    using Common.Domain;

    public abstract class PractitionerLicenseNumber : IdentificationNumber
    {

        #region Constructors

        protected PractitionerLicenseNumber(string number) : base(number)
        {
        }

        #endregion Constructors

    }
}