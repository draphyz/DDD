using System;
using System.Collections.Generic;
using EnsureThat;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;
    using Common.Domain;
    using Patients;
    using Practitioners;
    using Encounters;

    /// <summary>
    /// Represents a health-care program implemented by a qualified healthcare practitioner (physician, dentist, ...) in the form of instructions that govern the plan of care for an individual patient.
    /// </summary>
    public abstract class Prescription<TState>
        : DomainEntity, IStateObjectConvertible<TState>
        where TState : PrescriptionState, new()
    {

        #region Constructors

        protected Prescription(PrescriptionIdentifier identifier,
                               HealthcarePractitioner prescriber,
                               Patient patient,
                               Alpha2LanguageCode languageCode,
                               PrescriptionStatus status,
                               DateTime createdOn,
                               EncounterIdentifier encounterIdentifier = null,
                               DateTime? delivrableAt = null,
                               EntityState entityState = EntityState.Added,
                               IEnumerable<IDomainEvent> events = null)
            : base(entityState, events)
        {
            Ensure.That(identifier, nameof(identifier)).IsNotNull();
            Ensure.That(prescriber, nameof(prescriber)).IsNotNull();
            Ensure.That(patient, nameof(patient)).IsNotNull();
            Ensure.That(status, nameof(status)).IsNotNull();
            Ensure.That(languageCode, nameof(languageCode)).IsNotNull();
            this.Identifier = identifier;
            this.Prescriber = prescriber;
            this.Patient = patient;
            this.LanguageCode = languageCode;
            this.Status = status;
            this.CreatedOn = createdOn;
            this.EncounterIdentifier = encounterIdentifier;
            this.DeliverableAt = delivrableAt;
        }

        #endregion Constructors

        #region Properties

        public DateTime CreatedOn { get; }

        public DateTime? DeliverableAt { get; }

        public EncounterIdentifier EncounterIdentifier { get; private set; }

        public PrescriptionIdentifier Identifier { get; }

        public Alpha2LanguageCode LanguageCode { get; }

        public Patient Patient { get; }

        public HealthcarePractitioner Prescriber { get; }

        public PrescriptionStatus Status { get; private set; }

        #endregion Properties

        #region Methods

        public override ComparableValueObject Identity() => this.Identifier;

        public void Revoke(string reason)
        {
            Ensure.That(reason, nameof(reason)).IsNotNullOrWhiteSpace();
            if (this.IsRevocable())
            {
                this.Status = PrescriptionStatus.Revoked;
                this.MarkAsModified();
                this.AddPrescriptionRevokedEvent(reason);
            }
        }
        public virtual TState ToState()
        {
            return new TState
            {
                Identifier = this.Identifier.Value,
                Prescriber = this.Prescriber.ToState(),
                Patient = this.Patient.ToState(),
                EncounterIdentifier = this.EncounterIdentifier?.Value,
                Status = this.Status.Code,
                CreatedOn = this.CreatedOn,
                DeliverableAt = this.DeliverableAt,
                LanguageCode = this.LanguageCode.Value,
                EntityState = this.EntityState,
            };
        }

        protected abstract void AddPrescriptionRevokedEvent(string reason);

        protected bool IsRevocable() => this.Status == PrescriptionStatus.Created;

        #endregion Methods

    }
}