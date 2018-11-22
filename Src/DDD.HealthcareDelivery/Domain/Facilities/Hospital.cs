namespace DDD.HealthcareDelivery.Domain.Facilities
{
    public class Hospital : HealthFacility
    {

        #region Constructors

        public Hospital(int identifier,
                        string name, 
                        HealthFacilityLicenseNumber licenseNumber = null, 
                        string code = null) 
            : base(identifier, name, licenseNumber, code)
        {
        }

        #endregion Constructors

        #region Methods

        public override HealthFacilityState ToState()
        {
            var state = base.ToState();
            state.FacilityType = HealthFacilityType.Hospital.ToString();
            return state;
        }

        #endregion Methods

    }
}
