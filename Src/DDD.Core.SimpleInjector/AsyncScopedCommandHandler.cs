using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the asynchronous execution of a command.
    /// </summary>
    public class AsyncScopedCommandHandler<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly Container container;
        private readonly IAsyncCommandHandler<TCommand> handler;

        #endregion Fields

        #region Constructors

        public AsyncScopedCommandHandler(IAsyncCommandHandler<TCommand> handler, Container container)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handler = handler;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                await this.handler.HandleAsync(command);
            }
        }

        #endregion Methods

    }
}
