using System.Data.SqlClient;

namespace Xperthis.Core.Infrastructure.Data
{
     public class FakeSqlServerFixture : SqlServerFixture<SqlServerConnectionFactory>
    {
        #region Constructors

        public FakeSqlServerFixture() : base(new SqlServerConnectionFactory())
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
                this.ExecuteScript(SqlServerScripts.CreateDatabase, connection);
            }
        }

        #endregion Methods
    }
}