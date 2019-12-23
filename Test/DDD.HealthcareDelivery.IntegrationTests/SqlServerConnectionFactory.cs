using System.Configuration;

namespace DDD.HealthcareDelivery
{
    using Infrastructure;
    using Core.Infrastructure.Data;

    public class SqlServerConnectionFactory : DbConnectionFactory, IHealthcareConnectionFactory
    {

        #region Constructors

        private SqlServerConnectionFactory(string providerName, string connectionString) 
            : base(providerName, connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        public static SqlServerConnectionFactory Create()
        {
            var settings = ConfigurationManager.ConnectionStrings["SqlServer"];
            return new SqlServerConnectionFactory(settings.ProviderName, settings.ConnectionString);
        }

        #endregion Methods

    }
}
