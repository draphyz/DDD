using System.Threading;
using System.Threading.Tasks;
using Polly;
using Conditions;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;

    /// <summary>
    /// A decorator that applies a resilience policy to the asynchronous execution of a command.
    /// </summary>
    public class AsyncPollyCommandHandler<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly IAsyncCommandHandler<TCommand> handler;
        private readonly IAsyncPolicy policy;

        #endregion Fields

        #region Constructors

        public AsyncPollyCommandHandler(IAsyncCommandHandler<TCommand> handler, IAsyncPolicy policy)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            await policy.ExecuteAsync(() => this.handler.HandleAsync(command, cancellationToken));
        }

        #endregion Methods

    }
}