using Polly;
using EnsureThat;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;
    using DDD.Core.Domain;

    /// <summary>
    /// A decorator that applies a resilience policy to the synchronous execution of a query.
    /// </summary>
    public class SyncPollyQueryHandler<TQuery, TResult, TContext> : ISyncQueryHandler<TQuery, TResult, TContext>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly ISyncQueryHandler<TQuery, TResult, TContext> handler;
        private readonly ISyncPolicy policy;

        #endregion Fields

        #region Constructors

        public SyncPollyQueryHandler(IQueryHandler<TQuery, TResult, TContext> handler, ISyncPolicy policy)
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

        public TResult Handle(TQuery query, IMessageContext context = null)
        {
            return policy.Execute(() => this.handler.Handle(query, context));
        }

        #endregion Methods

    }
}