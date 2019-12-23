using System.Configuration;

namespace DDD.HealthcareDelivery
{
    using Infrastructure;
    using Core.Infrastructure.Data;

    public class OracleConnectionFactory : DbConnectionFactory, IHealthcareConnectionFactory
    {

        #region Constructors

        private OracleConnectionFactory(string providerName, string connectionString)
            : base(providerName, connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        public static OracleConnectionFactory Create()
        {
            var settings = ConfigurationManager.ConnectionStrings["Oracle"];
            return new OracleConnectionFactory(settings.ProviderName, settings.ConnectionString);
        }

        #endregion Methods

    }
}
