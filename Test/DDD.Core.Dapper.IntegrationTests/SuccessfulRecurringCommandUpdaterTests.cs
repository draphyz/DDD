using FluentAssertions;
using Dapper;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using System.Data.Common;

    public abstract class SuccessfulRecurringCommandUpdaterTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected SuccessfulRecurringCommandUpdaterTests(TFixture fixture)
        {
            this.Fixture = fixture;
            this.ConnectionProvider = fixture.CreateConnectionProvider();
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
            this.Fixture.ExecuteScriptFromResources("MarkRecurringCommandAsSuccessful");
            var handler = new SuccessfulRecurringCommandUpdater<TestContext>(this.ConnectionProvider);
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
            this.Fixture.ExecuteScriptFromResources("MarkRecurringCommandAsSuccessful");
            var handler = new SuccessfulRecurringCommandUpdater<TestContext>(this.ConnectionProvider);
            var command = CreateCommand();
            var expectedCommand = ExpectedCommand();
            // Act
            await handler.HandleAsync(command);
            // Assert
            UpdatedCommand().Should().BeEquivalentTo(expectedCommand);

        }

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        private static MarkRecurringCommandAsSuccessful CreateCommand()
        {
            return new MarkRecurringCommandAsSuccessful
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                ExecutionTime = new DateTime(2022, 2, 1)
            };
        }

        private RecurringCommandDetail UpdatedCommand()
        {
            using (var connection = this.Fixture.CreateConnection())
            {
                connection.Open();
                var sql = "SELECT CommandId, CommandType, Body, BodyFormat, RecurringExpression, LastExecutionTime, CASE LastExecutionStatus WHEN 'F' THEN 'Successful' WHEN 'S' THEN 'Successful' END LastExecutionStatus, LastExceptionInfo FROM Command WHERE CommandId = @CommandId";
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
                LastExecutionTime = new DateTime(2022, 2, 1),
                LastExecutionStatus = CommandExecutionStatus.Successful,
                LastExceptionInfo = null
            };
        }

        #endregion Methods
    }
}
