using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Threading;

    /// <summary>
    /// A decorator that defines a scope around the asynchronous execution of a command.
    /// </summary>
    public class AsyncScopedCommandHandler<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly Container container;
        private readonly Func<IAsyncCommandHandler<TCommand>> handlerProvider;

        #endregion Fields

        #region Constructors

        public AsyncScopedCommandHandler(Func<IAsyncCommandHandler<TCommand>> handlerProvider, Container container)
        {
            Condition.Requires(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command, IMessageContext context = null)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                await new SynchronizationContextRemover();
                var handler = this.handlerProvider();
                await handler.HandleAsync(command, context);
            }
        }

        #endregion Methods

    }
}
