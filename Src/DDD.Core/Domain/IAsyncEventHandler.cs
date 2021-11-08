using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Domain
{
    public interface IAsyncEventHandler
    {
        #region Properties

        Type EventType { get; }

        #endregion Properties

        #region Methods

        Task HandleAsync(IEvent @event, CancellationToken cancellationToken = default);

        #endregion Methods
    }
}