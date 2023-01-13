using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Core.Application
{
    /// <summary>
    /// Represents a component that registers, schedules and processes recurring commands.
    /// </summary>
    public interface IRecurringCommandManager
    {

        #region Properties

        /// <summary>
        /// Determines whether the manager is running.
        /// </summary>
        bool IsRunning { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///  Starts managing recurring commands.
        /// </summary>
        public void Start();

        /// <summary>
        /// Stops managing recurring commands.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Waits for the command management to complete execution within a specified time interval.
        /// </summary>
        public void Wait(TimeSpan? timeout);

        /// <summary>
        /// Registers a recurring command asynchronously.
        /// </summary>
        /// <returns></returns>
        Task RegisterAsync(ICommand command, string cronExpression, CancellationToken cancellationToken = default);

        #endregion Methods

    }
}