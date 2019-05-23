using Conditions;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    public abstract class PrescribedMedication : ValueObject
    {

        #region Fields

        private readonly int identifier;

        #endregion Fields

        #region Constructors

        protected PrescribedMedication(string nameOrDescription,
                                       string posology = null,
                                       string quantity = null,
                                       string duration = null,
                                       MedicationCode code = null,
                                       int identifier = 0)
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

        public override string ToString()
        {
            return $"{this.GetType().Name} [nameOrDescription={this.NameOrDescription}, posology={this.Posology}], quantity={this.Quantity}, duration={this.Duration}, code={this.Code}";
        }

        #endregion Methods

    }
}