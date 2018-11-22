namespace DDD.HealthcareDelivery.Domain.Providers
{
    using Common.Domain;

    public class HealthcareProviderState
    {

        #region Properties

        public ContactInformationState ContactInformation { get; set; }

        public string DisplayName { get; set; }

        public FullNameState FullName { get; set; }

        public int Identifier { get; set; }

        public string LicenseNumber { get; set; }

        public string ProviderType { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string Speciality { get; set; }

        #endregion Properties
    }
}
