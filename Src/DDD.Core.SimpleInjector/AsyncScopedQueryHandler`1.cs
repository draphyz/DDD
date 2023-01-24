using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using DDD.Core.Domain;
    using Threading;

    /// <summary>
    /// A decorator that defines a scope around the asynchronous execution of a query.
    /// </summary>
    public class AsyncScopedQueryHandler<TQuery, TResult, TContext> : IAsyncQueryHandler<TQuery, TResult, TContext>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext, new()
    {

        #region Fields

        private readonly Container container;
        private readonly TContext context;
        private readonly Func<IAsyncQueryHandler<TQuery, TResult, TContext>> handlerProvider;

        #endregion Fields

        #region Constructors

        public AsyncScopedQueryHandler(Func<IAsyncQueryHandler<TQuery, TResult, TContext>> handlerProvider, Container container)
        {
            Condition.Requires(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
            this.context = new TContext();
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.context;

        #endregion Properties

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query, IMessageContext context = null)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                await new SynchronizationContextRemover();
                var handler = this.handlerProvider();
                return await handler.HandleAsync(query, context);
            }
        }

        #endregion Methods

    }
}
