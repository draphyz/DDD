using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Core.Infrastructure.Validation;
    using Application.Prescriptions;

    internal class MedicationRenewalValidator : AbstractValidator<MedicationRenewal>
    {
        public MedicationRenewalValidator()
        {
            RuleFor(r => r.NameOrDescription).NotEmpty().WithErrorCode("RenewalNameOrDescriptionEmpty");
            RuleFor(r => (int)r.Number).LessThanOrEqualTo(12).WithErrorCode("RenewalNumberInvalid");
            RuleFor(r => r.Code).Length(7).WithErrorCode("RenewalCodeInvalid")
                                .Numeric().WithErrorCode("RenewalCodeInvalid");
            When(r => r.FrequencyValue.HasValue, () =>
            {
                RuleFor(r => r.FrequencyUnit).NotNull().WithErrorCode("RenewalDurationValueAndUnitIncompatible");
                RuleFor(r => (int)r.Number).GreaterThan(1).WithErrorCode("RenewalNumberAndDurationIncomptabile");
            });
            When(r => !r.FrequencyValue.HasValue, () =>
            {
                RuleFor(r => r.FrequencyUnit).Null().WithErrorCode("RenewalDurationValueAndUnitIncompatible");
                RuleFor(r => (int)r.Number).Equal(1).WithErrorCode("RenewalNumberAndDurationIncomptabile");
            });
            RuleSet("electronic", () =>
            {
                RuleFor(r => r).Must(HavePosologyOrDuration).WithErrorCode("RenewalPosologyAndDurationEmpty").WithMessage("A Posology or a duration must be specified.");
            });
        }

        private static bool HavePosologyOrDuration(MedicationRenewal renewal)
        {
            return !string.IsNullOrWhiteSpace(renewal.Posology)
                || !string.IsNullOrWhiteSpace(renewal.Duration);
        }
    }
}
