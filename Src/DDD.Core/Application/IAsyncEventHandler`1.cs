using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles asynchronously an event of a specified type.
    /// </summary>
    public interface IAsyncEventHandler<in TEvent> : IAsyncEventHandler
        where TEvent : class, IEvent
    {

        #region Methods

        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);

        #endregion Methods

    }
}