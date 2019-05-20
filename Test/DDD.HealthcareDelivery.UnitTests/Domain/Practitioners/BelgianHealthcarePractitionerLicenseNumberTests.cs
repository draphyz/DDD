using FluentAssertions;
using Xunit;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    using static BelgianHealthcarePractitionerLicenseNumber;

    public class BelgianHealthcarePractitionerLicenseNumberTests
    {

        #region Methods

        [Theory]
        [InlineData("16868201140", 1)]
        [InlineData("21035736001", 36)]
        [InlineData("38410515001", 15)]
        public void CheckDigit_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var practitionerNumber = new BelgianHealthcarePractitionerLicenseNumber(number);
            // Act
            var checkDigit = practitionerNumber.CheckDigit();
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("16868201140", 1)]
        [InlineData("21035736001", 36)]
        [InlineData("38410515001", 15)]
        [InlineData("41141757401", 57)]
        [InlineData("53048310521", 10)]
        [InlineData("67159434700", 34)]
        public void ComputeCheckDigit_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Act
            var checkDigit = BelgianHealthcarePractitionerLicenseNumber.ComputeCheckDigit(number);
            // Assert
            checkDigit.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("16868201140", 8682)]
        [InlineData("21035736001", 357)]
        [InlineData("38410515001", 4105)]
        public void PractitionerIdentifier_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var practitionerNumber = new BelgianHealthcarePractitionerLicenseNumber(number);
            // Act
            var practitionerIdentifier = practitionerNumber.PractitionerIdentifier();
            // Assert
            practitionerIdentifier.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("16868201140", 16868201)]
        [InlineData("21035736001", 21035736)]
        [InlineData("38410515001", 38410515)]
        public void PractitionerUniqueIdentifier_WhenValidNumber_ReturnsExpectedValue(string number, int expectedValue)
        {
            // Arrange
            var practitionerNumber = new BelgianHealthcarePractitionerLicenseNumber(number);
            // Act
            var practitionerUniqueIdentifier = practitionerNumber.PractitionerUniqueIdentifier();
            // Assert
            practitionerUniqueIdentifier.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("16868201140", "140")]
        [InlineData("21035736001", "001")]
        [InlineData("53048310521", "521")]
        public void QualificationCode_WhenValidNumber_ReturnsExpectedValue(string number, string expectedValue)
        {
            // Arrange
            var practitionerNumber = new BelgianHealthcarePractitionerLicenseNumber(number);
            // Act
            var qualificationCode = practitionerNumber.QualificationCode();
            // Assert
            qualificationCode.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("20415728002", BelgianProvince.FlemishBrabant)]
        [InlineData("21035736001", BelgianProvince.Antwerp)]
        [InlineData("22310394003", BelgianProvince.WalloonBrabant)]
        [InlineData("53048310521", BelgianProvince.WestFlanders)]
        [InlineData("16868201140", BelgianProvince.Liege)]
        [InlineData("67159434700", BelgianProvince.Limburg)]
        [InlineData("38410515001", BelgianProvince.Luxembourg)]
        public void Province_WhenValidNumber_ReturnsExpectedValue(string number, BelgianProvince expectedValue)
        {
            // Arrange
            var practitionerNumber = new BelgianHealthcarePractitionerLicenseNumber(number);
            // Act
            var province = practitionerNumber.Province();
            // Assert
            province.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("16868201140", HealthProfession.Physician)]
        [InlineData("21035736001", HealthProfession.Pharmacist)]
        [InlineData("38410515001", HealthProfession.Dentist)]
        [InlineData("41141757401", HealthProfession.NurseOrMidwife)]
        [InlineData("53048310521", HealthProfession.Physiotherapist)]
        [InlineData("67159434700", HealthProfession.Paramedic)]
        public void Profession_WhenValidNumber_ReturnsExpectedValue(string number, HealthProfession expectedValue)
        {
            // Arrange
            var practitionerNumber = new BelgianHealthcarePractitionerLicenseNumber(number);
            // Act
            var profession = practitionerNumber.Profession();
            // Assert
            profession.Should().Be(expectedValue);
        }

        #endregion Methods

    }
}
