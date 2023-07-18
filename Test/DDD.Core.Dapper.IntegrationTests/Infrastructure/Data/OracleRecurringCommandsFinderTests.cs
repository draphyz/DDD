using System;
using System.Collections.Generic;
using DDD.Core.Application;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    [Collection("Oracle")]
    public class OracleRecurringCommandsFinderTests : RecurringCommandsFinderTests<OracleFixture>
    {

        #region Constructors

        public OracleRecurringCommandsFinderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override IEnumerable<RecurringCommand> ExpectedResults()
        {
            yield return new RecurringCommand
            {
                CommandId = new Guid("d9fdd908-9d0a-b6e8-0802-5680bf57bb60"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.Messages",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = "* * * * *",
                RecurringExpressionFormat = "CRON"
            };
            yield return new RecurringCommand
            {
                CommandId = new Guid("d9fdd908-9e0a-bd0f-9ec5-ae08e4f34ff4"),
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