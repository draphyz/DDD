using System.Collections.Generic;

namespace DDD.Core.Domain
{
    public class FakeSimpleEntity : DomainEntity
    {
        #region Constructors

        public FakeSimpleEntity(FakeIntIdentityComponent component1)
        {
            this.Component1 = component1;
        }

        #endregion Constructors

        #region Properties

        public FakeIntIdentityComponent Component1 { get; }

        #endregion Properties

        #region Methods

        public override IEnumerable<ComparableValueObject> IdentityComponents()
        {
            yield return this.Component1;
        }

        public override string ToString() => $"FakeSimpleEntity [component1={this.Component1}]";

        #endregion Methods
    }
}