using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeSimpleValueObject : ComparableValueObject
    {
        #region Constructors

        public FakeSimpleValueObject(string component1, int component2)
        {
            this.Component1 = component1;
            this.Component2 = component2;
        }

        #endregion Constructors

        #region Properties

        public string Component1 { get; }
        public int Component2 { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Component1;
            yield return this.Component2;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Component1;
            yield return this.Component2;
        }

        public override string ToString()
        {
            return $"SimpleMockValueObject [component1={this.Component1}, component2={this.Component2}]";
        }

        #endregion Methods
    }
}