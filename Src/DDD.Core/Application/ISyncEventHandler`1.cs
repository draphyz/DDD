namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles synchronously an event of a specified type in a specific bounded context.
    /// </summary>
    public interface ISyncEventHandler<in TEvent, out TContext> : ISyncEventHandler
        where TEvent : class, IEvent
        where TContext : BoundedContext
    {
        #region Properties

        /// <summary>
        /// The bounded context in which the event is handled.
        /// </summary>
        new TContext Context { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles synchronously an event of a specified type in a specific bounded context.
        /// </summary>
        void Handle(TEvent @event, IMessageContext context = null);

        #endregion Methods

    }
}