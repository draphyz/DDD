using System.Data;
#if (NET6_0)
using System.Data.Common;
#endif
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Core.Application;
    using Core.Infrastructure.Serialization;
    using Mapping;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Cfg;

    public class OracleFixture : DbFixture<HealthcareDeliveryContext>, IPersistenceFixture
    {

#region Constructors

        public OracleFixture() : base("OracleScripts", ConfigurationManager.ConnectionStrings["Oracle"])
        {
        }

#endregion Constructors

#region Methods

        public IObjectTranslator<IEvent, Event> CreateEventTranslator()
        {
            return new EventTranslator(JsonSerializerWrapper.Create(false));
        }

        public DelegatingSessionFactory<HealthcareDeliveryContext> CreateSessionFactory(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            var configuration = new BelgianOracleHealthcareDeliveryConfiguration().DataBaseIntegration(db =>
            {
                db.Dialect<Oracle10gDialect>();
                db.Driver<OracleManagedDataClientDriver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.None;
            });
            return new DelegatingSessionFactory<HealthcareDeliveryContext>
            (
                configuration,
                options =>
                {
                    var connection = connectionProvider.GetOpenConnection();
                    options.Connection(connection);
                },
                async (options, cancellationToken) =>
                {
                    var connection = await connectionProvider.GetOpenConnectionAsync(cancellationToken);
                    options.Connection(connection);
                }
            );
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