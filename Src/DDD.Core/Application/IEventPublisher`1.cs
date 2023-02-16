namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines a method that publishes events of any type in a specific bounded context.
    /// </summary>
    public interface IEventPublisher<out TContext> : IEventPublisher
        where TContext: BoundedContext
    {
        #region Properties

        /// <summary>
        /// The bounded context in which events are published.
        /// </summary>
        new TContext Context { get; }

        #endregion Properties
    }
}
