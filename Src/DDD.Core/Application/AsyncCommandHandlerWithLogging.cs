using Conditions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about commands.
    /// </summary>
    public class AsyncCommandHandlerWithLogging<TCommand> : IAsyncCommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly IAsyncCommandHandler<TCommand> commandHandler;
        private readonly ILogger logger;

        #endregion Fields


        #region Constructors

        public AsyncCommandHandlerWithLogging(IAsyncCommandHandler<TCommand> commandHandler, ILogger logger)
        {
            Condition.Requires(commandHandler, nameof(commandHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.commandHandler = commandHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("Executing command {Command}.", command);
                var stopWatch = Stopwatch.StartNew();
                await this.commandHandler.HandleAsync(command, cancellationToken);
                stopWatch.Stop();
                this.logger.LogInformation("Command executed in {CommandExecutionTime} ms.", stopWatch.ElapsedMilliseconds);
            }
            else
                await this.commandHandler.HandleAsync(command, cancellationToken);
        }

        #endregion Methods

    }
}
