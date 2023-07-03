﻿using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DDD.Core.Application
{
    using DependencyInjection;
    using Serialization;
    using Domain;
    using Threading;

    public class EventConsumer<TContext> : IEventConsumer<TContext>, IDisposable
        where TContext : BoundedContext
    {

        #region Fields

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ICommandProcessor commandProcessor;
        private readonly IQueryProcessor queryProcessor;
        private readonly IEventPublisher<TContext> eventPublisher;
        private readonly IEnumerable<BoundedContext> boundedContexts;
        private readonly IKeyedServiceProvider<SerializationFormat, ITextSerializer> eventSerializers;
        private readonly ILogger logger;
        private readonly EventConsumerSettings<TContext> settings;
        private long consumationCount;
        private bool disposed;
        private Task consumeEvents;

        #endregion Fields

        #region Constructors

        public EventConsumer(ICommandProcessor commandProcessor,
                             IQueryProcessor queryProcessor,
                             IEventPublisher<TContext> eventPublisher,
                             IEnumerable<BoundedContext> boundedContexts,
                             IKeyedServiceProvider<SerializationFormat, ITextSerializer> eventSerializers,
                             ILogger logger,
                             EventConsumerSettings<TContext> settings)
        {
            Ensure.That(commandProcessor, nameof(commandProcessor)).IsNotNull();
            Ensure.That(queryProcessor, nameof(queryProcessor)).IsNotNull();
            Ensure.That(eventPublisher, nameof(eventPublisher)).IsNotNull();
            Ensure.That(boundedContexts, nameof(boundedContexts)).IsNotNull();
            Ensure.Enumerable.HasItems(boundedContexts, nameof(boundedContexts));
            Ensure.That(eventSerializers, nameof(eventSerializers)).IsNotNull();
            Ensure.That(logger, nameof(logger)).IsNotNull();
            Ensure.That(settings, nameof(settings)).IsNotNull();
            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
            this.eventPublisher = eventPublisher;
            this.boundedContexts = boundedContexts;
            this.eventSerializers = eventSerializers;
            this.logger = logger;
            this.settings = settings;
        }

        #endregion Constructors

        #region Properties

        public TContext Context => this.settings.Context;

        public bool IsRunning { get; private set; }

        BoundedContext IEventConsumer.Context => this.Context;
        protected CancellationToken CancellationToken => this.cancellationTokenSource.Token;

        #endregion Properties

        #region Methods

        public void Start()
        {
            if (!this.IsRunning)
            {
                this.logger.LogInformation("EventConsumer for the context '{Context}' is starting.", this.Context.Name);
                this.logger.LogDebug("The consumer settings for the context '{Context}' are as follows : {@Settings}", this.Context.Name, this.settings);
                this.IsRunning = true;
                this.consumeEvents = Task.Run(async () => await ConsumeEventsAsync());
                this.logger.LogInformation("EventConsumer for the context '{Context}' has started.", this.Context.Name);
            }
        }

        public void Stop()
        {
            if (this.IsRunning)
            {
                this.logger.LogInformation("EventConsumer for the context '{Context}' is stopping.", this.Context.Name);
                this.cancellationTokenSource.Cancel();
                this.consumeEvents.Wait();
            }
        }

        public void Wait(TimeSpan? timeout = null)
        {
            if (this.IsRunning)
            {
                if (timeout.HasValue)
                    this.consumeEvents.Wait(timeout.Value);
                else
                    this.consumeEvents.Wait();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.Stop();
                    this.cancellationTokenSource.Dispose();
                }
                disposed = true;
            }
        }

        private static bool IsTransientException(Exception exception)
        {
            if (exception is TimestampedException ex)
                return ex.IsTransient;
            return false;
        }

        private async Task ConsumeEventsAsync()
        {
            try
            {
                await new SynchronizationContextRemover();
                while (!ConsumationMaxReached())
                {
                    this.CancellationToken.ThrowIfCancellationRequested();
                    this.logger.LogInformation("Consumption of event streams in the context '{Context}' has started.", this.Context.Name);
                    var streamsInfo = await this.FindAllStreamsAsync();
                    this.logger.LogInformation("{StreamsCount} event streams were found for the context '{Context}'.", streamsInfo.Streams.Count(), this.Context.Name);
                    this.logger.LogInformation("{FailedStreamsCount} failed event streams were found for the context '{Context}'.", streamsInfo.FailedStreams.Count(), this.Context.Name);
                    await this.ConsumeAllStreamsAsync(streamsInfo);
                    this.IncrementConsumationCountIfRequired();
                    this.logger.LogInformation("Consumption of event streams in the context '{Context}' has finished.", this.Context.Name);
                    await Task.Delay(this.settings.ConsumationDelay, this.CancellationToken);
                }
            }
            catch(OperationCanceledException)
            {
                this.logger.LogInformation("EventConsumer for the context '{Context}' has stopped.", this.Context.Name);
            }
            catch (Exception exception)
            {
                this.logger.LogCritical(default, exception, "An error occurred while consuming event streams in the context '{Context}'.", this.Context.Name);
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        private async Task<StreamsInfo> FindAllStreamsAsync()
        {
            var findStreams = this.queryProcessor.InGeneric(Context).ProcessAsync(new FindEventStreams(), MessageContext.CancellableContext(this.CancellationToken));
            var findFailedStreams = this.queryProcessor.InGeneric(Context).ProcessAsync(new FindFailedEventStreams(), MessageContext.CancellableContext(this.CancellationToken));
            await Task.WhenAll(findStreams, findFailedStreams);
            return new StreamsInfo(findStreams.Result, findFailedStreams.Result);
        }

        private async Task ConsumeAllStreamsAsync(StreamsInfo streamsInfo)
        {
            var tasks = new List<Task>();
            foreach (var stream in streamsInfo.Streams)
            {
                var excludedStreams = streamsInfo.FailedStreams
                                                 .Where(s => s.StreamSource == stream.Source && s.StreamType == stream.Type)
                                                 .ToList();
                tasks.Add(ConsumeStreamAsync(stream, excludedStreams));
            }
            await Task.WhenAll(tasks);
        }

        private async Task ConsumeStreamAsync(EventStream stream, List<FailedEventStream> excludedStreams)
        {
            await ConsumeExcludedStreamsAsync(stream, excludedStreams);
            await ConsumeStreamWithoutExcludedStreamsAsync(stream, excludedStreams);
        }

        private async Task ConsumeExcludedStreamsAsync(EventStream stream, List<FailedEventStream> excludedStreams)
        {
            var activeStreams = excludedStreams.Where(s => s.IsActive()).ToList();
            var tasks = new List<Task>();
            foreach (var excludedStream in activeStreams)
                tasks.Add(ConsumeExcludedStreamAsync(stream, excludedStream, excludedStreams));
            await Task.WhenAll(tasks);
        }

        private async Task ConsumeStreamWithoutExcludedStreamsAsync(EventStream stream, List<FailedEventStream> excludedStreams)
        {
            this.logger.LogInformation("Consumption of the following event stream in the context '{Context}' has started : {Stream}", this.Context.Name, stream);
            var query = new ReadEventStream
            {
                Top = stream.BlockSize,
                StreamPosition = stream.Position,
                StreamType = stream.Type,
                ExcludedStreamIds = excludedStreams.Select(s => s.StreamId).ToArray()
            };
            var sourceContext = this.boundedContexts.Single(c => c.Code == stream.Source);
            var notifiedEvents = await this.queryProcessor.InSpecific(sourceContext).ProcessAsync(query, MessageContext.CancellableContext(this.CancellationToken));
            foreach (var notifiedEvent in notifiedEvents)
            {
                this.CancellationToken.ThrowIfCancellationRequested();
                try
                {
                    this.logger.LogDebug("Handling of event {EventId} in the context '{Context}' has started.", notifiedEvent.EventId, this.Context.Name);
                    var @event = this.DeserializeEvent(notifiedEvent);
                    await this.eventPublisher.PublishAsync(@event, CreateContext(notifiedEvent, stream));
                    stream.Position = notifiedEvent.EventId;
                    this.logger.LogDebug("Handling of event {EventId} in the context '{Context}' has finished.", notifiedEvent.EventId, this.Context.Name);
                }
                catch (Exception exception) when (!(exception is OperationCanceledException))
                {
                    this.logger.LogError(default, exception, "An error occurred while handling event {EventId} in the context '{Context}'.", notifiedEvent.EventId, this.Context.Name);
                    var isTransientException = IsTransientException(exception);
                    var baseException = exception.GetBaseException();
                    var command = new ExcludeFailedEventStream
                    {
                        StreamPosition = stream.Position,
                        StreamId = notifiedEvent.StreamId,
                        StreamType = notifiedEvent.StreamType,
                        StreamSource = stream.Source,
                        EventId = notifiedEvent.EventId,
                        EventType = notifiedEvent.EventType,
                        ExceptionTime = SystemTime.Local(),
                        ExceptionType = exception.GetType().ShortAssemblyQualifiedName(),
                        ExceptionMessage = exception.Message,
                        ExceptionSource = exception.FullSource(),
                        ExceptionInfo = exception.ToString(),
                        BaseExceptionType = baseException.GetType().ShortAssemblyQualifiedName(),
                        BaseExceptionMessage = baseException.Message,
                        RetryCount = 0,
                        RetryMax = this.GetRetryMax(isTransientException, stream),
                        RetryDelays = this.GetRetryDelays(isTransientException, stream),
                        BlockSize = stream.BlockSize
                    };
                    await this.commandProcessor.InGeneric(Context).ProcessAsync(command);
                    break;
                }
            }
            this.logger.LogInformation("Consumption of the following event stream in the context '{Context}' has finished : {Stream}", this.Context.Name, stream);
        }

        private async Task ConsumeExcludedStreamAsync(EventStream stream, FailedEventStream excludedStream, List<FailedEventStream> excludedStreams)
        {
            this.logger.LogInformation("Consumption of the following failed event stream in the context '{Context}' has started : {FailedStream}", this.Context.Name, excludedStream);
            var query = new ReadFailedEventStream
            {
                Top = excludedStream.BlockSize,
                EventIdMin = excludedStream.EventId,
                EventIdMax = stream.Position,
                StreamType = excludedStream.StreamType,
                StreamId = excludedStream.StreamId
            }; 
            var sourceContext = this.boundedContexts.Single(c => c.Code == excludedStream.StreamSource);
            var notifiedEvents = await this.queryProcessor.InSpecific(sourceContext).ProcessAsync(query, MessageContext.CancellableContext(this.CancellationToken));
            var success = true;
            foreach (var notifiedEvent in notifiedEvents)
            {
                this.CancellationToken.ThrowIfCancellationRequested();
                try
                {
                    this.logger.LogDebug("Handling of event {EventId} in the context '{Context}' has started.", notifiedEvent.EventId, this.Context.Name);
                    var @event = this.DeserializeEvent(notifiedEvent);
                    await this.eventPublisher.PublishAsync(@event, CreateContext(notifiedEvent, excludedStream));
                    excludedStream.StreamPosition = notifiedEvent.EventId;
                    this.logger.LogDebug("Handling of event {EventId} in the context '{Context}' has finished.", notifiedEvent.EventId, this.Context.Name);
                }
                catch (Exception exception) when (!(exception is OperationCanceledException))
                {
                    success = false;
                    this.logger.LogError(default, exception, "An error occurred while handling event {EventId} in the context '{Context}'.", notifiedEvent.EventId, this.Context.Name);
                    var isTransientException = IsTransientException(exception);
                    var isNewEvent = notifiedEvent.EventId != excludedStream.EventId;
                    var baseException = exception.GetBaseException();
                    var command = new UpdateFailedEventStream
                    {
                        StreamPosition = excludedStream.StreamPosition,
                        StreamId = notifiedEvent.StreamId,
                        StreamType = notifiedEvent.StreamType,
                        StreamSource = excludedStream.StreamSource,
                        EventId = notifiedEvent.EventId,
                        EventType = notifiedEvent.EventType,
                        ExceptionTime = SystemTime.Local(),
                        ExceptionType = exception.GetType().ShortAssemblyQualifiedName(),
                        ExceptionMessage = exception.Message,
                        ExceptionSource = exception.FullSource(),
                        ExceptionInfo = exception.ToString(),
                        BaseExceptionType = baseException.GetType().ShortAssemblyQualifiedName(),
                        BaseExceptionMessage = baseException.Message,
                        RetryCount = isNewEvent ? (byte)0 : ++excludedStream.RetryCount,
                        RetryMax = isNewEvent ? this.GetRetryMax(isTransientException, stream) : excludedStream.RetryMax,
                        RetryDelays = isNewEvent ? this.GetRetryDelays(isTransientException, stream) : excludedStream.RetryDelays
                    };
                    await this.commandProcessor.InGeneric(Context).ProcessAsync(command);
                    break;
                }
            }
            if (success)
            {
                var command = new IncludeFailedEventStream
                {
                    Id = excludedStream.StreamId,
                    Type = excludedStream.StreamType,
                    Source = excludedStream.StreamSource
                };
                await this.commandProcessor.InGeneric(Context).ProcessAsync(command);
                excludedStreams.Remove(excludedStream);
            }
            this.logger.LogInformation("Consumption of the following failed event stream in the context '{Context}' has finished : {FailedStream}", this.Context.Name, excludedStream);
        }
        private bool ConsumationMaxReached()
        {
            if (!this.settings.ConsumationMax.HasValue) return false;
            return this.consumationCount >= this.settings.ConsumationMax;
        }

        private IMessageContext CreateContext(Event @event, FailedEventStream stream)
        {
            var context = MessageContext.CancellableContext(this.CancellationToken);
            context.AddEvent(@event);
            context.AddFailedStream(stream);
            return context;
        }

        private IMessageContext CreateContext(Event @event, EventStream stream)
        {
            var context = MessageContext.CancellableContext(this.CancellationToken);
            context.AddEvent(@event);
            context.AddStream(stream);
            return context;
        }

        private IEvent DeserializeEvent(Event notifiedEvent)
        {
            var format = (SerializationFormat)Enum.Parse(typeof(SerializationFormat), notifiedEvent.BodyFormat, ignoreCase: true);
            var type = Type.GetType(notifiedEvent.EventType);
            var serializer = this.eventSerializers.GetService(format);
            return (IEvent)serializer.DeserializeFromString(notifiedEvent.Body, type);
        }

        private ICollection<IncrementalDelay> GetRetryDelays(bool isTransientException, EventStream stream)
        {
            if (isTransientException)
                return stream.RetryDelays;
            return Array.Empty<IncrementalDelay>();
        }

        private byte GetRetryMax(bool isTransientException, EventStream stream)
        {
            if (isTransientException)
                return stream.RetryMax;
            return 0;
        }

        private void IncrementConsumationCountIfRequired()
        {
            if (this.settings.ConsumationMax.HasValue)
                this.consumationCount++;
        }

        #endregion Methods

        #region Classes

        private class StreamsInfo
        {

            #region Constructors

            public StreamsInfo(IEnumerable<EventStream> streams, IEnumerable<FailedEventStream> failedStreams)
            {
                this.Streams = streams;
                this.FailedStreams = failedStreams;
            }

            #endregion Constructors

            #region Properties

            public IEnumerable<EventStream> Streams { get; }

            public IEnumerable<FailedEventStream> FailedStreams { get; }

            #endregion Properties

        }

        #endregion Classes

    }
}
