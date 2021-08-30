using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Validation;

    public class BelgianCreatePharmaceuticalPrescriptionValidator : AbstractValidator<CreatePharmaceuticalPrescription>
    {

        #region Constructors

        public BelgianCreatePharmaceuticalPrescriptionValidator()
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
            RuleFor(p => p.PrescriptionIdentifier).GreaterThan(0).WithErrorCode("PrescriptionIdentifierInvalid");
            RuleFor(p => p.Medications).NotEmpty().WithErrorCode("MedicationsEmpty")
                                       .MaximumCount(10).WithErrorCode("MedicationsCountInvalid");
            RuleForEach(p => p.Medications).NotNull().WithErrorCode("MedicationNull")
                                           .SetValidator(new BelgianPrescribedMedicationDescriptorValidator());
        }

        #endregion Constructors

    }
}