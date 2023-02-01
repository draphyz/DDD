using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public abstract class RecurringCommandsFinderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected RecurringCommandsFinderTests(TFixture fixture)
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
        public void Handle_WhenCalled_ReturnsExpectedResults()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("FindRecurringCommands");
            var query = new FindRecurringCommands();
            var expectedResults = ExpectedResults();
            var handler = new RecurringCommandsFinder<TestContext>(this.ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults()
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("FindRecurringCommands");
            var query = new FindRecurringCommands();
            var expectedResults = ExpectedResults();
            var handler = new RecurringCommandsFinder<TestContext>(this.ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        protected abstract IEnumerable<RecurringCommand> ExpectedResults();

        #endregion Methods
    }
}
