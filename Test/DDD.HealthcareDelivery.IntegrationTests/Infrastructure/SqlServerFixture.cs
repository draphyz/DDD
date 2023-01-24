using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Data.SqlClient;

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

        public DbHealthcareDeliveryContext CreateDbContext(IDbConnectionProvider<HealthcareDeliveryContext> connectionProvider)
        {
            return new SqlServerHealthcareDeliveryContext(connectionProvider);
        }

        public IObjectTranslator<IEvent, Event> CreateEventTranslator()
        {
            return new EventTranslator(JsonSerializerWrapper.Create(false));
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