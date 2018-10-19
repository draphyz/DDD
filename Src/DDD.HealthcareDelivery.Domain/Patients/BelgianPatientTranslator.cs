using Conditions;

namespace DDD.HealthcareDelivery.Domain.Patients
{
    using Core.Mapping;
    using Common.Domain;

    internal class BelgianPatientTranslator : IObjectTranslator<PatientState, Patient>
    {
        #region Methods

        public Patient Translate(PatientState state)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
            return new Patient
            (
                state.Identifier,
                FullName.FromState(state.FullName),
                Enumeration.FromCode<BelgianSex>(state.Sex),
                BelgianSocialSecurityNumber.CreateIfNotEmpty(state.SocialSecurityNumber),
                ContactInformation.FromState(state.ContactInformation),
                state.Birthdate
            );
        }

        #endregion Methods
    }
}
