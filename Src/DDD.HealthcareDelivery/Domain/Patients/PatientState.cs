using System;

namespace DDD.HealthcareDelivery.Domain.Patients
{
    using Common.Domain;

    public class PatientState
    {

        #region Properties

        public DateTime? Birthdate { get; set; }

        public ContactInformationState ContactInformation { get; set; }

        public FullNameState FullName { get; set; }

        public int Identifier { get; set; }

        public string Sex { get; set; }

        public string SocialSecurityNumber { get; set; }

        #endregion Properties

    }
}
