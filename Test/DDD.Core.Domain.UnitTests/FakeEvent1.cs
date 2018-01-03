using System;

namespace DDD.Core.Domain
{
    public class FakeEvent1 : IDomainEvent
    {
        #region Properties

        public DateTime OccurredOn => DateTime.Now;

        #endregion Properties
    }
}