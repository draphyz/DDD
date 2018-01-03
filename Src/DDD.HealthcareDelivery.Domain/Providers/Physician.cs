using DDD.Common.Domain;

namespace DDD.HealthcareDelivery.Domain.Providers
{
    public class Physician : HealthcareProvider
    {

        #region Constructors

        public Physician(int identifier, 
                         FullName fullName, 
                         PractitionerLicenseNumber licenseNumber,
                         SocialSecurityNumber socialSecurityNumber = null,
                         ContactInformation contactInformation = null, 
                         string speciality = null,
                         string displayName = null) 
            : base(identifier, fullName, licenseNumber, socialSecurityNumber, contactInformation, speciality, displayName)
        {
        }

        #endregion Constructors

        #region Methods

        public override HealthcareProviderState ToState()
        {
            var state = base.ToState();
            state.ProviderType = HealthcareProviderType.Physician.ToString();
            return state;
        }

        #endregion Methods

    }
}
