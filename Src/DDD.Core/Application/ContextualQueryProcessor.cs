using EnsureThat;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Threading;

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

        public TResult Process<TResult>(IQuery<TResult> query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(ISyncQueryHandler<,,>).MakeGenericType(query.GetType(), typeof(TResult), typeof(TContext));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.Handle((dynamic)query, context);
        }

        public object Process(IQuery query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var queryType = query.GetType();
            var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
            if (queryInterfaceType == null) throw new ArgumentException($"{queryType.Name} does not implement {typeof(IQuery<>).Name}.", nameof(query));
            var resultType = queryInterfaceType.GetGenericArguments()[0];
            var handlerType = typeof(ISyncQueryHandler<,,>).MakeGenericType(query.GetType(), resultType, typeof(TContext));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.Handle((dynamic)query, context);
        }

        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(IAsyncQueryHandler<,,>).MakeGenericType(query.GetType(), typeof(TResult), typeof(TContext));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)query, context);
        }

        public async Task<object> ProcessAsync(IQuery query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            await new SynchronizationContextRemover();
            var queryType = query.GetType();
            var queryInterfaceType = queryType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
            if (queryInterfaceType == null) throw new ArgumentException($"{queryType.Name} does not implement {typeof(IQuery<>).Name}.", nameof(query));
            var resultType = queryInterfaceType.GetGenericArguments()[0];
            var handlerType = typeof(IAsyncQueryHandler<,,>).MakeGenericType(query.GetType(), resultType, typeof(TContext));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return await handler.HandleAsync((dynamic)query, context);
        }

        #endregion Methods

    }
}