using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    public class FirstDelegatingDecorator : IDoSomething
    {
        #region Fields

        private readonly Func<IDoSomething> decoratedFactory;

        #endregion Fields

        #region Constructors

        public FirstDelegatingDecorator(Func<IDoSomething> decoratedFactory) 
        {
            this.decoratedFactory = decoratedFactory;
        }

        #endregion Constructors

        #region Methods

        public void DoSomething()
        {
            this.decoratedFactory().DoSomething();
        }

        #endregion Methods

    }
}
