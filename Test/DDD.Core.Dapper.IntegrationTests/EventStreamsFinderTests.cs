using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    public abstract class EventStreamsFinderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected EventStreamsFinderTests(TFixture fixture)
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
            var query = new FindEventStreams();
            var expectedResults = ExpectedResults();
            this.Fixture.ExecuteScriptFromResources("FindEventStreams");
            var handler = new EventStreamsFinder<TestContext>(this.ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults()
        {
            // Arrange
            var query = new FindEventStreams();
            var expectedResults = ExpectedResults();
            this.Fixture.ExecuteScriptFromResources("FindEventStreams");
            var handler = new EventStreamsFinder<TestContext>(this.ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        public void Dispose()
        {
            this.ConnectionProvider.Dispose();
        }

        private static IEnumerable<EventStream> ExpectedResults()
        {
            yield return new EventStream
            {
                Type = "Person",
                Source = "ID",
                Position = new Guid("f7df5bd0-8763-677e-7e6b-3a0044746810"),
                RetryMax = 5,
                RetryDelays = new[]
                        {
                            new IncrementalDelay { Delay = 10 },
                            new IncrementalDelay { Delay = 60 },
                            new IncrementalDelay { Delay = 120, Increment = 80 }
                        },
                BlockSize = 50
            };
            yield return new EventStream
            {
                Type = "MedicalProduct",
                Source = "OFR",
                Position = Guid.Empty,
                RetryMax = 3,
                RetryDelays = new[]
                {
                            new IncrementalDelay {  Delay = 60 }
                        },
                BlockSize = 100
            };
        }

        #endregion Methods
    }
}
