using Microsoft.Extensions.Logging;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using System.Threading.Tasks;
using System;
using System.IO;
using Xunit;

namespace DDD.Core.Application
{
    using DependencyInjection;
    using Serialization;
    using Infrastructure.Testing;
    using DDD.Core.Domain;

    public class RecurringCommandManagerTests : IDisposable
    {

        #region Fields

        private RecurringCommandManager<FakeContext> manager;

        #endregion Fields

        #region Methods

        public void Dispose()
        {
            this.manager?.Dispose();
        }

        [Fact]
        public async Task RegisterAsync_WhenRecurringExpressionIsInvalid_ThrowsArgumentException()
        {
            // Arrange
            var command = FakeCommand1();
            var recurringExpression = InvalidRecurringExpression();
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            Func<Task> registerAsync = async () => await manager.RegisterAsync(command, recurringExpression);
            // Assert
            await registerAsync.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void Start_WhenIsNotRunning_SetsIsRunningToTrue()
        {
            // Arrange
            var recurringCommand = FakeCommand1ForRecurringExecution();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            // Assert
            manager.IsRunning.Should().BeTrue();
        }

        [Fact]
        public void StartAndWait_WhenExceptionInDeserializingRecurringCommand_LogsException()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializersThrowingException(exception);
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInFindingRecurringCommands_LogsException()
        {
            // Arrange
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorThrowingExceptionWhenFindingRecurringCommands(exception);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public void StartAndWait_WhenExceptionInFindingRecurringCommands_SetsIsRunningToFalse()
        {
            // Arrange
            var exception = FakeException();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorThrowingExceptionWhenFindingRecurringCommands(exception);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            manager.IsRunning.Should().BeFalse();
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInMarkingRecurringCommandAsFailed_DoesNotRetryRecurringCommandOnNextOccurrence()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForRecurringExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenMarkingRecurringCommandAsFailed(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.Received(1)
                                  .ProcessAsync(Arg.Any<ICommand>(), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void StartAndWait_WhenExceptionInMarkingRecurringCommandAsFailed_LogsException()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenMarkingRecurringCommandAsFailed(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInMarkingRecurringCommandAsSuccessful_DoesNotRetryRecurringCommandOnNextOccurrence()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForRecurringExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenMarkingRecurringCommandAsSuccessful(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.Received(1)
                                  .ProcessAsync(Arg.Any<ICommand>(), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void StartAndWait_WhenExceptionInMarkingRecurringCommandAsSuccessful_LogsException()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenMarkingRecurringCommandAsSuccessful(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }
        [Fact]
        public void StartAndWait_WhenExceptionInProcessingRecurringCommand_LogsException()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenProcessingRecurringCommand(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            logger.Received(c => IsExpectedLogCall(c, LogLevel.Error, exception));
        }
        [Fact]
        public async Task StartAndWait_WhenExceptionInProcessingRecurringCommand_MarksRecurringCommandAsFailed()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenProcessingRecurringCommand(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>()
                                  .Received(1)
                                  .ProcessAsync(Arg.Is<MarkRecurringCommandAsFailed>(c => c.CommandId == recurringCommand.CommandId && c.ExceptionInfo == exception.ToString()), Arg.Any<IMessageContext>());
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInProcessingRecurringCommand_RetriesRecurringCommandOnNextOccurrence()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand = FakeCommand1ForDoubleExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenProcessingRecurringCommand(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.Received(2)
                                  .ProcessAsync(Arg.Any<ICommand>(), Arg.Any<IMessageContext>());
        }

        [Fact]
        public async Task StartAndWait_WhenExceptionInProcessingSpecificRecurringCommand_ContinuesToProcessOtherCommands()
        {
            // Arrange
            var exception = FakeException();
            var recurringCommand1 = FakeCommand1ForDoubleExecution();
            var recurringCommand2 = FakeCommand2ForDoubleExecution();
            var commandProcessor = FakeCommandProcessorThrowingExceptionWhenProcessingFakeCommand1(exception);
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand1, recurringCommand2);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.Received(2)
                                  .ProcessAsync(Arg.Any<FakeCommand2>(), Arg.Any<IMessageContext>());
        }


        [Fact]
        public async Task StartAndWait_WhenRecurringCommandSuccessfullyProcessed_MarksRecurringCommandAsSuccessful()
        {
            // Arrange
            var recurringCommand = FakeCommand1ForSingleExecution();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            // Act
            manager.Start();
            manager.Wait(TimeSpan.FromSeconds(5));
            // Assert
            await commandProcessor.In<FakeContext>()
                                  .Received(1)
                                  .ProcessAsync(Arg.Is<MarkRecurringCommandAsSuccessful>(c => c.CommandId == recurringCommand.CommandId), Arg.Any<IMessageContext>());
        }

        [Fact]
        public void Stop_WhenIsRunning_SetsIsRunningToFalse()
        {
            // Arrange
            var recurringCommand = FakeCommand1ForRecurringExecution();
            var commandProcessor = FakeCommandProcessor();
            var queryProcessor = FakeQueryProcessorFindingRecurringCommands(recurringCommand);;
            var commandSerializers = FakeCommandSerializers();
            var recurringScheduleFactory = FakeRecurringScheduleFactory();
            var logger = FakeLogger();
            var settings = FakeSettings();
            manager = new RecurringCommandManager<FakeContext>(commandProcessor, queryProcessor, commandSerializers, recurringScheduleFactory, logger, settings);
            manager.Start();
            // Act
            manager.Stop();
            // Assert
            manager.IsRunning.Should().BeFalse();
        }

        private static string RecurringExpressionForDoubleExecution() => "0,1 0 0 1 1";

        private static string RecurringExpressionForRecurringExecution() => "* * * * *";

        private static string RecurringExpressionForSingleExecution() => "0 0 0 1 1";

        private static ICommand FakeCommand1() => new FakeCommand1();

        private static RecurringCommand FakeCommand1ForDoubleExecution()
        {
            return new RecurringCommand
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.UnitTests",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = RecurringExpressionForDoubleExecution()
            };
        }

        private static RecurringCommand FakeCommand1ForRecurringExecution()
        {
            return new RecurringCommand
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.UnitTests",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = RecurringExpressionForRecurringExecution()
            };
        }

        private static RecurringCommand FakeCommand1ForSingleExecution()
        {
            return new RecurringCommand
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand1, DDD.Core.UnitTests",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = RecurringExpressionForSingleExecution()
            };
        }

        private static ICommand FakeCommand2() => new FakeCommand2();

        private static RecurringCommand FakeCommand2ForDoubleExecution()
        {
            return new RecurringCommand
            {
                CommandId = new Guid("36beb37d-1e01-bb7d-fb2a-3a0044745345"),
                CommandType = "DDD.Core.Application.FakeCommand2, DDD.Core.UnitTests",
                Body = "{\"Property1\":\"dummy\",\"Property2\":10}",
                BodyFormat = "JSON",
                RecurringExpression = RecurringExpressionForDoubleExecution()
            };
        }
        private static Guid FakeCommandId() => new Guid("f7df5bd0-8763-677e-7e6b-3a0044746810");

        private static IKeyedServiceProvider<SerializationFormat, ITextSerializer> FakeCommandSerializers()
        {
            var jsonSerializer = Substitute.For<ITextSerializer>();
            jsonSerializer.Encoding.Returns(JsonSerializationOptions.Encoding);
            jsonSerializer.Deserialize(Arg.Any<Stream>(), Arg.Is(typeof(FakeCommand1))).Returns(FakeCommand1());
            jsonSerializer.Deserialize(Arg.Any<Stream>(), Arg.Is(typeof(FakeCommand2))).Returns(FakeCommand2());
            var provider = Substitute.For<IKeyedServiceProvider<SerializationFormat, ITextSerializer>>();
            provider.GetService(Arg.Is<SerializationFormat>(f => f == SerializationFormat.Json)).Returns(jsonSerializer);
            return provider;
        }

        private static IKeyedServiceProvider<SerializationFormat, ITextSerializer> FakeCommandSerializersThrowingException(Exception exception)
        {
            var jsonSerializer = Substitute.For<ITextSerializer>();
            jsonSerializer.Encoding.Returns(JsonSerializationOptions.Encoding);
            jsonSerializer.When(s => s.Deserialize(Arg.Any<Stream>(), Arg.Any<Type>())).Throw(exception);
            var provider = Substitute.For<IKeyedServiceProvider<SerializationFormat, ITextSerializer>>();
            provider.GetService(Arg.Is<SerializationFormat>(f => f == SerializationFormat.Json)).Returns(jsonSerializer);
            return provider;
        }

        private static ICommandProcessor FakeCommandProcessor()
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenMarkingRecurringCommandAsFailed(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<MarkRecurringCommandAsFailed>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.When(p => p.ProcessAsync(Arg.Any<ICommand>(), Arg.Any<IMessageContext>()))
                            .Throw(FakeException());
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenMarkingRecurringCommandAsSuccessful(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<MarkRecurringCommandAsSuccessful>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenProcessingRecurringCommand(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.When(p => p.ProcessAsync(Arg.Any<ICommand>(), Arg.Any<IMessageContext>()))
                            .Throw(exception);
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static ICommandProcessor FakeCommandProcessorThrowingExceptionWhenProcessingFakeCommand1(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualCommandProcessor<FakeContext>>();
            var commandProcessor = Substitute.For<ICommandProcessor>();
            commandProcessor.When(p => p.ProcessAsync(Arg.Any<FakeCommand1>(), Arg.Any<IMessageContext>()))
                            .Throw(exception);
            commandProcessor.In<FakeContext>().Returns(contextualProcessor);
            return commandProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorFindingRecurringCommands(params RecurringCommand[] recurringCommands)
        {
            var contextualProcessor = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessor.ProcessAsync(Arg.Any<FindRecurringCommands>(), Arg.Any<IMessageContext>())
                               .Returns(recurringCommands);
            contextualProcessor.ProcessAsync(Arg.Any<GenerateRecurringCommandId>(), Arg.Any<IMessageContext>())
                               .Returns(FakeCommandId());
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessor);
            return queryProcessor;
        }

        private static IQueryProcessor FakeQueryProcessorThrowingExceptionWhenFindingRecurringCommands(Exception exception)
        {
            var contextualProcessor = Substitute.For<IContextualQueryProcessor<FakeContext>>();
            contextualProcessor.When(p => p.ProcessAsync(Arg.Any<FindRecurringCommands>(), Arg.Any<IMessageContext>()))
                               .Throw(exception);
            contextualProcessor.ProcessAsync(Arg.Any<GenerateRecurringCommandId>(), Arg.Any<IMessageContext>())
                               .Returns(FakeCommandId());
            var queryProcessor = Substitute.For<IQueryProcessor>();
            queryProcessor.In<FakeContext>().Returns(contextualProcessor);
            return queryProcessor;
        }

        private static IRecurringScheduleFactory FakeRecurringScheduleFactory()
        {
            var recurringScheduleForSingleExecution1 = FakeRecurringScheduleForSingleExecution();
            var recurringScheduleForDoubleExecution1 = FakeRecurringScheduleForDoubleExecution();
            var recurringScheduleForRecurringExecution1 = FakeRecurringScheduleForRecurringExecution();
            var recurringScheduleForSingleExecution2 = FakeRecurringScheduleForSingleExecution();
            var recurringScheduleForDoubleExecution2 = FakeRecurringScheduleForDoubleExecution();
            var recurringScheduleForRecurringExecution2 = FakeRecurringScheduleForRecurringExecution();
            var recurringSchedulefactory = Substitute.For<IRecurringScheduleFactory>();
            recurringSchedulefactory.Create(RecurringExpressionForSingleExecution())
                               .Returns(recurringScheduleForSingleExecution1, recurringScheduleForSingleExecution2);
            recurringSchedulefactory.Create(RecurringExpressionForDoubleExecution())
                               .Returns(recurringScheduleForDoubleExecution1, recurringScheduleForDoubleExecution2);
            recurringSchedulefactory.Create(RecurringExpressionForRecurringExecution())
                               .Returns(recurringScheduleForRecurringExecution1, recurringScheduleForRecurringExecution2);
            recurringSchedulefactory.When(f => f.Create(InvalidRecurringExpression()))
                               .Throw<FormatException>();
            return recurringSchedulefactory;
        }

        private static IRecurringSchedule FakeRecurringScheduleForDoubleExecution()
        {
            var recurringSchedule = Substitute.For<IRecurringSchedule>();
            recurringSchedule.GetNextOccurrence(Arg.Any<DateTime>())
                        .Returns(x => ((DateTime)x[0]).AddSeconds(1), x => ((DateTime)x[0]).AddSeconds(1), x => null);
            return recurringSchedule;
        }

        private static IRecurringSchedule FakeRecurringScheduleForRecurringExecution()
        {
            var recurringSchedule = Substitute.For<IRecurringSchedule>();
            recurringSchedule.GetNextOccurrence(Arg.Any<DateTime>())
                        .Returns(x => ((DateTime)x[0]).AddSeconds(1));
            return recurringSchedule;
        }

        private static IRecurringSchedule FakeRecurringScheduleForSingleExecution()
        {
            var recurringSchedule = Substitute.For<IRecurringSchedule>();
            recurringSchedule.GetNextOccurrence(Arg.Any<DateTime>())
                        .Returns(x => ((DateTime)x[0]).AddSeconds(1), x => null);
            return recurringSchedule;
        }

        private static Exception FakeException() => new Exception("Fake exception.");

        private static ILogger FakeLogger() => Substitute.For<ILogger>();

        private static RecurringCommandManagerSettings<FakeContext> FakeSettings() => new RecurringCommandManagerSettings<FakeContext>(SerializationFormat.Json);

        private static string InvalidRecurringExpression() => "* * * * * * * *";

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
