using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DDD.Common.Domain
{
    public class EnumerationTests
    {

        #region Methods

        public static IEnumerable<object[]> CodesAndRespectiveEnumerations()
        {
            yield return new object[]
            {
                "FK1",
                false,
                FakeEnumeration.Fake1
            };
            yield return new object[]
            {
                "FK2",
                false,
                FakeEnumeration.Fake2
            };
            yield return new object[]
            {
                "FK3",
                false,
                FakeEnumeration.Fake3
            };
            yield return new object[]
            {
                "fk1",
                true,
                FakeEnumeration.Fake1
            };
            yield return new object[]
            {
                "fk2",
                true,
                FakeEnumeration.Fake2
            };
            yield return new object[]
            {
                "fk3",
                true,
                FakeEnumeration.Fake3
            };
        }

        public static IEnumerable<object[]> NamesAndRespectiveEnumerations()
        {
            yield return new object[]
            {
                "Fake1",
                false,
                FakeEnumeration.Fake1
            };
            yield return new object[]
            {
                "Fake2",
                false,
                FakeEnumeration.Fake2
            };
            yield return new object[]
            {
                "Fake3",
                false,
                FakeEnumeration.Fake3
            };
            yield return new object[]
            {
                "FAKE1",
                true,
                FakeEnumeration.Fake1
            };
            yield return new object[]
            {
                "FAKE2",
                true,
                FakeEnumeration.Fake2
            };
            yield return new object[]
            {
                "FAKE3",
                true,
                FakeEnumeration.Fake3
            };
        }

        public static IEnumerable<object[]> ValuesAndRespectiveEnumerations()
        {
            yield return new object[]
            {
                1,
                FakeEnumeration.Fake1
            };
            yield return new object[]
            {
                2,
                FakeEnumeration.Fake2
            };
            yield return new object[]
            {
                3,
                FakeEnumeration.Fake3
            };
        }

        [Fact]
        public void All_WhenCalled_ReturnsAllConstants()
        {
            // Act
            var all = Enumeration.All<FakeEnumeration>();
            // Assert
            all.Should().BeEquivalentTo(new[] { FakeEnumeration.Fake1, FakeEnumeration.Fake2, FakeEnumeration.Fake3 });
        }

        [Theory]
        [InlineData("Dummy", true)]
        [InlineData("Dummy", false)]
        [InlineData("Fake1", true)]
        [InlineData("Fake1", false)]
        [InlineData("fk1", false)]
        public void ParseCode_WhenInvalidCode_ThrowsArgumentOutOfRangeException(string code, bool ignoreCase)
        {
            // Act
            Action fromCode = () => Enumeration.ParseCode<FakeEnumeration>(code, ignoreCase);
            // Assert
            fromCode.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(CodesAndRespectiveEnumerations))]
        public void ParseCode_WhenValidCode_ReturnsExpectedEnumeration(string code, bool ignoreCase, FakeEnumeration expected)
        {
            // Act
            var result = Enumeration.ParseCode<FakeEnumeration>(code, ignoreCase);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData("Dummy", true)]
        [InlineData("Dummy", false)]
        [InlineData("fk1", true)]
        [InlineData("fk1", false)]
        [InlineData("FAKE1", false)]
        public void ParseName_WhenInvalidName_ThrowsArgumentOutOfRangeException(string name, bool ignoreCase)
        {
            // Act
            Action fromName = () => Enumeration.ParseName<FakeEnumeration>(name, ignoreCase);
            // Assert
            fromName.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(NamesAndRespectiveEnumerations))]
        public void ParseName_WhenValidName_ReturnsExpectedEnumeration(string name, bool ignoreCase, FakeEnumeration expected)
        {
            // Act
            var result = Enumeration.ParseName<FakeEnumeration>(name, ignoreCase);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void ParseValue_WhenInvalidValue_ThrowsArgumentOutOfRangeException(int value)
        {
            // Act
            Action fromValue = () => Enumeration.ParseValue<FakeEnumeration>(value);
            // Assert
            fromValue.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(ValuesAndRespectiveEnumerations))]
        public void ParseValue_WhenValidValue_ReturnsExpectedEnumeration(int value, FakeEnumeration expected)
        {
            // Act
            var result = Enumeration.ParseValue<FakeEnumeration>(value);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("Dummy", true)]
        [InlineData("Dummy", false)]
        [InlineData("Fake1", true)]
        [InlineData("Fake1", false)]
        [InlineData("fk1", false)]
        public void TryParseCode_WhenInvalidCode_ReturnsFalse(string code, bool ignoreCase)
        {
            // Act
            var success = Enumeration.TryParseCode<FakeEnumeration>(code, ignoreCase, out _);
            // Assert
            success.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(CodesAndRespectiveEnumerations))]
        public void TryParseCode_WhenValidCode_HasExpectedResult(string code, bool ignoreCase, FakeEnumeration expected)
        {
            // Act
            Enumeration.TryParseCode<FakeEnumeration>(code, ignoreCase, out var result);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData("FK1", false)]
        [InlineData("FK2", false)]
        [InlineData("fk1", true)]
        [InlineData("fK2", true)]
        public void TryParseCode_WhenValidCode_ReturnsTrue(string code, bool ignoreCase)
        {
            // Act
            var success = Enumeration.TryParseCode<FakeEnumeration>(code, ignoreCase, out _);
            // Assert
            success.Should().BeTrue();
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("Dummy", true)]
        [InlineData("Dummy", false)]
        [InlineData("fk1", true)]
        [InlineData("fk1", false)]
        [InlineData("FAKE1", false)]
        public void TryParseName_WhenInvalidName_ReturnsFalse(string name, bool ignoreCase)
        {
            // Act
            var success = Enumeration.TryParseName<FakeEnumeration>(name, ignoreCase, out _);
            // Assert
            success.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(NamesAndRespectiveEnumerations))]
        public void TryParseName_WhenValidName_HasExpectedEnumeration(string name, bool ignoreCase, FakeEnumeration expected)
        {
            // Act
            Enumeration.TryParseName<FakeEnumeration>(name, ignoreCase, out var result);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData("Fake1", false)]
        [InlineData("FAKE1", true)]
        [InlineData("faKe2", true)]
        public void TryParseName_WhenValidName_ReturnsTrue(string name, bool ignoreCase)
        {
            // Act
            var success = Enumeration.TryParseName<FakeEnumeration>(name, ignoreCase, out _);
            // Assert
            success.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void TryParseValue_WhenInvalidValue_ReturnsFalse(int value)
        {
            // Act
            var success = Enumeration.TryParseValue<FakeEnumeration>(value, out _);
            // Assert
            success.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(ValuesAndRespectiveEnumerations))]
        public void TryParseValue_WhenValidValue_HasExpectedEnumeration(int value, FakeEnumeration expected)
        {
            // Act
            Enumeration.TryParseValue<FakeEnumeration>(value, out var result);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TryParseValue_WhenValidValue_ReturnsTrue(int value)
        {
            // Act
            var success = Enumeration.TryParseValue<FakeEnumeration>(value, out _);
            // Assert
            success.Should().BeTrue();
        }

        #endregion Methods

    }
}