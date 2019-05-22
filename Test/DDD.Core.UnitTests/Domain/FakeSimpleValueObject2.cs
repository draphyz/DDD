using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeSimpleValueObject2 : ComparableValueObject
    {

        #region Constructors

        public FakeSimpleValueObject2(string component)
        {
            this.Component = component;
        }

        #endregion Constructors

        #region Properties

        public string Component { get; }

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