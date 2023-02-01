using System.Threading.Tasks;
using Polly;
using EnsureThat;

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
            Ensure.That(handler, nameof(handler)).IsNotNull();
            Ensure.That(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command, IMessageContext context = null)
        {
            await policy.ExecuteAsync(() => this.handler.HandleAsync(command, context));
        }

        #endregion Methods

    }
}