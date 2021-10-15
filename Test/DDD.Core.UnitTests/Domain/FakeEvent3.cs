using System;

namespace DDD.Core.Domain
{
    public class FakeEvent3 : IEvent
    {
        #region Properties

        public DateTime OccurredOn => DateTime.Now;

        #endregion Properties
    }
}
