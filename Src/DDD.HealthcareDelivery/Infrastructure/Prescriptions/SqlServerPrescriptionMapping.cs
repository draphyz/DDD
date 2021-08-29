namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;
    using Domain.Practitioners;

    internal class SqlServerPrescriptionMapping<TPractitionerLicenseNumber, TSocialSecurityNumber, TSex>
        : PrescriptionMapping<TPractitionerLicenseNumber, TSocialSecurityNumber, TSex>
        where TPractitionerLicenseNumber : HealthcarePractitionerLicenseNumber
        where TSocialSecurityNumber : SocialSecurityNumber
        where TSex : Sex
    {
        #region Constructors

        public SqlServerPrescriptionMapping() 
        {
            // Fields
            this.Discriminator(m => m.Column(m1 => m1.SqlType("varchar(5)")));
            this.Property(p => p.CreatedOn, m => m.Column(m1 => m1.SqlType("smalldatetime")));
        }

        #endregion Constructors
    }
}