using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Validation;
    using Application.Prescriptions;

    internal class BelgianPrescribedMedicationDescriptorValidator : AbstractValidator<PrescribedMedicationDescriptor>
    {

        #region Constructors

        public BelgianPrescribedMedicationDescriptorValidator()
        {
            RuleFor(m => m.NameOrDescription).NotEmpty().WithErrorCode("MedicationNameOrDescriptionEmpty");
            RuleFor(m => m.Code).Length(7).WithErrorCode("MedicationCodeInvalid")
                                .Numeric().WithErrorCode("MedicationCodeInvalid");
        }

        #endregion Constructors

    }
}
