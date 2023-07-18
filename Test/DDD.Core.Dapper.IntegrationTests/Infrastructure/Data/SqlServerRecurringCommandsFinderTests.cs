using System;
using System.Collections.Generic;
using DDD.Core.Application;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("SqlServer")]
    public class SqlServerRecurringCommandsFinderTests : RecurringCommandsFinderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerRecurringCommandsFinderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IEnumerable<RecurringCommand> ExpectedResults()
        {
            yield return new RecurringCommand
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.Messages",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = "* * * * *",
                RecurringExpressionFormat = "CRON"
            };
            yield return new RecurringCommand
            {
                CommandId = new Guid("f7df5bd0-8763-677e-7e6b-3a0044746810"),
                CommandType = "DDD.Core.Application.FakeCommand2, DDD.Core.Messages",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10,\"Property3\":\"2022-12-24T14:49:44.361964+01:00\"}",
                BodyFormat = "JSON",
                RecurringExpression = "0  0 1 * *",
                RecurringExpressionFormat = "CRON"
            };
        }

        #endregion Methods

    }
}