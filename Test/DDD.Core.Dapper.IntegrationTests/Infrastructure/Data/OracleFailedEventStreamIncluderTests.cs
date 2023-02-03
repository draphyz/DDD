using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleFailedEventStreamIncluderTests : FailedEventStreamIncluderTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedEventStreamIncluderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}