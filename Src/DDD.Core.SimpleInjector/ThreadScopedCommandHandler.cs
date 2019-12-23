using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the synchronous execution of a command.
    /// </summary>
    public class ThreadScopedCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly Container container;
        private readonly ICommandHandler<TCommand> handler;

        #endregion Fields

        #region Constructors

        public ThreadScopedCommandHandler(ICommandHandler<TCommand> handler, Container container)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handler = handler;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public void Handle(TCommand command)
        {
            using (ThreadScopedLifestyle.BeginScope(container))
            {
                this.handler.Handle(command);
            }
        }

        #endregion Methods

    }
}
