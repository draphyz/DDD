using Polly;
using EnsureThat;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;

    /// <summary>
    /// A decorator that applies a resilience policy to the synchronous execution of a query.
    /// </summary>
    public class SyncPollyQueryHandler<TQuery, TResult> : ISyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Fields

        private readonly ISyncQueryHandler<TQuery, TResult> handler;
        private readonly ISyncPolicy policy;

        #endregion Fields

        #region Constructors

        public SyncPollyQueryHandler(ISyncQueryHandler<TQuery, TResult> handler, ISyncPolicy policy)
        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            Ensure.That(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public TResult Handle(TQuery query, IMessageContext context)
        {
            return policy.Execute(() => this.handler.Handle(query, context));
        }

        #endregion Methods

    }
}