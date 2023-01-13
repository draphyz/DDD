using System;

namespace DDD.Core.Application
{
    public class Event
    {

        #region Properties

        public Guid EventId { get; set; }

        public string EventType { get; set; }

        public DateTime OccurredOn { get; set; }

        public string Body { get; set; }

        public string BodyFormat { get; set; }

        public string StreamId { get; set; }

        public string StreamType { get; set; }

        public string StreamSource { get; set; }

        public string IssuedBy { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [ {nameof(EventId)}={this.EventId}, {nameof(EventType)}={this.EventType}, {nameof(StreamId)}={this.StreamId}, {nameof(StreamType)}={this.StreamType}, {nameof(StreamSource)}={this.StreamSource}]";

        #endregion Methods

    }
}
