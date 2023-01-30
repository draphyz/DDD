using EnsureThat;
using DDD.Core.Domain;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about queries.
    /// </summary>
    public class AsyncQueryHandlerWithLogging<TQuery, TResult, TContext> : IAsyncQueryHandler<TQuery, TResult, TContext>
        where TQuery : class, IQuery<TResult>
        where TContext : BoundedContext
    {

        #region Fields

        private readonly ILogger logger;
        private readonly IAsyncQueryHandler<TQuery, TResult, TContext> queryHandler;

        #endregion Fields

        #region Constructors

        public AsyncQueryHandlerWithLogging(IAsyncQueryHandler<TQuery, TResult, TContext> queryHandler, ILogger logger)
        {
            Ensure.That(queryHandler, nameof(queryHandler)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            this.queryHandler = queryHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.queryHandler.Context;

        #endregion Properties

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query, IMessageContext context = null)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Le traitement de la requête {Query} par {QueryHandler} a commencé.", query, this.queryHandler.GetType().Name);
                var stopWatch = Stopwatch.StartNew();
                var result = await this.queryHandler.HandleAsync(query, context);
                stopWatch.Stop();
                this.logger.LogDebug("Le traitement de la requête {Query} par {QueryHandler} s'est terminé (temps d'exécution: {QueryExecutionTime} ms).", query, this.queryHandler.GetType().Name, stopWatch.ElapsedMilliseconds);
                return result;
            }
            else
                return await this.queryHandler.HandleAsync(query, context);
        }

        #endregion Methods

    }
}
