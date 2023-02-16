namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines methods for consuming events in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to read external event streams and to publish those events in a bounded context.
    /// </remarks>
    public interface IEventConsumer<out TContext> : IEventConsumer
        where TContext : BoundedContext
    {
        #region Properties

        /// <summary>
        /// The bounded context in which events are consumed.
        /// </summary>
        new TContext Context { get; }

        #endregion Properties
    }
}
