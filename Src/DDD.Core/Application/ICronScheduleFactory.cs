using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a factory of cron schedule.
    /// </summary>
    public interface ICronScheduleFactory
    {

        #region Methods

        /// <summary>
        /// Creates a schedule initialized from a cron expression.
        /// </summary>
        /// <exception cref="ArgumentNullException">cronExpression is null</exception>
        /// <exception cref="FormatException">cronExpression is not a valid string representation of a cron Expression.</exception>
        ICronSchedule Create(string cronExpression);

        #endregion Methods

    }
}
