namespace Xperthis.Core.Infrastructure.Data
{
    public class SqlServerConnectionFactory : DbConnectionFactory
    {
        public SqlServerConnectionFactory() 
            : base("System.Data.SqlClient",
                  @"Data Source=(local)\SQLEXPRESS;Database=Test;Integrated Security=False;User ID=sa;Password=mathib;Pooling=false")
        {
        }
    }
}
