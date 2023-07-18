using FluentAssertions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class RecurringCommandRegisterTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected RecurringCommandRegisterTests(TFixture fixture)
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
        public void Handle_WhenCalled_RegistersCommand()
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("RegisterRecurringCommand");
            var handler = new RecurringCommandRegister<TestContext>(ConnectionProvider);
            var command = CreateCommand();
            var expectedCommands = ExpectedCommands();
            // Act
            handler.Handle(command);
            // Assert
            RegistredCommands().Should().BeEquivalentTo(expectedCommands);
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_RegistersCommand()
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("RegisterRecurringCommand");
            var handler = new RecurringCommandRegister<TestContext>(ConnectionProvider);
            var command = CreateCommand();
            var expectedCommands = ExpectedCommands();
            // Act
            await handler.HandleAsync(command);
            // Assert
            RegistredCommands().Should().BeEquivalentTo(expectedCommands);
        }

        public void Dispose()
        {
            ConnectionProvider.Dispose();
        }

        private static RegisterRecurringCommand CreateCommand()
        {
            return new RegisterRecurringCommand
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.Messages",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = "* * * * *",
                RecurringExpressionFormat = "CRON"
            };
        }

        private IEnumerable<RecurringCommand> RegistredCommands()
        {
            using (var connection = Fixture.CreateConnection())
            {
                connection.Open();
                return connection.Query<RecurringCommand>("SELECT CommandId, CommandType, Body, BodyFormat, RecurringExpression, RecurringExpressionFormat FROM Command");
            }
        }

        private static IEnumerable<RecurringCommand> ExpectedCommands()
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
        }

        #endregion Methods
    }
}
