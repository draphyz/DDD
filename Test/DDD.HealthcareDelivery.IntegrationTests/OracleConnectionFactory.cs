namespace DDD.HealthcareDelivery
{
    using Core.Infrastructure.Data;
    using Infrastructure;

    public class OracleConnectionFactory : DbConnectionFactory, IHealthcareConnectionFactory
    {

        #region Fields

        public const string ConnectionString = @"Data Source=Local;Persist Security Info=true;User Id=TEST;Password=dev;Pooling=false";

        #endregion Fields

        #region Constructors

        public OracleConnectionFactory()
                    : base("Oracle.ManagedDataAccess.Client", ConnectionString)
        {
        }

        #endregion Constructors

    }
}
