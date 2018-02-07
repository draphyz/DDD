namespace DDD.Core.Domain
{
    public interface IDomainEventPublisher
    {

        #region Methods

        void Publish(IDomainEvent @event);

        void Subscribe(IDomainEventHandler subscriber);

        void UnSubscribe(IDomainEventHandler subscriber);

        #endregion Methods

    }
}