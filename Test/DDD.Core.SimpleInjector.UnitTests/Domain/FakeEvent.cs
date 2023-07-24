using System;

namespace DDD.Core.Domain
{
    public class FakeEvent : IEvent
    {

        #region Properties

        public DateTime OccurredOn { get; }

        #endregion Properties

    }
}
