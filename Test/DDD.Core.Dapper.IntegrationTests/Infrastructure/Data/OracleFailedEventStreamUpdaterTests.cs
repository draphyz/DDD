using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleFailedEventStreamUpdaterTests : FailedEventStreamUpdaterTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedEventStreamUpdaterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}