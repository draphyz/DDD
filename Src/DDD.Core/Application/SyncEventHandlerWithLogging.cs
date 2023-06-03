using EnsureThat;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// A decorator that logs information about events.
    /// </summary>
    public class SyncEventHandlerWithLogging<TEvent, TContext> : ISyncEventHandler<TEvent, TContext>
        where TEvent : class, IEvent
        where TContext : BoundedContext
    {

        #region Fields

        private readonly ISyncEventHandler<TEvent, TContext> eventHandler;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public SyncEventHandlerWithLogging(ISyncEventHandler<TEvent, TContext> eventHandler, ILogger logger)
        {
            Ensure.That(eventHandler, nameof(eventHandler)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            this.eventHandler = eventHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => eventHandler.Context;

        BoundedContext ISyncEventHandler.Context => this.Context;

        Type ISyncEventHandler.EventType => this.eventHandler.EventType;

        #endregion Properties

        #region Methods

        public void Handle(TEvent @event, IMessageContext context)
        {
            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                this.logger.LogDebug("Le traitement de l'évènement {Event} par {EventHandler} a commencé.", @event, this.eventHandler.GetType().Name);
                var stopWatch = Stopwatch.StartNew();
                this.eventHandler.Handle(@event, context);
                stopWatch.Stop();
                this.logger.LogDebug("Le traitement de l'évènement {Event} par {EventHandler} s'est terminé (temps d'exécution: {EventExecutionTime} ms).", @event, this.eventHandler.GetType().Name, stopWatch.ElapsedMilliseconds);
            }
            else
                this.eventHandler.Handle(@event, context);
        }

        void ISyncEventHandler.Handle(IEvent @event, IMessageContext context) => this.Handle((TEvent)@event, context);

        #endregion Methods

    }
}
