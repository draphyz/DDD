using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    /// <summary>
    /// Publish asynchronously events outside the local bounded context (use to decouple bounded contexts).
    /// </summary>
    public interface IAsyncEventPublisher
    {

        #region Methods

        Task PublishAsync(IEvent @event);

        #endregion Methods

    }
}