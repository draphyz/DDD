using SimpleInjector;
using SimpleInjector.Lifestyles;
using Conditions;
using System.Threading.Tasks;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Application;

    /// <summary>
    /// A decorator that defines a scope around the asynchronous execution of a query.
    /// </summary>
    public class AsyncScopedQueryHandler<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {

        #region Fields

        private readonly Container container;
        private readonly IAsyncQueryHandler<TQuery, TResult> handler;

        #endregion Fields

        #region Constructors

        public AsyncScopedQueryHandler(IAsyncQueryHandler<TQuery, TResult> handler, Container container)
        {
            Condition.Requires(handler, nameof(handler)).IsNotNull();
            Condition.Requires(container, nameof(container)).IsNotNull();
            this.handler = handler;
            this.container = container;
        }

        #endregion Constructors

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query)
        {
            using (AsyncScopedLifestyle.BeginScope(container))
            {
                return await this.handler.HandleAsync(query);
            }
        }

        #endregion Methods

    }
}
