using NSubstitute;
using System;
using Xunit;

namespace DDD.Mapping
{
    public class MappingProcessorTests
    {
        #region Fields

        private readonly IObjectMapper<FakeObject1, FakeObject2> mapper1To2;
        private readonly IObjectMapper<FakeObject2, FakeObject1> mapper2To1;
        private readonly IObjectTranslator<FakeObject1, FakeObject2> translator1To2;
        private readonly IObjectTranslator<FakeObject2, FakeObject1> translator2To1;
        private readonly MappingProcessor processor;

        #endregion Fields

        #region Constructors

        public MappingProcessorTests()
        {
            this.mapper1To2 = Substitute.For<IObjectMapper<FakeObject1, FakeObject2>>();
            this.mapper2To1 = Substitute.For<IObjectMapper<FakeObject2, FakeObject1>>();
            this.translator1To2 = Substitute.For<IObjectTranslator<FakeObject1, FakeObject2>>();
            this.translator2To1 = Substitute.For<IObjectTranslator<FakeObject2, FakeObject1>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IObjectMapper<FakeObject1, FakeObject2>))))
                           .Returns(this.mapper1To2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IObjectMapper<FakeObject2, FakeObject1>))))
                           .Returns(this.mapper2To1);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IObjectTranslator<FakeObject1, FakeObject2>))))
                           .Returns(this.translator1To2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IObjectTranslator<FakeObject2, FakeObject1>))))
                           .Returns(this.translator2To1);
            processor = new MappingProcessor(serviceProvider);
        }

        #endregion Constructors

        #region Methods

        [Fact]
        public void Map_WhenCalled_CallsExpectedMapper()
        {
            // Arrange
            var source = new FakeObject1();
            var destination = new FakeObject2();
            // Act
            this.processor.Map(source, destination);
            // Assert
            this.mapper1To2.Received(1).Map(source, destination);
        }

        [Fact]
        public void Translate_WhenCalled_CallsExpectedTranslator()
        {
            // Arrange
            var source = new FakeObject1();
            // Act
            _ = this.processor.Translate<FakeObject2>(source);
            // Assert
            this.translator1To2.Received(1).Translate(source);
        }

        #endregion Methods
    }
}