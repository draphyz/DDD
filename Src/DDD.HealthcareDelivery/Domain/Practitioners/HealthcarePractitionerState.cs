namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    using Common.Domain;

    public class HealthcarePractitionerState
    {

        #region Properties

        public ContactInformationState ContactInformation { get; set; }

        public string DisplayName { get; set; }

        public FullNameState FullName { get; set; }

        public int Identifier { get; set; }

        public string LicenseNumber { get; set; }

        public string PractitionerType { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string Speciality { get; set; }

        #endregion Properties
    }
}
