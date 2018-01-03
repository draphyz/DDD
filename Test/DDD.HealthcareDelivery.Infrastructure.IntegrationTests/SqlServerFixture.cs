using System.Data.SqlClient;
using System.Data;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class SqlServerFixture : DbFixture<SqlServerConnectionFactory>
    {

        #region Constructors

        public SqlServerFixture() : base(new SqlServerConnectionFactory(), "SqlServerScripts")
        {
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods
    }
}