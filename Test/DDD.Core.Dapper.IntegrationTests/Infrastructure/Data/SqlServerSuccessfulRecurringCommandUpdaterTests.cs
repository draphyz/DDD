using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerSuccessfulRecurringCommandUpdaterTests : SuccessfulRecurringCommandUpdaterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerSuccessfulRecurringCommandUpdaterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}