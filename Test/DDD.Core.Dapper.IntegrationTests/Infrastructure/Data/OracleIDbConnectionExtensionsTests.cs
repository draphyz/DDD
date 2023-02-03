using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleIDbConnectionExtensionsTests : IDbConnectionExtensionsTests<OracleFixture>
    {

        #region Constructors

        public OracleIDbConnectionExtensionsTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}