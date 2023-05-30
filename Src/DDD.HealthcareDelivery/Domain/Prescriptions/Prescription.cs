using EnsureThat;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;
    using Core.Domain;
    using Patients;
    using Practitioners;
    using Encounters;

    /// <summary>
    /// Represents a health-care program implemented by a qualified healthcare practitioner (physician, dentist, ...) in the form of instructions that govern the plan of care for an individual patient.
    /// </summary>
    public abstract class Prescription : DomainEntity
    {

        #region Constructors

        protected Prescription() { }

        protected Prescription(PrescriptionIdentifier identifier,
                               HealthcarePractitioner prescriber,
                               Patient patient,
                               Alpha2LanguageCode languageCode,
                               PrescriptionStatus status,
                               DateTime createdOn,
                               EncounterIdentifier encounterIdentifier = null,
                               DateTime? delivrableAt = null,
                               IEnumerable<IDomainEvent> events = null)
            : base(events)
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

        public DateTime CreatedOn { get; private set; }

        public DateTime? DeliverableAt { get; private set; }

        public EncounterIdentifier EncounterIdentifier { get; private set; }

        public PrescriptionIdentifier Identifier { get; private set; }

        public Alpha2LanguageCode LanguageCode { get; private set; }

        public Patient Patient { get; private set; }

        public HealthcarePractitioner Prescriber { get; private set; }

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
                this.AddPrescriptionRevokedEvent(reason);
            }
        }

        protected abstract void AddPrescriptionRevokedEvent(string reason);

        protected bool IsRevocable() => this.Status == PrescriptionStatus.Created;

        #endregion Methods

    }
}