using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleEventWriterTests : EventWriterTests<OracleFixture>
    {

        #region Constructors

        public OracleEventWriterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}