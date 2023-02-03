using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;
#if (NET6_0)
using System.Data.Common;
#endif
using System.Configuration;

namespace DDD.Core.Infrastructure.Data
{
    using Application;
    using Domain;
    using Serialization;
    using Mapping;
    using Testing;

    public class OracleFixture : DbFixture<TestContext>, IPersistenceFixture
    {

        #region Constructors

        public OracleFixture() : base("OracleScripts", ConfigurationManager.ConnectionStrings["Oracle"])
        {
            SqlMapper.ResetTypeHandlers();
            SqlMapper.AddTypeHandler(new OracleGuidTypeHandler());
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
            using (var connection = CreateConnection())
            {
                var builder = new OracleConnectionStringBuilder(connection.ConnectionString) { UserID = "SYS", DBAPrivilege = "SYSDBA" };
                connection.ConnectionString = builder.ConnectionString;
                connection.Open();
                ExecuteScript(OracleScripts.CreateSchema, connection);
            }
            ExecuteScript(OracleScripts.FillSchema);
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