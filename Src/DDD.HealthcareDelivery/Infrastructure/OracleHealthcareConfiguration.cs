using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace DDD.HealthcareDelivery.Infrastructure
{
    public abstract class OracleHealthcareConfiguration : HealthcareConfiguration
    {
        #region Constructors

        protected OracleHealthcareConfiguration(string connectionString) : base(connectionString)
        {
            this.DataBaseIntegration(db =>
            {
                db.Dialect<Oracle10gDialect>();
                db.Driver<OracleManagedDataClientDriver>();
            });
        }

        #endregion Constructors
    }
}
