using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that publishes events of any type in a specific bounded context.
    /// </summary>
    public interface IEventPublisher
    {

        #region Properties

        /// <summary>
        /// The bounded context in which events are published.
        /// </summary>
        BoundedContext Context { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Publishes an event in a specific bounded context.
        /// </summary>
        Task PublishAsync<TEvent>(TEvent @event, IMessageContext context = null) where TEvent : class, IEvent;

        #endregion Methods

    }
}