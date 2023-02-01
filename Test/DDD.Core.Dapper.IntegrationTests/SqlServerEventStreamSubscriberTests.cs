using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerEventStreamSubscriberTests : EventStreamSubscriberTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerEventStreamSubscriberTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}