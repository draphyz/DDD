using System;

namespace DDD.Core.Infrastructure.DependencyInjection
{
    using Domain;

    public class FakeEvent : IEvent
    {

        #region Properties

        public DateTime OccurredOn { get; }

        #endregion Properties

    }
}
