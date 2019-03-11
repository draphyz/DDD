using System.Collections.Generic;
using Conditions;
using System;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core;
    using Core.Domain;

    public abstract class PrescribedMedication
        : ValueObject, IStateObjectConvertible<PrescribedMedicationState>
    {

        #region Fields

        private readonly EntityState entityState;
        private readonly int identifier;

        #endregion Fields

        #region Constructors

        protected PrescribedMedication(string nameOrDescription,
                                       string posology = null,
                                       string quantity = null,
                                       string duration = null,
                                       MedicationCode code = null,
                                       int identifier = 0,
                                       EntityState entityState = EntityState.Added)
        {
            Condition.Requires(nameOrDescription, nameof(nameOrDescription)).IsNotNullOrWhiteSpace();
            Condition.Requires(identifier, nameof(identifier)).IsGreaterOrEqual(0);
            this.NameOrDescription = nameOrDescription;
            this.Posology = posology;
            if (!string.IsNullOrWhiteSpace(quantity))
                this.Quantity = quantity;
            if (!string.IsNullOrWhiteSpace(duration))
                this.Duration = duration;
            this.Code = code;
            this.identifier = identifier;
            this.entityState = entityState;
        }

        #endregion Constructors

        #region Properties

        public MedicationCode Code { get; }

        public string Duration { get; }

        public string NameOrDescription { get; }

        public string Posology { get; }

        public string Quantity { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.NameOrDescription;
            yield return this.Posology;
            yield return this.Quantity;
            yield return this.Duration;
            yield return this.Code;
        }

        public virtual PrescribedMedicationState ToState()
        {
            return new PrescribedMedicationState
            {
                EntityState = this.entityState,
                Identifier = this.identifier,
                NameOrDescription = this.NameOrDescription,
                Posology = this.Posology,
                Quantity = this.Quantity,
                Duration = this.Duration,
                Code = this.Code?.Code
            };
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [nameOrDescription={this.NameOrDescription}, posology={this.Posology}], quantity={this.Quantity}, duration={this.Duration}, code={this.Code}";
        }

        #endregion Methods

    }
}