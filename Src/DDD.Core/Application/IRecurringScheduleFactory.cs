using System;

namespace DDD.Core.Application
{
    /// <summary>
    /// Defines a factory of recurring schedules.
    /// </summary>
    public interface IRecurringScheduleFactory
    {

        #region Properties

        RecurringExpressionFormat Format { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a schedule initialized from a recurring expression.
        /// </summary>
        /// <exception cref="ArgumentNullException">recurringExpression is null</exception>
        /// <exception cref="FormatException">recurringExpression is not valid.</exception>
        IRecurringSchedule Create(string recurringExpression);

        #endregion Methods
    }
}
