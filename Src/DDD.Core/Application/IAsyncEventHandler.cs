using System;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles asynchronously an event of a specified type.
    /// </summary>
    public interface IAsyncEventHandler
    {
        #region Properties

        Type EventType { get; }

        #endregion Properties

        #region Methods

        Task HandleAsync(IEvent @event, IMessageContext context = null);

        #endregion Methods
    }
}