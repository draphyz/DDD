namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles synchronously an event of a specified type.
    /// </summary>
    public interface ISyncEventHandler<in TEvent> : ISyncEventHandler
        where TEvent : class, IEvent
    {

        #region Methods

        void Handle(TEvent @event, IMessageContext context = null);

        #endregion Methods

    }
}