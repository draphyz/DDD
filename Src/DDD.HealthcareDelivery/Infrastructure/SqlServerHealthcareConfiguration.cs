using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace DDD.HealthcareDelivery.Infrastructure
{
    public abstract class SqlServerHealthcareConfiguration : HealthcareConfiguration
    {
        #region Constructors

        protected SqlServerHealthcareConfiguration(string connectionString) : base(connectionString)
        {
            this.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<SqlClientDriver>();
            });
        }

        #endregion Constructors
    }
}
