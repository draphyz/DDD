using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeComplexEntity : DomainEntity
    {
        #region Constructors

        public FakeComplexEntity(FakeStringIdentityComponent component1, FakeIntIdentityComponent component2)
        {
            this.Component1 = component1;
            this.Component2 = component2;
        }

        #endregion Constructors

        #region Properties

        public FakeStringIdentityComponent Component1 { get; }

        public FakeIntIdentityComponent Component2 { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<ComparableValueObject> IdentityComponents()
        {
            yield return this.Component1;
            yield return this.Component2;
        }

        public override string ToString()
        {
            return $"FakeComplexEntity [component1={this.Component1}, component2={this.Component2}]";
        }

        #endregion Methods
    }
}