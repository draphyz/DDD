using DDD.Common.Domain;

namespace DDD.HealthcareDelivery.Domain.Practitioners
{
    /// <summary>
    /// Represents a person who is authorised to practise medicine.
    /// </summary>
    public class Physician : HealthcarePractitioner
    {

        #region Constructors

        public Physician(int identifier, 
                         FullName fullName, 
                         HealthcarePractitionerLicenseNumber licenseNumber,
                         SocialSecurityNumber socialSecurityNumber = null,
                         ContactInformation contactInformation = null, 
                         string speciality = null,
                         string displayName = null) 
            : base(identifier, fullName, licenseNumber, socialSecurityNumber, contactInformation, speciality, displayName)
        {
        }

        #endregion Constructors

        #region Methods

        public override HealthcarePractitionerState ToState()
        {
            var state = base.ToState();
            state.PractitionerType = "Physician";
            return state;
        }

        #endregion Methods

    }
}
