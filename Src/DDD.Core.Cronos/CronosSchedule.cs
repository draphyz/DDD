using EnsureThat;
using System;
using Cronos;

namespace DDD.Core.Infrastructure
{
    using Application;

    public class CronosSchedule : IRecurringSchedule
    {

        #region Fields

        CronExpression expression;

        #endregion Fields

        #region Constructors

        public CronosSchedule(CronExpression expression)
        {
            Ensure.That(expression, nameof(expression)).IsNotNull();
            this.expression = expression;
        }

        #endregion Constructors

        #region Methods

        public DateTime? GetNextOccurrence(DateTime startTime)
        {
            switch(startTime.Kind)
            {
                case DateTimeKind.Utc:
                    return this.expression.GetNextOccurrence(startTime);
                case DateTimeKind.Local:
                    return this.expression.GetNextOccurrence(new DateTimeOffset(startTime), TimeZoneInfo.Local)?.DateTime;
                default:
                    throw new ArgumentOutOfRangeException($"The kind '{startTime.Kind}' is not expected.");
            }
        }

        #endregion Methods

    }
}
