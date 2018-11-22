namespace DDD.Core.Domain
{
    public interface IEventPublisher
    {

        #region Methods

        void Publish(IEvent @event);

        void Subscribe(IEventHandler subscriber);

        void UnSubscribe(IEventHandler subscriber);

        #endregion Methods

    }
}