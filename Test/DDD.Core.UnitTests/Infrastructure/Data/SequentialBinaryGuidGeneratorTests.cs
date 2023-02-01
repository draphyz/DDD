using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    using Collections;

    public class SequentialBinaryGuidGeneratorTests
    {

        #region Methods

        [Fact]
        public void Generate_WhenCalledConsecutively_GeneratesSequentialBinaryValues()
        {
            // Arrange
            var generator = new SequentialBinaryGuidGenerator();
            var binaryValues = new List<byte[]>();
            var comparer = Comparer<byte[]>.Create((x, y) => x.StructuralCompare(y));
            // Act
            for (var i = 0; i < 100; i++)
                binaryValues.Add(generator.Generate().ToByteArray());
            // Assert
            binaryValues.Should().BeInAscendingOrder(comparer);
        }

        #endregion Methods

    }
}