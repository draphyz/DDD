using Cronos;
using Conditions;
using System;

namespace DDD.Core.Infrastructure
{
    using Application;

    public class CronosScheduleFactory : IRecurringScheduleFactory
    {

        #region Methods

        public IRecurringSchedule Create(string recurringExpression)
        {
            Condition.Requires(recurringExpression, nameof(recurringExpression)).IsNotNull();
            var parts = recurringExpression.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var format = CronFormat.Standard;
            if (parts.Length == 6)
                format |= CronFormat.IncludeSeconds;
            else if (parts.Length != 5)
                throw new CronFormatException(
                    $"Wrong number of parts in the `{recurringExpression}` recurring expression, you can only use 5 or 6 (with seconds) part-based expressions.");
            var expression = CronExpression.Parse(recurringExpression, format);
            return new CronosSchedule(expression);
        }

        #endregion Methods

    }
}
