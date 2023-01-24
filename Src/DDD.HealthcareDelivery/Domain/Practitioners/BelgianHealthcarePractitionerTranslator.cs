using Conditions;
using System;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    using Mapping;
    using Common.Domain;

    internal class BelgianHealthcarePractitionerTranslator : ObjectTranslator<HealthcarePractitionerState, HealthcarePractitioner>
    {

        #region Methods

        public override HealthcarePractitioner Translate(HealthcarePractitionerState state,
                                                IDictionary<string, object> context = null)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            switch (state.PractitionerType)
            {
                case "Physician":
                    return CreatePhysician(state);
                default:
                    throw new ArgumentException($"Practitioner type '{state.PractitionerType}' not expected.", nameof(state));
            }
        }

        private static Physician CreatePhysician(HealthcarePractitionerState state)
        {
            return new Physician
            (
                state.Identifier,
                FullName.FromState(state.FullName),
                new BelgianHealthcarePractitionerLicenseNumber(state.LicenseNumber),
                BelgianSocialSecurityNumber.CreateIfNotEmpty(state.SocialSecurityNumber),
                ContactInformation.FromState(state.ContactInformation),
                state.Speciality,
                state.DisplayName
            );
        }

        #endregion Methods

    }
}
