namespace DDD.HealthcareDelivery.Infrastructure.Prescriptions
{
    using Common.Domain;

    internal class OraclePrescriptionMapping<TSocialSecurityNumber, TSex>
        : PrescriptionMapping<TSocialSecurityNumber, TSex>
        where TSocialSecurityNumber : SocialSecurityNumber
        where TSex : Sex
    {
        #region Constructors

        public OraclePrescriptionMapping(bool useUpperCase) : base(useUpperCase)
        {
            // Fields
            this.Discriminator(m => m.Column(m1 => m1.SqlType("varchar2(5)")));
        }

        #endregion Constructors
    }
}
