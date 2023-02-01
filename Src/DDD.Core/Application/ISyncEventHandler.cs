using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles synchronously an event of a specified type.
    /// </summary>
    public interface ISyncEventHandler
    {
        #region Properties

        Type EventType { get; }

        #endregion Properties

        #region Methods

        void Handle(IEvent @event, IMessageContext context = null);

        #endregion Methods
    }
}