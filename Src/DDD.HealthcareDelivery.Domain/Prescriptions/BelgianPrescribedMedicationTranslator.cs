using System;
using Conditions;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Core;
    using Core.Mapping;

    internal class BelgianPrescribedMedicationTranslator : IObjectTranslator<PrescribedMedicationState, PrescribedMedication>
    {

        #region Methods

        public PrescribedMedication Translate(PrescribedMedicationState state)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            switch (state.MedicationType.ToEnum<PrescribedMedicationType>())
            {
                case PrescribedMedicationType.Product:
                    return CreateProduct(state);
                case PrescribedMedicationType.Substance:
                    return CreateSubstance(state);
                case PrescribedMedicationType.Compounding:
                    return CreateCompounding(state);
                default:
                    throw new ArgumentException($"Medication type '{state.MedicationType}' not expected.", nameof(state));

            }
        }

        private static PrescribedPharmaceuticalCompounding CreateCompounding(PrescribedMedicationState state)
        {
            return new PrescribedPharmaceuticalCompounding
            (
                state.NameOrDescription,
                state.Posology,
                state.Quantity,
                state.Duration,
                state.Identifier,
                state.EntityState
            );
        }

        private static PrescribedPharmaceuticalProduct CreateProduct(PrescribedMedicationState state)
        {
            return new PrescribedPharmaceuticalProduct
            (
                state.NameOrDescription,
                state.Posology,
                state.Quantity,
                state.Duration,
                BelgianMedicationCode.CreateIfNotEmpty(state.Code),
                state.Identifier,
                state.EntityState
            );
        }

        private static PrescribedPharmaceuticalSubstance CreateSubstance(PrescribedMedicationState state)
        {
            return new PrescribedPharmaceuticalSubstance
            (
                state.NameOrDescription,
                state.Posology,
                state.Quantity,
                state.Duration,
                BelgianMedicationCode.CreateIfNotEmpty(state.Code),
                state.Identifier,
                state.EntityState
            );
        }

        #endregion Methods

    }
}
