using System;
using System.Collections.Generic;
using System.Linq;
using Conditions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Mapping;
    using Domain.Prescriptions;
    using Domain.Facilities;
    using Domain.Patients;
    using Domain.Practitioners;
    using Common.Domain;

    public class BelgianPharmaceuticalPrescriptionTranslator
        : IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>>
    {

        #region Methods

        public IEnumerable<PharmaceuticalPrescription> Translate(CreatePharmaceuticalPrescriptions command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var prescriber = ToPrescriber(command);
            var patient = ToPatient(command);
            var facility = ToHealthFacility(command);
            var languageCode = new Alpha2LanguageCode(command.LanguageCode);
            foreach (var prescription in command.Prescriptions)
                yield return PharmaceuticalPrescription.Create
                (
                    new PrescriptionIdentifier(prescription.PrescriptionIdentifier),
                    prescriber,
                    patient,
                    facility,
                    prescription.Medications.Select(m => ToPrescribedMedication(m)),
                    prescription.CreatedOn,
                    languageCode,
                    prescription.DelivrableAt
                );
        }

        private static HealthcareCenter ToCenter(CreatePharmaceuticalPrescriptions command)
        {
            return new HealthcareCenter
            (
                command.HealthFacilityIdentifier,
                command.HealthFacilityName
            );
        }

        private static PrescribedPharmaceuticalCompounding ToCompounding(PrescribedMedicationDescriptor medication)
        {
            return new PrescribedPharmaceuticalCompounding
            (
                medication.NameOrDescription,
                medication.Posology,
                medication.Quantity,
                medication.Duration
            );
        }

        private static HealthFacility ToHealthFacility(CreatePharmaceuticalPrescriptions command)
        {
            switch (command.HealthFacilityType)
            {
                case HealthFacilityType.Hospital:
                    return ToHospital(command);
                case HealthFacilityType.Center:
                    return ToCenter(command);
                default:
                    throw new ArgumentException($"Health facility type '{command.HealthFacilityType}' not expected.", nameof(command));
            }
        }

        private static Hospital ToHospital(CreatePharmaceuticalPrescriptions command)
        {
            return new Hospital
            (
                command.HealthFacilityIdentifier,
                command.HealthFacilityName,
                new BelgianHealthFacilityLicenseNumber(command.HealthFacilityLicenseNumber),
                command.HealthFacilityCode
            );
        }

        private static Patient ToPatient(CreatePharmaceuticalPrescriptions command)
        {
            return new Patient
            (
                command.PatientIdentifier,
                new FullName(command.PatientLastName, command.PatientFirstName),
                Enumeration.FromValue<BelgianSex>((int)command.PatientSex),
                BelgianSocialSecurityNumber.CreateIfNotEmpty(command.PatientSocialSecurityNumber),
                null,
                command.PatientBirthdate
            );
        }

        private static Physician ToPhysician(CreatePharmaceuticalPrescriptions command)
        {
            return new Physician
            (
                command.PrescriberIdentifier,
                new FullName(command.PrescriberLastName, command.PrescriberFirstName),
                new BelgianHealthcarePractitionerLicenseNumber(command.PrescriberLicenseNumber),
                BelgianSocialSecurityNumber.CreateIfNotEmpty(command.PrescriberSocialSecurityNumber),
                ToPrescriberContactInformation(command),
                command.PrescriberSpeciality,
                command.PrescriberDisplayName
            );
        }

        private static PrescribedMedication ToPrescribedMedication(PrescribedMedicationDescriptor medication)
        {
            switch (medication.MedicationType)
            {
                case PrescribedMedicationType.Product:
                    return ToProduct(medication);
                case PrescribedMedicationType.Substance:
                    return ToSubstance(medication);
                case PrescribedMedicationType.Compounding:
                    return ToCompounding(medication);
                default:
                    throw new ArgumentException($"Medication type '{medication.MedicationType}' not expected.", nameof(medication));
            }
        }

        private static PrescribedPharmaceuticalProduct ToProduct(PrescribedMedicationDescriptor medication)
        {
            return new PrescribedPharmaceuticalProduct
            (
                medication.NameOrDescription,
                medication.Posology,
                medication.Quantity,
                medication.Duration,
                BelgianMedicationCode.CreateIfNotEmpty(medication.Code)
            );
        }

        private static HealthcarePractitioner ToPrescriber(CreatePharmaceuticalPrescriptions command)
        {
            switch (command.PrescriberType)
            {
                case HealthcarePractitionerType.Physician:
                    return ToPhysician(command);
                default:
                    throw new ArgumentException($"Prescriber type '{command.PrescriberType}' not expected.", nameof(command));
            }
        }

        private static ContactInformation ToPrescriberContactInformation(CreatePharmaceuticalPrescriptions command)
        {
            return new ContactInformation
            (ToPrescriberPostalAddress(command),
                command.PrescriberPrimaryTelephoneNumber,
                command.PrescriberSecondaryTelephoneNumber,
                null,
                EmailAddress.CreateIfNotEmpty(command.PrescriberPrimaryEmailAddress),
                EmailAddress.CreateIfNotEmpty(command.PrescriberSecondaryEmailAddress),
                string.IsNullOrWhiteSpace(command.PrescriberWebSite) ? null : new Uri(command.PrescriberWebSite)
            );
        }

        private static PostalAddress ToPrescriberPostalAddress(CreatePharmaceuticalPrescriptions command)
        {
            return PostalAddress.CreateIfNotEmpty
            (
                command.PrescriberStreet,
                command.PrescriberCity,
                command.PrescriberPostalCode,
                Alpha2CountryCode.CreateIfNotEmpty(command.PrescriberCountryCode),
                command.PrescriberHouseNumber,
                command.PrescriberBoxNumber
            );
        }

        private static PrescribedPharmaceuticalSubstance ToSubstance(PrescribedMedicationDescriptor medication)
        {
            return new PrescribedPharmaceuticalSubstance
           (
                medication.NameOrDescription,
                medication.Posology,
                medication.Quantity,
                medication.Duration,
                BelgianMedicationCode.CreateIfNotEmpty(medication.Code)
           );
        }

        #endregion Methods

    }
}
