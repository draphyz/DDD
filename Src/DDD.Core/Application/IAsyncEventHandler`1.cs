using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles asynchronously an event of a specified type in a specific bounded context.
    /// </summary>
    public interface IAsyncEventHandler<in TEvent, out TContext> : IAsyncEventHandler
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
        /// Handles asynchronously an event of a specified type in a specific bounded context.
        /// </summary>
        Task HandleAsync(TEvent @event, IMessageContext context);

        #endregion Methods

    }
}