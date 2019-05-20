using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace DDD
{
    public class StringExtensionsTests
    {

        #region Methods

        public static IEnumerable<object[]> IsShortDateString()
        {
            yield return new object[]
            {
                "6/1/2009",
                new CultureInfo("en-US"),
                true
            };
            yield return new object[]
            {
                "1-6-2009",
                new CultureInfo("en-US"),
                false
            };
            yield return new object[]
            {
                "01/06/2009",
                new CultureInfo("fr-FR"),
                true
            };
            yield return new object[]
            {
                "6/1/2009",
                new CultureInfo("fr-FR"),
                false
            };
            yield return new object[]
            {
                "1-6-2009",
                new CultureInfo("fr-FR"),
                false
            };

            yield return new object[]
            {
                "1-6-2009",
                new CultureInfo("nl-NL"),
                true
            };
        }

        [Theory]
        [InlineData("Olivier", 0, "")]
        [InlineData("Olivier", 10, "Olivier")]
        [InlineData("Olivier", 3, "ier")]
        [InlineData("Olivier", 4, "vier")]
        public void Right_WhenSpecifiedLengthGreaterOrEqualToZero_ReturnsExpectedString(string instance, int length, string expected)
        {
            // Act
            var result = instance.Right(length);
            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Right_WhenSpecifiedLengthLessThanZero_ThrowsArgumentException()
        {
            // Arrange
            var instance = "Olivier";
            // Act
            Action action = () => instance.Right(-1);
            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("Olivier", 0, "")]
        [InlineData("Olivier", 10, "Olivier")]
        [InlineData("Olivier", 3, "Oli")]
        [InlineData("Olivier", 4, "Oliv")]
        public void Left_WhenSpecifiedLengthGreaterOrEqualToZero_ReturnsExpectedString(string instance, int length, string expected)
        {
            // Act
            var result = instance.Left(length);
            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Left_WhenSpecifiedLengthLessThanZero_ThrowsArgumentException()
        {
            // Arrange
            var instance = "Olivier";
            // Act
            Action action = () => instance.Left(-1);
            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("asd", true)]
        [InlineData("aSd", true)]
        [InlineData("123345", false)]
        [InlineData("123asd", false)]
        [InlineData("!##$", false)]
        public void IsAlphabetic_WhenStringNotNull_ReturnsExpectedValue(string instance, bool expected)
        {
            // Act
            var result = instance.IsAlphabetic();
            // Assert
            result.Should().Be(expected);
        }


        [Theory]
        [InlineData("123345", true)]
        [InlineData("asd", false)]
        [InlineData("aSd", false)]
        [InlineData("123asd", false)]
        [InlineData("!##$", false)]
        public void IsNumeric_WhenStringNotNull_ReturnsExpectedValue(string instance, bool expected)
        {
            // Act
            var result = instance.IsNumeric();
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("asd", true)]
        [InlineData("aSd", true)]
        [InlineData("123345", true)]
        [InlineData("123asd", true)]
        [InlineData("!##$", false)]
        [InlineData("a!##$", false)]
        public void IsAlphanumeric_WhenStringNotNull_ReturnsExpectedValue(string instance, bool expected)
        {
            // Act
            var result = instance.IsAlphanumeric();
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("H", "H")]
        [InlineData("Hello World", "dlroW olleH")]
        public void Reverse_WhenStringNotNull_ReturnsExpectedValue(string instance, string expected)
        {
            // Act
            var result = instance.Reverse();
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(IsShortDateString))]
        public void IsShortDateString_WhenStringNotNull_ReturnsExpectedValue(string instance, IFormatProvider provider, bool expected)
        {
            // Act
            var result = instance.IsShortDateString(provider);
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("01/01/2001", true)]
        [InlineData("10/01/2001", true)]
        [InlineData("01/10/2001", true)]
        [InlineData("01.01.2001", false)]
        [InlineData("01-01-2001", false)]
        [InlineData("01/01/01", false)]
        [InlineData("1/1/2001", false)]
        [InlineData("10/1/2001", false)]
        [InlineData("1/10/2001", false)]
        public void IsFrenchShortDateString_WhenStringNotNull_ReturnsExpectedValue(string instance, bool expected)
        {
            // Act
            var result = instance.IsFrenchShortDateString();
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("OLIVER", "Oliver")]
        [InlineData("oliver", "Oliver")]
        [InlineData("oLIver", "Oliver")]
        [InlineData("this is a Title", "This Is A Title")]
        public void ToTitleCase_WhenStringNotNull_ReturnsExpectedValue(string instance, string expected)
        {
            // Act
            var result = instance.ToTitleCase();
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("iPhone", "i_Phone")]
        [InlineData("DreamWorks", "Dream_Works")]
        public void ToSnakeCase_WhenStringNotNull_ReturnsExpectedValue(string instance, string expected)
        {
            // Act
            var result = instance.ToSnakeCase();
            // Assert
            result.Should().Be(expected);
        }

        #endregion Methods

    }
}
