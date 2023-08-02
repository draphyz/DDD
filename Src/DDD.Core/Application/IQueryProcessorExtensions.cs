using EnsureThat;
using System.Threading.Tasks;
using DDD.Validation;

namespace DDD.Core.Application
{

    public static class IQueryProcessorExtensions
    {

        #region Methods

        public static TResult Process<TResult>(this IQueryProcessor processor,
                                               IQuery<TResult> query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, new MessageContext());
        }

        public static TResult Process<TResult>(this IQueryProcessor processor,
                                               IQuery<TResult> query,
                                               object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, MessageContext.FromObject(context));
        }

        public static object Process(this IQueryProcessor processor,
                                      IQuery query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, new MessageContext());
        }

        public static object Process(this IQueryProcessor processor,
                                     IQuery query,
                                     object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, MessageContext.FromObject(context));
        }

        public static Task<TResult> ProcessAsync<TResult>(this IQueryProcessor processor,
                                                          IQuery<TResult> query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, new MessageContext());
        }

        public static Task<TResult> ProcessAsync<TResult>(this IQueryProcessor processor,
                                                          IQuery<TResult> query,
                                                          object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, MessageContext.FromObject(context));
        }

        public static Task<object> ProcessAsync(this IQueryProcessor processor,
                                                IQuery query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, new MessageContext());
        }

        public static Task<object> ProcessAsync(this IQueryProcessor processor,
                                                IQuery query,
                                                object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, MessageContext.FromObject(context));
        }

        public static ValidationResult Validate<TQuery>(this IQueryProcessor processor,
                                                        TQuery query)
            where TQuery : class, IQuery
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(query, new ValidationContext());
        }

        public static ValidationResult Validate<TQuery>(this IQueryProcessor processor, 
                                                        TQuery query, 
                                                        object context) 
            where TQuery : class, IQuery
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(query, ValidationContext.FromObject(context));
        }

        public static ValidationResult Validate(this IQueryProcessor processor,
                                                IQuery query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(query, new ValidationContext());
        }

        public static ValidationResult Validate(this IQueryProcessor processor,
                                                IQuery query,
                                                object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Validate(query, ValidationContext.FromObject(context));
        }

        public static Task<ValidationResult> ValidateAsync<TQuery>(this IQueryProcessor processor,
                                                                   TQuery query)
            where TQuery : class, IQuery
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(query, new ValidationContext());
        }

        public static Task<ValidationResult> ValidateAsync<TQuery>(this IQueryProcessor processor, 
                                                                   TQuery query, 
                                                                   object context) 
            where TQuery : class, IQuery
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(query, ValidationContext.FromObject(context));
        }

        public static Task<ValidationResult> ValidateAsync(this IQueryProcessor processor,
                                                           IQuery query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(query, new ValidationContext());
        }

        public static Task<ValidationResult> ValidateAsync(this IQueryProcessor processor,
                                                           IQuery query,
                                                           object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ValidateAsync(query, ValidationContext.FromObject(context));
        }

        #endregion Methods

    }
}