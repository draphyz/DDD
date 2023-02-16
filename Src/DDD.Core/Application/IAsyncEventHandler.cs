using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles asynchronously an event of a specified type in a specific bounded context.
    /// </summary>
    public interface IAsyncEventHandler
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
        /// Handles asynchronously an event of a specified type in a specific bounded context.
        /// </summary>
        Task HandleAsync(IEvent @event, IMessageContext context = null);

        #endregion Methods
    }
}