using System.Linq;
using Conditions;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Mapping;
    using Common.Domain;
    using Facilities;
    using Patients;
    using Practitioners;

    public class BelgianPharmaceuticalPrescriptionTranslator
        : IObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription>
    {

        #region Fields

        private readonly IObjectTranslator<HealthFacilityState, HealthFacility> facilityTranslator;
        private readonly IObjectTranslator<PrescribedMedicationState, PrescribedMedication> medicationTranslator;
        private readonly IObjectTranslator<PatientState, Patient> patientTranslator;
        private readonly IObjectTranslator<HealthcarePractitionerState, HealthcarePractitioner> practitionerTranslator;

        #endregion Fields

        #region Constructors

        public BelgianPharmaceuticalPrescriptionTranslator()
        {
            this.practitionerTranslator = new BelgianHealthcarePractitionerTranslator();
            this.patientTranslator = new BelgianPatientTranslator();
            this.facilityTranslator = new BelgianHealthFacilityTranslator();
            this.medicationTranslator = new BelgianPrescribedMedicationTranslator();
        }

        #endregion Constructors

        #region Methods

        public PharmaceuticalPrescription Translate(PharmaceuticalPrescriptionState state,
                                                    IDictionary<string, object> options = null)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            return new PharmaceuticalPrescription
            (
                new PrescriptionIdentifier(state.Identifier),
                this.practitionerTranslator.Translate(state.Prescriber),
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
