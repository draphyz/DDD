using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using DDD.Validation;

namespace DDD.Core.Application
{
    public class CommandProcessorTests
    {
        #region Fields

        private readonly ISyncCommandHandler<FakeCommand1> syncHandlerOfCommand1;
        private readonly ISyncCommandHandler<FakeCommand2> syncHandlerOfCommand2;
        private readonly ISyncObjectValidator<FakeCommand1> syncValidatorOfCommand1;
        private readonly ISyncObjectValidator<FakeCommand2> syncValidatorOfCommand2;
        private readonly IAsyncCommandHandler<FakeCommand1> asyncHandlerOfCommand1;
        private readonly IAsyncCommandHandler<FakeCommand2> asyncHandlerOfCommand2;
        private readonly IAsyncObjectValidator<FakeCommand1> asyncValidatorOfCommand1;
        private readonly IAsyncObjectValidator<FakeCommand2> asyncValidatorOfCommand2;
        private readonly CommandProcessor processor;

        #endregion Fields

        #region Constructors

        public CommandProcessorTests()
        {
            this.syncHandlerOfCommand1 = Substitute.For<ISyncCommandHandler<FakeCommand1>>();
            this.syncHandlerOfCommand2 = Substitute.For<ISyncCommandHandler<FakeCommand2>>();
            this.syncValidatorOfCommand1 = Substitute.For<ISyncObjectValidator<FakeCommand1>>();
            this.syncValidatorOfCommand2 = Substitute.For<ISyncObjectValidator<FakeCommand2>>();
            this.asyncHandlerOfCommand1 = Substitute.For<IAsyncCommandHandler<FakeCommand1>>();
            this.asyncHandlerOfCommand2 = Substitute.For<IAsyncCommandHandler<FakeCommand2>>();
            this.asyncValidatorOfCommand1 = Substitute.For<IAsyncObjectValidator<FakeCommand1>>();
            this.asyncValidatorOfCommand2 = Substitute.For<IAsyncObjectValidator<FakeCommand2>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncCommandHandler<FakeCommand1>))))
                           .Returns(this.syncHandlerOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncCommandHandler<FakeCommand2>))))
                           .Returns(this.syncHandlerOfCommand2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeCommand1>))))
                           .Returns(this.syncValidatorOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeCommand2>))))
                           .Returns(this.syncValidatorOfCommand2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncCommandHandler<FakeCommand1>))))
                           .Returns(this.asyncHandlerOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncCommandHandler<FakeCommand2>))))
                           .Returns(this.asyncHandlerOfCommand2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncObjectValidator<FakeCommand1>))))
                           .Returns(this.asyncValidatorOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncObjectValidator<FakeCommand2>))))
                           .Returns(this.asyncValidatorOfCommand2);
            processor = new CommandProcessor(serviceProvider, new CommandProcessorSettings());
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Process_WhenConcreteCommand_CallsExpectedHandler()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new MessageContext();
            // Act
            this.processor.Process(command, context);
            // Assert
            this.syncHandlerOfCommand1.Received(1).Handle(command, context);
        }

        [Fact]
        public async Task ProcessAsync_WhenConcreteCommand_CallsExpectedHandler()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new MessageContext();
            // Act
            await this.processor.ProcessAsync(command, context);
            // Assert
            await this.asyncHandlerOfCommand1.Received(1).HandleAsync(command, context);
        }

        [Fact]
        public void Process_WhenAbstractCommand_CallsExpectedHandler()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new MessageContext();
            // Act
            this.processor.Process((ICommand)command, context);
            // Assert
            this.syncHandlerOfCommand1.Received(1).Handle(command, context);
        }

        [Fact]
        public async Task ProcessAsync_WhenAbstractCommand_CallsExpectedHandler()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new MessageContext();
            // Act
            await this.processor.ProcessAsync((ICommand)command, context);
            // Assert
            await this.asyncHandlerOfCommand1.Received(1).HandleAsync(command, context);
        }

        [Fact]
        public void Validate_WhenConcreteCommand_CallsExpectedValidator()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new ValidationContext();
            // Act
            this.processor.Validate(command, context);
            // Assert
            this.syncValidatorOfCommand1.Received(1).Validate(command, context);
        }

        [Fact]
        public async Task ValidateAsync_WhenConcreteCommand_CallsExpectedValidator()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new ValidationContext();
            // Act
            await this.processor.ValidateAsync(command, context);
            // Assert
            await this.asyncValidatorOfCommand1.Received(1).ValidateAsync(command, context);
        }

        [Fact]
        public void Validate_WhenAbstractCommand_CallsExpectedValidator()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new ValidationContext();
            // Act
            this.processor.Validate((ICommand)command, context);
            // Assert
            this.syncValidatorOfCommand1.Received(1).Validate(command, context);
        }

        [Fact]
        public async Task ValidateAsync_WhenAbstractCommand_CallsExpectedValidator()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new ValidationContext();
            // Act
            await this.processor.ValidateAsync((ICommand)command, context);
            // Assert
            await this.asyncValidatorOfCommand1.Received(1).ValidateAsync(command, context);
        }

        #endregion Methods
    }
}