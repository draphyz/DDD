using EnsureThat;
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
    public class AsyncEventHandlerWithLogging<TEvent, TContext> : IAsyncEventHandler<TEvent, TContext>
        where TEvent : class, IEvent
        where TContext : BoundedContext
    {

        #region Fields

        private readonly IAsyncEventHandler<TEvent, TContext> eventHandler;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public AsyncEventHandlerWithLogging(IAsyncEventHandler<TEvent, TContext> eventHandler, ILogger logger)
        {
            Ensure.That(eventHandler, nameof(eventHandler)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            this.eventHandler = eventHandler;
            this.logger = logger;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => eventHandler.Context;

        BoundedContext IAsyncEventHandler.Context => this.Context;

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
