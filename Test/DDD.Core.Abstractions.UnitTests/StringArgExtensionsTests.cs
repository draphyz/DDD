using System;
using Xunit;
using FluentAssertions;
using EnsureThat;

namespace DDD
{
    public class StringArgExtensionsTests
    {

        #region Methods

        [Theory]
        [InlineData("abc", 4)]
        [InlineData("abc", 10)]
        public void HasMinLength_WhenInvalidLength_ThrowsArgumentException(string value, int minLength)
        {
            // Act
            Action hasMinLength = () => Ensure.String.HasMinLength(value, minLength, nameof(value));
            // Assert
            hasMinLength.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void HasMinLength_WhenStringNull_ThrowsArgumentNullException()
        {
            // Arrange
            string value = null;
            var minLength = 0;
            // Act
            Action hasMinLength = () => Ensure.String.HasMinLength(value, minLength, nameof(value));
            // Assert
            hasMinLength.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("abc", 3)]
        [InlineData("abc", 1)]
        public void HasMinLength_WhenValidLength_DoesNotThrow(string value, int minLength)
        {
            // Act
            Action hasMinLength = () => Ensure.String.HasMinLength(value, minLength, nameof(value));
            // Assert
            hasMinLength.Should().NotThrow();
        }

        [Theory]
        [InlineData("4s58")]
        [InlineData("_00951")]
        [InlineData("10@")]
        [InlineData("1a5")]
        public void IsAllDigits_WhenInvalidCharacters_ThrowsArgumentException(string value)
        {
            // Act
            Action isAllDigits = () => Ensure.String.IsAllDigits(value, nameof(value));
            // Assert
            isAllDigits.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void IsAllDigits_WhenStringNull_ThrowsArgumentNullException()
        {
            // Arrange
            string value = null;
            // Act
            Action isAllDigits = () => Ensure.String.IsAllDigits(value, nameof(value));
            // Assert
            isAllDigits.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("458")]
        [InlineData("00951")]
        public void IsAllDigits_WhenValidCharacters_DoesNotThrow(string value)
        {
            // Act
            Action isAllDigits = () => Ensure.String.IsAllDigits(value, nameof(value));
            // Assert
            isAllDigits.Should().NotThrow();
        }

        [Theory]
        [InlineData("ajdz4dd")]
        [InlineData("_aekdl")]
        [InlineData("ol@")]
        [InlineData("PAZ.")]
        public void IsAllLetters_WhenInvalidCharacters_ThrowsArgumentException(string value)
        {
            // Act
            Action isAllLetters = () => Ensure.String.IsAllLetters(value, nameof(value));
            // Assert
            isAllLetters.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void IsAllLetters_WhenStringNull_ThrowsArgumentNullException()
        {
            // Arrange
            string value = null;
            // Act
            Action isAllLetters = () => Ensure.String.IsAllLetters(value, nameof(value));
            // Assert
            isAllLetters.Should().ThrowExactly<ArgumentNullException>();
        }
        [Theory]
        [InlineData("ajdzdd")]
        [InlineData("aekdl")]
        [InlineData("ol")]
        [InlineData("PAZ")]
        public void IsAllLetters_WhenValidCharacters_DoesNotThrow(string value)
        {
            // Act
            Action isAllLetters = () => Ensure.String.IsAllLetters(value, nameof(value));
            // Assert
            isAllLetters.Should().NotThrow();
        }

        #endregion Methods

    }
}
