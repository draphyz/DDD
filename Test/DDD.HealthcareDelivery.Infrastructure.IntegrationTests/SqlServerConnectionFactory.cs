namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class SqlServerConnectionFactory : DbConnectionFactory, IHealthcareConnectionFactory
    {
        public SqlServerConnectionFactory()
            : base("System.Data.SqlClient",
                   @"Data Source=(local)\SQLEXPRESS;Database=Test;Integrated Security=False;User ID=sa;Password=dev;Pooling=false")
        {
        }
    }
}
