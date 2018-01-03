using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeIntIdentityComponent : ComparableValueObject
    {
        #region Constructors

        public FakeIntIdentityComponent(int value)
        {
            this.Value = value;
        }

        #endregion Constructors

        #region Properties

        public int Value { get; }

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

        public override string ToString() => $"FakeIntIdentityComponent [value={this.Value}]";

        #endregion Methods
    }
}