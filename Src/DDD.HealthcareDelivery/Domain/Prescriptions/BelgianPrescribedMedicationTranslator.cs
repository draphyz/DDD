﻿using System;
using EnsureThat;

namespace DDD.HealthcareDelivery.Domain.Prescriptions
{
    using Mapping;

    internal class BelgianPrescribedMedicationTranslator : ObjectTranslator<PrescribedMedicationState, PrescribedMedication>
    {

        #region Methods

        public override PrescribedMedication Translate(PrescribedMedicationState state,
                                                       IMappingContext context)
        {
            Ensure.That(state, nameof(state)).IsNotNull();
            Ensure.That(context, nameof(context));
            switch (state.MedicationType)
            {
                case "Product":
                    return CreateProduct(state);
                case "Substance":
                    return CreateSubstance(state);
                case "Compounding":
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
                BelgianMedicationCode.CreateIfNotEmpty(state.Code),
                state.Identifier,
                state.EntityState
            );
        }

        #endregion Methods

    }
}
