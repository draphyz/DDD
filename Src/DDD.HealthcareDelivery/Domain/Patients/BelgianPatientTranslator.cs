using EnsureThat;

namespace DDD.HealthcareDelivery.Domain.Patients
{
    using Mapping;
    using Common.Domain;

    internal class BelgianPatientTranslator : ObjectTranslator<PatientState, Patient>
    {
        #region Methods

        public override Patient Translate(PatientState state,
                                          IMappingContext context)
        {
            Ensure.That(state, nameof(state)).IsNotNull();
            Ensure.That(context, nameof(context));
            return new Patient
            (
                state.Identifier,
                FullName.FromState(state.FullName),
                Enumeration.ParseCode<BelgianSex>(state.Sex),
                BelgianSocialSecurityNumber.CreateIfNotEmpty(state.SocialSecurityNumber),
                ContactInformation.FromState(state.ContactInformation),
                state.Birthdate
            );
        }

        #endregion Methods
    }
}
