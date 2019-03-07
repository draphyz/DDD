using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Domain.Facilities;
    using Domain.Practitioners;
    using Core.Application;
    using Common.Application;

    /// <summary>
    /// Encapsulates all information needed to create a list of pharmaceutical prescriptions associated with one prescriber and one patient.
    /// </summary>
    public class CreatePharmaceuticalPrescriptions : ICommand
    {

        #region Properties

        public string HealthFacilityCode { get; set; }

        public int HealthFacilityIdentifier { get; set; }

        public string HealthFacilityLicenseNumber { get; set; }

        public string HealthFacilityName { get; set; }

        public HealthFacilityType HealthFacilityType { get; set; }

        public string LanguageCode { get; set; }

        public DateTime? PatientBirthdate { get; set; }

        public string PatientFirstName { get; set; }

        public int PatientIdentifier { get; set; }

        public string PatientLastName { get; set; }

        public Sex PatientSex { get; set; }

        public string PatientSocialSecurityNumber { get; set; }

        public string PrescriberBoxNumber { get; set; }

        public string PrescriberCity { get; set; }

        public string PrescriberCountryCode { get; set; }

        public string PrescriberDisplayName { get; set; }

        public string PrescriberFirstName { get; set; }

        public string PrescriberHouseNumber { get; set; }

        public int PrescriberIdentifier { get; set; }

        public string PrescriberLastName { get; set; }

        public string PrescriberLicenseNumber { get; set; }

        public string PrescriberPostalCode { get; set; }

        public string PrescriberPrimaryEmailAddress { get; set; }

        public string PrescriberPrimaryTelephoneNumber { get; set; }

        public string PrescriberSecondaryEmailAddress { get; set; }

        public string PrescriberSecondaryTelephoneNumber { get; set; }

        public string PrescriberSocialSecurityNumber { get; set; }

        public string PrescriberSpeciality { get; set; }

        public string PrescriberStreet { get; set; }

        public HealthcarePractitionerType PrescriberType { get; set; }

        public string PrescriberWebSite { get; set; }
        public ICollection<PharmaceuticalPrescriptionDescriptor> Prescriptions { get; set; }
            = new List<PharmaceuticalPrescriptionDescriptor>();

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{this.GetType().Name} [prescriberIdentifier={this.PrescriberIdentifier}, patientIdentifier={this.PatientIdentifier}, healthFacilityIdentifier={this.HealthFacilityIdentifier}]";
        }

        #endregion Methods

    }
}
