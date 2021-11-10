namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that handles an event of a specified type.
    /// </summary>
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : class, IEvent
    {
        #region Methods

        void Handle(TEvent @event);

        #endregion Methods
    }
}