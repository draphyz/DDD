using System;
using System.Collections.Generic;
using System.Linq;
using Conditions;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Core.Mapping;
    using Domain.Prescriptions;
    using Domain.Facilities;
    using Domain.Patients;
    using Domain.Providers;
    using Common.Domain;

    public class BelgianPharmaceuticalPrescriptionTranslator
        : IObjectTranslator<CreatePharmaceuticalPrescriptions, IEnumerable<PharmaceuticalPrescription>>
    {

        #region Methods

        public IEnumerable<PharmaceuticalPrescription> Translate(CreatePharmaceuticalPrescriptions command)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            var provider = ToProvider(command);
            var patient = ToPatient(command);
            var facility = ToHealthFacility(command);
            var languageCode = new Alpha2LanguageCode(command.LanguageCode);
            foreach (var prescription in command.Prescriptions)
                yield return PharmaceuticalPrescription.Create
                (
                    new PrescriptionIdentifier(prescription.PrescriptionIdentifier),
                    provider,
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
                string.IsNullOrWhiteSpace(command.PatientSocialSecurityNumber) ? null : new BelgianSocialSecurityNumber(command.PatientSocialSecurityNumber),
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
                new BelgianPractitionerLicenseNumber(command.PrescriberLicenseNumber),
                string.IsNullOrWhiteSpace(command.PrescriberSocialSecurityNumber) ? null : new BelgianSocialSecurityNumber(command.PrescriberSocialSecurityNumber),
                ToProviderContactInformation(command),
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
                string.IsNullOrWhiteSpace(medication.Code) ? null : new BelgianMedicationCode(medication.Code)
            );
        }

        private static HealthcareProvider ToProvider(CreatePharmaceuticalPrescriptions command)
        {
            switch (command.PrescriberType)
            {
                case HealthcareProviderType.Physician:
                    return ToPhysician(command);
                default:
                    throw new ArgumentException($"Prescriber type '{command.PrescriberType}' not expected.", nameof(command));
            }
        }

        private static ContactInformation ToProviderContactInformation(CreatePharmaceuticalPrescriptions command)
        {
            return new ContactInformation
            (ToProviderPostalAddress(command),
                command.PrescriberPrimaryTelephoneNumber,
                command.PrescriberSecondaryTelephoneNumber,
                null,
                string.IsNullOrWhiteSpace(command.PrescriberPrimaryEmailAddress) ? null : new EmailAddress(command.PrescriberPrimaryEmailAddress),
                string.IsNullOrWhiteSpace(command.PrescriberSecondaryEmailAddress) ? null : new EmailAddress(command.PrescriberSecondaryEmailAddress),
                string.IsNullOrWhiteSpace(command.PrescriberWebSite) ? null : new Uri(command.PrescriberWebSite)
            );
        }

        private static PostalAddress ToProviderPostalAddress(CreatePharmaceuticalPrescriptions command)
        {
            if (string.IsNullOrWhiteSpace(command.PrescriberStreet) || string.IsNullOrWhiteSpace(command.PrescriberCity)) return null;
            return new PostalAddress
            (
                command.PrescriberStreet,
                command.PrescriberCity,
                command.PrescriberPostalCode,
                string.IsNullOrWhiteSpace(command.PrescriberCountryCode) ? null : new Alpha2CountryCode(command.PrescriberCountryCode),
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
                string.IsNullOrWhiteSpace(medication.Code) ? null : new BelgianMedicationCode(medication.Code)
           );
        }

        #endregion Methods

    }
}
