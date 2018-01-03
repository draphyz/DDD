using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core.Domain;

    public abstract class MedicationCode : ComparableValueObject
    {

        #region Constructors

        protected MedicationCode(string code)
        {
            Condition.Requires(code, nameof(code)).IsNotNullOrWhiteSpace();
            this.Code = code.ToUpper();
        }

        #endregion Constructors

        #region Properties

        public string Code { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Code;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Code;
        }

        public override string ToString() => $"{this.GetType().Name} [code={this.Code}]";

        #endregion Methods

    }
}
