using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    public class SecondDelegatingDecorator : IDoSomething
    {
        #region Fields

        private readonly Func<IDoSomething> decoratedFactory;

        #endregion Fields

        #region Constructors

        public SecondDelegatingDecorator(Func<IDoSomething> decoratedFactory) 
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
