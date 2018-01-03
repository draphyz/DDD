namespace DDD.Core.Domain
{
    public interface IDomainEventHandler<in TEvent> : IDomainEventHandler
        where TEvent : IDomainEvent
    {
        #region Methods

        void Handle(TEvent @event);

        #endregion Methods
    }
}