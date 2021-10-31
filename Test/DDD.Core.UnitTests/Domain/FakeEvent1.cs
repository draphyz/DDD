using System;

namespace DDD.Core.Domain
{
    public class FakeEvent1 : IEvent
    {
        #region Properties

        public DateTime OccurredOn => SystemTime.Local();

        #endregion Properties
    }
}