namespace DDD.Core.Domain
{
    public class FakeEntity1 : DomainEntity
    {

        #region Constructors

        public FakeEntity1(FakeSimpleValueObject1 component)
        {
            this.Component = component;
        }

        #endregion Constructors

        #region Properties

        public FakeSimpleValueObject1 Component { get; }

        #endregion Properties

        #region Methods

        public override ComparableValueObject Identity() => this.Component;

        public override string ToString() => $"{this.GetType().Name} [component={this.Component}]";

        #endregion Methods

    }
}