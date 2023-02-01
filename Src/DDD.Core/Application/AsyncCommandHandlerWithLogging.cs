using EnsureThat;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
            Ensure.That(commandHandler, nameof(commandHandler)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            this.commandHandler = commandHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Methods

        public async Task HandleAsync(TCommand command, IMessageContext context = null)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Le traitement de la commande {Command} par {CommandHandler} a commencé.", command, this.commandHandler.GetType().Name);
                var stopWatch = Stopwatch.StartNew();
                await this.commandHandler.HandleAsync(command, context);
                stopWatch.Stop();
                this.logger.LogDebug("Le traitement de la commande {Command} par {CommandHandler} s'est terminé (temps d'exécution: {CommandExecutionTime} ms).", command, this.commandHandler.GetType().Name, stopWatch.ElapsedMilliseconds);
            }
            else
                await this.commandHandler.HandleAsync(command, context);
        }

        #endregion Methods

    }
}
