using Conditions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about queries.
    /// </summary>
    public class SyncQueryHandlerWithLogging<TQuery, TResult> : ISyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        #region Fields

        private readonly ISyncQueryHandler<TQuery, TResult> queryHandler;
        private readonly ILogger logger;

        public SyncQueryHandlerWithLogging(ISyncQueryHandler<TQuery, TResult> queryHandler, ILogger logger)
        {
            Condition.Requires(queryHandler, nameof(queryHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.queryHandler = queryHandler;
            this.logger = logger;
        }

        #endregion Fields

        #region Methods

        public TResult Handle(TQuery query, IMessageContext context = null)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Le traitement de la requête {Query} par {QueryHandler} a commencé.", query, this.queryHandler.GetType().Name);
                var stopWatch = Stopwatch.StartNew();
                var result = this.queryHandler.Handle(query, context);
                stopWatch.Stop();
                this.logger.LogDebug("Le traitement de la requête {Query} par {QueryHandler} s'est terminé (temps d'exécution: {QueryExecutionTime} ms).", query, this.queryHandler.GetType().Name, stopWatch.ElapsedMilliseconds);
                return result;
            }
            else
                return this.queryHandler.Handle(query, context);
        }

        #endregion Methods
    }
}
