using EnsureThat;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    public static class IAsyncQueryHandlerExtensions
    {

        #region Methods

        public static Task<TResult> HandleAsync<TQuery, TResult>(this IAsyncQueryHandler<TQuery, TResult> handler,
                                                                 TQuery query)
            where TQuery : class, IQuery<TResult>

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            return handler.HandleAsync(query, new MessageContext());
        }

        public static Task<TResult> HandleAsync<TQuery, TResult>(this IAsyncQueryHandler<TQuery, TResult> handler, 
                                                                 TQuery query, 
                                                                 object context)
            where TQuery : class, IQuery<TResult>

        {
            Ensure.That(handler, nameof(handler)).IsNotNull();
            return handler.HandleAsync(query, MessageContext.FromObject(context));
        }

        #endregion Methods

    }
}
