using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Common.Application;
    using Core.Application;
    using Practitioners;

    /// <summary>
    /// Encapsulates all information needed to create a pharmaceutical prescription.
    /// </summary>
    public class CreatePharmaceuticalPrescription : ICommand
    {

        #region Properties

        public DateTime CreatedOn { get; set; }

        public DateTime? DeliverableAt { get; set; }

        public int? EncounterIdentifier { get; set; }

        public string LanguageCode { get; set; }

        public ICollection<PrescribedMedicationDescriptor> Medications { get; set; }
            = new List<PrescribedMedicationDescriptor>();

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

        public int PrescriptionIdentifier { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            var format = $"{0} [prescriptionIdentifier={1}, prescriberIdentifier={2}, patientIdentifier={3}]";
            return string.Format(format, this.GetType().Name, this.PrescriptionIdentifier, this.PrescriberIdentifier, this.PatientIdentifier);
        }

        #endregion Methods

    }
}