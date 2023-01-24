using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DDD.Core.Infrastructure.Data
{
    public class SequentialStringGuidGeneratorTests
    {

        #region Methods

        [Fact]
        public void Generate_WhenCalledConsecutively_GeneratesSequentialStringValues()
        {
            // Arrange
            var generator = new SequentialStringGuidGenerator();
            var stringValues = new List<string>();
            // Act
            for (var i = 0; i < 100; i++)
                stringValues.Add(generator.Generate().ToString());
            // Assert
            stringValues.Should().BeInAscendingOrder();
        }

        #endregion Methods

    }
}