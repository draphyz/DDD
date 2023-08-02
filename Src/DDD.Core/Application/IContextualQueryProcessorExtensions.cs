using EnsureThat;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    public static class IContextualQueryProcessorExtensions
    {

        #region Methods

        public static TResult Process<TResult>(this IContextualQueryProcessor processor,
                                               IQuery<TResult> query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, new MessageContext());
        }

        public static TResult Process<TResult>(this IContextualQueryProcessor processor,
                                               IQuery<TResult> query,
                                               object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, MessageContext.FromObject(context));
        }

        public static object Process(this IContextualQueryProcessor processor,
                                     IQuery query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, new MessageContext());
        }

        public static object Process(this IContextualQueryProcessor processor,
                                     IQuery query,
                                     object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.Process(query, MessageContext.FromObject(context));
        }

        public static Task<TResult> ProcessAsync<TResult>(this IContextualQueryProcessor processor,
                                                          IQuery<TResult> query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, new MessageContext());
        }

        public static Task<TResult> ProcessAsync<TResult>(this IContextualQueryProcessor processor,
                                                          IQuery<TResult> query,
                                                          object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, MessageContext.FromObject(context));
        }

        public static Task<object> ProcessAsync(this IContextualQueryProcessor processor,
                                                IQuery query)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, new MessageContext());
        }

        public static Task<object> ProcessAsync(this IContextualQueryProcessor processor,
                                                IQuery query,
                                                object context)
        {
            Ensure.That(processor, nameof(processor)).IsNotNull();
            return processor.ProcessAsync(query, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}
