using EnsureThat;
using DDD.Core.Domain;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DDD.Core.Application
{
    /// <summary>
    /// A decorator that logs information about commands.
    /// </summary>
    public class SyncCommandHandlerWithLogging<TCommand, TContext> : ISyncCommandHandler<TCommand, TContext>
        where TCommand : class, ICommand
        where TContext : BoundedContext
    {

        #region Fields

        private readonly ISyncCommandHandler<TCommand, TContext> commandHandler;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public SyncCommandHandlerWithLogging(ISyncCommandHandler<TCommand, TContext> commandHandler, ILogger logger)
        {
            Ensure.That(commandHandler, nameof(commandHandler)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            this.commandHandler = commandHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.commandHandler.Context;

        #endregion Properties

        #region Methods

        public void Handle(TCommand command, IMessageContext context = null)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Le traitement de la commande {Command} par {CommandHandler} a commencé.", command, this.commandHandler.GetType().Name);
                var stopWatch = Stopwatch.StartNew();
                this.commandHandler.Handle(command, context);
                stopWatch.Stop();
                this.logger.LogDebug("Le traitement de la commande {Command} par {CommandHandler} s'est terminé (temps d'exécution: {CommandExecutionTime} ms).", command, this.commandHandler.GetType().Name, stopWatch.ElapsedMilliseconds);
            }
            else
                this.commandHandler.Handle(command, context);
        }

        #endregion Methods

    }
}
