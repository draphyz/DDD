using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Domain.Facilities;
    using Application.Prescriptions;
    using Core.Infrastructure.Validation;

    public class BelgianCreatePharmaceuticalPrescriptionsValidator : AbstractValidator<CreatePharmaceuticalPrescriptions>
    {

        #region Constructors

        public BelgianCreatePharmaceuticalPrescriptionsValidator()
        {
            RuleFor(c => c.LanguageCode).NotEmpty().WithErrorCode("LanguageCodeEmpty")
                                        .Length(2).WithErrorCode("LanguageCodeInvalid")
                                        .Alphabetic().WithErrorCode("LanguageCodeInvalid");
            RuleFor(c => c.PrescriberIdentifier).GreaterThan(0).WithErrorCode("PrescriberIdentifierInvalid");
            RuleFor(c => c.PrescriberFirstName).NotEmpty().WithErrorCode("PrescriberFirstNameEmpty");
            RuleFor(c => c.PrescriberLastName).NotEmpty().WithErrorCode("PrescriberLastNameEmpty");
            RuleFor(c => c.PrescriberLicenseNumber).NotEmpty().WithErrorCode("PrescriberLicenseNumberEmpty")
                                                   .Length(11).WithErrorCode("PrescriberLicenseNumberInvalid")
                                                   .Numeric().WithErrorCode("PrescriberLicenseNumberInvalid");
            RuleFor(c => c.PrescriberSocialSecurityNumber).Length(11).WithErrorCode("PrescriberSocialSecurityNumberInvalid")
                                                          .Numeric().WithErrorCode("PrescriberSocialSecurityNumberInvalid");
            RuleFor(c => c.PatientIdentifier).GreaterThan(0).WithErrorCode("PatientIdentifierInvalid");
            RuleFor(c => c.PatientFirstName).NotEmpty().WithErrorCode("PatientFirstNameEmpty");
            RuleFor(c => c.PatientLastName).NotEmpty().WithErrorCode("PatientLastNameEmpty");

            RuleFor(c => c.PatientSocialSecurityNumber).Length(11).WithErrorCode("PatientSocialSecurityNumberInvalid")
                                                       .Numeric().WithErrorCode("PatientSocialSecurityNumberInvalid");

            RuleFor(c => c.HealthFacilityIdentifier).GreaterThan(0).WithErrorCode("HealthFacilityIdentifierInvalid");
            RuleFor(c => c.HealthFacilityName).NotEmpty().WithErrorCode("HealthFacilityNameEmpty");
            RuleFor(c => c.HealthFacilityLicenseNumber).Length(8).WithErrorCode("HealthFacilityLicenseNumberInvalid")
                                                       .Numeric().WithErrorCode("HealthFacilityLicenseNumberInvalid");
            RuleFor(c => c.Prescriptions).SetCollectionValidator(c => new BelgianPharmaceuticalPrescriptionDescriptorValidator(c.IsElectronic))
                                         .NotEmpty().WithErrorCode("PrescriptionsEmpty");
            RuleForEach(c => c.Prescriptions).NotNull().WithErrorCode("PrescriptionNull");
            When(c => c.IsElectronic, () =>
            {
                RuleFor(c => c).Must(HaveTelephoneNumberForPrescriber).WithErrorCode("PrescriberTelephoneNumbersEmpty").WithMessage("At least one telephone number must be specified for the prescriber.");
                RuleFor(c => c.PrescriberStreet).NotEmpty().WithErrorCode("PrescriberStreetEmpty");
                RuleFor(c => c.PrescriberCity).NotEmpty().WithErrorCode("PrescriberCityEmpty");
                RuleFor(c => c.PatientSocialSecurityNumber).NotEmpty().WithErrorCode("PatientSocialSecurityNumberEmpty");
                RuleFor(c => c.PatientBirthdate).NotNull().WithErrorCode("PatientBirthdateEmpty");
                When(c => c.HealthFacilityType == HealthFacilityType.Center, () =>
                {
                    RuleFor(c => c.PrescriberSocialSecurityNumber).NotEmpty().WithErrorCode("PrescriberSocialSecurityNumberEmpty");
                });
                When(c => c.HealthFacilityType == HealthFacilityType.Hospital, () =>
                {
                    RuleFor(c => c.HealthFacilityLicenseNumber).NotEmpty().WithErrorCode("HealthFacilityLicenseNumberEmpty");
                    RuleFor(c => c.HealthFacilityCode).NotEmpty().WithErrorCode("HealthFacilityCodeEmpty");
                });
            });
        }

        #endregion Constructors

        #region Methods

        private static bool HaveTelephoneNumberForPrescriber(CreatePharmaceuticalPrescriptions command)
        {
            return !string.IsNullOrWhiteSpace(command.PrescriberPrimaryTelephoneNumber) 
                || !string.IsNullOrWhiteSpace(command.PrescriberSecondaryTelephoneNumber);
        }

        #endregion Methods

    }
}
