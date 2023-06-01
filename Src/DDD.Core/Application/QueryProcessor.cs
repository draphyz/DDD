using EnsureThat;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;
    using Validation;

    /// <summary>
    /// The default query processor for processing and validating queries of any type.  
    /// </summary>
    public class QueryProcessor : IQueryProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public QueryProcessor(IServiceProvider serviceProvider)
        {
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public IContextualQueryProcessor<TContext> In<TContext>(TContext context) where TContext : BoundedContext
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            return new ContextualQueryProcessor<TContext>(this.serviceProvider, context);
        }

        public IContextualQueryProcessor In(BoundedContext context)
        {
            Ensure.That(context, nameof(context)).IsNotNull();
            var processorType = typeof(ContextualQueryProcessor<>).MakeGenericType(context.GetType());
            return (IContextualQueryProcessor)Activator.CreateInstance(processorType, this.serviceProvider, context);
        }

        public TResult Process<TResult>(IQuery<TResult> query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(ISyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.Handle((dynamic)query, context);
        }

        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context = null)
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)query, context);
        }

        public ValidationResult Validate<TQuery>(TQuery query, string ruleSet = null) where TQuery : class, IQuery
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var validator = this.serviceProvider.GetService<ISyncObjectValidator<TQuery>>();
            if (validator == null) throw new InvalidOperationException($"The query validator for type {typeof(ISyncObjectValidator<TQuery>)} could not be found.");
            return validator.Validate(query, ruleSet);
        }

        public Task<ValidationResult> ValidateAsync<TQuery>(TQuery query, string ruleSet = null, CancellationToken cancellationToken = default) where TQuery : class, IQuery
        {
            Ensure.That(query, nameof(query)).IsNotNull();
            var validator = this.serviceProvider.GetService<IAsyncObjectValidator<TQuery>>();
            if (validator == null) throw new InvalidOperationException($"The query validator for type {typeof(IAsyncObjectValidator<TQuery>)} could not be found.");
            return validator.ValidateAsync(query, ruleSet, cancellationToken);
        }

        #endregion Methods
    }
}