using System.Linq;
using System;
using System.Collections.Generic;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Collections;
    using Core.Domain;
    using Common.Domain;
    using Patients;
    using Facilities;
    using Providers;

    /// <summary>
    /// Represents a pharmaceutical prescription.
    /// </summary>
    public class PharmaceuticalPrescription : Prescription<PharmaceuticalPrescriptionState>
    {

        #region Constructors

        public PharmaceuticalPrescription(PrescriptionIdentifier identifier, 
                                          HealthcareProvider prescriber, 
                                          Patient patient, 
                                          HealthFacility healthFacility , 
                                          IEnumerable<PrescribedMedication> prescribedMedications,
                                          Alpha2LanguageCode languageCode,
                                          PrescriptionStatus status,
                                          DateTime createdOn,
                                          DateTime? delivrableAt = null,
                                          EntityState entityState = EntityState.Added, 
                                          IEnumerable<IDomainEvent> events = null) 
            : base(identifier, prescriber, patient, healthFacility, languageCode, status, createdOn, delivrableAt, entityState, events)
        {
            Condition.Requires(prescribedMedications, nameof(prescribedMedications))
                     .IsNotNull()
                     .IsNotEmpty()
                     .DoesNotContain(null);
            this.PrescribedMedications.AddRange(prescribedMedications);
        }

        #endregion Constructors

        #region Properties

        protected ISet<PrescribedMedication> PrescribedMedications { get; } = new HashSet<PrescribedMedication>();

        #endregion Properties

        #region Methods

        public static PharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                        HealthcareProvider prescriber,
                                                        Patient patient,
                                                        HealthFacility healthFacility,
                                                        IEnumerable<PrescribedMedication> prescribedMedications,
                                                        DateTime createdOn,
                                                        Alpha2LanguageCode languageCode,
                                                        DateTime? delivrableAt = null)
        {
            var prescription = new PharmaceuticalPrescription
            (
                identifier,
                prescriber,
                patient,
                healthFacility,
                prescribedMedications,
                languageCode,
                PrescriptionStatus.Created,
                createdOn,
                delivrableAt
            );
            prescription.AddEvent(new PharmaceuticalPrescriptionCreated(identifier.Identifier, createdOn));
            return prescription;
        }

        public static PharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                        HealthcareProvider prescriber,
                                                        Patient patient,
                                                        HealthFacility healthFacility,
                                                        IEnumerable<PrescribedMedication> prescribedMedications,
                                                        Alpha2LanguageCode languageCode,
                                                        DateTime? delivrableAt = null)
        {
            return Create(identifier, prescriber, patient, healthFacility, prescribedMedications, DateTime.Now, languageCode, delivrableAt);
        }

        public override PharmaceuticalPrescriptionState ToState()
        {
            var state = base.ToState();
            state.PrescribedMedications.AddRange(this.PrescribedMedications.Select(m => ToPrescribedMedicationState(m, this.Identifier.Identifier)));
            return state;
        }

        protected override void AddPrescriptionRevokedEvent(string reason)
        {
            this.AddEvent(new PharmaceuticalPrescriptionRevoked(this.Identifier.Identifier, reason));
        }

        private static PrescribedMedicationState ToPrescribedMedicationState(PrescribedMedication medication,
                                                                             int prescriptionIdentifier)
        {
            var state = medication.ToState();
            state.PrescriptionIdentifier = prescriptionIdentifier;
            return state;
        }

        #endregion Methods
    }
}
