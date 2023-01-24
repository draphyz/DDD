using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a component used to schedule recurring events.
    /// </summary>
    public interface IRecurringSchedule
    {

        #region Methods

        /// <summary>
        /// Gets the next occurrence of this schedule starting with a base time.
        /// </summary>
        DateTime? GetNextOccurrence(DateTime startTime);

        #endregion Methods

    }
}