using System;

namespace DDD.Core.Domain
{
    public class FakeEvent3 : IEvent
    {
        #region Properties

        public DateTime OccurredOn => SystemTime.Local();

        #endregion Properties
    }
}
