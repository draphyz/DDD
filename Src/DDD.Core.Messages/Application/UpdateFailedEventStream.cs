using System;
using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class UpdateFailedEventStream : ICommand
    {

        #region Properties

        public string StreamId { get; set; }

        public string StreamType { get; set; }

        public string StreamSource { get; set; }

        public Guid StreamPosition { get; set; }

        public Guid EventId { get; set; }

        public string EventType { get; set; }

        public DateTime ExceptionTime { get; set; }

        public string ExceptionType { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionSource { get; set; }

        public string ExceptionInfo { get; set; }

        public string BaseExceptionType { get; set; }

        public string BaseExceptionMessage { get; set; }

        public byte RetryCount { get; set; }

        public byte RetryMax { get; set; }

        public ICollection<IncrementalDelay> RetryDelays { get; set; } = new List<IncrementalDelay>();

        #endregion Properties

    }
}