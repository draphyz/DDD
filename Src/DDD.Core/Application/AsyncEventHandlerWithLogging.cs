using Conditions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// A decorator that logs information about events.
    /// </summary>
    public class AsyncEventHandlerWithLogging<TEvent> : IAsyncEventHandler<TEvent>
        where TEvent : class, IEvent
    {

        #region Fields

        private readonly IAsyncEventHandler<TEvent> eventHandler;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public AsyncEventHandlerWithLogging(IAsyncEventHandler<TEvent> eventHandler, ILogger logger)
        {
            Condition.Requires(eventHandler, nameof(eventHandler)).IsNotNull();
            Condition.Requires(logger, nameof(logger)).IsNotNull();
            this.eventHandler = eventHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        Type IAsyncEventHandler.EventType => this.eventHandler.EventType;

        #endregion Properties

        #region Methods

        public async Task HandleAsync(TEvent @event, IMessageContext context = null)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Le traitement de l'évènement {Event} par {EventHandler} a commencé.", @event, this.eventHandler.GetType().Name);
                var stopWatch = Stopwatch.StartNew();
                await this.eventHandler.HandleAsync(@event, context);
                stopWatch.Stop();
                this.logger.LogDebug("Le traitement de l'évènement {Event} par {EventHandler} s'est terminé (temps d'exécution: {EventExecutionTime} ms).", @event, this.eventHandler.GetType().Name, stopWatch.ElapsedMilliseconds);
            }
            else
                await this.eventHandler.HandleAsync(@event, context);
        }

        Task IAsyncEventHandler.HandleAsync(IEvent @event, IMessageContext context) => this.HandleAsync((TEvent)@event, context);

        #endregion Methods

    }
}
