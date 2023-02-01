using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a method that consumes events.
    /// </summary>
    /// <remarks>
    /// This component is used to read external event streams and to publish those events inside a bounded context.
    /// </remarks>
    public interface IEventConsumer
    {

        #region Properties

        /// <summary>
        /// Determines whether the consumer is running.
        /// </summary>
        bool IsRunning { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///  Starts consuming events.
        /// </summary>
        public void Start();

        /// <summary>
        /// Stops consuming events.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Waits for the event consumation to complete execution within a specified time interval.
        /// </summary>
        public void Wait(TimeSpan? timeout);

        #endregion Methods

    }
}