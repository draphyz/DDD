using NSubstitute;
using System;
using Xunit;

namespace DDD.Core.Application
{
    public class QueryProcessorTests
    {
        #region Fields

        private readonly IQueryHandler<FakeQuery1, FakeResult> handlerOfQuery1;
        private readonly IQueryHandler<FakeQuery2, FakeResult> handlerOfQuery2;
        private readonly IQueryValidator<FakeQuery1> validatorOfQuery1;
        private readonly IQueryValidator<FakeQuery2> validatorOfQuery2;
        private readonly QueryProcessor processor;

        #endregion Fields

        #region Constructors

        public QueryProcessorTests()
        {
            this.handlerOfQuery1 = Substitute.For<IQueryHandler<FakeQuery1, FakeResult>>();
            this.handlerOfQuery2 = Substitute.For<IQueryHandler<FakeQuery2, FakeResult>>();
            this.validatorOfQuery1 = Substitute.For<IQueryValidator<FakeQuery1>>();
            this.validatorOfQuery2 = Substitute.For<IQueryValidator<FakeQuery2>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IQueryHandler<FakeQuery1, FakeResult>))))
                           .Returns(this.handlerOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IQueryHandler<FakeQuery2, FakeResult>))))
                           .Returns(this.handlerOfQuery2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IQueryValidator<FakeQuery1>))))
                           .Returns(this.validatorOfQuery1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IQueryValidator<FakeQuery2>))))
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
            // Act
            this.processor.Process(query);
            // Assert
            this.handlerOfQuery1.Received(1).Handle(query);
        }

        [Fact]
        public void Validate_WhenQueryDefined_CallsExpectedValidator()
        {
            // Arrange
            var query = new FakeQuery1();
            // Act
            this.processor.Validate(query);
            // Assert
            this.validatorOfQuery1.Received(1).Validate(query);
        }

        #endregion Methods
    }
}