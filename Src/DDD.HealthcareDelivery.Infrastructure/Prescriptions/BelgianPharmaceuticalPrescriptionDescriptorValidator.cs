using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Validation;

    internal class BelgianPharmaceuticalPrescriptionDescriptorValidator : AbstractValidator<PharmaceuticalPrescriptionDescriptor>
    {
        private readonly bool isElectronic;

        #region Constructors

        public BelgianPharmaceuticalPrescriptionDescriptorValidator(bool isElectronic)
        {
            this.isElectronic = isElectronic;
            RuleFor(p => p.PrescriptionIdentifier).GreaterThan(0).WithErrorCode("PrescriptionIdentifierInvalid");
            RuleFor(p => p.Medications).SetCollectionValidator(new BelgianPrescribedMedicationDescriptorValidator(isElectronic))
                                       .NotEmpty().WithErrorCode("MedicationsEmpty")
                                       .MaximumCount(10).WithErrorCode("MedicationsCountInvalid");
            RuleForEach(p => p.Medications).NotNull().WithErrorCode("MedicationNull");
        }

        #endregion Constructors
    }
}
