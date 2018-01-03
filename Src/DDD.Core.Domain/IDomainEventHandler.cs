using System;

namespace DDD.Core.Domain
{
    public interface IDomainEventHandler
    {
        #region Properties

        Type EventType { get; }

        #endregion Properties

        #region Methods

        void Handle(IDomainEvent @event);

        #endregion Methods
    }
}