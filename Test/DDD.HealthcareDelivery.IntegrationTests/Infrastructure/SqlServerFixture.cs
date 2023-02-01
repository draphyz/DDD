using Microsoft.Data.SqlClient;
using System.Data;
#if (NET6_0)
using System.Data.Common;
#endif
using System.Configuration;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Domain;
    using Core.Infrastructure.Testing;
    using Core.Infrastructure.Data;
    using Core.Domain;
    using Core.Application;
    using Core.Infrastructure.Serialization;
    using Mapping;

    public class SqlServerFixture : DbFixture<HealthcareDeliveryContext>, IPersistenceFixture
    {

        #region Constructors

        public SqlServerFixture() : base("SqlServerScripts", ConfigurationManager.ConnectionStrings["SqlServer"])
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
            var configuration = new BelgianSqlServerHealthcareDeliveryConfiguration().DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<MicrosoftDataSqlClientDriver>();
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
                var builder = new SqlConnectionStringBuilder(connection.ConnectionString) { InitialCatalog = "master" };
                connection.ConnectionString = builder.ConnectionString;
                connection.Open();
                this.ExecuteScript(SqlServerScripts.CreateDatabase, connection);
            }
        }

        protected override int[] ExecuteScript(string script, IDbConnection connection)
        {
            return connection.ExecuteScript(script);
        }

        protected override void LoadConfiguration()
        {
#if (NET6_0)
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
#endif
        }

        protected override string SetPooling(string connectionString, bool pooling)
        {
            var builder = new SqlConnectionStringBuilder(connectionString) { Pooling = pooling };
            return builder.ConnectionString;
        }


        #endregion Methods

    }
}