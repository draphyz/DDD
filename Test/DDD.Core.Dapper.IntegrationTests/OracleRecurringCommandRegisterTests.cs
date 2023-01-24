using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleRecurringCommandRegisterTests : RecurringCommandRegisterTests<OracleFixture>
    {

        #region Constructors

        public OracleRecurringCommandRegisterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}