using EnsureThat;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DDD
{
    public class EnumerableArgExtensionsTests
    {

        #region Methods

        public static IEnumerable<object[]> SequencesWithNull()
        {
            yield return new object[] { new string[] { "", null, "aajs" } };
            yield return new object[] { new object[] { "", null, "aajs" } };
        }

        public static IEnumerable<object[]> SequencesWithoutNull()
        {
            yield return new object[] { new string[] { "" } };
            yield return new object[] { new object[] { "", "aajs" } };
        }

        [Theory]
        [MemberData(nameof(SequencesWithNull))]
        public void HasNoNull_WhenAnyNull_ThrowsArgumentException(IEnumerable<object> value)
        {
            // Act
            Action hasNoNull = () => Ensure.Enumerable.HasNoNull(value, nameof(value));
            // Assert
            hasNoNull.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(SequencesWithoutNull))]
        public void HasNoNull_WhenNoNull_DoesNotThrow(IEnumerable<object> value)
        {
            // Act
            Action hasNoNull = () => Ensure.Enumerable.HasNoNull(value, nameof(value));
            // Assert
            hasNoNull.Should().NotThrow();
        }

        [Fact]
        public void HasNoNull_WhenSequenceNull_ThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<object> value = null;
            // Act
            Action hasNoNull = () => Ensure.Enumerable.HasNoNull(value, nameof(value));
            // Assert
            hasNoNull.Should().ThrowExactly<ArgumentNullException>();
        }

        #endregion Methods
    }
}
