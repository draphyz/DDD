using NSubstitute;
using System;
using Xunit;
using DDD.Validation;

namespace DDD.Core.Application
{
    public class QueryProcessorTests
    {
        #region Fields

        private readonly ISyncQueryHandler<FakeQuery1, FakeResult> handlerOfQuery1;
        private readonly ISyncQueryHandler<FakeQuery2, FakeResult> handlerOfQuery2;
        private readonly ISyncObjectValidator<FakeQuery1> validatorOfQuery1;
        private readonly ISyncObjectValidator<FakeQuery2> validatorOfQuery2;
        private readonly QueryProcessor processor;

        #endregion Fields

        #region Constructors

        public QueryProcessorTests()
        {
            this.handlerOfQuery1 = Substitute.For<ISyncQueryHandler<FakeQuery1, FakeResult>>();
            this.handlerOfQuery2 = Substitute.For<ISyncQueryHandler<FakeQuery2, FakeResult>>();
            this.validatorOfQuery1 = Substitute.For<ISyncObjectValidator<FakeQuery1>>();
            this.validatorOfQuery2 = Substitute.For<ISyncObjectValidator<FakeQuery2>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncQueryHandler<FakeQuery1, FakeResult>))))
                           .Returns(this.handlerOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncQueryHandler<FakeQuery2, FakeResult>))))
                           .Returns(this.handlerOfQuery2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeQuery1>))))
                           .Returns(this.validatorOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(ISyncObjectValidator<FakeQuery2>))))
                           .Returns(this.validatorOfQuery2);
            processor = new QueryProcessor(serviceProvider);
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Process_WhenQueryDefined_CallsExpectedHandler()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new MessageContext();
            // Act
            this.processor.Process(query, context);
            // Assert
            this.handlerOfQuery1.Received(1).Handle(query, context);
        }

        [Fact]
        public void Validate_WhenQueryDefined_CallsExpectedValidator()
        {
            // Arrange
            var query = new FakeQuery1();
            var context = new ValidationContext();
            // Act
            this.processor.Validate(query, context);
            // Assert
            this.validatorOfQuery1.Received(1).Validate(query, context);
        }

        #endregion Methods
    }
}