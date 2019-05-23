using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Common.Domain;
    using Core.Domain;
    using Facilities;
    using Patients;
    using Practitioners;

    /// <summary>
    /// Represents a health-care program implemented by a qualified healthcare practitioner (physician, dentist, ...) in the form of instructions that govern the plan of care for an individual patient.
    /// </summary>
    public abstract class Prescription : DomainEntity
    {

        #region Constructors

        protected Prescription(PrescriptionIdentifier identifier,
                               HealthcarePractitioner prescriber,
                               Patient patient,
                               HealthFacility healthFacility,
                               Alpha2LanguageCode languageCode,
                               PrescriptionStatus status,
                               DateTime createdOn,
                               DateTime? delivrableAt = null,
                               IEnumerable<IDomainEvent> events = null)
            : base(events)
        {
            Condition.Requires(identifier, nameof(identifier)).IsNotNull();
            Condition.Requires(prescriber, nameof(prescriber)).IsNotNull();
            Condition.Requires(patient, nameof(patient)).IsNotNull();
            Condition.Requires(healthFacility, nameof(healthFacility)).IsNotNull();
            Condition.Requires(status, nameof(status)).IsNotNull();
            Condition.Requires(languageCode, nameof(languageCode)).IsNotNull();
            this.Identifier = identifier;
            this.Prescriber = prescriber;
            this.Patient = patient;
            this.HealthFacility = healthFacility;
            this.Status = status;
            this.CreatedOn = createdOn;
            this.DelivrableAt = delivrableAt;
            this.LanguageCode = languageCode;
        }

        #endregion Constructors

        #region Properties

        public DateTime CreatedOn { get; }

        public DateTime? DelivrableAt { get; }

        public HealthFacility HealthFacility { get; }

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
            Condition.Requires(reason, nameof(reason)).IsNotNullOrWhiteSpace();
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