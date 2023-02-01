using Polly;
using EnsureThat;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;
    using DDD.Core.Domain;

    /// <summary>
    /// A decorator that applies a resilience policy to the asynchronous execution of a query.
    /// </summary>
    public class AsyncPollyQueryHandler<TQuery, TResult, TContext> : IAsyncQueryHandler<TQuery, TResult, TContext>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IAsyncQueryHandler<TQuery, TResult, TContext> handler;
        private readonly IAsyncPolicy policy;

        #endregion Fields

        #region Constructors

        public AsyncPollyQueryHandler(IAsyncQueryHandler<TQuery, TResult, TContext> handler, IAsyncPolicy policy)
        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            Ensure.That(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.handler.Context;

        #endregion Properties

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query, IMessageContext context = null)
        {
            return await policy.ExecuteAsync(() => this.handler.HandleAsync(query, context));
        }

        #endregion Methods

    }
}