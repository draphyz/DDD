using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the synchronous execution of a query.
    /// </summary>
    public class ThreadScopedQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Fields

        private readonly Container container;
        private readonly IQueryHandler<TQuery, TResult> handler;

        #endregion Fields

        #region Constructors

        public ThreadScopedQueryHandler(IQueryHandler<TQuery, TResult> handler, Container container)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handler = handler;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public TResult Handle(TQuery query)
        {
            using (ThreadScopedLifestyle.BeginScope(container))
            {
                return this.handler.Handle(query);
            }
        }

        #endregion Methods

    }
}
