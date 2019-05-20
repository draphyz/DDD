using NSubstitute;
using System;
using Xunit;

namespace DDD.Core.Application
{
    public class CommandProcessorTests
    {
        #region Fields

        private readonly ICommandHandler<FakeCommand1> handlerOfCommand1;
        private readonly ICommandHandler<FakeCommand2> handlerOfCommand2;
        private readonly ICommandValidator<FakeCommand1> validatorOfCommand1;
        private readonly ICommandValidator<FakeCommand2> validatorOfCommand2;
        private readonly CommandProcessor processor;

        #endregion Fields

        #region Constructors

        public CommandProcessorTests()
        {
            this.handlerOfCommand1 = Substitute.For<ICommandHandler<FakeCommand1>>();
            this.handlerOfCommand2 = Substitute.For<ICommandHandler<FakeCommand2>>();
            this.validatorOfCommand1 = Substitute.For<ICommandValidator<FakeCommand1>>();
            this.validatorOfCommand2 = Substitute.For<ICommandValidator<FakeCommand2>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ICommandHandler<FakeCommand1>))))
                           .Returns(this.handlerOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ICommandHandler<FakeCommand2>))))
                           .Returns(this.handlerOfCommand2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ICommandValidator<FakeCommand1>))))
                           .Returns(this.validatorOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ICommandValidator<FakeCommand2>))))
                           .Returns(this.validatorOfCommand2);
            processor = new CommandProcessor(serviceProvider);
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Process_WhenCommandDefined_CallsExpectedHandler()
        {
            // Arrange
            var command = new FakeCommand1();
            // Act
            this.processor.Process(command);
            // Assert
            this.handlerOfCommand1.Received(1).Handle(command);
        }

        [Fact]
        public void Validate_WhenCommandDefined_CallsExpectedValidator()
        {
            // Arrange
            var command = new FakeCommand1();
            // Act
            this.processor.Validate(command);
            // Assert
            this.validatorOfCommand1.Received(1).Validate(command);
        }

        #endregion Methods
    }
}