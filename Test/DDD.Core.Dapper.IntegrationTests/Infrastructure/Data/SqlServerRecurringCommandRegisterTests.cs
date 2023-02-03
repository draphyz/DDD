using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerRecurringCommandRegisterTests : RecurringCommandRegisterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerRecurringCommandRegisterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

    }
}