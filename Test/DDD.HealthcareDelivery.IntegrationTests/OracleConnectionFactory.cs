namespace DDD.HealthcareDelivery
{
    using Infrastructure;
    using Core.Infrastructure.Data;

    public class OracleConnectionFactory : DbConnectionFactory, IHealthcareConnectionFactory
    {
        public OracleConnectionFactory()
            : base("Oracle.ManagedDataAccess.Client",
                   @"Data Source=Local;Persist Security Info=true;User Id=TEST;Password=dev;Pooling=false")
        {
        }
    }
}
