using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace DDD.Core.Application
{
    using DependencyInjection;
    using Serialization;
    using Domain;
    using Infrastructure.Testing;

    public class EventConsumerTests : IDisposable
    {

        #region Fields

        private EventConsumer<FakeContext> consumer;

        #endregion Fields

        #region Methods

        public void Dispose()
        {
            this.consumer?.Dispose();
        }

        [Fact]
        public void Start_WhenIsNotRunning_SetsIsRunningToTrue()
        {
            // Arrange
            var @event = FakeEvent();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            // Assert
            consumer.IsRunning.Should().BeTrue();
        }

        [Fact]
        public void Stop_WhenIsRunning_SetsIsRunningToFalse()
        {
            // Arrange
            var @event = FakeEvent();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            consumer.Start();
            // Act
            consumer.Stop();
            // Assert
            consumer.IsRunning.Should().BeFalse();
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInDeserializingEvent_ExcludesFailedEventStream()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializersThrowingException(exception);
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>()
                                  .Received(1)
                                  .ProcessAsync(Arg.Is<ExcludeFailedEventStream>(c => c.EventId == @event.EventId && c.ExceptionMessage == exception.Message), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void StartAndWait_WhenExceptionInDeserializingEvent_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializersThrowingException(exception);
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInDeserializingFailedEvent_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializersThrowingException(exception);
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInDeserializingFailedEvent_UpdatesFailedEventStream()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializersThrowingException(exception);
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>()
                                  .Received(1)
                                  .ProcessAsync(Arg.Is<UpdateFailedEventStream>(c => c.EventId == @event.EventId && c.ExceptionMessage == exception.Message), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void StartAndWait_WhenExceptionInExcludingFailedEventStream_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenExcludingFailedEventStream(exception);
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisherThrowingException(FakeException());
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInFindingEventStreams_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorThrowingExceptionWhenFindingEventStreams(exception);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInFindingFailedEventStreams_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorThrowingExceptionWhenFindingFailedEventStreams(exception);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInIncludingFailedEventStream_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenIncludingFailedEventStream(exception);
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInPublishingEvent_ExcludesFailedEventStream()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisherThrowingException(exception);
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>()
                                  .Received(1)
                                  .ProcessAsync(Arg.Is<ExcludeFailedEventStream>(c => c.EventId == @event.EventId && c.ExceptionMessage == exception.Message), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void StartAndWait_WhenExceptionInPublishingEvent_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingEventStream(@event);
            var eventPublisher = FakeEventPublisherThrowingException(exception);
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInPublishingFailedEvent_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisherThrowingException(exception);
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInPublishingFailedEvent_UpdatesFailedEventStream()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisherThrowingException(exception);
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>()
                                  .Received(1)
                                  .ProcessAsync(Arg.Is<UpdateFailedEventStream>(c => c.EventId == @event.EventId && c.ExceptionMessage == exception.Message), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void StartAndWait_WhenExceptionInReadingEventStream_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorThrowingExceptionWhenReadingEventStream(exception);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInReadingFailedEventStream_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorThrowingExceptionWhenReadingFailedEventStream(exception);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInUpdatingFailedEventStream_LogsException()
        {
            // Arrange
            var @event = FakeEvent();
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenUpdatingFailedEventStream(exception);
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisherThrowingException(FakeException());
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher,boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public async Task StartAndWait_WhenFailedEventStreamSuccessfullyProcessed_DeletesFailedEventStream()
        {
            // Arrange
            var @event = FakeEvent();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorReadingFailedEventStream(@event);
            var eventPublisher = FakeEventPublisher();
            var boundedContexts = FakeBoundedContexts();
            var eventSerializers = FakeEventSerializers();
            var logger = FakeLogger();
            var settings = FakeSettings();
            consumer = new EventConsumer<FakeContext>(commandProcessor, queryProcessor, eventPublisher, boundedContexts, eventSerializers, logger, settings);
            // Act
            consumer.Start();
            consumer.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>().Received(1)
                                  .ProcessAsync(Arg.Any<IncludeFailedEventStream>(), Arg.Any<IMessageContext>());
        }
        private static ICommandProcessor FakeCommandProcessor()
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenExcludingFailedEventStream(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<ExcludeFailedEventStream>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenIncludingFailedEventStream(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<IncludeFailedEventStream>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenUpdatingFailedEventStream(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<UpdateFailedEventStream>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static Event FakeEvent()
        {
            return new Event
            {
                EventId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                EventType = "DDD.Core.Domain.FakeEvent1, DDD.Core.UnitTests",
                OccurredOn = new DateTime(2021, 11, 18, 10, 1, 23, 528),
                Body = "{\"occurredOn\":\"2021-11-18T10:01:23.5277314+01:00\"}",
                BodyFormat = "Json",
                StreamId = "1",
                StreamType = "FakeStream",
                IssuedBy = "Dr Maboul"
            };
        }

        private static IEventPublisher FakeEventPublisher() => Substitute.For<IEventPublisher>();

        private static IEventPublisher FakeEventPublisherThrowingException(Exception exception)
        {
            var eventPublisher = Substitute.For<IEventPublisher>();
            eventPublisher.When(p => p.PublishAsync(Arg.Any<IEvent>(), Arg.Any<IMessageContext>()))
                          .Throw(exception);
            return eventPublisher;
        }

        private static IKeyedServiceProvider<SerializationFormat, ITextSerializer> FakeEventSerializers()
        {
            var jsonSerializer = Substitute.For<ITextSerializer>();
            jsonSerializer.Encoding.Returns(JsonSerializationOptions.Encoding);
            jsonSerializer.Deserialize(Arg.Any<Stream>(), Arg.Any<Type>()).Returns(Substitute.For<IEvent>());
            var provider = Substitute.For<IKeyedServiceProvider<SerializationFormat, ITextSerializer>>();
            provider.GetService(Arg.Is<SerializationFormat>(f => f == SerializationFormat.Json)).Returns(jsonSerializer);
            return provider;
        }

        private static IKeyedServiceProvider<SerializationFormat, ITextSerializer> FakeEventSerializersThrowingException(Exception exception)
        {
            var jsonSerializer = Substitute.For<ITextSerializer>();
            jsonSerializer.Encoding.Returns(JsonSerializationOptions.Encoding);
            jsonSerializer.When(s => s.Deserialize(Arg.Any<Stream>(), Arg.Any<Type>())).Throw(exception);
            var provider = Substitute.For<IKeyedServiceProvider<SerializationFormat, ITextSerializer>>();
            provider.GetService(Arg.Is<SerializationFormat>(f => f == SerializationFormat.Json)).Returns(jsonSerializer);
            return provider;
        }

        private static Exception FakeException() => new Exception("Fake exception.");

        private static ILogger FakeLogger() => Substitute.For<ILogger>();

        private static IQueryProcessor FakeQueryProcessorReadingEventStream(params Event[] events)
        {
            var contextualProcessorForFakeContext = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessorForFakeContext.ProcessAsync(Arg.Any<FindEventStreams>(), Arg.Any<IMessageContext>())
                                             .Returns(new[] { new EventStream { Source = "FKS" } });
            var contextualProcessorForFakeSourceContext = Substitute.For<IContextualQueryProcessor<FakeSourceContext>>();
            contextualProcessorForFakeSourceContext.ProcessAsync(Arg.Any<ReadEventStream>(), Arg.Any<IMessageContext>())
                                                   .Returns(events);
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessorForFakeContext);
            queryProcessor.In(Arg.Any<BoundedContext>()).Returns(contextualProcessorForFakeSourceContext);
            return queryProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorReadingFailedEventStream(params Event[] events)
        {
            var contextualProcessorForFakeContext = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessorForFakeContext.ProcessAsync(Arg.Any<FindEventStreams>(), Arg.Any<IMessageContext>())
                                             .Returns(new[] { new EventStream { Source = "FKS" } });
            contextualProcessorForFakeContext.ProcessAsync(Arg.Any<FindFailedEventStreams>(), Arg.Any<IMessageContext>())
                                             .Returns(new[] { new FailedEventStream { StreamSource = "FKS", RetryMax = 5, RetryDelays = new[] { new IncrementalDelay { Delay = 10 } } } });
            var contextualProcessorForFakeSourceContext = Substitute.For<IContextualQueryProcessor<FakeSourceContext>>();
            contextualProcessorForFakeSourceContext.ProcessAsync(Arg.Any<ReadFailedEventStream>(), Arg.Any<IMessageContext>())
                                                   .Returns(events);
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessorForFakeContext);
            queryProcessor.In(Arg.Any<BoundedContext>()).Returns(contextualProcessorForFakeSourceContext);
            return queryProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorThrowingExceptionWhenFindingEventStreams(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<FindEventStreams>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessor);
            return queryProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorThrowingExceptionWhenFindingFailedEventStreams(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<FindFailedEventStreams>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessor);
            return queryProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorThrowingExceptionWhenReadingEventStream(Exception exception)
        {
            var contextualProcessorForFakeContext = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessorForFakeContext.ProcessAsync(Arg.Any<FindEventStreams>(), Arg.Any<IMessageContext>())
                                             .Returns(new[] { new EventStream { Source = "FKS" } });
            var contextualProcessorForFakeSourceContext = Substitute.For<IContextualQueryProcessor<FakeSourceContext>>();
            contextualProcessorForFakeSourceContext.When(p => p.ProcessAsync(Arg.Any<ReadEventStream>(), Arg.Any<IMessageContext>()))
                                                   .Throw(exception);
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessorForFakeContext);
            queryProcessor.In(Arg.Any<BoundedContext>()).Returns(contextualProcessorForFakeSourceContext);
            return queryProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorThrowingExceptionWhenReadingFailedEventStream(Exception exception)
        {
            var contextualProcessorForFakeContext = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessorForFakeContext.ProcessAsync(Arg.Any<FindEventStreams>(), Arg.Any<IMessageContext>())
                                             .Returns(new[] { new EventStream { Source = "FKS" } });
            contextualProcessorForFakeContext.ProcessAsync(Arg.Any<FindFailedEventStreams>(), Arg.Any<IMessageContext>())
                                             .Returns(new[] { new FailedEventStream { StreamSource = "FKS", RetryMax = 5, RetryDelays = new[] { new IncrementalDelay { Delay = 10 } } } });
            var contextualProcessorForFakeSourceContext = Substitute.For<IContextualQueryProcessor<FakeSourceContext>>();
            contextualProcessorForFakeSourceContext.When(p => p.ProcessAsync(Arg.Any<ReadFailedEventStream>(), Arg.Any<IMessageContext>()))
                                                   .Throw(exception);
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessorForFakeContext);
            queryProcessor.In(Arg.Any<BoundedContext>()).Returns(contextualProcessorForFakeSourceContext);
            return queryProcessor;
        }

        private static EventConsumerSettings<FakeContext> FakeSettings() => new EventConsumerSettings<FakeContext>(1, 1);

        private static IEnumerable<BoundedContext> FakeBoundedContexts()
        {
            yield return new FakeContext();
            yield return new FakeSourceContext();
        }

        private static bool IsExpectedLogCall(ICall call, LogLevel level, Exception exception)
        {
            if (call.GetMethodInfo().Name != "Log") return false;
            var args = call.GetArguments();
            if ((LogLevel)args[0] != level) return false;
            if (args[3] != exception) return false;
            return true;
        }

        #endregion Methods

    }
}
