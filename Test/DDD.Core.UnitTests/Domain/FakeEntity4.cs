namespace DDD.Core.Domain
{
    public class FakeEntity4 : DomainEntity
    {

        #region Constructors

        public FakeEntity4(FakeComplexValueObject2 component)
        {
            this.Component = component;
        }

        #endregion Constructors

        #region Properties

        public FakeComplexValueObject2 Component { get; }

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