using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    public static class IQueryProcessorExtensions
    {

        #region Methods

        public static IContextualQueryProcessor<TContext> In<TContext>(this IQueryProcessor processor) where TContext : BoundedContext, new()
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            return processor.In<TContext>(new TContext());
        }

        public static TResult Process<TResult>(this IQueryProcessor processor,
                                               IQuery<TResult> query,
                                               object context)
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, MessageContext.FromObject(context));
        }

        public static Task<TResult> ProcessAsync<TResult>(this IQueryProcessor processor,
                                                          IQuery<TResult> query,
                                                          object context)
        {
            Condition.Requires(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}