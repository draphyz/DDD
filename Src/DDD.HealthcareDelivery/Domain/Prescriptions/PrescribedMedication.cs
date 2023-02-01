﻿using EnsureThat;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    public abstract class PrescribedMedication : ValueObject
    {

        #region Fields

        private int identifier;

        #endregion Fields

        #region Constructors

        protected PrescribedMedication() { }

        protected PrescribedMedication(string nameOrDescription,
                                       string posology = null,
                                       byte? quantity = null,
                                       MedicationCode code = null,
                                       int identifier = 0)
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
        }

        #endregion Constructors

        #region Properties

        public MedicationCode Code { get; private set; }

        public string NameOrDescription { get; private set; }

        public string Posology { get; private set; }

        public byte? Quantity { get; private set; }

        #endregion Properties

        #region Methods

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.NameOrDescription;
            yield return this.Posology;
            yield return this.Quantity;
            yield return this.Code;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [nameOrDescription={this.NameOrDescription}, posology={this.Posology}, quantity={this.Quantity}, code={this.Code}]";
        }

        #endregion Methods

    }
}