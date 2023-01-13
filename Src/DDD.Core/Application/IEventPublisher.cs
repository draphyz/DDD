using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that publishes events of any type.
    /// </summary>
    /// <remarks>
    /// This component is used to publish events inside a bounded context.
    /// </remarks>
    public interface IEventPublisher
    {

        #region Methods

        Task PublishAsync<TEvent>(TEvent @event, IMessageContext context = null) where TEvent : class, IEvent;

        #endregion Methods

    }
}