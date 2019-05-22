using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeSimpleValueObject1 : ComparableValueObject
    {
        #region Constructors

        public FakeSimpleValueObject1(int component)
        {
            this.Component = component;
        }

        #endregion Constructors

        #region Properties

        public int Component { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Component;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Component;
        }

        public override string ToString() => $"{this.GetType().Name} [component={this.Component}]";

        #endregion Methods
    }
}