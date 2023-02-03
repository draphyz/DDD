using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerFailedRecurringCommandUpdaterTests : FailedRecurringCommandUpdaterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerFailedRecurringCommandUpdaterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}