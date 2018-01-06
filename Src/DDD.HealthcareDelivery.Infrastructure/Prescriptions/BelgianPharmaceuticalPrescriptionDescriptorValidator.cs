using FluentValidation;

namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Application.Prescriptions;
    using Core.Infrastructure.Validation;

    internal class BelgianPharmaceuticalPrescriptionDescriptorValidator : AbstractValidator<PharmaceuticalPrescriptionDescriptor>
    {

        #region Constructors

        public BelgianPharmaceuticalPrescriptionDescriptorValidator()
        {
            RuleFor(p => p.PrescriptionIdentifier).GreaterThan(0).WithErrorCode("PrescriptionIdentifierInvalid");
            RuleFor(p => p.Medications).SetCollectionValidator(new BelgianPrescribedMedicationDescriptorValidator())
                                       .NotEmpty().WithErrorCode("MedicationsEmpty")
                                       .MaximumCount(10).WithErrorCode("MedicationsCountInvalid");
            RuleForEach(p => p.Medications).NotNull().WithErrorCode("MedicationNull");
        }

        #endregion Constructors
    }
}
