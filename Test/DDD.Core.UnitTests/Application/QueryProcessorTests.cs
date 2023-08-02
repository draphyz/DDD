using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using DDD.Validation;

namespace DDD.Core.Application
{
    public class QueryProcessorTests
    {
        #region Fields

        private readonly ISyncQueryHandler<FakeQuery1, FakeResult> syncHandlerOfQuery1;
        private readonly ISyncQueryHandler<FakeQuery2, FakeResult> syncHandlerOfQuery2;
        private readonly ISyncObjectValidator<FakeQuery1> syncValidatorOfQuery1;
        private readonly ISyncObjectValidator<FakeQuery2> syncValidatorOfQuery2;
        private readonly IAsyncQueryHandler<FakeQuery1, FakeResult> asyncHandlerOfQuery1;
        private readonly IAsyncQueryHandler<FakeQuery2, FakeResult> asyncHandlerOfQuery2;
        private readonly IAsyncObjectValidator<FakeQuery1> asyncValidatorOfQuery1;
        private readonly IAsyncObjectValidator<FakeQuery2> asyncValidatorOfQuery2;
        private readonly QueryProcessor processor;

        #endregion Fields

        #region Constructors

        public QueryProcessorTests()
        {
            this.syncHandlerOfQuery1 = Substitute.For<ISyncQueryHandler<FakeQuery1, FakeResult>>();
            this.syncHandlerOfQuery2 = Substitute.For<ISyncQueryHandler<FakeQuery2, FakeResult>>();
            this.syncValidatorOfQuery1 = Substitute.For<ISyncObjectValidator<FakeQuery1>>();
            this.syncValidatorOfQuery2 = Substitute.For<ISyncObjectValidator<FakeQuery2>>();
            this.asyncHandlerOfQuery1 = Substitute.For<IAsyncQueryHandler<FakeQuery1, FakeResult>>();
            this.asyncHandlerOfQuery2 = Substitute.For<IAsyncQueryHandler<FakeQuery2, FakeResult>>();
            this.asyncValidatorOfQuery1 = Substitute.For<IAsyncObjectValidator<FakeQuery1>>();
            this.asyncValidatorOfQuery2 = Substitute.For<IAsyncObjectValidator<FakeQuery2>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncQueryHandler<FakeQuery1, FakeResult>))))
                           .Returns(this.syncHandlerOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncQueryHandler<FakeQuery2, FakeResult>))))
                           .Returns(this.syncHandlerOfQuery2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeQuery1>))))
                           .Returns(this.syncValidatorOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeQuery2>))))
                           .Returns(this.syncValidatorOfQuery2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncQueryHandler<FakeQuery1, FakeResult>))))
                           .Returns(this.asyncHandlerOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncQueryHandler<FakeQuery2, FakeResult>))))
                           .Returns(this.asyncHandlerOfQuery2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncObjectValidator<FakeQuery1>))))
                           .Returns(this.asyncValidatorOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IAsyncObjectValidator<FakeQuery2>))))
                           .Returns(this.asyncValidatorOfQuery2);
            processor = new QueryProcessor(serviceProvider, new QueryProcessorSettings());
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Process_WhenConcreteQuery_CallsExpectedHandler()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new MessageContext();
            // Act
            this.processor.Process(query, context);
            // Assert
            this.syncHandlerOfQuery1.Received(1).Handle(query, context);
        }

        [Fact]
        public async Task ProcessAsync_WhenConcreteQuery_CallsExpectedHandler()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new MessageContext();
            // Act
            await this.processor.ProcessAsync(query, context);
            // Assert
            await this.asyncHandlerOfQuery1.Received(1).HandleAsync(query, context);
        }

        [Fact]
        public void Process_WhenAbstractQuery_CallsExpectedHandler()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new MessageContext();
            // Act
            this.processor.Process((IQuery)query, context);
            // Assert
            this.syncHandlerOfQuery1.Received(1).Handle(query, context);
        }

        [Fact]
        public async Task ProcessAsync_WhenAbstractQuery_CallsExpectedHandler()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new MessageContext();
            // Act
            await this.processor.ProcessAsync((IQuery)query, context);
            // Assert
            await this.asyncHandlerOfQuery1.Received(1).HandleAsync(query, context);
        }

        [Fact]
        public void Validate_WhenConcreteQuery_CallsExpectedValidator()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new ValidationContext();
            // Act
            this.processor.Validate(query, context);
            // Assert
            this.syncValidatorOfQuery1.Received(1).Validate(query, context);
        }

        [Fact]
        public async Task ValidateAsync_WhenConcreteQuery_CallsExpectedValidator()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new ValidationContext();
            // Act
            await this.processor.ValidateAsync(query, context);
            // Assert
            await this.asyncValidatorOfQuery1.Received(1).ValidateAsync(query, context);
        }

        [Fact]
        public void Validate_WhenAbstractQuery_CallsExpectedValidator()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new ValidationContext();
            // Act
            this.processor.Validate((IQuery)query, context);
            // Assert
            this.syncValidatorOfQuery1.Received(1).Validate(query, context);
        }

        [Fact]
        public async Task ValidateAsync_WhenAbstractQuery_CallsExpectedValidator()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new ValidationContext();
            // Act
            await this.processor.ValidateAsync((IQuery)query, context);
            // Assert
            await this.asyncValidatorOfQuery1.Received(1).ValidateAsync(query, context);
        }

        #endregion Methods
    }
}