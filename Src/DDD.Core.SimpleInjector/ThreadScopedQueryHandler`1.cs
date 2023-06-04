using SimpleInjector;
using SimpleInjector.Lifestyles;
using EnsureThat;
using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;
    using Domain;

    /// <summary>
    /// A decorator that defines a scope around the synchronous execution of a query.
    /// </summary>
    public class ThreadScopedQueryHandler<TQuery, TResult, TContext> : ISyncQueryHandler<TQuery, TResult, TContext>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly Container container;
        private readonly Func<ISyncQueryHandler<TQuery, TResult, TContext>> handlerProvider;

        #endregion Fields

        #region Constructors

        public ThreadScopedQueryHandler(Func<ISyncQueryHandler<TQuery, TResult, TContext>> handlerProvider, Container container)
        {
            Ensure.That(handlerProvider, nameof(handlerProvider)).IsNotNull();
            Ensure.That(container, nameof(container)).IsNotNull();
            this.handlerProvider = handlerProvider;
            this.container = container;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.handlerProvider().Context;

        #endregion Properties

        #region Methods

        public TResult Handle(TQuery query, IMessageContext context)
        {
            using (ThreadScopedLifestyle.BeginScope(container))
            {
                var handler = this.handlerProvider();
                return handler.Handle(query, context);
            }
        }

        #endregion Methods

    }
}
