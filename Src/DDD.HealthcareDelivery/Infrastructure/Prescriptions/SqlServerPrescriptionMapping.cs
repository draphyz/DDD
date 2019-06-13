namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Domain.Practitioners;
    using Domain.Facilities;

    internal class SqlServerPrescriptionMapping<TPractitionerLicenseNumber, TFacilityLicenseNumber, TSocialSecurityNumber, TSex>
        : PrescriptionMapping<TPractitionerLicenseNumber, TFacilityLicenseNumber, TSocialSecurityNumber, TSex>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
        where TFacilityLicenseNumber : HealthFacilityLicenseNumber
        where TSocialSecurityNumber : SocialSecurityNumber
        where TSex : Sex
    {
        public SqlServerPrescriptionMapping() : base(false)
        {
            // Fields
            this.Discriminator(m => m.Column(m1 => m1.SqlType("varchar(5)")));
        }
    }
}