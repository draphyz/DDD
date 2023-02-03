using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;

    [Collection("SqlServer")]
    public class SqlServerEventStreamReaderTests : EventStreamReaderTests<SqlServerFixture>
    {

        #region Constructors

        public SqlServerEventStreamReaderTests(SqlServerFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<object[]> QueriesAndResults_2()
        {
            yield return new object[]
            {
                new ReadEventStream
                {
                    StreamType = "MessageBox",
                    StreamPosition = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345")
                },
                new []
                {
                    new Event
                    {
                        Body = "{\"boxId\":3,\"occurredOn\":\"2022-01-19T14:22:00.3683143+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("097d449e-539e-bdf1-b50d-3a0184ad3555"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageBoxCreated, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 19, 14, 22, 0, 368),
                        StreamId = "3",
                        StreamType = "MessageBox",
                        IssuedBy = "Dr Folamour"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults_2))]
        public void Handle_WhenCalled_ReturnsExpectedResults_2(ReadEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("ReadEventStream");
            var handler = new EventStreamReader<TestContext>(ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults_2))]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults_2(ReadEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            Fixture.ExecuteScriptFromResources("ReadEventStream");
            var handler = new EventStreamReader<TestContext>(ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        #endregion Methods

    }
}