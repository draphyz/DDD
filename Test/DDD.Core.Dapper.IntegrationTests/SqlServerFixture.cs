using Dapper;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace DDD.Core.Infrastructure.Data
{
    using Core.Application;
    using Core.Domain;
    using Core.Infrastructure.Serialization;
    using Mapping;
    using Testing;

    public class SqlServerFixture : DbFixture<TestContext>, IPersistenceFixture
    {

        #region Constructors

        public SqlServerFixture() : base("SqlServerScripts", ConfigurationManager.ConnectionStrings["SqlServer"])
        {
            SqlMapper.ResetTypeHandlers();
            SqlMapper.AddTypeHandler(new IncrementalDelaysTypeMapper());
        }

        #endregion Constructors

        #region Methods

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