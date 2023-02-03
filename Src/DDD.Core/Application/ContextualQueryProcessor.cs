using EnsureThat;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// The query processor for processing and validating generic queries in a specific bounded context.  
    /// </summary>
    public class ContextualQueryProcessor<TContext> : IContextualQueryProcessor<TContext>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public ContextualQueryProcessor(IServiceProvider serviceProvider, TContext context)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Ensure.That(context, nameof(context)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.Context = context;
        }

        #endregion Constructors

        #region Properties

        public TContext Context { get; }

        BoundedContext IContextualQueryProcessor.Context => this.Context;

        #endregion Properties

        #region Methods

        public TResult Process<TResult>(IQuery<TResult> query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(ISyncQueryHandler<,,>).MakeGenericType(query.GetType(), typeof(TResult), typeof(TContext));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.Handle((dynamic)query, context);
        }

        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(IAsyncQueryHandler<,,>).MakeGenericType(query.GetType(), typeof(TResult), typeof(TContext));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)query, context);
        }

        #endregion Methods

    }
}