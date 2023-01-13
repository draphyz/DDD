using Conditions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Validation;

    /// <summary>
    /// The query processor by default for processing and validating queries.  
    /// </summary>
    public class DefaultQueryProcessor : IQueryProcessor
    {

        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructors

        public DefaultQueryProcessor(IServiceProvider serviceProvider)
        {
            Condition.Requires(serviceProvider, nameof(serviceProvider)).IsNotNull();
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Methods

        public TResult Process<TResult>(IQuery<TResult> query, IMessageContext context = null)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            var handlerType = typeof(ISyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.Handle((dynamic)query, context);
        }

        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query, IMessageContext context = null)
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = this.serviceProvider.GetService(handlerType);
            if (handler == null) throw new InvalidOperationException($"The query handler for type {handlerType} could not be found.");
            return handler.HandleAsync((dynamic)query, context);
        }

        public ValidationResult Validate<TQuery>(TQuery query, string ruleSet = null) where TQuery : class, IQuery
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            var validator = this.serviceProvider.GetService<ISyncQueryValidator<TQuery>>();
            if (validator == null) throw new InvalidOperationException($"The query validator for type {typeof(ISyncQueryValidator<TQuery>)} could not be found.");
            return validator.Validate(query, ruleSet);
        }

        public Task<ValidationResult> ValidateAsync<TQuery>(TQuery query, string ruleSet = null, CancellationToken cancellationToken = default) where TQuery : class, IQuery
        {
            Condition.Requires(query, nameof(query)).IsNotNull();
            var validator = this.serviceProvider.GetService<IAsyncQueryValidator<TQuery>>();
            if (validator == null) throw new InvalidOperationException($"The query validator for type {typeof(IAsyncQueryValidator<TQuery>)} could not be found.");
            return validator.ValidateAsync(query, ruleSet, cancellationToken);
        }

        #endregion Methods

    }
}