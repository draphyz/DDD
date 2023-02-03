using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleEventStreamsFinderTests : EventStreamsFinderTests<OracleFixture>
    {

        #region Constructors

        public OracleEventStreamsFinderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}