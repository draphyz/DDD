using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DDD.Mapping
{
    public class IMappingProcessorExtensionsTests
    {

        #region Fields

        private readonly MappingProcessor processor;
        private readonly IObjectTranslator<FakeObject1, FakeObject2> translator1To2;
        private readonly IObjectTranslator<FakeObject2, FakeObject1> translator2To1;

        #endregion Fields

        #region Constructors

        public IMappingProcessorExtensionsTests()
        {
            this.translator1To2 = Substitute.For<IObjectTranslator<FakeObject1, FakeObject2>>();
            this.translator2To1 = Substitute.For<IObjectTranslator<FakeObject2, FakeObject1>>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IObjectTranslator<FakeObject1, FakeObject2>))))
                           .Returns(this.translator1To2);
            serviceProvider.GetService(Arg.Is<Type>(t => t.IsAssignableFrom(typeof(IObjectTranslator<FakeObject2, FakeObject1>))))
                           .Returns(this.translator2To1);
            processor = new MappingProcessor(serviceProvider);
        }

        #endregion Constructors

        #region Methods

        public static IEnumerable<object[]> Sources()
        {
            var emptyArray = new FakeObject1[] { };
            yield return new object[] { emptyArray };
            var array = new FakeObject1[]
            {
                new FakeObject1(),
                new FakeObject1(),
                new FakeObject1()
            };
            yield return new object[] { array };
            var emptyList = new List<FakeObject1>();
            yield return new object[] { emptyList };
            var list = new List<FakeObject1>();
            list.Add(new FakeObject1());
            yield return new object[] { list };
        }

        [Theory]
        [MemberData(nameof(Sources))]
        public void TranslateCollection_OfObject_CallsExpectedTranslator(IEnumerable<FakeObject1> source)
        {
            // Arrange
            var context = new MappingContext();
            // Act
            IMappingProcessorExtensions.TranslateCollection<FakeObject2>(this.processor, source, context)
                                       .ToList();
            // Assert
            this.translator1To2.Received(source.Count()).Translate(Arg.Is<FakeObject1>(x => source.Contains(x)), context);
        }

        [Theory]
        [MemberData(nameof(Sources))]
        public void TranslateCollection_OfSpecifiedType_CallsExpectedTranslator(IEnumerable<FakeObject1> source)
        {
            // Arrange
            var context = new MappingContext();
            // Act
            IMappingProcessorExtensions.TranslateCollection<FakeObject1, FakeObject2>(this.processor, source, context)
                                       .ToList();
            // Assert
            this.translator1To2.Received(source.Count()).Translate(Arg.Is<FakeObject1>(x => source.Contains(x)), context);
        }

        #endregion Methods

    }
}