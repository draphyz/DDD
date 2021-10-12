using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    public interface IAsyncEventPublisher
    {

        #region Methods

        Task PublishAsync(IEvent @event);

        #endregion Methods

    }
}