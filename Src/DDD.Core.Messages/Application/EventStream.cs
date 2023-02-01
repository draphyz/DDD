using System;
using System.Collections.Generic;

namespace DDD.Core.Application
{
    public class EventStream
    {

        #region Properties

        public string Type { get; set; }

        public string Source { get; set; }

        public Guid Position { get; set; }

        public byte RetryMax { get; set; }

        public ICollection<IncrementalDelay> RetryDelays { get; set; } = new List<IncrementalDelay>();

        public short BlockSize { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
            => $"{this.GetType().Name} [{nameof(Type)}={this.Type}, {nameof(Source)}={this.Source}]";

        #endregion Methods

    }
}