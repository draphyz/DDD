namespace DDD.HealthcareDelivery
{
    using Core.Infrastructure.Data;
    using Infrastructure;

    public class SqlServerConnectionFactory : DbConnectionFactory, IHealthcareConnectionFactory
    {

        #region Fields

        public const string ConnectionString = @"Data Source=(local)\SQLEXPRESS;Database=Test;Integrated Security=False;User ID=sa;Password=dev;Pooling=false";

        #endregion Fields

        #region Constructors

        public SqlServerConnectionFactory()
            : base("System.Data.SqlClient", ConnectionString)
        {
        }

        #endregion Constructors

    }
}
