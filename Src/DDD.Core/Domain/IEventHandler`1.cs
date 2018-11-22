namespace DDD.Core.Domain
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IEvent
    {
        #region Methods

        void Handle(TEvent @event);

        #endregion Methods
    }
}