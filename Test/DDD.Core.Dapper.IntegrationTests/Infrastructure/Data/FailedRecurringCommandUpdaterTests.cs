using FluentAssertions;
using Dapper;
using System;
using System.Threading.Tasks;
using System.Data.Common;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class FailedRecurringCommandUpdaterTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected FailedRecurringCommandUpdaterTests(TFixture fixture)
        {
            Fixture = fixture;
            ConnectionProvider = fixture.CreateConnectionProvider();
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }

        protected IDbConnectionProvider<TestContext> ConnectionProvider { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public void Handle_WhenCalled_UpdatesCommand()
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("MarkRecurringCommandAsFailed");
            var handler = new FailedRecurringCommandUpdater<TestContext>(ConnectionProvider);
            var command = CreateCommand();
            var expectedCommand = ExpectedCommand();
            // Act
            handler.Handle(command);
            // Assert
            UpdatedCommand().Should().BeEquivalentTo(expectedCommand);
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_UpdatesCommand()
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("MarkRecurringCommandAsFailed");
            var handler = new FailedRecurringCommandUpdater<TestContext>(ConnectionProvider);
            var command = CreateCommand();
            var expectedCommand = ExpectedCommand();
            // Act
            await handler.HandleAsync(command);
            // Assert
            UpdatedCommand().Should().BeEquivalentTo(expectedCommand);

        }

        public void Dispose()
        {
            ConnectionProvider.Dispose();
        }

        private static MarkRecurringCommandAsFailed CreateCommand()
        {
            return new MarkRecurringCommandAsFailed
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                ExecutionTime = new DateTime(2022, 1, 1),
                ExceptionInfo = "System.TimeoutException: The operation has timed-out."
            };
        }

        private RecurringCommandDetail UpdatedCommand()
        {
            using (var connection = Fixture.CreateConnection())
            {
                connection.Open();
                var sql = "SELECT CommandId, CommandType, Body, BodyFormat, RecurringExpression, LastExecutionTime, CASE LastExecutionStatus WHEN 'F' THEN 'Failed' WHEN 'S' THEN 'Successful' END LastExecutionStatus, LastExceptionInfo FROM Command WHERE CommandId = @CommandId";
                sql = sql.Replace("@", connection.Expressions().ParameterPrefix());
                return connection.QuerySingle<RecurringCommandDetail>(sql, Parameters(connection));
            }
        }

        private static object Parameters(DbConnection connection)
        {
            if (connection.HasOracleProvider())
                return new { CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345").ToByteArray() };
            return new { CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345") };
        }


        private static RecurringCommandDetail ExpectedCommand()
        {
            return new RecurringCommandDetail
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.Messages",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = "* * * * *",
                LastExecutionTime = new DateTime(2022, 1, 1),
                LastExecutionStatus = CommandExecutionStatus.Failed,
                LastExceptionInfo = "System.TimeoutException: The operation has timed-out."
            };
        }

        #endregion Methods
    }
}
