namespace DDD.Core.Domain
{
    public class FakeEntity2 : DomainEntity
    {

        #region Constructors

        public FakeEntity2(FakeComplexValueObject1 component)
        {
            this.Component = component;
        }

        #endregion Constructors

        #region Properties

        public FakeComplexValueObject1 Component { get; }

        #endregion Properties

        #region Methods

        public override ComparableValueObject Identity() => this.Component;

        public override string ToString()
        {
            return $"{this.GetType().Name} [component={this.Component}]";
        }

        #endregion Methods

    }
}