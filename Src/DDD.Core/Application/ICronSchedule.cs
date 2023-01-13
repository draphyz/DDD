using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a schedule initialized from a cron expression.
    /// </summary>
    public interface ICronSchedule
    {

        #region Methods

        /// <summary>
        /// Gets the next occurrence of this schedule starting with a base time.
        /// </summary>
        DateTime? GetNextOccurence(DateTime startTime);

        #endregion Methods

    }
}