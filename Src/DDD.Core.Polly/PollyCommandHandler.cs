using Polly;
using Conditions;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;

    /// <summary>
    /// A decorator that applies a resilience policy to the synchronous execution of a command.
    /// </summary>
    public class PollyCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly ICommandHandler<TCommand> handler;
        private readonly ISyncPolicy policy;

        #endregion Fields

        #region Constructors

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public PollyCommandHandler(ICommandHandler<TCommand> handler, ISyncPolicy policy)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public void Handle(TCommand command)
        {
            policy.Execute(() => this.handler.Handle(command));
        }

        #endregion Methods

    }
}