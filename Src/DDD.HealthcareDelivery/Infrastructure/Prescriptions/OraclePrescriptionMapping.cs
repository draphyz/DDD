namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Domain.Practitioners;

    internal class OraclePrescriptionMapping<TPractitionerLicenseNumber, TSocialSecurityNumber, TSex>
        : PrescriptionMapping<TPractitionerLicenseNumber, TSocialSecurityNumber, TSex>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
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
