using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the asynchronous execution of a query.
    /// </summary>
    public class AsyncScopedQueryHandler<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Fields

        private readonly Container container;
        private readonly Func<IAsyncQueryHandler<TQuery, TResult>> handlerProvider;

        #endregion Fields

        #region Constructors

        public AsyncScopedQueryHandler(Func<IAsyncQueryHandler<TQuery, TResult>> handlerProvider, Container container)
        {
            Condition.Requires(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                var handler = this.handlerProvider();
                return await handler.HandleAsync(query, cancellationToken);
            }
        }

        #endregion Methods

    }
}
