using System;
using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class ReadEventStream : IQuery<IEnumerable<Event>>
    {

        #region Properties

        public string[] ExcludedStreamIds { get; set; } = { };
        public Guid StreamPosition { get; set; }
        public string StreamType { get; set; }
        public short Top { get; set; } = 100;

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(StreamType)}={this.StreamType}, {nameof(StreamPosition)}={this.StreamPosition}]";

        #endregion Methods

    }
}