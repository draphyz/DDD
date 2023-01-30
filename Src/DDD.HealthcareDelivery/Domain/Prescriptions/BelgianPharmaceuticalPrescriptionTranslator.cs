﻿using System.Linq;
using EnsureThat;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Mapping;
    using Common.Domain;
    using Patients;
    using Practitioners;
    using Encounters;

    public class BelgianPharmaceuticalPrescriptionTranslator
        : ObjectTranslator<PharmaceuticalPrescriptionState, PharmaceuticalPrescription>
    {

        #region Fields

        private readonly IObjectTranslator<PrescribedMedicationState, PrescribedMedication> medicationTranslator;
        private readonly IObjectTranslator<PatientState, Patient> patientTranslator;
        private readonly IObjectTranslator<HealthcarePractitionerState, HealthcarePractitioner> practitionerTranslator;

        #endregion Fields

        #region Constructors

        public BelgianPharmaceuticalPrescriptionTranslator()
        {
            this.practitionerTranslator = new BelgianHealthcarePractitionerTranslator();
            this.patientTranslator = new BelgianPatientTranslator();
            this.medicationTranslator = new BelgianPrescribedMedicationTranslator();
        }

        #endregion Constructors

        #region Methods

        public override PharmaceuticalPrescription Translate(PharmaceuticalPrescriptionState state,
                                                             IDictionary<string, object> context = null)
        {
            Ensure.That(state, nameof(state)).IsNotNull();
            return new PharmaceuticalPrescription
            (
                new PrescriptionIdentifier(state.Identifier),
                this.practitionerTranslator.Translate(state.Prescriber),
                this.patientTranslator.Translate(state.Patient),
                state.PrescribedMedications.Select(m => this.medicationTranslator.Translate(m)),
                new Alpha2LanguageCode(state.LanguageCode),
                Enumeration.ParseCode<PrescriptionStatus>(state.Status),
                state.CreatedOn,
                EncounterIdentifier.CreateIfNotEmpty(state.EncounterIdentifier),
                state.DeliverableAt,
                state.EntityState
            );

        }

        #endregion Methods

    }
}
