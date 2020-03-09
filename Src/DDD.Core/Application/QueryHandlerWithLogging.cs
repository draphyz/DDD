using Conditions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about queries.
    /// </summary>
    public class QueryHandlerWithLogging<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        #region Fields

        private readonly IQueryHandler<TQuery, TResult> queryHandler;
        private readonly ILogger logger;

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public QueryHandlerWithLogging(IQueryHandler<TQuery, TResult> queryHandler, ILogger logger)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            Condition.Requires(queryHandler, nameof(queryHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.queryHandler = queryHandler;
            this.logger = logger;
        }

        #endregion Fields

        #region Methods

        public TResult Handle(TQuery query)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("Executing query {Query}.", query);
                var stopWatch = Stopwatch.StartNew();
                var result = this.queryHandler.Handle(query);
                stopWatch.Stop();
                this.logger.LogInformation("Query executed in {QueryExecutionTime} ms.", stopWatch.ElapsedMilliseconds);
                return result;
            }
            else
                return this.queryHandler.Handle(query);
        }

        #endregion Methods
    }
}
