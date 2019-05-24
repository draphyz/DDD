using Conditions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Collections;
    using Common.Domain;
    using Core.Domain;
    using Facilities;
    using Patients;
    using Practitioners;

    /// <summary>
    /// Represents a pharmaceutical prescription.
    /// </summary>
    public class PharmaceuticalPrescription : Prescription<PharmaceuticalPrescriptionState>
    {

        #region Fields

        private readonly ISet<PrescribedMedication> prescribedMedications = new HashSet<PrescribedMedication>();

        #endregion Fields

        #region Constructors

        public PharmaceuticalPrescription(PrescriptionIdentifier identifier,
                                          HealthcarePractitioner prescriber,
                                          Patient patient,
                                          HealthFacility healthFacility,
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
            this.prescribedMedications.AddRange(prescribedMedications);
        }

        #endregion Constructors

        #region Methods

        public static PharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                        HealthcarePractitioner prescriber,
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
            prescription.AddEvent(new PharmaceuticalPrescriptionCreated(identifier.Value, createdOn));
            return prescription;
        }

        public static PharmaceuticalPrescription Create(PrescriptionIdentifier identifier,
                                                        HealthcarePractitioner prescriber,
                                                        Patient patient,
                                                        HealthFacility healthFacility,
                                                        IEnumerable<PrescribedMedication> prescribedMedications,
                                                        Alpha2LanguageCode languageCode,
                                                        DateTime? delivrableAt = null)
        {
            return Create(identifier, prescriber, patient, healthFacility, prescribedMedications, DateTime.Now, languageCode, delivrableAt);
        }

        public IEnumerable<PrescribedMedication> PrescribedMedications() => this.prescribedMedications.ToImmutableHashSet();

        public override PharmaceuticalPrescriptionState ToState()
        {
            var state = base.ToState();
            state.PrescribedMedications.AddRange(this.prescribedMedications.Select(m => ToPrescribedMedicationState(m, this.Identifier.Value)));
            return state;
        }

        protected override void AddPrescriptionRevokedEvent(string reason)
        {
            this.AddEvent(new PharmaceuticalPrescriptionRevoked(this.Identifier.Value, reason));
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
