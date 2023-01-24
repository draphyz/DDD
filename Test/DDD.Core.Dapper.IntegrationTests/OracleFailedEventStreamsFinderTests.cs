using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleFailedEventStreamsFinderTests : FailedEventStreamsFinderTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedEventStreamsFinderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}