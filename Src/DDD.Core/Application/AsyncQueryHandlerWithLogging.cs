using Conditions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about queries.
    /// </summary>
    public class AsyncQueryHandlerWithLogging<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        #region Fields

        private readonly IAsyncQueryHandler<TQuery, TResult> queryHandler;
        private readonly ILogger logger;

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public AsyncQueryHandlerWithLogging(IAsyncQueryHandler<TQuery, TResult> queryHandler, ILogger logger)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            Condition.Requires(queryHandler, nameof(queryHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.queryHandler = queryHandler;
            this.logger = logger;
        }

        #endregion Fields

        #region Methods

        public async Task<TResult> HandleAsync(TQuery query)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("Executing query {Query}.", query);
                var stopWatch = Stopwatch.StartNew();
                var result = await this.queryHandler.HandleAsync(query);
                stopWatch.Stop();
                this.logger.LogInformation("Query executed in {QueryExecutionTime} ms.", stopWatch.ElapsedMilliseconds);
                return result;
            }
            else
                return await this.queryHandler.HandleAsync(query);
        }

        #endregion Methods
    }
}
