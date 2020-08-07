using Conditions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about commands.
    /// </summary>
    public class CommandHandlerWithLogging<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {

        #region Fields

        private readonly ICommandHandler<TCommand> commandHandler;
        private readonly ILogger logger;

        #endregion Fields


        #region Constructors

        public CommandHandlerWithLogging(ICommandHandler<TCommand> commandHandler, ILogger logger)
        {
            Condition.Requires(commandHandler, nameof(commandHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.commandHandler = commandHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Methods

        public void Handle(TCommand command)
        {
            if (this.logger.IsEnabled(LogLevel.Information))
            {
                this.logger.LogInformation("Executing command {Command}.", command);
                var stopWatch = Stopwatch.StartNew();
                this.commandHandler.Handle(command);
                stopWatch.Stop();
                this.logger.LogInformation("Command executed in {CommandExecutionTime} ms.", stopWatch.ElapsedMilliseconds);
            }
            else
                this.commandHandler.Handle(command);
        }

        #endregion Methods

    }
}
