using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a component that publishes events of any type.
    /// </summary>
    public interface IEventPublisher
    {

        #region Methods

        void Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;

        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class, IEvent;

        #endregion Methods

    }
}