using Polly;
using EnsureThat;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;

    /// <summary>
    /// A decorator that applies a resilience policy to the asynchronous execution of a query.
    /// </summary>
    public class AsyncPollyQueryHandler<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Fields

        private readonly IAsyncQueryHandler<TQuery, TResult> handler;
        private readonly IAsyncPolicy policy;

        #endregion Fields

        #region Constructors

        public AsyncPollyQueryHandler(IAsyncQueryHandler<TQuery, TResult> handler, IAsyncPolicy policy)
        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            Ensure.That(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query, IMessageContext context = null)
        {
            return await policy.ExecuteAsync(() => this.handler.HandleAsync(query, context));
        }

        #endregion Methods

    }
}