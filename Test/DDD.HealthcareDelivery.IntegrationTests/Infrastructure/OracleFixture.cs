using System.Data;
using System.Data.Common;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Microsoft.EntityFrameworkCore;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Mapping;
    using Core.Application;
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Core.Infrastructure.Serialization;

    public class OracleFixture : DbFixture<HealthcareDeliveryContext>, IPersistenceFixture
    {

        #region Constructors

        public OracleFixture() : base("OracleScripts", ConfigurationManager.ConnectionStrings["Oracle"])
        {
        }

        #endregion Constructors

        #region Methods

        public IDbContextFactory<DbHealthcareDeliveryContext> CreateDbContextFactory(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            return new DelegatingDbContextFactory<DbHealthcareDeliveryContext>
            (
                optionsBuilder =>
                {
                    var connection = connectionProvider.GetOpenConnection();
                    optionsBuilder.UseOracle(connection, o => o.UseOracleSQLCompatibility("11"));
                    return new OracleHealthcareDeliveryContext(optionsBuilder.Options);
                },
                async (optionsBuilder, cancellationToken) =>
                {
                    var connection = await connectionProvider.GetOpenConnectionAsync(cancellationToken);
                    optionsBuilder.UseOracle(connection, o => o.UseOracleSQLCompatibility("11"));
                    return new OracleHealthcareDeliveryContext(optionsBuilder.Options);
                }
            );
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