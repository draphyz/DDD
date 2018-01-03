using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Validation;
    using Application.Prescriptions;

    internal class BelgianPrescribedMedicationDescriptorValidator : AbstractValidator<PrescribedMedicationDescriptor>
    {
        private readonly bool isElectronic;

        #region Constructors

        public BelgianPrescribedMedicationDescriptorValidator(bool isElectronic)
        {
            this.isElectronic = isElectronic;
            RuleFor(m => m.NameOrDescription).NotEmpty().WithErrorCode("MedicationNameOrDescriptionEmpty");
            RuleFor(m => m.Code).Length(7).WithErrorCode("MedicationCodeInvalid")
                                .Numeric().WithErrorCode("MedicationCodeInvalid");
            When(m => isElectronic, () =>
            {
                RuleFor(m => m).Must(HavePosologyOrDuration).WithErrorCode("MedicationPosologyAndDurationEmpty").WithMessage("A Posology or a duration must be specified.");
            });
        }

        #endregion Constructors

        private static bool HavePosologyOrDuration(PrescribedMedicationDescriptor medication)
        {
            return !string.IsNullOrWhiteSpace(medication.Posology)
                || !string.IsNullOrWhiteSpace(medication.Duration);
        }
    }
}
