using Xunit;
using FluentValidation;
using FluentAssertions;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;

    [Trait("Category", "Unit")]
    public class SchedulePharmaceuticalPrescriptionsValidatorTests
    {

        #region Methods

        public static IEnumerable<object[]> QueriesWithRenewalsEmpty()
        {
            yield return new object[]
            {
                QueryWithRenewals(null)
            };
            yield return new object[]
            {
                QueryWithRenewals(new MedicationRenewal[] { })
            };
        }

        [Theory]
        [MemberData(nameof(QueriesWithRenewalsEmpty))]
        public void Validate_WhenRenewalsEmpty_ReturnsExpectedFailure(SchedulePharmaceuticalPrescriptions query)
        {
            // Arrange
            var validator = CreateValidator();
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalsEmpty" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void Validate_WhenRenewalsNotEmpty_ReturnsNoSpecificFailure(int renewalCount)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewalCount(renewalCount);
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalsEmpty");
        }

        [Theory]
        [InlineData(6)]
        [InlineData(8)]
        public void Validate_WhenRenewalsCountInvalid_ReturnsExpectedFailure(int renewalCount)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewalCount(renewalCount);
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalsCountInvalid" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public void Validate_WhenRenewalsCountValid_ReturnsNoSpecificFailure(int renewalCount)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewalCount(renewalCount);
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalsCountInvalid");
        }

        [Fact]
        public void Validate_WhenRenewalNull_ReturnsExpectedFailure()
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals((MedicationRenewal)null);
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalNull" && f.Severity == Severity.Error);
        }

        [Fact]
        public void Validate_WhenRenewalNotNull_ReturnsNoSpecificFailure()
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewalCount(2);
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalNull");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WhenRenewalNameOrDescriptionEmpty_ReturnsExpectedFailure(string nameOrDescription)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { NameOrDescription = nameOrDescription }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalNameOrDescriptionEmpty" && f.Severity == Severity.Error);
        }

        [Fact]
        public void Validate_WhenRenewalNameOrDescriptionNotEmpty_ReturnsNoSpecificFailure()
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { NameOrDescription = "Neovis Sol. 10 ml" }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalNameOrDescriptionEmpty");
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("   ", " ")]
        [InlineData(null, "")]
        [InlineData(" ", null)]
        public void Validate_WhenElectronicRenewalPosologyAndDurationEmpty_ReturnsExpectedFailure(string posology, string duration)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Posology = posology, Duration = duration }
            );
            // Act
            var results = validator.Validate(query, ruleSet: "default,electronic");
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalPosologyAndDurationEmpty" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData("1 goutte 1 x/jour", null)]
        [InlineData("1 goutte 1 x/jour", "")]
        [InlineData("1 goutte 1 x/jour", "  ")]
        [InlineData(null, "1 mois")]
        [InlineData("", "1 mois")]
        [InlineData("  ", "1 mois")]
        public void Validate_WhenElectronicRenewalPosologyOrDurationNotEmpty_ReturnsNoSpecificFailure(string posology, string duration)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Posology = posology, Duration = duration }
            );
            // Act
            var results = validator.Validate(query, ruleSet: "default,electronic");
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalPosologyAndDurationEmpty");
        }

        [Theory]
        [InlineData("aa")]
        [InlineData("11111")]
        [InlineData("111111a")]
        public void Validate_WhenRenewalCodeInvalid_ReturnsExpectedFailure(string renewalCode)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Code = renewalCode }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().Contain(f => f.ErrorCode == "RenewalCodeInvalid" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData("0000000")]
        [InlineData("0123456")]
        public void Validate_WhenRenewalCodeValid_ReturnsNoSpecificFailure(string renewalCode)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Code = renewalCode }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalCodeInvalid");
        }

        [Theory]
        [InlineData(13)]
        [InlineData(18)]
        public void Validate_WhenRenewalNumberInvalid_ReturnsExpectedFailure(byte number)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Number = number }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalNumberInvalid" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData(11)]
        [InlineData(12)]
        public void Validate_WhenRenewalNumberValid_ReturnsNoSpecificFailure(byte number)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Number = number }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalNumberInvalid");
        }

        [Theory]
        [InlineData((byte)1, null)]
        [InlineData(null, DurationUnit.Month)]
        public void Validate_WhenRenewalFrequencyValueAndUnitIncompatible_ReturnsExpectedFailure(byte? durationValue, DurationUnit? durationUnit)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { FrequencyValue = durationValue, FrequencyUnit = durationUnit }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalDurationValueAndUnitIncompatible" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData((byte)1, DurationUnit.Month)]
        [InlineData((byte)3, DurationUnit.Month)]
        [InlineData((byte)1, DurationUnit.Week)]
        [InlineData((byte)4, DurationUnit.Week)]
        public void Validate_WhenRenewalFrequencyValueAndUnitCompatible_ReturnsNoSpecificFailure(byte? durationValue, DurationUnit? durationUnit)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { FrequencyValue = durationValue, FrequencyUnit = durationUnit }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalDurationValueAndUnitIncompatible");
        }

        [Theory]
        [InlineData(1, (byte)2)]
        [InlineData(1, (byte)5)]
        [InlineData(2, null)]
        [InlineData(5, null)]
        public void Validate_WhenRenewalNumberAndFrequencyIncomptabile_ReturnsExpectedFailure(byte number, byte? durationValue)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Number = number, FrequencyValue = durationValue }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().ContainSingle(f => f.ErrorCode == "RenewalNumberAndDurationIncomptabile" && f.Severity == Severity.Error);
        }

        [Theory]
        [InlineData(1, null)]
        [InlineData(2, (byte)1)]
        [InlineData(5, (byte)3)]
        public void Validate_WhenRenewalNumberAndFrequencyComptabile_ReturnsNoSpecificFailure(byte number, byte? durationValue)
        {
            // Arrange
            var validator = CreateValidator();
            var query = QueryWithRenewals
            (
                new MedicationRenewal { Number = number, FrequencyValue = durationValue }
            );
            // Act
            var results = validator.Validate(query);
            // Assert
            results.Errors.Should().NotContain(f => f.ErrorCode == "RenewalNumberAndDurationIncomptabile");
        }

        private static SchedulePharmaceuticalPrescriptionsValidator CreateValidator()
        {
            return new SchedulePharmaceuticalPrescriptionsValidator();
        }

        private static SchedulePharmaceuticalPrescriptions QueryWithRenewals(params MedicationRenewal[] renewals)
        {
            return new SchedulePharmaceuticalPrescriptions
            {
                Renewals = renewals
            };
        }

        private static SchedulePharmaceuticalPrescriptions QueryWithRenewalCount(int renewalCount)
        {
            var query = new SchedulePharmaceuticalPrescriptions();
            for (var i = 0; i < renewalCount; i++)
                query.Renewals.Add(new MedicationRenewal());
            return query;
        }

        #endregion Methods
    }
}
