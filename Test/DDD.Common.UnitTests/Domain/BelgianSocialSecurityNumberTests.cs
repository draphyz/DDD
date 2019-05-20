using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DDD.Common.Domain
{
    public class BelgianSocialSecurityNumberTests
    {

        #region Methods

        public static IEnumerable<object[]> ValidNumbersAndExpectedBirthdates()
        {
            yield return new object[]
            {
                new BelgianSocialSecurityNumber("85073003328"),
                new DateTime(1985, 7, 30)
            };
            yield return new object[]
            {
                new BelgianSocialSecurityNumber("53422003890"),
                new DateTime(1953, 2, 20)
            };
            yield return new object[]
            {
                new BelgianSocialSecurityNumber("17073003384"),
                new DateTime(2017, 7, 30)
            };
            yield return new object[]
            {
                new BelgianSocialSecurityNumber("00000100364"),
                null
            };
            yield return new object[]
            {
                new BelgianSocialSecurityNumber("40000095579"),
                null
            };
        }

        [Theory]
        [MemberData(nameof(ValidNumbersAndExpectedBirthdates))]
        public void Birthdate_WhenValidNumber_ReturnsExpectedValue(BelgianSocialSecurityNumber socialSecurityNumber, DateTime? expectedValue)
        {
            // Act
            var birthdate = socialSecurityNumber.Birthdate();
            // Assert
            birthdate.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", 30)]
        [InlineData("53422003890", 20)]
        [InlineData("00000100364", null)]
        [InlineData("40000095579", null)]
        public void BirthDay_WhenValidNumber_ReturnsExpectedValue(string number, int? expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var day = socialSecurityNumber.BirthDay();
            // Assert
            day.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", 7)]
        [InlineData("53422003890", 2)]
        [InlineData("00000100364", null)]
        [InlineData("40000095579", null)]
        public void BirthMonth_WhenValidNumber_ReturnsExpectedValue(string number, int? expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var month = socialSecurityNumber.BirthMonth();
            // Assert
            month.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", 1985)]
        [InlineData("17073003384", 2017)]
        [InlineData("53422003890", 1953)]
        [InlineData("06492803907", 2006)]
        [InlineData("40000095579", 1940)]
        [InlineData("00000100364", null)]
        public void BirthYear_WhenValidNumber_ReturnsExpectedValue(string number, int? expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var year = socialSecurityNumber.BirthYear();
            // Assert
            year.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", true)]
        [InlineData("17073003384", false)]
        [InlineData("53422003890", true)]
        [InlineData("06492803907", false)]
        public void BornBefore2000_WhenValidNumber_ReturnsExpectedValue(string number, bool expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var bornBeforeValue = socialSecurityNumber.BornBefore2000();
            // Assert
            bornBeforeValue.Should().Be(expectedValue);
        }
        [Theory]
        [InlineData("85073003328", 28)]
        [InlineData("17073003384", 84)]
        public void CheckDigit_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var checkDigit = socialSecurityNumber.CheckDigit();
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", 28, true)]
        [InlineData("17073003384", 84, false)]
        [InlineData("53422003890", 90, true)]
        [InlineData("06492803907", 07, false)]
        [InlineData("75010112470", 70, true)]
        [InlineData("750101124", 70, true)]
        [InlineData("04042400397", 97, false)]
        [InlineData("40000095579", 79, true)]
        public void ComputeCheckDigit_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue, bool bornBefore2000)
        {
            // Act
            var checkDigit = BelgianSocialSecurityNumber.ComputeCheckDigit(number, bornBefore2000);
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("53422003890", false)]
        [InlineData("00000100364", false)]
        [InlineData("40000100133", true)]
        public void HasPartialBirthdate_WhenValidNumber_ReturnsExpectedValue(string number, bool expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var hasPartialBirthdate = socialSecurityNumber.HasPartialBirthdate();
            // Assert
            hasPartialBirthdate.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("53422003890", false)]
        [InlineData("00000100364", true)]
        [InlineData("40000100133", false)]
        public void HasUnknownBirthdate_WhenValidNumber_ReturnsExpectedValue(string number, bool expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var hasUnknownBirthdate = socialSecurityNumber.HasUnknownBirthdate();
            // Assert
            hasUnknownBirthdate.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", false)]
        [InlineData("17073003384", false)]
        [InlineData("53422003890", true)]
        [InlineData("06492803907", true)]
        public void IsBisRegisterNumber_WhenValidNumber_ReturnsExpectedValue(string number, bool expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var isBisRegisterNumber = socialSecurityNumber.IsBisRegisterNumber();
            // Assert
            isBisRegisterNumber.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", true)]
        [InlineData("17073003384", true)]
        [InlineData("53422003890", false)]
        [InlineData("06492803907", false)]
        public void IsNationalRegisterNumber_WhenValidNumber_ReturnsExpectedValue(string number, bool expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var isNationalRegisterNumber = socialSecurityNumber.IsNationalRegisterNumber();
            // Assert
            isNationalRegisterNumber.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", 850730033)]
        [InlineData("17073003384", 170730033)]
        public void PersonUniqueIdentifier_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var uniqueIdentifier = socialSecurityNumber.PersonUniqueIdentifier();
            // Assert
            uniqueIdentifier.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("85073003328", 33)]
        [InlineData("53422003890", 38)]
        [InlineData("00000100364", 3)]
        [InlineData("40000100133", 1001)]
        public void SequenceNumber_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var sequenceNumber = socialSecurityNumber.SequenceNumber();
            // Assert
            sequenceNumber.Should().Be(expectedValue);
        }
        [Theory]
        [InlineData("75010112470", BelgianSocialSecurityNumber.Sex.Female)]
        [InlineData("85073003328", BelgianSocialSecurityNumber.Sex.Male)]
        public void SexAtBirth_WhenValidNumber_ReturnsExpectedValue(string number, BelgianSocialSecurityNumber.Sex expectedValue)
        {
            // Arrange
            var socialSecurityNumber = new BelgianSocialSecurityNumber(number);
            // Act
            var sexAtBirth = socialSecurityNumber.SexAtBirth();
            // Assert
            sexAtBirth.Should().Be(expectedValue);
        }

        #endregion Methods

    }
}
