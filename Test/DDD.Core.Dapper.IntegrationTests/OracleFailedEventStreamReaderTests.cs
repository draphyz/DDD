using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Application;

    [Collection("Oracle")]
    public class OracleFailedEventStreamReaderTests : EventsByStreamIdFinderTests<OracleFixture>
    {

        #region Constructors

        public OracleFailedEventStreamReaderTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<object[]> QueriesAndResults_2()
        {
            yield return new object[]
            {
                new ReadFailedEventStream
                {
                    StreamType = "Message",
                    StreamId = "5",
                    EventIdMin = new Guid("d9fdd908-9e0a-c80f-e72d-e94a0f7d4902"),
                    EventIdMax = new Guid("d9fdd908-9e0a-ca0f-7c1b-78288db70ee6")
                },
                new []
                {
                    new Event
                    {
                        Body = "{\"messageId\":5,\"occurredOn\":\"2022-01-20T14:01:41.3251974+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("d9fdd908-9e0a-c80f-e72d-e94a0f7d4902"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageRead, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 14, 1, 41, 325),
                        StreamId = "5",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":5,\"source\":\"Inbox\",\"destination\":\"Binbox\",\"occurredOn\":\"2022-01-20T14:01:48.6157105+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("d9fdd908-9e0a-c90f-3cfe-09c19f22f068"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageSentToBin, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 14, 1, 48, 616),
                        StreamId = "5",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    },
                    new Event
                    {
                        Body = "{\"messageId\":5,\"occurredOn\":\"2022-01-20T14:02:05.9149942+01:00\"}",
                        BodyFormat = "JSON",
                        EventId = new Guid("d9fdd908-9e0a-ca0f-7c1b-78288db70ee6"),
                        EventType = "DDD.Collaboration.Domain.Messages.MessageDeleted, DDD.Collaboration.Messages",
                        OccurredOn = new DateTime(2022, 1, 20, 14, 2, 5, 915),
                        StreamId = "5",
                        StreamType = "Message",
                        IssuedBy = "Dr Folamour"
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults_2))]
        public void Handle_WhenCalled_ReturnsExpectedResults_2(ReadFailedEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ReadFailedEventStream");
            var handler = new FailedEventStreamReader<TestContext>(this.ConnectionProvider);
            // Act
            var results = handler.Handle(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        [Theory]
        [MemberData(nameof(QueriesAndResults_2))]
        public async Task HandleAsync_WhenCalled_ReturnsExpectedResults_2(ReadFailedEventStream query, IEnumerable<Event> expectedResults)
        {
            // Arrange
            this.Fixture.ExecuteScriptFromResources("ReadFailedEventStream");
            var handler = new FailedEventStreamReader<TestContext>(this.ConnectionProvider);
            // Act
            var results = await handler.HandleAsync(query);
            // Assert
            results.Should().BeEquivalentTo(expectedResults, context => context.WithStrictOrdering());
        }

        #endregion Methods

    }
}