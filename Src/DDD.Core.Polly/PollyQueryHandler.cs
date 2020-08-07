using Polly;
using Conditions;

namespace DDD.Core.Infrastructure.ErrorHandling
{
    using Application;

    /// <summary>
    /// A decorator that applies a resilience policy to the synchronous execution of a query.
    /// </summary>
    public class PollyQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Fields

        private readonly IQueryHandler<TQuery, TResult> handler;
        private readonly ISyncPolicy policy;

        #endregion Fields

        #region Constructors

        public PollyQueryHandler(IQueryHandler<TQuery, TResult> handler, ISyncPolicy policy)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(policy, nameof(policy)).IsNotNull();
            this.handler = handler;
            this.policy = policy;
        }

        #endregion Constructors

        #region Methods

        public TResult Handle(TQuery query)
        {
            return policy.Execute(() => this.handler.Handle(query));
        }

        #endregion Methods

    }
}