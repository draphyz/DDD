using System.Data;
using System.Text;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
#if (NETCOREAPP3_1 || NET5_0)
using System.Data.Common;
#endif

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Mapping;

    public class OracleFixture : DbFixture<OracleConnectionFactory>, IPersistenceFixture<OracleConnectionFactory>
    {

        #region Fields

        private readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());

        #endregion Fields

        #region Constructors

        public OracleFixture() : base(OracleConnectionFactory.Create(), "OracleScripts")
        {
        }

        #endregion Constructors

        #region Methods

        public HealthcareDeliveryContext CreateContext()
        {
            return new OracleHealthcareDeliveryContextWithLogging(OracleConnectionFactory.ConnectionString, this.loggerFactory);
        }

        public IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator()
        {
            return new StoredEventTranslator(JsonSerializerWrapper.Create(false));
        }

        protected override void CreateDatabase()
        {
            using (var connection = this.ConnectionFactory.CreateConnection())
            {
                var builder = new OracleConnectionStringBuilder(connection.ConnectionString) { UserID = "SYS", DBAPrivilege = "SYSDBA" };
                connection.ConnectionString = builder.ConnectionString;
                connection.Open();
                this.ExecuteScript(OracleScripts.CreateSchema, connection);
            }
            this.ExecuteScript(OracleScripts.FillSchema);
        }

        protected override int[] ExecuteScript(string script, IDbConnection connection)
        {
            return connection.ExecuteScript(script, batchSeparator: "/");
        }

        protected override void RegisterDbProviderFactory()
        {
#if (NETCOREAPP3_1 || NET5_0)
            DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance);
#endif
        }

        #endregion Methods

    }
}