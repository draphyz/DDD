using Conditions;
using System;

namespace DDD.HealthcareDelivery.Domain.Providers
{
    using Core;
    using Core.Mapping;
    using Common.Domain;

    internal class BelgianHealthcareProviderTranslator : IObjectTranslator<HealthcareProviderState, HealthcareProvider>
    {

        #region Methods

        public HealthcareProvider Translate(HealthcareProviderState state)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            switch (state.ProviderType.ToEnum<HealthcareProviderType>())
            {
                case HealthcareProviderType.Physician:
                    return CreatePhysician(state);
                default:
                    throw new ArgumentException($"Provider type '{state.ProviderType}' not expected.", nameof(state));
            }
        }

        private static Physician CreatePhysician(HealthcareProviderState state)
        {
            return new Physician
            (
                state.Identifier,
                FullName.FromState(state.FullName),
                new BelgianPractitionerLicenseNumber(state.LicenseNumber),
                string.IsNullOrWhiteSpace(state.SocialSecurityNumber) ? null : new BelgianSocialSecurityNumber(state.SocialSecurityNumber),
                ContactInformation.FromState(state.ContactInformation),
                state.Speciality,
                state.DisplayName
            );
        }

        #endregion Methods

    }
}
