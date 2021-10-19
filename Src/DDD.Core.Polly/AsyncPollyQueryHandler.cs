using Polly;
using Conditions;
using System.Threading;
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
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            return await policy.ExecuteAsync(() => this.handler.HandleAsync(query, cancellationToken));
        }

        #endregion Methods

    }
}