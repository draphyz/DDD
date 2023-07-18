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

        private readonly ISyncCommandHandler<FakeCommand1> handlerOfCommand1;
        private readonly ISyncCommandHandler<FakeCommand2> handlerOfCommand2;
        private readonly ISyncObjectValidator<FakeCommand1> validatorOfCommand1;
        private readonly ISyncObjectValidator<FakeCommand2> validatorOfCommand2;
        private readonly CommandProcessor processor;

        #endregion Fields

        #region Constructors

        public CommandProcessorTests()
        {
            this.handlerOfCommand1 = Substitute.For<ISyncCommandHandler<FakeCommand1>>();
            this.handlerOfCommand2 = Substitute.For<ISyncCommandHandler<FakeCommand2>>();
            this.validatorOfCommand1 = Substitute.For<ISyncObjectValidator<FakeCommand1>>();
            this.validatorOfCommand2 = Substitute.For<ISyncObjectValidator<FakeCommand2>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncCommandHandler<FakeCommand1>))))
                           .Returns(this.handlerOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncCommandHandler<FakeCommand2>))))
                           .Returns(this.handlerOfCommand2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeCommand1>))))
                           .Returns(this.validatorOfCommand1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeCommand2>))))
                           .Returns(this.validatorOfCommand2);
            processor = new CommandProcessor(serviceProvider, new CommandProcessorSettings());
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Process_WhenCommandDefined_CallsExpectedHandler()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new MessageContext();
            // Act
            this.processor.Process(command, context);
            // Assert
            this.handlerOfCommand1.Received(1).Handle(command, context);
        }

        [Fact]
        public void Validate_WhenCommandDefined_CallsExpectedValidator()
        {
            // Arrange
            var command = new FakeCommand1();
            var context = new ValidationContext();
            // Act
            this.processor.Validate(command, context);
            // Assert
            this.validatorOfCommand1.Received(1).Validate(command, context);
        }

        #endregion Methods
    }
}