using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Application;
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Mapping;
    using DDD.HealthcareDelivery;
    using System.Data.Common;

    public class OracleFixture : DbFixture<HealthcareDeliveryContext>, IPersistenceFixture
    {

        #region Constructors

        public OracleFixture() : base("OracleScripts", ConfigurationManager.ConnectionStrings["Oracle"])
        {
        }

        #endregion Constructors

        #region Methods

        public DbHealthcareDeliveryContext CreateDbContext(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            return new OracleHealthcareDeliveryContext(connectionProvider);
        }

        public IObjectTranslator<IEvent, Event> CreateEventTranslator()
        {
            return new EventTranslator(JsonSerializerWrapper.Create(false));
        }

        protected override void CreateDatabase()
        {
            using (var connection = this.CreateConnection())
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

        protected override void LoadConfiguration()
        {
#if (NET6_0)
            DbProviderFactories.RegisterFactory("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance);
#endif
        }

        protected override string SetPooling(string connectionString, bool pooling)
        {
            var builder = new OracleConnectionStringBuilder(connectionString) { Pooling = pooling };
            return builder.ConnectionString;
        }

        #endregion Methods

    }
}