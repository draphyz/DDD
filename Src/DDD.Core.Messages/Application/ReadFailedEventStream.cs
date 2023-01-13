using System;
using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class ReadFailedEventStream : IQuery<IEnumerable<Event>>
    {

        #region Properties

        public string StreamId { get; set; }
        public Guid EventIdMin { get; set; }
        public Guid EventIdMax { get; set; }
        public string StreamSource { get; set; }
        public string StreamType { get; set; }
        public int Top { get; set; } = 100;

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(StreamId)}={this.StreamId}, {nameof(StreamType)}={this.StreamType}, {nameof(StreamSource)}={this.StreamSource}, {nameof(EventIdMin)}={this.EventIdMin}, {nameof(EventIdMax)}={this.EventIdMax}]";

        #endregion Methods

    }
}