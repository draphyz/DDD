using System;

namespace DDD.Core.Domain
{
    public class FakeEvent3 : IDomainEvent
    {
        #region Properties

        public DateTime OccurredOn => DateTime.Now;

        #endregion Properties
    }
}
