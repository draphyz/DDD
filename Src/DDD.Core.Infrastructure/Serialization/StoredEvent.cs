using System;

namespace DDD.Core.Infrastructure.Serialization
{
    public class StoredEvent
    {

        #region Properties

        public string Body { get; set; }

        public Guid CommitId { get; set; }

        public bool Dispatched { get; set; } = false;

        public string EventType { get; set; }

        public long Id { get; set; }

        public DateTime OccurredOn { get; set; }

        public string StreamId { get; set; }

        public string Subject { get; set; }

        #endregion Properties

    }
}