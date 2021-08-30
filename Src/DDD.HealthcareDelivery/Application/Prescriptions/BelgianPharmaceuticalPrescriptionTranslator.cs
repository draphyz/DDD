using Conditions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.HealthcareDelivery.Application.Prescriptions
{
    using Common.Domain;
    using Practitioners;
    using Domain.Patients;
    using Domain.Practitioners;
    using Domain.Prescriptions;
    using Domain.Encounters;
    using Mapping;

    public class BelgianPharmaceuticalPrescriptionTranslator
        : IObjectTranslator<CreatePharmaceuticalPrescription, PharmaceuticalPrescription>
    {

        #region Methods

        public PharmaceuticalPrescription Translate(CreatePharmaceuticalPrescription command,
                                                    IDictionary<string, object> options = null)
        {
            Condition.Requires(command, nameof(command)).IsNotNull();
            return PharmaceuticalPrescription.Create
                (
                    new PrescriptionIdentifier(command.PrescriptionIdentifier),
                    ToPrescriber(command),
                    ToPatient(command),
                    command.Medications.Select(m => ToPrescribedMedication(m)),
                    command.CreatedOn,
                    new Alpha2LanguageCode(command.LanguageCode),
                    EncounterIdentifier.CreateIfNotEmpty(command.EncounterIdentifier),
                    command.DeliverableAt
                );
        }

        private static PrescribedPharmaceuticalCompounding ToCompounding(PrescribedMedicationDescriptor medication)
        {
            return new PrescribedPharmaceuticalCompounding
            (
                medication.NameOrDescription,
                medication.Posology,
                medication.Quantity
            );
        }

        private static Patient ToPatient(CreatePharmaceuticalPrescription command)
        {
            return new Patient
            (
                command.PatientIdentifier,
                new FullName(command.PatientLastName, command.PatientFirstName),
                Enumeration.ParseValue<BelgianSex>((int)command.PatientSex),
                BelgianSocialSecurityNumber.CreateIfNotEmpty(command.PatientSocialSecurityNumber),
                null,
                command.PatientBirthdate
            );
        }

        private static Physician ToPhysician(CreatePharmaceuticalPrescription command)
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

        private static HealthcarePractitioner ToPrescriber(CreatePharmaceuticalPrescription command)
        {
            switch (command.PrescriberType)
            {
                case HealthcarePractitionerType.Physician:
                    return ToPhysician(command);
                default:
                    throw new ArgumentException($"Prescriber type '{command.PrescriberType}' not expected.", nameof(command));
            }
        }

        private static ContactInformation ToPrescriberContactInformation(CreatePharmaceuticalPrescription command)
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

        private static PostalAddress ToPrescriberPostalAddress(CreatePharmaceuticalPrescription command)
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

        private static PrescribedPharmaceuticalProduct ToProduct(PrescribedMedicationDescriptor medication)
        {
            return new PrescribedPharmaceuticalProduct
            (
                medication.NameOrDescription,
                medication.Posology,
                medication.Quantity,
                BelgianMedicationCode.CreateIfNotEmpty(medication.Code)
            );
        }
        private static PrescribedPharmaceuticalSubstance ToSubstance(PrescribedMedicationDescriptor medication)
        {
            return new PrescribedPharmaceuticalSubstance
           (
                medication.NameOrDescription,
                medication.Posology,
                medication.Quantity,
                BelgianMedicationCode.CreateIfNotEmpty(medication.Code)
           );
        }

        #endregion Methods

    }
}