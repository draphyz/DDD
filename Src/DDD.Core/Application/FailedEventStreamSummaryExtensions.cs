using System;
using System.Collections.Generic;
using System.Linq;
using Conditions;

namespace DDD.Core.Application
{
    internal static class FailedEventStreamSummaryExtensions
    {

        #region Methods

        public static bool IsActive(this FailedEventStream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            return stream.RetryCount < stream.RetryMax && SystemTime.Local() >= stream.RetryTime();
        }

        public static short RetryDelay(this FailedEventStream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            var retryDelays = stream.AllRetryDelays();
            if (stream.RetryCount >= retryDelays.Count())
                return retryDelays.Last();
            return retryDelays.ElementAt(stream.RetryCount);
        }

        public static IEnumerable<short> AllRetryDelays(this FailedEventStream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            if (stream.RetryDelays == null || stream.RetryDelays.Count == 0 || stream.RetryMax == 0)
                return Array.Empty<short>();
            if (stream.RetryDelays.Count >= stream.RetryMax)
                return stream.RetryDelays.Take(stream.RetryMax).Select(d => d.Delay);
            var delays = new List<short>(stream.RetryDelays.Select(d => d.Delay));
            var last = stream.RetryDelays.Last();
            while (delays.Count < stream.RetryMax)
            {
                last = last.Next();
                delays.Add(last.Delay);
            }
            return delays;
        }

        public static DateTime RetryTime(this FailedEventStream stream)
        {
            Condition.Requires(stream, nameof(stream)).IsNotNull();
            return stream.ExceptionTime.AddMinutes(stream.RetryDelay());
        }

        private static IncrementalDelay Next(this IncrementalDelay delay)
        {
            Condition.Requires(delay, nameof(delay))
                     .IsNotNull()
                     .Evaluate(delay.Increment >= 0);
            return new IncrementalDelay
            {
                Delay = (short)(delay.Delay + delay.Increment),
                Increment = delay.Increment
            };
        }

        #endregion Methods

    }
}
