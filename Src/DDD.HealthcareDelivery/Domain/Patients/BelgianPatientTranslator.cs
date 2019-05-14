using Conditions;
using System.Collections.Generic;

namespace DDD.HealthcareDelivery.Domain.Patients
{
    using Mapping;
    using Common.Domain;

    internal class BelgianPatientTranslator : IObjectTranslator<PatientState, Patient>
    {
        #region Methods

        public Patient Translate(PatientState state,
                                 IDictionary<string, object> options = null)
        {
            Condition.Requires(state, nameof(state)).IsNotNull();
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
