using System;
using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class FailedEventStream
    {

        #region Properties

        public string StreamId { get; set; }

        public string StreamType { get; set; }

        public string StreamSource { get; set; }

        public Guid StreamPosition { get; set; }

        public Guid EventId { get; set; }

        public DateTime ExceptionTime { get; set; }

        public byte RetryCount { get; set; }

        public byte RetryMax { get; set; }

        public ICollection<IncrementalDelay> RetryDelays { get; set; } = new List<IncrementalDelay>();

        public short BlockSize { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(StreamId)}={this.StreamId}, {nameof(StreamType)}={this.StreamType}, {nameof(StreamSource)}={this.StreamSource}]";

        #endregion Methods

    }
}