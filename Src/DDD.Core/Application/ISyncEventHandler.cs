using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles synchronously an event of a specified type in a specific bounded context.
    /// </summary>
    public interface ISyncEventHandler
    {

        #region Properties

        /// <summary>
        /// The bounded context in which the event is handled.
        /// </summary>
        BoundedContext Context { get; }

        /// <summary>
        /// The event type.
        /// </summary>
        Type EventType { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles synchronously an event of a specified type in a specific bounded context.
        /// </summary>
        void Handle(IEvent @event, IMessageContext context = null);

        #endregion Methods

    }
}