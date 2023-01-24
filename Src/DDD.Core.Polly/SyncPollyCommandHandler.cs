using Polly;
using Conditions;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;

    /// <summary>
    /// A decorator that applies a resilience policy to the synchronous execution of a command.
    /// </summary>
    public class SyncPollyCommandHandler<TCommand> : ISyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly ISyncCommandHandler<TCommand> handler;
        private readonly ISyncPolicy policy;

        #endregion Fields

        #region Constructors

        public SyncPollyCommandHandler(ISyncCommandHandler<TCommand> handler, ISyncPolicy policy)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public void Handle(TCommand command, IMessageContext context = null)
        {
            policy.Execute(() => this.handler.Handle(command, context));
        }

        #endregion Methods

    }
}