namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class SqlServerConnectionFactory : DbConnectionFactory, IHealthcareDeliveryConnectionFactory
    {

        #region Fields

        /// <remarks>
        /// Pooling=false is used to ensure that the System.Transactions infrastructure doesn't automatically escalates the transaction to be managed by the Microsoft Distributed Transaction Coordinator (MSDTC).
        /// Do not use Pooling=false in production.
        /// Persist Security Info=True is used by Entity Framework to create a connection to the master database whith the specified password when EnsureDeleted or EnsureCreated is used.
        /// </remarks>
        public const string ConnectionString 
            = @"Data Source=(local)\SQLEXPRESS;Database=Test;Integrated Security=False;User ID=sa;Password=dev;Pooling=false;Persist Security Info=True";

        #endregion Fields

        #region Constructors

        private SqlServerConnectionFactory(string providerName, string connectionString)
                    : base(providerName, connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        public static SqlServerConnectionFactory Create()
        {
            return new SqlServerConnectionFactory("Microsoft.Data.SqlClient", ConnectionString);
        }

        #endregion Methods
    }
}
