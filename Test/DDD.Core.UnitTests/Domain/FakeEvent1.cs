using System;

namespace DDD.Core.Domain
{
    public class FakeEvent1 : IEvent
    {

        #region Constructors

        public FakeEvent1()
        {
            this.OccurredOn = SystemTime.Local();
        }

        #endregion Constructors

        #region Properties

        public DateTime OccurredOn { get; private set; }

        #endregion Properties

    }
}