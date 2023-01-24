using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleFailedEventStreamExcluderTests : FailedEventStreamExcluderTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedEventStreamExcluderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}