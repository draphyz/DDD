using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleEventStreamPositionUpdaterTests : EventStreamPositionUpdaterTests<OracleFixture>
    {

        #region Constructors

        public OracleEventStreamPositionUpdaterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}