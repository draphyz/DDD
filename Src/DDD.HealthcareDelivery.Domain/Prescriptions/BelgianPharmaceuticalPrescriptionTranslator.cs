using System.Linq;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Mapping;
    using Common.Domain;
    using Facilities;
    using Patients;
    using Providers;

    public class BelgianPharmaceuticalPrescriptionTranslator
        : IObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription>
    {

        #region Fields

        private readonly IObjectTranslator<HealthFacilityState, HealthFacility> facilityTranslator;
        private readonly IObjectTranslator<PrescribedMedicationState, PrescribedMedication> medicationTranslator;
        private readonly IObjectTranslator<PatientState, Patient> patientTranslator;
        private readonly IObjectTranslator<HealthcareProviderState, HealthcareProvider> providerTranslator;

        #endregion Fields

        #region Constructors

        public BelgianPharmaceuticalPrescriptionTranslator()
        {
            this.providerTranslator = new BelgianHealthcareProviderTranslator();
            this.patientTranslator = new BelgianPatientTranslator();
            this.facilityTranslator = new BelgianHealthFacilityTranslator();
            this.medicationTranslator = new BelgianPrescribedMedicationTranslator();
        }

        #endregion Constructors

        #region Methods

        public PharmaceuticalPrescription Translate(PharmaceuticalPrescriptionState state)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            if (state.IsElectronic)
                return CreateElectronicPrescription(state);
            return CreateHandwrittenPrescription(state);

        }

        private ElectronicPharmaceuticalPrescription CreateElectronicPrescription(PharmaceuticalPrescriptionState state)
        {
            return new ElectronicPharmaceuticalPrescription
            (
                new PrescriptionIdentifier(state.Identifier),
                this.providerTranslator.Translate(state.Prescriber),
                this.patientTranslator.Translate(state.Patient),
                this.facilityTranslator.Translate(state.HealthFacility),
                state.PrescribedMedications.Select(m => this.medicationTranslator.Translate(m)),
                new Alpha2LanguageCode(state.LanguageCode),
                Enumeration.FromCode<PrescriptionStatus>(state.Status),
                state.CreatedOn,
                state.DelivrableAt,
                string.IsNullOrWhiteSpace(state.ElectronicNumber) ? null : new BelgianElectronicPrescriptionNumber(state.ElectronicNumber),
                state.EntityState
            );
        }

        private PharmaceuticalPrescription CreateHandwrittenPrescription(PharmaceuticalPrescriptionState state)
        {
            return new PharmaceuticalPrescription
            (
                new PrescriptionIdentifier(state.Identifier),
                this.providerTranslator.Translate(state.Prescriber),
                this.patientTranslator.Translate(state.Patient),
                this.facilityTranslator.Translate(state.HealthFacility),
                state.PrescribedMedications.Select(m => this.medicationTranslator.Translate(m)),
                new Alpha2LanguageCode(state.LanguageCode),
                Enumeration.FromCode<PrescriptionStatus>(state.Status),
                state.CreatedOn,
                state.DelivrableAt,
                state.EntityState
            );
        }

        #endregion Methods

    }
}
