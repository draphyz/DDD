using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Validation;

    public class SchedulePharmaceuticalPrescriptionsValidator 
        : AbstractValidator<SchedulePharmaceuticalPrescriptions>
    {

        #region Constructors

        public SchedulePharmaceuticalPrescriptionsValidator()
        {
            RuleFor(q => q.Renewals).SetCollectionValidator(new MedicationRenewalValidator())
                                    .NotEmpty().WithErrorCode("RenewalsEmpty")
                                    .MaximumCount(5).WithErrorCode("RenewalsCountInvalid");
            RuleForEach(q => q.Renewals).NotNull().WithErrorCode("RenewalNull");
        }

        #endregion Constructors

    }
}
