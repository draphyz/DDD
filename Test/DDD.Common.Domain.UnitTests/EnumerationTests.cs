using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DDD.Common.Domain
{
    [Trait("Category", "Unit")]
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
            all.Should().BeEquivalentTo(FakeEnumeration.Fake1, FakeEnumeration.Fake2, FakeEnumeration.Fake3);
        }

        [Theory]
        [InlineData("Dummy", true)]
        [InlineData("Dummy", false)]
        [InlineData("Fake1", true)]
        [InlineData("Fake1", false)]
        [InlineData("fk1", false)]
        public void FromCode_WhenInvalidCode_ThrowsArgumentOutOfRangeException(string code, bool ignoreCase)
        {
            // Act
            Action fromCode = () => Enumeration.FromCode<FakeEnumeration>(code, ignoreCase);
            // Assert
            fromCode.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(CodesAndRespectiveEnumerations))]
        public void FromCode_WhenValidCode_ReturnsExpectedEnumeration(string code, bool ignoreCase, FakeEnumeration expected)
        {
            // Act
            var result = Enumeration.FromCode<FakeEnumeration>(code, ignoreCase);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData("Dummy", true)]
        [InlineData("Dummy", false)]
        [InlineData("fk1", true)]
        [InlineData("fk1", false)]
        [InlineData("FAKE1", false)]
        public void FromName_WhenInvalidName_ThrowsArgumentOutOfRangeException(string name, bool ignoreCase)
        {
            // Act
            Action fromName = () => Enumeration.FromName<FakeEnumeration>(name, ignoreCase);
            // Assert
            fromName.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(NamesAndRespectiveEnumerations))]
        public void FromName_WhenValidName_ReturnsExpectedEnumeration(string name, bool ignoreCase, FakeEnumeration expected)
        {
            // Act
            var result = Enumeration.FromName<FakeEnumeration>(name, ignoreCase);
            // Assert
            result.Should().BeSameAs(expected);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void FromValue_WhenInvalidValue_ThrowsArgumentOutOfRangeException(int value)
        {
            // Act
            Action fromValue = () => Enumeration.FromValue<FakeEnumeration>(value);
            // Assert
            fromValue.ShouldThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [MemberData(nameof(ValuesAndRespectiveEnumerations))]
        public void FromValue_WhenValidValue_ReturnsExpectedEnumeration(int value, FakeEnumeration expected)
        {
            // Act
            var result = Enumeration.FromValue<FakeEnumeration>(value);
            // Assert
            result.Should().BeSameAs(expected);
        }

        #endregion Methods
    }
}