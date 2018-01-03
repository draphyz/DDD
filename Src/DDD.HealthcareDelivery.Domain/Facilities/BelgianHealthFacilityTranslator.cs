using Conditions;
using System;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Core;
    using Core.Mapping;

    internal class BelgianHealthFacilityTranslator : IObjectTranslator<HealthFacilityState, HealthFacility>
    {

        #region Methods

        public HealthFacility Translate(HealthFacilityState state)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            switch (state.FacilityType.ToEnum<HealthFacilityType>())
            {
                case HealthFacilityType.Center:
                    return CreateCenter(state);
                case HealthFacilityType.Hospital:
                    return CreateHospital(state);
                default:
                    throw new ArgumentException($"Facility type '{state.FacilityType}' not expected.", nameof(state));
            }
        }

        private static HealthcareCenter CreateCenter(HealthFacilityState state)
        {
            return new HealthcareCenter(state.Identifier, state.Name);
        }

        private static Hospital CreateHospital(HealthFacilityState state)
        {
            return new Hospital
            (
                state.Identifier,
                state.Name,
                new BelgianHealthFacilityLicenseNumber(state.LicenseNumber),
                state.Code
            );
        }

        #endregion Methods

    }
}
