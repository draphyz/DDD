using System.Threading.Tasks;
using Polly;
using Conditions;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;
    using DDD.Core.Domain;

    /// <summary>
    /// A decorator that applies a resilience policy to the asynchronous execution of a command.
    /// </summary>
    public class AsyncPollyCommandHandler<TCommand, TContext> : IAsyncCommandHandler<TCommand, TContext>
        where TCommand : class, ICommand
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IAsyncCommandHandler<TCommand, TContext> handler;
        private readonly IAsyncPolicy policy;

        #endregion Fields

        #region Constructors

        public AsyncPollyCommandHandler(IAsyncCommandHandler<TCommand, TContext> handler, IAsyncPolicy policy)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.handler.Context;

        #endregion Properties

        #region Methods

        public async Task HandleAsync(TCommand command, IMessageContext context = null)
        {
            await policy.ExecuteAsync(() => this.handler.HandleAsync(command, context));
        }

        #endregion Methods

    }
}