using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace DDD
{
    public class DateTimeExtensionsTests
    {

        #region Methods

        public static IEnumerable<object[]> ToFrenchShortDateStringArguments()
        {
            yield return new object[]
            {
                new DateTime(2009, 6, 1, 8, 42, 50),
                "01/06/2009"
            };
            yield return new object[]
            {
                new DateTime(2009, 10, 1, 8, 42, 50),
                "01/10/2009"
            };
            yield return new object[]
            {
                new DateTime(2009, 10, 12, 8, 42, 50),
                "12/10/2009"
            };
            yield return new object[]
            {
                new DateTime(2009, 6, 12, 8, 42, 50),
                "12/06/2009"
            };
        }

        public static IEnumerable<object[]> ToShortDateStringArguments()
        {
            yield return new object[]
            {
                new DateTime(2009, 6, 1, 8, 42, 50),
                new CultureInfo("en-US"),
                "6/1/2009"
            };
            yield return new object[]
            {
                new DateTime(2009, 6, 1, 8, 42, 50),
                new CultureInfo("fr-FR"),
                "01/06/2009"
            };
            yield return new object[]
            {
                new DateTime(2009, 6, 1, 8, 42, 50),
                new CultureInfo("nl-NL"),
                "1-6-2009"
            };
        }

        [Theory]
        [MemberData(nameof(ToFrenchShortDateStringArguments))]
        public void ToFrenchShortDateString_WhenCalled_ReturnsExpectedString(DateTime instance, string expected)
        {
            // Act
            var result = instance.ToFrenchShortDateString();
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(ToShortDateStringArguments))]
        public void ToShortDateString_WhenCalled_ReturnsExpectedString(DateTime instance, IFormatProvider provider, string expected)
        {
            // Act
            var result = instance.ToShortDateString(provider);
            // Assert
            result.Should().Be(expected);
        }

        #endregion Methods
    }
}
