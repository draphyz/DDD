using EnsureThat;
using FluentAssertions;
using System;
using Xunit;

namespace DDD
{
    public class ParamExtensionsTests
    {

        #region Methods

        [Fact]
        public void Satisfy_WhenConditionNotSatisfied_ThrowsArgumentException()
        {
            // Arrange
            var value = -1;
            Func<int, bool> predicate = i => i > 0;
            // Act
            Action satisfy = () => Ensure.That(value, nameof(value)).Satisfy(predicate);
            // Assert
            satisfy.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Satisfy_WhenConditionNull_DoesNotThrow()
        {
            // Arrange
            var value = 1;
            Func<int, bool> predicate = null;
            // Act
            Action satisfy = () => Ensure.That(value, nameof(value)).Satisfy(predicate);
            // Assert
            satisfy.Should().NotThrow();
        }

        [Fact]
        public void Satisfy_WhenConditionSatisfied_DoesNotThrow()
        {
            // Arrange
            var value = 1;
            Func<int, bool> predicate = i => i > 0;
            // Act
            Action satisfy = () => Ensure.That(value, nameof(value)).Satisfy(predicate);
            // Assert
            satisfy.Should().NotThrow();
        }

        #endregion Methods

    }
}
