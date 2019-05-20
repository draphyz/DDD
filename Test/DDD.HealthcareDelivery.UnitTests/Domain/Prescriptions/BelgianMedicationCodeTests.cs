using FluentAssertions;
using Xunit;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    public class BelgianMedicationCodeTests
    {

        #region Methods

        [Theory]
        [InlineData("3260072", 2)]
        [InlineData("2480630", 0)]
        [InlineData("2944106", 6)]
        public void CheckDigit_WhenValidCode_ReturnsExpectedValue(string code, int expectedValue)
        {
            // Arrange
            var medicationCode = new BelgianMedicationCode(code);
            // Act
            var checkDigit = medicationCode.CheckDigit();
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("1013168", 8)]
        [InlineData("3260072", 2)]
        [InlineData("2480630", 0)]
        [InlineData("1544550", 0)]
        [InlineData("2944106", 6)]
        [InlineData("294410", 6)]
        public void ComputeCheckDigit_WhenValidCode_ReturnsExpectedValue(string code, int expectedValue)
        {
            // Act
            var checkDigit = BelgianMedicationCode.ComputeCheckDigit(code);
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("3260072", 326007)]
        [InlineData("2480630", 248063)]
        [InlineData("2944106", 294410)]
        public void MedicationUniqueIdentifier_WhenValidCode_ReturnsExpectedValue(string code, int expectedValue)
        {
            // Arrange
            var medicationCode = new BelgianMedicationCode(code);
            // Act
            var identifier = medicationCode.MedicationUniqueIdentifier();
            // Assert
            identifier.Should().Be(expectedValue);
        }

        #endregion Methods
    }
}
