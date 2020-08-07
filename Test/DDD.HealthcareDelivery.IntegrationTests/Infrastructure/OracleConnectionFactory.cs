namespace DDD.HealthcareDelivery.Infrastructure
{
    using Core.Infrastructure.Data;

    public class OracleConnectionFactory : DbConnectionFactory, IHealthcareDeliveryConnectionFactory
    {

        #region Fields

        /// <remarks>
        /// Pooling=false is used to ensure that the System.Transactions infrastructure doesn't automatically escalates the transaction to be managed by the Microsoft Distributed Transaction Coordinator (MSDTC).
        /// Do not use Pooling=false in production.
        /// </remarks>
        public const string ConnectionString 
            = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XE)));Persist Security Info=false;User Id=TEST;Password=dev;Pooling=false";

        #endregion Fields

        #region Constructors

        private OracleConnectionFactory(string providerName, string connectionString)
            : base(providerName, connectionString)
        {
        }

        #endregion Constructors

        #region Methods

        public static OracleConnectionFactory Create()
        {
            return new OracleConnectionFactory("Oracle.ManagedDataAccess.Client", ConnectionString);
        }

        #endregion Methods

    }
}
