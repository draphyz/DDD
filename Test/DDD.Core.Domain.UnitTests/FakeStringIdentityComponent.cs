using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeStringIdentityComponent : ComparableValueObject
    {
        #region Constructors

        public FakeStringIdentityComponent(string value)
        {
            this.Value = value;
        }

        #endregion Constructors

        #region Properties

        public string Value { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Value;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Value;
        }

        public override string ToString() => $"FakeStringIdentityComponent [value={this.Value}]";

        #endregion Methods
    }
}