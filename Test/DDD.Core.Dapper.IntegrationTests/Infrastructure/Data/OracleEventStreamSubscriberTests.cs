using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleEventStreamSubscriberTests : EventStreamSubscriberTests<OracleFixture>
    {

        #region Constructors

        public OracleEventStreamSubscriberTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}