using System.Collections.Generic;
using EnsureThat;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
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
                                       byte? quantity = null,
                                       MedicationCode code = null,
                                       int identifier = 0,
                                       EntityState entityState = EntityState.Added)
        {
            Ensure.That(nameOrDescription, nameof(nameOrDescription)).IsNotNullOrWhiteSpace();
            Ensure.That(identifier, nameof(identifier)).IsGte(0);
            this.NameOrDescription = nameOrDescription;
            this.Posology = posology;
            if (quantity.HasValue)
            {
                Ensure.That(quantity.Value, nameof(quantity)).IsGte((byte)1);
                this.Quantity = quantity;
            }
            this.Code = code;
            this.identifier = identifier;
            this.entityState = entityState;
        }

        #endregion Constructors

        #region Properties

        public MedicationCode Code { get; }

        public string NameOrDescription { get; }

        public string Posology { get; }

        public byte? Quantity { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.NameOrDescription;
            yield return this.Posology;
            yield return this.Quantity;
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
                Code = this.Code?.Value
            };
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [nameOrDescription={this.NameOrDescription}, posology={this.Posology}, quantity={this.Quantity}, code={this.Code}]";
        }

        #endregion Methods

    }
}