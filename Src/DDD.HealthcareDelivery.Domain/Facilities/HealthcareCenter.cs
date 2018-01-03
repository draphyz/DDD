namespace DDD.HealthcareDelivery.Domain.Facilities
{
    public class HealthcareCenter : HealthFacility
    {

        #region Constructors

        public HealthcareCenter(int identifier, 
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
            state.FacilityType = HealthFacilityType.Center.ToString();
            return state;
        }

        #endregion Methods

    }
}
