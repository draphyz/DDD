using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class OracleFixture : DbFixture<OracleConnectionFactory>
    {

        #region Constructors

        public OracleFixture() : base(new OracleConnectionFactory(), "OracleScripts")
        {
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods

    }
}