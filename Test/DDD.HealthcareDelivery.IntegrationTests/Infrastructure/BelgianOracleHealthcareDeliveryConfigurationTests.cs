using NHibernate.Dialect;
using NHibernate.Driver;
using Xunit;

namespace DDD.HealthcareDelivery.Infrastructure
{
    [Collection("Oracle")]
    public class BelgianOracleHealthcareDeliveryConfigurationTests : HealthcareDeliveryConfigurationTests<OracleFixture>
    {
        #region Constructors

        public BelgianOracleHealthcareDeliveryConfigurationTests(OracleFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors

        #region Methods

        protected override HealthcareDeliveryConfiguration CreateConfiguration()
        {
            var configuration = new BelgianOracleHealthcareDeliveryConfiguration().DataBaseIntegration(db =>
            {
                db.Dialect<Oracle10gDialect>();
                db.Driver<OracleManagedDataClientDriver>();
                db.ConnectionStringName = "Oracle";
            });
            return (HealthcareDeliveryConfiguration)configuration;
        }

        #endregion Methods
    }
}