namespace DDD.Core.Domain
{
    /// <summary>
    /// Publish synchronously events inside the local bounded context (use to decouple layers).
    /// </summary>
    public interface IEventPublisher
    {

        #region Methods

        void Publish(IEvent @event);

        void Subscribe(IEventHandler subscriber);

        void UnSubscribe(IEventHandler subscriber);

        #endregion Methods

    }
}