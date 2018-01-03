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
            if (string.IsNullOrWhiteSpace(posology) && string.IsNullOrWhiteSpace(duration))
                throw new ArgumentException("A posology or a duration must be specified.");
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
                QuantityAsByte = this.QuantityAsByte(),
                Duration = this.Duration,
                Code = this.Code?.Code
            };
        }

        public virtual byte? QuantityAsByte()
        {
            if (this.Quantity == null) return null;
            string digits = string.Empty;
            for (var i = 0; i < this.Quantity.Length; i++)
            {
                var c = this.Quantity[i];
                if (c.IsDigit())
                    digits += c;
                else if (c.IsLetter())
                    break;
            }
            if (digits == string.Empty) return 1;
            return byte.Parse(digits);
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [nameOrDescription={this.NameOrDescription}, posology={this.Posology}], quantity={this.Quantity}, duration={this.Duration}, code={this.Code}";
        }

        #endregion Methods

    }
}