using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    public class PrescriptionIdentifier : ComparableValueObject
    {

        #region Constructors

        public PrescriptionIdentifier(int identifier)
        {
            Condition.Requires(identifier, nameof(identifier)).IsGreaterThan(0);
            this.Identifier = identifier;
        }

        #endregion Constructors

        #region Properties

        public int Identifier { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Identifier;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Identifier;
        }

        public override string ToString() => $"{this.GetType().Name} [identifier={this.Identifier}]";

        #endregion Methods

    }
}