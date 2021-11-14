using System;

namespace DDD.Core.Infrastructure.Data
{
    public class StoredEvent
    {

        #region Properties

        public string Body { get; set; }

        public string EventType { get; set; }

        public long Id { get; set; }

        public DateTime OccurredOn { get; set; }

        public string StreamId { get; set; }

        public string StreamType { get; set; }

        public string IssuedBy { get; set; }

        public byte Version { get; set; } = 1;

        #endregion Properties

    }
}