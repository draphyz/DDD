using System;

namespace DDD.Core.Application
{
    using Domain;

    /// <summary>
    /// Defines methods for consuming events in a specific bounded context.
    /// </summary>
    /// <remarks>
    /// This component is used to read external event streams and to publish those events in a bounded context.
    /// </remarks>
    public interface IEventConsumer
    {

        #region Properties

        /// <summary>
        /// The bounded context in which events are consumed.
        /// </summary>
        BoundedContext Context { get; }

        /// <summary>
        /// Determines whether the consumer is running.
        /// </summary>
        bool IsRunning { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///  Starts consuming events in a specific bounded context.
        /// </summary>
        public void Start();

        /// <summary>
        /// Stops consuming events in a specific bounded context.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Waits for the event consumation to complete execution within a specified time interval.
        /// </summary>
        public void Wait(TimeSpan? timeout);

        #endregion Methods

    }
}