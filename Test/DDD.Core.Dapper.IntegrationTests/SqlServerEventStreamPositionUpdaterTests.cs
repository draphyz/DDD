using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerEventStreamPositionUpdaterTests : EventStreamPositionUpdaterTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerEventStreamPositionUpdaterTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}