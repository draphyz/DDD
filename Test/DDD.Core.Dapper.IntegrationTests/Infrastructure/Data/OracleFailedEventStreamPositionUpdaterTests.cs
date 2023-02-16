using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleFailedEventStreamPositionUpdaterTests : FailedEventStreamPositionUpdaterTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedEventStreamPositionUpdaterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}