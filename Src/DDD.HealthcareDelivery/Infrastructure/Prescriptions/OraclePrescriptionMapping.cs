namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Domain.Practitioners;
    using Domain.Facilities;

    internal class OraclePrescriptionMapping<TPractitionerLicenseNumber, TFacilityLicenseNumber, TSocialSecurityNumber, TSex>
        : PrescriptionMapping<TPractitionerLicenseNumber, TFacilityLicenseNumber, TSocialSecurityNumber, TSex>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
        where TFacilityLicenseNumber : HealthFacilityLicenseNumber
        where TSocialSecurityNumber : SocialSecurityNumber
        where TSex : Sex
    {
        #region Constructors

        public OraclePrescriptionMapping()
        {
            // Fields
            this.Discriminator(m => m.Column(m1 => m1.SqlType("varchar2(5)")));
        }

        #endregion Constructors
    }
}
