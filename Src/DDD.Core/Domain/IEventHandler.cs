using System;

namespace DDD.Core.Domain
{
    public interface IEventHandler
    {
        #region Properties

        Type EventType { get; }

        #endregion Properties

        #region Methods

        void Handle(IEvent @event);

        #endregion Methods
    }
}