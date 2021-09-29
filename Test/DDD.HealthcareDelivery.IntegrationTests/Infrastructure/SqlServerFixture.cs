using Microsoft.Data.SqlClient;
using NHibernate;
using System.Data;
using System.Text;
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

    public class SqlServerFixture : DbFixture<SqlServerConnectionFactory>, IPersistenceFixture<SqlServerConnectionFactory>
    {

        #region Constructors

        public SqlServerFixture() : base(SqlServerConnectionFactory.Create(), "SqlServerScripts")
        {
        }

        #endregion Constructors

        #region Methods

        public IObjectTranslator<IEvent, StoredEvent> CreateEventTranslator()
        {
            return new StoredEventTranslator(JsonSerializerWrapper.Create(false));
        }

        public ISessionFactory CreateSessionFactory()
        {
            var configuration = new BelgianSqlServerHealthcareDeliveryConfiguration(SqlServerConnectionFactory.ConnectionString);
            return configuration.BuildSessionFactory();
        }

        protected override void CreateDatabase()
        {
            using (var connection = this.ConnectionFactory.CreateConnection())
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

        protected override void RegisterDbProviderFactory()
        {
#if (NETCOREAPP3_1 || NET5_0)
            DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
#endif
        }

        #endregion Methods

    }
}