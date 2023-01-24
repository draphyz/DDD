using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleSuccessfulRecurringCommandUpdaterTests : SuccessfulRecurringCommandUpdaterTests<OracleFixture>
    {

        #region Constructors

        public OracleSuccessfulRecurringCommandUpdaterTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}