namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;

    internal class SqlServerPrescriptionMapping<TSocialSecurityNumber, TSex> 
        : PrescriptionMapping<TSocialSecurityNumber, TSex>
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
