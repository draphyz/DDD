using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Facilities
{
    using Mapping;

    internal class BelgianHealthFacilityTranslator : IObjectTranslator<HealthFacilityState, HealthFacility>
    {

        #region Methods

        public HealthFacility Translate(HealthFacilityState state, 
                                        IDictionary<string, object> options = null)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            switch (state.FacilityType.ToEnum<HealthFacilityType>())
            {
                case HealthFacilityType.MedicalOffice:
                    return CreateMedicalOffice(state);
                case HealthFacilityType.Hospital:
                    return CreateHospital(state);
                default:
                    throw new ArgumentException($"Facility type '{state.FacilityType}' not expected.", nameof(state));
            }
        }

        private static MedicalOffice CreateMedicalOffice(HealthFacilityState state)
        {
            return new MedicalOffice(state.Identifier, state.Name, BelgianHealthFacilityLicenseNumber.CreateIfNotEmpty(state.LicenseNumber));
        }

        private static Hospital CreateHospital(HealthFacilityState state)
        {
            return new Hospital
            (
                state.Identifier,
                state.Name,
                new BelgianHealthFacilityLicenseNumber(state.LicenseNumber)
            );
        }

        #endregion Methods

    }
}
