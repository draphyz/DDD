using System;
using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeComplexValueObject2 : ComparableValueObject
    {
        #region Constructors

        public FakeComplexValueObject2(string component1, int component2, FakeComplexValueObject1 component3)
        {
            this.Component1 = component1;
            this.Component2 = component2;
            this.Component3 = component3;
        }

        #endregion Constructors

        #region Properties

        public string Component1 { get; }

        public int Component2 { get; }

        public FakeComplexValueObject1 Component3 { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<IComparable> ComparableComponents()
        {
            yield return this.Component3;
        }

        public override IEnumerable<object> EqualityComponents()
        {
            yield return this.Component1;
            yield return this.Component2;
            yield return this.Component3;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} [component1={this.Component1}, component2={this.Component2}, component3={this.Component3}]";
        }

        #endregion Methods
    }
}