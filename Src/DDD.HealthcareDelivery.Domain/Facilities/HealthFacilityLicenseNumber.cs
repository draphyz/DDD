using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Core.Domain;

    public abstract class HealthFacilityLicenseNumber : ComparableValueObject
    {

        #region Constructors

        protected HealthFacilityLicenseNumber(string number)
        {
            Condition.Requires(number, nameof(number)).IsNotNullOrWhiteSpace();
            this.Number = number.ToUpper();
        }

        #endregion Constructors

        #region Properties

        public string Number { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Number;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Number;
        }

        public override string ToString() => $"{this.GetType().Name} [number={this.Number}]";

        #endregion Methods

    }
}