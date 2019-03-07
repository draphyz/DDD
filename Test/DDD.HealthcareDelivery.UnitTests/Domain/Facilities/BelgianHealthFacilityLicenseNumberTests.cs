using FluentAssertions;
using Xunit;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    [Trait("Category", "Unit")]
    public class BelgianHealthFacilityLicenseNumberTests
    {

        #region Methods

        [Theory]
        [InlineData("78901184", 84)]
        [InlineData("73000418210", 18)]
        public void CheckDigit_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var facilityNumber = new BelgianHealthFacilityLicenseNumber(number);
            // Act
            var checkDigit = facilityNumber.CheckDigit();
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("73000418210", 18)]
        [InlineData("78901184", 84)]
        [InlineData("75500543", 43)]
        [InlineData("82662608", 8)]
        [InlineData("71000436", 36)]
        [InlineData("789011", 84)]
        public void ComputeCheckDigit_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Act
            var checkDigit = BelgianHealthFacilityLicenseNumber.ComputeCheckDigit(number);
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("78901184", 789011)]
        [InlineData("73000418210", 730004)]
        public void FacilityUniqueIdentifier_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var facilityNumber = new BelgianHealthFacilityLicenseNumber(number);
            // Act
            var facilityUniqueIdentifier = facilityNumber.FacilityUniqueIdentifier();
            // Assert
            facilityUniqueIdentifier.Should().Be(expectedValue);
        }

        #endregion Methods

    }
}
