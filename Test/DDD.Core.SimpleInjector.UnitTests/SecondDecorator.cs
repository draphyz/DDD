namespace DDD.Core.Infrastructure.DependencyInjection
{
    public class SecondDecorator: IDoSomething
    {

        #region Fields

        private readonly IDoSomething decorated;

        #endregion Fields

        #region Constructors

        public SecondDecorator(IDoSomething decorated)
        {
            this.decorated = decorated;
        }

        #endregion Constructors

        #region Methods

        public void DoSomething()
        {
            this.decorated.DoSomething();
        }

        #endregion Methods

    }
}
