using EnsureThat;
using System;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    using Mapping;
    using Common.Domain;

    internal class BelgianHealthcarePractitionerTranslator : ObjectTranslator<HealthcarePractitionerState, HealthcarePractitioner>
    {

        #region Methods

        public override HealthcarePractitioner Translate(HealthcarePractitionerState state,
                                                         IMappingContext context)
        {
            Ensure.That(state, nameof(state)).IsNotNull();
            Ensure.That(context, nameof(context));
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
