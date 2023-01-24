using Conditions;
using DDD.Core.Domain;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about commands.
    /// </summary>
    public class AsyncCommandHandlerWithLogging<TCommand, TContext> : IAsyncCommandHandler<TCommand, TContext>
        where TCommand : class, ICommand
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IAsyncCommandHandler<TCommand, TContext> commandHandler;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public AsyncCommandHandlerWithLogging(IAsyncCommandHandler<TCommand, TContext> commandHandler, ILogger logger)
        {
            Condition.Requires(commandHandler, nameof(commandHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.commandHandler = commandHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.commandHandler.Context;

        #endregion Properties

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
