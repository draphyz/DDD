using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;
using System;

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
        private readonly Func<IQueryHandler<TQuery, TResult>> handlerProvider;

        #endregion Fields

        #region Constructors

        public ThreadScopedQueryHandler(Func<IQueryHandler<TQuery, TResult>> handlerProvider, Container container)
        {
            Condition.Requires(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public TResult Handle(TQuery query)
        {
            using (ThreadScopedLifestyle.BeginScope(container))
            {
                var handler = this.handlerProvider();
                return handler.Handle(query);
            }
        }

        #endregion Methods

    }
}
