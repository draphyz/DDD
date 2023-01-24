using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleFailedRecurringCommandUpdaterTests : FailedRecurringCommandUpdaterTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedRecurringCommandUpdaterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}