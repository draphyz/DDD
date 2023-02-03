using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    public abstract class FailedEventStreamsFinderTests<TFixture> : IDisposable
        where TFixture : IPersistenceFixture
    {

        #region Constructors

        protected FailedEventStreamsFinderTests(TFixture fixture)
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
        public void Handle_WhenCalled_ReturnsExpectedResults()
        {
            // Arrange
            var query = new FindFailedEventStreams();
            var expectedResults = ExpectedResults();
            Fixture.ExecuteScriptFromResources("FindFailedEventStreams");
            var handler = new FailedEventStreamsFinder<TestContext>(ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        [Fact]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults()
        {
            // Arrange
            var query = new FindFailedEventStreams();
            var expectedResults = ExpectedResults();
            Fixture.ExecuteScriptFromResources("FindFailedEventStreams");
            var handler = new FailedEventStreamsFinder<TestContext>(ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        public void Dispose()
        {
            ConnectionProvider.Dispose();
        }

        private static IEnumerable<FailedEventStream> ExpectedResults()
        {
            yield return new FailedEventStream
            {
                StreamPosition = new Guid("0a77707a-c147-9e1b-883a-08da0e368663"),
                StreamId = "2",
                StreamType = "MessageBox",
                StreamSource = "COL",
                EventId = new Guid("e10add4d-1851-7ede-883b-08da0e368663"),
                ExceptionTime = new DateTime(2021, 11, 19),
                RetryCount = 0,
                RetryMax = 5,
                RetryDelays = new[]
                        {
                            new IncrementalDelay{ Delay = 10 },
                            new IncrementalDelay{ Delay = 60 },
                            new IncrementalDelay{ Delay = 360 }
                        },
                BlockSize = 100
            };
        }

        #endregion Methods
    }
}
