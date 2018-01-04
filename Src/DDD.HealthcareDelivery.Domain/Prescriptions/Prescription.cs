using System;
using System.Collections.Generic;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;
    using Common.Domain;
    using Patients;
    using Facilities;
    using Providers;

    /// <summary>
    /// Represents a medical prescription.
    /// </summary>
    public abstract class Prescription<TState>
        : DomainEntity, IStateObjectConvertible<TState>
        where TState : PrescriptionState, new()
    {

        #region Constructors

        protected Prescription(PrescriptionIdentifier identifier,
                               HealthcareProvider prescriber,
                               Patient patient,
                               HealthFacility healthFacility,
                               Alpha2LanguageCode languageCode,
                               PrescriptionStatus status,
                               DateTime createdOn,
                               DateTime? delivrableAt = null,
                               EntityState entityState = EntityState.Added,
                               IEnumerable<IDomainEvent> events = null)
            : base(entityState, events)
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

        protected DateTime CreatedOn { get; }

        protected DateTime? DelivrableAt { get; }

        protected HealthFacility HealthFacility { get; }

        protected PrescriptionIdentifier Identifier { get; }

        protected Alpha2LanguageCode LanguageCode { get; }
        protected Patient Patient { get; }

        protected HealthcareProvider Prescriber { get; }

        protected PrescriptionStatus Status { get; set; }

        #endregion Properties

        #region Methods

        public override IEnumerable<ComparableValueObject> IdentityComponents()
        {
            yield return this.Identifier;
        }

        public void Revoke(string reason)
        {
            Condition.Requires(reason, nameof(reason)).IsNotNullOrWhiteSpace();
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
                Identifier = this.Identifier.Identifier,
                Prescriber = this.Prescriber.ToState(),
                Patient = this.Patient.ToState(),
                HealthFacility = this.HealthFacility.ToState(),
                Status = this.Status.Code,
                IsElectronic = false,
                CreatedOn = this.CreatedOn,
                DelivrableAt = this.DelivrableAt,
                LanguageCode = this.LanguageCode.Code,
                EntityState = this.EntityState,
            };
        }

        protected abstract void AddPrescriptionRevokedEvent(string reason);

        protected bool IsRevocable() => this.Status == PrescriptionStatus.Created;

        #endregion Methods

    }
}