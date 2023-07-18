using EnsureThat;
using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using System.Collections.Concurrent;
    using Validation;

    /// <summary>
    /// The default query processor for processing and validating queries of any type.  
    /// </summary>
    public class QueryProcessor : IQueryProcessor
    {

        #region Fields

        private static readonly ConcurrentDictionary<BoundedContext, IContextualQueryProcessor> contextualProcessors
            = new ConcurrentDictionary<BoundedContext, IContextualQueryProcessor>();
        private readonly QueryProcessorSettings settings;
        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public QueryProcessor(IServiceProvider serviceProvider, QueryProcessorSettings settings)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            this.serviceProvider = serviceProvider;
            this.settings = settings;
        }

        #endregion Constructors

        #region Methods

        public IContextualQueryProcessor<TContext> InGeneric<TContext>(TContext context) where TContext : BoundedContext
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return (IContextualQueryProcessor<TContext>)contextualProcessors.GetOrAdd(context, _ =>
            new ContextualQueryProcessor<TContext>(this.serviceProvider, context));
        }

        public IContextualQueryProcessor InSpecific(BoundedContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return contextualProcessors.GetOrAdd(context, _ =>
            {
                var processorType = typeof(ContextualQueryProcessor<>).MakeGenericType(context.GetType());
                return (IContextualQueryProcessor)Activator.CreateInstance(processorType, this.serviceProvider, context);
            });
        }

        public TResult Process<TResult>(IQuery<TResult> query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(ISyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.Handle((dynamic)query, context);
        }

        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)query, context);
        }

        public ValidationResult Validate<TQuery>(TQuery query, IValidationContext context) where TQuery : class, IQuery
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var validator = this.serviceProvider.GetService<ISyncObjectValidator<TQuery>>();
            if (validator == null)
            {
                if (this.settings.DefaultValidator == null)
                    throw new InvalidOperationException($"The query validator for type {typeof(ISyncObjectValidator<TQuery>)} could not be found.");
                return this.settings.DefaultValidator.Validate(query, context);
            } 
            return validator.Validate(query, context);
        }

        public Task<ValidationResult> ValidateAsync<TQuery>(TQuery query, IValidationContext context) where TQuery : class, IQuery
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var validator = this.serviceProvider.GetService<IAsyncObjectValidator<TQuery>>();
            if (validator == null)
            {
                if (this.settings.DefaultValidator == null)
                    throw new InvalidOperationException($"The query validator for type {typeof(IAsyncObjectValidator<TQuery>)} could not be found.");
                return this.settings.DefaultValidator.ValidateAsync(query, context);
            }
            return validator.ValidateAsync(query, context);
        }

        #endregion Methods

    }
}